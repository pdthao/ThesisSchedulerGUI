using System.Collections.Generic;

namespace ThesisSchedulerGUI
{
    // ============================================================
    // C# -> Python
    // Input JSON Model
    // ============================================================

    public class FuturePlanningInput
    {
        // Graduation information
        public int required_credits { get; set; }
        public int completed_credits { get; set; }
        public int current_credits { get; set; }

        // Planning settings
        public int max_credits_per_semester { get; set; }

        public int program_semesters { get; set; }
        public int used_semesters { get; set; }

        // Optional: show Gurobi console log (0 = hide, 1 = show)
        public int show_gurobi_log { get; set; }

        // Completed courses
        public List<string> completed_course_ids { get; set; }
            = new List<string>();

        // Current semester selected courses
        public List<string> current_course_ids { get; set; }
            = new List<string>();

        // Candidate future courses
        public List<FuturePlanningInputCourse> courses { get; set; }
            = new List<FuturePlanningInputCourse>();
    }

    public class FuturePlanningInputCourse
    {
        public string course_id { get; set; }

        public string course_name { get; set; }

        public int credits { get; set; }

        public string category { get; set; }

        // Earliest semester that the course can be taken
        public int earliest_semester { get; set; }

        // Course offering semesters
        public List<int> offered_semesters { get; set; }
            = new List<int>();

        // Prerequisite course IDs
        public List<string> prerequisites { get; set; }
            = new List<string>();

        public bool mandatory { get; set; }
    }

    // ============================================================
    // Python -> C#
    // Output JSON Model
    // ============================================================

    public class FuturePlanningResult
    {
        public string status { get; set; }

        public string message { get; set; }

        public double objective_value { get; set; }

        public int required_credits { get; set; }

        public int completed_credits { get; set; }

        public int current_credits { get; set; }

        public int remaining_credits { get; set; }

        public int selected_future_credits { get; set; }

        public int estimated_future_semesters { get; set; }

        public List<string> iis_constraints { get; set; }
            = new List<string>();

        public List<FuturePlanCourse> future_plan { get; set; }
            = new List<FuturePlanCourse>();
    }

    public class FuturePlanCourse
    {
        public int expected_semester { get; set; }

        public string course_id { get; set; }

        public string course_name { get; set; }

        public int credits { get; set; }

        public string category { get; set; }
    }
}