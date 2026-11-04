/*using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ThesisSchedulerGUI
{
    public partial class Form4 : Form
    {
        private Form2 _form2;
        private SchedulerOutputJson _solverResult;
        private string _academicYear;

        private Label lblTitle;
        private Panel pnlStatusCard;
        private RichTextBox rtbStudentInfo;
        private RichTextBox rtbAcademicInfo;

        private Label lblProgressTitle;
        private Label lblProgressPercent;
        private Label lblProgressGoal;
        private ProgressBar progressGraduation;

        private DataGridView dgvPreviousCourses;
        private DataGridView dgvCurrentSemester;
        private DataGridView dgvFuturePlan;

        private Label lblPreviousFooter;
        private Label lblCurrentFooter;
        private Label lblFutureFooter;

        private PictureBox picCompletedChart;
        private PictureBox picFutureChart;

        private Button btnClose;

        private const int DEFAULT_FUTURE_SEMESTER_CREDITS = 16;

        public Form4(Form2 form2, SchedulerOutputJson solverResult, string academicYear)
        {
            _form2 = form2;
            _solverResult = solverResult;
            _academicYear = academicYear;

            InitializeComponent();
            BuildUI();
            LoadGraduationSummary();
        }

        private void BuildUI()
        {
            this.Text = "Graduation Planning Summary";
            this.Width = 1900;
            this.Height = 1100;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.AutoScroll = true;

            lblTitle = new Label();
            lblTitle.Text = "🎓 Graduation Planning Summary";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Left = 25;
            lblTitle.Top = 25;
            lblTitle.Width = 900;
            lblTitle.Height = 55;
            this.Controls.Add(lblTitle);

            pnlStatusCard = new Panel();
            pnlStatusCard.Left = 25;
            pnlStatusCard.Top = 95;
            pnlStatusCard.Width = 1800;
            pnlStatusCard.Height = 165;
            pnlStatusCard.BackColor = Color.White;
            pnlStatusCard.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pnlStatusCard);

            rtbStudentInfo = CreateInfoBox(20, 15, 760, 135);
            pnlStatusCard.Controls.Add(rtbStudentInfo);

            rtbAcademicInfo = CreateInfoBox(850, 15, 1000, 135);
            pnlStatusCard.Controls.Add(rtbAcademicInfo);

            lblProgressTitle = new Label();
            lblProgressTitle.Text = "Graduation Progress";
            lblProgressTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblProgressTitle.Left = 25;
            lblProgressTitle.Top = 285;
            lblProgressTitle.Width = 400;
            lblProgressTitle.Height = 35;
            this.Controls.Add(lblProgressTitle);

            progressGraduation = new ProgressBar();
            progressGraduation.Left = 25;
            progressGraduation.Top = 330;
            progressGraduation.Width = 1280;
            progressGraduation.Height = 32;
            this.Controls.Add(progressGraduation);

            lblProgressPercent = new Label();
            lblProgressPercent.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblProgressPercent.ForeColor = Color.Green;
            lblProgressPercent.Left = 1325;
            lblProgressPercent.Top = 315;
            lblProgressPercent.Width = 250;
            lblProgressPercent.Height = 70;
            this.Controls.Add(lblProgressPercent);

            lblProgressGoal = new Label();
            lblProgressGoal.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblProgressGoal.ForeColor = Color.DimGray;
            lblProgressGoal.Left = 25;
            lblProgressGoal.Top = 370;
            lblProgressGoal.Width = 700;
            lblProgressGoal.Height = 25;
            this.Controls.Add(lblProgressGoal);

            AddSectionLabel("📘 Previous Completed Courses", 25, 425, Color.FromArgb(0, 80, 180));
            AddSectionLabel("✅ This Semester Recommendation", 480, 425, Color.Green);
            AddSectionLabel("📅 Expected Future Semester Plan", 935, 425, Color.DarkOrange);

            dgvPreviousCourses = MakeGrid(25, 465, 430, 500);
            dgvCurrentSemester = MakeGrid(480, 465, 430, 500);
            dgvFuturePlan = MakeGrid(935, 465, 430, 500);

            this.Controls.Add(dgvPreviousCourses);
            this.Controls.Add(dgvCurrentSemester);
            this.Controls.Add(dgvFuturePlan);

            lblPreviousFooter = MakeFooter(25, 975, 430);
            lblCurrentFooter = MakeFooter(480, 975, 430);
            lblFutureFooter = MakeFooter(935, 975, 430);

            this.Controls.Add(lblPreviousFooter);
            this.Controls.Add(lblCurrentFooter);
            this.Controls.Add(lblFutureFooter);

            picCompletedChart = MakeChartBox(1400, 425, 430, 250);
            picFutureChart = MakeChartBox(1400, 700, 430, 250);

            this.Controls.Add(picCompletedChart);
            this.Controls.Add(picFutureChart);

            Button btnSave = new Button();
            btnSave.Text = "Save Summary";
            btnSave.Left = 1500;
            btnSave.Top = 975;
            btnSave.Width = 150;
            btnSave.Height = 38;
            btnSave.Font = new Font("Segoe UI", 10F);
            btnSave.Click += btnSaveSummary_Click;
            this.Controls.Add(btnSave);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Left = 1670;
            btnClose.Top = 975;
            btnClose.Width = 150;
            btnClose.Height = 38;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void LoadGraduationSummary()
        {
            int requiredCredits = GetRequiredCredits();
            string programLevel = GetProgramLevel();

            ParseAcademicYear(out string programLevelText, out string currentSemesterText);

            List<CourseInfo> completedCourses = GetCompletedCourses();
            List<CourseInfo> currentCourses = GetCurrentSemesterCourses();
            List<CourseInfo> allRemainingCourses = GetRemainingCourses(completedCourses, currentCourses);

            int completedCredits = completedCourses.Sum(c => c.Credits);
            int currentCredits = currentCourses.Sum(c => c.Credits);
            int afterThisSemester = completedCredits + currentCredits;

            int remainingCredits = Math.Max(requiredCredits - afterThisSemester, 0);
            int futureSemestersLeft = GetRemainingAcademicSemesters();

            int semestersNeededByCredits = remainingCredits == 0
                ? 0
                : (int)Math.Ceiling(remainingCredits / (double)DEFAULT_FUTURE_SEMESTER_CREDITS);

            int estimatedFutureSemesters = Math.Min(semestersNeededByCredits, futureSemestersLeft);

            List<CourseInfo> futurePlanCourses = BuildFuturePlanCourses(
                allRemainingCourses,
                remainingCredits,
                estimatedFutureSemesters);

            double progressDouble = requiredCredits <= 0
                ? 0
                : afterThisSemester * 100.0 / requiredCredits;

            if (progressDouble > 100)
                progressDouble = 100;

            int progress = Math.Min(100, (int)Math.Round(progressDouble));

            string risk = GetGraduationRisk(remainingCredits, semestersNeededByCredits, futureSemestersLeft);

            string studentName = Program.Page1.GetStudentName();
            string studentId = Program.Page1.GetStudentId();
            string studentDept = Program.Page1.GetStudentDepartment();
            string studentGrade = Program.Page1.GetStudentGrade();

            rtbStudentInfo.Clear();
            AppendBold(rtbStudentInfo, "Student Information\n");
            AppendNormal(rtbStudentInfo,
                $"Name: {studentName}\n" +
                $"Student ID: {studentId}\n" +
                $"Department: {studentDept}\n" +
                $"Grade: {studentGrade}");

            rtbAcademicInfo.Clear();
            rtbAcademicInfo.SelectionTabs = new int[] { 320, 650 };

            AppendBold(rtbAcademicInfo, "Current Academic Status\n");
            AppendNormal(rtbAcademicInfo,
                $"Program Level: {programLevelText}\tCurrent Semester: {currentSemesterText}\n" +
                $"Required Credits: {requiredCredits}\tCompleted Credits: {completedCredits} / {requiredCredits}\n\n");

            AppendBold(rtbAcademicInfo, "This Semester Recommendation\n");
            AppendNormal(rtbAcademicInfo,
                $"Recommended Credits: {currentCredits}\tAfter This Semester: {afterThisSemester} / {requiredCredits}\n\n");

            AppendBold(rtbAcademicInfo, "Remaining to Graduate\n");
            AppendNormal(rtbAcademicInfo,
                $"Remaining Credits: {remainingCredits}\tEstimated Future Semesters: {estimatedFutureSemesters}\tGraduation Risk: {risk}");

            progressGraduation.Value = progress;

            lblProgressPercent.Text =
                $"{progressDouble:0.00}%\n({Math.Min(afterThisSemester, requiredCredits)} / {requiredCredits})";

            lblProgressGoal.Text =
                $"Goal: {requiredCredits} credits for {programLevel} graduation";

            LoadPreviousCoursesGrid(completedCourses);
            LoadCurrentSemesterGrid(currentCourses);
            LoadFuturePlanGrid(futurePlanCourses, estimatedFutureSemesters, remainingCredits);
            LoadPieCharts(completedCourses, futurePlanCourses);

            lblPreviousFooter.Text =
                $"Total Courses: {completedCourses.Count}        Total Credits: {completedCredits}";

            lblCurrentFooter.Text =
                $"Total Courses: {currentCourses.Count}        Total Credits: {currentCredits}";

            lblFutureFooter.Text =
                $"Estimated Future Semesters: {estimatedFutureSemesters}        Remaining Credits: {remainingCredits}";
        }

        private List<CourseInfo> BuildFuturePlanCourses(List<CourseInfo> allRemainingCourses, int remainingCredits, int maxFutureSemesters)
        {
            List<CourseInfo> result = new List<CourseInfo>();

            if (remainingCredits <= 0 || maxFutureSemesters <= 0)
                return result;

            int addedCredits = 0;
            int semesterNo = 1;
            int semesterCredits = 0;

            foreach (CourseInfo c in allRemainingCourses)
            {
                if (addedCredits >= remainingCredits)
                    break;

                if (semesterCredits + c.Credits > DEFAULT_FUTURE_SEMESTER_CREDITS)
                {
                    semesterNo++;
                    semesterCredits = 0;
                }

                if (semesterNo > maxFutureSemesters)
                    break;

                result.Add(c);
                addedCredits += c.Credits;
                semesterCredits += c.Credits;
            }

            return result;
        }

        private int GetRemainingAcademicSemesters()
        {
            string s = (_academicYear ?? "").ToLowerInvariant();

            if (s.Contains("bachelor"))
            {
                if (s.Contains("1st year") && s.Contains("semester 1")) return 7;
                if (s.Contains("1st year") && s.Contains("semester 2")) return 6;
                if (s.Contains("2nd year") && s.Contains("semester 1")) return 5;
                if (s.Contains("2nd year") && s.Contains("semester 2")) return 4;
                if (s.Contains("3rd year") && s.Contains("semester 1")) return 3;
                if (s.Contains("3rd year") && s.Contains("semester 2")) return 2;
                if (s.Contains("4th year") && s.Contains("semester 1")) return 1;
                if (s.Contains("4th year") && s.Contains("semester 2")) return 0;
            }

            if (s.Contains("master"))
            {
                if (s.Contains("1st year") && s.Contains("semester 1")) return 3;
                if (s.Contains("1st year") && s.Contains("semester 2")) return 2;
                if (s.Contains("2nd year") && s.Contains("semester 1")) return 1;
                if (s.Contains("2nd year") && s.Contains("semester 2")) return 0;
            }

            if (s.Contains("phd"))
            {
                if (s.Contains("1st year") && s.Contains("semester 1")) return 7;
                if (s.Contains("1st year") && s.Contains("semester 2")) return 6;
                if (s.Contains("2nd year") && s.Contains("semester 1")) return 5;
                if (s.Contains("2nd year") && s.Contains("semester 2")) return 4;
                if (s.Contains("3rd year") && s.Contains("semester 1")) return 3;
                if (s.Contains("3rd year") && s.Contains("semester 2")) return 2;
                if (s.Contains("4th year") && s.Contains("semester 1")) return 1;
                if (s.Contains("4th year") && s.Contains("semester 2")) return 0;
            }

            return 0;
        }

        private string GetGraduationRisk(int remainingCredits, int semestersNeededByCredits, int futureSemestersLeft)
        {
            if (remainingCredits == 0)
                return "Low";

            if (futureSemestersLeft == 0)
                return "High";

            if (semestersNeededByCredits <= futureSemestersLeft)
                return "Low";

            return "High";
        }

        private void ParseAcademicYear(out string programLevelText, out string currentSemesterText)
        {
            string s = _academicYear ?? "";
            string[] parts = s.Split(',');

            programLevelText = parts.Length > 0 ? parts[0].Trim() : s;
            currentSemesterText = parts.Length > 1 ? parts[1].Trim() : "";
        }

        private RichTextBox CreateInfoBox(int left, int top, int width, int height)
        {
            RichTextBox rtb = new RichTextBox();

            rtb.Left = left;
            rtb.Top = top;
            rtb.Width = width;
            rtb.Height = height;
            rtb.BorderStyle = BorderStyle.None;
            rtb.BackColor = Color.White;
            rtb.ReadOnly = true;
            rtb.ScrollBars = RichTextBoxScrollBars.None;
            rtb.TabStop = false;
            rtb.Font = new Font("Segoe UI", 10F);
            rtb.Cursor = Cursors.Arrow;
            rtb.HideSelection = true;
            rtb.GotFocus += (s, e) => this.ActiveControl = null;

            return rtb;
        }

        private void AppendBold(RichTextBox rtb, string text)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            rtb.AppendText(text);
        }

        private void AppendNormal(RichTextBox rtb, string text)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            rtb.AppendText(text);
        }

        private void AddSectionLabel(string text, int left, int top, Color color)
        {
            Label lbl = new Label();

            lbl.Text = text;
            lbl.Left = left;
            lbl.Top = top;
            lbl.Width = 430;
            lbl.Height = 30;
            lbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbl.ForeColor = color;

            this.Controls.Add(lbl);
        }

        private DataGridView MakeGrid(int left, int top, int width, int height)
        {
            DataGridView dgv = new DataGridView();

            dgv.Left = left;
            dgv.Top = top;
            dgv.Width = width;
            dgv.Height = height;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 250);
            dgv.BackgroundColor = Color.White;

            return dgv;
        }

        private Label MakeFooter(int left, int top, int width)
        {
            Label lbl = new Label();

            lbl.Left = left;
            lbl.Top = top;
            lbl.Width = width;
            lbl.Height = 35;
            lbl.BackColor = Color.FromArgb(235, 245, 235);
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lbl.Padding = new Padding(10, 7, 10, 5);
            lbl.BorderStyle = BorderStyle.FixedSingle;

            return lbl;
        }

        private PictureBox MakeChartBox(int left, int top, int width, int height)
        {
            PictureBox pic = new PictureBox();

            pic.Left = left;
            pic.Top = top;
            pic.Width = width;
            pic.Height = height;
            pic.BackColor = Color.White;
            pic.BorderStyle = BorderStyle.FixedSingle;

            return pic;
        }

        private void LoadPieCharts(List<CourseInfo> completedCourses, List<CourseInfo> futurePlanCourses)
        {
            Dictionary<string, int> completedByCategory = completedCourses
                .GroupBy(c => string.IsNullOrWhiteSpace(c.Category) ? "Unknown" : c.Category)
                .ToDictionary(g => g.Key, g => g.Sum(c => c.Credits));

            Dictionary<string, int> futureSemesterCredits = BuildFutureSemesterCredits(futurePlanCourses);

            DrawPieChart(picCompletedChart, completedByCategory, "Completed Credits by Category");
            DrawPieChart(picFutureChart, futureSemesterCredits, "Planned Credits by Future Semester");
        }

        private Dictionary<string, int> BuildFutureSemesterCredits(List<CourseInfo> courses)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            if (courses == null || courses.Count == 0)
                return result;

            int semesterNo = 1;
            int semesterCredits = 0;

            foreach (CourseInfo c in courses)
            {
                if (semesterCredits + c.Credits > DEFAULT_FUTURE_SEMESTER_CREDITS)
                {
                    semesterNo++;
                    semesterCredits = 0;
                }

                string semesterName = "Future Semester " + semesterNo;

                if (!result.ContainsKey(semesterName))
                    result[semesterName] = 0;

                result[semesterName] += c.Credits;
                semesterCredits += c.Credits;
            }

            return result;
        }

        private void DrawPieChart(PictureBox pic, Dictionary<string, int> data, string title)
        {
            Bitmap bmp = new Bitmap(pic.Width, pic.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Font titleFont = new Font("Segoe UI", 11F, FontStyle.Bold);
                Font labelFont = new Font("Segoe UI", 8F);

                g.DrawString(title, titleFont, Brushes.Black, 10, 8);

                if (data == null || data.Count == 0 || data.Sum(x => x.Value) == 0)
                {
                    g.DrawString("No future semester plan needed", labelFont, Brushes.Gray, 20, 60);
                    pic.Image = bmp;
                    return;
                }

                int total = data.Sum(x => x.Value);
                Rectangle pieRect = new Rectangle(25, 50, 165, 165);

                Color[] colors =
                {
                    Color.SteelBlue,
                    Color.Orange,
                    Color.SeaGreen,
                    Color.MediumPurple,
                    Color.IndianRed,
                    Color.Goldenrod,
                    Color.Teal,
                    Color.SlateGray
                };

                float startAngle = 0;
                int i = 0;
                int legendX = 215;
                int legendY = 55;

                foreach (var item in data)
                {
                    float sweepAngle = item.Value * 360f / total;

                    using (Brush b = new SolidBrush(colors[i % colors.Length]))
                    {
                        g.FillPie(b, pieRect, startAngle, sweepAngle);
                        g.FillRectangle(b, legendX, legendY + i * 25, 14, 14);
                    }

                    g.DrawPie(Pens.White, pieRect, startAngle, sweepAngle);
                    g.DrawString($"{item.Key}: {item.Value} credits", labelFont, Brushes.Black, legendX + 20, legendY + i * 25 - 2);

                    startAngle += sweepAngle;
                    i++;
                }
            }

            pic.Image = bmp;
        }

        private List<CourseInfo> GetCompletedCourses()
        {
            List<CourseInfo> result = new List<CourseInfo>();

            if (_form2 == null)
                return result;

            DataTable dt = _form2.GetStep5Table();

            if (dt == null)
                return result;

            foreach (DataRow row in dt.Rows)
            {
                string passed = row["Check passed"]?.ToString()?.Trim() ?? "";

                if (passed != "✓")
                    continue;

                result.Add(new CourseInfo
                {
                    CourseID = row["Course ID"]?.ToString() ?? "",
                    CourseName = row["English Name Course"]?.ToString() ?? "",
                    Credits = ParseInt(row["Credits"]?.ToString() ?? ""),
                    Category = row["Category"]?.ToString() ?? ""
                });
            }

            return result;
        }

        private List<CourseInfo> GetCurrentSemesterCourses()
        {
            List<CourseInfo> result = new List<CourseInfo>();

            if (_solverResult == null || _solverResult.selected_courses == null)
                return result;

            foreach (var c in _solverResult.selected_courses)
            {
                result.Add(new CourseInfo
                {
                    CourseID = c.course_id,
                    CourseName = c.english_course_name,
                    Credits = c.credits,
                    Category = c.type
                });
            }

            return result;
        }

        private List<CourseInfo> GetRemainingCourses(List<CourseInfo> completed, List<CourseInfo> current)
        {
            List<CourseInfo> result = new List<CourseInfo>();

            if (_form2 == null)
                return result;

            DataTable dt = _form2.GetStep5Table();

            if (dt == null)
                return result;

            HashSet<string> completedIds = new HashSet<string>(
                completed.Select(c => c.CourseID),
                StringComparer.OrdinalIgnoreCase);

            HashSet<string> currentIds = new HashSet<string>(
                current.Select(c => c.CourseID),
                StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in dt.Rows)
            {
                string cid = row["Course ID"]?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(cid))
                    continue;

                if (completedIds.Contains(cid))
                    continue;

                if (currentIds.Contains(cid))
                    continue;

                result.Add(new CourseInfo
                {
                    CourseID = cid,
                    CourseName = row["English Name Course"]?.ToString() ?? "",
                    Credits = ParseInt(row["Credits"]?.ToString() ?? ""),
                    Category = row["Category"]?.ToString() ?? ""
                });
            }

            return result;
        }

        private void LoadPreviousCoursesGrid(List<CourseInfo> courses)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Course ID");
            dt.Columns.Add("Course Name");
            dt.Columns.Add("Credits");
            dt.Columns.Add("Category");

            foreach (var c in courses)
                dt.Rows.Add(c.CourseID, c.CourseName, c.Credits, c.Category);

            dgvPreviousCourses.DataSource = dt;
        }

        private void LoadCurrentSemesterGrid(List<CourseInfo> courses)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Course ID");
            dt.Columns.Add("Course Name");
            dt.Columns.Add("Credits");
            dt.Columns.Add("Category");

            foreach (var c in courses)
                dt.Rows.Add(c.CourseID, c.CourseName, c.Credits, c.Category);

            dgvCurrentSemester.DataSource = dt;
        }

        private void LoadFuturePlanGrid(List<CourseInfo> courses, int futureSemesters, int remainingCredits)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Expected");
            dt.Columns.Add("Course ID");
            dt.Columns.Add("Course Name");
            dt.Columns.Add("Credits");

            if (remainingCredits <= 0 || futureSemesters <= 0 || courses.Count == 0)
            {
                dgvFuturePlan.DataSource = dt;
                return;
            }

            int semesterNo = 1;
            int semesterCredits = 0;

            foreach (CourseInfo c in courses)
            {
                if (semesterCredits + c.Credits > DEFAULT_FUTURE_SEMESTER_CREDITS)
                {
                    semesterNo++;
                    semesterCredits = 0;
                }

                if (semesterNo > futureSemesters)
                    break;

                dt.Rows.Add(
                    "Future Semester " + semesterNo,
                    c.CourseID,
                    c.CourseName,
                    c.Credits);

                semesterCredits += c.Credits;
            }

            dgvFuturePlan.DataSource = dt;
        }

        private int GetRequiredCredits()
        {
            string s = (_academicYear ?? "").ToLowerInvariant();

            if (s.Contains("bachelor"))
                return 128;

            if (s.Contains("master"))
                return 32;

            if (s.Contains("phd") || s.Contains("doctor"))
                return 30;

            return 128;
        }

        private string GetProgramLevel()
        {
            string s = (_academicYear ?? "").ToLowerInvariant();

            if (s.Contains("bachelor"))
                return "Bachelor";

            if (s.Contains("master"))
                return "Master";

            if (s.Contains("phd") || s.Contains("doctor"))
                return "Doctoral";

            return "Bachelor";
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


        private void btnSaveSummary_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    string defaultFolder =
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";

                    sfd.Filter = "PNG Image|*.png";
                    sfd.Title = "Save Graduation Planning Summary";
                    sfd.InitialDirectory = defaultFolder;
                    sfd.FileName = Path.GetFileName(
                        GetNextAvailableFileName(
                            defaultFolder,
                            "Graduation_Planning_Summary",
                            ".png"));

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        SaveFormAsImage(sfd.FileName);

                        MessageBox.Show(
                            "Graduation planning summary saved successfully.",
                            "Saved",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to save summary:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }



        private void SaveFormAsImage(string filePath)
        {
            this.Refresh();

            using (Bitmap bmp = new Bitmap(this.Width, this.Height))
            {
                this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private int ParseInt(string s)
        {
            int value;

            if (int.TryParse(s, out value))
                return value;

            return 0;
        }

        private class CourseInfo
        {
            public string CourseID { get; set; }
            public string CourseName { get; set; }
            public int Credits { get; set; }
            public string Category { get; set; }
        }
    }
}*/

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ThesisSchedulerGUI
{
    public partial class Form4 : Form
    {
        private Form2 _form2;
        private SchedulerOutputJson _solverResult;
        private string _academicYear;

        private Label lblTitle;
        private Panel pnlStatusCard;
        private RichTextBox rtbStudentInfo;
        private RichTextBox rtbAcademicInfo;

        private Label lblProgressTitle;
        private Label lblProgressPercent;
        private Label lblProgressGoal;
        private ProgressBar progressGraduation;

        private DataGridView dgvPreviousCourses;
        private DataGridView dgvCurrentSemester;
        private DataGridView dgvFuturePlan;

        private Label lblPreviousFooter;
        private Label lblCurrentFooter;
        private Label lblFutureFooter;

        private PictureBox picCompletedChart;
        private PictureBox picFutureChart;

        private Button btnExportPrevious;
        private Button btnExportCurrent;
        private Button btnExportFuture;

        private Button btnClose;

        private const int DEFAULT_FUTURE_SEMESTER_CREDITS = 16;

        public Form4(Form2 form2, SchedulerOutputJson solverResult, string academicYear)
        {
            _form2 = form2;
            _solverResult = solverResult;
            _academicYear = academicYear;

            InitializeComponent();
            BuildUI();
            LoadGraduationSummary();
            this.AutoScaleMode = AutoScaleMode.None;
        }

        private void BuildUI()
        {
            this.Text = "Graduation Planning Summary";
            this.Width = 1900;
            this.Height = 1120;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.AutoScroll = true;

            lblTitle = new Label();
            lblTitle.Text = "🎓 Graduation Planning Summary";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Left = 25;
            lblTitle.Top = 25;
            lblTitle.Width = 900;
            lblTitle.Height = 55;
            this.Controls.Add(lblTitle);

            pnlStatusCard = new Panel();
            pnlStatusCard.Left = 25;
            pnlStatusCard.Top = 95;
            pnlStatusCard.Width = 1800;
            pnlStatusCard.Height = 165;
            pnlStatusCard.BackColor = Color.White;
            pnlStatusCard.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pnlStatusCard);

            rtbStudentInfo = CreateInfoBox(20, 15, 760, 135);
            pnlStatusCard.Controls.Add(rtbStudentInfo);

            rtbAcademicInfo = CreateInfoBox(850, 15, 1000, 135);
            pnlStatusCard.Controls.Add(rtbAcademicInfo);

            lblProgressTitle = new Label();
            lblProgressTitle.Text = "Graduation Progress";
            lblProgressTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblProgressTitle.Left = 25;
            lblProgressTitle.Top = 285;
            lblProgressTitle.Width = 400;
            lblProgressTitle.Height = 35;
            this.Controls.Add(lblProgressTitle);

            progressGraduation = new ProgressBar();
            progressGraduation.Left = 25;
            progressGraduation.Top = 330;
            progressGraduation.Width = 1280;
            progressGraduation.Height = 32;
            this.Controls.Add(progressGraduation);

            lblProgressPercent = new Label();
            lblProgressPercent.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblProgressPercent.ForeColor = Color.Green;
            lblProgressPercent.Left = 1325;
            lblProgressPercent.Top = 315;
            lblProgressPercent.Width = 250;
            lblProgressPercent.Height = 70;
            this.Controls.Add(lblProgressPercent);

            lblProgressGoal = new Label();
            lblProgressGoal.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblProgressGoal.ForeColor = Color.DimGray;
            lblProgressGoal.Left = 25;
            lblProgressGoal.Top = 370;
            lblProgressGoal.Width = 700;
            lblProgressGoal.Height = 25;
            this.Controls.Add(lblProgressGoal);

            AddSectionLabel("📘 Previous Completed Courses", 25, 425, Color.FromArgb(0, 80, 180));
            AddSectionLabel("✅ This Semester Recommendation", 480, 425, Color.Green);
            AddSectionLabel("📅 Expected Future Semester Plan", 935, 425, Color.DarkOrange);

            dgvPreviousCourses = MakeGrid(25, 465, 430, 500);
            dgvCurrentSemester = MakeGrid(480, 465, 430, 500);
            dgvFuturePlan = MakeGrid(935, 465, 430, 500);

            this.Controls.Add(dgvPreviousCourses);
            this.Controls.Add(dgvCurrentSemester);
            this.Controls.Add(dgvFuturePlan);

            lblPreviousFooter = MakeFooter(25, 975, 430);
            lblCurrentFooter = MakeFooter(480, 975, 430);
            lblFutureFooter = MakeFooter(935, 975, 430);

            this.Controls.Add(lblPreviousFooter);
            this.Controls.Add(lblCurrentFooter);
            this.Controls.Add(lblFutureFooter);

            btnExportPrevious = MakeExportButton(25, 1020, 430, "Export Completed Courses");
            btnExportPrevious.Click += (s, e) =>
                ExportGridToExcel(dgvPreviousCourses, "Completed_Courses");

            btnExportCurrent = MakeExportButton(480, 1020, 430, "Export Recommended Courses");
            btnExportCurrent.Click += (s, e) =>
                ExportGridToExcel(dgvCurrentSemester, "Recommended_Courses");

            btnExportFuture = MakeExportButton(935, 1020, 430, "Export Future Plan");
            btnExportFuture.Click += (s, e) =>
                ExportGridToExcel(dgvFuturePlan, "Future_Semester_Plan");

            this.Controls.Add(btnExportPrevious);
            this.Controls.Add(btnExportCurrent);
            this.Controls.Add(btnExportFuture);

            picCompletedChart = MakeChartBox(1400, 425, 430, 250);
            picFutureChart = MakeChartBox(1400, 700, 430, 250);

            this.Controls.Add(picCompletedChart);
            this.Controls.Add(picFutureChart);

            Button btnSave = new Button();
            btnSave.Text = "Save Summary";
            btnSave.Left = 1500;
            btnSave.Top = 975;
            btnSave.Width = 150;
            btnSave.Height = 38;
            btnSave.Font = new Font("Segoe UI", 10F);
            btnSave.Click += btnSaveSummary_Click;
            this.Controls.Add(btnSave);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Left = 1670;
            btnClose.Top = 975;
            btnClose.Width = 150;
            btnClose.Height = 38;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private Button MakeExportButton(int left, int top, int width, string text)
        {
            Button btn = new Button();

            btn.Left = left;
            btn.Top = top;
            btn.Width = width;
            btn.Height = 35;
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 10F);

            return btn;
        }

        private void ExportGridToExcel(DataGridView dgv, string baseFileName)
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to export.");
                return;
            }

            try
            {
                string defaultFolder =
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Workbook|*.xlsx";
                    sfd.Title = "Export to Excel";
                    sfd.InitialDirectory = defaultFolder;
                    sfd.FileName = Path.GetFileName(
                        GetNextAvailableFileName(defaultFolder, baseFileName, ".xlsx"));

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        DataTable dt = new DataTable();

                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            if (col.Visible)
                                dt.Columns.Add(col.HeaderText);
                        }

                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.IsNewRow)
                                continue;

                            DataRow dr = dt.NewRow();
                            int colIndex = 0;

                            foreach (DataGridViewColumn col in dgv.Columns)
                            {
                                if (!col.Visible)
                                    continue;

                                dr[colIndex] = row.Cells[col.Index].Value?.ToString() ?? "";
                                colIndex++;
                            }

                            dt.Rows.Add(dr);
                        }

                        var ws = wb.Worksheets.Add(dt, "Data");
                        ws.Row(1).Style.Font.Bold = true;
                        ws.Columns().AdjustToContents();

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show(
                        "Excel file exported successfully.",
                        "Export Completed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to export Excel file:\n" + ex.Message,
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /*        private void LoadGraduationSummary()
                {
                    int requiredCredits = GetRequiredCredits();
                    string programLevel = GetProgramLevel();

                    ParseAcademicYear(out string programLevelText, out string currentSemesterText);

                    List<CourseInfo> completedCourses = GetCompletedCourses();
                    List<CourseInfo> currentCourses = GetCurrentSemesterCourses();
                    List<CourseInfo> allRemainingCourses = GetRemainingCourses(completedCourses, currentCourses);

                    int completedCredits = completedCourses.Sum(c => c.Credits);
                    int currentCredits = currentCourses.Sum(c => c.Credits);
                    int afterThisSemester = completedCredits + currentCredits;

                    int remainingCredits = Math.Max(requiredCredits - afterThisSemester, 0);
                    int futureSemestersLeft = GetRemainingAcademicSemesters();

                    int semestersNeededByCredits = remainingCredits == 0
                        ? 0
                        : (int)Math.Ceiling(remainingCredits / (double)DEFAULT_FUTURE_SEMESTER_CREDITS);

                    int estimatedFutureSemesters = Math.Min(semestersNeededByCredits, futureSemestersLeft);

                    List<CourseInfo> futurePlanCourses = BuildFuturePlanCourses(
                        allRemainingCourses,
                        remainingCredits,
                        estimatedFutureSemesters);

                    double progressDouble = requiredCredits <= 0
                        ? 0
                        : afterThisSemester * 100.0 / requiredCredits;

                    if (progressDouble > 100)
                        progressDouble = 100;

                    int progress = Math.Min(100, (int)Math.Round(progressDouble));

                    string risk = GetGraduationRisk(remainingCredits, semestersNeededByCredits, futureSemestersLeft);

                    string studentName = Program.Page1.GetStudentName();
                    string studentId = Program.Page1.GetStudentId();
                    string studentDept = Program.Page1.GetStudentDepartment();
                    string studentGrade = Program.Page1.GetStudentGrade();

                    rtbStudentInfo.Clear();
                    AppendBold(rtbStudentInfo, "Student Information\n");
                    AppendNormal(rtbStudentInfo,
                        $"Name: {studentName}\n" +
                        $"Student ID: {studentId}\n" +
                        $"Department: {studentDept}\n" +
                        $"Grade: {studentGrade}");

                    rtbAcademicInfo.Clear();
                    rtbAcademicInfo.SelectionTabs = new int[] { 320, 650 };

                    AppendBold(rtbAcademicInfo, "Current Academic Status\n");
                    AppendNormal(rtbAcademicInfo,
                        $"Program Level: {programLevelText}\tCurrent Semester: {currentSemesterText}\n" +
                        $"Required Credits: {requiredCredits}\tCompleted Credits: {completedCredits} / {requiredCredits}\n\n");

                    AppendBold(rtbAcademicInfo, "This Semester Recommendation\n");
                    AppendNormal(rtbAcademicInfo,
                        $"Recommended Credits: {currentCredits}\tAfter This Semester: {afterThisSemester} / {requiredCredits}\n\n");

                    AppendBold(rtbAcademicInfo, "Remaining to Graduate\n");
                    AppendNormal(rtbAcademicInfo,
                        $"Remaining Credits: {remainingCredits}\tEstimated Future Semesters: {estimatedFutureSemesters}\tGraduation Risk: {risk}");

                    progressGraduation.Value = progress;

                    lblProgressPercent.Text =
                        $"{progressDouble:0.00}%\n({Math.Min(afterThisSemester, requiredCredits)} / {requiredCredits})";

                    lblProgressGoal.Text =
                        $"Goal: {requiredCredits} credits for {programLevel} graduation";

                    LoadPreviousCoursesGrid(completedCourses);
                    LoadCurrentSemesterGrid(currentCourses);
                    LoadFuturePlanGrid(futurePlanCourses, estimatedFutureSemesters, remainingCredits);
                    LoadPieCharts(completedCourses, futurePlanCourses);

                    lblPreviousFooter.Text =
                        $"Total Courses: {completedCourses.Count}        Total Credits: {completedCredits}";

                    lblCurrentFooter.Text =
                        $"Total Courses: {currentCourses.Count}        Total Credits: {currentCredits}";

                    lblFutureFooter.Text =
                        $"Estimated Future Semesters: {estimatedFutureSemesters}        Remaining Credits: {remainingCredits}";
                }*/


        private void LoadGraduationSummary()
        {
            // ============================================================
            // 1. Read graduation and academic information
            // ============================================================

            int requiredCredits = GetRequiredCredits();
            string programLevel = GetProgramLevel();

            ParseAcademicYear(
                out string programLevelText,
                out string currentSemesterText);

            List<CourseInfo> completedCourses =
                GetCompletedCourses();

            List<CourseInfo> currentCourses =
                GetCurrentSemesterCourses();

            List<CourseInfo> allRemainingCourses =
                GetRemainingCourses(
                    completedCourses,
                    currentCourses);

            // ============================================================
            // 2. Calculate current credit progress
            // ============================================================

            int completedCredits =
                completedCourses.Sum(course => course.Credits);

            int currentCredits =
                currentCourses.Sum(course => course.Credits);

            int afterThisSemester =
                completedCredits + currentCredits;

            int remainingCredits =
                Math.Max(
                    requiredCredits - afterThisSemester,
                    0);

            int futureSemestersLeft =
                GetRemainingAcademicSemesters();

            // ============================================================
            // 3. Run the Python + Gurobi future-planning solver
            // ============================================================

            List<CourseInfo> futurePlanCourses =
                new List<CourseInfo>();

            int estimatedFutureSemesters = 0;
            FuturePlanningResult gurobiResult = null;

            if (remainingCredits > 0 &&
                allRemainingCourses != null &&
                allRemainingCourses.Count > 0)
            {
                try
                {
                    FuturePlanningInput solverInput =
                        BuildFuturePlanningInput(
                            completedCourses,
                            currentCourses,
                            allRemainingCourses,
                            requiredCredits,
                            completedCredits,
                            currentCredits);

                    gurobiResult =
                        FuturePlanningSolverService.Solve(
                            solverInput);

                    if (gurobiResult != null &&
                        string.Equals(
                            gurobiResult.status,
                            "OPTIMAL",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        futurePlanCourses =
                            ConvertFuturePlanningResult(
                                gurobiResult);

                        estimatedFutureSemesters =
                            gurobiResult.estimated_future_semesters;
                    }
                    else
                    {
                        string status =
                            gurobiResult?.status ?? "UNKNOWN";

                        string message =
                            gurobiResult?.message ??
                            "The solver did not return a result.";

                        MessageBox.Show(
                            "Future planning could not be completed.\n\n" +
                            "Status: " + status + "\n" +
                            "Message: " + message,
                            "Future Planning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "An error occurred while running the " +
                        "future graduation solver.\n\n" +
                        ex.Message,
                        "Future Planning Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            // ============================================================
            // 4. Calculate graduation progress and risk
            // ============================================================

            double progressDouble = requiredCredits <= 0
                ? 0
                : afterThisSemester * 100.0 / requiredCredits;

            progressDouble =
                Math.Max(
                    0,
                    Math.Min(progressDouble, 100));

            int progress =
                (int)Math.Round(progressDouble);

            string risk =
                GetGraduationRisk(
                    remainingCredits,
                    estimatedFutureSemesters,
                    futureSemestersLeft);

            // ============================================================
            // 5. Read student information
            // ============================================================

            string studentName =
                Program.Page1.GetStudentName();

            string studentId =
                Program.Page1.GetStudentId();

            string studentDept =
                Program.Page1.GetStudentDepartment();

            string studentGrade =
                Program.Page1.GetStudentGrade();

            // ============================================================
            // 6. Display student information
            // ============================================================

            rtbStudentInfo.Clear();

            AppendBold(
                rtbStudentInfo,
                "Student Information\n");

            AppendNormal(
                rtbStudentInfo,
                $"Name: {studentName}\n" +
                $"Student ID: {studentId}\n" +
                $"Department: {studentDept}\n" +
                $"Grade: {studentGrade}");

            // ============================================================
            // 7. Display academic information
            // ============================================================

            rtbAcademicInfo.Clear();
            rtbAcademicInfo.SelectionTabs =
                new int[] { 320, 650 };

            AppendBold(
                rtbAcademicInfo,
                "Current Academic Status\n");

            AppendNormal(
                rtbAcademicInfo,
                $"Program Level: {programLevelText}\t" +
                $"Current Semester: {currentSemesterText}\n" +
                $"Required Credits: {requiredCredits}\t" +
                $"Completed Credits: {completedCredits} / " +
                $"{requiredCredits}\n\n");

            AppendBold(
                rtbAcademicInfo,
                "This Semester Recommendation\n");

            AppendNormal(
                rtbAcademicInfo,
                $"Recommended Credits: {currentCredits}\t" +
                $"After This Semester: {afterThisSemester} / " +
                $"{requiredCredits}\n\n");

            AppendBold(
                rtbAcademicInfo,
                "Remaining to Graduate\n");

            AppendNormal(
                rtbAcademicInfo,
                $"Remaining Credits: {remainingCredits}\t" +
                $"Estimated Future Semesters: " +
                $"{estimatedFutureSemesters}\t" +
                $"Graduation Risk: {risk}");

            // ============================================================
            // 8. Update graduation progress controls
            // ============================================================

            progressGraduation.Value =
                Math.Max(
                    progressGraduation.Minimum,
                    Math.Min(
                        progress,
                        progressGraduation.Maximum));

            lblProgressPercent.Text =
                $"{progressDouble:0.00}%\n" +
                $"({Math.Min(afterThisSemester, requiredCredits)} " +
                $"/ {requiredCredits})";

            lblProgressGoal.Text =
                $"Goal: {requiredCredits} credits for " +
                $"{programLevel} graduation";

            // ============================================================
            // 9. Load tables and charts
            // ============================================================

            LoadPreviousCoursesGrid(
                completedCourses);

            LoadCurrentSemesterGrid(
                currentCourses);

            LoadFuturePlanGrid(
                futurePlanCourses,
                estimatedFutureSemesters,
                remainingCredits);

            LoadPieCharts(
                completedCourses,
                futurePlanCourses);

            // ============================================================
            // 10. Update table footers
            // ============================================================

            lblPreviousFooter.Text =
                $"Total Courses: {completedCourses.Count}        " +
                $"Total Credits: {completedCredits}";

            lblCurrentFooter.Text =
                $"Total Courses: {currentCourses.Count}        " +
                $"Total Credits: {currentCredits}";

            int selectedFutureCredits =
                gurobiResult?.selected_future_credits ??
                futurePlanCourses.Sum(course => course.Credits);

            lblFutureFooter.Text =
                $"Estimated Future Semesters: " +
                $"{estimatedFutureSemesters}        " +
                $"Selected Future Credits: " +
                $"{selectedFutureCredits}        " +
                $"Remaining Credits: {remainingCredits}";
        }

        private void LoadFuturePlanGrid(List<CourseInfo> courses,int futureSemesters,int remainingCredits)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Expected");
            dt.Columns.Add("Course ID");
            dt.Columns.Add("Course Name");
            dt.Columns.Add("Credits");

            if (remainingCredits <= 0 ||
                futureSemesters <= 0 ||
                courses == null ||
                courses.Count == 0)
            {
                dgvFuturePlan.DataSource = dt;
                return;
            }

            foreach (CourseInfo course in courses
                .Where(c => c.ExpectedSemester > 0)
                .OrderBy(c => c.ExpectedSemester)
                .ThenBy(c => c.CourseID))
            {
                dt.Rows.Add(
                    "Future Semester " + course.ExpectedSemester,
                    course.CourseID,
                    course.CourseName,
                    course.Credits);
            }

            dgvFuturePlan.DataSource = dt;
        }



        private List<CourseInfo> BuildFuturePlanCourses(List<CourseInfo> allRemainingCourses, int remainingCredits, int maxFutureSemesters)
        {
            List<CourseInfo> result = new List<CourseInfo>();

            if (remainingCredits <= 0 || maxFutureSemesters <= 0)
                return result;

            int addedCredits = 0;
            int semesterNo = 1;
            int semesterCredits = 0;

            foreach (CourseInfo c in allRemainingCourses)
            {
                if (addedCredits >= remainingCredits)
                    break;

                if (semesterCredits + c.Credits > DEFAULT_FUTURE_SEMESTER_CREDITS)
                {
                    semesterNo++;
                    semesterCredits = 0;
                }

                if (semesterNo > maxFutureSemesters)
                    break;

                result.Add(c);
                addedCredits += c.Credits;
                semesterCredits += c.Credits;
            }

            return result;
        }

        private int GetRemainingAcademicSemesters()
        {
            string s = (_academicYear ?? "").ToLowerInvariant();

            if (s.Contains("bachelor"))
            {
                if (s.Contains("1st year") && s.Contains("semester 1")) return 7;
                if (s.Contains("1st year") && s.Contains("semester 2")) return 6;
                if (s.Contains("2nd year") && s.Contains("semester 1")) return 5;
                if (s.Contains("2nd year") && s.Contains("semester 2")) return 4;
                if (s.Contains("3rd year") && s.Contains("semester 1")) return 3;
                if (s.Contains("3rd year") && s.Contains("semester 2")) return 2;
                if (s.Contains("4th year") && s.Contains("semester 1")) return 1;
                if (s.Contains("4th year") && s.Contains("semester 2")) return 0;
            }

            if (s.Contains("master"))
            {
                if (s.Contains("1st year") && s.Contains("semester 1")) return 3;
                if (s.Contains("1st year") && s.Contains("semester 2")) return 2;
                if (s.Contains("2nd year") && s.Contains("semester 1")) return 1;
                if (s.Contains("2nd year") && s.Contains("semester 2")) return 0;
            }

            if (s.Contains("phd"))
            {
                if (s.Contains("1st year") && s.Contains("semester 1")) return 7;
                if (s.Contains("1st year") && s.Contains("semester 2")) return 6;
                if (s.Contains("2nd year") && s.Contains("semester 1")) return 5;
                if (s.Contains("2nd year") && s.Contains("semester 2")) return 4;
                if (s.Contains("3rd year") && s.Contains("semester 1")) return 3;
                if (s.Contains("3rd year") && s.Contains("semester 2")) return 2;
                if (s.Contains("4th year") && s.Contains("semester 1")) return 1;
                if (s.Contains("4th year") && s.Contains("semester 2")) return 0;
            }

            return 0;
        }

        private string GetGraduationRisk(int remainingCredits, int semestersNeededByCredits, int futureSemestersLeft)
        {
            if (remainingCredits == 0)
                return "Low";

            if (futureSemestersLeft == 0)
                return "High";

            if (semestersNeededByCredits <= futureSemestersLeft)
                return "Low";

            return "High";
        }

        private void ParseAcademicYear(out string programLevelText, out string currentSemesterText)
        {
            string s = _academicYear ?? "";
            string[] parts = s.Split(',');

            programLevelText = parts.Length > 0 ? parts[0].Trim() : s;
            currentSemesterText = parts.Length > 1 ? parts[1].Trim() : "";
        }

        private RichTextBox CreateInfoBox(int left, int top, int width, int height)
        {
            RichTextBox rtb = new RichTextBox();

            rtb.Left = left;
            rtb.Top = top;
            rtb.Width = width;
            rtb.Height = height;
            rtb.BorderStyle = BorderStyle.None;
            rtb.BackColor = Color.White;
            rtb.ReadOnly = true;
            rtb.ScrollBars = RichTextBoxScrollBars.None;
            rtb.TabStop = false;
            rtb.Font = new Font("Segoe UI", 10F);
            rtb.Cursor = Cursors.Arrow;
            rtb.HideSelection = true;
            rtb.GotFocus += (s, e) => this.ActiveControl = null;

            return rtb;
        }

        private void AppendBold(RichTextBox rtb, string text)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            rtb.AppendText(text);
        }

        private void AppendNormal(RichTextBox rtb, string text)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            rtb.AppendText(text);
        }

        private void AddSectionLabel(string text, int left, int top, Color color)
        {
            Label lbl = new Label();

            lbl.Text = text;
            lbl.Left = left;
            lbl.Top = top;
            lbl.Width = 430;
            lbl.Height = 30;
            lbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbl.ForeColor = color;

            this.Controls.Add(lbl);
        }

        private DataGridView MakeGrid(int left, int top, int width, int height)
        {
            DataGridView dgv = new DataGridView();

            dgv.Left = left;
            dgv.Top = top;
            dgv.Width = width;
            dgv.Height = height;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 250);
            dgv.BackgroundColor = Color.White;

            return dgv;
        }

        private Label MakeFooter(int left, int top, int width)
        {
            Label lbl = new Label();

            lbl.Left = left;
            lbl.Top = top;
            lbl.Width = width;
            lbl.Height = 35;
            lbl.BackColor = Color.FromArgb(235, 245, 235);
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lbl.Padding = new Padding(10, 7, 10, 5);
            lbl.BorderStyle = BorderStyle.FixedSingle;

            return lbl;
        }

        private PictureBox MakeChartBox(int left, int top, int width, int height)
        {
            PictureBox pic = new PictureBox();

            pic.Left = left;
            pic.Top = top;
            pic.Width = width;
            pic.Height = height;
            pic.BackColor = Color.White;
            pic.BorderStyle = BorderStyle.FixedSingle;

            return pic;
        }

        private void LoadPieCharts(List<CourseInfo> completedCourses, List<CourseInfo> futurePlanCourses)
        {
            Dictionary<string, int> completedByCategory = completedCourses
                .GroupBy(c => string.IsNullOrWhiteSpace(c.Category) ? "Unknown" : c.Category)
                .ToDictionary(g => g.Key, g => g.Sum(c => c.Credits));

            Dictionary<string, int> futureSemesterCredits = BuildFutureSemesterCredits(futurePlanCourses);

            DrawPieChart(picCompletedChart, completedByCategory, "Completed Credits by Category");
            DrawPieChart(picFutureChart, futureSemesterCredits, "Planned Credits by Future Semester");
        }

        private List<CourseInfo> ConvertFuturePlanningResult(FuturePlanningResult result)
        {
            if (result == null || result.future_plan == null)
                return new List<CourseInfo>();

            return result.future_plan
                .Select(course => new CourseInfo
                {
                    CourseID = course.course_id,
                    CourseName = course.course_name,
                    Credits = course.credits,
                    Category = course.category,
                    ExpectedSemester = course.expected_semester
                })
                .OrderBy(course => course.ExpectedSemester)
                .ThenBy(course => course.CourseID)
                .ToList();
        }

        private int GetProgramSemesterLimit()
        {
            string s = (_academicYear ?? "").ToLowerInvariant();

            if (s.Contains("bachelor"))
                return 8;

            if (s.Contains("master"))
                return 4;

            if (s.Contains("phd") || s.Contains("doctor"))
                return 8;

            return 8;
        }

        private int GetUsedAcademicSemesters()
        {
            string s = (_academicYear ?? "").ToLowerInvariant();

            if (s.Contains("1st year") &&
                s.Contains("semester 1"))
                return 1;

            if (s.Contains("1st year") &&
                s.Contains("semester 2"))
                return 2;

            if (s.Contains("2nd year") &&
                s.Contains("semester 1"))
                return 3;

            if (s.Contains("2nd year") &&
                s.Contains("semester 2"))
                return 4;

            if (s.Contains("3rd year") &&
                s.Contains("semester 1"))
                return 5;

            if (s.Contains("3rd year") &&
                s.Contains("semester 2"))
                return 6;

            if (s.Contains("4th year") &&
                s.Contains("semester 1"))
                return 7;

            if (s.Contains("4th year") &&
                s.Contains("semester 2"))
                return 8;

            return 0;
        }

        private List<string> GetFutureCoursePrerequisites(string normalizedCourseId)
        {
            switch (normalizedCourseId)
            {
                // ====================================================
                // Taiwanese student: English path
                // ====================================================

                // English I -> corresponding English II level
                case "LC201":
                    return new List<string> { "LC101" };

                case "LC202":
                    return new List<string> { "LC102" };

                case "LC203":
                    return new List<string> { "LC103" };

                // EL360 may have separate registration conditions.
                // Do not create an academic prerequisite here unless
                // your official curriculum data gives one.
                case "EL360":
                    return new List<string>();

                // ====================================================
                // International student: Mandarin path
                // ====================================================

                case "EL171":
                    return new List<string> { "EL170" };

                case "EL270":
                    return new List<string>
            {
                "EL170",
                "EL171"
            };

                case "EL271":
                    return new List<string>
            {
                "EL170",
                "EL171"
            };

                case "EL370":
                    return new List<string>
            {
                "EL270",
                "EL271"
            };

                case "EL372":
                    return new List<string>
            {
                "EL270",
                "EL271"
            };

                // ====================================================
                // Practical Training path
                // ====================================================

                case "IN422":
                    return new List<string> { "IN421" };

                case "IN423":
                    return new List<string> { "IN422" };

                case "IN424":
                    return new List<string> { "IN423" };

                case "IN425":
                    return new List<string> { "IN424" };

                case "IN426":
                    return new List<string> { "IN425" };

                // ====================================================
                // Special Project path
                // ====================================================

                case "IN432":
                    return new List<string> { "IN431" };

                default:
                    return new List<string>();
            }
        }

        private bool IsIndividualGraduationActivityCourse(string normalizedCourseId)
        {
            return normalizedCourseId == "IN421" ||
                   normalizedCourseId == "IN422" ||
                   normalizedCourseId == "IN423" ||
                   normalizedCourseId == "IN424" ||
                   normalizedCourseId == "IN425" ||
                   normalizedCourseId == "IN426" ||
                   normalizedCourseId == "IN431" ||
                   normalizedCourseId == "IN432";
        }

        private string NormalizeCourseId(string courseId)
        {
            if (string.IsNullOrWhiteSpace(courseId))
                return string.Empty;

            return courseId
                .Replace(" ", "")
                .Trim()
                .ToUpperInvariant();
        }

        private FuturePlanningInput BuildFuturePlanningInput(
                List<CourseInfo> completedCourses,
                List<CourseInfo> currentCourses,
                List<CourseInfo> remainingCourses,
                int requiredCredits,
                int completedCredits,
                int currentCredits)
        {
            int programSemesters = GetProgramSemesterLimit();
            int usedSemesters = GetUsedAcademicSemesters();
            int futureSemesters =
                Math.Max(programSemesters - usedSemesters, 0);

            LanguageCoursePath languagePath = DetectLanguageCoursePath(completedCourses,currentCourses,remainingCourses);

            FuturePlanningInput input = new FuturePlanningInput
            {
                required_credits = requiredCredits,
                completed_credits = completedCredits,
                current_credits = currentCredits,

                max_credits_per_semester =
                    DEFAULT_FUTURE_SEMESTER_CREDITS,

                program_semesters = programSemesters,
                used_semesters = usedSemesters,

                show_gurobi_log = 0,

                completed_course_ids = completedCourses
                    .Select(c => c.CourseID)
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .ToList(),

                current_course_ids = currentCourses
                    .Select(c => c.CourseID)
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .ToList(),

                courses = new List<FuturePlanningInputCourse>()
            };


            bool hasUnresolvedFirstActivity = currentCourses.Any(course => NormalizeCourseId(course.CourseID) == "IN421/IN431");


            foreach (CourseInfo course in remainingCourses)
            {
                string courseId =
                    NormalizeCourseId(course.CourseID);

                // Do not send Mandarin-path courses for an English-path student.
                if (languagePath == LanguageCoursePath.English &&
                    IsMandarinPathCourse(courseId))
                {
                    continue;
                }

                // Do not send English-path courses for a Mandarin-path student.
                if (languagePath == LanguageCoursePath.Mandarin &&
                    IsEnglishPathCourse(courseId))
                {
                    continue;
                }

                if (hasUnresolvedFirstActivity &&
                    IsIndividualGraduationActivityCourse(courseId))
                {
                    continue;
                }

                input.courses.Add(
                    new FuturePlanningInputCourse
                    {
                        course_id = course.CourseID,
                        course_name = course.CourseName,
                        credits = course.Credits,
                        category = course.Category,

                        earliest_semester = 1,

                        offered_semesters = Enumerable
                            .Range(1, futureSemesters)
                            .ToList(),

                        prerequisites =
                            GetFutureCoursePrerequisites(courseId),

                        mandatory =
                                IsMandatoryLanguageCompletionCourse(
                                    languagePath,
                                    courseId,
                                    completedCourses,
                                    currentCourses)
                    });
            }

            if (hasUnresolvedFirstActivity)
            {
                input.courses.Add(
                    new FuturePlanningInputCourse
                    {
                        course_id = "IN422 / IN432",

                        course_name =
                            "Practical Training (II) / Special Project (II)",

                        credits = 3,

                        category =
                            "Alternative Graduation Activity",

                        earliest_semester = 1,

                        offered_semesters = Enumerable
                            .Range(1, futureSemesters)
                            .ToList(),

                        prerequisites = new List<string>
                        {
                "IN421 / IN431"
                        },

                        mandatory = true
                    });
            }




            return input;
        }


        /*      private Dictionary<string, int> BuildFutureSemesterCredits(List<CourseInfo> courses)
              {
                  Dictionary<string, int> result = new Dictionary<string, int>();

                  if (courses == null || courses.Count == 0)
                      return result;

                  int semesterNo = 1;
                  int semesterCredits = 0;

                  foreach (CourseInfo c in courses)
                  {
                      if (semesterCredits + c.Credits > DEFAULT_FUTURE_SEMESTER_CREDITS)
                      {
                          semesterNo++;
                          semesterCredits = 0;
                      }

                      string semesterName = "Future Semester " + semesterNo;

                      if (!result.ContainsKey(semesterName))
                          result[semesterName] = 0;

                      result[semesterName] += c.Credits;
                      semesterCredits += c.Credits;
                  }

                  return result;
              }*/

        private Dictionary<string, int> BuildFutureSemesterCredits(List<CourseInfo> courses)
        {
            if (courses == null || courses.Count == 0)
                return new Dictionary<string, int>();

            return courses
                .Where(c => c.ExpectedSemester > 0)
                .GroupBy(c => c.ExpectedSemester)
                .OrderBy(group => group.Key)
                .ToDictionary(
                    group =>
                        "Future Semester " + group.Key,
                    group =>
                        group.Sum(c => c.Credits));
        }

        private void DrawPieChart(PictureBox pic, Dictionary<string, int> data, string title)
        {
            Bitmap bmp = new Bitmap(pic.Width, pic.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Font titleFont = new Font("Segoe UI", 11F, FontStyle.Bold);
                Font labelFont = new Font("Segoe UI", 8F);

                g.DrawString(title, titleFont, Brushes.Black, 10, 8);

                if (data == null || data.Count == 0 || data.Sum(x => x.Value) == 0)
                {
                    g.DrawString("No future semester plan needed", labelFont, Brushes.Gray, 20, 60);
                    pic.Image = bmp;
                    return;
                }

                int total = data.Sum(x => x.Value);
                Rectangle pieRect = new Rectangle(25, 50, 165, 165);

                Color[] colors =
                {
                    Color.SteelBlue,
                    Color.Orange,
                    Color.SeaGreen,
                    Color.MediumPurple,
                    Color.IndianRed,
                    Color.Goldenrod,
                    Color.Teal,
                    Color.SlateGray
                };

                float startAngle = 0;
                int i = 0;
                int legendX = 215;
                int legendY = 55;

                foreach (var item in data)
                {
                    float sweepAngle = item.Value * 360f / total;

                    using (Brush b = new SolidBrush(colors[i % colors.Length]))
                    {
                        g.FillPie(b, pieRect, startAngle, sweepAngle);
                        g.FillRectangle(b, legendX, legendY + i * 25, 14, 14);
                    }

                    g.DrawPie(Pens.White, pieRect, startAngle, sweepAngle);
                    g.DrawString($"{item.Key}: {item.Value} credits", labelFont, Brushes.Black, legendX + 20, legendY + i * 25 - 2);

                    startAngle += sweepAngle;
                    i++;
                }
            }

            pic.Image = bmp;
        }

        private List<CourseInfo> GetCompletedCourses()
        {
            List<CourseInfo> result = new List<CourseInfo>();

            if (_form2 == null)
                return result;

            DataTable dt = _form2.GetStep5Table();

            if (dt == null)
                return result;

            foreach (DataRow row in dt.Rows)
            {
                string passed = row["Check passed"]?.ToString()?.Trim() ?? "";

                if (passed != "✓")
                    continue;

                result.Add(new CourseInfo
                {
                    CourseID = row["Course ID"]?.ToString() ?? "",
                    CourseName = row["English Name Course"]?.ToString() ?? "",
                    Credits = ParseInt(row["Credits"]?.ToString() ?? ""),
                    Category = row["Category"]?.ToString() ?? ""
                });
            }

            return result;
        }

        private List<CourseInfo> GetCurrentSemesterCourses()
        {
            List<CourseInfo> result = new List<CourseInfo>();

            if (_solverResult == null || _solverResult.selected_courses == null)
                return result;

            foreach (var c in _solverResult.selected_courses)
            {
                result.Add(new CourseInfo
                {
                    CourseID = c.course_id,
                    CourseName = c.english_course_name,
                    Credits = c.credits,
                    Category = c.type
                });
            }

            return result;
        }

        private List<CourseInfo> GetRemainingCourses(List<CourseInfo> completed, List<CourseInfo> current)
        {
            List<CourseInfo> result = new List<CourseInfo>();

            if (_form2 == null)
                return result;

            DataTable dt = _form2.GetStep5Table();

            if (dt == null)
                return result;

            HashSet<string> completedIds = new HashSet<string>(
                completed.Select(c => c.CourseID),
                StringComparer.OrdinalIgnoreCase);

            HashSet<string> currentIds = new HashSet<string>(
                current.Select(c => c.CourseID),
                StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in dt.Rows)
            {
                string cid = row["Course ID"]?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(cid))
                    continue;

                if (completedIds.Contains(cid))
                    continue;

                if (currentIds.Contains(cid))
                    continue;

                result.Add(new CourseInfo
                {
                    CourseID = cid,
                    CourseName = row["English Name Course"]?.ToString() ?? "",
                    Credits = ParseInt(row["Credits"]?.ToString() ?? ""),
                    Category = row["Category"]?.ToString() ?? ""
                });
            }

            return result;
        }

        private void LoadPreviousCoursesGrid(List<CourseInfo> courses)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Course ID");
            dt.Columns.Add("Course Name");
            dt.Columns.Add("Credits");
            dt.Columns.Add("Category");

            foreach (var c in courses)
                dt.Rows.Add(c.CourseID, c.CourseName, c.Credits, c.Category);

            dgvPreviousCourses.DataSource = dt;
        }

        private void LoadCurrentSemesterGrid(List<CourseInfo> courses)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Course ID");
            dt.Columns.Add("Course Name");
            dt.Columns.Add("Credits");
            dt.Columns.Add("Category");

            foreach (var c in courses)
                dt.Rows.Add(c.CourseID, c.CourseName, c.Credits, c.Category);

            dgvCurrentSemester.DataSource = dt;
        }


        private int GetRequiredCredits()
        {
            string s = (_academicYear ?? "").ToLowerInvariant();

            if (s.Contains("bachelor"))
                return 128;

            if (s.Contains("master"))
                return 32;

            if (s.Contains("phd") || s.Contains("doctor"))
                return 30;

            return 128;
        }

        private string GetProgramLevel()
        {
            string s = (_academicYear ?? "").ToLowerInvariant();

            if (s.Contains("bachelor"))
                return "Bachelor";

            if (s.Contains("master"))
                return "Master";

            if (s.Contains("phd") || s.Contains("doctor"))
                return "Doctoral";

            return "Bachelor";
        }


        private enum LanguageCoursePath
        {
            Unknown,
            English,
            Mandarin
        }

        private bool HasCourse(
            List<CourseInfo> completedCourses,
            List<CourseInfo> currentCourses,
            string courseId)
        {
            string id = NormalizeCourseId(courseId);

            return completedCourses.Any(c =>
                        NormalizeCourseId(c.CourseID) == id)
                ||
                   currentCourses.Any(c =>
                        NormalizeCourseId(c.CourseID) == id);
        }

        private LanguageCoursePath DetectLanguageCoursePath(
            List<CourseInfo> completedCourses,
            List<CourseInfo> currentCourses,
            List<CourseInfo> remainingCourses)
        {
            List<CourseInfo> allCourses = new List<CourseInfo>();

            if (completedCourses != null)
                allCourses.AddRange(completedCourses);

            if (currentCourses != null)
                allCourses.AddRange(currentCourses);

            if (remainingCourses != null)
                allCourses.AddRange(remainingCourses);

            bool hasMandarinCourse = allCourses.Any(course =>
            {
                string id = NormalizeCourseId(course.CourseID);
                string name = course.CourseName ?? string.Empty;

                return id == "EL170" ||
                       id == "EL171" ||
                       id == "EL270" ||
                       id == "EL271" ||
                       id == "EL370" ||
                       id == "EL372" ||
                       name.IndexOf(
                           "Mandarin Chinese",
                           StringComparison.OrdinalIgnoreCase) >= 0 ||
                       name.IndexOf(
                           "Chinese Proficiency Test",
                           StringComparison.OrdinalIgnoreCase) >= 0;
            });

            if (hasMandarinCourse)
                return LanguageCoursePath.Mandarin;

            bool hasEnglishCourse = allCourses.Any(course =>
            {
                string id = NormalizeCourseId(course.CourseID);
                string name = course.CourseName ?? string.Empty;

                return id == "LC101" ||
                       id == "LC102" ||
                       id == "LC103" ||
                       id == "LC201" ||
                       id == "LC202" ||
                       id == "LC203" ||
                       id == "EL360" ||
                       name.IndexOf(
                           "Freshmen English",
                           StringComparison.OrdinalIgnoreCase) >= 0 ||
                       name.IndexOf(
                           "English Testing",
                           StringComparison.OrdinalIgnoreCase) >= 0;
            });

            if (hasEnglishCourse)
                return LanguageCoursePath.English;

            return LanguageCoursePath.Unknown;
        }


        private bool IsMandatoryLanguageCompletionCourse(
            LanguageCoursePath languagePath,
            string normalizedCourseId,
            List<CourseInfo> completedCourses,
            List<CourseInfo> currentCourses)
        {
            if (languagePath != LanguageCoursePath.Mandarin)
                return false;

            bool has170 = HasCourse(completedCourses, currentCourses, "EL170");
            bool has171 = HasCourse(completedCourses, currentCourses, "EL171");
            bool has270 = HasCourse(completedCourses, currentCourses, "EL270");
            bool has271 = HasCourse(completedCourses, currentCourses, "EL271");
            bool has370 = HasCourse(completedCourses, currentCourses, "EL370");
            bool has372 = HasCourse(completedCourses, currentCourses, "EL372");

            switch (normalizedCourseId)
            {
                case "EL170":
                    return false;

                case "EL171":
                    return has170 && !has171;

                case "EL270":
                    return has171 && !has270;

                case "EL271":
                    return has270 && !has271;

                case "EL370":
                    return has271 && !has370;

                case "EL372":
                    return has271 && !has372;

                default:
                    return false;
            }
        }

        private bool IsEnglishPathCourse(string normalizedCourseId)
        {
            return normalizedCourseId == "LC101" ||
                   normalizedCourseId == "LC102" ||
                   normalizedCourseId == "LC103" ||
                   normalizedCourseId == "LC201" ||
                   normalizedCourseId == "LC202" ||
                   normalizedCourseId == "LC203" ||
                   normalizedCourseId == "EL360";
        }

        private bool IsMandarinPathCourse(string normalizedCourseId)
        {
            return normalizedCourseId == "EL170" ||
                   normalizedCourseId == "EL171" ||
                   normalizedCourseId == "EL270" ||
                   normalizedCourseId == "EL271" ||
                   normalizedCourseId == "EL370" ||
                   normalizedCourseId == "EL372";
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

        private void btnSaveSummary_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    string defaultFolder =
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";

                    sfd.Filter = "PNG Image|*.png";
                    sfd.Title = "Save Graduation Planning Summary";
                    sfd.InitialDirectory = defaultFolder;
                    sfd.FileName = Path.GetFileName(
                        GetNextAvailableFileName(
                            defaultFolder,
                            "Graduation_Planning_Summary",
                            ".png"));

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        SaveFormAsImage(sfd.FileName);

                        MessageBox.Show(
                            "Graduation planning summary saved successfully.",
                            "Saved",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to save summary:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SaveFormAsImage(string filePath)
        {
            this.Refresh();

            using (Bitmap bmp = new Bitmap(this.Width, this.Height))
            {
                this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private int ParseInt(string s)
        {
            int value;

            if (int.TryParse(s, out value))
                return value;

            return 0;
        }

        private class CourseInfo
        {
            public string CourseID { get; set; }
            public string CourseName { get; set; }
            public int Credits { get; set; }
            public string Category { get; set; }

            public int ExpectedSemester { get; set; }
        }
    }
}