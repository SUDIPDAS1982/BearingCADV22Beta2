namespace BearingCAD22
{
    partial class frmSeal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeal));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbEndSealData = new System.Windows.Forms.TabControl();
            this.tabFront = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.txtL_Front = new System.Windows.Forms.TextBox();
            this.cmbBlade_Count_Front = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.lblMat_LiningT_Front_Metric = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.txtMat_Lining_Name_Front = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkMat_LiningExists_Front = new System.Windows.Forms.CheckBox();
            this.txtMat_Base_Name_Front = new System.Windows.Forms.TextBox();
            this.txtMat_LiningT_Front = new System.Windows.Forms.TextBox();
            this.lblMat_LiningT_Front = new System.Windows.Forms.Label();
            this.cmbMat_Lining_WCode_Front = new System.Windows.Forms.ComboBox();
            this.cmbMat_Base_WCode_Front = new System.Windows.Forms.ComboBox();
            this.lblDeg_Front = new System.Windows.Forms.Label();
            this.lblBlade_TapAng_Front = new System.Windows.Forms.Label();
            this.cmbBlade_AngTaper_Front = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBlade_T_Front = new System.Windows.Forms.TextBox();
            this.lblBladeThick_Front = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDBore_Range_Max_Front = new System.Windows.Forms.TextBox();
            this.txtDBore_Range_Min_Front = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbType_Front = new System.Windows.Forms.ComboBox();
            this.tabBack = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.txtL_Back = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label46 = new System.Windows.Forms.Label();
            this.lblMat_LiningT_Back_Metric = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMat_Lining_Name_Back = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMat_Base_Name_Back = new System.Windows.Forms.TextBox();
            this.txtMat_LiningT_Back = new System.Windows.Forms.TextBox();
            this.lblMat_LiningT_Back = new System.Windows.Forms.Label();
            this.cmbMat_Lining_WCode_Back = new System.Windows.Forms.ComboBox();
            this.cmbMat_Base_WCode_Back = new System.Windows.Forms.ComboBox();
            this.chkMat_LiningExists_Back = new System.Windows.Forms.CheckBox();
            this.cmbBlade_AngTaper_Back = new System.Windows.Forms.ComboBox();
            this.lblDeg_Back = new System.Windows.Forms.Label();
            this.lblBlade_TapAng_Back = new System.Windows.Forms.Label();
            this.cmbBlade_Count_Back = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBlade_T_Back = new System.Windows.Forms.TextBox();
            this.lblBladeThick_Back = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtDBore_Range_Max_Back = new System.Windows.Forms.TextBox();
            this.txtDBore_Range_Min_Back = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.cmbType_Back = new System.Windows.Forms.ComboBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tbEndSealData.SuspendLayout();
            this.tabFront.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabBack.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmdClose);
            this.panel1.Controls.Add(this.tbEndSealData);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 372);
            this.panel1.TabIndex = 1;
            // 
            // tbEndSealData
            // 
            this.tbEndSealData.Controls.Add(this.tabFront);
            this.tbEndSealData.Controls.Add(this.tabBack);
            this.tbEndSealData.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEndSealData.Location = new System.Drawing.Point(9, 9);
            this.tbEndSealData.Name = "tbEndSealData";
            this.tbEndSealData.SelectedIndex = 0;
            this.tbEndSealData.Size = new System.Drawing.Size(454, 309);
            this.tbEndSealData.TabIndex = 0;
            this.tbEndSealData.SelectedIndexChanged += new System.EventHandler(this.tbEndSealData_SelectedIndexChanged);
            // 
            // tabFront
            // 
            this.tabFront.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabFront.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabFront.Controls.Add(this.label14);
            this.tabFront.Controls.Add(this.txtL_Front);
            this.tabFront.Controls.Add(this.cmbBlade_Count_Front);
            this.tabFront.Controls.Add(this.groupBox1);
            this.tabFront.Controls.Add(this.lblDeg_Front);
            this.tabFront.Controls.Add(this.lblBlade_TapAng_Front);
            this.tabFront.Controls.Add(this.cmbBlade_AngTaper_Front);
            this.tabFront.Controls.Add(this.label10);
            this.tabFront.Controls.Add(this.label4);
            this.tabFront.Controls.Add(this.txtBlade_T_Front);
            this.tabFront.Controls.Add(this.lblBladeThick_Front);
            this.tabFront.Controls.Add(this.label7);
            this.tabFront.Controls.Add(this.label5);
            this.tabFront.Controls.Add(this.txtDBore_Range_Max_Front);
            this.tabFront.Controls.Add(this.txtDBore_Range_Min_Front);
            this.tabFront.Controls.Add(this.label3);
            this.tabFront.Controls.Add(this.label2);
            this.tabFront.Controls.Add(this.cmbType_Front);
            this.tabFront.Location = new System.Drawing.Point(4, 22);
            this.tabFront.Name = "tabFront";
            this.tabFront.Padding = new System.Windows.Forms.Padding(3);
            this.tabFront.Size = new System.Drawing.Size(446, 283);
            this.tabFront.TabIndex = 0;
            this.tabFront.Text = "Front";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(289, 67);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 13);
            this.label14.TabIndex = 490;
            this.label14.Text = "Length";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtL_Front
            // 
            this.txtL_Front.BackColor = System.Drawing.Color.White;
            this.txtL_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtL_Front.ForeColor = System.Drawing.Color.Black;
            this.txtL_Front.Location = new System.Drawing.Point(340, 63);
            this.txtL_Front.Name = "txtL_Front";
            this.txtL_Front.Size = new System.Drawing.Size(59, 21);
            this.txtL_Front.TabIndex = 489;
            this.txtL_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtL_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // cmbBlade_Count_Front
            // 
            this.cmbBlade_Count_Front.BackColor = System.Drawing.Color.LightSteelBlue;
            this.cmbBlade_Count_Front.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlade_Count_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlade_Count_Front.FormattingEnabled = true;
            this.cmbBlade_Count_Front.Location = new System.Drawing.Point(79, 122);
            this.cmbBlade_Count_Front.Name = "cmbBlade_Count_Front";
            this.cmbBlade_Count_Front.Size = new System.Drawing.Size(39, 21);
            this.cmbBlade_Count_Front.TabIndex = 455;
            this.cmbBlade_Count_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.lblMat_LiningT_Front_Metric);
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Controls.Add(this.txtMat_Lining_Name_Front);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.chkMat_LiningExists_Front);
            this.groupBox1.Controls.Add(this.txtMat_Base_Name_Front);
            this.groupBox1.Controls.Add(this.txtMat_LiningT_Front);
            this.groupBox1.Controls.Add(this.lblMat_LiningT_Front);
            this.groupBox1.Controls.Add(this.cmbMat_Lining_WCode_Front);
            this.groupBox1.Controls.Add(this.cmbMat_Base_WCode_Front);
            this.groupBox1.Location = new System.Drawing.Point(9, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 120);
            this.groupBox1.TabIndex = 488;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Material:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label15.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(217, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 13);
            this.label15.TabIndex = 492;
            this.label15.Text = "Name";
            this.label15.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblMat_LiningT_Front_Metric
            // 
            this.lblMat_LiningT_Front_Metric.AutoSize = true;
            this.lblMat_LiningT_Front_Metric.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblMat_LiningT_Front_Metric.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMat_LiningT_Front_Metric.Location = new System.Drawing.Point(371, 96);
            this.lblMat_LiningT_Front_Metric.Name = "lblMat_LiningT_Front_Metric";
            this.lblMat_LiningT_Front_Metric.Size = new System.Drawing.Size(0, 13);
            this.lblMat_LiningT_Front_Metric.TabIndex = 489;
            this.lblMat_LiningT_Front_Metric.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label29.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(78, 20);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(73, 13);
            this.label29.TabIndex = 482;
            this.label29.Text = "WBM Code ";
            this.label29.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtMat_Lining_Name_Front
            // 
            this.txtMat_Lining_Name_Front.BackColor = System.Drawing.SystemColors.Control;
            this.txtMat_Lining_Name_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMat_Lining_Name_Front.ForeColor = System.Drawing.Color.Black;
            this.txtMat_Lining_Name_Front.Location = new System.Drawing.Point(173, 71);
            this.txtMat_Lining_Name_Front.Name = "txtMat_Lining_Name_Front";
            this.txtMat_Lining_Name_Front.ReadOnly = true;
            this.txtMat_Lining_Name_Front.Size = new System.Drawing.Size(128, 21);
            this.txtMat_Lining_Name_Front.TabIndex = 487;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(33, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 468;
            this.label8.Text = "Base";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkMat_LiningExists_Front
            // 
            this.chkMat_LiningExists_Front.AutoSize = true;
            this.chkMat_LiningExists_Front.Checked = true;
            this.chkMat_LiningExists_Front.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMat_LiningExists_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMat_LiningExists_Front.Location = new System.Drawing.Point(9, 73);
            this.chkMat_LiningExists_Front.Name = "chkMat_LiningExists_Front";
            this.chkMat_LiningExists_Front.Size = new System.Drawing.Size(59, 17);
            this.chkMat_LiningExists_Front.TabIndex = 468;
            this.chkMat_LiningExists_Front.Text = "Lining";
            this.chkMat_LiningExists_Front.UseVisualStyleBackColor = true;
            this.chkMat_LiningExists_Front.CheckedChanged += new System.EventHandler(this.chkMat_LiningExists_Front_CheckedChanged);
            // 
            // txtMat_Base_Name_Front
            // 
            this.txtMat_Base_Name_Front.BackColor = System.Drawing.SystemColors.Control;
            this.txtMat_Base_Name_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMat_Base_Name_Front.ForeColor = System.Drawing.Color.Black;
            this.txtMat_Base_Name_Front.Location = new System.Drawing.Point(173, 38);
            this.txtMat_Base_Name_Front.Name = "txtMat_Base_Name_Front";
            this.txtMat_Base_Name_Front.ReadOnly = true;
            this.txtMat_Base_Name_Front.Size = new System.Drawing.Size(128, 21);
            this.txtMat_Base_Name_Front.TabIndex = 486;
            // 
            // txtMat_LiningT_Front
            // 
            this.txtMat_LiningT_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMat_LiningT_Front.Location = new System.Drawing.Point(355, 71);
            this.txtMat_LiningT_Front.Name = "txtMat_LiningT_Front";
            this.txtMat_LiningT_Front.Size = new System.Drawing.Size(67, 21);
            this.txtMat_LiningT_Front.TabIndex = 460;
            this.txtMat_LiningT_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMat_LiningT_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtMat_LiningT_Front.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
            // 
            // lblMat_LiningT_Front
            // 
            this.lblMat_LiningT_Front.AutoSize = true;
            this.lblMat_LiningT_Front.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblMat_LiningT_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMat_LiningT_Front.Location = new System.Drawing.Point(312, 75);
            this.lblMat_LiningT_Front.Name = "lblMat_LiningT_Front";
            this.lblMat_LiningT_Front.Size = new System.Drawing.Size(37, 13);
            this.lblMat_LiningT_Front.TabIndex = 470;
            this.lblMat_LiningT_Front.Text = "Thick";
            this.lblMat_LiningT_Front.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMat_Lining_WCode_Front
            // 
            this.cmbMat_Lining_WCode_Front.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMat_Lining_WCode_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMat_Lining_WCode_Front.FormattingEnabled = true;
            this.cmbMat_Lining_WCode_Front.Location = new System.Drawing.Point(73, 71);
            this.cmbMat_Lining_WCode_Front.Name = "cmbMat_Lining_WCode_Front";
            this.cmbMat_Lining_WCode_Front.Size = new System.Drawing.Size(82, 21);
            this.cmbMat_Lining_WCode_Front.TabIndex = 480;
            this.cmbMat_Lining_WCode_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmbMat_Base_WCode_Front
            // 
            this.cmbMat_Base_WCode_Front.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMat_Base_WCode_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMat_Base_WCode_Front.FormattingEnabled = true;
            this.cmbMat_Base_WCode_Front.Location = new System.Drawing.Point(73, 38);
            this.cmbMat_Base_WCode_Front.Name = "cmbMat_Base_WCode_Front";
            this.cmbMat_Base_WCode_Front.Size = new System.Drawing.Size(82, 21);
            this.cmbMat_Base_WCode_Front.TabIndex = 479;
            this.cmbMat_Base_WCode_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // lblDeg_Front
            // 
            this.lblDeg_Front.AutoSize = true;
            this.lblDeg_Front.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblDeg_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeg_Front.Location = new System.Drawing.Point(327, 125);
            this.lblDeg_Front.Name = "lblDeg_Front";
            this.lblDeg_Front.Size = new System.Drawing.Size(32, 13);
            this.lblDeg_Front.TabIndex = 475;
            this.lblDeg_Front.Text = "deg.";
            this.lblDeg_Front.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBlade_TapAng_Front
            // 
            this.lblBlade_TapAng_Front.AutoSize = true;
            this.lblBlade_TapAng_Front.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblBlade_TapAng_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlade_TapAng_Front.Location = new System.Drawing.Point(270, 103);
            this.lblBlade_TapAng_Front.Name = "lblBlade_TapAng_Front";
            this.lblBlade_TapAng_Front.Size = new System.Drawing.Size(69, 13);
            this.lblBlade_TapAng_Front.TabIndex = 474;
            this.lblBlade_TapAng_Front.Text = "Taper Ang.";
            this.lblBlade_TapAng_Front.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cmbBlade_AngTaper_Front
            // 
            this.cmbBlade_AngTaper_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlade_AngTaper_Front.FormattingEnabled = true;
            this.cmbBlade_AngTaper_Front.Location = new System.Drawing.Point(285, 121);
            this.cmbBlade_AngTaper_Front.Name = "cmbBlade_AngTaper_Front";
            this.cmbBlade_AngTaper_Front.Size = new System.Drawing.Size(38, 21);
            this.cmbBlade_AngTaper_Front.TabIndex = 457;
            this.cmbBlade_AngTaper_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            this.cmbBlade_AngTaper_Front.TextChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(95, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 13);
            this.label10.TabIndex = 473;
            this.label10.Text = "Min";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(193, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 472;
            this.label4.Text = "Max";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtBlade_T_Front
            // 
            this.txtBlade_T_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlade_T_Front.ForeColor = System.Drawing.Color.Black;
            this.txtBlade_T_Front.Location = new System.Drawing.Point(179, 121);
            this.txtBlade_T_Front.Name = "txtBlade_T_Front";
            this.txtBlade_T_Front.ReadOnly = true;
            this.txtBlade_T_Front.Size = new System.Drawing.Size(59, 21);
            this.txtBlade_T_Front.TabIndex = 456;
            this.txtBlade_T_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBlade_T_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtBlade_T_Front.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
            // 
            // lblBladeThick_Front
            // 
            this.lblBladeThick_Front.AutoSize = true;
            this.lblBladeThick_Front.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblBladeThick_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBladeThick_Front.Location = new System.Drawing.Point(136, 125);
            this.lblBladeThick_Front.Name = "lblBladeThick_Front";
            this.lblBladeThick_Front.Size = new System.Drawing.Size(37, 13);
            this.lblBladeThick_Front.TabIndex = 465;
            this.lblBladeThick_Front.Text = "Thick";
            this.lblBladeThick_Front.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(46, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 464;
            this.label7.Text = "Qty";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 17);
            this.label5.TabIndex = 463;
            this.label5.Text = "Blade:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDBore_Range_Max_Front
            // 
            this.txtDBore_Range_Max_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBore_Range_Max_Front.Location = new System.Drawing.Point(179, 63);
            this.txtDBore_Range_Max_Front.Name = "txtDBore_Range_Max_Front";
            this.txtDBore_Range_Max_Front.ReadOnly = true;
            this.txtDBore_Range_Max_Front.Size = new System.Drawing.Size(59, 21);
            this.txtDBore_Range_Max_Front.TabIndex = 454;
            this.txtDBore_Range_Max_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDBore_Range_Max_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtDBore_Range_Max_Front.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // txtDBore_Range_Min_Front
            // 
            this.txtDBore_Range_Min_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBore_Range_Min_Front.Location = new System.Drawing.Point(79, 63);
            this.txtDBore_Range_Min_Front.Name = "txtDBore_Range_Min_Front";
            this.txtDBore_Range_Min_Front.ReadOnly = true;
            this.txtDBore_Range_Min_Front.Size = new System.Drawing.Size(59, 21);
            this.txtDBore_Range_Min_Front.TabIndex = 453;
            this.txtDBore_Range_Min_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDBore_Range_Min_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtDBore_Range_Min_Front.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 462;
            this.label3.Text = "Bore Dia";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 461;
            this.label2.Text = "Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbType_Front
            // 
            this.cmbType_Front.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType_Front.Enabled = false;
            this.cmbType_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType_Front.FormattingEnabled = true;
            this.cmbType_Front.Location = new System.Drawing.Point(79, 14);
            this.cmbType_Front.Name = "cmbType_Front";
            this.cmbType_Front.Size = new System.Drawing.Size(81, 21);
            this.cmbType_Front.TabIndex = 452;
            this.cmbType_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // tabBack
            // 
            this.tabBack.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabBack.Controls.Add(this.label16);
            this.tabBack.Controls.Add(this.txtL_Back);
            this.tabBack.Controls.Add(this.groupBox2);
            this.tabBack.Controls.Add(this.cmbBlade_AngTaper_Back);
            this.tabBack.Controls.Add(this.lblDeg_Back);
            this.tabBack.Controls.Add(this.lblBlade_TapAng_Back);
            this.tabBack.Controls.Add(this.cmbBlade_Count_Back);
            this.tabBack.Controls.Add(this.label12);
            this.tabBack.Controls.Add(this.label13);
            this.tabBack.Controls.Add(this.txtBlade_T_Back);
            this.tabBack.Controls.Add(this.lblBladeThick_Back);
            this.tabBack.Controls.Add(this.label19);
            this.tabBack.Controls.Add(this.label20);
            this.tabBack.Controls.Add(this.txtDBore_Range_Max_Back);
            this.tabBack.Controls.Add(this.txtDBore_Range_Min_Back);
            this.tabBack.Controls.Add(this.label21);
            this.tabBack.Controls.Add(this.label22);
            this.tabBack.Controls.Add(this.cmbType_Back);
            this.tabBack.Location = new System.Drawing.Point(4, 22);
            this.tabBack.Name = "tabBack";
            this.tabBack.Padding = new System.Windows.Forms.Padding(3);
            this.tabBack.Size = new System.Drawing.Size(446, 283);
            this.tabBack.TabIndex = 1;
            this.tabBack.Text = "Back";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label16.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(289, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(45, 13);
            this.label16.TabIndex = 491;
            this.label16.Text = "Length";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtL_Back
            // 
            this.txtL_Back.BackColor = System.Drawing.Color.White;
            this.txtL_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtL_Back.ForeColor = System.Drawing.Color.Black;
            this.txtL_Back.Location = new System.Drawing.Point(340, 63);
            this.txtL_Back.Name = "txtL_Back";
            this.txtL_Back.Size = new System.Drawing.Size(59, 21);
            this.txtL_Back.TabIndex = 490;
            this.txtL_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtL_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label46);
            this.groupBox2.Controls.Add(this.lblMat_LiningT_Back_Metric);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtMat_Lining_Name_Back);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtMat_Base_Name_Back);
            this.groupBox2.Controls.Add(this.txtMat_LiningT_Back);
            this.groupBox2.Controls.Add(this.lblMat_LiningT_Back);
            this.groupBox2.Controls.Add(this.cmbMat_Lining_WCode_Back);
            this.groupBox2.Controls.Add(this.cmbMat_Base_WCode_Back);
            this.groupBox2.Controls.Add(this.chkMat_LiningExists_Back);
            this.groupBox2.Location = new System.Drawing.Point(9, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 120);
            this.groupBox2.TabIndex = 489;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Material:";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label46.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(217, 20);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(40, 13);
            this.label46.TabIndex = 491;
            this.label46.Text = "Name";
            this.label46.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblMat_LiningT_Back_Metric
            // 
            this.lblMat_LiningT_Back_Metric.AutoSize = true;
            this.lblMat_LiningT_Back_Metric.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblMat_LiningT_Back_Metric.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMat_LiningT_Back_Metric.Location = new System.Drawing.Point(371, 96);
            this.lblMat_LiningT_Back_Metric.Name = "lblMat_LiningT_Back_Metric";
            this.lblMat_LiningT_Back_Metric.Size = new System.Drawing.Size(0, 13);
            this.lblMat_LiningT_Back_Metric.TabIndex = 490;
            this.lblMat_LiningT_Back_Metric.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(78, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 482;
            this.label6.Text = "WBM Code ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtMat_Lining_Name_Back
            // 
            this.txtMat_Lining_Name_Back.BackColor = System.Drawing.SystemColors.Control;
            this.txtMat_Lining_Name_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMat_Lining_Name_Back.ForeColor = System.Drawing.Color.Black;
            this.txtMat_Lining_Name_Back.Location = new System.Drawing.Point(173, 71);
            this.txtMat_Lining_Name_Back.Name = "txtMat_Lining_Name_Back";
            this.txtMat_Lining_Name_Back.ReadOnly = true;
            this.txtMat_Lining_Name_Back.Size = new System.Drawing.Size(128, 21);
            this.txtMat_Lining_Name_Back.TabIndex = 487;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(33, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 468;
            this.label9.Text = "Base";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMat_Base_Name_Back
            // 
            this.txtMat_Base_Name_Back.BackColor = System.Drawing.SystemColors.Control;
            this.txtMat_Base_Name_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMat_Base_Name_Back.ForeColor = System.Drawing.Color.Black;
            this.txtMat_Base_Name_Back.Location = new System.Drawing.Point(173, 38);
            this.txtMat_Base_Name_Back.Name = "txtMat_Base_Name_Back";
            this.txtMat_Base_Name_Back.ReadOnly = true;
            this.txtMat_Base_Name_Back.Size = new System.Drawing.Size(128, 21);
            this.txtMat_Base_Name_Back.TabIndex = 486;
            // 
            // txtMat_LiningT_Back
            // 
            this.txtMat_LiningT_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMat_LiningT_Back.Location = new System.Drawing.Point(355, 71);
            this.txtMat_LiningT_Back.Name = "txtMat_LiningT_Back";
            this.txtMat_LiningT_Back.Size = new System.Drawing.Size(67, 21);
            this.txtMat_LiningT_Back.TabIndex = 460;
            this.txtMat_LiningT_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMat_LiningT_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // lblMat_LiningT_Back
            // 
            this.lblMat_LiningT_Back.AutoSize = true;
            this.lblMat_LiningT_Back.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblMat_LiningT_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMat_LiningT_Back.Location = new System.Drawing.Point(312, 75);
            this.lblMat_LiningT_Back.Name = "lblMat_LiningT_Back";
            this.lblMat_LiningT_Back.Size = new System.Drawing.Size(37, 13);
            this.lblMat_LiningT_Back.TabIndex = 470;
            this.lblMat_LiningT_Back.Text = "Thick";
            this.lblMat_LiningT_Back.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMat_Lining_WCode_Back
            // 
            this.cmbMat_Lining_WCode_Back.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMat_Lining_WCode_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMat_Lining_WCode_Back.FormattingEnabled = true;
            this.cmbMat_Lining_WCode_Back.Location = new System.Drawing.Point(73, 71);
            this.cmbMat_Lining_WCode_Back.Name = "cmbMat_Lining_WCode_Back";
            this.cmbMat_Lining_WCode_Back.Size = new System.Drawing.Size(82, 21);
            this.cmbMat_Lining_WCode_Back.TabIndex = 480;
            this.cmbMat_Lining_WCode_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmbMat_Base_WCode_Back
            // 
            this.cmbMat_Base_WCode_Back.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMat_Base_WCode_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMat_Base_WCode_Back.FormattingEnabled = true;
            this.cmbMat_Base_WCode_Back.Location = new System.Drawing.Point(73, 38);
            this.cmbMat_Base_WCode_Back.Name = "cmbMat_Base_WCode_Back";
            this.cmbMat_Base_WCode_Back.Size = new System.Drawing.Size(82, 21);
            this.cmbMat_Base_WCode_Back.TabIndex = 479;
            this.cmbMat_Base_WCode_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // chkMat_LiningExists_Back
            // 
            this.chkMat_LiningExists_Back.AutoSize = true;
            this.chkMat_LiningExists_Back.Checked = true;
            this.chkMat_LiningExists_Back.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMat_LiningExists_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMat_LiningExists_Back.Location = new System.Drawing.Point(9, 73);
            this.chkMat_LiningExists_Back.Name = "chkMat_LiningExists_Back";
            this.chkMat_LiningExists_Back.Size = new System.Drawing.Size(59, 17);
            this.chkMat_LiningExists_Back.TabIndex = 468;
            this.chkMat_LiningExists_Back.Text = "Lining";
            this.chkMat_LiningExists_Back.UseVisualStyleBackColor = true;
            this.chkMat_LiningExists_Back.CheckedChanged += new System.EventHandler(this.chkMat_LiningExists_Back_CheckedChanged);
            // 
            // cmbBlade_AngTaper_Back
            // 
            this.cmbBlade_AngTaper_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlade_AngTaper_Back.FormattingEnabled = true;
            this.cmbBlade_AngTaper_Back.Location = new System.Drawing.Point(285, 121);
            this.cmbBlade_AngTaper_Back.Name = "cmbBlade_AngTaper_Back";
            this.cmbBlade_AngTaper_Back.Size = new System.Drawing.Size(38, 21);
            this.cmbBlade_AngTaper_Back.TabIndex = 476;
            this.cmbBlade_AngTaper_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            this.cmbBlade_AngTaper_Back.TextChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // lblDeg_Back
            // 
            this.lblDeg_Back.AutoSize = true;
            this.lblDeg_Back.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblDeg_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeg_Back.Location = new System.Drawing.Point(327, 125);
            this.lblDeg_Back.Name = "lblDeg_Back";
            this.lblDeg_Back.Size = new System.Drawing.Size(32, 13);
            this.lblDeg_Back.TabIndex = 475;
            this.lblDeg_Back.Text = "deg.";
            this.lblDeg_Back.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBlade_TapAng_Back
            // 
            this.lblBlade_TapAng_Back.AutoSize = true;
            this.lblBlade_TapAng_Back.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblBlade_TapAng_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlade_TapAng_Back.Location = new System.Drawing.Point(270, 103);
            this.lblBlade_TapAng_Back.Name = "lblBlade_TapAng_Back";
            this.lblBlade_TapAng_Back.Size = new System.Drawing.Size(69, 13);
            this.lblBlade_TapAng_Back.TabIndex = 474;
            this.lblBlade_TapAng_Back.Text = "Taper Ang.";
            this.lblBlade_TapAng_Back.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cmbBlade_Count_Back
            // 
            this.cmbBlade_Count_Back.BackColor = System.Drawing.Color.LightSteelBlue;
            this.cmbBlade_Count_Back.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlade_Count_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlade_Count_Back.FormattingEnabled = true;
            this.cmbBlade_Count_Back.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmbBlade_Count_Back.Location = new System.Drawing.Point(79, 122);
            this.cmbBlade_Count_Back.Name = "cmbBlade_Count_Back";
            this.cmbBlade_Count_Back.Size = new System.Drawing.Size(39, 21);
            this.cmbBlade_Count_Back.TabIndex = 455;
            this.cmbBlade_Count_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(95, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 473;
            this.label12.Text = "Min";
            this.label12.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(193, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 13);
            this.label13.TabIndex = 472;
            this.label13.Text = "Max";
            this.label13.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtBlade_T_Back
            // 
            this.txtBlade_T_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlade_T_Back.ForeColor = System.Drawing.Color.Black;
            this.txtBlade_T_Back.Location = new System.Drawing.Point(179, 121);
            this.txtBlade_T_Back.Name = "txtBlade_T_Back";
            this.txtBlade_T_Back.ReadOnly = true;
            this.txtBlade_T_Back.Size = new System.Drawing.Size(59, 21);
            this.txtBlade_T_Back.TabIndex = 456;
            this.txtBlade_T_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBlade_T_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtBlade_T_Back.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
            // 
            // lblBladeThick_Back
            // 
            this.lblBladeThick_Back.AutoSize = true;
            this.lblBladeThick_Back.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblBladeThick_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBladeThick_Back.Location = new System.Drawing.Point(136, 125);
            this.lblBladeThick_Back.Name = "lblBladeThick_Back";
            this.lblBladeThick_Back.Size = new System.Drawing.Size(37, 13);
            this.lblBladeThick_Back.TabIndex = 465;
            this.lblBladeThick_Back.Text = "Thick";
            this.lblBladeThick_Back.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label19.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(46, 126);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(27, 13);
            this.label19.TabIndex = 464;
            this.label19.Text = "Qty";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label20.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(18, 95);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 17);
            this.label20.TabIndex = 463;
            this.label20.Text = "Blade:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDBore_Range_Max_Back
            // 
            this.txtDBore_Range_Max_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBore_Range_Max_Back.Location = new System.Drawing.Point(179, 63);
            this.txtDBore_Range_Max_Back.Name = "txtDBore_Range_Max_Back";
            this.txtDBore_Range_Max_Back.ReadOnly = true;
            this.txtDBore_Range_Max_Back.Size = new System.Drawing.Size(59, 21);
            this.txtDBore_Range_Max_Back.TabIndex = 454;
            this.txtDBore_Range_Max_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDBore_Range_Max_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtDBore_Range_Max_Back.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // txtDBore_Range_Min_Back
            // 
            this.txtDBore_Range_Min_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBore_Range_Min_Back.Location = new System.Drawing.Point(79, 63);
            this.txtDBore_Range_Min_Back.Name = "txtDBore_Range_Min_Back";
            this.txtDBore_Range_Min_Back.ReadOnly = true;
            this.txtDBore_Range_Min_Back.Size = new System.Drawing.Size(59, 21);
            this.txtDBore_Range_Min_Back.TabIndex = 453;
            this.txtDBore_Range_Min_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDBore_Range_Min_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtDBore_Range_Min_Back.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label21.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(16, 67);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(57, 13);
            this.label21.TabIndex = 462;
            this.label21.Text = "Bore Dia";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label22.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(39, 18);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(34, 13);
            this.label22.TabIndex = 461;
            this.label22.Text = "Type";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbType_Back
            // 
            this.cmbType_Back.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType_Back.Enabled = false;
            this.cmbType_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType_Back.FormattingEnabled = true;
            this.cmbType_Back.Items.AddRange(new object[] {
            "Fixed",
            "Floating"});
            this.cmbType_Back.Location = new System.Drawing.Point(79, 14);
            this.cmbType_Back.Name = "cmbType_Back";
            this.cmbType_Back.Size = new System.Drawing.Size(81, 21);
            this.cmbType_Back.TabIndex = 452;
            this.cmbType_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(287, 332);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 466;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(383, 332);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 467;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(474, 374);
            this.label1.TabIndex = 0;
            // 
            // cmdClose
            // 
            this.cmdClose.BackColor = System.Drawing.Color.Silver;
            this.cmdClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdClose.Image = ((System.Drawing.Image)(resources.GetObject("cmdClose.Image")));
            this.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdClose.Location = new System.Drawing.Point(11, 332);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(80, 32);
            this.cmdClose.TabIndex = 746;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = false;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // frmSeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(478, 377);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmSeal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "End Seal Data";
            this.Load += new System.EventHandler(this.frmSeal_Load);
            this.panel1.ResumeLayout(false);
            this.tbEndSealData.ResumeLayout(false);
            this.tabFront.ResumeLayout(false);
            this.tabFront.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabBack.ResumeLayout(false);
            this.tabBack.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tbEndSealData;
        private System.Windows.Forms.TabPage tabFront;
        internal System.Windows.Forms.Label lblDeg_Front;
        internal System.Windows.Forms.Label lblBlade_TapAng_Front;
        private System.Windows.Forms.ComboBox cmbBlade_AngTaper_Front;
        private System.Windows.Forms.ComboBox cmbBlade_Count_Front;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label lblMat_LiningT_Front;
        private System.Windows.Forms.TextBox txtMat_LiningT_Front;
        internal System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.TextBox txtBlade_T_Front;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDBore_Range_Max_Front;
        private System.Windows.Forms.TextBox txtDBore_Range_Min_Front;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbType_Front;
        private System.Windows.Forms.TabPage tabBack;
        internal System.Windows.Forms.Label lblDeg_Back;
        internal System.Windows.Forms.Label lblBlade_TapAng_Back;
        private System.Windows.Forms.ComboBox cmbBlade_Count_Back;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBlade_T_Back;
        internal System.Windows.Forms.Label lblBladeThick_Back;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtDBore_Range_Max_Back;
        private System.Windows.Forms.TextBox txtDBore_Range_Min_Back;
        internal System.Windows.Forms.Label label21;
        internal System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox cmbType_Back;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label lblBladeThick_Front;
        private System.Windows.Forms.ComboBox cmbBlade_AngTaper_Back;
        private System.Windows.Forms.ComboBox cmbMat_Lining_WCode_Front;
        private System.Windows.Forms.ComboBox cmbMat_Base_WCode_Front;
        internal System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtMat_Base_Name_Front;
        private System.Windows.Forms.CheckBox chkMat_LiningExists_Front;
        private System.Windows.Forms.TextBox txtMat_Lining_Name_Front;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMat_Lining_Name_Back;
        private System.Windows.Forms.CheckBox chkMat_LiningExists_Back;
        internal System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMat_Base_Name_Back;
        private System.Windows.Forms.TextBox txtMat_LiningT_Back;
        internal System.Windows.Forms.Label lblMat_LiningT_Back;
        private System.Windows.Forms.ComboBox cmbMat_Lining_WCode_Back;
        private System.Windows.Forms.ComboBox cmbMat_Base_WCode_Back;
        internal System.Windows.Forms.Label lblMat_LiningT_Front_Metric;
        internal System.Windows.Forms.Label lblMat_LiningT_Back_Metric;
        internal System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtL_Front;
        internal System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtL_Back;
        internal System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Label label46;
        private System.Windows.Forms.Button cmdClose;

    }
}