using ClosedXML.Excel;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UglyToad.PdfPig;
using HAP = HtmlAgilityPack;

namespace ThesisSchedulerGUI
{
    public partial class Form2 : Form
    {
        // =========================================================
        // Step 6 — Current Semester Timetable Of Department
        // =========================================================

        // private readonly string BASE_DIR =
        //     @"C:\Users\User\Desktop\ThesisSchedulerGUI\Department Timetable";

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

        private DataTable _masterTable;
        private readonly HashSet<string> _loadedDepts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // =========================================================
        // Step 5 — Completed Course Record
        // =========================================================

        private DataTable _step5Table;

        private class PassedCourseRow
        {
            public string CourseID { get; set; }
            public string Grade { get; set; }   // PASS or #
            public string EnglishNameCourse { get; set; }   // for now store Chinese name here
            public string Credits { get; set; }
            public string Category { get; set; }

            public string RawBlock { get; set; }
        }

        public Form2()
        {
            InitializeComponent();

            btnExportStep5Excel.Enabled = false;

            this.Load += Form2_Load;
            btnUploadFileStep5.Click += btnUploadFileStep5_Click;

            btnAddCoursePreferences.Enabled = false;
            this.AutoScaleMode = AutoScaleMode.None;
        }

        // =========================================================
        // Required stubs
        // =========================================================

        private void Form2_FormClosing(object sender, FormClosingEventArgs e) {
            Application.Exit();
        }
        private void dataGridView2Page2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void UpdateAddCoursePreferencesButton()
        {
            bool step5Ready = dataGridViewStep5.Rows.Count > 0 &&
                              dataGridViewStep5.Rows.Cast<DataGridViewRow>()
                                  .Any(r => !r.IsNewRow);

            bool step6Ready = dataGridView2Page2.Rows.Count > 0 &&
                              dataGridView2Page2.Rows.Cast<DataGridViewRow>()
                                  .Any(r => !r.IsNewRow);

            btnAddCoursePreferences.Enabled = step5Ready && step6Ready;
        }

        // =========================================================
        // Load
        // =========================================================

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBoxDept.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBoxDept.BeginUpdate();
            comboBoxDept.Items.Clear();
            comboBoxDept.Items.AddRange(DeptToExactFile.Keys.ToArray());
            comboBoxDept.EndUpdate();

            if (comboBoxDept.Items.Count > 0)
                comboBoxDept.SelectedIndex = 0;

            // Step 5
            if (_step5Table == null)
            {
                _step5Table = CreateStep5Table();
                dataGridViewStep5.AutoGenerateColumns = true;
                dataGridViewStep5.DataSource = _step5Table;
                dataGridViewStep5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }

            // Step 6
            _masterTable = CreateOutputTable();

            dataGridView2Page2.AutoGenerateColumns = true;
            dataGridView2Page2.DataSource = _masterTable;
            dataGridView2Page2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView2Page2.DataBindingComplete += (s, ev) =>
            {
                if (dataGridView2Page2.Columns.Contains("Teaching Language"))
                {
                    var col = dataGridView2Page2.Columns["Teaching Language"];
                    col.CellTemplate = new EnglishRedTextCell();
                }
            };

            dataGridView2Page2.CellFormatting += dataGridView2Page2_CellFormatting;
            UpdateAddCoursePreferencesButton();

        }

        // =========================================================
        // Step 5 — Table
        // =========================================================

        private DataTable CreateStep5Table()
        {
            var dt = new DataTable();

            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Course ID");
            dt.Columns.Add("Credits");
            dt.Columns.Add("English Name Course");
            dt.Columns.Add("Category");
            dt.Columns.Add("Check passed");

            return dt;
        }

