using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using WinColor = System.Drawing.Color;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ExcelDataReader;
using HtmlAgilityPack;
using MimeKit;

namespace ThesisSchedulerGUI
{
    public partial class Form1 : Form
    {
        // =========================================================
        // IMPORTANT (Designer control names you must have)
        // =========================================================
        // Step 1:
        //   txtName, txtStudentId, cmbDept, cmbGrade, btnConfirm, btnEdit, lblStatus
        // Step 2:
        //   txtCommonCourse, txtCompulsoryAndElectiveCourse
        //   btnParseCourses   ("Export" / Parse pasted courses)
        //   btnUploadFile     (Upload latest offering file)
        //   btnValidateCourse ("Validate course")
        //   lblUploadStatus
        //   dataGridView1
        // =========================================================

        // =========================
        // Step 1 — Student Info
        // =========================
        private readonly string[] DEPARTMENTS = new[]
        {
            "300 College of Engineering",
            "302 Department of Mechanical Engineering(Undergraduate Program)",
            "303 Department of Chemical Engineering and Materials Science(Undergraduate Program)",
            "305 Department of Industrial Engineering and Management(Undergraduate Program)",
            "309 International Bachelor Program in Engineering(Undergraduate Program)",
            "320 Post-Baccalaureate Program for Carbon Zero Sustainable Development Preparatory Office(Undergraduate Program)",
            "322 Department of Mechanical Engineering(Master Program)",
            "323 Department of Chemical Engineering and Materials Science(Master Program)",
            "325 Department of Industrial Engineering and Management(Master Program)",
            "330 Graduate school of renewable energy engineering(Master Program)",
            "340 Master of Science Degree of Smart Manufacturing for Electronics(Master Program)",
            "352 Department of Mechanical Engineering(Ph.D. Program)",
            "353 Department of Chemical Engineering and Materials Science(Ph.D. Program)",
            "355 Department of Industrial Engineering and Management(Ph.D. Program)",

            "500 College of Management",
            "505 Bachelor of Business Administration Program, College of Management(Undergraduate Program)",
            "530 Master of Business Administration Program, College of Management(Master Program)",
            "531 Master of Science in Finance and Accounting, College of Management(Master Program)",
            "532 Executive Master of Business Administration Program, College of Management(Master Program)",
            "554 Doctoral Program, College of Management(Ph.D. Program)",

            "600 College of Humanities and Social Sciences",
            "601 Department of Foreign Languages and Applied Linguistics(Undergraduate Program)",
            "602 Department of Chinese Linguistics and Literature(Undergraduate Program)",
            "603 Department of Art and Design(Undergraduate Program)",
            "604 Department of Social and Policy Sciences(Undergraduate Program)",
            "608 International Bachelor Program in Strategic Communication, College of Humanities and Social Sciences(Undergraduate Program)",
            "621 Department of Foreign Languages and Applied Linguistics, Master's Program(Master Program)",
            "622 Department of Chinese Linguistics and Literature(Master Program)",
            "623 Master Program in Art and Design Management, Department of Art and Design(Master Program)",
            "624 Department of Social and Policy Sciences(Master Program)",
            "656 Doctor of Philosophy in Cultural Industries and Cultural Policy(Ph.D. Program)",

            "700 College of Informatics",
            "304 Department of Computer Science and Engineering(Undergraduate Program)",
            "701 Department of Information Management(Undergraduate Program)",
            "702 Department of Information Communication(Undergraduate Program)",
            "705 International Bachelor Program in Informatics(Undergraduate Program)",
            "721 Department of Information Management(Master Program)",
            "722 Department of Information Communication(Master Program)",
            "723 Graduate School of Social Informatics(Master Program)",
            "724 Department of Computer Science and Engineering(Master Program)",
            "725 Graduate Program in Biomedical Informatics(Master Program)",
            "751 Department of Information Management(Ph.D. Program)",
            "754 Department of Computer Science and Engineering(Ph.D. Program)",

            "800 College of Electrical and Communication Engineering",
            "310 International Bachelor Program in Electrical and Communication Engineering(Undergraduate Program)",
            "311 Department of Electrical Engineering(Undergraduate Program)",
            "312 Department of Electrical Engineering(Undergraduate Program)",
            "313 Department of Electrical Engineering(Undergraduate Program)",
            "326 Department of Electrical Engineering(Master Program)",
            "331 Department of Electrical Engineering(Master Program)",
            "332 Department of Electrical Engineering(Master Program)",
            "333 Department of Electrical Engineering(Master Program)",
            "359 Department of Electrical Engineering(Ph.D. Program)",
            "360 Department of Electrical Engineering(Ph.D. Program)",
            "361 Department of Electrical Engineering(Ph.D. Program)",
            "301 Department of Electrical Engineering(Undergraduate Program)",
            "307 Department of Communications Engineering(Undergraduate Program)",
            "308 Department of Photonics Engineering(Undergraduate Program)",
            "327 Department of Communications Engineering(Master Program)",
            "328 Department of Photonics Engineering(Master Program)",
            "356 Department of Electrical Engineering(Ph.D. Program)",
            "357 Department of Communications Engineering(Ph.D. Program)",
            "358 Department of Photonics Engineering(Ph.D. Program)",

            "A00 College of Medicine and Nursing",
            "329 Graduate School of Biotechnology and Bioengineering(Master Program)",
            "A11 School of Nursing(Undergraduate Program)",
            "A21 Graduate Institute of Medicine(Master Program)",

            "901 General Education Center",
            "903 Military Education Office",
            "904 Physical Education Office",
            "906 International Language and Culture Center",
            "907 Global Affairs Office",
            "908 Moocs",
            "909 Exploring Interdiscipline",
            "910 Taiwan Artificial Intelligence College Alliance"
        };

        private readonly string[] UG_GRADES = { "Freshmen", "Sophomore", "Junior", "Senior" };
        private readonly string[] MASTER_GRADES = { "First Year", "Second Year" };
        private readonly string[] PHD_GRADES = { "First Year", "Second Year", "Third Year", "Fourth Year" };
        private readonly string[] STEP3_CATEGORY ={   "Common Course",  "Compulsory & Elective"};

        private string student_name, student_id, student_dept, student_grade;

