/*using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ThesisSchedulerGUI
{
    public partial class Form3 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;
        private const int TIMETABLE_ROW_COUNT = 13;

        private Form2 _form2;

        private SchedulerOutputJson _lastSolverResult;


        public Form3()
        {
            InitializeComponent();
            InitializeFormData();
            SetPlaceholders();
            InitializeRecommendScheduleGrid();
            ApplyNoNeedSettingState();
        }

        public Form3(Form2 form2)
        {
            InitializeComponent();
            _form2 = form2;

            InitializeFormData();
            SetPlaceholders();
            InitializeRecommendScheduleGrid();
            ApplyNoNeedSettingState();
        }

        // =========================
        // Initialization
        // =========================
        private void InitializeFormData()
        {
            comboBoxAcademicYear.Items.Clear();
            comboBoxAcademicYear.Items.AddRange(new string[]
            {
                "Bachelor 1st Year (Freshman), Semester 1",
                "Bachelor 1st Year (Freshman), Semester 2",
                "Bachelor 2nd Year (Sophomore), Semester 1",
                "Bachelor 2nd Year (Sophomore), Semester 2",
                "Bachelor 3rd Year (Junior), Semester 1",
                "Bachelor 3rd Year (Junior), Semester 2",
                "Bachelor 4th Year (Senior), Semester 1",
                "Bachelor 4th Year (Senior), Semester 2",
                "Master 1st Year, Semester 1",
                "Master 1st Year, Semester 2",
                "Master 2nd Year, Semester 1",
                "Master 2nd Year, Semester 2",
                "PhD 1st Year, Semester 1",
                "PhD 1st Year, Semester 2",
                "PhD 2nd Year, Semester 1",
                "PhD 2nd Year, Semester 2",
                "PhD 3rd Year, Semester 1",
                "PhD 3rd Year, Semester 2",
                "PhD 4th Year, Semester 1",
                "PhD 4th Year, Semester 2"
            });
            comboBoxAcademicYear.SelectedIndex = -1;

            comboBoxCreditNumber.Items.Clear();
            comboBoxCreditNumber.Items.Add("None");
            for (int i = 0; i <= 31; i++)
                comboBoxCreditNumber.Items.Add(i.ToString());
            comboBoxCreditNumber.SelectedIndex = 0;

            comboBoxOtherTimeSlotAvoid.Items.Clear();
            comboBoxOtherTimeSlotAvoid.Items.Add("None");
            comboBoxOtherTimeSlotAvoid.Items.AddRange(new string[]
            {
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
                "20:30-21:20"
            });
            comboBoxOtherTimeSlotAvoid.SelectedIndex = 0;

            comboBoxLanguage.Items.Clear();
            comboBoxLanguage.Items.AddRange(new string[]
            {
                "No Specific",
                "Both",
                "English",
                "Chinese"
            });
            comboBoxLanguage.SelectedIndex = 0;

            txtOtherPreferredCredits.Enabled = false;
            txtOtherTimeSlotAvoid.Enabled = false;
        }

        private void SetPlaceholders()
        {
            SendMessage(txtOtherPreferredCredits.Handle, EM_SETCUEBANNER, 0, "36");
            SendMessage(txtOtherTimeSlotAvoid.Handle, EM_SETCUEBANNER, 0, "08:10-09:00, 09:10-10:00");
            SendMessage(txtCourseName.Handle, EM_SETCUEBANNER, 0, "None, Python, C++");
            SendMessage(txtProfessorName.Handle, EM_SETCUEBANNER, 0, "None, 芳妙草, XXX");
        }

        private void InitializeRecommendScheduleGrid()
        {
            dataGridRecommendSchedule.Rows.Clear();
            dataGridRecommendSchedule.Columns.Clear();

            dataGridRecommendSchedule.AllowUserToAddRows = false;
            dataGridRecommendSchedule.AllowUserToDeleteRows = false;
            dataGridRecommendSchedule.AllowUserToResizeRows = true;
            dataGridRecommendSchedule.AllowUserToResizeColumns = true;
            dataGridRecommendSchedule.ReadOnly = true;
            dataGridRecommendSchedule.RowHeadersVisible = false;
            dataGridRecommendSchedule.MultiSelect = false;
            dataGridRecommendSchedule.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridRecommendSchedule.ScrollBars = ScrollBars.Both;
            dataGridRecommendSchedule.BackgroundColor = Color.White;
            dataGridRecommendSchedule.BorderStyle = BorderStyle.FixedSingle;
            dataGridRecommendSchedule.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridRecommendSchedule.EnableHeadersVisualStyles = false;

            dataGridRecommendSchedule.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridRecommendSchedule.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridRecommendSchedule.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dataGridRecommendSchedule.DefaultCellStyle.Padding = new Padding(4);

            dataGridRecommendSchedule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridRecommendSchedule.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridRecommendSchedule.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridRecommendSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridRecommendSchedule.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridRecommendSchedule.Columns.Add("TimeSection", "Time / Day");
            dataGridRecommendSchedule.Columns.Add("Monday", "Monday");
            dataGridRecommendSchedule.Columns.Add("Tuesday", "Tuesday");
            dataGridRecommendSchedule.Columns.Add("Wednesday", "Wednesday");
            dataGridRecommendSchedule.Columns.Add("Thursday", "Thursday");
            dataGridRecommendSchedule.Columns.Add("Friday", "Friday");
            dataGridRecommendSchedule.Columns.Add("Saturday", "Saturday");
            dataGridRecommendSchedule.Columns.Add("Sunday", "Sunday");

            dataGridRecommendSchedule.Columns["TimeSection"].Width = 120;
            dataGridRecommendSchedule.Columns["Monday"].Width = 180;
            dataGridRecommendSchedule.Columns["Tuesday"].Width = 180;
            dataGridRecommendSchedule.Columns["Wednesday"].Width = 180;
            dataGridRecommendSchedule.Columns["Thursday"].Width = 180;
            dataGridRecommendSchedule.Columns["Friday"].Width = 180;
            dataGridRecommendSchedule.Columns["Saturday"].Width = 180;
            dataGridRecommendSchedule.Columns["Sunday"].Width = 180;

            string[] timeSlots =
            {
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
                "20:30-21:20"
            };

            dataGridRecommendSchedule.CellPainting -= dataGridRecommendSchedule_CellPainting;
            dataGridRecommendSchedule.CellPainting += dataGridRecommendSchedule_CellPainting;

            foreach (string slot in timeSlots)
            {
                int rowIndex = dataGridRecommendSchedule.Rows.Add();
                dataGridRecommendSchedule.Rows[rowIndex].Cells[0].Value = slot;
                dataGridRecommendSchedule.Rows[rowIndex].MinimumHeight = 55;
            }
        }

        private void ApplyNoNeedSettingState()
        {
            bool enableControls = !checkBox28.Checked;
            SetPreferenceControlsEnabled(enableControls);
        }

        private void SetPreferenceControlsEnabled(bool enabled)
        {
            comboBoxAcademicYear.Enabled = true;
            checkBoxOtherPreferredCredits.Enabled = true;
            comboBoxCreditNumber.Enabled = !checkBoxOtherPreferredCredits.Checked;
            txtOtherPreferredCredits.Enabled = checkBoxOtherPreferredCredits.Checked;

            comboBoxOtherTimeSlotAvoid.Enabled = enabled && !checkBoxOtherTimeSlotAvoid.Checked;
            checkBoxOtherTimeSlotAvoid.Enabled = enabled;
            txtOtherTimeSlotAvoid.Enabled = enabled && checkBoxOtherTimeSlotAvoid.Checked;

            comboBoxLanguage.Enabled = enabled;
            txtCourseName.Enabled = enabled;
            txtProfessorName.Enabled = enabled;

            checkBox1.Enabled = enabled;
            checkBox2.Enabled = enabled;
            checkBox4.Enabled = enabled;
            checkBox3.Enabled = enabled;
            checkBox5.Enabled = enabled;
            checkBox6.Enabled = enabled;
            checkBox7.Enabled = enabled;

            checkBox27.Enabled = enabled;
            checkBox26.Enabled = enabled;
            checkBox25.Enabled = enabled;
            checkBox24.Enabled = enabled;
            checkBox23.Enabled = enabled;
            checkBox22.Enabled = enabled;
            checkBox21.Enabled = enabled;

            checkBox8.Enabled = enabled;
            checkBox9.Enabled = enabled;
            checkBox11.Enabled = enabled;
            checkBox10.Enabled = enabled;
            checkBox12.Enabled = enabled;
            checkBox17.Enabled = enabled;
            checkBox16.Enabled = enabled;
            checkBox15.Enabled = enabled;
            checkBox14.Enabled = enabled;
            checkBox13.Enabled = enabled;
            checkBox20.Enabled = enabled;
            checkBox19.Enabled = enabled;
            checkBox18.Enabled = enabled;
        }

        private void ClearAllPreferences()
        {
            comboBoxAcademicYear.SelectedIndex = -1;
            comboBoxCreditNumber.SelectedIndex = 0;
            comboBoxOtherTimeSlotAvoid.SelectedIndex = 0;
            comboBoxLanguage.SelectedIndex = 0;

            txtOtherPreferredCredits.Clear();
            txtOtherTimeSlotAvoid.Clear();
            txtCourseName.Clear();
            txtProfessorName.Clear();

            checkBoxOtherPreferredCredits.Checked = false;
            checkBoxOtherTimeSlotAvoid.Checked = false;
            checkBox28.Checked = false;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
            checkBox3.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;

            checkBox27.Checked = false;
            checkBox26.Checked = false;
            checkBox25.Checked = false;
            checkBox24.Checked = false;
            checkBox23.Checked = false;
            checkBox22.Checked = false;
            checkBox21.Checked = false;

            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox11.Checked = false;
            checkBox10.Checked = false;
            checkBox12.Checked = false;
            checkBox17.Checked = false;
            checkBox16.Checked = false;
            checkBox15.Checked = false;
            checkBox14.Checked = false;
            checkBox13.Checked = false;
            checkBox20.Checked = false;
            checkBox19.Checked = false;
            checkBox18.Checked = false;

            ApplyNoNeedSettingState();
        }

        // =========================
        // Helper Methods
        // =========================
        private List<string> GetSelectedPreferredClassDays()
        {
            List<string> days = new List<string>();

            if (checkBox1.Checked) days.Add("Monday");
            if (checkBox2.Checked) days.Add("Tuesday");
            if (checkBox4.Checked) days.Add("Wednesday");
            if (checkBox3.Checked) days.Add("Thursday");
            if (checkBox5.Checked) days.Add("Friday");
            if (checkBox6.Checked) days.Add("Saturday");
            if (checkBox7.Checked) days.Add("Sunday");

            return days;
        }

        private List<string> GetSelectedPreferredDayOff()
        {
            List<string> days = new List<string>();

            if (checkBox27.Checked) days.Add("Monday");
            if (checkBox26.Checked) days.Add("Tuesday");
            if (checkBox25.Checked) days.Add("Wednesday");
            if (checkBox24.Checked) days.Add("Thursday");
            if (checkBox23.Checked) days.Add("Friday");
            if (checkBox22.Checked) days.Add("Saturday");
            if (checkBox21.Checked) days.Add("Sunday");

            return days;
        }

        private List<string> GetSelectedPreferredTimeSlots()
        {
            List<string> slots = new List<string>();

            if (checkBox8.Checked) slots.Add("08:10-09:00");
            if (checkBox9.Checked) slots.Add("09:10-10:00");
            if (checkBox11.Checked) slots.Add("10:10-11:00");
            if (checkBox10.Checked) slots.Add("11:10-12:00");
            if (checkBox12.Checked) slots.Add("12:10-13:00");
            if (checkBox17.Checked) slots.Add("13:10-14:00");
            if (checkBox16.Checked) slots.Add("14:10-15:00");
            if (checkBox15.Checked) slots.Add("15:10-16:00");
            if (checkBox14.Checked) slots.Add("16:10-17:00");
            if (checkBox13.Checked) slots.Add("17:10-18:00");
            if (checkBox20.Checked) slots.Add("18:30-19:20");
            if (checkBox19.Checked) slots.Add("19:30-20:20");
            if (checkBox18.Checked) slots.Add("20:30-21:20");

            return slots;
        }

        private string NormalizeCourseId(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";

            s = s.Trim().ToUpperInvariant();
            s = s.Replace("\u00A0", " ").Replace("\u200B", "");
            s = Regex.Replace(s, @"\s+", "");
            s = Regex.Replace(s, @"^([A-Z]{2}\d{3}).*$", "$1");

            return s;
        }

        private int ParseIntSafe(string s)
        {
            if (int.TryParse((s ?? "").Trim(), out int value))
                return value;

            return 0;
        }

        private string CleanTextOrNone(string s)
        {
            s = (s ?? "").Trim();
            if (string.IsNullOrWhiteSpace(s)) return "";
            if (s.Equals("None", StringComparison.OrdinalIgnoreCase)) return "";
            return s;
        }

        private int GetMaxCreditsFromForm3()
        {
            string value = checkBoxOtherPreferredCredits.Checked
                ? txtOtherPreferredCredits.Text.Trim()
                : (comboBoxCreditNumber.SelectedItem?.ToString() ?? "");

            if (string.IsNullOrWhiteSpace(value) ||
                value.Equals("None", StringComparison.OrdinalIgnoreCase))
                return 0;

            if (int.TryParse(value, out int credits))
                return credits;

            return 0;
        }

        private List<Step5CourseJson> BuildStep5Courses()
        {
            List<Step5CourseJson> result = new List<Step5CourseJson>();

            if (_form2 == null) return result;

            DataTable dt = _form2.GetStep5Table();
            if (dt == null) return result;

            foreach (DataRow row in dt.Rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");
                if (string.IsNullOrWhiteSpace(courseId))
                    continue;

                result.Add(new Step5CourseJson
                {
                    course_id = courseId,
                    credits = row["Credits"]?.ToString()?.Trim() ?? "",
                    english_name_course = row["English Name Course"]?.ToString()?.Trim() ?? "",
                    category = row["Category"]?.ToString()?.Trim() ?? "",
                    passed = (row["Check passed"]?.ToString()?.Trim() ?? "") == "✓"
                });
            }

            return result;
        }

        private List<string> GetUnpassedCourseIds()
        {
            List<string> result = new List<string>();

            if (_form2 == null) return result;

            DataTable dt = _form2.GetStep5Table();
            if (dt == null) return result;

            foreach (DataRow row in dt.Rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");
                string checkPassed = row["Check passed"]?.ToString()?.Trim() ?? "";

                if (!string.IsNullOrWhiteSpace(courseId) && checkPassed != "✓")
                    result.Add(courseId);
            }

            return result
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private List<DataRow> GetMatchedStep6Rows()
        {
            List<DataRow> result = new List<DataRow>();

            if (_form2 == null) return result;

            DataTable dt = _form2.GetStep6Table();
            if (dt == null) return result;

            HashSet<string> unpassed = new HashSet<string>(
                GetUnpassedCourseIds(),
                StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in dt.Rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");

                if (unpassed.Contains(courseId))
                    result.Add(row);
            }

            return result;
        }

        private List<TimeSlotJson> ParseTimeSlotsFromTimetable(string timetableText)
        {
            List<TimeSlotJson> result = new List<TimeSlotJson>();

            if (string.IsNullOrWhiteSpace(timetableText))
                return result;

            string[] codes = timetableText
                .Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string code in codes)
            {
                string c = code.Trim();

                if (!Regex.IsMatch(c, @"^\d{3}$"))
                    continue;

                string day = MapDayCode(c.Substring(0, 1));
                string timeSlot = MapPeriodCode(c.Substring(1, 2));

                if (string.IsNullOrWhiteSpace(day) || string.IsNullOrWhiteSpace(timeSlot))
                    continue;

                string[] parts = timeSlot.Split('-');
                if (parts.Length != 2)
                    continue;

                result.Add(new TimeSlotJson
                {
                    day = day,
                    start = parts[0],
                    end = parts[1],
                    slot_key = $"{day}|{timeSlot}"
                });
            }

            return result;
        }

        private string MapDayCode(string dayCode)
        {
            switch (dayCode)
            {
                case "1": return "Monday";
                case "2": return "Tuesday";
                case "3": return "Wednesday";
                case "4": return "Thursday";
                case "5": return "Friday";
                case "6": return "Saturday";
                case "7": return "Sunday";
                default: return "";
            }
        }

        private string MapPeriodCode(string periodCode)
        {
            switch (periodCode)
            {
                case "01": return "08:10-09:00";
                case "02": return "09:10-10:00";
                case "03": return "10:10-11:00";
                case "04": return "11:10-12:00";
                case "05": return "12:10-13:00";
                case "06": return "13:10-14:00";
                case "07": return "14:10-15:00";
                case "08": return "15:10-16:00";
                case "09": return "16:10-17:00";
                case "10": return "17:10-18:00";
                case "11": return "18:30-19:20";
                case "12": return "19:30-20:20";
                case "13": return "20:30-21:20";
                default: return "";
            }
        }

        private Dictionary<string, int> BuildStep5CreditMap()
        {
            Dictionary<string, int> result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            if (_form2 == null) return result;

            DataTable dt = _form2.GetStep5Table();
            if (dt == null) return result;

            foreach (DataRow row in dt.Rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");
                int credits = ParseIntSafe(row["Credits"]?.ToString() ?? "");

                if (!string.IsNullOrWhiteSpace(courseId) && !result.ContainsKey(courseId))
                    result[courseId] = credits;
            }

            return result;
        }

        private List<CandidateCourseJson> BuildCandidateCourses()
        {
            List<CandidateCourseJson> result = new List<CandidateCourseJson>();

            List<DataRow> rows = GetMatchedStep6Rows();

            int no = 1;
            foreach (DataRow row in rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");

                CandidateCourseJson item = new CandidateCourseJson
                {
                    row_no = no++,
                    course_id = courseId,
                    class_code = row["Class"]?.ToString()?.Trim() ?? "",
                    department = row["Department"]?.ToString()?.Trim() ?? "",
                    year_available = row["Year Available"]?.ToString()?.Trim() ?? "",
                    chinese_course_name = row["Chinese Course Name"]?.ToString()?.Trim() ?? "",
                    english_course_name = row["English Course Name"]?.ToString()?.Trim() ?? "",
                    teaching_language = row["Teaching Language"]?.ToString()?.Trim() ?? "",
                    type = row["Type"]?.ToString()?.Trim() ?? "",
                    timetable_raw = row["Timetable"]?.ToString()?.Trim() ?? "",
                    classroom = row["Classroom"]?.ToString()?.Trim() ?? "",
                    instructor = row["Instructor"]?.ToString()?.Trim() ?? "",
                    credits = 0,
                    time_slots = ParseTimeSlotsFromTimetable(row["Timetable"]?.ToString()?.Trim() ?? ""),
                    prerequisites = new List<string>()
                };

                result.Add(item);
            }

            Dictionary<string, int> step5CreditMap = BuildStep5CreditMap();

            foreach (var item in result)
            {
                if (step5CreditMap.ContainsKey(item.course_id))
                    item.credits = step5CreditMap[item.course_id];
            }

            return result;
        }

        private PreferenceJson BuildPreferenceJson()
        {
            PreferenceJson pref = new PreferenceJson();

            pref.preferred_class_days = GetSelectedPreferredClassDays();
            pref.preferred_day_off = GetSelectedPreferredDayOff();
            pref.preferred_time_slots = GetSelectedPreferredTimeSlots();

            if (checkBoxOtherTimeSlotAvoid.Checked)
            {
                string txt = txtOtherTimeSlotAvoid.Text.Trim();
                if (!string.IsNullOrWhiteSpace(txt))
                {
                    pref.avoid_time_slots = txt
                        .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x) &&
                                    !x.Equals("None", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }
            else
            {
                string selected = comboBoxOtherTimeSlotAvoid.SelectedItem?.ToString() ?? "";
                selected = CleanTextOrNone(selected);

                if (!string.IsNullOrWhiteSpace(selected))
                    pref.avoid_time_slots.Add(selected);
            }

            pref.preferred_course_name = CleanTextOrNone(txtCourseName.Text);
            pref.preferred_professor_name = CleanTextOrNone(txtProfessorName.Text);
            pref.preferred_language = comboBoxLanguage.SelectedItem?.ToString() ?? "No Specific";

            return pref;
        }

        private SchedulerInputJson BuildSchedulerInputJson()
        {
            SchedulerInputJson input = new SchedulerInputJson();

            input.min_credits = 0;
            input.max_credits = GetMaxCreditsFromForm3();

            input.academic_year = comboBoxAcademicYear.SelectedItem?.ToString() ?? "";
            input.no_need_setting = checkBox28.Checked;

            input.step5_courses = BuildStep5Courses();
            input.unpassed_course_ids = GetUnpassedCourseIds();
            input.candidate_courses = BuildCandidateCourses();
            input.preferences = BuildPreferenceJson();

            return input;
        }

        private string ExportJson()
        {
            SchedulerInputJson input = BuildSchedulerInputJson();

            string folder = Path.Combine(Application.StartupPath, "SchedulerData");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filePath = Path.Combine(folder, "input_schedule.json");

            string json = JsonConvert.SerializeObject(input, Formatting.Indented);
            File.WriteAllText(filePath, json);

            return filePath;
        }

        private string GetProjectRootPath()
        {
            return Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\.."));
        }

        private string GetSolverPath()
        {
            return Path.Combine(GetProjectRootPath(), "schedule_solver.py");
        }

        private string GetInputJsonPath()
        {
            return Path.Combine(Application.StartupPath, "SchedulerData", "input_schedule.json");
        }

        private string GetOutputJsonPath()
        {
            return Path.Combine(Application.StartupPath, "SchedulerData", "output_schedule.json");
        }

        private void RunPythonSolver()
        {
            string pythonExe = "python";
            string solverPath = GetSolverPath();
            string inputPath = GetInputJsonPath();
            string outputPath = GetOutputJsonPath();

            if (!File.Exists(solverPath))
                throw new FileNotFoundException("Cannot find schedule_solver.py", solverPath);

            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Cannot find input_schedule.json", inputPath);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = $"\"{solverPath}\" \"{inputPath}\" \"{outputPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = GetProjectRootPath()
            };

            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.Start();

                string stdout = process.StandardOutput.ReadToEnd();
                string stderr = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception("Python solver failed.\n\n" + stderr);

                if (!string.IsNullOrWhiteSpace(stderr))
                    MessageBox.Show("Python message:\n\n" + stderr, "Python Solver");
            }
        }

        private SchedulerOutputJson LoadSolverResult()
        {
            string outputPath = GetOutputJsonPath();

            if (!File.Exists(outputPath))
                throw new FileNotFoundException("Cannot find output_schedule.json", outputPath);

            string json = File.ReadAllText(outputPath);
            SchedulerOutputJson result = JsonConvert.DeserializeObject<SchedulerOutputJson>(json);

            if (result == null)
                throw new Exception("Failed to read output_schedule.json");

            return result;
        }

        private void DisplaySolverResultOnGrid(SchedulerOutputJson result)
        {
            ClearScheduleGridContent();

            var noTimeCourses = new List<Tuple<string, string, string>>();

            foreach (var course in result.selected_courses)
            {
                string displayText =
                    course.course_id + " " + course.class_code + Environment.NewLine +
                    course.english_course_name + Environment.NewLine +
                    "Instructor: " + course.instructor + Environment.NewLine +
                    course.credits + " Credits";

                if (course.time_slots != null && course.time_slots.Count > 0)
                {
                    foreach (var ts in course.time_slots)
                    {
                        string timeSlot = ts.start + "-" + ts.end;
                        SetScheduleCell(ts.day, timeSlot, displayText);
                    }
                }
                else
                {
                    noTimeCourses.Add(Tuple.Create(
                        Convert.ToString(course.course_id),
                        Convert.ToString(course.english_course_name),
                        Convert.ToString(course.credits)
                    ));
                }
            }

            AddNoTimeCoursesInsideMainGrid(noTimeCourses);

            dataGridRecommendSchedule.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

            MessageBox.Show(
                "Status: " + result.status + Environment.NewLine +
                "Total Credits: " + result.total_credits,
                "Schedule Generated",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =========================
        // Grid Helpers
        // =========================
        private void SetScheduleCell(string dayName, string timeSlot, string content)
        {
            int columnIndex = -1;
            int rowIndex = -1;

            foreach (DataGridViewColumn column in dataGridRecommendSchedule.Columns)
            {
                if (column.Name.Equals(dayName, StringComparison.OrdinalIgnoreCase))
                {
                    columnIndex = column.Index;
                    break;
                }
            }

            foreach (DataGridViewRow row in dataGridRecommendSchedule.Rows)
            {
                if (row.Cells[0].Value != null &&
                    row.Cells[0].Value.ToString().Equals(timeSlot, StringComparison.OrdinalIgnoreCase))
                {
                    rowIndex = row.Index;
                    break;
                }
            }

            if (columnIndex >= 0 && rowIndex >= 0)
            {
                string oldValue = dataGridRecommendSchedule.Rows[rowIndex].Cells[columnIndex].Value?.ToString();

                if (string.IsNullOrWhiteSpace(oldValue))
                {
                    dataGridRecommendSchedule.Rows[rowIndex].Cells[columnIndex].Value = content;
                }
                else
                {
                    dataGridRecommendSchedule.Rows[rowIndex].Cells[columnIndex].Value =
                        oldValue + Environment.NewLine +
                        "----------------" + Environment.NewLine +
                        content;
                }

                dataGridRecommendSchedule.Rows[rowIndex].MinimumHeight = 70;
            }
        }

        private void ClearScheduleGridContent()
        {
            while (dataGridRecommendSchedule.Rows.Count > TIMETABLE_ROW_COUNT)
                dataGridRecommendSchedule.Rows.RemoveAt(dataGridRecommendSchedule.Rows.Count - 1);

            for (int row = 0; row < dataGridRecommendSchedule.Rows.Count; row++)
            {
                dataGridRecommendSchedule.Rows[row].DefaultCellStyle.BackColor = Color.White;
                dataGridRecommendSchedule.Rows[row].DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                dataGridRecommendSchedule.Rows[row].MinimumHeight = 55;

                for (int col = 1; col < dataGridRecommendSchedule.Columns.Count; col++)
                    dataGridRecommendSchedule.Rows[row].Cells[col].Value = "";
            }
        }

        private void AddNoTimeCoursesInsideMainGrid(List<Tuple<string, string, string>> courses)
        {
            if (courses == null || courses.Count == 0)
                return;

            int blankRow = dataGridRecommendSchedule.Rows.Add();
            dataGridRecommendSchedule.Rows[blankRow].Height = 10;

            int titleRow = dataGridRecommendSchedule.Rows.Add();
            dataGridRecommendSchedule.Rows[titleRow].Cells[0].Value = "Courses\nWithout Fixed\nTimetable";
            dataGridRecommendSchedule.Rows[titleRow].DefaultCellStyle.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridRecommendSchedule.Rows[titleRow].DefaultCellStyle.BackColor =
                Color.FromArgb(235, 242, 255);
            dataGridRecommendSchedule.Rows[titleRow].Height = 55;

            int headerRow = dataGridRecommendSchedule.Rows.Add();
            dataGridRecommendSchedule.Rows[headerRow].Cells[0].Value = "Course ID";
            dataGridRecommendSchedule.Rows[headerRow].Cells[1].Value = "Course Name";
            dataGridRecommendSchedule.Rows[headerRow].Cells[2].Value = "Credits";

            dataGridRecommendSchedule.Rows[headerRow].DefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridRecommendSchedule.Rows[headerRow].DefaultCellStyle.BackColor =
                Color.Gainsboro;
            dataGridRecommendSchedule.Rows[headerRow].Height = 30;

            foreach (var c in courses)
            {
                int row = dataGridRecommendSchedule.Rows.Add();

                dataGridRecommendSchedule.Rows[row].Cells[0].Value = c.Item1;
                dataGridRecommendSchedule.Rows[row].Cells[1].Value = c.Item2;
                dataGridRecommendSchedule.Rows[row].Cells[2].Value = c.Item3;

                dataGridRecommendSchedule.Rows[row].Height = 32;
            }

            // Hide unused columns after Credits column
            for (int r = titleRow; r < dataGridRecommendSchedule.Rows.Count; r++)
            {
                for (int c = 3; c < dataGridRecommendSchedule.Columns.Count; c++)
                {
                    var cell = dataGridRecommendSchedule.Rows[r].Cells[c];

                    cell.Value = "";
                    cell.Style.BackColor = Color.White;
                    cell.Style.ForeColor = Color.White;
                    cell.Style.SelectionBackColor = Color.White;
                    cell.Style.SelectionForeColor = Color.White;
                }
            }

            dataGridRecommendSchedule.Invalidate();
        }

        private void dataGridRecommendSchedule_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < TIMETABLE_ROW_COUNT)
                return;

            if (e.ColumnIndex >= 3)
            {
                using (Brush brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }

                e.Handled = true;
            }
        }

        // =========================
        // Events
        // =========================
        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            ApplyNoNeedSettingState();
        }

        private void comboBoxAcademicYear_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBoxCreditNumber_SelectedIndexChanged(object sender, EventArgs e) { }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox4_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox3_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox5_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox6_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox7_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox27_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox26_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox25_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox24_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox23_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox22_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox21_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox8_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox9_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox11_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox10_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox12_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox17_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox16_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox15_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox14_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox13_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox20_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox19_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox18_CheckedChanged(object sender, EventArgs e) { }
        private void comboBoxOtherTimeSlotAvoid_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtCourseName_TextChanged(object sender, EventArgs e) { }
        private void txtProfessorName_TextChanged(object sender, EventArgs e) { }
        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e) { }

        private void checkBoxOtherPreferredCredits_CheckedChanged(object sender, EventArgs e)
        {
            txtOtherPreferredCredits.Enabled = checkBoxOtherPreferredCredits.Checked;
            comboBoxCreditNumber.Enabled = !checkBoxOtherPreferredCredits.Checked;
        }

        private void txtOtherPreferredCredits_TextChanged(object sender, EventArgs e) { }

        private void checkBoxOtherTimeSlotAvoid_CheckedChanged(object sender, EventArgs e)
        {
            txtOtherTimeSlotAvoid.Enabled = !checkBox28.Checked && checkBoxOtherTimeSlotAvoid.Checked;
            comboBoxOtherTimeSlotAvoid.Enabled = !checkBox28.Checked && !checkBoxOtherTimeSlotAvoid.Checked;
        }

        private void txtOtherTimeSlotAvoid_TextChanged(object sender, EventArgs e) { }

        private void dataGridRecommendSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllPreferences();
            ClearScheduleGridContent();
        }

        private void btnGenerateSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                ExportJson();
                RunPythonSolver();
                SchedulerOutputJson result = LoadSolverResult();
                _lastSolverResult = result;
                DisplaySolverResultOnGrid(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to generate schedule:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private string GetNextAvailableFileName(
                string folder,
                string baseFileName,
                string extension)
        {
            string filePath = Path.Combine(folder, baseFileName + extension);

            if (!File.Exists(filePath))
                return filePath;

            int counter = 1;

            while (true)
            {
                filePath = Path.Combine(
                    folder,
                    $"{baseFileName}({counter}){extension}");

                if (!File.Exists(filePath))
                    return filePath;

                counter++;
            }
        }

        private void btnSaveScheduleAsImage_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    string defaultFolder =
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.UserProfile)
                        + @"\Downloads";

                    sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                    sfd.Title = "Save Schedule As Image";
                    sfd.InitialDirectory = defaultFolder;

                    sfd.FileName = Path.GetFileName(
                        GetNextAvailableFileName(
                            defaultFolder,
                            "Schedule",
                            ".png"));

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        SaveDataGridViewAsImage(
                            dataGridRecommendSchedule,
                            sfd.FileName);

                        MessageBox.Show(
                            "Schedule saved successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error saving image:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SaveScheduleResultAsImage(string filePath)
        {
            SaveDataGridViewAsImage(dataGridRecommendSchedule, filePath);
        }

        private void SaveDataGridViewAsImage(DataGridView dgv, string filePath)
        {
            dgv.ClearSelection();

            int originalWidth = dgv.Width;
            int originalHeight = dgv.Height;
            ScrollBars originalScrollBars = dgv.ScrollBars;
            DataGridViewAutoSizeRowsMode originalAutoSizeRowsMode = dgv.AutoSizeRowsMode;

            try
            {
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                int totalWidth = 0;
                if (dgv.RowHeadersVisible)
                    totalWidth += dgv.RowHeadersWidth;

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                        totalWidth += col.Width;
                }

                int totalHeight = dgv.ColumnHeadersVisible ? dgv.ColumnHeadersHeight : 0;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Visible)
                        totalHeight += row.Height;
                }

                totalWidth += 2;
                totalHeight += 2;

                dgv.ScrollBars = ScrollBars.None;
                dgv.Width = totalWidth;
                dgv.Height = totalHeight;
                dgv.Refresh();

                using (Bitmap bmp = new Bitmap(totalWidth, totalHeight))
                {
                    dgv.DrawToBitmap(bmp, new Rectangle(0, 0, totalWidth, totalHeight));

                    if (filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                    {
                        bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            finally
            {
                dgv.Width = originalWidth;
                dgv.Height = originalHeight;
                dgv.ScrollBars = originalScrollBars;
                dgv.AutoSizeRowsMode = originalAutoSizeRowsMode;
                dgv.Refresh();
            }
        }



        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnGraduationSummary_Click(object sender, EventArgs e)
        {
            if (_lastSolverResult == null)
            {
                MessageBox.Show(
                    "Please generate a schedule first.",
                    "No Schedule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Form4 form4 = new Form4(
                _form2,
                _lastSolverResult,
                comboBoxAcademicYear.Text
            );

            form4.Show();
        }

        private void comboBoxDept_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewPage3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}*/