        public void LoadStep5CoursesFromForm1(List<Form1.CourseRow> rows)
        {
            _step5Table = CreateStep5Table();

            if (rows != null)
            {
                int stt = 1;
                foreach (var r in rows)
                {
                    var nr = _step5Table.NewRow();
                    nr["STT"] = stt++;
                    nr["Course ID"] = r.CourseID ?? "";
                    nr["Credits"] = r.Credits ?? "";
                    nr["English Name Course"] = r.EnglishNameCourse ?? "";
                    nr["Category"] = r.Category ?? "";
                    nr["Check passed"] = "";
                    _step5Table.Rows.Add(nr);
                }
            }

            dataGridViewStep5.AutoGenerateColumns = true;
            dataGridViewStep5.DataSource = _step5Table;
            dataGridViewStep5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btnUploadFileStep5_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = Color.Black;
            lblStatus.Text = "Calculating Total Available Credits";

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Upload Completed Course Record";
                ofd.Filter = "PDF Files|*.pdf|All Files|*.*";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    var passedRows = ParsePassedCourseFileAsSimpleRows(ofd.FileName);
                    ApplyPassedCoursesToStep5(passedRows);

                    MessageBox.Show(
                        "Step 5 completed successfully.\n\nParsed rows: " + passedRows.Count,
                        "Step 5",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Failed to process passed-course file:\n" + ex.Message,
                        "Step 5 Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            if (dataGridViewStep5.Rows.Count > 0)
            {
                btnExportStep5Excel.Enabled = true;
            }
        }

        private List<PassedCourseRow> ParsePassedCourseFileAsSimpleRows(string filePath)
        {
            var result = new List<PassedCourseRow>();

            string fullText = "";

            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    fullText += page.Text + "\n";
                }
            }

            if (string.IsNullOrWhiteSpace(fullText))
                return result;

            // normalize
            fullText = fullText.Replace("\u00A0", " ");
            fullText = fullText.Replace("\r", " ");
            fullText = fullText.Replace("\n", " ");
            fullText = Regex.Replace(fullText, @"\s+", " ").Trim();

            // cut away explanation section
            int cut = fullText.IndexOf("審查未通過原因說明", StringComparison.Ordinal);
            if (cut >= 0)
                fullText = fullText.Substring(0, cut);

            // much looser pattern:
            // find every Course ID, then read text until next Course ID
            var matches = Regex.Matches(
                fullText,
                @"([A-Z]{2}\d{3})(.*?)(?=[A-Z]{2}\d{3}|$)",
                RegexOptions.Singleline);

            foreach (Match m in matches)
            {
                string courseId = m.Groups[1].Value.Trim();
                string block = m.Value;

                if (string.IsNullOrWhiteSpace(courseId))
                    continue;

                string grade = "";

                if (block.Contains("未過") && block.Contains("#"))
                    grade = "#";
                else if (block.Contains("通過"))
                    grade = "PASS";
                else if (block.Contains("抵免"))
                    grade = "PASS";
                else
                    continue;

                string credits = ExtractCreditsFromBlock(block, courseId);
                string courseName = ExtractChineseCourseNameFromBlock(block, courseId);
                string category = ExtractCategoryFromBlock(block);

                result.Add(new PassedCourseRow
                {
                    CourseID = courseId,
                    Grade = grade,
                    EnglishNameCourse = courseName,
                    Credits = credits,
                    Category = category,
                    RawBlock = block
                });
            }

            // merge duplicates: PASS wins
            var merged = result
                .GroupBy(x => NormalizeCourseId(x.CourseID), StringComparer.OrdinalIgnoreCase)
                .Select(g =>
                {
                    var best = g.FirstOrDefault(x => HasRealGrade(x.Grade)) ?? g.First();

                    return new PassedCourseRow
                    {
                        CourseID = best.CourseID,
                        Grade = g.Any(x => HasRealGrade(x.Grade)) ? "PASS" : "#",
                        EnglishNameCourse = best.EnglishNameCourse,
                        Credits = best.Credits,
                        Category = best.Category
                    };
                })
                .ToList();