        // State flags
        private bool _studentConfirmed = false;
        private bool _pastesParsed = false;
        private bool _fileUploaded = false;
        private bool _coursesValidated = false;

        // Parsed pasted rows cache (after Parse)
        private List<CourseRow> _parsedRows = new List<CourseRow>();

        // Uploaded file index for validation
        private HashSet<string> _validCourseIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _validEnglishNameKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // ✅ NEW: Big normalized blob (for paragraph-only matches, no course IDs)
        private string _uploadedEnglishBlobKey = "";

        // =========================
        // Step 2 — Data model
        // =========================
        public class CourseRow
        {
            public int STT { get; set; }
            public string CourseID { get; set; }
            public string Credits { get; set; }
            public string EnglishNameCourse { get; set; }
            public string ChineseNameCourse { get; set; } // internal
            public string Category { get; set; }          // "Common Course" or "Compulsory & Elective"
        }

        // STRICT ID
        private static readonly Regex CourseIdPat = new Regex(@"\b[A-Z]{2}\d{3}\b", RegexOptions.Compiled);

        // LOOSE ID in uploaded docs: "LC 101" => "LC101"
        private static readonly Regex CourseIdPatLoose =
            new Regex(@"\b([A-Z]{2})\s*([0-9]{3})\b", RegexOptions.Compiled);

        // Credits: （3） or (3)
        private static readonly Regex CreditPat =
            new Regex(@"(?:（\s*(\d+)\s*）|\(\s*(\d+)\s*\))", RegexOptions.Compiled);

        private DataGridView dgvCourses;

        public Form1()
        {
            InitializeComponent();
            dgvCourses = dataGridView1;
            this.AutoScaleMode = AutoScaleMode.None;
            InitStep1UI();
            InitStep2UI();
            InitStep3UI();
        }

        // Keep this to satisfy Designer event wiring (if exists)
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // =========================================================
        // Step 2 — UI helpers / rules
        // =========================================================
        private void SetPasteBoxesEnabled(bool enabled)
        {
            txtCommonCourse.ReadOnly = !enabled;
            txtCompulsoryAndElectiveCourse.ReadOnly = !enabled;

            txtCommonCourse.Enabled = enabled;
            txtCompulsoryAndElectiveCourse.Enabled = enabled;
        }

        private bool PasteBoxesFilled()
        {
            string commonText = (txtCommonCourse.Text ?? "").Trim();
            string compText = (txtCompulsoryAndElectiveCourse.Text ?? "").Trim();
            return !string.IsNullOrWhiteSpace(commonText) && !string.IsNullOrWhiteSpace(compText);
        }

        private void ResetStep2Progress()
        {
            _coursesValidated = false;
            btnAddCourse.Enabled = false;
            btnUpdateCourse.Enabled = false;
            btnDeleteCourse.Enabled = false;
            _pastesParsed = false;
            _fileUploaded = false;
            btnNextPage.Enabled = false;
            _parsedRows.Clear();

            _validCourseIds.Clear();
            _validEnglishNameKeys.Clear();
            _uploadedEnglishBlobKey = "";

            dgvCourses.DataSource = null;

        }

        private void UpdateStep2GuidanceAndButtons()
        {
            btnParseCourses.Enabled = false;
            btnUploadFile.Enabled = false;
            btnValidateCourse.Enabled = false;

            if (!_studentConfirmed)
            {
                lblUploadStatus.ForeColor = WinColor.DarkOrange;
                lblUploadStatus.Text = "Please confirm Student Info first.";
                return;
            }

            if (!PasteBoxesFilled())
            {
                lblUploadStatus.ForeColor = WinColor.DarkOrange;
                lblUploadStatus.Text =
                    "Step 2:\n" +
                    "1) Paste courses into BOTH boxes.\n" +
                    "2) Click 'Export' (Parse) to show them in the table.\n" +
                    "3) Click 'Upload File' (your reference doc / offering list).\n" +
                    "4) Click 'Validate course' to double-check courses .\n\n";
                return;
            }

            btnParseCourses.Enabled = true;

            if (!_pastesParsed)
            {
                lblUploadStatus.ForeColor = WinColor.DarkOrange;
                lblUploadStatus.Text =
                    "Good ✅\n" +
                    "Next: Click 'Export' to parse your pasted courses and show them in the table.\n" +
                    "After that, you can upload the file.";
                return;
            }

            btnUploadFile.Enabled = true;

            if (!_fileUploaded)
            {
                lblUploadStatus.ForeColor = WinColor.DarkOrange;
                lblUploadStatus.Text =
                    "Parsed ✓\n" +
                    "Next: Click 'Upload File' to upload your reference doc.\n" +
                    "Then click 'Validate course' to keep only matched courses.";
                return;
            }

            btnValidateCourse.Enabled = true;
            lblUploadStatus.ForeColor = WinColor.SeaGreen;
            lblUploadStatus.Text =
                "Uploaded ✓\n" +
                "Now click 'Validate course'.\n" +
                "It will remove courses not found in the uploaded file.";
        }

        // =========================================================
        // Step 1 UI init
        // =========================================================
        private void InitStep1UI()
        {
            cmbDept.Items.Clear();
            cmbDept.Items.AddRange(DEPARTMENTS);

            string defaultDept = "705 International Bachelor Program in Informatics(Undergraduate Program)";
            cmbDept.SelectedItem = DEPARTMENTS.Contains(defaultDept) ? defaultDept : DEPARTMENTS[0];

            RefreshGradeOptions();

            cmbDept.SelectedIndexChanged += (s, e) => RefreshGradeOptions();
            btnConfirm.Click += BtnConfirm_Click;
            btnEdit.Click += BtnEdit_Click;

            btnEdit.Enabled = false;
            LockInputs(false);

            AddPlaceholder(txtName, "e.g., 芳妙草");
            AddPlaceholder(txtStudentId, "e.g., 1135444");

            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.BackColor = WinColor.SeaGreen;
            btnConfirm.ForeColor = WinColor.White;
            btnConfirm.FlatAppearance.BorderSize = 0;

            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.BackColor = WinColor.FromArgb(255, 255, 102, 102);
            btnEdit.ForeColor = WinColor.Black;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.UseVisualStyleBackColor = false;

            lblStatus.ForeColor = WinColor.DarkOrange;
            lblStatus.Text = "Please enter your name.";

            _studentConfirmed = false;

            SetPasteBoxesEnabled(false);
            ResetStep2Progress();
            UpdateStep2GuidanceAndButtons();
        }

