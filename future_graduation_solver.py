from __future__ import annotations

import json
import sys
from typing import Any

import gurobipy as gp
from gurobipy import GRB


def solve_future_graduation_plan(
    data: dict[str, Any]
) -> dict[str, Any]:
    """
    Solve the future graduation planning model using Gurobi.

    Design B:
    - Candidate courses are optional.
    - Each candidate course can be selected at most once.
    - The model selects enough courses to satisfy the remaining credits.
    """

    # ---------------------------------------------------------
    # 1. Read general parameters
    # ---------------------------------------------------------

    required_credits = int(data["required_credits"])
    completed_credits = int(data["completed_credits"])
    current_credits = int(data["current_credits"])

    max_credits_per_semester = int(
        data.get("max_credits_per_semester", 16)
    )

    program_semesters = int(data["program_semesters"])
    used_semesters = int(data["used_semesters"])

    courses = data.get("courses", [])
    mandatory = {
    str(course["course_id"]).strip():
    bool(course.get("mandatory", False))
    for course in courses}

    # ---------------------------------------------------------
    # 2. Constraint 5: calculate remaining graduation credits
    # ---------------------------------------------------------

    remaining_credits = max(
        required_credits
        - completed_credits
        - current_credits,
        0
    )

    # ---------------------------------------------------------
    # 3. Constraint 3: valid future planning horizon
    # ---------------------------------------------------------

    max_future_semesters = max(
        program_semesters - used_semesters,
        0
    )

    future_semesters = list(
        range(1, max_future_semesters + 1)
    )

    # No future planning is needed.
    if remaining_credits == 0:
        return {
            "status": "OPTIMAL",
            "message": "Graduation credit requirement is already satisfied.",
            "objective_value": 0,
            "required_credits": required_credits,
            "completed_credits": completed_credits,
            "current_credits": current_credits,
            "remaining_credits": 0,
            "estimated_future_semesters": 0,
            "selected_future_credits": 0,
            "future_plan": []
        }

    if max_future_semesters == 0:
        return {
            "status": "NO_FUTURE_SEMESTERS",
            "message": (
                "The student still requires credits, but no future "
                "academic semesters remain in the normal program period."
            ),
            "required_credits": required_credits,
            "completed_credits": completed_credits,
            "current_credits": current_credits,
            "remaining_credits": remaining_credits,
            "estimated_future_semesters": 0,
            "selected_future_credits": 0,
            "future_plan": []
        }

    # ---------------------------------------------------------
    # 4. Constraint 1:
    #    R = J \\ (I union P_current)
    #
    # The C# program can already send only remaining courses.
    # This filtering is retained here as a safety check.
    # ---------------------------------------------------------

    completed_course_ids = {
        str(course_id).strip()
        for course_id in data.get("completed_course_ids", [])
    }

    current_course_ids = {
        str(course_id).strip()
        for course_id in data.get("current_course_ids", [])
    }

    remaining_courses = []

    for course in courses:
        course_id = str(course["course_id"]).strip()

        if not course_id:
            continue

        if course_id in completed_course_ids:
            continue

        if course_id in current_course_ids:
            continue

        remaining_courses.append(course)

    if not remaining_courses:
        return {
            "status": "NO_CANDIDATE_COURSES",
            "message": (
                "No remaining candidate courses are available for "
                "future graduation planning."
            ),
            "required_credits": required_credits,
            "completed_credits": completed_credits,
            "current_credits": current_credits,
            "remaining_credits": remaining_credits,
            "estimated_future_semesters": 0,
            "selected_future_credits": 0,
            "future_plan": []
        }

    course_ids = [
        str(course["course_id"]).strip()
        for course in remaining_courses
    ]

    course_by_id = {
        str(course["course_id"]).strip(): course
        for course in remaining_courses
    }

    credits = {
        course_id: int(course_by_id[course_id]["credits"])
        for course_id in course_ids
    }

    # ---------------------------------------------------------
    # 5. Build availability parameter A[i,f]
    # ---------------------------------------------------------

    availability: dict[tuple[str, int], int] = {}

    for course_id in course_ids:
        course = course_by_id[course_id]

        earliest_semester = int(
            course.get("earliest_semester", 1)
        )

        offered_semesters_raw = course.get(
            "offered_semesters",
            future_semesters
        )

        offered_semesters = {
            int(semester)
            for semester in offered_semesters_raw
        }

        for semester in future_semesters:
            availability[course_id, semester] = (
                1
                if (
                    semester in offered_semesters
                    and semester >= earliest_semester
                )
                else 0
            )

    # ---------------------------------------------------------
    # 6. Create Gurobi model
    # ---------------------------------------------------------

    model = gp.Model("FutureGraduationPlanning")

    model.Params.OutputFlag = int(
        data.get("show_gurobi_log", 0)
    )

    # ---------------------------------------------------------
    # 7. Decision variables
    # ---------------------------------------------------------

    # x[i,f] = 1 if course i is selected in future semester f
    x = model.addVars(
        course_ids,
        future_semesters,
        vtype=GRB.BINARY,
        name="x"
    )

    # y[f] = 1 if future semester f is used
    y = model.addVars(
        future_semesters,
        vtype=GRB.BINARY,
        name="y"
    )

    # z[i] = 1 if course i is selected in any future semester
    #
    # This extra variable is useful for Design B and prerequisite
    # relationships.
    z = model.addVars(
        course_ids,
        vtype=GRB.BINARY,
        name="z"
    )

    # ---------------------------------------------------------
    # 8. Objective function
    #
    # Equation (23):
    # min Z1 = sum_f y[f]
    # ---------------------------------------------------------

    model.setObjective(
        gp.quicksum(
            y[semester]
            for semester in future_semesters
        ),
        GRB.MINIMIZE
    )

    # ---------------------------------------------------------
    # 9. Constraint 2:
    #    Each candidate course is selected at most once.
    #
    # sum_f x[i,f] <= 1
    #
    # z[i] = sum_f x[i,f]
    # ---------------------------------------------------------

    for course_id in course_ids:
        model.addConstr(
            gp.quicksum(
                x[course_id, semester]
                for semester in future_semesters
            ) == z[course_id],
            name=f"course_selection_{course_id}"
        )

    for course_id in course_ids:
        if mandatory.get(course_id, False):
            model.addConstr(
                z[course_id] == 1,
                name=f"mandatory_course_{course_id}"
            )

    # Because z[i] is binary, the equality above automatically gives:
    #
    # sum x[i,f] <= 1
    # sum x[i,f] == 1

    # ---------------------------------------------------------
    # 10. Link course assignments to semester-use variables
    #
    # x[i,f] <= y[f]
    # ---------------------------------------------------------

    for course_id in course_ids:
        for semester in future_semesters:
            model.addConstr(
                x[course_id, semester] <= y[semester],
                name=(
                    f"semester_activation_"
                    f"{course_id}_{semester}"
                )
            )

    # Strengthening constraint:
    # A used semester must contain at least one selected course.
    for semester in future_semesters:
        model.addConstr(
            y[semester]
            <= gp.quicksum(
                x[course_id, semester]
                for course_id in course_ids
            ),
            name=f"semester_has_course_{semester}"
        )

    # ---------------------------------------------------------
    # 11. Equations (24) and (25), Constraint 6:
    #     course availability
    #
    # x[i,f] <= A[i,f]
    # ---------------------------------------------------------

    for course_id in course_ids:
        for semester in future_semesters:
            model.addConstr(
                x[course_id, semester]
                <= availability[course_id, semester],
                name=(
                    f"course_availability_"
                    f"{course_id}_{semester}"
                )
            )

    # ---------------------------------------------------------
    # 12. Constraint 4:
    #     Maximum credit limit per future semester
    #
    # sum_i c[i] x[i,f] <= Cmax
    # ---------------------------------------------------------

    for semester in future_semesters:
        model.addConstr(
            gp.quicksum(
                credits[course_id]
                * x[course_id, semester]
                for course_id in course_ids
            )
            <= max_credits_per_semester,
            name=f"semester_credit_limit_{semester}"
        )

    # ---------------------------------------------------------
    # 13. Constraint 5:
    #     Remaining graduation credit requirement
    #
    # sum_f sum_i c[i] x[i,f] >= C_remaining
    # ---------------------------------------------------------

    selected_credit_expression = gp.quicksum(
        credits[course_id]
        * x[course_id, semester]
        for course_id in course_ids
        for semester in future_semesters
    )

    model.addConstr(
        selected_credit_expression >= remaining_credits,
        name="remaining_graduation_credits"
    )

    # ---------------------------------------------------------
    # 14. Constraint 7:
    #     Prerequisite-course ordering
    #
    # If course i is selected and prerequisite p is still in R,
    # prerequisite p must also be selected and must be scheduled
    # before course i.
    # ---------------------------------------------------------

    big_m = max_future_semesters + 1

    prerequisite_pairs: list[tuple[str, str]] = []

    for course_id in course_ids:
        course = course_by_id[course_id]

        prerequisites = course.get("prerequisites", [])

        for prerequisite_id_raw in prerequisites:
            prerequisite_id = str(
                prerequisite_id_raw
            ).strip()

            if not prerequisite_id:
                continue

            # A prerequisite already completed is satisfied.
            if prerequisite_id in completed_course_ids:
                continue

            # A prerequisite currently being taken is treated
            # as satisfied before the future planning horizon.
            if prerequisite_id in current_course_ids:
                continue

            # Only create an ordering relationship if the
            # prerequisite is also a candidate future course.
            if prerequisite_id in course_by_id:
                prerequisite_pairs.append(
                    (prerequisite_id, course_id)
                )

    for prerequisite_id, course_id in prerequisite_pairs:

        # If the dependent course is selected, its prerequisite
        # must also be selected.
        #
        # z[i] <= z[p]
        model.addConstr(
            z[course_id] <= z[prerequisite_id],
            name=(
                f"prerequisite_selection_"
                f"{prerequisite_id}_{course_id}"
            )
        )

        prerequisite_semester_expression = gp.quicksum(
            semester
            * x[prerequisite_id, semester]
            for semester in future_semesters
        )

        course_semester_expression = gp.quicksum(
            semester
            * x[course_id, semester]
            for semester in future_semesters
        )

        # Conditional ordering:
        #
        # semester(p) + 1 <= semester(i)
        #
        # The Big-M term deactivates the constraint when course i
        # is not selected.
        model.addConstr(
            prerequisite_semester_expression + 1
            <= course_semester_expression
            + big_m * (1 - z[course_id]),
            name=(
                f"prerequisite_order_"
                f"{prerequisite_id}_{course_id}"
            )
        )

    # ---------------------------------------------------------
    # 15. Prevent unused gaps in the semester plan
    #
    # y[f] >= y[f+1]
    # ---------------------------------------------------------

    for semester in future_semesters[:-1]:
        model.addConstr(
            y[semester] >= y[semester + 1],
            name=f"consecutive_semesters_{semester}"
        )

    # ---------------------------------------------------------
    # 16. Secondary objective:
    #     Avoid selecting unnecessary extra credits.
    #
    # Primary objective: minimize semesters.
    # Secondary objective: minimize selected credits.
    # ---------------------------------------------------------

    model.ModelSense = GRB.MINIMIZE

    model.setObjectiveN(
        gp.quicksum(
            y[semester]
            for semester in future_semesters
        ),
        index=0,
        priority=2,
        weight=1.0,
        name="MinimizeFutureSemesters"
    )

    model.setObjectiveN(
        selected_credit_expression,
        index=1,
        priority=1,
        weight=1.0,
        name="MinimizeSelectedCredits"
    )

    # ---------------------------------------------------------
    # 17. Solve
    # ---------------------------------------------------------

    model.optimize()

    # ---------------------------------------------------------
    # 18. Handle infeasibility
    # ---------------------------------------------------------

    if model.Status == GRB.INFEASIBLE:
        model.computeIIS()

        iis_constraints = [
            constraint.ConstrName
            for constraint in model.getConstrs()
            if constraint.IISConstr
        ]

        return {
            "status": "INFEASIBLE",
            "message": (
                "No feasible future graduation plan was found. "
                "Check course availability, prerequisites, remaining "
                "credits, semester limits, and credit limits."
            ),
            "remaining_credits": remaining_credits,
            "estimated_future_semesters": 0,
            "selected_future_credits": 0,
            "iis_constraints": iis_constraints,
            "future_plan": []
        }

    if model.Status != GRB.OPTIMAL:
        return {
            "status": str(model.Status),
            "message": (
                "Gurobi did not return an optimal solution."
            ),
            "remaining_credits": remaining_credits,
            "estimated_future_semesters": 0,
            "selected_future_credits": 0,
            "future_plan": []
        }

    # ---------------------------------------------------------
    # 19. Read solution
    # ---------------------------------------------------------

    future_plan: list[dict[str, Any]] = []
    used_future_semesters: list[int] = []
    selected_future_credits = 0

    for semester in future_semesters:
        if y[semester].X > 0.5:
            used_future_semesters.append(semester)

        for course_id in course_ids:
            if x[course_id, semester].X > 0.5:
                course = course_by_id[course_id]
                course_credits = credits[course_id]

                selected_future_credits += course_credits

                future_plan.append({
                    "expected_semester": semester,
                    "course_id": course_id,
                    "course_name": course.get(
                        "course_name",
                        ""
                    ),
                    "credits": course_credits,
                    "category": course.get(
                        "category",
                        ""
                    )
                })

    future_plan.sort(
        key=lambda item: (
            item["expected_semester"],
            item["course_id"]
        )
    )

    estimated_future_semesters = len(
        used_future_semesters
    )

    return {
        "status": "OPTIMAL",
        "message": (
            "A feasible future graduation plan was generated."
        ),
        "objective_value": estimated_future_semesters,
        "required_credits": required_credits,
        "completed_credits": completed_credits,
        "current_credits": current_credits,
        "remaining_credits": remaining_credits,
        "selected_future_credits": selected_future_credits,
        "estimated_future_semesters": (
            estimated_future_semesters
        ),
        "future_plan": future_plan
    }


def main() -> None:
    try:
        input_text = sys.stdin.read()

        if not input_text.strip():
            raise ValueError(
                "No JSON input was received."
            )

        input_data = json.loads(input_text)

        result = solve_future_graduation_plan(
            input_data
        )

        print(
            json.dumps(
                result,
                ensure_ascii=False,
                indent=2
            )
        )

    except gp.GurobiError as ex:
        error_result = {
            "status": "GUROBI_ERROR",
            "message": str(ex),
            "future_plan": []
        }

        print(
            json.dumps(
                error_result,
                ensure_ascii=False
            )
        )

        sys.exit(1)

    except Exception as ex:
        error_result = {
            "status": "ERROR",
            "message": str(ex),
            "future_plan": []
        }

        print(
            json.dumps(
                error_result,
                ensure_ascii=False
            )
        )

        sys.exit(1)


if __name__ == "__main__":
    main()