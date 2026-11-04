"""
import json
import os
import sys
from collections import defaultdict
from gurobipy import Model, GRB, quicksum


def load_json(path: str):
    with open(path, "r", encoding="utf-8") as f:
        return json.load(f)


def save_json(path: str, data):
    with open(path, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)


def normalize_text(s: str) -> str:
    return (s or "").strip().lower()


def split_keywords(text: str):
    if not text:
        return []
    return [x.strip().lower() for x in text.split(",") if x.strip()]


def build_course_score(course: dict, preferences: dict, no_need_setting: bool) -> int:

    if no_need_setting:
        return 0

    score = 0

    preferred_days = set(preferences.get("preferred_class_days", []))
    preferred_day_off = set(preferences.get("preferred_day_off", []))
    preferred_time_slots = set(preferences.get("preferred_time_slots", []))
    avoid_time_slots = set(preferences.get("avoid_time_slots", []))

    preferred_course_name = normalize_text(preferences.get("preferred_course_name", ""))
    preferred_professor_name = normalize_text(preferences.get("preferred_professor_name", ""))
    preferred_language = normalize_text(preferences.get("preferred_language", ""))

    course_name_en = normalize_text(course.get("english_course_name", ""))
    course_name_zh = normalize_text(course.get("chinese_course_name", ""))
    instructor = normalize_text(course.get("instructor", ""))
    teaching_language = normalize_text(course.get("teaching_language", ""))

    # day / time preferences
    for ts in course.get("time_slots", []):
        day = ts.get("day", "")
        slot = f"{ts.get('start', '')}-{ts.get('end', '')}"

        if day in preferred_days:
            score += 10

        if day in preferred_day_off:
            score -= 12

        if slot in preferred_time_slots:
            score += 8

        if slot in avoid_time_slots:
            score -= 10

    # course name preference
    if preferred_course_name:
        keywords = split_keywords(preferred_course_name)
        for kw in keywords:
            if kw in course_name_en or kw in course_name_zh:
                score += 6
                break

    # professor preference
    if preferred_professor_name:
        keywords = split_keywords(preferred_professor_name)
        for kw in keywords:
            if kw in instructor:
                score += 5
                break

    # language preference
    if preferred_language and preferred_language not in ["no specific", "both"]:
        if preferred_language == teaching_language:
            score += 4

    return score


def solve_schedule(input_path: str, output_path: str):
    data = load_json(input_path)

    min_credits = int(data.get("min_credits", 0) or 0)
    max_credits = int(data.get("max_credits", 0) or 0)
    no_need_setting = bool(data.get("no_need_setting", False))

    candidate_courses = data.get("candidate_courses", [])
    preferences = data.get("preferences", {})

    if not candidate_courses:
        result = {
            "status": "NO_CANDIDATE_COURSES",
            "message": "No candidate courses found in input JSON.",
            "selected_courses": [],
            "total_credits": 0
        }
        save_json(output_path, result)
        return

    m = Model("ThesisScheduler")

    # -----------------------------
    # Decision variables
    # x[i] = 1 if choose course row i
    # -----------------------------
    x = {}
    for i, course in enumerate(candidate_courses):
        course_id = course.get("course_id", f"course_{i}")
        class_code = course.get("class_code", "")
        x[i] = m.addVar(vtype=GRB.BINARY, name=f"x_{course_id}_{class_code}_{i}")

    m.update()

    # -----------------------------
    # Constraint 1: total credits
    # -----------------------------
    total_credits_expr = quicksum(
        int(course.get("credits", 0) or 0) * x[i]
        for i, course in enumerate(candidate_courses)
    )

    if min_credits > 0:
        m.addConstr(total_credits_expr >= min_credits, name="min_credits")

    if max_credits > 0:
        m.addConstr(total_credits_expr <= max_credits, name="max_credits")

    # -----------------------------
    # Constraint 2: at most one section per course_id
    # -----------------------------
    course_id_to_indices = defaultdict(list)
    for i, course in enumerate(candidate_courses):
        cid = course.get("course_id", "").strip()
        if cid:
            course_id_to_indices[cid].append(i)

    for cid, indices in course_id_to_indices.items():
        if len(indices) > 1:
            m.addConstr(quicksum(x[i] for i in indices) <= 1, name=f"one_section_{cid}")

    # -----------------------------
    # Constraint 3: no time conflict
    # Only for courses that have time_slots
    # -----------------------------
    slot_to_indices = defaultdict(list)

    for i, course in enumerate(candidate_courses):
        for ts in course.get("time_slots", []):
            slot_key = ts.get("slot_key", "").strip()
            if slot_key:
                slot_to_indices[slot_key].append(i)

    for slot_key, indices in slot_to_indices.items():
        if len(indices) > 1:
            m.addConstr(quicksum(x[i] for i in indices) <= 1, name=f"slot_{slot_key.replace('|', '_').replace(':', '').replace('-', '_')}")

    # -----------------------------
    # Objective
    # -----------------------------
    preference_scores = {}
    for i, course in enumerate(candidate_courses):
        preference_scores[i] = build_course_score(course, preferences, no_need_setting)

    # If no_need_setting is true:
    # maximize credits first, then number of selected courses
    if no_need_setting:
        objective = (
            quicksum(int(course.get("credits", 0) or 0) * 100 * x[i]
                     for i, course in enumerate(candidate_courses))
            + quicksum(x[i] for i in x)
        )
    else:
        # preference score + some reward for credits and course count
        objective = (
            quicksum(preference_scores[i] * x[i] for i in x)
            + quicksum(int(candidate_courses[i].get("credits", 0) or 0) * 2 * x[i] for i in x)
            + quicksum(x[i] for i in x)
        )

    m.setObjective(objective, GRB.MAXIMIZE)

    # Optional solver output
    m.Params.OutputFlag = 1

    m.optimize()

    # -----------------------------
    # Build result JSON
    # -----------------------------
    result = {
        "status_code": int(m.Status),
        "status": "UNKNOWN",
        "objective_value": None,
        "total_credits": 0,
        "selected_courses": [],
        "not_selected_courses": []
    }

    if m.Status == GRB.OPTIMAL:
        result["status"] = "OPTIMAL"
        result["objective_value"] = float(m.ObjVal)

    elif m.Status == GRB.INFEASIBLE:
        result["status"] = "INFEASIBLE"
        save_json(output_path, result)
        return

    elif m.Status == GRB.TIME_LIMIT:
        result["status"] = "TIME_LIMIT"
        if m.SolCount > 0:
            result["objective_value"] = float(m.ObjVal)
    else:
        result["status"] = f"STATUS_{m.Status}"
        if m.SolCount > 0:
            result["objective_value"] = float(m.ObjVal)

    total_credits = 0

    for i, course in enumerate(candidate_courses):
        chosen = 0
        if m.SolCount > 0 and x[i].X > 0.5:
            chosen = 1

        item = {
            "row_no": course.get("row_no"),
            "course_id": course.get("course_id"),
            "class_code": course.get("class_code"),
            "department": course.get("department"),
            "year_available": course.get("year_available"),
            "chinese_course_name": course.get("chinese_course_name"),
            "english_course_name": course.get("english_course_name"),
            "teaching_language": course.get("teaching_language"),
            "type": course.get("type"),
            "timetable_raw": course.get("timetable_raw"),
            "classroom": course.get("classroom"),
            "instructor": course.get("instructor"),
            "credits": course.get("credits"),
            "time_slots": course.get("time_slots", []),
            "preference_score": preference_scores[i]
        }

        if chosen:
            result["selected_courses"].append(item)
            total_credits += int(course.get("credits", 0) or 0)
        else:
            result["not_selected_courses"].append(item)

    result["total_credits"] = total_credits

    save_json(output_path, result)


if __name__ == "__main__":
    # Default paths
    base_dir = os.path.dirname(os.path.abspath(__file__))
    default_input = os.path.join(base_dir, "SchedulerData", "input_schedule.json")
    default_output = os.path.join(base_dir, "SchedulerData", "output_schedule.json")

    input_path = default_input
    output_path = default_output

    if len(sys.argv) >= 2:
        input_path = sys.argv[1]
    if len(sys.argv) >= 3:
        output_path = sys.argv[2]

    solve_schedule(input_path, output_path)
    """

