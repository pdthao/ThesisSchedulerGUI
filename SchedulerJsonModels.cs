using System.Collections.Generic;

namespace ThesisSchedulerGUI
{
    public class SchedulerInputJson
    {
        public int min_credits { get; set; } = 0;
        public int max_credits { get; set; } = 0;

        public string academic_year { get; set; } = "";
        public bool no_need_setting { get; set; }

        public List<string> unpassed_course_ids { get; set; } = new List<string>();
        public List<Step5CourseJson> step5_courses { get; set; } = new List<Step5CourseJson>();
        public List<CandidateCourseJson> candidate_courses { get; set; } = new List<CandidateCourseJson>();
        public PreferenceJson preferences { get; set; } = new PreferenceJson();
    }

    public class Step5CourseJson
    {
        public string course_id { get; set; } = "";
        public string credits { get; set; } = "";
        public string english_name_course { get; set; } = "";
        public string category { get; set; } = "";
        public bool passed { get; set; }
    }

    public class CandidateCourseJson
    {
        public int row_no { get; set; }
        public string course_id { get; set; } = "";
        public string class_code { get; set; } = "";
        public string department { get; set; } = "";
        public string year_available { get; set; } = "";
        public string chinese_course_name { get; set; } = "";
        public string english_course_name { get; set; } = "";
        public string teaching_language { get; set; } = "";
        public string type { get; set; } = "";
        public string timetable_raw { get; set; } = "";
        public string classroom { get; set; } = "";
        public string instructor { get; set; } = "";
        public int credits { get; set; }
        public List<TimeSlotJson> time_slots { get; set; } = new List<TimeSlotJson>();
        public List<string> prerequisites { get; set; } = new List<string>();
    }

    public class TimeSlotJson
    {
        public string day { get; set; } = "";
        public string start { get; set; } = "";
        public string end { get; set; } = "";
        public string slot_key { get; set; } = "";
    }

    public class PreferenceJson
    {
        public List<string> preferred_class_days { get; set; } = new List<string>();
        public List<string> preferred_day_off { get; set; } = new List<string>();
        public List<string> preferred_time_slots { get; set; } = new List<string>();
        public List<string> avoid_time_slots { get; set; } = new List<string>();

        public string preferred_course_name { get; set; } = "";
        public string preferred_professor_name { get; set; } = "";
        public string preferred_language { get; set; } = "";
    }

    public class SchedulerOutputJson
    {
        public int status_code { get; set; }
        public string status { get; set; } = "";
        public double? objective_value { get; set; }
        public int total_credits { get; set; }
        public List<SelectedCourseJson> selected_courses { get; set; } = new List<SelectedCourseJson>();
        public List<SelectedCourseJson> not_selected_courses { get; set; } = new List<SelectedCourseJson>();
    }

    public class SelectedCourseJson
    {
        public int row_no { get; set; }
        public string course_id { get; set; } = "";
        public string class_code { get; set; } = "";
        public string department { get; set; } = "";
        public string year_available { get; set; } = "";
        public string chinese_course_name { get; set; } = "";
        public string english_course_name { get; set; } = "";
        public string teaching_language { get; set; } = "";
        public string type { get; set; } = "";
        public string timetable_raw { get; set; } = "";
        public string classroom { get; set; } = "";
        public string instructor { get; set; } = "";
        public int credits { get; set; }
        public List<TimeSlotJson> time_slots { get; set; } = new List<TimeSlotJson>();
        public int preference_score { get; set; }
    }
}