        private void AddPlaceholder(TextBox tb, string placeholder)
        {
            tb.Tag = placeholder;
            tb.ForeColor = WinColor.Gray;
            tb.Text = placeholder;

            tb.GotFocus += (s, e) =>
            {
                if (tb.Text == (string)tb.Tag)
                {
                    tb.Text = "";
                    tb.ForeColor = WinColor.Black;
                }
            };

            tb.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = (string)tb.Tag;
                    tb.ForeColor = WinColor.Gray;
                }
            };
        }

        private bool IsPlaceholder(TextBox tb) => tb.Tag is string p && tb.Text == p;

        private string DeptLevel(string dept)
        {
            string s = (dept ?? "").ToLowerInvariant();
            if (s.Contains("ph.d") || s.Contains("phd")) return "phd";
            if (s.Contains("master")) return "master";
            if (s.Contains("undergraduate")) return "ug";
            return "ug";
        }

        private string[] GradesForDept(string dept)
        {
            string level = DeptLevel(dept);
            if (level == "master") return MASTER_GRADES;
            if (level == "phd") return PHD_GRADES;
            return UG_GRADES;
        }

        private void RefreshGradeOptions()
        {
            string dept = cmbDept.SelectedItem == null ? "" : cmbDept.SelectedItem.ToString();
            string[] options = GradesForDept(dept);

            string old = cmbGrade.SelectedItem == null ? "" : cmbGrade.SelectedItem.ToString();

            cmbGrade.Items.Clear();
            cmbGrade.Items.AddRange(options);

            if (!string.IsNullOrEmpty(old) && options.Contains(old)) cmbGrade.SelectedItem = old;
            else if (cmbGrade.Items.Count > 0) cmbGrade.SelectedIndex = 0;
        }

        private void LockInputs(bool locked)
        {
            txtName.ReadOnly = locked;
            txtStudentId.ReadOnly = locked;
            cmbDept.Enabled = !locked;
            cmbGrade.Enabled = !locked;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = WinColor.DarkOrange;
            lblStatus.Text = "Saving...";

            if (string.IsNullOrWhiteSpace(txtName.Text) || IsPlaceholder(txtName))
            {
                lblStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblStatus.Text = "Please enter your name.";
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtStudentId.Text) || IsPlaceholder(txtStudentId))
            {
                lblStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblStatus.Text = "Please enter your Student ID.";
                txtStudentId.Focus();
                return;
            }

            student_name = txtName.Text.Trim();
            student_id = txtStudentId.Text.Trim();
            student_dept = cmbDept.SelectedItem == null ? "" : cmbDept.SelectedItem.ToString();
            student_grade = cmbGrade.SelectedItem == null ? "" : cmbGrade.SelectedItem.ToString();

            LockInputs(true);
            btnConfirm.Enabled = false;
            btnEdit.Enabled = true;

            _studentConfirmed = true;

            SetPasteBoxesEnabled(true);

            ResetStep2Progress();
            UpdateStep2GuidanceAndButtons();

            lblStatus.ForeColor = WinColor.SeaGreen;
            lblStatus.Text = "Saved ✅  (" + student_name + ", " + student_id + ")";
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            LockInputs(false);
            btnConfirm.Enabled = true;
            btnEdit.Enabled = false;

            _studentConfirmed = false;

            lblStatus.ForeColor = WinColor.DarkOrange;
            lblStatus.Text = "Editing... Please update fields then click Confirm.";

            SetPasteBoxesEnabled(false);

            ResetStep2Progress();
            UpdateStep2GuidanceAndButtons();
        }

        // =========================================================
        // Step 2 UI init
        // =========================================================
        private void InitStep2UI()
        {
            btnParseCourses.Click += BtnParseCourses_Click;
            btnUploadFile.Click += BtnUploadFile_Click;
            btnValidateCourse.Click += BtnValidateCourse_Click;

            SetPasteBoxesEnabled(false);

            txtCommonCourse.TextChanged += (s, e) =>
            {
                _pastesParsed = false;
                _fileUploaded = false;
                _parsedRows.Clear();
                dgvCourses.DataSource = null;
                UpdateStep2GuidanceAndButtons();
            };

            txtCompulsoryAndElectiveCourse.TextChanged += (s, e) =>
            {
                _pastesParsed = false;
                _fileUploaded = false;
                _parsedRows.Clear();
                dgvCourses.DataSource = null;
                UpdateStep2GuidanceAndButtons();
            };

            lblUploadStatus.ForeColor = WinColor.DarkOrange;
            lblUploadStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblUploadStatus.Text = "Please confirm Student Info first.";

            dgvCourses.AutoGenerateColumns = false;
            dgvCourses.AllowUserToAddRows = false;
            dgvCourses.ReadOnly = true;
            dgvCourses.RowHeadersVisible = false;
            dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCourses.Columns.Clear();
            dgvCourses.Columns.Add(MakeTextCol("STT", "STT", 60));
            dgvCourses.Columns.Add(MakeTextCol("CourseID", "Course ID", 90));
            dgvCourses.Columns.Add(MakeTextCol("Credits", "Credits", 70));
            dgvCourses.Columns.Add(MakeTextCol("EnglishNameCourse", "English Name Course", 300));
            dgvCourses.Columns.Add(MakeTextCol("Category", "Category", 150));

            dgvCourses.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12F);
            dgvCourses.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dgvCourses.EnableHeadersVisualStyles = false;
            dgvCourses.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCourses.RowTemplate.Height = 28;
            dgvCourses.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            ResetStep2Progress();
            UpdateStep2GuidanceAndButtons();
        }

        private DataGridViewTextBoxColumn MakeTextCol(string dataProp, string header, int minWidth)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataProp,
                HeaderText = header,
                MinimumWidth = minWidth,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
            };
        }

        // =========================================================
        // Parse pasted courses -> show table
        // =========================================================
        private void BtnParseCourses_Click(object sender, EventArgs e)
        {
            if (!_studentConfirmed)
            {
                lblUploadStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblUploadStatus.Text = "Student information hasn't been confirmed yet. Please click Confirm in Step 1.";
                return;
            }

            if (!PasteBoxesFilled())
            {
                lblUploadStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblUploadStatus.Text = "Please paste BOTH Common Course and Compulsory & Elective course first.";
                return;
            }

            string commonText = (txtCommonCourse.Text ?? "").Trim();
            string compText = (txtCompulsoryAndElectiveCourse.Text ?? "").Trim();

            lblUploadStatus.ForeColor = WinColor.DarkOrange;
            lblUploadStatus.Text = "Parsing pasted courses...";

            var rows = new List<CourseRow>();
            rows.AddRange(ParsePortalPaste(commonText, "Common Course"));
            rows.AddRange(ParsePortalPaste(compText, "Compulsory & Elective"));

            // ✅ De-dup by (CourseID + Category) or (NameKey + Category)
            rows = rows
                .Where(r => !string.IsNullOrWhiteSpace(r.CourseID) || !string.IsNullOrWhiteSpace(r.EnglishNameCourse))
                .GroupBy(r =>
                {
                    string id = NormalizeCourseId(r.CourseID);
                    if (!string.IsNullOrWhiteSpace(id))
                        return "ID:" + id + "|CAT:" + (r.Category ?? "");

                    string nm = LowKey(BaseTitle(r.EnglishNameCourse));
                    return "NM:" + nm + "|CAT:" + (r.Category ?? "");
                }, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();

            for (int i = 0; i < rows.Count; i++)
                rows[i].STT = i + 1;

            _parsedRows = rows;
            _pastesParsed = true;

            dgvCourses.DataSource = new BindingList<CourseRow>(_parsedRows);

            lblUploadStatus.ForeColor = WinColor.SeaGreen;
            lblUploadStatus.Text = $"Parsed ✓  Total (Common+Compulsory): {_parsedRows.Count}\nNext: Upload File.";

            UpdateStep2GuidanceAndButtons();
        }

        // =========================================================
        // Upload file
        // =========================================================
        private void BtnUploadFile_Click(object sender, EventArgs e)
        {
            if (!_studentConfirmed)
            {
                lblUploadStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblUploadStatus.Text = "Student information hasn't been confirmed yet. Please click Confirm in Step 1.";
                return;
            }

            if (!_pastesParsed)
            {
                lblUploadStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblUploadStatus.Text = "Please click 'Export' (Parse) first.";
                return;
            }

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Upload file (offering list / graduation doc / rules)";
                ofd.Filter =
                    "Supported Files (*.txt;*.csv;*.docx;*.xlsx;*.xls;*.html;*.htm;*.mhtml;*.mht)|*.txt;*.csv;*.docx;*.xlsx;*.xls;*.html;*.htm;*.mhtml;*.mht|" +
                    "All files (*.*)|*.*";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                lblUploadStatus.ForeColor = WinColor.DarkOrange;
                lblUploadStatus.Text = "Loading file...";

                List<string> lines = ReadAnyFileAsLines(ofd.FileName);
                BuildValidIndexFromUploadedFile(lines);

                _fileUploaded = true;

                lblUploadStatus.ForeColor = WinColor.SeaGreen;
                lblUploadStatus.Text =
                    $"Uploaded ✓  Found {_validCourseIds.Count} course IDs / {_validEnglishNameKeys.Count} English-name keys.\nNext: Validate course.";

                UpdateStep2GuidanceAndButtons();
            }
        }

        // =========================================================
        // Validate course (remove not found in uploaded file)
        // =========================================================
        private void BtnValidateCourse_Click(object sender, EventArgs e)
        {
            if (!_studentConfirmed)
            {
                lblUploadStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblUploadStatus.Text = "Student information hasn't been confirmed yet. Please click Confirm in Step 1.";
                return;
            }

            if (!_pastesParsed)
            {
                lblUploadStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblUploadStatus.Text = "Please click 'Export' (Parse) first.";
                return;
            }

            if (!_fileUploaded)
            {
                lblUploadStatus.ForeColor = WinColor.FromArgb(220, 53, 69);
                lblUploadStatus.Text = "Please click Upload File first.";
                return;
            }

            _coursesValidated = true;
            btnNextPage.Enabled = true;

            lblUploadStatus.ForeColor = WinColor.DarkOrange;
            lblUploadStatus.Text = "Validating courses against uploaded file...";

            var kept = _parsedRows.Where(IsRowValidByUploadedFile).ToList();

            for (int i = 0; i < kept.Count; i++)
                kept[i].STT = i + 1;

            _parsedRows = kept;
            dgvCourses.DataSource = new BindingList<CourseRow>(_parsedRows);

            lblUploadStatus.ForeColor = WinColor.SeaGreen;
            lblUploadStatus.Text = $"Validated ✓  Courses kept: {_parsedRows.Count}";

            UpdateStep2GuidanceAndButtons();
            _coursesValidated = true;

            btnAddCourse.Enabled = true;
            btnUpdateCourse.Enabled = true;
            btnDeleteCourse.Enabled = true;

            SetCourseStatus("Validated. You can now Add/Update/Delete courses.", WinColor.SeaGreen);
        }

        // =========================================================
        // Matching rules
        // - ID match first
        // - then name key match
        // - then blob "contains" match (for paragraph-only docs)
        // =========================================================
        private bool IsRowValidByUploadedFile(CourseRow r)
        {
            if (r == null) return false;

            // 1) Course ID match (if available)
            string id = NormalizeCourseId(r.CourseID);
            if (!string.IsNullOrEmpty(id) && _validCourseIds.Contains(id))
                return true;

            // 2) Name match (BaseTitle removes ": ..." and "; ...")
            string key = LowKey(BaseTitle(r.EnglishNameCourse));
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (_validEnglishNameKeys.Contains(key))
                return true;

            // 3) ✅ Blob exact-token contains (best for paragraph-only “no course ID” cases)
            if (!string.IsNullOrWhiteSpace(_uploadedEnglishBlobKey) &&
                _uploadedEnglishBlobKey.Contains("|" + key + "|"))
                return true;

            // 4) Optional partial match if pasted name is shorter (use carefully)
            if (key.Length >= 18 &&
                !string.IsNullOrWhiteSpace(_uploadedEnglishBlobKey) &&
                _uploadedEnglishBlobKey.Contains(key))
                return true;

            return false;
        }
        private IEnumerable<string> ExtractParenRomanTitles(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                yield break;

            text = text.Replace("（", "(").Replace("）", ")");

            // Pattern:
            // 1) Capture base title
            // 2) Capture first (I)
            // 3) Capture optional chained "(II), (III), ..."
            var pat = new Regex(
                @"\b([A-Za-z][A-Za-z\s,&\-]+?)\s*\(\s*(I|II|III|IV|V|VI|VII|VIII|IX|X)\s*\)" +
                @"((?:\s*(?:,|and|or|&)\s*\(\s*(I|II|III|IV|V|VI|VII|VIII|IX|X)\s*\))*)",
                RegexOptions.IgnoreCase);

            foreach (Match m in pat.Matches(text))
            {
                string title = (m.Groups[1].Value ?? "").Trim();
                string firstRoman = (m.Groups[2].Value ?? "").Trim().ToUpperInvariant();

                if (title.Length < 2)
                    continue;

                // yield first
                yield return $"{title} ({firstRoman})";

                // extract additional romans
                string chain = m.Groups[3].Value;

                if (!string.IsNullOrWhiteSpace(chain))
                {
                    var extraRomans = Regex.Matches(
                        chain,
                        @"\(\s*(I|II|III|IV|V|VI|VII|VIII|IX|X)\s*\)",
                        RegexOptions.IgnoreCase);

                    foreach (Match rm in extraRomans)
                    {
                        string roman = (rm.Groups[1].Value ?? "").Trim().ToUpperInvariant();
                        yield return $"{title} ({roman})";
                    }
                }
            }
        }
        // =========================================================
        // Build index from uploaded file (IDs + name keys + blob)
        // =========================================================
        private void BuildValidIndexFromUploadedFile(List<string> lines)
        {
            _validCourseIds.Clear();
            _validEnglishNameKeys.Clear();
            _uploadedEnglishBlobKey = "";

            if (lines == null) return;

            var blobParts = new List<string>();

            foreach (var raw in lines)
            {
                string s = (raw ?? "").Trim();
                if (s.Length == 0) continue;

                // 1) Course IDs (optional, but helps a lot)
                foreach (Match m in CourseIdPat.Matches(s))
                {
                    string id = NormalizeCourseId(m.Value);
                    if (!string.IsNullOrWhiteSpace(id))
                        _validCourseIds.Add(id);
                }

                foreach (Match m in CourseIdPatLoose.Matches(s))
                {
                    string id = NormalizeCourseId(m.Groups[1].Value + m.Groups[2].Value);
                    if (!string.IsNullOrWhiteSpace(id))
                        _validCourseIds.Add(id);
                }

                // 2) Clean line for English indexing
                string cleaned = s;

                cleaned = cleaned.Replace("\u00A0", " ").Replace("\u200B", "");
                cleaned = cleaned.Replace("（", "(").Replace("）", ")");

                // remove credit patterns + "2 credits"
                cleaned = CreditPat.Replace(cleaned, " ");
                cleaned = Regex.Replace(cleaned, @"\b\d+\s*credits?\b", " ", RegexOptions.IgnoreCase);

                // remove strict IDs
                cleaned = CourseIdPat.Replace(cleaned, " ");

                // remove stars
                cleaned = Regex.Replace(cleaned, @"[★☆✶✸✦✧✩✪✫✬✭✮✯✰✱✳✴]", " ");

                // normalize spaces
                cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

                if (!Regex.IsMatch(cleaned, @"[A-Za-z]"))
                    continue;

                // try split zh/en
                SplitZhEn(cleaned, out _, out string en);
                if (string.IsNullOrWhiteSpace(en))
                    en = cleaned;

                // 3) Add exact keys (still useful)
                string kBase = LowKey(BaseTitle(en));
                if (!string.IsNullOrWhiteSpace(kBase))
                    _validEnglishNameKeys.Add(kBase);

                foreach (var chunk in ExtractEnglishChunks(cleaned))
                {
                    string k = LowKey(BaseTitle(chunk));
                    if (!string.IsNullOrWhiteSpace(k))
                        _validEnglishNameKeys.Add(k);
                }

                foreach (var title in ExtractParenRomanTitles(en))
                {
                    string k = LowKey(BaseTitle(title));
                    if (!string.IsNullOrWhiteSpace(k))
                        _validEnglishNameKeys.Add(k);
                }

                // 4) Strong fix: Mandarin Chinese buried in sentences
                if (cleaned.IndexOf("mandarin chinese", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var romanMatches = Regex.Matches(
                        cleaned,
                        @"mandarin\s+chinese\s*\(\s*(i|ii|iii|iv|v|vi|vii|viii|ix|x)\s*\)",
                        RegexOptions.IgnoreCase);

                    foreach (Match rm in romanMatches)
                    {
                        string roman = (rm.Groups[1].Value ?? "").ToUpperInvariant();
                        if (string.IsNullOrWhiteSpace(roman)) continue;

                        string forcedTitle = $"Mandarin Chinese ({roman})";
                        string forcedKey = LowKey(forcedTitle);
                        if (!string.IsNullOrWhiteSpace(forcedKey))
                            _validEnglishNameKeys.Add(forcedKey);
                    }
                }

                // 5) Build a BIG normalized blob for "contains" matching
                string blob1 = LowKey(en);
                if (!string.IsNullOrWhiteSpace(blob1)) blobParts.Add(blob1);

                string blob2 = LowKey(cleaned);
                if (!string.IsNullOrWhiteSpace(blob2)) blobParts.Add(blob2);
            }

            // Join with separators so we can do "|key|" exact-token contains
            _uploadedEnglishBlobKey = "|" + string.Join("|", blobParts.Distinct()) + "|";
        }

        // Remove suffix details after ":" or ";" so:
        // "English(I): Freshmen English ; advanced-level" -> "English(I)"
        private string BaseTitle(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            s = s.Replace("（", "(").Replace("）", ")");

            int p = s.IndexOf(':');
            if (p >= 0) s = s.Substring(0, p);

            p = s.IndexOf(';');
            if (p >= 0) s = s.Substring(0, p);

            return s.Trim();
        }

        // LowKey with credit stripping (digits) but keeps roman numerals (I/II/III)
        private string LowKey(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";

            s = s.Trim();

            s = s.Replace("\u00A0", " ");
            s = s.Replace("\u200B", "");
            s = s.Replace("（", "(").Replace("）", ")");

            // roman numerals
            s = s.Replace("Ⅰ", "I").Replace("Ⅱ", "II").Replace("Ⅲ", "III").Replace("Ⅳ", "IV").Replace("Ⅴ", "V");
            s = s.Replace("ⅰ", "i").Replace("ⅱ", "ii").Replace("ⅲ", "iii").Replace("ⅳ", "iv").Replace("ⅴ", "v");

            s = s.Replace("–", "-").Replace("—", "-");

            s = s.ToLowerInvariant();

            // common typo
            s = s.Replace("madarin", "mandarin");

            // remove credit digits like "(2)" or "2 credits"
            s = Regex.Replace(s, @"\(\s*\d+\s*\)", " ");
            s = Regex.Replace(s, @"\b\d+\s*credits?\b", " ");

            s = Regex.Replace(s, @"\s+", " ").Trim();

            // keep only letters/digits (removes spaces, punctuation)
            s = Regex.Replace(s, @"[^\p{L}\p{N}]+", "");

            return s.Trim();
        }

        private string NormalizeCourseId(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            s = s.Trim().ToUpperInvariant();

            s = s.Replace("\u00A0", " ").Replace("\u200B", "");
            s = Regex.Replace(s, @"\s+", "");

            // keep base like LC101 even if "LC101-1"
            s = Regex.Replace(s, @"^([A-Z]{2}\d{3}).*$", "$1");

            return s;
        }

        private IEnumerable<string> ExtractEnglishChunks(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) yield break;

            string t = s.Replace("|", " ").Replace("／", " ").Replace("/", " ").Replace("、", " ");
            t = Regex.Replace(t, @"\s+", " ").Trim();

            foreach (Match m in Regex.Matches(t, @"[A-Za-z][A-Za-z0-9\s\-\(\)ⅠⅡⅢⅣⅤⅰⅱⅲⅳⅴ]+"))
            {
                string chunk = (m.Value ?? "").Trim();
                if (chunk.Length < 3) continue;
                yield return chunk;
            }
        }

        // =========================================================
        // Portal paste parser
        // =========================================================
        private List<CourseRow> ParsePortalPaste(string text, string category)
        {
            var result = new List<CourseRow>();
            if (string.IsNullOrWhiteSpace(text)) return result;

            var lines = text.Replace("\r\n", "\n")
                            .Split('\n')
                            .Select(x => (x ?? "").Trim())
                            .Where(x => x.Length > 0)
                            .ToList();

            var patCreditsWord = new Regex(@"\b([A-Z]{2}\d{3})\b\s*\(\s*(\d+)\s*Credits?\s*\)\s*(.+)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var patIdCreditParen = new Regex(@"\b([A-Z]{2}\d{3})\b\s*(?:（\s*(\d+)\s*）|\(\s*(\d+)\s*\))\s*(.*)$",
                RegexOptions.Compiled);

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                // A) "LC101 (2 Credits) English(I): Freshmen English ; advanced-level"
                var mA = patCreditsWord.Match(line);
                if (mA.Success)
                {
                    string cid = mA.Groups[1].Value.Trim();
                    string credits = mA.Groups[2].Value.Trim();
                    string tail = (mA.Groups[3].Value ?? "").Trim();

                    SplitZhEn(tail, out string zhA, out string enA);
                    zhA = RemoveLeadingStar(zhA);

                    result.Add(new CourseRow
                    {
                        CourseID = cid,
                        Credits = credits,
                        EnglishNameCourse = (enA ?? "").Trim(),
                        ChineseNameCourse = (zhA ?? "").Trim(),
                        Category = category
                    });
                    continue;
                }

                // B) "IN101(3) Calculus(I)" or "IN101(3)" alone
                var mB = patIdCreditParen.Match(line);
                if (mB.Success)
                {
                    string cid = mB.Groups[1].Value.Trim();
                    string credits = (!string.IsNullOrEmpty(mB.Groups[2].Value) ? mB.Groups[2].Value : mB.Groups[3].Value).Trim();
                    string tail = (mB.Groups[4].Value ?? "").Trim();

                    if (string.IsNullOrWhiteSpace(tail))
                    {
                        string zh = FindNearestChineseLine(lines, i - 1);
                        string en = FindNearestEnglishLine(lines, i - 1);
                        zh = RemoveLeadingStar(zh);

                        result.Add(new CourseRow
                        {
                            CourseID = cid,
                            Credits = credits,
                            EnglishNameCourse = (en ?? "").Trim(),
                            ChineseNameCourse = (zh ?? "").Trim(),
                            Category = category
                        });
                    }
                    else
                    {
                        SplitZhEn(tail, out string zhB, out string enB);
                        zhB = RemoveLeadingStar(zhB);

                        result.Add(new CourseRow
                        {
                            CourseID = cid,
                            Credits = credits,
                            EnglishNameCourse = (enB ?? "").Trim(),
                            ChineseNameCourse = (zhB ?? "").Trim(),
                            Category = category
                        });
                    }

                    continue;
                }

                // C) fallback: line contains course id somewhere
                var mid = CourseIdPat.Match(line);
                if (mid.Success)
                {
                    string cid = mid.Value.Trim();
                    int? c = ExtractCreditNear(cid, line);
                    string credits = c.HasValue ? c.Value.ToString() : "";

                    string zh = FindNearestChineseLine(lines, i - 1);
                    string en = FindNearestEnglishLine(lines, i - 1);
                    zh = RemoveLeadingStar(zh);

                    result.Add(new CourseRow
                    {
                        CourseID = cid,
                        Credits = credits,
                        EnglishNameCourse = (en ?? "").Trim(),
                        ChineseNameCourse = (zh ?? "").Trim(),
                        Category = category
                    });
                    continue;
                }
            }

            return result;
        }

        private string RemoveLeadingStar(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            s = s.Trim();
            if (s.StartsWith("★")) s = s.Substring(1).Trim();
            return s;
        }

        private string FindNearestChineseLine(List<string> lines, int startIndex)
        {
            for (int k = startIndex; k >= 0 && (startIndex - k) < 6; k--)
            {
                string s = (lines[k] ?? "").Trim();
                if (s.Length == 0) continue;
                if (CourseIdPat.IsMatch(s)) continue;
                if (HasChinese(s)) return s;
            }
            return "";
        }

        private string FindNearestEnglishLine(List<string> lines, int startIndex)
        {
            for (int k = startIndex; k >= 0 && (startIndex - k) < 6; k--)
            {
                string s = (lines[k] ?? "").Trim();
                if (s.Length == 0) continue;
                if (CourseIdPat.IsMatch(s)) continue;
                if (Regex.IsMatch(s, @"[A-Za-z]")) return s;
            }
            return "";
        }

        private bool HasChinese(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            foreach (char ch in s)
            {
                if (ch >= 0x4E00 && ch <= 0x9FFF) return true;
            }
            return false;
        }

        private int? ExtractCreditNear(string courseId, string raw)
        {
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(raw))
                return null;

            int idx = raw.IndexOf(courseId, StringComparison.Ordinal);
            string tail = idx >= 0 ? raw.Substring(idx + courseId.Length) : raw;

            Match m = CreditPat.Match(tail);
            if (!m.Success) return null;

            string c = !string.IsNullOrEmpty(m.Groups[1].Value) ? m.Groups[1].Value : m.Groups[2].Value;
            if (int.TryParse(c, out int val)) return val;
            return null;
        }

        private void SplitZhEn(string nameChunk, out string zh, out string en)
        {
            zh = "";
            en = "";

            if (string.IsNullOrWhiteSpace(nameChunk))
                return;

            string s = CreditPat.Replace(nameChunk, "").Trim();
            s = Regex.Replace(s, @"\s+", " ").Trim();

            Match m = Regex.Match(s, @"[A-Za-z]");
            if (!m.Success)
            {
                zh = s;
                en = "";
                return;
            }

            int cut = m.Index;
            zh = s.Substring(0, cut).Trim(' ', '-', '–', '—', ':', '：');
            en = s.Substring(cut).Trim(' ', '-', '–', '—', ':', '：');
        }

        // =========================================================
        // Universal file -> LINES
        // =========================================================
        private List<string> ReadAnyFileAsLines(string path)
        {
            string ext = (Path.GetExtension(path) ?? "").ToLowerInvariant();

            try
            {
                if (ext == ".txt" || ext == ".csv") return SplitLines(ReadTextShared(path));
                if (ext == ".html" || ext == ".htm") return SplitLines(ReadHtmlFileAsText(path));
                if (ext == ".mhtml" || ext == ".mht") return SplitLines(ReadMhtmlAsText(path));
                if (ext == ".docx") return ReadDocxAsRowLines(path);
                if (ext == ".xlsx" || ext == ".xls") return ReadExcelAsRowLines(path);

                return SplitLines(ReadTextShared(path));
            }
            catch (IOException ioEx)
            {
                MessageBox.Show(
                    "File is being used by another program.\n\n" + ioEx.Message +
                    "\n\nPlease close Word/Excel/WPS or turn off File Explorer Preview pane, then try again.",
                    "File locked",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return new List<string>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to read file:\n" + ex.Message,
                    "Read error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return new List<string>();
            }
        }

        private List<string> SplitLines(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                       .Select(x => (x ?? "").Trim())
                       .Where(x => x.Length > 0)
                       .ToList();
        }

        private string ReadTextShared(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var ms = new MemoryStream())
            {
                fs.CopyTo(ms);
                byte[] bytes = ms.ToArray();

                // UTF-8 BOM
                if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                    return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);

                try { return Encoding.UTF8.GetString(bytes); }
                catch { return Encoding.Default.GetString(bytes); }
            }
        }

        private string ReadHtmlFileAsText(string path)
        {
            string html = ReadTextShared(path);
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var garbage = doc.DocumentNode.Descendants()
                .Where(n => n.Name == "script" || n.Name == "style")
                .ToList();
            foreach (var n in garbage) n.Remove();

            string text = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);
            return NormalizeLines(text);
        }

        private string ReadMhtmlAsText(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var message = MimeMessage.Load(fs);

                string html = message.HtmlBody;
                if (!string.IsNullOrEmpty(html))
                {
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    var garbage = doc.DocumentNode.Descendants()
                        .Where(n => n.Name == "script" || n.Name == "style")
                        .ToList();
                    foreach (var n in garbage) n.Remove();

                    return NormalizeLines(HtmlEntity.DeEntitize(doc.DocumentNode.InnerText));
                }

                return NormalizeLines(message.TextBody ?? "");
            }
        }

        private string NormalizeLines(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            s = Regex.Replace(s, @"\r\n|\r", "\n");
            s = Regex.Replace(s, @"[ \t]+", " ");
            s = Regex.Replace(s, @"\n{3,}", "\n\n");
            return s.Trim();
        }

        // DOCX => add each cell as line + optional combined row
        private List<string> ReadDocxAsRowLines(string path)
        {
            var lines = new List<string>();

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var doc = WordprocessingDocument.Open(fs, false))
            {
                Body body = doc.MainDocumentPart?.Document?.Body;
                if (body == null) return lines;

                foreach (var table in body.Descendants<Table>())
                {
                    foreach (var row in table.Descendants<TableRow>())
                    {
                        var cells = row.Descendants<TableCell>()
                            .Select(c => (c.InnerText ?? "").Trim())
                            .Where(x => x.Length > 0)
                            .ToList();

                        if (cells.Count == 0) continue;

                        lines.AddRange(cells);
                        lines.Add(string.Join(" | ", cells));
                    }
                }

                foreach (var p in body.Descendants<Paragraph>())
                {
                    string t = (p.InnerText ?? "").Trim();
                    if (t.Length > 0) lines.Add(t);
                }
            }

            return lines.Distinct().ToList();
        }

        // Excel => add each cell as line + optional combined row
        private List<string> ReadExcelAsRowLines(string path)
        {
            var lines = new List<string>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = ExcelReaderFactory.CreateReader(fs))
            {
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = false }
                };

                DataSet ds = reader.AsDataSet(conf);

                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var cells = row.ItemArray
                            .Select(x => (x == null ? "" : x.ToString().Trim()))
                            .Where(x => x.Length > 0)
                            .ToList();

                        if (cells.Count == 0) continue;

                        lines.AddRange(cells);
                        lines.Add(string.Join(" | ", cells));
                    }
                }
            }

            return lines.Distinct().ToList();
        }

        private void InitStep3UI()
        {
            btnAddCourse.Click += BtnAddCourse_Click;
            btnUpdateCourse.Click += BtnUpdateCourse_Click;
            btnDeleteCourse.Click += BtnDeleteCourse_Click;
            btnNextPage.Click += btnNextPage_Click;
            txtCategory.Items.Clear();
            txtCategory.Items.AddRange(STEP3_CATEGORY);
            txtCategory.SelectedIndex = 0;
            btnNextPage.Enabled = false;

            lblCourseStatus.ForeColor = WinColor.DarkOrange;
            lblCourseStatus.Text = "Please validate courses first. Then enter Course ID or English Name.";
        }

        private void SetCourseStatus(string text, WinColor color)
        {
            lblCourseStatus.ForeColor = color;
            lblCourseStatus.Text = text;
        }

        private bool GetSearchKey(out string idKey, out string nameKey)
        {
            idKey = NormalizeCourseId(txtCourseID.Text);
            nameKey = LowKey(BaseTitle(txtEnglishNameCourse.Text));

            if (string.IsNullOrWhiteSpace(idKey) && string.IsNullOrWhiteSpace(nameKey))
            {
                SetCourseStatus("Please enter Course ID or English Name.", WinColor.FromArgb(220, 53, 69));
                return false;
            }
            return true;
        }

        private int FindCourseIndex(string idKey, string nameKey)
        {
            for (int i = 0; i < _parsedRows.Count; i++)
            {
                var r = _parsedRows[i];

                string rid = NormalizeCourseId(r.CourseID);
                string rname = LowKey(BaseTitle(r.EnglishNameCourse));

                if (!string.IsNullOrWhiteSpace(idKey) &&
                    rid.Equals(idKey, StringComparison.OrdinalIgnoreCase))
                    return i;

                if (string.IsNullOrWhiteSpace(idKey) &&
                    rname.Equals(nameKey, StringComparison.OrdinalIgnoreCase))
                    return i;

                if (!string.IsNullOrWhiteSpace(idKey) &&
                    !string.IsNullOrWhiteSpace(nameKey) &&
                    rname.Equals(nameKey, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }

        private void RebuildSTT()
        {
            for (int i = 0; i < _parsedRows.Count; i++)
                _parsedRows[i].STT = i + 1;
        }

        private bool EnsureValidatedForStep3()
        {
            if (!_coursesValidated)
            {
                SetCourseStatus("Please click 'Validate Course' first.", WinColor.FromArgb(220, 53, 69));
                return false;
            }
            return true;
        }

        private void BtnAddCourse_Click(object sender, EventArgs e)
        {
            if (!EnsureValidatedForStep3()) return;

            if (!GetSearchKey(out var idKey, out var nameKey)) return;

            int idx = FindCourseIndex(idKey, nameKey);
            if (idx >= 0)
            {
                BtnUpdateCourse_Click(sender, e);
                SetCourseStatus("Course existed. Updated instead.", WinColor.SeaGreen);
                return;
            }

            var row = new CourseRow
            {
                CourseID = txtCourseID.Text.Trim(),
                Credits = txtCredits.Text.Trim(),
                EnglishNameCourse = txtEnglishNameCourse.Text.Trim(),
                Category = txtCategory.SelectedItem?.ToString() ?? ""
            };

            _parsedRows.Add(row);
            RebuildSTT();
            dgvCourses.DataSource = new BindingList<CourseRow>(_parsedRows);

            SetCourseStatus("Course added.", WinColor.SeaGreen);
        }

        private void BtnUpdateCourse_Click(object sender, EventArgs e)
        {
            if (!EnsureValidatedForStep3()) return;

            if (!GetSearchKey(out var idKey, out var nameKey)) return;

            int idx = FindCourseIndex(idKey, nameKey);
            if (idx < 0)
            {
                SetCourseStatus("Course not found.", WinColor.FromArgb(220, 53, 69));
                return;
            }

            var r = _parsedRows[idx];

            if (!string.IsNullOrWhiteSpace(txtCourseID.Text)) r.CourseID = txtCourseID.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtCredits.Text)) r.Credits = txtCredits.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtEnglishNameCourse.Text)) r.EnglishNameCourse = txtEnglishNameCourse.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtCategory.Text)) r.Category = txtCategory.Text.Trim();

            dgvCourses.Refresh();
            SetCourseStatus("Course updated.", WinColor.SeaGreen);
        }

        private void BtnDeleteCourse_Click(object sender, EventArgs e)
        {
            if (!EnsureValidatedForStep3()) return;

            if (!GetSearchKey(out var idKey, out var nameKey)) return;

            int idx = FindCourseIndex(idKey, nameKey);
            if (idx < 0)
            {
                SetCourseStatus("Course not found.", WinColor.FromArgb(220, 53, 69));
                return;
            }

            _parsedRows.RemoveAt(idx);
            RebuildSTT();
            dgvCourses.DataSource = new BindingList<CourseRow>(_parsedRows);

            SetCourseStatus("Course deleted.", WinColor.SeaGreen);
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (!_coursesValidated)
                return;

            Program.Page2.LoadStep5CoursesFromForm1(GetValidatedCoursesForPage2());
            Program.Page2.Show();
            this.Hide();
            Program.Page3.Hide();
        }


        public List<CourseRow> GetValidatedCoursesForPage2()
        {
            return _parsedRows
                .Select(r => new CourseRow
                {
                    STT = r.STT,
                    CourseID = r.CourseID,
                    Credits = r.Credits,
                    EnglishNameCourse = r.EnglishNameCourse,
                    ChineseNameCourse = r.ChineseNameCourse,
                    Category = r.Category
                })
                .ToList();
        }

        public string GetStudentName()
        {
            return student_name ?? "";
        }

        public string GetStudentId()
        {
            return student_id ?? "";
        }

        public string GetStudentDepartment()
        {
            return student_dept ?? "";
        }

        public string GetStudentGrade()
        {
            return student_grade ?? "";
        }
    }
}