import json
import os
import sys
from collections import defaultdict
from gurobipy import Model, GRB, quicksum


DAYS = [
    "Monday", "Tuesday", "Wednesday", "Thursday",
    "Friday", "Saturday", "Sunday"
]

TIME_SLOTS = [
    "08:10-09:00",
    "09:10-10:00",
    "10:10-11:00",
    "11:10-12:00",
    "12:10-13:00",
    "13:10-14:00",
    "14:10-15:00",
    "15:10-16:00",
    "16:10-17:00",
    "17:10-18:00",
    "18:30-19:20",
    "19:30-20:20",
    "20:30-21:20",
]


def load_json(path):
    with open(path, "r", encoding="utf-8") as f:
        return json.load(f)


def save_json(path, data):
    with open(path, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)


def normalize_text(s):
    return (s or "").strip().lower()


def normalize_course_id(s):
    return (s or "").strip().upper()


def split_keywords(text):
    if not text:
        return []

    parts = [x.strip().lower() for x in text.split(",") if x.strip()]
    ignored = {"none", "no specific", "xxx"}

    return [x for x in parts if x not in ignored]


def safe_int(value, default=0):
    try:
        return int(value or 0)
    except Exception:
        return default


def safe_float(value, default=0.0):
    try:
        return float(value)
    except Exception:
        return default


