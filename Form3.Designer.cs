namespace ThesisSchedulerGUI
{
    partial class Form3
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
            this.label2 = new System.Windows.Forms.Label();
            this.lblCourseName = new System.Windows.Forms.Label();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.lblTimeSections = new System.Windows.Forms.Label();
            this.lblClassDays = new System.Windows.Forms.Label();
            this.lblCreditNunber = new System.Windows.Forms.Label();
            this.lblProfessorName = new System.Windows.Forms.Label();
            this.comboBoxCreditNumber = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.checkBox20 = new System.Windows.Forms.CheckBox();
            this.comboBoxAcademicYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCourseName = new System.Windows.Forms.TextBox();
            this.txtProfessorName = new System.Windows.Forms.TextBox();
            this.checkBox21 = new System.Windows.Forms.CheckBox();
            this.checkBox22 = new System.Windows.Forms.CheckBox();
            this.checkBox23 = new System.Windows.Forms.CheckBox();
            this.checkBox24 = new System.Windows.Forms.CheckBox();
            this.checkBox25 = new System.Windows.Forms.CheckBox();
            this.checkBox26 = new System.Windows.Forms.CheckBox();
            this.checkBox27 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxOtherTimeSlotAvoid = new System.Windows.Forms.ComboBox();
            this.checkBox28 = new System.Windows.Forms.CheckBox();
            this.checkBoxOtherPreferredCredits = new System.Windows.Forms.CheckBox();
            this.txtOtherPreferredCredits = new System.Windows.Forms.TextBox();
            this.txtOtherTimeSlotAvoid = new System.Windows.Forms.TextBox();
            this.checkBoxOtherTimeSlotAvoid = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridRecommendSchedule = new System.Windows.Forms.DataGridView();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnGenerateSchedule = new System.Windows.Forms.Button();
            this.btnSaveScheduleAsImage = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnChoose = new System.Windows.Forms.Button();
            this.comboBoxDept = new System.Windows.Forms.ComboBox();
            this.dataGridViewPage3 = new System.Windows.Forms.DataGridView();
            this.btnApplySelectedExtraCourses = new System.Windows.Forms.Button();
            this.btnGraduationSummary = new System.Windows.Forms.Button();
            this.btnBackToStep1 = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridRecommendSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPage3)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(33, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(300, 22);
            this.label2.TabIndex = 20;
            this.label2.Text = "Step 7 — Define Scheduling Preferences";
            // 
            // lblCourseName
            // 
            this.lblCourseName.AutoSize = true;
            this.lblCourseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCourseName.Location = new System.Drawing.Point(62, 518);
            this.lblCourseName.Name = "lblCourseName";
            this.lblCourseName.Size = new System.Drawing.Size(110, 20);
            this.lblCourseName.TabIndex = 27;
            this.lblCourseName.Text = "Course Name:";
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(223, 632);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(799, 28);
            this.comboBoxLanguage.TabIndex = 26;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguage_SelectedIndexChanged);
            // 
            // lblTimeSections
            // 
            this.lblTimeSections.AutoSize = true;
            this.lblTimeSections.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblTimeSections.Location = new System.Drawing.Point(15, 304);
            this.lblTimeSections.Name = "lblTimeSections";
            this.lblTimeSections.Size = new System.Drawing.Size(157, 20);
            this.lblTimeSections.TabIndex = 25;
            this.lblTimeSections.Text = "Preferred Time Slots:";
            // 
            // lblClassDays
            // 
            this.lblClassDays.AutoSize = true;
            this.lblClassDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblClassDays.Location = new System.Drawing.Point(10, 197);
            this.lblClassDays.Name = "lblClassDays";
            this.lblClassDays.Size = new System.Drawing.Size(162, 20);
            this.lblClassDays.TabIndex = 23;
            this.lblClassDays.Text = "Preferred Class Days:";
            // 
            // lblCreditNunber
            // 
            this.lblCreditNunber.AutoSize = true;
            this.lblCreditNunber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCreditNunber.Location = new System.Drawing.Point(39, 142);
            this.lblCreditNunber.Name = "lblCreditNunber";
            this.lblCreditNunber.Size = new System.Drawing.Size(133, 20);
            this.lblCreditNunber.TabIndex = 21;
            this.lblCreditNunber.Text = "Preferred Credits:";
            // 
            // lblProfessorName
            // 
            this.lblProfessorName.AutoSize = true;
            this.lblProfessorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblProfessorName.Location = new System.Drawing.Point(45, 571);
            this.lblProfessorName.Name = "lblProfessorName";
            this.lblProfessorName.Size = new System.Drawing.Size(127, 20);
            this.lblProfessorName.TabIndex = 29;
            this.lblProfessorName.Text = "Professor Name:";
            // 
            // comboBoxCreditNumber
            // 
            this.comboBoxCreditNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxCreditNumber.FormattingEnabled = true;
            this.comboBoxCreditNumber.Location = new System.Drawing.Point(224, 134);
            this.comboBoxCreditNumber.Name = "comboBoxCreditNumber";
            this.comboBoxCreditNumber.Size = new System.Drawing.Size(277, 28);
            this.comboBoxCreditNumber.TabIndex = 32;
            this.comboBoxCreditNumber.SelectedIndexChanged += new System.EventHandler(this.comboBoxCreditNumber_SelectedIndexChanged);
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblLanguage.Location = new System.Drawing.Point(86, 635);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(85, 20);
            this.lblLanguage.TabIndex = 31;
            this.lblLanguage.Text = "Language:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox1.Location = new System.Drawing.Point(224, 193);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 24);
            this.checkBox1.TabIndex = 33;
            this.checkBox1.Text = "Monday";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox2.Location = new System.Drawing.Point(334, 193);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(88, 24);
            this.checkBox2.TabIndex = 34;
            this.checkBox2.Text = "Tuesday";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox3.Location = new System.Drawing.Point(588, 193);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(93, 24);
            this.checkBox3.TabIndex = 36;
            this.checkBox3.Text = "Thursday";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox4.Location = new System.Drawing.Point(451, 193);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(112, 24);
            this.checkBox4.TabIndex = 35;
            this.checkBox4.Text = "Wednesday";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox5.Location = new System.Drawing.Point(707, 193);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(71, 24);
            this.checkBox5.TabIndex = 37;
            this.checkBox5.Text = "Friday";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox6.Location = new System.Drawing.Point(811, 193);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(92, 24);
            this.checkBox6.TabIndex = 38;
            this.checkBox6.Text = "Saturday";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox7.Location = new System.Drawing.Point(941, 193);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(82, 24);
            this.checkBox7.TabIndex = 39;
            this.checkBox7.Text = "Sunday";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox8.Location = new System.Drawing.Point(224, 305);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(113, 24);
            this.checkBox8.TabIndex = 40;
            this.checkBox8.Text = "08:10-09:00";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox9.Location = new System.Drawing.Point(372, 305);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(113, 24);
            this.checkBox9.TabIndex = 41;
            this.checkBox9.Text = "09:10-10:00";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox10.Location = new System.Drawing.Point(665, 305);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(113, 24);
            this.checkBox10.TabIndex = 43;
            this.checkBox10.Text = "11:10-12:00";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox11.Location = new System.Drawing.Point(517, 305);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(113, 24);
            this.checkBox11.TabIndex = 42;
            this.checkBox11.Text = "10:10-11:00";
            this.checkBox11.UseVisualStyleBackColor = true;
            this.checkBox11.CheckedChanged += new System.EventHandler(this.checkBox11_CheckedChanged);
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox12.Location = new System.Drawing.Point(811, 304);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(113, 24);
            this.checkBox12.TabIndex = 44;
            this.checkBox12.Text = "12:10-13:00";
            this.checkBox12.UseVisualStyleBackColor = true;
            this.checkBox12.CheckedChanged += new System.EventHandler(this.checkBox12_CheckedChanged);
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox13.Location = new System.Drawing.Point(811, 353);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(113, 24);
            this.checkBox13.TabIndex = 49;
            this.checkBox13.Text = "17:10-18:00";
            this.checkBox13.UseVisualStyleBackColor = true;
            this.checkBox13.CheckedChanged += new System.EventHandler(this.checkBox13_CheckedChanged);
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox14.Location = new System.Drawing.Point(665, 354);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(113, 24);
            this.checkBox14.TabIndex = 48;
            this.checkBox14.Text = "16:10-17:00";
            this.checkBox14.UseVisualStyleBackColor = true;
            this.checkBox14.CheckedChanged += new System.EventHandler(this.checkBox14_CheckedChanged);
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox15.Location = new System.Drawing.Point(517, 354);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(113, 24);
            this.checkBox15.TabIndex = 47;
            this.checkBox15.Text = "15:10-16:00";
            this.checkBox15.UseVisualStyleBackColor = true;
            this.checkBox15.CheckedChanged += new System.EventHandler(this.checkBox15_CheckedChanged);
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox16.Location = new System.Drawing.Point(372, 354);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(113, 24);
            this.checkBox16.TabIndex = 46;
            this.checkBox16.Text = "14:10-15:00";
            this.checkBox16.UseVisualStyleBackColor = true;
            this.checkBox16.CheckedChanged += new System.EventHandler(this.checkBox16_CheckedChanged);
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox17.Location = new System.Drawing.Point(224, 354);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(113, 24);
            this.checkBox17.TabIndex = 45;
            this.checkBox17.Text = "13:10-14:00";
            this.checkBox17.UseVisualStyleBackColor = true;
            this.checkBox17.CheckedChanged += new System.EventHandler(this.checkBox17_CheckedChanged);
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox18.Location = new System.Drawing.Point(517, 399);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(113, 24);
            this.checkBox18.TabIndex = 52;
            this.checkBox18.Text = "20:30-21:20";
            this.checkBox18.UseVisualStyleBackColor = true;
            this.checkBox18.CheckedChanged += new System.EventHandler(this.checkBox18_CheckedChanged);
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox19.Location = new System.Drawing.Point(371, 400);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(113, 24);
            this.checkBox19.TabIndex = 51;
            this.checkBox19.Text = "19:30-20:20";
            this.checkBox19.UseVisualStyleBackColor = true;
            this.checkBox19.CheckedChanged += new System.EventHandler(this.checkBox19_CheckedChanged);
            // 
            // checkBox20
            // 
            this.checkBox20.AutoSize = true;
            this.checkBox20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox20.Location = new System.Drawing.Point(223, 400);
            this.checkBox20.Name = "checkBox20";
            this.checkBox20.Size = new System.Drawing.Size(113, 24);
            this.checkBox20.TabIndex = 50;
            this.checkBox20.Text = "18:30-19:20";
            this.checkBox20.UseVisualStyleBackColor = true;
            this.checkBox20.CheckedChanged += new System.EventHandler(this.checkBox20_CheckedChanged);
            // 
            // comboBoxAcademicYear
            // 
            this.comboBoxAcademicYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxAcademicYear.FormattingEnabled = true;
            this.comboBoxAcademicYear.Location = new System.Drawing.Point(223, 83);
            this.comboBoxAcademicYear.Name = "comboBoxAcademicYear";
            this.comboBoxAcademicYear.Size = new System.Drawing.Size(800, 28);
            this.comboBoxAcademicYear.TabIndex = 54;
            this.comboBoxAcademicYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxAcademicYear_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(54, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 53;
            this.label1.Text = "Academic Year:";
            // 
            // txtCourseName
            // 
            this.txtCourseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCourseName.Location = new System.Drawing.Point(223, 518);
            this.txtCourseName.Name = "txtCourseName";
            this.txtCourseName.Size = new System.Drawing.Size(799, 26);
            this.txtCourseName.TabIndex = 55;
            this.txtCourseName.TextChanged += new System.EventHandler(this.txtCourseName_TextChanged);
            // 
            // txtProfessorName
            // 
            this.txtProfessorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtProfessorName.Location = new System.Drawing.Point(223, 571);
            this.txtProfessorName.Name = "txtProfessorName";
            this.txtProfessorName.Size = new System.Drawing.Size(799, 26);
            this.txtProfessorName.TabIndex = 56;
            this.txtProfessorName.TextChanged += new System.EventHandler(this.txtProfessorName_TextChanged);
            // 
            // checkBox21
            // 
            this.checkBox21.AutoSize = true;
            this.checkBox21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox21.Location = new System.Drawing.Point(941, 241);
            this.checkBox21.Name = "checkBox21";
            this.checkBox21.Size = new System.Drawing.Size(82, 24);
            this.checkBox21.TabIndex = 64;
            this.checkBox21.Text = "Sunday";
            this.checkBox21.UseVisualStyleBackColor = true;
            this.checkBox21.CheckedChanged += new System.EventHandler(this.checkBox21_CheckedChanged);
            // 
            // checkBox22
            // 
            this.checkBox22.AutoSize = true;
            this.checkBox22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox22.Location = new System.Drawing.Point(811, 241);
            this.checkBox22.Name = "checkBox22";
            this.checkBox22.Size = new System.Drawing.Size(92, 24);
            this.checkBox22.TabIndex = 63;
            this.checkBox22.Text = "Saturday";
            this.checkBox22.UseVisualStyleBackColor = true;
            this.checkBox22.CheckedChanged += new System.EventHandler(this.checkBox22_CheckedChanged);
            // 
            // checkBox23
            // 
            this.checkBox23.AutoSize = true;
            this.checkBox23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox23.Location = new System.Drawing.Point(707, 241);
            this.checkBox23.Name = "checkBox23";
            this.checkBox23.Size = new System.Drawing.Size(71, 24);
            this.checkBox23.TabIndex = 62;
            this.checkBox23.Text = "Friday";
            this.checkBox23.UseVisualStyleBackColor = true;
            this.checkBox23.CheckedChanged += new System.EventHandler(this.checkBox23_CheckedChanged);
            // 
            // checkBox24
            // 
            this.checkBox24.AutoSize = true;
            this.checkBox24.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox24.Location = new System.Drawing.Point(588, 241);
            this.checkBox24.Name = "checkBox24";
            this.checkBox24.Size = new System.Drawing.Size(93, 24);
            this.checkBox24.TabIndex = 61;
            this.checkBox24.Text = "Thursday";
            this.checkBox24.UseVisualStyleBackColor = true;
            this.checkBox24.CheckedChanged += new System.EventHandler(this.checkBox24_CheckedChanged);
            // 
            // checkBox25
            // 
            this.checkBox25.AutoSize = true;
            this.checkBox25.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox25.Location = new System.Drawing.Point(451, 241);
            this.checkBox25.Name = "checkBox25";
            this.checkBox25.Size = new System.Drawing.Size(112, 24);
            this.checkBox25.TabIndex = 60;
            this.checkBox25.Text = "Wednesday";
            this.checkBox25.UseVisualStyleBackColor = true;
            this.checkBox25.CheckedChanged += new System.EventHandler(this.checkBox25_CheckedChanged);
            // 
            // checkBox26
            // 
            this.checkBox26.AutoSize = true;
            this.checkBox26.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox26.Location = new System.Drawing.Point(334, 241);
            this.checkBox26.Name = "checkBox26";
            this.checkBox26.Size = new System.Drawing.Size(88, 24);
            this.checkBox26.TabIndex = 59;
            this.checkBox26.Text = "Tuesday";
            this.checkBox26.UseVisualStyleBackColor = true;
            this.checkBox26.CheckedChanged += new System.EventHandler(this.checkBox26_CheckedChanged);
            // 
            // checkBox27
            // 
            this.checkBox27.AutoSize = true;
            this.checkBox27.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox27.Location = new System.Drawing.Point(224, 241);
            this.checkBox27.Name = "checkBox27";
            this.checkBox27.Size = new System.Drawing.Size(84, 24);
            this.checkBox27.TabIndex = 58;
            this.checkBox27.Text = "Monday";
            this.checkBox27.UseVisualStyleBackColor = true;
            this.checkBox27.CheckedChanged += new System.EventHandler(this.checkBox27_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(35, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 20);
            this.label3.TabIndex = 57;
            this.label3.Text = "Preferred Day Off:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(5, 458);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 20);
            this.label4.TabIndex = 66;
            this.label4.Text = "Other Time Slot Avoid:";
            // 
            // comboBoxOtherTimeSlotAvoid
            // 
            this.comboBoxOtherTimeSlotAvoid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxOtherTimeSlotAvoid.FormattingEnabled = true;
            this.comboBoxOtherTimeSlotAvoid.Location = new System.Drawing.Point(223, 458);
            this.comboBoxOtherTimeSlotAvoid.Name = "comboBoxOtherTimeSlotAvoid";
            this.comboBoxOtherTimeSlotAvoid.Size = new System.Drawing.Size(278, 28);
            this.comboBoxOtherTimeSlotAvoid.TabIndex = 65;
            this.comboBoxOtherTimeSlotAvoid.SelectedIndexChanged += new System.EventHandler(this.comboBoxOtherTimeSlotAvoid_SelectedIndexChanged);
            // 
            // checkBox28
            // 
            this.checkBox28.AutoSize = true;
            this.checkBox28.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBox28.Location = new System.Drawing.Point(440, 31);
            this.checkBox28.Name = "checkBox28";
            this.checkBox28.Size = new System.Drawing.Size(149, 24);
            this.checkBox28.TabIndex = 67;
            this.checkBox28.Text = "No Need Setting ";
            this.checkBox28.UseVisualStyleBackColor = true;
            this.checkBox28.CheckedChanged += new System.EventHandler(this.checkBox28_CheckedChanged);
            // 
            // checkBoxOtherPreferredCredits
            // 
            this.checkBoxOtherPreferredCredits.AutoSize = true;
            this.checkBoxOtherPreferredCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBoxOtherPreferredCredits.Location = new System.Drawing.Point(532, 138);
            this.checkBoxOtherPreferredCredits.Name = "checkBoxOtherPreferredCredits";
            this.checkBoxOtherPreferredCredits.Size = new System.Drawing.Size(68, 24);
            this.checkBoxOtherPreferredCredits.TabIndex = 68;
            this.checkBoxOtherPreferredCredits.Text = "Other";
            this.checkBoxOtherPreferredCredits.UseVisualStyleBackColor = true;
            this.checkBoxOtherPreferredCredits.CheckedChanged += new System.EventHandler(this.checkBoxOtherPreferredCredits_CheckedChanged);
            // 
            // txtOtherPreferredCredits
            // 
            this.txtOtherPreferredCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtOtherPreferredCredits.Location = new System.Drawing.Point(606, 136);
            this.txtOtherPreferredCredits.Name = "txtOtherPreferredCredits";
            this.txtOtherPreferredCredits.Size = new System.Drawing.Size(417, 26);
            this.txtOtherPreferredCredits.TabIndex = 69;
            this.txtOtherPreferredCredits.TextChanged += new System.EventHandler(this.txtOtherPreferredCredits_TextChanged);
            // 
            // txtOtherTimeSlotAvoid
            // 
            this.txtOtherTimeSlotAvoid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtOtherTimeSlotAvoid.Location = new System.Drawing.Point(606, 455);
            this.txtOtherTimeSlotAvoid.Name = "txtOtherTimeSlotAvoid";
            this.txtOtherTimeSlotAvoid.Size = new System.Drawing.Size(417, 26);
            this.txtOtherTimeSlotAvoid.TabIndex = 71;
            this.txtOtherTimeSlotAvoid.TextChanged += new System.EventHandler(this.txtOtherTimeSlotAvoid_TextChanged);
            // 
            // checkBoxOtherTimeSlotAvoid
            // 
            this.checkBoxOtherTimeSlotAvoid.AutoSize = true;
            this.checkBoxOtherTimeSlotAvoid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBoxOtherTimeSlotAvoid.Location = new System.Drawing.Point(532, 457);
            this.checkBoxOtherTimeSlotAvoid.Name = "checkBoxOtherTimeSlotAvoid";
            this.checkBoxOtherTimeSlotAvoid.Size = new System.Drawing.Size(68, 24);
            this.checkBoxOtherTimeSlotAvoid.TabIndex = 70;
            this.checkBoxOtherTimeSlotAvoid.Text = "Other";
            this.checkBoxOtherTimeSlotAvoid.UseVisualStyleBackColor = true;
            this.checkBoxOtherTimeSlotAvoid.CheckedChanged += new System.EventHandler(this.checkBoxOtherTimeSlotAvoid_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(1031, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(246, 22);
            this.label5.TabIndex = 72;
            this.label5.Text = "Review Recommended Schedule";
            // 
            // dataGridRecommendSchedule
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridRecommendSchedule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridRecommendSchedule.ColumnHeadersHeight = 30;
            this.dataGridRecommendSchedule.Location = new System.Drawing.Point(1044, 71);
            this.dataGridRecommendSchedule.Name = "dataGridRecommendSchedule";
            this.dataGridRecommendSchedule.Size = new System.Drawing.Size(855, 915);
            this.dataGridRecommendSchedule.TabIndex = 73;
            this.dataGridRecommendSchedule.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridRecommendSchedule_CellContentClick);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnClear.Location = new System.Drawing.Point(331, 690);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(103, 29);
            this.btnClear.TabIndex = 74;
            this.btnClear.Text = "Clear ";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click_1);
            // 
            // btnGenerateSchedule
            // 
            this.btnGenerateSchedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnGenerateSchedule.Location = new System.Drawing.Point(532, 690);
            this.btnGenerateSchedule.Name = "btnGenerateSchedule";
            this.btnGenerateSchedule.Size = new System.Drawing.Size(202, 29);
            this.btnGenerateSchedule.TabIndex = 75;
            this.btnGenerateSchedule.Text = "Generate Schedule";
            this.btnGenerateSchedule.UseVisualStyleBackColor = true;
            this.btnGenerateSchedule.Click += new System.EventHandler(this.btnGenerateSchedule_Click);
            // 
            // btnSaveScheduleAsImage
            // 
            this.btnSaveScheduleAsImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnSaveScheduleAsImage.Location = new System.Drawing.Point(1382, 1006);
            this.btnSaveScheduleAsImage.Name = "btnSaveScheduleAsImage";
            this.btnSaveScheduleAsImage.Size = new System.Drawing.Size(205, 38);
            this.btnSaveScheduleAsImage.TabIndex = 76;
            this.btnSaveScheduleAsImage.Text = "Save Schedule As Image";
            this.btnSaveScheduleAsImage.UseVisualStyleBackColor = true;
            this.btnSaveScheduleAsImage.Click += new System.EventHandler(this.btnSaveScheduleAsImage_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(33, 748);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(394, 22);
            this.label6.TabIndex = 77;
            this.label6.Text = "Step 8 — Add More Courses From Other Departments";
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnDelete.Location = new System.Drawing.Point(790, 749);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(97, 29);
            this.btnDelete.TabIndex = 80;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnChoose
            // 
            this.btnChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnChoose.Location = new System.Drawing.Point(790, 797);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(97, 29);
            this.btnChoose.TabIndex = 79;
            this.btnChoose.Text = "Choose";
            this.btnChoose.UseVisualStyleBackColor = true;
            // 
            // comboBoxDept
            // 
            this.comboBoxDept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxDept.FormattingEnabled = true;
            this.comboBoxDept.Location = new System.Drawing.Point(33, 797);
            this.comboBoxDept.Name = "comboBoxDept";
            this.comboBoxDept.Size = new System.Drawing.Size(731, 28);
            this.comboBoxDept.TabIndex = 78;
            this.comboBoxDept.SelectedIndexChanged += new System.EventHandler(this.comboBoxDept_SelectedIndexChanged);
            // 
            // dataGridViewPage3
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPage3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewPage3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPage3.Location = new System.Drawing.Point(29, 848);
            this.dataGridViewPage3.Name = "dataGridViewPage3";
            this.dataGridViewPage3.Size = new System.Drawing.Size(975, 276);
            this.dataGridViewPage3.TabIndex = 81;
            this.dataGridViewPage3.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPage3_CellContentClick);
            // 
            // btnApplySelectedExtraCourses
            // 
            this.btnApplySelectedExtraCourses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnApplySelectedExtraCourses.Location = new System.Drawing.Point(1044, 1006);
            this.btnApplySelectedExtraCourses.Name = "btnApplySelectedExtraCourses";
            this.btnApplySelectedExtraCourses.Size = new System.Drawing.Size(233, 38);
            this.btnApplySelectedExtraCourses.TabIndex = 82;
            this.btnApplySelectedExtraCourses.Text = "Apply Selected Extra Courses";
            this.btnApplySelectedExtraCourses.UseVisualStyleBackColor = true;
            // 
            // btnGraduationSummary
            // 
            this.btnGraduationSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnGraduationSummary.Location = new System.Drawing.Point(1690, 1006);
            this.btnGraduationSummary.Name = "btnGraduationSummary";
            this.btnGraduationSummary.Size = new System.Drawing.Size(205, 38);
            this.btnGraduationSummary.TabIndex = 83;
            this.btnGraduationSummary.Text = "Graduation Summary";
            this.btnGraduationSummary.UseVisualStyleBackColor = true;
            this.btnGraduationSummary.Click += new System.EventHandler(this.btnGraduationSummary_Click);
            // 
            // btnBackToStep1
            // 
            this.btnBackToStep1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnBackToStep1.Location = new System.Drawing.Point(1382, 1065);
            this.btnBackToStep1.Name = "btnBackToStep1";
            this.btnBackToStep1.Size = new System.Drawing.Size(205, 38);
            this.btnBackToStep1.TabIndex = 84;
            this.btnBackToStep1.Text = "Back to Step 1";
            this.btnBackToStep1.UseVisualStyleBackColor = true;
            this.btnBackToStep1.Click += new System.EventHandler(this.btnBackToStep1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnClose.Location = new System.Drawing.Point(1690, 1065);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(205, 38);
            this.btnClose.TabIndex = 85;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form3
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1900, 1120);
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnBackToStep1);
            this.Controls.Add(this.btnGraduationSummary);
            this.Controls.Add(this.btnApplySelectedExtraCourses);
            this.Controls.Add(this.dataGridViewPage3);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.comboBoxDept);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSaveScheduleAsImage);
            this.Controls.Add(this.btnGenerateSchedule);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.dataGridRecommendSchedule);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtOtherTimeSlotAvoid);
            this.Controls.Add(this.checkBoxOtherTimeSlotAvoid);
            this.Controls.Add(this.txtOtherPreferredCredits);
            this.Controls.Add(this.checkBoxOtherPreferredCredits);
            this.Controls.Add(this.checkBox28);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxOtherTimeSlotAvoid);
            this.Controls.Add(this.checkBox21);
            this.Controls.Add(this.checkBox22);
            this.Controls.Add(this.checkBox23);
            this.Controls.Add(this.checkBox24);
            this.Controls.Add(this.checkBox25);
            this.Controls.Add(this.checkBox26);
            this.Controls.Add(this.checkBox27);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtProfessorName);
            this.Controls.Add(this.txtCourseName);
            this.Controls.Add(this.comboBoxAcademicYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox18);
            this.Controls.Add(this.checkBox19);
            this.Controls.Add(this.checkBox20);
            this.Controls.Add(this.checkBox13);
            this.Controls.Add(this.checkBox14);
            this.Controls.Add(this.checkBox15);
            this.Controls.Add(this.checkBox16);
            this.Controls.Add(this.checkBox17);
            this.Controls.Add(this.checkBox12);
            this.Controls.Add(this.checkBox10);
            this.Controls.Add(this.checkBox11);
            this.Controls.Add(this.checkBox9);
            this.Controls.Add(this.checkBox8);
            this.Controls.Add(this.checkBox7);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBoxCreditNumber);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.lblProfessorName);
            this.Controls.Add(this.lblCourseName);
            this.Controls.Add(this.comboBoxLanguage);
            this.Controls.Add(this.lblTimeSections);
            this.Controls.Add(this.lblClassDays);
            this.Controls.Add(this.lblCreditNunber);
            this.Controls.Add(this.label2);
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridRecommendSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPage3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCourseName;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.Label lblTimeSections;
        private System.Windows.Forms.Label lblClassDays;
        private System.Windows.Forms.Label lblCreditNunber;
        private System.Windows.Forms.Label lblProfessorName;
        private System.Windows.Forms.ComboBox comboBoxCreditNumber;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.CheckBox checkBox13;
        private System.Windows.Forms.CheckBox checkBox14;
        private System.Windows.Forms.CheckBox checkBox15;
        private System.Windows.Forms.CheckBox checkBox16;
        private System.Windows.Forms.CheckBox checkBox17;
        private System.Windows.Forms.CheckBox checkBox18;
        private System.Windows.Forms.CheckBox checkBox19;
        private System.Windows.Forms.CheckBox checkBox20;
        private System.Windows.Forms.ComboBox comboBoxAcademicYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCourseName;
        private System.Windows.Forms.TextBox txtProfessorName;
        private System.Windows.Forms.CheckBox checkBox21;
        private System.Windows.Forms.CheckBox checkBox22;
        private System.Windows.Forms.CheckBox checkBox23;
        private System.Windows.Forms.CheckBox checkBox24;
        private System.Windows.Forms.CheckBox checkBox25;
        private System.Windows.Forms.CheckBox checkBox26;
        private System.Windows.Forms.CheckBox checkBox27;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxOtherTimeSlotAvoid;
        private System.Windows.Forms.CheckBox checkBox28;
        private System.Windows.Forms.CheckBox checkBoxOtherPreferredCredits;
        private System.Windows.Forms.TextBox txtOtherPreferredCredits;
        private System.Windows.Forms.TextBox txtOtherTimeSlotAvoid;
        private System.Windows.Forms.CheckBox checkBoxOtherTimeSlotAvoid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridRecommendSchedule;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnGenerateSchedule;
        private System.Windows.Forms.Button btnSaveScheduleAsImage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.ComboBox comboBoxDept;
        private System.Windows.Forms.DataGridView dataGridViewPage3;
        private System.Windows.Forms.Button btnApplySelectedExtraCourses;
        private System.Windows.Forms.Button btnGraduationSummary;
        private System.Windows.Forms.Button btnBackToStep1;
        private System.Windows.Forms.Button btnClose;
    }
}