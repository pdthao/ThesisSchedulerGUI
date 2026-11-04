namespace ThesisSchedulerGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtStudentId = new System.Windows.Forms.TextBox();
            this.lblStudentId = new System.Windows.Forms.Label();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.cmbDept = new System.Windows.Forms.ComboBox();
            this.lblGrade = new System.Windows.Forms.Label();
            this.cmbGrade = new System.Windows.Forms.ComboBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnValidateCourse = new System.Windows.Forms.Button();
            this.lblUploadStatus = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCommonCourse = new System.Windows.Forms.Label();
            this.lblCompulsoryAndElectiveCourse = new System.Windows.Forms.Label();
            this.txtCommonCourse = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtCompulsoryAndElectiveCourse = new System.Windows.Forms.TextBox();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.btnParseCourses = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCourseID = new System.Windows.Forms.TextBox();
            this.lblCourseID = new System.Windows.Forms.Label();
            this.txtCredits = new System.Windows.Forms.TextBox();
            this.lblCredits = new System.Windows.Forms.Label();
            this.txtEnglishNameCourse = new System.Windows.Forms.TextBox();
            this.lblEnglishNameCourse = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.ComboBox();
            this.btnUpdateCourse = new System.Windows.Forms.Button();
            this.btnAddCourse = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnDeleteCourse = new System.Windows.Forms.Button();
            this.lblCourseStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(22, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Step 1 — Student Info";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblName.Location = new System.Drawing.Point(81, 85);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(55, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtName.Location = new System.Drawing.Point(172, 79);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(599, 26);
            this.txtName.TabIndex = 2;
            // 
            // txtStudentId
            // 
            this.txtStudentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtStudentId.Location = new System.Drawing.Point(172, 134);
            this.txtStudentId.Name = "txtStudentId";
            this.txtStudentId.Size = new System.Drawing.Size(599, 26);
            this.txtStudentId.TabIndex = 4;
            // 
            // lblStudentId
            // 
            this.lblStudentId.AutoSize = true;
            this.lblStudentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblStudentId.Location = new System.Drawing.Point(52, 140);
            this.lblStudentId.Name = "lblStudentId";
            this.lblStudentId.Size = new System.Drawing.Size(91, 20);
            this.lblStudentId.TabIndex = 3;
            this.lblStudentId.Text = "Student ID:";
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblDepartment.Location = new System.Drawing.Point(45, 195);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(98, 20);
            this.lblDepartment.TabIndex = 5;
            this.lblDepartment.Text = "Department:";
            // 
            // cmbDept
            // 
            this.cmbDept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbDept.FormattingEnabled = true;
            this.cmbDept.Location = new System.Drawing.Point(172, 187);
            this.cmbDept.Name = "cmbDept";
            this.cmbDept.Size = new System.Drawing.Size(599, 28);
            this.cmbDept.TabIndex = 6;
            // 
            // lblGrade
            // 
            this.lblGrade.AutoSize = true;
            this.lblGrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblGrade.Location = new System.Drawing.Point(78, 243);
            this.lblGrade.Name = "lblGrade";
            this.lblGrade.Size = new System.Drawing.Size(58, 20);
            this.lblGrade.TabIndex = 7;
            this.lblGrade.Text = "Grade:";
            // 
            // cmbGrade
            // 
            this.cmbGrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbGrade.FormattingEnabled = true;
            this.cmbGrade.Location = new System.Drawing.Point(172, 235);
            this.cmbGrade.Name = "cmbGrade";
            this.cmbGrade.Size = new System.Drawing.Size(599, 28);
            this.cmbGrade.TabIndex = 8;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnConfirm.Location = new System.Drawing.Point(475, 305);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(103, 29);
            this.btnConfirm.TabIndex = 9;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnEdit.Location = new System.Drawing.Point(643, 305);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(128, 29);
            this.btnEdit.TabIndex = 10;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(77, 314);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(65, 20);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "Ready.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(10, 388);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(313, 22);
            this.label2.TabIndex = 12;
            this.label2.Text = "Step 2 — Upload List Of Required Courses";
            // 
            // btnValidateCourse
            // 
            this.btnValidateCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnValidateCourse.Location = new System.Drawing.Point(793, 848);
            this.btnValidateCourse.Name = "btnValidateCourse";
            this.btnValidateCourse.Size = new System.Drawing.Size(113, 57);
            this.btnValidateCourse.TabIndex = 13;
            this.btnValidateCourse.Text = "Validate Course";
            this.btnValidateCourse.UseVisualStyleBackColor = true;
            // 
            // lblUploadStatus
            // 
            this.lblUploadStatus.AutoSize = true;
            this.lblUploadStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblUploadStatus.Location = new System.Drawing.Point(472, 388);
            this.lblUploadStatus.Name = "lblUploadStatus";
            this.lblUploadStatus.Size = new System.Drawing.Size(65, 20);
            this.lblUploadStatus.TabIndex = 14;
            this.lblUploadStatus.Text = "Ready.";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(973, 388);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(896, 662);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(18, 485);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(325, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Portal -> Course Approval -> 修業科目表";
            // 
            // lblCommonCourse
            // 
            this.lblCommonCourse.AutoSize = true;
            this.lblCommonCourse.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCommonCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCommonCourse.Location = new System.Drawing.Point(85, 540);
            this.lblCommonCourse.Name = "lblCommonCourse";
            this.lblCommonCourse.Size = new System.Drawing.Size(143, 22);
            this.lblCommonCourse.TabIndex = 17;
            this.lblCommonCourse.Text = "Common Course";
            // 
            // lblCompulsoryAndElectiveCourse
            // 
            this.lblCompulsoryAndElectiveCourse.AutoSize = true;
            this.lblCompulsoryAndElectiveCourse.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCompulsoryAndElectiveCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCompulsoryAndElectiveCourse.Location = new System.Drawing.Point(431, 540);
            this.lblCompulsoryAndElectiveCourse.Name = "lblCompulsoryAndElectiveCourse";
            this.lblCompulsoryAndElectiveCourse.Size = new System.Drawing.Size(271, 22);
            this.lblCompulsoryAndElectiveCourse.TabIndex = 18;
            this.lblCompulsoryAndElectiveCourse.Text = "Compulsory And Elective Course";
            // 
            // txtCommonCourse
            // 
            this.txtCommonCourse.AcceptsReturn = true;
            this.txtCommonCourse.AcceptsTab = true;
            this.txtCommonCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCommonCourse.Location = new System.Drawing.Point(22, 585);
            this.txtCommonCourse.Multiline = true;
            this.txtCommonCourse.Name = "txtCommonCourse";
            this.txtCommonCourse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCommonCourse.Size = new System.Drawing.Size(313, 467);
            this.txtCommonCourse.TabIndex = 19;
            this.txtCommonCourse.WordWrap = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtCompulsoryAndElectiveCourse
            // 
            this.txtCompulsoryAndElectiveCourse.AcceptsReturn = true;
            this.txtCompulsoryAndElectiveCourse.AcceptsTab = true;
            this.txtCompulsoryAndElectiveCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCompulsoryAndElectiveCourse.Location = new System.Drawing.Point(382, 585);
            this.txtCompulsoryAndElectiveCourse.Multiline = true;
            this.txtCompulsoryAndElectiveCourse.Name = "txtCompulsoryAndElectiveCourse";
            this.txtCompulsoryAndElectiveCourse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCompulsoryAndElectiveCourse.Size = new System.Drawing.Size(348, 467);
            this.txtCompulsoryAndElectiveCourse.TabIndex = 21;
            this.txtCompulsoryAndElectiveCourse.WordWrap = false;
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnUploadFile.Location = new System.Drawing.Point(793, 768);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(113, 55);
            this.btnUploadFile.TabIndex = 22;
            this.btnUploadFile.Text = "Upload File";
            this.btnUploadFile.UseVisualStyleBackColor = true;
            // 
            // btnParseCourses
            // 
            this.btnParseCourses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnParseCourses.Location = new System.Drawing.Point(793, 705);
            this.btnParseCourses.Name = "btnParseCourses";
            this.btnParseCourses.Size = new System.Drawing.Size(113, 36);
            this.btnParseCourses.TabIndex = 23;
            this.btnParseCourses.Text = "Export";
            this.btnParseCourses.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(943, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(316, 22);
            this.label4.TabIndex = 24;
            this.label4.Text = "Step 3 — Physically Added/ Delete Courses";
            // 
            // txtCourseID
            // 
            this.txtCourseID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCourseID.Location = new System.Drawing.Point(1136, 83);
            this.txtCourseID.Name = "txtCourseID";
            this.txtCourseID.Size = new System.Drawing.Size(599, 26);
            this.txtCourseID.TabIndex = 26;
            // 
            // lblCourseID
            // 
            this.lblCourseID.AutoSize = true;
            this.lblCourseID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCourseID.Location = new System.Drawing.Point(996, 83);
            this.lblCourseID.Name = "lblCourseID";
            this.lblCourseID.Size = new System.Drawing.Size(85, 20);
            this.lblCourseID.TabIndex = 25;
            this.lblCourseID.Text = "Course ID:";
            // 
            // txtCredits
            // 
            this.txtCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCredits.Location = new System.Drawing.Point(1136, 132);
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.Size = new System.Drawing.Size(599, 26);
            this.txtCredits.TabIndex = 28;
            // 
            // lblCredits
            // 
            this.lblCredits.AutoSize = true;
            this.lblCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCredits.Location = new System.Drawing.Point(1018, 135);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(63, 20);
            this.lblCredits.TabIndex = 27;
            this.lblCredits.Text = "Credits:";
            // 
            // txtEnglishNameCourse
            // 
            this.txtEnglishNameCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtEnglishNameCourse.Location = new System.Drawing.Point(1136, 185);
            this.txtEnglishNameCourse.Name = "txtEnglishNameCourse";
            this.txtEnglishNameCourse.Size = new System.Drawing.Size(599, 26);
            this.txtEnglishNameCourse.TabIndex = 30;
            // 
            // lblEnglishNameCourse
            // 
            this.lblEnglishNameCourse.AutoSize = true;
            this.lblEnglishNameCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblEnglishNameCourse.Location = new System.Drawing.Point(915, 188);
            this.lblEnglishNameCourse.Name = "lblEnglishNameCourse";
            this.lblEnglishNameCourse.Size = new System.Drawing.Size(166, 20);
            this.lblEnglishNameCourse.TabIndex = 29;
            this.lblEnglishNameCourse.Text = "English Name Course:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCategory.Location = new System.Drawing.Point(1004, 236);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(77, 20);
            this.lblCategory.TabIndex = 31;
            this.lblCategory.Text = "Category:";
            // 
            // txtCategory
            // 
            this.txtCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCategory.FormattingEnabled = true;
            this.txtCategory.Location = new System.Drawing.Point(1136, 231);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(599, 28);
            this.txtCategory.TabIndex = 33;
            // 
            // btnUpdateCourse
            // 
            this.btnUpdateCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnUpdateCourse.Location = new System.Drawing.Point(1329, 291);
            this.btnUpdateCourse.Name = "btnUpdateCourse";
            this.btnUpdateCourse.Size = new System.Drawing.Size(128, 29);
            this.btnUpdateCourse.TabIndex = 35;
            this.btnUpdateCourse.Text = "Update Course";
            this.btnUpdateCourse.UseVisualStyleBackColor = true;
            // 
            // btnAddCourse
            // 
            this.btnAddCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnAddCourse.Location = new System.Drawing.Point(1120, 291);
            this.btnAddCourse.Name = "btnAddCourse";
            this.btnAddCourse.Size = new System.Drawing.Size(128, 29);
            this.btnAddCourse.TabIndex = 36;
            this.btnAddCourse.Text = "Add Course";
            this.btnAddCourse.UseVisualStyleBackColor = true;
            // 
            // btnNextPage
            // 
            this.btnNextPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnNextPage.Location = new System.Drawing.Point(1769, 1069);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(128, 29);
            this.btnNextPage.TabIndex = 37;
            this.btnNextPage.Text = "Next Page";
            this.btnNextPage.UseVisualStyleBackColor = true;
            // 
            // btnDeleteCourse
            // 
            this.btnDeleteCourse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnDeleteCourse.Location = new System.Drawing.Point(1523, 291);
            this.btnDeleteCourse.Name = "btnDeleteCourse";
            this.btnDeleteCourse.Size = new System.Drawing.Size(128, 29);
            this.btnDeleteCourse.TabIndex = 38;
            this.btnDeleteCourse.Text = "Delete Course";
            this.btnDeleteCourse.UseVisualStyleBackColor = true;
            // 
            // lblCourseStatus
            // 
            this.lblCourseStatus.AutoSize = true;
            this.lblCourseStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCourseStatus.Location = new System.Drawing.Point(1116, 348);
            this.lblCourseStatus.Name = "lblCourseStatus";
            this.lblCourseStatus.Size = new System.Drawing.Size(65, 20);
            this.lblCourseStatus.TabIndex = 39;
            this.lblCourseStatus.Text = "Ready.";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1900, 1120);
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.Controls.Add(this.lblCourseStatus);
            this.Controls.Add(this.btnDeleteCourse);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnAddCourse);
            this.Controls.Add(this.btnUpdateCourse);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtEnglishNameCourse);
            this.Controls.Add(this.lblEnglishNameCourse);
            this.Controls.Add(this.txtCredits);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.txtCourseID);
            this.Controls.Add(this.lblCourseID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnParseCourses);
            this.Controls.Add(this.btnUploadFile);
            this.Controls.Add(this.txtCompulsoryAndElectiveCourse);
            this.Controls.Add(this.txtCommonCourse);
            this.Controls.Add(this.lblCompulsoryAndElectiveCourse);
            this.Controls.Add(this.lblCommonCourse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblUploadStatus);
            this.Controls.Add(this.btnValidateCourse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.cmbGrade);
            this.Controls.Add(this.lblGrade);
            this.Controls.Add(this.cmbDept);
            this.Controls.Add(this.lblDepartment);
            this.Controls.Add(this.txtStudentId);
            this.Controls.Add(this.lblStudentId);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtStudentId;
        private System.Windows.Forms.Label lblStudentId;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.ComboBox cmbDept;
        private System.Windows.Forms.Label lblGrade;
        private System.Windows.Forms.ComboBox cmbGrade;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnValidateCourse;
        private System.Windows.Forms.Label lblUploadStatus;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCommonCourse;
        private System.Windows.Forms.Label lblCompulsoryAndElectiveCourse;
        private System.Windows.Forms.TextBox txtCommonCourse;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txtCompulsoryAndElectiveCourse;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.Button btnParseCourses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCourseID;
        private System.Windows.Forms.Label lblCourseID;
        private System.Windows.Forms.TextBox txtCredits;
        private System.Windows.Forms.Label lblCredits;
        private System.Windows.Forms.TextBox txtEnglishNameCourse;
        private System.Windows.Forms.Label lblEnglishNameCourse;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox txtCategory;
        private System.Windows.Forms.Button btnUpdateCourse;
        private System.Windows.Forms.Button btnAddCourse;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnDeleteCourse;
        private System.Windows.Forms.Label lblCourseStatus;
    }
}