def is_practical_training(course):
    cid = normalize_course_id(course.get("course_id", ""))
    name = normalize_text(course.get("english_course_name", ""))

    return cid in ["IN421", "IN422", "IN423"] or "practical training" in name


def is_special_project(course):
    cid = normalize_course_id(course.get("course_id", ""))
    name = normalize_text(course.get("english_course_name", ""))

    return cid in ["IN431", "IN432"] or "special project" in name


def is_graduation_activity(course):
    return is_practical_training(course) or is_special_project(course)


def get_course_days(course):
    return {
        ts.get("day", "")
        for ts in course.get("time_slots", [])
        if ts.get("day", "")
    }


def get_course_slots(course):
    slots = set()

    for ts in course.get("time_slots", []):
        start = ts.get("start", "")
        end = ts.get("end", "")

        if start and end:
            slots.add(f"{start}-{end}")

    return slots


def get_course_slot_keys(course):
    keys = set()

    for ts in course.get("time_slots", []):
        key = ts.get("slot_key", "")

        if key:
            keys.add(key)
            continue

        day = ts.get("day", "")
        start = ts.get("start", "")
        end = ts.get("end", "")

        if day and start and end:
            keys.add(f"{day}|{start}-{end}")

    return keys


def course_uses_slot(course, day, slot):
    target = f"{day}|{slot}"
    return target in get_course_slot_keys(course)


def build_course_score(course, preferences, no_need_setting):
    if no_need_setting:
        return 0

    score = 0

    preferred_course_name = normalize_text(preferences.get("preferred_course_name", ""))
    preferred_professor_name = normalize_text(preferences.get("preferred_professor_name", ""))
    preferred_language = normalize_text(preferences.get("preferred_language", ""))

    course_name_en = normalize_text(course.get("english_course_name", ""))
    course_name_zh = normalize_text(course.get("chinese_course_name", ""))
    instructor = normalize_text(course.get("instructor", ""))
    teaching_language = normalize_text(course.get("teaching_language", ""))

    for kw in split_keywords(preferred_course_name):
        if kw in course_name_en or kw in course_name_zh:
            score += 6
            break

    for kw in split_keywords(preferred_professor_name):
        if kw in instructor:
            score += 5
            break

    if preferred_language and preferred_language not in ["no specific", "both"]:
        if preferred_language == teaching_language:
            score += 4

    return score


def build_time_penalty(slot):
    if slot in ["08:10-09:00", "18:30-19:20", "19:30-20:20", "20:30-21:20"]:
        return 5

    if slot in ["09:10-10:00", "17:10-18:00"]:
        return 3

    return 1