            return merged;
        }

        private void ApplyPassedCoursesToStep5(List<PassedCourseRow> passedRows)
        {
            if (_step5Table == null)
                _step5Table = CreateStep5Table();

            foreach (var p in passedRows)
            {
                string pid = NormalizeCourseId(p.CourseID);
                if (string.IsNullOrWhiteSpace(pid))
                    continue;

                if (!HasRealGrade(p.Grade))
                    continue;

                bool found = false;

                foreach (DataRow row in _step5Table.Rows)
                {
                    string existingId = NormalizeCourseId(row["Course ID"]?.ToString());

                    if (existingId.Equals(pid, StringComparison.OrdinalIgnoreCase))
                    {
                        row["Check passed"] = "✓";

                        if (string.IsNullOrWhiteSpace(row["Credits"]?.ToString()) && !string.IsNullOrWhiteSpace(p.Credits))
                            row["Credits"] = p.Credits;

                        if (string.IsNullOrWhiteSpace(row["English Name Course"]?.ToString()) && !string.IsNullOrWhiteSpace(p.EnglishNameCourse))
                            row["English Name Course"] = p.EnglishNameCourse;

                        if (string.IsNullOrWhiteSpace(row["Category"]?.ToString()) && !string.IsNullOrWhiteSpace(p.Category))
                            row["Category"] = p.Category;

                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var nr = _step5Table.NewRow();

                    string credits = p.Credits;

                    // fallback: if parser failed, try reading from block again
                    if (string.IsNullOrWhiteSpace(credits))
                        credits = ExtractCreditsFromBlock(p.RawBlock, p.CourseID);

                    nr["STT"] = _step5Table.Rows.Count + 1;
                    nr["Course ID"] = p.CourseID ?? "";
                    nr["Credits"] = credits ?? "";
                    nr["English Name Course"] = p.EnglishNameCourse ?? "";
                    nr["Category"] = p.Category ?? "";
                    nr["Check passed"] = "✓";

                    _step5Table.Rows.Add(nr);
                }
            }

            RebuildStep5STT();

            dataGridViewStep5.DataSource = null;
            dataGridViewStep5.DataSource = _step5Table;
            dataGridViewStep5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            UpdatePassedCredits();
            UpdateAddCoursePreferencesButton();
        }

        private void RebuildStep5STT()
        {
            if (_step5Table == null) return;

            for (int i = 0; i < _step5Table.Rows.Count; i++)
                _step5Table.Rows[i]["STT"] = i + 1;
        }

        private void UpdatePassedCredits()
        {
            int total = 0;

            foreach (DataRow row in _step5Table.Rows)
            {
                string passed = row["Check passed"]?.ToString();
                string creditStr = row["Credits"]?.ToString();

                if (passed == "✓" && int.TryParse(creditStr, out int credit))
                {
                    total += credit;
                }
            }

            lblStatus.Text = "Total Passed Credits: " + total;
            // change color to green after update
            lblStatus.ForeColor = Color.Green;
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

        private bool HasRealGrade(string grade)
        {
            string g = (grade ?? "").Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(g))
                return false;

            if (g == "#")
                return false;

            if (g == "PASS")
                return true;

            return false;
        }


        private string MapCategory(string chineseCategory)
        {
            string c = (chineseCategory ?? "").Trim();

            if (c.Contains("共必") || c.Contains("共同必修"))
                return "Common Course";

            if (c.Contains("系必") || c.Contains("必選"))
                return "Compulsory & Elective";

            if (c.Contains("通識"))
                return "General Education";

            if (c == "選" || c.Contains("選"))
                return "Elective";

            return "";
        }

        private string ExtractCategoryFromBlock(string block)
        {
            Match m = Regex.Match(block, @"(共同必修|共必|系必|必選|通識|選)");
            if (m.Success)
                return MapCategory(m.Value);

            return "";
        }



        private string ExtractChineseCourseNameFromBlock(string block, string courseId)
        {
            if (string.IsNullOrWhiteSpace(block))
                return "";

            // --------
            // Version A: normal spaced text
            // --------
            string s1 = block.Replace("\r", " ").Replace("\n", " ");
            s1 = Regex.Replace(s1, @"\s+", " ").Trim();

            int idx1 = s1.IndexOf(courseId, StringComparison.OrdinalIgnoreCase);
            if (idx1 >= 0)
                s1 = s1.Substring(idx1 + courseId.Length).Trim();

            s1 = Regex.Replace(s1, @"^\d+\s+\d+\s+\d+\s+", "");
            s1 = Regex.Replace(s1, @"^\d+\s+\d+\s+", "");
            s1 = Regex.Replace(s1, @"^\d+\s+", "");

            string[] stopWords =
            {
        "共同必修", "共必", "系必", "必選", "通識", "選",
        "通過", "未過", "抵免"
    };

            int cut1 = s1.Length;
            foreach (string w in stopWords)
            {
                int p = s1.IndexOf(w, StringComparison.Ordinal);
                if (p >= 0 && p < cut1)
                    cut1 = p;
            }

            s1 = s1.Substring(0, cut1).Trim();
            s1 = Regex.Replace(s1, @"^\d+", "");
            s1 = s1.Trim(' ', '　', ':', '：', ';', '；', ',', '，');


            // remove (*E) (*D)
            s1 = Regex.Replace(s1, @"\(\*[A-Za-z]\)", "");


            if (!string.IsNullOrWhiteSpace(s1))
                return s1;

            // --------
            // Version B: merged text like IN1091行銷傳播3
            // --------
            string s2 = block.Replace("\r", "").Replace("\n", "");
            s2 = s2.Replace(" ", "").Replace("\u00A0", "");

            Match m = Regex.Match(
                s2,
                Regex.Escape(courseId) + @"\d([\u4e00-\u9fff（）()、，,：:；;．\.\-A-Za-z\*]+?)(\d{1,2})(?=\D|$)"
            );

            if (m.Success)
                return m.Groups[1].Value.Trim();

            return "";
        }


        private string ExtractCreditsFromBlock(string block, string courseId)
        {
            if (string.IsNullOrWhiteSpace(block))
                return "";

            // Version A: spaced text
            string s1 = block.Replace("\r", " ").Replace("\n", " ");
            s1 = Regex.Replace(s1, @"\s+", " ").Trim();

            // Example:
            // 通過 3 44 95
            // 未過 2 17 #
            // 抵免 2 63 82
            Match m1 = Regex.Match(s1, @"(?:通過|未過|抵免)\s*([0-4])\b");
            if (m1.Success)
                return m1.Groups[1].Value.Trim();

            // Version B: merged text
            // Example:
            // CP1011基礎程式設計-C++實習（一）(*E)共必通過25377
            string s2 = block.Replace("\r", "").Replace("\n", "");
            s2 = s2.Replace(" ", "").Replace("\u00A0", "");

            Match m2 = Regex.Match(s2, @"(?:通過|未過|抵免)([0-4])");
            if (m2.Success)
                return m2.Groups[1].Value.Trim();

            return "";
        }


        // =========================================================
        // Step 6 — UI formatting
        // =========================================================

        private void dataGridView2Page2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2Page2.Columns[e.ColumnIndex].Name != "Teaching Language") return;

            string text = (e.Value ?? "").ToString()
                .Replace('\u00A0', ' ')
                .Trim();

            if (text.Equals("English", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.SelectionForeColor = Color.Red;
            }
        }

        // =========================================================
        // Step 6 — Choose / load timetable
        // =========================================================

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (comboBoxDept.SelectedItem == null)
            {
                MessageBox.Show("Please choose a department first.");
                return;
            }

            string deptText = comboBoxDept.SelectedItem.ToString();
            if (_loadedDepts.Contains(deptText))
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

                AppendToMaster(processed);
                ReNumberNO(_masterTable);
                _loadedDepts.Add(deptText);

                dataGridView2Page2.DataSource = _masterTable;
                dataGridView2Page2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                UpdateAddCoursePreferencesButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed:\n" + ex.Message);
            }
        }

        private string FindDepartmentFileAuto(string baseDir, string deptText)
        {
            if (!Directory.Exists(baseDir)) return null;

            var exts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".mhtml", ".mht", ".html", ".htm"
            };

            var files = Directory.EnumerateFiles(baseDir, "*.*", SearchOption.TopDirectoryOnly)
                .Where(p => exts.Contains(Path.GetExtension(p)))
                .ToList();

            if (files.Count == 0) return null;

            if (DeptToExactFile.TryGetValue(deptText, out var exactBaseName))
            {
                string keyExact = NormalizeKey(exactBaseName);

                var exact = files.FirstOrDefault(f =>
                    NormalizeKey(Path.GetFileNameWithoutExtension(f)) == keyExact);

                if (exact != null) return exact;
            }

            string key = NormalizeKey(deptText);
            var exact2 = files.FirstOrDefault(f =>
                NormalizeKey(Path.GetFileNameWithoutExtension(f)) == key);
            if (exact2 != null) return exact2;

            foreach (var f in files)
            {
                string n = NormalizeKey(Path.GetFileNameWithoutExtension(f));
                if (n.Contains(key) || key.Contains(n)) return f;
            }

            string code = ExtractLeadingCode(deptText);
            if (!string.IsNullOrEmpty(code))
            {
                var byCode = files.FirstOrDefault(f =>
                    Path.GetFileNameWithoutExtension(f).TrimStart()
                        .StartsWith(code, StringComparison.OrdinalIgnoreCase));
                if (byCode != null) return byCode;
            }

            return null;
        }

        private static string ExtractLeadingCode(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            var m = Regex.Match(s.Trim(), @"^([A-Za-z0-9]{3})\b");
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

        // =========================================================
        // Step 6 — Output table
        // =========================================================

        private DataTable CreateOutputTable()
        {
            var t = new DataTable();

            t.Columns.Add("NO", typeof(int));
            t.Columns.Add("Course ID");
            t.Columns.Add("Class");
            t.Columns.Add("Department");
            t.Columns.Add("Year Available");
            t.Columns.Add("Chinese Course Name");
            t.Columns.Add("English Course Name");
            t.Columns.Add("Teaching Language");
            t.Columns.Add("Type");
            t.Columns.Add("Timetable");
            t.Columns.Add("Classroom");
            t.Columns.Add("Instructor");

            return t;
        }

        private void AppendToMaster(DataTable add)
        {
            if (_masterTable == null)
                _masterTable = CreateOutputTable();

            foreach (DataRow r in add.Rows)
            {
                var nr = _masterTable.NewRow();
                foreach (DataColumn col in _masterTable.Columns)
                {
                    nr[col.ColumnName] = r.Table.Columns.Contains(col.ColumnName)
                        ? (r[col.ColumnName]?.ToString() ?? "")
                        : "";
                }
                _masterTable.Rows.Add(nr);
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

        // =========================================================
        // Step 6 — Read HTML / MHTML
        // =========================================================

        private static string ReadHtmlOrMhtml(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();

            if (ext == ".html" || ext == ".htm")
                return File.ReadAllText(path, Encoding.UTF8);

            return ExtractMainHtmlFromMhtml(path);
        }

        private static string ExtractMainHtmlFromMhtml(string mhtmlPath)
        {
            using (var stream = File.OpenRead(mhtmlPath))
            {
                var msg = MimeMessage.Load(stream);

                var htmlParts = msg.BodyParts
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

        // =========================================================
        // Step 6 — Parse HTML table
        // =========================================================

        private DataTable ParseCourseTableFromHtml(string html)
        {
            var doc = new HAP.HtmlDocument();
            doc.LoadHtml(html);

            var tables = doc.DocumentNode.SelectNodes("//table");
            if (tables == null || tables.Count == 0)
                throw new Exception("No <table> found in HTML.");

            foreach (var table in tables)
            {
                var dt = TryParseCourseTable_PythonStyle(table);
                if (dt != null) return dt;
            }

            throw new Exception("Cannot find the course list table with the expected 7 columns.");
        }

        private static string CleanCellNode(HAP.HtmlNode cell)
        {
            if (cell == null) return "";

            var clone = cell.Clone();

            var brs = clone.SelectNodes(".//br");
            if (brs != null)
            {
                foreach (var br in brs)
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
            var rows = tableNode.SelectNodes(".//tr");
            if (rows == null || rows.Count < 2) return null;

            var grid = new List<List<string>>();
            foreach (var tr in rows)
            {
                var cells = tr.SelectNodes("./th|./td");
                if (cells == null) continue;

                var row = cells.Select(CleanCellNode).ToList();
                if (row.Count > 0) grid.Add(row);
            }
            if (grid.Count < 2) return null;

            bool HeaderOk(List<string> header)
            {
                var set = new HashSet<string>(
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
            var header = grid[headerRowIndex];

            var dt = new DataTable();
            foreach (var hRaw in header)
            {
                var h = (hRaw ?? "").Trim();
                if (string.IsNullOrEmpty(h)) h = "Column";

                string col = h;
                int k = 1;
                while (dt.Columns.Contains(col))
                    col = h + "_" + (k++);

                dt.Columns.Add(col);
            }

            for (int r = headerRowIndex + 1; r < grid.Count; r++)
            {
                var row = grid[r];
                var values = row.Select(x => x ?? "").ToList();

                while (values.Count < dt.Columns.Count) values.Add("");
                if (values.Count > dt.Columns.Count) values = values.Take(dt.Columns.Count).ToList();

                if (values.All(v => string.IsNullOrWhiteSpace(v))) continue;

                dt.Rows.Add(values.ToArray());
            }

            return dt.Rows.Count == 0 ? null : dt;
        }

        // =========================================================
        // Step 6 — Preprocess
        // =========================================================

        private DataTable PreprocessStep6(DataTable raw)
        {
            var output = CreateOutputTable();

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

                var row = output.NewRow();

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

            var m = Regex.Match(s, @"^\s*([A-Za-z0-9]+)\s+([A-Za-z0-9]+)\s*$");
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
                var parts = sNoTags.Split(new[] { "(Syllabus)" }, 2, StringSplitOptions.None);
                zh = parts[0].Trim();
                en = parts.Length > 1 ? parts[1].Trim() : "";
            }
            else
            {
                var m = Regex.Match(sNoTags, @"[A-Za-z]");
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

            var timeMatches = Regex.Matches(text, @"(?<!\d)\d{3}(?!\d)")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            var seenT = new HashSet<string>();
            var orderedT = new List<string>();
            foreach (var t in timeMatches)
                if (seenT.Add(t)) orderedT.Add(t);

            var roomMatches = Regex.Matches(text, @"(?<!\d)\d{4}(?!\d)")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            var seenR = new HashSet<string>();
            var orderedR = new List<string>();
            foreach (var r in roomMatches)
                if (seenR.Add(r)) orderedR.Add(r);

            timetable = string.Join(" ", orderedT);
            classroom = string.Join("; ", orderedR);
        }

        // =========================================================
        // Step 6 — Custom cell
        // =========================================================

        private sealed class EnglishRedTextCell : DataGridViewTextBoxCell
        {
            protected override void Paint(Graphics graphics,
                Rectangle clipBounds,
                Rectangle cellBounds,
                int rowIndex,
                DataGridViewElementStates cellState,
                object value,
                object formattedValue,
                string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                string text = (formattedValue ?? "").ToString()
                    .Replace('\u00A0', ' ')
                    .Trim();

                if (!text.Equals("English", StringComparison.OrdinalIgnoreCase))
                {
                    base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState,
                        value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                    return;
                }

                var partsWithoutText = paintParts & ~DataGridViewPaintParts.ContentForeground;

                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState,
                    value, formattedValue, errorText, cellStyle, advancedBorderStyle, partsWithoutText);

                var font = cellStyle.Font ?? this.DataGridView?.Font ?? Control.DefaultFont;
                Rectangle textRect = cellBounds;
                textRect.Inflate(-4, -2);

                int y = textRect.Y + (textRect.Height - font.Height) / 2;

                TextRenderer.DrawText(graphics, "English", font,
                    new Point(textRect.X, y),
                    Color.Red,
                    TextFormatFlags.NoPadding);
            }
        }

        // =========================================================
        // Step 6 — Delete loaded department
        // =========================================================

        private static string NormalizeDeptKey(string deptText)
        {
            if (string.IsNullOrWhiteSpace(deptText)) return "";

            string s = Regex.Replace(deptText.Trim(), @"^[A-Za-z0-9]{3}\s+", "");
            s = Regex.Replace(s, @"\s+", " ");
            s = s.Replace(" (", "(").Replace("( ", "(").Replace(" )", ")").Replace(") ", ")");

            return s.Trim();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (comboBoxDept.SelectedItem == null)
            {
                MessageBox.Show("Please choose a department first.");
                return;
            }

            string deptText = comboBoxDept.SelectedItem.ToString();

            if (!_loadedDepts.Contains(deptText))
            {
                MessageBox.Show($"⚠ {deptText} is not loaded yet.");
                return;
            }

            if (_masterTable == null || _masterTable.Rows.Count == 0)
            {
                MessageBox.Show("No data to delete.");
                return;
            }

            string deptKey = NormalizeDeptKey(deptText);

            int removed = 0;
            for (int i = _masterTable.Rows.Count - 1; i >= 0; i--)
            {
                var depCell = _masterTable.Rows[i]["Department"]?.ToString() ?? "";
                string depKeyInTable = NormalizeDeptKey(depCell);

                if (depKeyInTable.Equals(deptKey, StringComparison.OrdinalIgnoreCase) ||
                    depKeyInTable.IndexOf(deptKey, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    deptKey.IndexOf(depKeyInTable, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _masterTable.Rows.RemoveAt(i);
                    removed++;
                }
            }

            if (removed == 0)
            {
                MessageBox.Show($"⚠ Cannot find rows to delete for:\n{deptText}\n(Department text format may differ.)");
                return;
            }

            _loadedDepts.Remove(deptText);

            ReNumberNO(_masterTable);

            dataGridView2Page2.DataSource = null;
            dataGridView2Page2.DataSource = _masterTable;
            dataGridView2Page2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            MessageBox.Show($"✅ Deleted {removed} rows for:\n{deptText}");
        }



        private void btnAddCoursePreferences_Click(object sender, EventArgs e)
        {
            if (!btnAddCoursePreferences.Enabled)
                return;

            Program.Page1.Hide();
            this.Hide();
            Program.Page3.Show();
        }

        public DataTable GetStep5Table()
        {
            return _step5Table;
        }

        public DataTable GetStep6Table()
        {
            return _masterTable;
        }

        private void btnExportStep5Excel_Click(object sender, EventArgs e)
        {
            ExportGridToExcel(dataGridViewStep5, "Completed_Course_Record");

            if (!btnExportStep5Excel.Enabled)
            {
                MessageBox.Show(
                    "Please upload the transcript file first.",
                    "No Course Data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            ExportGridToExcel(
                dataGridViewStep5,
                "Completed_Course_Record");
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

                        var ws = wb.Worksheets.Add(dt, "Completed Courses");
                        ws.Row(1).Style.Font.Bold = true;
                        ws.Columns().AdjustToContents();

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Excel file exported successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to export Excel file:\n" + ex.Message);
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
    }
}