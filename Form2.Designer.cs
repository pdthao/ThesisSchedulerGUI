namespace ThesisSchedulerGUI
{
    partial class Form2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUploadFileStep5 = new System.Windows.Forms.Button();
            this.dataGridViewStep5 = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dataGridView2Page2 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDept = new System.Windows.Forms.ComboBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddCoursePreferences = new System.Windows.Forms.Button();
            this.btnExportStep5Excel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStep5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2Page2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(30, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "Step 5 — Upload Completed Course Record";
            // 
            // btnUploadFileStep5
            // 
            this.btnUploadFileStep5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnUploadFileStep5.Location = new System.Drawing.Point(30, 82);
            this.btnUploadFileStep5.Name = "btnUploadFileStep5";
            this.btnUploadFileStep5.Size = new System.Drawing.Size(103, 29);
            this.btnUploadFileStep5.TabIndex = 10;
            this.btnUploadFileStep5.Text = "Upload File";
            this.btnUploadFileStep5.UseVisualStyleBackColor = true;
            // 
            // dataGridViewStep5
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewStep5.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewStep5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStep5.Location = new System.Drawing.Point(12, 145);
            this.dataGridViewStep5.Name = "dataGridViewStep5";
            this.dataGridViewStep5.Size = new System.Drawing.Size(716, 868);
            this.dataGridViewStep5.TabIndex = 16;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(189, 82);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(65, 20);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = "Ready.";
            // 
            // dataGridView2Page2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2Page2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2Page2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2Page2.Location = new System.Drawing.Point(769, 145);
            this.dataGridView2Page2.Name = "dataGridView2Page2";
            this.dataGridView2Page2.Size = new System.Drawing.Size(1122, 868);
            this.dataGridView2Page2.TabIndex = 18;
            this.dataGridView2Page2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2Page2_CellContentClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(769, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(389, 22);
            this.label2.TabIndex = 19;
            this.label2.Text = "Step 6 — Current Semester Timetable Of Department";
            // 
            // comboBoxDept
            // 
            this.comboBoxDept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxDept.FormattingEnabled = true;
            this.comboBoxDept.Location = new System.Drawing.Point(769, 73);
            this.comboBoxDept.Name = "comboBoxDept";
            this.comboBoxDept.Size = new System.Drawing.Size(731, 28);
            this.comboBoxDept.TabIndex = 20;
            // 
            // btnChoose
            // 
            this.btnChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnChoose.Location = new System.Drawing.Point(1526, 73);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(97, 29);
            this.btnChoose.TabIndex = 21;
            this.btnChoose.Text = "Choose";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnDelete.Location = new System.Drawing.Point(1526, 25);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(97, 29);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAddCoursePreferences
            // 
            this.btnAddCoursePreferences.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnAddCoursePreferences.Location = new System.Drawing.Point(1533, 1050);
            this.btnAddCoursePreferences.Name = "btnAddCoursePreferences";
            this.btnAddCoursePreferences.Size = new System.Drawing.Size(358, 31);
            this.btnAddCoursePreferences.TabIndex = 23;
            this.btnAddCoursePreferences.Text = "Next Page - Add Course Preferences";
            this.btnAddCoursePreferences.UseVisualStyleBackColor = true;
            this.btnAddCoursePreferences.Click += new System.EventHandler(this.btnAddCoursePreferences_Click);
            // 
            // btnExportStep5Excel
            // 
            this.btnExportStep5Excel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnExportStep5Excel.Location = new System.Drawing.Point(456, 1050);
            this.btnExportStep5Excel.Name = "btnExportStep5Excel";
            this.btnExportStep5Excel.Size = new System.Drawing.Size(272, 31);
            this.btnExportStep5Excel.TabIndex = 24;
            this.btnExportStep5Excel.Text = "Export Completed Course List";
            this.btnExportStep5Excel.UseVisualStyleBackColor = true;
            this.btnExportStep5Excel.Click += new System.EventHandler(this.btnExportStep5Excel_Click);
            // 
            // Form2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1900, 1120);
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.Controls.Add(this.btnExportStep5Excel);
            this.Controls.Add(this.btnAddCoursePreferences);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.comboBoxDept);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView2Page2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.dataGridViewStep5);
            this.Controls.Add(this.btnUploadFileStep5);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStep5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2Page2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUploadFileStep5;
        private System.Windows.Forms.DataGridView dataGridViewStep5;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView dataGridView2Page2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxDept;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAddCoursePreferences;
        private System.Windows.Forms.Button btnExportStep5Excel;
    }
}