def make_output_item(course, preference_score):
    return {
        "row_no": course.get("row_no"),
        "course_id": course.get("course_id"),
        "class_code": course.get("class_code"),
        "department": course.get("department"),
        "year_available": course.get("year_available"),
        "chinese_course_name": course.get("chinese_course_name"),
        "english_course_name": course.get("english_course_name"),
        "teaching_language": course.get("teaching_language"),
        "type": course.get("type"),
        "timetable_raw": course.get("timetable_raw"),
        "classroom": course.get("classroom"),
        "instructor": course.get("instructor"),
        "credits": course.get("credits"),
        "time_slots": course.get("time_slots", []),
        "preference_score": preference_score
    }


def make_alternative_start_item(base_item):
    return {
        "row_no": base_item.get("row_no"),
        "course_id": "IN421 / IN431",
        "class_code": "",
        "department": base_item.get("department"),
        "year_available": base_item.get("year_available"),
        "chinese_course_name": "",
        "english_course_name": "Practical Training (I) / Special Project (I)",
        "teaching_language": base_item.get("teaching_language"),
        "type": "Alternative Graduation Activity",
        "timetable_raw": "",
        "classroom": "",
        "instructor": "",
        "credits": base_item.get("credits", 3),
        "time_slots": [],
        "preference_score": base_item.get("preference_score", 0),
        "note": "The student has not started Practical Training or Special Project. Either Practical Training (I) or Special Project (I) may be selected."
    }