using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HAP = HtmlAgilityPack;

namespace ThesisSchedulerGUI
{
    public partial class Form3 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;
        private const int TIMETABLE_ROW_COUNT = 13;

        private Form2 _form2;
        private SchedulerOutputJson _lastSolverResult;

        // =========================================================
        // Step 8 — Add More Courses From Departments
        // Form3 loads department files by itself.
        // =========================================================
        //private readonly string BASE_DIR = @"C:\Users\User\Desktop\ThesisSchedulerGUI\Department Timetable";
        private readonly string BASE_DIR = Path.Combine(Application.StartupPath, "Department Timetable");

        private readonly Dictionary<string, string> DeptToExactFile =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "300 College of Engineering", "College of Engineering(Master Program)" },
            { "302 Department of Mechanical Engineering(Undergraduate Program)", "302 Department of Mechanical Engineering(Undergraduate Program)" },
            { "303 Department of Chemical Engineering and Materials Science(Undergraduate Program)", "303 Department of Chemical Engineering and Materials Science(Undergraduate Program)" },
            { "305 Department of Industrial Engineering and Management(Undergraduate Program)", "305 Department of Industrial Engineering and Management(Undergraduate Program)" },
            { "309 International Bachelor Program in Engineering(Undergraduate Program)", "309 International Bachelor Program in Engineering(Undergraduate Program)" },
            { "320 Post-Baccalaureate Program for Carbon Zero Sustainable Development Preparatory Office(Undergraduate Program)", "320 Post-Baccalaureate Program for Carbon Zero Sustable Develooment Prepatory Office(Undergraduate Progran)" },
            { "322 Department of Mechanical Engineering(Master Program)", "322 Department of Mechanical Engineering(Master Program)" },
            { "323 Department of Chemical Engineering and Materials Science(Master Program)", "323 Department of Chemical Engineering and Materials Science(Master Program)" },
            { "325 Department of Industrial Engineering and Management(Master Program)", "325 Department of Industrial Engineering and Management(Master Program)" },
            { "330 Graduate school of renewable energy engineering(Master Program)", "330 Graduate school of renewable energy engineering(Master Program)" },
            { "340 Master of Science Degree of Smart Manufacturing for Electronics(Master Program)", "340 Master of Science Degree of Smart Manufacturing for Electronics(Master Program)" },
            { "352 Department of Mechanical Engineering(Ph.D. Program)", "352 Department of Mechanical Engineering(Ph.D. Program)" },
            { "353 Department of Chemical Engineering and Materials Science(Ph.D. Program)", "353 Department of Chemical Engineering and Materials Science(Ph.D. Program)" },
            { "355 Department of Industrial Engineering and Management(Ph.D. Program)", "355 Department of Industrial Engineering and Management(Ph.D. Program)" },

            { "500 College of Management", "College of Management(Master Program)" },
            { "505 Bachelor of Business Administration Program, College of Management(Undergraduate Program)", "505 Bachelor of Business Administration Program(Undergraduate Program)" },
            { "530 Master of Business Administration Program, College of Management(Master Program)", "530 Master of Business Administration Program(Master Program)" },
            { "531 Master of Science in Finance and Accounting, College of Management(Master Program)", "531 Master of Science in Finance and Accounting(Master Program)" },
            { "532 Executive Master of Business Administration Program, College of Management(Master Program)", "532 Executive Master of Business Administration Program(Master Program)" },
            { "554 Doctoral Program, College of Management(Ph.D. Program)", "554 Doctoral Program(Ph.D. Program)" },

            { "600 College of Humanities and Social Sciences", "College of Humanities and Social Sciences(Master Program)" },
            { "601 Department of Foreign Languages and Applied Linguistics(Undergraduate Program)", "601 Department of Foreign Languages and Applied Linguistics(Undergraduate Program)" },
            { "602 Department of Chinese Linguistics and Literature(Undergraduate Program)", "602 Department of Chinese Linguistics and Literature(Undergraduate Program)" },
            { "603 Department of Art and Design(Undergraduate Program)", "603 Department of Art and Design(Undergraduate Program)" },
            { "604 Department of Social and Policy Sciences(Undergraduate Program)", "604 Department of Social and Policy Sciences(Undergraduate Program)" },
            { "608 International Bachelor Program in Strategic Communication, College of Humanities and Social Sciences(Undergraduate Program)", "608 International Bachelor Program in Strategic Communication(Undergraduate Program)" },
            { "621 Department of Foreign Languages and Applied Linguistics, Master's Program(Master Program)", "621 Department of Foreign Languages and Applied Linguistics(Master Program)" },
            { "622 Department of Chinese Linguistics and Literature(Master Program)", "622 Department of Chinese Linguistics and Literature(Master Program)" },
            { "623 Master Program in Art and Design Management, Department of Art and Design(Master Program)", "623 Master Program in Art and Design Management(Master Program)" },
            { "624 Department of Social and Policy Sciences(Master Program)", "624 Department of Social and Policy Sciences(Master Program)" },
            { "656 Doctor of Philosophy in Cultural Industries and Cultural Policy(Ph.D. Program)", "656 Doctor of Philosophy in Cultural Industries and Cultural Policy(Ph.D. Program)" },

            { "700 College of Informatics", "College of Informatics(Master Program)" },
            { "304 Department of Computer Science and Engineering(Undergraduate Program)", "304 Department of Computer Science and Engineering(Undergraduate Program)" },
            { "701 Department of Information Management(Undergraduate Program)", "701 Department of Information Management(Undergraduate Program)" },
            { "702 Department of Information Communication(Undergraduate Program)", "702 Department of Information Communication(Undergraduate Program)" },
            { "705 International Bachelor Program in Informatics(Undergraduate Program)", "705 International Bachelor Program in Informatics(Undergraduate Program)" },
            { "721 Department of Information Management(Master Program)", "721 Department of Information Management(Master Program)" },
            { "722 Department of Information Communication(Master Program)", "722 Department of Information Communication(Master Program)" },
            { "723 Graduate School of Social Informatics(Master Program)", "723 Graduate School of Social Informatics(Master Program)" },
            { "724 Department of Computer Science and Engineering(Master Program)", "724 Department of Computer Science and Engineering(Master Program)" },
            { "725 Graduate Program in Biomedical Informatics(Master Program)", "725 Graduate Program in Biomedical Informatics(Master Program)" },
            { "751 Department of Information Management(Ph.D. Program)", "751 Department of Information Management(Ph.D. Program)" },
            { "754 Department of Computer Science and Engineering(Ph.D. Program)", "754 Department of Computer Science and Engineering(Ph.D. Program)" },

            { "800 College of Electrical and Communication Engineering", "College of Electrical and Communication Engineering(Master Program)" },
            { "310 International Bachelor Program in Electrical and Communication Engineering(Undergraduate Program)", "310 International Bachelor Program in Electrical and Communication Engineering(Undergraduate Program)" },
            { "311 Department of Electrical Engineering(Undergraduate Program)", "311 Department of Electrical Engineering(Undergraduate Program)" },
            { "312 Department of Electrical Engineering(Undergraduate Program)", "312 Department of Electrical Engineering(Undergraduate Program)" },
            { "313 Department of Electrical Engineering(Undergraduate Program)", "313 Department of Electrical Engineering(Undergraduate Program)" },
            { "326 Department of Electrical Engineering(Master Program)", "326 Department of Electrical Engineering(Master Program)" },
            { "331 Department of Electrical Engineering(Master Program)", "331 Department of Electrical Engineering(Master Program)" },
            { "332 Department of Electrical Engineering(Master Program)", "332 Department of Electrical Engineering(Master Program)" },
            { "333 Department of Electrical Engineering(Master Program)", "333 Department of Electrical Engineering(Master Program)" },
            { "359 Department of Electrical Engineering(Ph.D. Program)", "359 Department of Electrical Engineering(Ph.D. Program)" },
            { "360 Department of Electrical Engineering(Ph.D. Program)", "360 Department of Electrical Engineering(Ph.D. Program)" },
            { "361 Department of Electrical Engineering(Ph.D. Program)", "361 Department of Electrical Engineering(Ph.D. Program)" },
            { "301 Department of Electrical Engineering(Undergraduate Program)", "301 Department of Electrical Engineering(Undergraduate Program)" },
            { "307 Department of Communications Engineering(Undergraduate Program)", "307 Department of Communications Engineering(Undergraduate Program)" },
            { "308 Department of Photonics Engineering(Undergraduate Program)", "308 Department of Photonics Engineering(Undergraduate Program)" },
            { "327 Department of Communications Engineering(Master Program)", "327 Department of Communications Engineering(Master Program)" },
            { "328 Department of Photonics Engineering(Master Program)", "328 Department of Photonics Engineering(Master Program)" },
            { "356 Department of Electrical Engineering(Ph.D. Program)", "356 Department of Electrical Engineering(Ph.D. Program)" },
            { "357 Department of Communications Engineering(Ph.D. Program)", "357 Department of Communications Engineering(Ph.D. Program)" },
            { "358 Department of Photonics Engineering(Ph.D. Program)", "358 Department of Photonics Engineering(Ph.D. Program)" },

            { "A00 College of Medicine and Nursing", "College of Medicine and Nursing(Master Program)" },
            { "329 Graduate School of Biotechnology and Bioengineering(Master Program)", "Graduate School of Biotechnology and Bioengineering(Master Program)" },
            { "A11 School of Nursing(Undergraduate Program)", "School of Nursing(Undergraduate Program)" },
            { "A21 Graduate Institute of Medicine(Master Program)", "Graduate Institute of Medicine(Master Program)" },

            { "901 General Education Center", "General Education Center(Master Program)" },
            { "903 Military Education Office", "Military Education Office(Master Program)" },
            { "904 Physical Education Office", "Physical Education Office(Master Program)" },
            { "906 International Language and Culture Center", "International Language and Culture Center(Master Program)" },
            { "907 Global Affairs Office", "907 Global Affairs Office" },
            { "908 Moocs", "908 Moocs" },
            { "909 Exploring Interdiscipline", "Exploring Interdiscipline(Master Program)" },
            { "910 Taiwan Artificial Intelligence College Alliance", "Taiwan Artificial Intelligence College Alliance(Master Program)" },
        };

        private readonly string[] TARGET_COLS = new[]
        {
            "NO",
            "Course ID and Class",
            "Department & Year Available",
            "Course Name",
            "Type",
            "Timetable & Classroom",
            "Instructor",
        };

        private DataTable _page3DepartmentTable;
        private readonly HashSet<string> _loadedPage3Depts =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public Form3()
        {
            InitializeComponent();
            InitializeFormData();
            SetPlaceholders();
            InitializeRecommendScheduleGrid();
            InitializeStep8DepartmentGrid();
            ApplyNoNeedSettingState();
            this.AutoScaleMode = AutoScaleMode.None;
        }

        public Form3(Form2 form2)
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            btnChoose.Click -= btnChoose_Click;
            btnChoose.Click += btnChoose_Click;

            btnDelete.Click -= btnDelete_Click;
            btnDelete.Click += btnDelete_Click;

            btnApplySelectedExtraCourses.Click -= btnApplySelectedExtraCourses_Click;
            btnApplySelectedExtraCourses.Click += btnApplySelectedExtraCourses_Click;

            _form2 = form2;

            InitializeFormData();
            SetPlaceholders();
            InitializeRecommendScheduleGrid();
            InitializeStep8DepartmentGrid();
            ApplyNoNeedSettingState();
        }

        // =========================
        // Initialization
        // =========================
        private void InitializeFormData()
        {
            comboBoxAcademicYear.Items.Clear();
            comboBoxAcademicYear.Items.AddRange(new string[]
            {
                "Bachelor 1st Year (Freshman), Semester 1",
                "Bachelor 1st Year (Freshman), Semester 2",
                "Bachelor 2nd Year (Sophomore), Semester 1",
                "Bachelor 2nd Year (Sophomore), Semester 2",
                "Bachelor 3rd Year (Junior), Semester 1",
                "Bachelor 3rd Year (Junior), Semester 2",
                "Bachelor 4th Year (Senior), Semester 1",
                "Bachelor 4th Year (Senior), Semester 2",
                "Master 1st Year, Semester 1",
                "Master 1st Year, Semester 2",
                "Master 2nd Year, Semester 1",
                "Master 2nd Year, Semester 2",
                "PhD 1st Year, Semester 1",
                "PhD 1st Year, Semester 2",
                "PhD 2nd Year, Semester 1",
                "PhD 2nd Year, Semester 2",
                "PhD 3rd Year, Semester 1",
                "PhD 3rd Year, Semester 2",
                "PhD 4th Year, Semester 1",
                "PhD 4th Year, Semester 2"
            });
            comboBoxAcademicYear.SelectedIndex = -1;

            comboBoxCreditNumber.Items.Clear();
            comboBoxCreditNumber.Items.Add("None");
            for (int i = 0; i <= 31; i++)
                comboBoxCreditNumber.Items.Add(i.ToString());
            comboBoxCreditNumber.SelectedIndex = 0;

            comboBoxOtherTimeSlotAvoid.Items.Clear();
            comboBoxOtherTimeSlotAvoid.Items.Add("None");
            comboBoxOtherTimeSlotAvoid.Items.AddRange(new string[]
            {
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
                "20:30-21:20"
            });
            comboBoxOtherTimeSlotAvoid.SelectedIndex = 0;

            comboBoxLanguage.Items.Clear();
            comboBoxLanguage.Items.AddRange(new string[]
            {
                "No Specific",
                "Both",
                "English",
                "Chinese"
            });
            comboBoxLanguage.SelectedIndex = 0;

            txtOtherPreferredCredits.Enabled = false;
            txtOtherTimeSlotAvoid.Enabled = false;
        }

        private void SetPlaceholders()
        {
            SendMessage(txtOtherPreferredCredits.Handle, EM_SETCUEBANNER, 0, "36");
            SendMessage(txtOtherTimeSlotAvoid.Handle, EM_SETCUEBANNER, 0, "08:10-09:00, 09:10-10:00");
            SendMessage(txtCourseName.Handle, EM_SETCUEBANNER, 0, "None, Python, C++");
            SendMessage(txtProfessorName.Handle, EM_SETCUEBANNER, 0, "None, XXX");
        }

        private void InitializeStep8DepartmentGrid()
        {
            comboBoxDept.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBoxDept.BeginUpdate();
            comboBoxDept.Items.Clear();
            comboBoxDept.Items.AddRange(DeptToExactFile.Keys.ToArray());
            comboBoxDept.EndUpdate();

            if (comboBoxDept.Items.Count > 0)
                comboBoxDept.SelectedIndex = 0;

            _page3DepartmentTable = CreateOutputTable();

            dataGridViewPage3.AutoGenerateColumns = true;
            dataGridViewPage3.DataSource = _page3DepartmentTable;

            EnsureSelectionColumn();

            dataGridViewPage3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewPage3.BackgroundColor = Color.White;

            dataGridViewPage3.CellFormatting -= dataGridViewPage3_CellFormatting;
            dataGridViewPage3.CellFormatting += dataGridViewPage3_CellFormatting;

            dataGridViewPage3.CurrentCellDirtyStateChanged -= dataGridViewPage3_CurrentCellDirtyStateChanged;
            dataGridViewPage3.CurrentCellDirtyStateChanged += dataGridViewPage3_CurrentCellDirtyStateChanged;

            dataGridViewPage3.CellValueChanged -= dataGridViewPage3_CellValueChanged;
            dataGridViewPage3.CellValueChanged += dataGridViewPage3_CellValueChanged;
        }

        private void InitializeRecommendScheduleGrid()
        {
            dataGridRecommendSchedule.Rows.Clear();
            dataGridRecommendSchedule.Columns.Clear();

            dataGridRecommendSchedule.AllowUserToAddRows = false;
            dataGridRecommendSchedule.AllowUserToDeleteRows = false;
            dataGridRecommendSchedule.AllowUserToResizeRows = false;
            dataGridRecommendSchedule.AllowUserToResizeColumns = false;
            dataGridRecommendSchedule.AllowUserToOrderColumns = false;
            dataGridRecommendSchedule.ReadOnly = true;
            dataGridRecommendSchedule.RowHeadersVisible = false;
            dataGridRecommendSchedule.MultiSelect = false;
            dataGridRecommendSchedule.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridRecommendSchedule.ScrollBars = ScrollBars.Both;
            dataGridRecommendSchedule.BackgroundColor = Color.White;
            dataGridRecommendSchedule.BorderStyle = BorderStyle.FixedSingle;
            dataGridRecommendSchedule.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridRecommendSchedule.EnableHeadersVisualStyles = false;

            dataGridRecommendSchedule.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridRecommendSchedule.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridRecommendSchedule.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dataGridRecommendSchedule.DefaultCellStyle.Padding = new Padding(4);

            dataGridRecommendSchedule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridRecommendSchedule.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridRecommendSchedule.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridRecommendSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridRecommendSchedule.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridRecommendSchedule.Columns.Add("TimeSection", "Time / Day");
            dataGridRecommendSchedule.Columns.Add("Monday", "Monday");
            dataGridRecommendSchedule.Columns.Add("Tuesday", "Tuesday");
            dataGridRecommendSchedule.Columns.Add("Wednesday", "Wednesday");
            dataGridRecommendSchedule.Columns.Add("Thursday", "Thursday");
            dataGridRecommendSchedule.Columns.Add("Friday", "Friday");
            dataGridRecommendSchedule.Columns.Add("Saturday", "Saturday");
            dataGridRecommendSchedule.Columns.Add("Sunday", "Sunday");

            dataGridRecommendSchedule.Columns["TimeSection"].Width = 120;
            dataGridRecommendSchedule.Columns["Monday"].Width = 180;
            dataGridRecommendSchedule.Columns["Tuesday"].Width = 180;
            dataGridRecommendSchedule.Columns["Wednesday"].Width = 180;
            dataGridRecommendSchedule.Columns["Thursday"].Width = 180;
            dataGridRecommendSchedule.Columns["Friday"].Width = 180;
            dataGridRecommendSchedule.Columns["Saturday"].Width = 180;
            dataGridRecommendSchedule.Columns["Sunday"].Width = 180;

            string[] timeSlots =
            {
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
                "20:30-21:20"
            };

            dataGridRecommendSchedule.CellPainting -= dataGridRecommendSchedule_CellPainting;
            dataGridRecommendSchedule.CellPainting += dataGridRecommendSchedule_CellPainting;

            foreach (string slot in timeSlots)
            {
                int rowIndex = dataGridRecommendSchedule.Rows.Add();
                dataGridRecommendSchedule.Rows[rowIndex].Cells[0].Value = slot;
                dataGridRecommendSchedule.Rows[rowIndex].MinimumHeight = 55;
            }

            dataGridRecommendSchedule.SelectionChanged += (s, e) =>
            {
                dataGridRecommendSchedule.ClearSelection();
            };

            dataGridRecommendSchedule.ColumnHeaderMouseClick += (s, e) =>
            {
                dataGridRecommendSchedule.ClearSelection();
            };

            foreach (DataGridViewColumn col in dataGridRecommendSchedule.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.Resizable = DataGridViewTriState.False;
            }
        }

        private void ApplyNoNeedSettingState()
        {
            bool enableControls = !checkBox28.Checked;
            SetPreferenceControlsEnabled(enableControls);
        }

        private void SetPreferenceControlsEnabled(bool enabled)
        {
            comboBoxAcademicYear.Enabled = true;
            checkBoxOtherPreferredCredits.Enabled = true;
            comboBoxCreditNumber.Enabled = !checkBoxOtherPreferredCredits.Checked;
            txtOtherPreferredCredits.Enabled = checkBoxOtherPreferredCredits.Checked;

            comboBoxOtherTimeSlotAvoid.Enabled = enabled && !checkBoxOtherTimeSlotAvoid.Checked;
            checkBoxOtherTimeSlotAvoid.Enabled = enabled;
            txtOtherTimeSlotAvoid.Enabled = enabled && checkBoxOtherTimeSlotAvoid.Checked;

            comboBoxLanguage.Enabled = enabled;
            txtCourseName.Enabled = enabled;
            txtProfessorName.Enabled = enabled;

            checkBox1.Enabled = enabled;
            checkBox2.Enabled = enabled;
            checkBox4.Enabled = enabled;
            checkBox3.Enabled = enabled;
            checkBox5.Enabled = enabled;
            checkBox6.Enabled = enabled;
            checkBox7.Enabled = enabled;

            checkBox27.Enabled = enabled;
            checkBox26.Enabled = enabled;
            checkBox25.Enabled = enabled;
            checkBox24.Enabled = enabled;
            checkBox23.Enabled = enabled;
            checkBox22.Enabled = enabled;
            checkBox21.Enabled = enabled;

            checkBox8.Enabled = enabled;
            checkBox9.Enabled = enabled;
            checkBox11.Enabled = enabled;
            checkBox10.Enabled = enabled;
            checkBox12.Enabled = enabled;
            checkBox17.Enabled = enabled;
            checkBox16.Enabled = enabled;
            checkBox15.Enabled = enabled;
            checkBox14.Enabled = enabled;
            checkBox13.Enabled = enabled;
            checkBox20.Enabled = enabled;
            checkBox19.Enabled = enabled;
            checkBox18.Enabled = enabled;
        }

        private void ClearAllPreferences()
        {
            comboBoxAcademicYear.SelectedIndex = -1;
            comboBoxCreditNumber.SelectedIndex = 0;
            comboBoxOtherTimeSlotAvoid.SelectedIndex = 0;
            comboBoxLanguage.SelectedIndex = 0;

            txtOtherPreferredCredits.Clear();
            txtOtherTimeSlotAvoid.Clear();
            txtCourseName.Clear();
            txtProfessorName.Clear();

            checkBoxOtherPreferredCredits.Checked = false;
            checkBoxOtherTimeSlotAvoid.Checked = false;
            checkBox28.Checked = false;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
            checkBox3.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;

            checkBox27.Checked = false;
            checkBox26.Checked = false;
            checkBox25.Checked = false;
            checkBox24.Checked = false;
            checkBox23.Checked = false;
            checkBox22.Checked = false;
            checkBox21.Checked = false;

            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox11.Checked = false;
            checkBox10.Checked = false;
            checkBox12.Checked = false;
            checkBox17.Checked = false;
            checkBox16.Checked = false;
            checkBox15.Checked = false;
            checkBox14.Checked = false;
            checkBox13.Checked = false;
            checkBox20.Checked = false;
            checkBox19.Checked = false;
            checkBox18.Checked = false;

            ApplyNoNeedSettingState();
        }

        // =========================
        // Step 8 Department Loading Logic
        // =========================
        private DataTable CreateOutputTable()
        {
            DataTable t = new DataTable();

            t.Columns.Add("NO", typeof(int));
            t.Columns.Add("Course ID");
            t.Columns.Add("Class");
            t.Columns.Add("English Course Name");
            t.Columns.Add("Timetable");
            t.Columns.Add("Instructor");
            t.Columns.Add("Year Available");
            t.Columns.Add("Chinese Course Name");
            t.Columns.Add("Teaching Language");
            t.Columns.Add("Type");
            t.Columns.Add("Classroom");
            t.Columns.Add("Department");

            return t;
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (comboBoxDept.SelectedItem == null)
            {
                MessageBox.Show("Please choose a department first.");
                return;
            }

            string deptText = comboBoxDept.SelectedItem.ToString();

            if (_loadedPage3Depts.Contains(deptText))
            {
                MessageBox.Show($"⚠ {deptText} is already loaded.\nIf you want to reload, delete it first.");
                return;
            }

            string filePath = FindDepartmentFileAuto(BASE_DIR, deptText);

            if (filePath == null)
            {
                MessageBox.Show(
                    "File not found for:\n" + deptText +
                    "\n\nSearched in:\n" + BASE_DIR +
                    "\n\nTip: Check the filename in Department Timetable folder.");
                return;
            }

            try
            {
                string html = ReadHtmlOrMhtml(filePath);
                DataTable raw = ParseCourseTableFromHtml(html);
                DataTable processed = PreprocessStep6(raw);

                AppendToPage3DepartmentTable(processed);
                ReNumberNO(_page3DepartmentTable);
                _loadedPage3Depts.Add(deptText);

                dataGridViewPage3.DataSource = null;
                dataGridViewPage3.DataSource = _page3DepartmentTable;

                EnsureSelectionColumn();

                dataGridViewPage3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load department:\n" + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (comboBoxDept.SelectedItem == null)
            {
                MessageBox.Show("Please choose a department first.");
                return;
            }

            string deptText = comboBoxDept.SelectedItem.ToString();

            if (!_loadedPage3Depts.Contains(deptText))
            {
                MessageBox.Show($"⚠ {deptText} is not loaded yet.");
                return;
            }

            if (_page3DepartmentTable == null || _page3DepartmentTable.Rows.Count == 0)
            {
                MessageBox.Show("No data to delete.");
                return;
            }

            string deptKey = NormalizeDeptKey(deptText);

            int removed = 0;
            for (int i = _page3DepartmentTable.Rows.Count - 1; i >= 0; i--)
            {
                string depCell = _page3DepartmentTable.Rows[i]["Department"]?.ToString() ?? "";
                string depKeyInTable = NormalizeDeptKey(depCell);

                if (depKeyInTable.Equals(deptKey, StringComparison.OrdinalIgnoreCase) ||
                    depKeyInTable.IndexOf(deptKey, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    deptKey.IndexOf(depKeyInTable, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _page3DepartmentTable.Rows.RemoveAt(i);
                    removed++;
                }
            }

            _loadedPage3Depts.Remove(deptText);
            ReNumberNO(_page3DepartmentTable);

            dataGridViewPage3.DataSource = null;
            dataGridViewPage3.DataSource = _page3DepartmentTable;

            EnsureSelectionColumn();

            dataGridViewPage3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            MessageBox.Show($"Deleted {removed} rows for:\n{deptText}");
        }

        private void AppendToPage3DepartmentTable(DataTable add)
        {
            if (_page3DepartmentTable == null)
                _page3DepartmentTable = CreateOutputTable();

            foreach (DataRow r in add.Rows)
            {
                DataRow nr = _page3DepartmentTable.NewRow();

                foreach (DataColumn col in _page3DepartmentTable.Columns)
                {
                    nr[col.ColumnName] = r.Table.Columns.Contains(col.ColumnName)
                        ? (r[col.ColumnName]?.ToString() ?? "")
                        : "";
                }

                _page3DepartmentTable.Rows.Add(nr);
            }
        }

        private static void ReNumberNO(DataTable dt)
        {
            if (dt == null) return;

            if (!dt.Columns.Contains("NO"))
                dt.Columns.Add("NO", typeof(int));

            int n = 1;
            foreach (DataRow r in dt.Rows)
                r["NO"] = n++;
        }

        private string FindDepartmentFileAuto(string baseDir, string deptText)
        {
            if (!Directory.Exists(baseDir)) return null;

            HashSet<string> exts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".mhtml", ".mht", ".html", ".htm"
            };

            List<string> files = Directory.EnumerateFiles(baseDir, "*.*", SearchOption.TopDirectoryOnly)
                .Where(p => exts.Contains(Path.GetExtension(p)))
                .ToList();

            if (files.Count == 0) return null;

            if (DeptToExactFile.TryGetValue(deptText, out string exactBaseName))
            {
                string keyExact = NormalizeKey(exactBaseName);

                string exact = files.FirstOrDefault(f =>
                    NormalizeKey(Path.GetFileNameWithoutExtension(f)) == keyExact);

                if (exact != null) return exact;
            }

            string key = NormalizeKey(deptText);
            string exact2 = files.FirstOrDefault(f =>
                NormalizeKey(Path.GetFileNameWithoutExtension(f)) == key);

            if (exact2 != null) return exact2;

            foreach (string f in files)
            {
                string n = NormalizeKey(Path.GetFileNameWithoutExtension(f));
                if (n.Contains(key) || key.Contains(n)) return f;
            }

            string code = ExtractLeadingCode(deptText);
            if (!string.IsNullOrEmpty(code))
            {
                string byCode = files.FirstOrDefault(f =>
                    Path.GetFileNameWithoutExtension(f).TrimStart()
                        .StartsWith(code, StringComparison.OrdinalIgnoreCase));

                if (byCode != null) return byCode;
            }

            return null;
        }

        private static string ExtractLeadingCode(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";

            Match m = Regex.Match(s.Trim(), @"^([A-Za-z0-9]{3})\b");
            return m.Success ? m.Groups[1].Value : "";
        }

        private static string NormalizeKey(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";

            s = s.Trim().ToLowerInvariant();
            s = Regex.Replace(s, @"[\s\-_]+", "");
            s = s.Replace("(", "").Replace(")", "");
            s = s.Replace(",", "").Replace(".", "");

            return s;
        }

        private static string NormalizeDeptKey(string deptText)
        {
            if (string.IsNullOrWhiteSpace(deptText)) return "";

            string s = Regex.Replace(deptText.Trim(), @"^[A-Za-z0-9]{3}\s+", "");
            s = Regex.Replace(s, @"\s+", " ");
            s = s.Replace(" (", "(")
                 .Replace("( ", "(")
                 .Replace(" )", ")")
                 .Replace(") ", ")");

            return s.Trim();
        }

        private static string ReadHtmlOrMhtml(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();

            if (ext == ".html" || ext == ".htm")
                return File.ReadAllText(path, Encoding.UTF8);

            return ExtractMainHtmlFromMhtml(path);
        }

        private static string ExtractMainHtmlFromMhtml(string mhtmlPath)
        {
            using (FileStream stream = File.OpenRead(mhtmlPath))
            {
                MimeMessage msg = MimeMessage.Load(stream);

                List<string> htmlParts = msg.BodyParts
                    .OfType<TextPart>()
                    .Where(p => p.IsHtml)
                    .Select(p => p.Text ?? "")
                    .OrderByDescending(s => s.Length)
                    .ToList();

                if (htmlParts.Count == 0)
                    throw new Exception("No HTML found inside this MHTML.");

                return htmlParts[0];
            }
        }

        private DataTable ParseCourseTableFromHtml(string html)
        {
            HAP.HtmlDocument doc = new HAP.HtmlDocument();
            doc.LoadHtml(html);

            HAP.HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table");
            if (tables == null || tables.Count == 0)
                throw new Exception("No <table> found in HTML.");

            foreach (HAP.HtmlNode table in tables)
            {
                DataTable dt = TryParseCourseTable_PythonStyle(table);
                if (dt != null) return dt;
            }

            throw new Exception("Cannot find the course list table with the expected 7 columns.");
        }

        private static string CleanCellNode(HAP.HtmlNode cell)
        {
            if (cell == null) return "";

            HAP.HtmlNode clone = cell.Clone();

            HAP.HtmlNodeCollection brs = clone.SelectNodes(".//br");
            if (brs != null)
            {
                foreach (HAP.HtmlNode br in brs)
                    br.ParentNode.ReplaceChild(clone.OwnerDocument.CreateTextNode("\n"), br);
            }

            string s = clone.InnerText ?? "";
            s = HAP.HtmlEntity.DeEntitize(s);
            s = s.Replace("\u00A0", " ");
            s = s.Replace("\r", "");
            s = Regex.Replace(s, @"[ \t]+", " ");
            s = Regex.Replace(s, @"\n+", "\n").Trim();

            return s;
        }

        private DataTable TryParseCourseTable_PythonStyle(HAP.HtmlNode tableNode)
        {
            HAP.HtmlNodeCollection rows = tableNode.SelectNodes(".//tr");
            if (rows == null || rows.Count < 2) return null;

            List<List<string>> grid = new List<List<string>>();

            foreach (HAP.HtmlNode tr in rows)
            {
                HAP.HtmlNodeCollection cells = tr.SelectNodes("./th|./td");
                if (cells == null) continue;

                List<string> row = cells.Select(CleanCellNode).ToList();
                if (row.Count > 0) grid.Add(row);
            }

            if (grid.Count < 2) return null;

            bool HeaderOk(List<string> header)
            {
                HashSet<string> set = new HashSet<string>(
                    header.Select(x => (x ?? "").Trim()),
                    StringComparer.OrdinalIgnoreCase);

                return TARGET_COLS.All(c => set.Contains(c));
            }

            if (HeaderOk(grid[0]))
                return BuildDataTableFromGrid(grid, 0);

            if (grid.Count >= 3 && HeaderOk(grid[1]))
                return BuildDataTableFromGrid(grid, 1);

            return null;
        }

        private DataTable BuildDataTableFromGrid(List<List<string>> grid, int headerRowIndex)
        {
            List<string> header = grid[headerRowIndex];

            DataTable dt = new DataTable();

            foreach (string hRaw in header)
            {
                string h = (hRaw ?? "").Trim();
                if (string.IsNullOrEmpty(h)) h = "Column";

                string col = h;
                int k = 1;

                while (dt.Columns.Contains(col))
                    col = h + "_" + (k++);

                dt.Columns.Add(col);
            }

            for (int r = headerRowIndex + 1; r < grid.Count; r++)
            {
                List<string> row = grid[r];
                List<string> values = row.Select(x => x ?? "").ToList();

                while (values.Count < dt.Columns.Count) values.Add("");
                if (values.Count > dt.Columns.Count) values = values.Take(dt.Columns.Count).ToList();

                if (values.All(v => string.IsNullOrWhiteSpace(v))) continue;

                dt.Rows.Add(values.ToArray());
            }

            return dt.Rows.Count == 0 ? null : dt;
        }

        private DataTable PreprocessStep6(DataTable raw)
        {
            DataTable output = CreateOutputTable();

            foreach (DataRow r in raw.Rows)
            {
                string joined = string.Join(" | ",
                    TARGET_COLS.Select(c => r.Table.Columns.Contains(c) ? (r[c] ?? "").ToString() : ""));

                if (joined.IndexOf("Selection message", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                string courseName = r.Table.Columns.Contains("Course Name") ? (r["Course Name"] ?? "").ToString() : "";
                SplitCourseNameAndLanguage(courseName, out string zh, out string en, out string teachLang);

                string tt = r.Table.Columns.Contains("Timetable & Classroom") ? (r["Timetable & Classroom"] ?? "").ToString() : "";
                SplitTimetableAndRoomStrict(tt, out string timetable, out string classroom);

                string depYear = r.Table.Columns.Contains("Department & Year Available")
                    ? (r["Department & Year Available"]?.ToString() ?? "")
                    : "";
                SplitDepartmentYear(depYear, out string department, out string year);

                string cidClass = r.Table.Columns.Contains("Course ID and Class")
                    ? (r["Course ID and Class"]?.ToString() ?? "")
                    : "";
                SplitCourseIdAndClass(cidClass, out string courseId, out string classCode);

                DataRow row = output.NewRow();

                row["NO"] = r.Table.Columns.Contains("NO") ? (r["NO"]?.ToString() ?? "") : "";
                row["Course ID"] = courseId;
                row["Class"] = classCode;
                row["Department"] = department;
                row["Year Available"] = year;
                row["Chinese Course Name"] = zh;
                row["English Course Name"] = en;
                row["Teaching Language"] = teachLang;
                row["Type"] = r.Table.Columns.Contains("Type") ? ((r["Type"]?.ToString() ?? "").Trim()) : "";
                row["Timetable"] = timetable;
                row["Classroom"] = classroom;
                row["Instructor"] = r.Table.Columns.Contains("Instructor") ? (r["Instructor"]?.ToString() ?? "") : "";

                output.Rows.Add(row);
            }

            return output;
        }

        private static void SplitDepartmentYear(string depYear, out string department, out string year)
        {
            department = (depYear ?? "").Trim();
            year = "";

            if (string.IsNullOrWhiteSpace(depYear))
                return;

            int comma = depYear.IndexOf(',');
            if (comma >= 0)
            {
                department = depYear.Substring(0, comma).Trim();
                year = depYear.Substring(comma + 1).Trim();
                return;
            }

            int dot = depYear.IndexOf('.');
            if (dot >= 0)
            {
                department = depYear.Substring(0, dot).Trim();
                year = depYear.Substring(dot + 1).Trim();
                return;
            }

            department = depYear.Trim();
        }

        private static void SplitCourseIdAndClass(string cidClass, out string courseId, out string classCode)
        {
            courseId = "";
            classCode = "";

            string s = (cidClass ?? "").Trim();
            if (string.IsNullOrWhiteSpace(s)) return;

            Match m = Regex.Match(s, @"^\s*([A-Za-z0-9]+)\s+([A-Za-z0-9]+)\s*$");
            if (m.Success)
            {
                courseId = m.Groups[1].Value.Trim();
                classCode = m.Groups[2].Value.Trim();
                return;
            }

            courseId = s;
            classCode = "";
        }

        private static void SplitCourseNameAndLanguage(string input, out string zh, out string en, out string teachLang)
        {
            string s = (input ?? "").Trim();
            s = Regex.Replace(s, @"\s+", " ");

            teachLang = Regex.IsMatch(s, @"\*Teaching in English\b", RegexOptions.IgnoreCase)
                ? "English"
                : "Chinese";

            string sNoTags = Regex.Replace(s, @"\s*\*[^*]+", "").Trim();

            if (sNoTags.Contains("(Syllabus)"))
            {
                string[] parts = sNoTags.Split(new[] { "(Syllabus)" }, 2, StringSplitOptions.None);
                zh = parts[0].Trim();
                en = parts.Length > 1 ? parts[1].Trim() : "";
            }
            else
            {
                Match m = Regex.Match(sNoTags, @"[A-Za-z]");
                if (m.Success)
                {
                    zh = sNoTags.Substring(0, m.Index).Trim();
                    en = sNoTags.Substring(m.Index).Trim();
                }
                else
                {
                    zh = sNoTags.Trim();
                    en = "";
                }
            }

            zh = zh.Replace("(Syllabus)", "").Trim();
            en = en.Replace("(Syllabus)", "").Trim();
        }

        private static void SplitTimetableAndRoomStrict(string input, out string timetable, out string classroom)
        {
            string text = input ?? "";
            text = HAP.HtmlEntity.DeEntitize(text);
            text = text.Replace("\u00A0", " ");
            text = Regex.Replace(text, @"\s+", " ").Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                timetable = "";
                classroom = "";
                return;
            }

            List<string> timeMatches = Regex.Matches(text, @"(?<!\d)\d{3}(?!\d)")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            HashSet<string> seenT = new HashSet<string>();
            List<string> orderedT = new List<string>();

            foreach (string t in timeMatches)
            {
                if (seenT.Add(t))
                    orderedT.Add(t);
            }

            List<string> roomMatches = Regex.Matches(text, @"(?<!\d)\d{4}(?!\d)")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            HashSet<string> seenR = new HashSet<string>();
            List<string> orderedR = new List<string>();

            foreach (string r in roomMatches)
            {
                if (seenR.Add(r))
                    orderedR.Add(r);
            }

            timetable = string.Join(" ", orderedT);
            classroom = string.Join("; ", orderedR);
        }

        private void dataGridViewPage3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!dataGridViewPage3.Columns.Contains("Teaching Language")) return;
            if (dataGridViewPage3.Columns[e.ColumnIndex].Name != "Teaching Language") return;

            string text = (e.Value ?? "").ToString()
                .Replace('\u00A0', ' ')
                .Trim();

            if (text.Equals("English", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.SelectionForeColor = Color.Red;
            }
        }

        // =========================
        // Helper Methods
        // =========================
        private List<string> GetSelectedPreferredClassDays()
        {
            List<string> days = new List<string>();

            if (checkBox1.Checked) days.Add("Monday");
            if (checkBox2.Checked) days.Add("Tuesday");
            if (checkBox4.Checked) days.Add("Wednesday");
            if (checkBox3.Checked) days.Add("Thursday");
            if (checkBox5.Checked) days.Add("Friday");
            if (checkBox6.Checked) days.Add("Saturday");
            if (checkBox7.Checked) days.Add("Sunday");

            return days;
        }

        private List<string> GetSelectedPreferredDayOff()
        {
            List<string> days = new List<string>();

            if (checkBox27.Checked) days.Add("Monday");
            if (checkBox26.Checked) days.Add("Tuesday");
            if (checkBox25.Checked) days.Add("Wednesday");
            if (checkBox24.Checked) days.Add("Thursday");
            if (checkBox23.Checked) days.Add("Friday");
            if (checkBox22.Checked) days.Add("Saturday");
            if (checkBox21.Checked) days.Add("Sunday");

            return days;
        }

        private List<string> GetSelectedPreferredTimeSlots()
        {
            List<string> slots = new List<string>();

            if (checkBox8.Checked) slots.Add("08:10-09:00");
            if (checkBox9.Checked) slots.Add("09:10-10:00");
            if (checkBox11.Checked) slots.Add("10:10-11:00");
            if (checkBox10.Checked) slots.Add("11:10-12:00");
            if (checkBox12.Checked) slots.Add("12:10-13:00");
            if (checkBox17.Checked) slots.Add("13:10-14:00");
            if (checkBox16.Checked) slots.Add("14:10-15:00");
            if (checkBox15.Checked) slots.Add("15:10-16:00");
            if (checkBox14.Checked) slots.Add("16:10-17:00");
            if (checkBox13.Checked) slots.Add("17:10-18:00");
            if (checkBox20.Checked) slots.Add("18:30-19:20");
            if (checkBox19.Checked) slots.Add("19:30-20:20");
            if (checkBox18.Checked) slots.Add("20:30-21:20");

            return slots;
        }

        private string NormalizeCourseId(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";

            s = s.Trim().ToUpperInvariant();
            s = s.Replace("\u00A0", " ").Replace("\u200B", "");
            s = Regex.Replace(s, @"\s+", "");
            s = Regex.Replace(s, @"^([A-Z]{2}\d{3}).*$", "$1");

            return s;
        }

        private int ParseIntSafe(string s)
        {
            if (int.TryParse((s ?? "").Trim(), out int value))
                return value;

            return 0;
        }

        private string CleanTextOrNone(string s)
        {
            s = (s ?? "").Trim();
            if (string.IsNullOrWhiteSpace(s)) return "";
            if (s.Equals("None", StringComparison.OrdinalIgnoreCase)) return "";
            return s;
        }

        private int GetMaxCreditsFromForm3()
        {
            string value = checkBoxOtherPreferredCredits.Checked
                ? txtOtherPreferredCredits.Text.Trim()
                : (comboBoxCreditNumber.SelectedItem?.ToString() ?? "");

            if (string.IsNullOrWhiteSpace(value) ||
                value.Equals("None", StringComparison.OrdinalIgnoreCase))
                return 0;

            if (int.TryParse(value, out int credits))
                return credits;

            return 0;
        }

        private List<Step5CourseJson> BuildStep5Courses()
        {
            List<Step5CourseJson> result = new List<Step5CourseJson>();

            if (_form2 == null) return result;

            DataTable dt = _form2.GetStep5Table();
            if (dt == null) return result;

            foreach (DataRow row in dt.Rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");
                if (string.IsNullOrWhiteSpace(courseId))
                    continue;

                result.Add(new Step5CourseJson
                {
                    course_id = courseId,
                    credits = row["Credits"]?.ToString()?.Trim() ?? "",
                    english_name_course = row["English Name Course"]?.ToString()?.Trim() ?? "",
                    category = row["Category"]?.ToString()?.Trim() ?? "",
                    passed = (row["Check passed"]?.ToString()?.Trim() ?? "") == "✓"
                });
            }

            return result;
        }

        private List<string> GetUnpassedCourseIds()
        {
            List<string> result = new List<string>();

            if (_form2 == null) return result;

            DataTable dt = _form2.GetStep5Table();
            if (dt == null) return result;

            foreach (DataRow row in dt.Rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");
                string checkPassed = row["Check passed"]?.ToString()?.Trim() ?? "";

                if (!string.IsNullOrWhiteSpace(courseId) && checkPassed != "✓")
                    result.Add(courseId);
            }

            return result
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private List<DataRow> GetMatchedStep6Rows()
        {
            List<DataRow> result = new List<DataRow>();

            HashSet<string> unpassed = new HashSet<string>(
                GetUnpassedCourseIds(),
                StringComparer.OrdinalIgnoreCase);

            if (_form2 != null)
            {
                DataTable step6 = _form2.GetStep6Table();

                if (step6 != null)
                {
                    foreach (DataRow row in step6.Rows)
                    {
                        string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");

                        if (unpassed.Contains(courseId))
                            result.Add(row);
                    }
                }
            }

            if (_page3DepartmentTable != null)
            {
                foreach (DataRow row in _page3DepartmentTable.Rows)
                {
                    string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");

                    if (unpassed.Contains(courseId))
                        result.Add(row);
                }
            }

            return result;
        }

        private List<TimeSlotJson> ParseTimeSlotsFromTimetable(string timetableText)
        {
            List<TimeSlotJson> result = new List<TimeSlotJson>();

            if (string.IsNullOrWhiteSpace(timetableText))
                return result;

            string[] codes = timetableText
                .Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string code in codes)
            {
                string c = code.Trim();

                if (!Regex.IsMatch(c, @"^\d{3}$"))
                    continue;

                string day = MapDayCode(c.Substring(0, 1));
                string timeSlot = MapPeriodCode(c.Substring(1, 2));

                if (string.IsNullOrWhiteSpace(day) || string.IsNullOrWhiteSpace(timeSlot))
                    continue;

                string[] parts = timeSlot.Split('-');
                if (parts.Length != 2)
                    continue;

                result.Add(new TimeSlotJson
                {
                    day = day,
                    start = parts[0],
                    end = parts[1],
                    slot_key = $"{day}|{timeSlot}"
                });
            }

            return result;
        }

        private string MapDayCode(string dayCode)
        {
            switch (dayCode)
            {
                case "1": return "Monday";
                case "2": return "Tuesday";
                case "3": return "Wednesday";
                case "4": return "Thursday";
                case "5": return "Friday";
                case "6": return "Saturday";
                case "7": return "Sunday";
                default: return "";
            }
        }

        private string MapPeriodCode(string periodCode)
        {
            switch (periodCode)
            {
                case "01": return "08:10-09:00";
                case "02": return "09:10-10:00";
                case "03": return "10:10-11:00";
                case "04": return "11:10-12:00";
                case "05": return "12:10-13:00";
                case "06": return "13:10-14:00";
                case "07": return "14:10-15:00";
                case "08": return "15:10-16:00";
                case "09": return "16:10-17:00";
                case "10": return "17:10-18:00";
                case "11": return "18:30-19:20";
                case "12": return "19:30-20:20";
                case "13": return "20:30-21:20";
                default: return "";
            }
        }

        private Dictionary<string, int> BuildStep5CreditMap()
        {
            Dictionary<string, int> result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            if (_form2 == null) return result;

            DataTable dt = _form2.GetStep5Table();
            if (dt == null) return result;

            foreach (DataRow row in dt.Rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");
                int credits = ParseIntSafe(row["Credits"]?.ToString() ?? "");

                if (!string.IsNullOrWhiteSpace(courseId) && !result.ContainsKey(courseId))
                    result[courseId] = credits;
            }

            return result;
        }

        private List<CandidateCourseJson> BuildCandidateCourses()
        {
            List<CandidateCourseJson> result = new List<CandidateCourseJson>();

            List<DataRow> rows = GetMatchedStep6Rows();

            int no = 1;
            foreach (DataRow row in rows)
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");

                CandidateCourseJson item = new CandidateCourseJson
                {
                    row_no = no++,
                    course_id = courseId,
                    class_code = row["Class"]?.ToString()?.Trim() ?? "",
                    department = row["Department"]?.ToString()?.Trim() ?? "",
                    year_available = row["Year Available"]?.ToString()?.Trim() ?? "",
                    chinese_course_name = row["Chinese Course Name"]?.ToString()?.Trim() ?? "",
                    english_course_name = row["English Course Name"]?.ToString()?.Trim() ?? "",
                    teaching_language = row["Teaching Language"]?.ToString()?.Trim() ?? "",
                    type = row["Type"]?.ToString()?.Trim() ?? "",
                    timetable_raw = row["Timetable"]?.ToString()?.Trim() ?? "",
                    classroom = row["Classroom"]?.ToString()?.Trim() ?? "",
                    instructor = row["Instructor"]?.ToString()?.Trim() ?? "",
                    credits = 0,
                    time_slots = ParseTimeSlotsFromTimetable(row["Timetable"]?.ToString()?.Trim() ?? ""),
                    prerequisites = new List<string>()
                };

                result.Add(item);
            }

            Dictionary<string, int> step5CreditMap = BuildStep5CreditMap();

            foreach (CandidateCourseJson item in result)
            {
                if (step5CreditMap.ContainsKey(item.course_id))
                    item.credits = step5CreditMap[item.course_id];
            }

            return result;
        }

        private PreferenceJson BuildPreferenceJson()
        {
            PreferenceJson pref = new PreferenceJson();

            pref.preferred_class_days = GetSelectedPreferredClassDays();
            pref.preferred_day_off = GetSelectedPreferredDayOff();
            pref.preferred_time_slots = GetSelectedPreferredTimeSlots();

            if (checkBoxOtherTimeSlotAvoid.Checked)
            {
                string txt = txtOtherTimeSlotAvoid.Text.Trim();
                if (!string.IsNullOrWhiteSpace(txt))
                {
                    pref.avoid_time_slots = txt
                        .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x) &&
                                    !x.Equals("None", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }
            else
            {
                string selected = comboBoxOtherTimeSlotAvoid.SelectedItem?.ToString() ?? "";
                selected = CleanTextOrNone(selected);

                if (!string.IsNullOrWhiteSpace(selected))
                    pref.avoid_time_slots.Add(selected);
            }

            pref.preferred_course_name = CleanTextOrNone(txtCourseName.Text);
            pref.preferred_professor_name = CleanTextOrNone(txtProfessorName.Text);
            pref.preferred_language = comboBoxLanguage.SelectedItem?.ToString() ?? "No Specific";

            return pref;
        }


        private SchedulerInputJson BuildSchedulerInputJson()
        {
            SchedulerInputJson input = new SchedulerInputJson();

            input.min_credits = 0;
            input.academic_year = comboBoxAcademicYear.SelectedItem?.ToString() ?? "";
            input.no_need_setting = checkBox28.Checked;
            input.step5_courses = BuildStep5Courses();
            input.unpassed_course_ids = GetUnpassedCourseIds();

            List<CandidateCourseJson> selectedExtraCourses = BuildSelectedExtraCourses();

            if (selectedExtraCourses.Count > 0)
            {
                List<CandidateCourseJson> currentScheduleCourses = new List<CandidateCourseJson>();

                if (_lastSolverResult != null && _lastSolverResult.selected_courses != null)
                {
                    int no = 1;

                    foreach (var c in _lastSolverResult.selected_courses)
                    {
                        currentScheduleCourses.Add(new CandidateCourseJson
                        {
                            row_no = no++,
                            course_id = c.course_id,
                            class_code = c.class_code,
                            english_course_name = c.english_course_name,
                            instructor = c.instructor,
                            credits = c.credits,
                            type = c.type,
                            time_slots = c.time_slots,
                            prerequisites = new List<string>()
                        });
                    }
                }

                foreach (CandidateCourseJson extra in selectedExtraCourses)
                {
                    bool exists = currentScheduleCourses.Any(c =>
                        c.course_id.Equals(extra.course_id, StringComparison.OrdinalIgnoreCase) &&
                        c.class_code.Equals(extra.class_code, StringComparison.OrdinalIgnoreCase));

                    if (!exists)
                        currentScheduleCourses.Add(extra);
                }

                input.candidate_courses = currentScheduleCourses;
                input.max_credits = currentScheduleCourses.Sum(c => c.credits);
            }
            else
            {
                input.max_credits = GetMaxCreditsFromForm3();
                input.candidate_courses = BuildCandidateCourses();
            }

            input.preferences = BuildPreferenceJson();

            return input;
        }




        private string ExportJson()
        {
            SchedulerInputJson input = BuildSchedulerInputJson();

            if (input == null)
                return "";

            string folder = Path.Combine(Application.StartupPath, "SchedulerData");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filePath = Path.Combine(folder, "input_schedule.json");

            string json = JsonConvert.SerializeObject(input, Formatting.Indented);
            File.WriteAllText(filePath, json);

            return filePath;
        }

        /*private string GetProjectRootPath()
        {
            return Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\.."));
        }*/

        private string GetProjectRootPath()
        {
            return Application.StartupPath;
        }

        /*private string GetSolverPath()
        {
            return Path.Combine(GetProjectRootPath(), "schedule_solver.py");
        }*/

        private string GetSolverPath()
        {
            return Path.Combine(GetProjectRootPath(), "schedule_solver.py");
        }

        private string GetInputJsonPath()
        {
            return Path.Combine(Application.StartupPath,
                                "SchedulerData",
                                "input_schedule.json");
        }

        private string GetOutputJsonPath()
        {
            return Path.Combine(Application.StartupPath,
                                "SchedulerData",
                                "output_schedule.json");
        }

        /*private string GetInputJsonPath()
        {
            return Path.Combine(Application.StartupPath, "SchedulerData", "input_schedule.json");
        }

        private string GetOutputJsonPath()
        {
            return Path.Combine(Application.StartupPath, "SchedulerData", "output_schedule.json");
        }*/

        private void RunPythonSolver()
        {
            string pythonExe = "python";
            string solverPath = GetSolverPath();
            string inputPath = GetInputJsonPath();
            string outputPath = GetOutputJsonPath();

            if (!File.Exists(solverPath))
                throw new FileNotFoundException("Cannot find schedule_solver.py", solverPath);

            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Cannot find input_schedule.json", inputPath);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = $"\"{solverPath}\" \"{inputPath}\" \"{outputPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = GetProjectRootPath()
            };

            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.Start();

                string stdout = process.StandardOutput.ReadToEnd();
                string stderr = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception("Python solver failed.\n\n" + stderr);

                if (!string.IsNullOrWhiteSpace(stderr))
                    MessageBox.Show("Python message:\n\n" + stderr, "Python Solver");
            }
        }

        private SchedulerOutputJson LoadSolverResult()
        {
            string outputPath = GetOutputJsonPath();

            if (!File.Exists(outputPath))
                throw new FileNotFoundException("Cannot find output_schedule.json", outputPath);

            string json = File.ReadAllText(outputPath);
            SchedulerOutputJson result = JsonConvert.DeserializeObject<SchedulerOutputJson>(json);

            if (result == null)
                throw new Exception("Failed to read output_schedule.json");

            return result;
        }



        private void DisplaySolverResultOnGrid(SchedulerOutputJson result)
        {
            ClearScheduleGridContent();

            List<Tuple<string, string, string>> noTimeCourses = new List<Tuple<string, string, string>>();

            foreach (var course in result.selected_courses)
            {
                string displayText =
                    course.course_id + " " + course.class_code + Environment.NewLine +
                    course.english_course_name + Environment.NewLine +
                    "Instructor: " + course.instructor + Environment.NewLine +
                    course.credits + " Credits";

                if (course.time_slots != null && course.time_slots.Count > 0)
                {
                    foreach (var ts in course.time_slots)
                    {
                        string timeSlot = ts.start + "-" + ts.end;
                        SetScheduleCell(ts.day, timeSlot, displayText);
                    }
                }
                else
                {
                    noTimeCourses.Add(Tuple.Create(
                        Convert.ToString(course.course_id),
                        Convert.ToString(course.english_course_name),
                        Convert.ToString(course.credits)
                    ));
                }
            }

            AddNoTimeCoursesInsideMainGrid(noTimeCourses);

            dataGridRecommendSchedule.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

            dataGridRecommendSchedule.ClearSelection();

            if (dataGridRecommendSchedule.Rows.Count > 0)
            {
                dataGridRecommendSchedule.FirstDisplayedScrollingRowIndex = 0;
            }

            MessageBox.Show(
                "Status: " + result.status + Environment.NewLine +
                "Total Credits: " + result.total_credits,
                "Schedule Generated",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =========================
        // Grid Helpers
        // =========================
        private void SetScheduleCell(string dayName, string timeSlot, string content)
        {
            int columnIndex = -1;
            int rowIndex = -1;

            foreach (DataGridViewColumn column in dataGridRecommendSchedule.Columns)
            {
                if (column.Name.Equals(dayName, StringComparison.OrdinalIgnoreCase))
                {
                    columnIndex = column.Index;
                    break;
                }
            }

            foreach (DataGridViewRow row in dataGridRecommendSchedule.Rows)
            {
                if (row.Cells[0].Value != null &&
                    row.Cells[0].Value.ToString().Equals(timeSlot, StringComparison.OrdinalIgnoreCase))
                {
                    rowIndex = row.Index;
                    break;
                }
            }

            if (columnIndex >= 0 && rowIndex >= 0)
            {
                string oldValue = dataGridRecommendSchedule.Rows[rowIndex].Cells[columnIndex].Value?.ToString();

                if (string.IsNullOrWhiteSpace(oldValue))
                {
                    dataGridRecommendSchedule.Rows[rowIndex].Cells[columnIndex].Value = content;
                }
                else
                {
                    dataGridRecommendSchedule.Rows[rowIndex].Cells[columnIndex].Value =
                        oldValue + Environment.NewLine +
                        "----------------" + Environment.NewLine +
                        content;
                }

                dataGridRecommendSchedule.Rows[rowIndex].MinimumHeight = 70;
            }
        }

        private void ClearScheduleGridContent()
        {
            while (dataGridRecommendSchedule.Rows.Count > TIMETABLE_ROW_COUNT)
                dataGridRecommendSchedule.Rows.RemoveAt(dataGridRecommendSchedule.Rows.Count - 1);

            for (int row = 0; row < dataGridRecommendSchedule.Rows.Count; row++)
            {
                dataGridRecommendSchedule.Rows[row].DefaultCellStyle.BackColor = Color.White;
                dataGridRecommendSchedule.Rows[row].DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                dataGridRecommendSchedule.Rows[row].MinimumHeight = 55;

                for (int col = 1; col < dataGridRecommendSchedule.Columns.Count; col++)
                    dataGridRecommendSchedule.Rows[row].Cells[col].Value = "";
            }
        }

        private void AddNoTimeCoursesInsideMainGrid(List<Tuple<string, string, string>> courses)
        {
            if (courses == null || courses.Count == 0)
                return;

            int blankRow = dataGridRecommendSchedule.Rows.Add();
            dataGridRecommendSchedule.Rows[blankRow].Height = 10;

            int titleRow = dataGridRecommendSchedule.Rows.Add();
            dataGridRecommendSchedule.Rows[titleRow].Cells[0].Value = "Courses\nWithout Fixed\nTimetable";
            dataGridRecommendSchedule.Rows[titleRow].DefaultCellStyle.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridRecommendSchedule.Rows[titleRow].DefaultCellStyle.BackColor =
                Color.FromArgb(235, 242, 255);
            dataGridRecommendSchedule.Rows[titleRow].Height = 55;

            int headerRow = dataGridRecommendSchedule.Rows.Add();
            dataGridRecommendSchedule.Rows[headerRow].Cells[0].Value = "Course ID";
            dataGridRecommendSchedule.Rows[headerRow].Cells[1].Value = "Course Name";
            dataGridRecommendSchedule.Rows[headerRow].Cells[2].Value = "Credits";

            dataGridRecommendSchedule.Rows[headerRow].DefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridRecommendSchedule.Rows[headerRow].DefaultCellStyle.BackColor =
                Color.Gainsboro;
            dataGridRecommendSchedule.Rows[headerRow].Height = 30;

            foreach (Tuple<string, string, string> c in courses)
            {
                int row = dataGridRecommendSchedule.Rows.Add();

                dataGridRecommendSchedule.Rows[row].Cells[0].Value = c.Item1;
                dataGridRecommendSchedule.Rows[row].Cells[1].Value = c.Item2;
                dataGridRecommendSchedule.Rows[row].Cells[2].Value = c.Item3;

                dataGridRecommendSchedule.Rows[row].Height = 32;
            }

            for (int r = titleRow; r < dataGridRecommendSchedule.Rows.Count; r++)
            {
                for (int c = 3; c < dataGridRecommendSchedule.Columns.Count; c++)
                {
                    DataGridViewCell cell = dataGridRecommendSchedule.Rows[r].Cells[c];

                    cell.Value = "";
                    cell.Style.BackColor = Color.White;
                    cell.Style.ForeColor = Color.White;
                    cell.Style.SelectionBackColor = Color.White;
                    cell.Style.SelectionForeColor = Color.White;
                }
            }

            dataGridRecommendSchedule.Invalidate();
        }

        private void dataGridRecommendSchedule_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < TIMETABLE_ROW_COUNT)
                return;

            if (e.ColumnIndex >= 3)
            {
                using (Brush brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }

                e.Handled = true;
            }
        }

        // =========================
        // Events
        // =========================
        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            ApplyNoNeedSettingState();
        }

        private void comboBoxAcademicYear_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBoxCreditNumber_SelectedIndexChanged(object sender, EventArgs e) { }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox4_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox3_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox5_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox6_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox7_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox27_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox26_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox25_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox24_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox23_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox22_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox21_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox8_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox9_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox11_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox10_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox12_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox17_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox16_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox15_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox14_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox13_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox20_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox19_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox18_CheckedChanged(object sender, EventArgs e) { }
        private void comboBoxOtherTimeSlotAvoid_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtCourseName_TextChanged(object sender, EventArgs e) { }
        private void txtProfessorName_TextChanged(object sender, EventArgs e) { }
        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e) { }

        private void checkBoxOtherPreferredCredits_CheckedChanged(object sender, EventArgs e)
        {
            txtOtherPreferredCredits.Enabled = checkBoxOtherPreferredCredits.Checked;
            comboBoxCreditNumber.Enabled = !checkBoxOtherPreferredCredits.Checked;
        }

        private void txtOtherPreferredCredits_TextChanged(object sender, EventArgs e) { }

        private void checkBoxOtherTimeSlotAvoid_CheckedChanged(object sender, EventArgs e)
        {
            txtOtherTimeSlotAvoid.Enabled = !checkBox28.Checked && checkBoxOtherTimeSlotAvoid.Checked;
            comboBoxOtherTimeSlotAvoid.Enabled = !checkBox28.Checked && !checkBoxOtherTimeSlotAvoid.Checked;
        }

        private void txtOtherTimeSlotAvoid_TextChanged(object sender, EventArgs e) { }
        private void dataGridRecommendSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void comboBoxDept_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dataGridViewPage3_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllPreferences();
            ClearScheduleGridContent();
        }

        private void btnGenerateSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                _lastSolverResult = null;
                ClearScheduleGridContent();

                ExportJson();

                string outputPath = GetOutputJsonPath();
                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                RunPythonSolver();

                SchedulerOutputJson result = LoadSolverResult();
                _lastSolverResult = result;

                DisplaySolverResultOnGrid(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to generate schedule:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private string GetNextAvailableFileName(string folder, string baseFileName, string extension)
        {
            string filePath = Path.Combine(folder, baseFileName + extension);

            if (!File.Exists(filePath))
                return filePath;

            int counter = 1;

            while (true)
            {
                filePath = Path.Combine(folder, $"{baseFileName}({counter}){extension}");

                if (!File.Exists(filePath))
                    return filePath;

                counter++;
            }
        }

        private void btnSaveScheduleAsImage_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    string defaultFolder =
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";

                    sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                    sfd.Title = "Save Schedule As Image";
                    sfd.InitialDirectory = defaultFolder;
                    sfd.FileName = Path.GetFileName(
                        GetNextAvailableFileName(
                            defaultFolder,
                            "Schedule",
                            ".png"));

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        SaveDataGridViewAsImage(dataGridRecommendSchedule, sfd.FileName);

                        MessageBox.Show(
                            "Schedule saved successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error saving image:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SaveScheduleResultAsImage(string filePath)
        {
            SaveDataGridViewAsImage(dataGridRecommendSchedule, filePath);
        }

        private void SaveDataGridViewAsImage(DataGridView dgv, string filePath)
        {
            dgv.ClearSelection();

            int originalWidth = dgv.Width;
            int originalHeight = dgv.Height;
            ScrollBars originalScrollBars = dgv.ScrollBars;
            DataGridViewAutoSizeRowsMode originalAutoSizeRowsMode = dgv.AutoSizeRowsMode;

            try
            {
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                int totalWidth = 0;
                if (dgv.RowHeadersVisible)
                    totalWidth += dgv.RowHeadersWidth;

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                        totalWidth += col.Width;
                }

                int totalHeight = dgv.ColumnHeadersVisible ? dgv.ColumnHeadersHeight : 0;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Visible)
                        totalHeight += row.Height;
                }

                totalWidth += 2;
                totalHeight += 2;

                dgv.ScrollBars = ScrollBars.None;
                dgv.Width = totalWidth;
                dgv.Height = totalHeight;
                dgv.Refresh();

                using (Bitmap bmp = new Bitmap(totalWidth, totalHeight))
                {
                    dgv.DrawToBitmap(bmp, new Rectangle(0, 0, totalWidth, totalHeight));

                    if (filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                    {
                        bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            finally
            {
                dgv.Width = originalWidth;
                dgv.Height = originalHeight;
                dgv.ScrollBars = originalScrollBars;
                dgv.AutoSizeRowsMode = originalAutoSizeRowsMode;
                dgv.Refresh();
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnGraduationSummary_Click(object sender, EventArgs e)
        {
            if (_lastSolverResult == null)
            {
                MessageBox.Show(
                    "Please generate a schedule first.",
                    "No Schedule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Form4 form4 = new Form4(
                _form2,
                _lastSolverResult,
                comboBoxAcademicYear.Text
            );

            form4.Show();
        }

        private void EnsureSelectionColumn()
        {
            if (!dataGridViewPage3.Columns.Contains("Select"))
            {
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();

                chk.Name = "Select";
                chk.HeaderText = "";
                chk.Width = 35;
                chk.ReadOnly = false;

                dataGridViewPage3.Columns.Insert(0, chk);
            }

            dataGridViewPage3.RowHeadersVisible = false;
        }

        private List<DataRow> GetSelectedExtraCourseRows()
        {
            List<DataRow> result = new List<DataRow>();

            dataGridViewPage3.EndEdit();

            foreach (DataGridViewRow gridRow in dataGridViewPage3.Rows)
            {
                if (gridRow.IsNewRow)
                    continue;

                bool isChecked = false;

                if (gridRow.Cells["Select"].Value != null)
                    bool.TryParse(gridRow.Cells["Select"].Value.ToString(), out isChecked);

                if (!isChecked)
                    continue;

                DataRowView drv = gridRow.DataBoundItem as DataRowView;

                if (drv != null)
                    result.Add(drv.Row);
            }

            return result;
        }

        private List<CandidateCourseJson> BuildSelectedExtraCourses()
        {
            List<CandidateCourseJson> result = new List<CandidateCourseJson>();

            Dictionary<string, int> step5CreditMap = BuildStep5CreditMap();

            int no = 1;

            foreach (DataRow row in GetSelectedExtraCourseRows())
            {
                string courseId = NormalizeCourseId(row["Course ID"]?.ToString() ?? "");

                int credits = 2;

                if (step5CreditMap.ContainsKey(courseId))
                    credits = step5CreditMap[courseId];

                CandidateCourseJson item = new CandidateCourseJson
                {
                    row_no = no++,
                    course_id = courseId,
                    class_code = row["Class"]?.ToString()?.Trim() ?? "",
                    department = row["Department"]?.ToString()?.Trim() ?? "",
                    year_available = row["Year Available"]?.ToString()?.Trim() ?? "",
                    chinese_course_name = row["Chinese Course Name"]?.ToString()?.Trim() ?? "",
                    english_course_name = row["English Course Name"]?.ToString()?.Trim() ?? "",
                    teaching_language = row["Teaching Language"]?.ToString()?.Trim() ?? "",
                    type = row["Type"]?.ToString()?.Trim() ?? "",
                    timetable_raw = row["Timetable"]?.ToString()?.Trim() ?? "",
                    classroom = row["Classroom"]?.ToString()?.Trim() ?? "",
                    instructor = row["Instructor"]?.ToString()?.Trim() ?? "",
                    credits = credits,
                    time_slots = ParseTimeSlotsFromTimetable(row["Timetable"]?.ToString()?.Trim() ?? ""),
                    prerequisites = new List<string>()
                };

                result.Add(item);
            }

            return result;
        }

        private CandidateCourseJson FindConflictCourseInCurrentSchedule(CandidateCourseJson extra)
        {
            if (_lastSolverResult == null || _lastSolverResult.selected_courses == null)
                return null;

            foreach (var old in _lastSolverResult.selected_courses)
            {
                CandidateCourseJson oldCourse = new CandidateCourseJson
                {
                    course_id = old.course_id,
                    class_code = old.class_code,
                    english_course_name = old.english_course_name,
                    instructor = old.instructor,
                    credits = old.credits,
                    type = old.type,
                    time_slots = old.time_slots
                };

                if (HasTimeConflict(extra, oldCourse))
                    return oldCourse;
            }

            return null;
        }

        private bool HasTimeConflict(CandidateCourseJson a, CandidateCourseJson b)
        {
            if (a.time_slots == null || b.time_slots == null)
                return false;

            foreach (var ta in a.time_slots)
            {
                foreach (var tb in b.time_slots)
                {
                    if (ta.day != tb.day)
                        continue;

                    if (string.Compare(ta.start, tb.end) < 0 &&
                        string.Compare(ta.end, tb.start) > 0)
                        return true;
                }
            }

            return false;
        }


        private void AddCourseToScheduleGrid(CandidateCourseJson course)
        {
            if (course.time_slots == null)
                return;

            string displayText =
                course.course_id + " " + course.class_code + Environment.NewLine +
                course.english_course_name + Environment.NewLine +
                "Instructor: " + course.instructor + Environment.NewLine +
                course.credits + " Credits";

            foreach (var ts in course.time_slots)
            {
                string timeSlot = ts.start + "-" + ts.end;
                SetScheduleCell(ts.day, timeSlot, displayText);
            }
        }

        private void RemoveCourseFromScheduleGrid(CandidateCourseJson course)
        {
            if (course.time_slots == null)
                return;

            foreach (var ts in course.time_slots)
            {
                string timeSlot = ts.start + "-" + ts.end;

                int col = -1;
                int row = -1;

                foreach (DataGridViewColumn column in dataGridRecommendSchedule.Columns)
                {
                    if (column.Name.Equals(ts.day, StringComparison.OrdinalIgnoreCase))
                    {
                        col = column.Index;
                        break;
                    }
                }

                foreach (DataGridViewRow r in dataGridRecommendSchedule.Rows)
                {
                    if ((r.Cells[0].Value ?? "").ToString()
                        .Equals(timeSlot, StringComparison.OrdinalIgnoreCase))
                    {
                        row = r.Index;
                        break;
                    }
                }

                if (row >= 0 && col >= 0)
                    dataGridRecommendSchedule.Rows[row].Cells[col].Value = "";
            }
        }

        private SelectedCourseJson ConvertCandidateToSelected(CandidateCourseJson c)
        {
            return new SelectedCourseJson
            {
                course_id = c.course_id,
                class_code = c.class_code,
                chinese_course_name = c.chinese_course_name,
                english_course_name = c.english_course_name,
                instructor = c.instructor,
                credits = c.credits,
                type = c.type,
                time_slots = c.time_slots
            };
        }


        private void AddCourseToSolverResult(CandidateCourseJson course)
        {
            if (_lastSolverResult == null)
                return;

            bool exists =
                _lastSolverResult.selected_courses.Any(x =>
                    x.course_id == course.course_id &&
                    x.class_code == course.class_code);

            if (exists)
                return;

            _lastSolverResult.selected_courses.Add(
                ConvertCandidateToSelected(course));
        }

        private void RemoveCourseFromSolverResult(CandidateCourseJson course)
        {
            if (_lastSolverResult == null)
                return;

            var old =
                _lastSolverResult.selected_courses
                .FirstOrDefault(x =>
                    x.course_id == course.course_id &&
                    x.class_code == course.class_code);

            if (old != null)
                _lastSolverResult.selected_courses.Remove(old);
        }


        private void btnApplySelectedExtraCourses_Click(object sender, EventArgs e)
        {
            List<CandidateCourseJson> selectedExtras = BuildSelectedExtraCourses();

            if (selectedExtras.Count == 0)
            {
                MessageBox.Show("Please select one extra course first.");
                return;
            }

            CandidateCourseJson extra = selectedExtras[0];

            if (IsAlreadyPassed(extra.course_id))
            {
                MessageBox.Show(
                    extra.english_course_name + " is already passed, so it cannot be added again.",
                    "Already Passed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            CandidateCourseJson sameCourse = FindSameCourseInCurrentSchedule(extra);

            if (sameCourse != null)
            {
                DialogResult switchResult = MessageBox.Show(
                    $"{sameCourse.english_course_name} by instructor {sameCourse.instructor} is already in the current schedule.\n\n" +
                    $"Do you want to switch to:\n{extra.english_course_name} by instructor {extra.instructor}?",
                    "Course Already Added",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (switchResult == DialogResult.No)
                    return;

                RemoveCourseFromScheduleGrid(sameCourse);
                RemoveCourseFromSolverResult(sameCourse);
            }

            CandidateCourseJson conflictCourse = FindConflictCourseInCurrentSchedule(extra);

            if (conflictCourse != null)
            {
                DialogResult result = MessageBox.Show(
                    $"{extra.english_course_name} is overlapped with current course:\n\n" +
                    $"{conflictCourse.english_course_name}\n\n" +
                    $"Yes = choose {extra.english_course_name}\n" +
                    $"No = keep {conflictCourse.english_course_name}",
                    "Time Conflict",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                    return;

                RemoveCourseFromScheduleGrid(conflictCourse);
                RemoveCourseFromSolverResult(conflictCourse);
            }

            AddCourseToScheduleGrid(extra);
            AddCourseToSolverResult(extra);

            MessageBox.Show(
                "Selected extra course has been applied.",
                "Updated",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }




        private bool IsAlreadyPassed(string courseId)
        {
            return BuildStep5Courses().Any(c =>
                c.passed &&
                c.course_id.Equals(courseId, StringComparison.OrdinalIgnoreCase));
        }

        private CandidateCourseJson FindSameCourseInCurrentSchedule(CandidateCourseJson extra)
        {
            if (_lastSolverResult == null || _lastSolverResult.selected_courses == null)
                return null;

            foreach (var old in _lastSolverResult.selected_courses)
            {
                if (old.course_id.Equals(extra.course_id, StringComparison.OrdinalIgnoreCase))
                {
                    return new CandidateCourseJson
                    {
                        course_id = old.course_id,
                        class_code = old.class_code,
                        english_course_name = old.english_course_name,
                        instructor = old.instructor,
                        credits = old.credits,
                        type = old.type,
                        time_slots = old.time_slots
                    };
                }
            }

            return null;
        }





        private void dataGridViewPage3_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewPage3.IsCurrentCellDirty)
                dataGridViewPage3.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridViewPage3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dataGridViewPage3.Columns[e.ColumnIndex].Name != "Select")
                return;

            bool checkedNow = Convert.ToBoolean(dataGridViewPage3.Rows[e.RowIndex].Cells["Select"].Value ?? false);

            if (!checkedNow)
                return;

            foreach (DataGridViewRow row in dataGridViewPage3.Rows)
            {
                if (row.Index != e.RowIndex && !row.IsNewRow)
                    row.Cells["Select"].Value = false;
            }
        }




        private List<CandidateCourseJson> ResolveSelectedExtraCourses()
        {
            List<CandidateCourseJson> finalCourses = BuildCandidateCourses();
            List<CandidateCourseJson> selectedExtras = BuildSelectedExtraCourses();

            foreach (CandidateCourseJson extra in selectedExtras)
            {
                CandidateCourseJson conflictCourse = finalCourses
                    .FirstOrDefault(c => HasTimeConflict(c, extra));

                if (conflictCourse == null)
                {
                    finalCourses.Add(extra);
                }
                else
                {
                    DialogResult choice = MessageBox.Show(
                        $"{extra.english_course_name} is overlapped with current course:\n\n" +
                        $"{conflictCourse.english_course_name}\n\n" +
                        $"Yes = choose {extra.english_course_name}\n" +
                        $"No = keep {conflictCourse.english_course_name}",
                        "Time Conflict",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (choice == DialogResult.Yes)
                    {
                        finalCourses.Remove(conflictCourse);
                        finalCourses.Add(extra);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return finalCourses;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnBackToStep1_Click(object sender, EventArgs e)
        {
            _lastSolverResult = null;

            ClearScheduleGridContent();

            Program.Page1 = new Form1();
            Program.Page2 = new Form2();
            Program.Page3 = new Form3(Program.Page2);

            Program.Page1.Show();

            this.Hide();
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            // ComboBoxes
            comboBoxAcademicYear.SelectedIndex = -1;
            comboBoxCreditNumber.SelectedIndex = -1;
            comboBoxOtherTimeSlotAvoid.SelectedIndex = -1;
            comboBoxLanguage.SelectedIndex = -1;

            // TextBoxes
            txtOtherPreferredCredits.Clear();
            txtOtherTimeSlotAvoid.Clear();
            txtCourseName.Clear();
            txtProfessorName.Clear();

            // CheckBoxes
            checkBoxOtherPreferredCredits.Checked = false;
            checkBoxOtherTimeSlotAvoid.Checked = false;

            // Clear checkBox1 ~ checkBox27
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is CheckBox cb)
                {
                    cb.Checked = false;
                }
            }


            txtOtherPreferredCredits.Enabled = false;
            txtOtherTimeSlotAvoid.Enabled = false;


            comboBoxOtherTimeSlotAvoid.SelectedIndex = 0; 
            comboBoxLanguage.SelectedIndex = 0;     
        }

    }
}