def solve_schedule(input_path, output_path):
    data = load_json(input_path)

    min_credits = safe_int(data.get("min_credits", 0), 0)
    max_credits = safe_int(data.get("max_credits", 0), 0)
    credit_target = max_credits if max_credits > 0 else safe_int(data.get("preferred_credits", 18), 18)

    no_need_setting = bool(data.get("no_need_setting", False))

    candidate_courses = data.get("candidate_courses", [])
    step5_courses = data.get("step5_courses", [])
    preferences = data.get("preferences", {})

    if not candidate_courses:
        save_json(output_path, {
            "status": "NO_CANDIDATE_COURSES",
            "message": "No candidate courses found.",
            "selected_courses": [],
            "not_selected_courses": [],
            "total_credits": 0
        })
        return

    passed_course_ids = {
        normalize_course_id(c.get("course_id", ""))
        for c in step5_courses
        if c.get("passed", False)
    }

    practical_started = "IN421" in passed_course_ids or "IN422" in passed_course_ids or "IN423" in passed_course_ids
    project_started = "IN431" in passed_course_ids or "IN432" in passed_course_ids

    show_start_alternative = (
        not practical_started
        and not project_started
    )

    m = Model("ThesisScheduler")

    x = {}
    for j, course in enumerate(candidate_courses):
        cid = normalize_course_id(course.get("course_id", f"course_{j}"))
        cls = course.get("class_code", "")
        x[j] = m.addVar(vtype=GRB.BINARY, name=f"x_{cid}_{cls}_{j}")

    z = {}
    for j, course in enumerate(candidate_courses):
        for d in DAYS:
            for k in TIME_SLOTS:
                z[j, d, k] = m.addVar(vtype=GRB.BINARY, name=f"z_{j}_{d}_{k}")

    u = {}
    for d in DAYS:
        for k in TIME_SLOTS:
            u[d, k] = m.addVar(vtype=GRB.BINARY, name=f"u_{d}_{k}")

    y = {}
    for d in DAYS:
        y[d] = m.addVar(vtype=GRB.BINARY, name=f"y_{d}")

    g = {}
    for d in DAYS:
        for idx in range(1, len(TIME_SLOTS) - 1):
            k = TIME_SLOTS[idx]
            g[d, k] = m.addVar(vtype=GRB.BINARY, name=f"gap_{d}_{k}")

    y_s = {
        "training_project_group": m.addVar(vtype=GRB.BINARY, name="y_training_project_group")
    }

    m.update()

    course_credits = {
        j: safe_int(course.get("credits", 0), 0)
        for j, course in enumerate(candidate_courses)
    }

    omega = {
        j: build_course_score(course, preferences, no_need_setting)
        for j, course in enumerate(candidate_courses)
    }

    alpha = {
        (d, k): build_time_penalty(k)
        for d in DAYS
        for k in TIME_SLOTS
    }

    beta = safe_float(data.get("beta", 1.0), 1.0)
    eta = safe_float(data.get("eta", 1.0), 1.0)

    F1 = quicksum(course_credits[j] * x[j] for j in x)

    F2 = (
        quicksum(omega[j] * x[j] for j in x)
        - beta * quicksum(alpha[d, k] * u[d, k] for d in DAYS for k in TIME_SLOTS)
        - eta * quicksum(y[d] for d in DAYS)
    )

    F3 = quicksum(g[d, k] for d in DAYS for k in TIME_SLOTS[1:-1])

    course_id_to_indices = defaultdict(list)

    for j, course in enumerate(candidate_courses):
        cid = normalize_course_id(course.get("course_id", ""))
        if cid:
            course_id_to_indices[cid].append(j)

    # Constraint 1: credit limits
    if min_credits > 0:
        m.addConstr(F1 >= min_credits, name="C_min")

    if max_credits > 0:
        m.addConstr(F1 <= max_credits, name="C_max")

    # Constraint 2: at most one section for each course
    for cid, indices in course_id_to_indices.items():
        if len(indices) > 1:
            m.addConstr(
                quicksum(x[j] for j in indices) <= 1,
                name=f"one_section_{cid}"
            )

    # Constraint 3: already-passed courses cannot be selected
    for j, course in enumerate(candidate_courses):
        cid = normalize_course_id(course.get("course_id", ""))

        if cid in passed_course_ids:
            m.addConstr(x[j] == 0, name=f"already_passed_{cid}_{j}")

    # Constraint 4: sequence rules
    required_previous = {
        "IN422": "IN421",
        "IN423": "IN422",
        "IN432": "IN431",
    }

    for current_id, previous_id in required_previous.items():
        if previous_id not in passed_course_ids:
            for j in course_id_to_indices.get(current_id, []):
                m.addConstr(
                    x[j] == 0,
                    name=f"block_{current_id}_without_{previous_id}_{j}"
                )

    # If Practical Training track already started, continue Practical Training only.
    if "IN421" in passed_course_ids and "IN422" not in passed_course_ids:
        for cid in ["IN431", "IN432"]:
            for j in course_id_to_indices.get(cid, []):
                m.addConstr(x[j] == 0, name=f"block_{cid}_after_PT1_started")

    if "IN422" in passed_course_ids and "IN423" not in passed_course_ids:
        for cid in ["IN431", "IN432"]:
            for j in course_id_to_indices.get(cid, []):
                m.addConstr(x[j] == 0, name=f"block_{cid}_after_PT2_started")

    # If Special Project track already started, continue Special Project only.
    if "IN431" in passed_course_ids and "IN432" not in passed_course_ids:
        for cid in ["IN421", "IN422", "IN423"]:
            for j in course_id_to_indices.get(cid, []):
                m.addConstr(x[j] == 0, name=f"block_{cid}_after_SP1_started")

    if "IN432" in passed_course_ids:
        for cid in ["IN421", "IN422", "IN423"]:
            for j in course_id_to_indices.get(cid, []):
                m.addConstr(x[j] == 0, name=f"block_{cid}_after_SP_completed")

    # Constraint 5: link selected course with timetable slots
    for j, course in enumerate(candidate_courses):
        slot_keys = get_course_slot_keys(course)
        r_j = len(slot_keys)

        for d in DAYS:
            for k in TIME_SLOTS:
                if course_uses_slot(course, d, k):
                    m.addConstr(z[j, d, k] == x[j], name=f"z_link_used_{j}_{d}_{k}")
                else:
                    m.addConstr(z[j, d, k] == 0, name=f"z_link_unused_{j}_{d}_{k}")

        m.addConstr(
            quicksum(z[j, d, k] for d in DAYS for k in TIME_SLOTS) == r_j * x[j],
            name=f"meeting_count_{j}"
        )

    # Constraint 6: no time conflict
    for d in DAYS:
        for k in TIME_SLOTS:
            m.addConstr(
                quicksum(z[j, d, k] for j in x) <= 1,
                name=f"no_conflict_{d}_{k}"
            )

    # Constraint 7: slot usage and day usage indicators
    for d in DAYS:
        for k in TIME_SLOTS:
            for j in x:
                m.addConstr(
                    u[d, k] >= z[j, d, k],
                    name=f"slot_usage_{j}_{d}_{k}"
                )

            m.addConstr(
                u[d, k] <= quicksum(z[j, d, k] for j in x),
                name=f"slot_usage_upper_{d}_{k}"
            )

            m.addConstr(
                y[d] >= u[d, k],
                name=f"day_usage_{d}_{k}"
            )

    # Constraint 8: idle gap indicator
    for d in DAYS:
        for idx in range(1, len(TIME_SLOTS) - 1):
            prev_k = TIME_SLOTS[idx - 1]
            curr_k = TIME_SLOTS[idx]
            next_k = TIME_SLOTS[idx + 1]

            m.addConstr(
                g[d, curr_k] >= u[d, prev_k] + u[d, next_k] - 1 - u[d, curr_k],
                name=f"gap_def_{d}_{curr_k}"
            )

    # Constraint 9: binary domains are enforced by GRB.BINARY

    # Constraint 10: practical training / special project group
    activity_indices = []

    for j, course in enumerate(candidate_courses):
        if is_graduation_activity(course):
            activity_indices.append(j)

    if activity_indices:
        m.addConstr(
            quicksum(y_s[s] for s in y_s) <= 1,
            name="group_indicator_limit"
        )

        m.addConstr(
            quicksum(x[j] for j in activity_indices) == y_s["training_project_group"],
            name="training_project_group_count"
        )

    # Strict day/time preferences
    if not no_need_setting:
        preferred_days = set(preferences.get("preferred_class_days", []))
        preferred_day_off = set(preferences.get("preferred_day_off", []))
        preferred_time_slots = set(preferences.get("preferred_time_slots", []))
        avoid_time_slots = set(preferences.get("avoid_time_slots", []))

        for j, course in enumerate(candidate_courses):
            time_slots = course.get("time_slots", [])

            if not time_slots:
                continue

            course_days = get_course_days(course)
            course_slots = get_course_slots(course)

            if preferred_days and not course_days.issubset(preferred_days):
                m.addConstr(x[j] == 0, name=f"strict_preferred_day_{j}")

            if preferred_day_off and course_days.intersection(preferred_day_off):
                m.addConstr(x[j] == 0, name=f"strict_day_off_{j}")

            if preferred_time_slots and not course_slots.issubset(preferred_time_slots):
                m.addConstr(x[j] == 0, name=f"strict_preferred_time_{j}")

            if avoid_time_slots and course_slots.intersection(avoid_time_slots):
                m.addConstr(x[j] == 0, name=f"strict_avoid_time_{j}")

    # Normalized objective
    credit_dev = m.addVar(vtype=GRB.CONTINUOUS, lb=0, name="credit_deviation")

    m.addConstr(credit_dev >= F1 - credit_target, name="credit_dev_pos")
    m.addConstr(credit_dev >= credit_target - F1, name="credit_dev_neg")

    c_min_norm = min_credits if min_credits > 0 else 0
    c_max_norm = max_credits if max_credits > 0 else max(credit_target, 1)
    credit_range = max(c_max_norm - c_min_norm, 1)

    Z1 = 1 - (credit_dev / credit_range)

    max_possible_pref = sum(max(omega[j], 0) for j in x) + len(candidate_courses)
    if max_possible_pref <= 0:
        max_possible_pref = 1

    max_time_penalty = sum(alpha[d, k] for d in DAYS for k in TIME_SLOTS) + len(DAYS)
    F2_max_const = max(max_possible_pref + max_time_penalty, 1)

    Z2 = F2 / F2_max_const

    F3_min_const = 0
    F3_max_const = len(DAYS) * max(len(TIME_SLOTS) - 2, 1)

    Z3 = (F3_max_const - F3) / max(F3_max_const - F3_min_const, 1)

    W1 = safe_float(data.get("W1", 0.40), 0.40)
    W2 = safe_float(data.get("W2", 0.40), 0.40)
    W3 = safe_float(data.get("W3", 0.20), 0.20)

    weight_sum = W1 + W2 + W3

    if weight_sum <= 0:
        W1, W2, W3 = 0.40, 0.40, 0.20
    else:
        W1, W2, W3 = W1 / weight_sum, W2 / weight_sum, W3 / weight_sum

    final_objective = W1 * Z1 + W2 * Z2 + W3 * Z3

    m.setObjective(final_objective, GRB.MAXIMIZE)

    m.Params.OutputFlag = 1
    m.optimize()

    result = {
        "status_code": int(m.Status),
        "status": "UNKNOWN",
        "objective_value": None,
        "total_credits": 0,
        "selected_courses": [],
        "not_selected_courses": [],
        "objective_components": {}
    }

    if m.Status == GRB.OPTIMAL:
        result["status"] = "OPTIMAL"
        result["objective_value"] = float(m.ObjVal)

    elif m.Status == GRB.INFEASIBLE:
        result["status"] = "INFEASIBLE"
        result["message"] = (
            "No feasible schedule found. Please relax preferred days, preferred time slots, "
            "day-off settings, sequence constraints, or credit requirements."
        )
        save_json(output_path, result)
        return

    elif m.Status == GRB.TIME_LIMIT:
        result["status"] = "TIME_LIMIT"
        if m.SolCount > 0:
            result["objective_value"] = float(m.ObjVal)

    else:
        result["status"] = f"STATUS_{m.Status}"
        if m.SolCount > 0:
            result["objective_value"] = float(m.ObjVal)

    total_credits = 0
    selected_items = []
    not_selected_items = []

    for j, course in enumerate(candidate_courses):
        chosen = False

        if m.SolCount > 0 and x[j].X > 0.5:
            chosen = True

        item = make_output_item(course, omega[j])

        if chosen:
            selected_items.append(item)
            total_credits += safe_int(course.get("credits", 0), 0)
        else:
            not_selected_items.append(item)

    # Display alternative start option:
    # if neither track has started and solver selected IN421 or IN431,
    # show "IN421 / IN431" instead of only one course.
    if show_start_alternative:
        new_selected = []

        replaced = False

        for item in selected_items:
            cid = normalize_course_id(item.get("course_id", ""))

            if cid in ["IN421", "IN431"] and not replaced:
                new_selected.append(make_alternative_start_item(item))
                replaced = True
            elif cid in ["IN421", "IN431"]:
                continue
            else:
                new_selected.append(item)

        selected_items = new_selected

    result["selected_courses"] = selected_items
    result["not_selected_courses"] = not_selected_items
    result["total_credits"] = total_credits

    if m.SolCount > 0:
        result["objective_components"] = {
            "F1_total_credits": float(F1.getValue()),
            "F2_preference_quality": float(F2.getValue()),
            "F3_idle_gaps": float(F3.getValue()),
            "Z1_credit_satisfaction": float(Z1.getValue()),
            "Z2_preference_satisfaction": float(Z2.getValue()),
            "Z3_schedule_quality": float(Z3.getValue()),
            "W1": W1,
            "W2": W2,
            "W3": W3
        }

    save_json(output_path, result)


if __name__ == "__main__":
    base_dir = os.path.dirname(os.path.abspath(__file__))

    default_input = os.path.join(base_dir, "SchedulerData", "input_schedule.json")
    default_output = os.path.join(base_dir, "SchedulerData", "output_schedule.json")

    input_path = default_input
    output_path = default_output

    if len(sys.argv) >= 2:
        input_path = sys.argv[1]

    if len(sys.argv) >= 3:
        output_path = sys.argv[2]

    solve_schedule(input_path, output_path)