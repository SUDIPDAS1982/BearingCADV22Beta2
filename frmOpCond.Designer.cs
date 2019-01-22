namespace BearingCAD22
{
    partial class frmOpCond
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpCond));
            this.lblBorder = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbOpCond = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.grpStaticLoad = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblStaticLoad_Thrust_Back = new System.Windows.Forms.Label();
            this.lblStaticLoad_Thrust_Front = new System.Windows.Forms.Label();
            this.txtThrust_Load_Front = new System.Windows.Forms.TextBox();
            this.txtThrust_Load_Back = new System.Windows.Forms.TextBox();
            this.lblStaticLoad_Thrust = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRadial_Load = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtRadial_LoadAng = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpeed_Design = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.grpRot_Directionality = new System.Windows.Forms.GroupBox();
            this.optRot_Directionality_Bi = new System.Windows.Forms.RadioButton();
            this.optRot_Directionality_Uni = new System.Windows.Forms.RadioButton();
            this.tabOilSupply = new System.Windows.Forms.TabPage();
            this.lblFlowReqd_Unit = new System.Windows.Forms.Label();
            this.txtFlowReqd = new System.Windows.Forms.TextBox();
            this.label95 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtOilSupply_Temp = new System.Windows.Forms.TextBox();
            this.lblPress = new System.Windows.Forms.Label();
            this.lblPressUnit = new System.Windows.Forms.Label();
            this.lblTemp = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTempUnit = new System.Windows.Forms.Label();
            this.lblTempDegF = new System.Windows.Forms.Label();
            this.txtOilSupply_Press = new System.Windows.Forms.TextBox();
            this.lblTempDegC = new System.Windows.Forms.Label();
            this.txtLube_Type = new System.Windows.Forms.TextBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmdClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tbOpCond.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.grpStaticLoad.SuspendLayout();
            this.grpRot_Directionality.SuspendLayout();
            this.tabOilSupply.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBorder.BackColor = System.Drawing.Color.Black;
            this.lblBorder.Location = new System.Drawing.Point(3, 4);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(418, 328);
            this.lblBorder.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmdClose);
            this.panel1.Controls.Add(this.tbOpCond);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Location = new System.Drawing.Point(4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(416, 326);
            this.panel1.TabIndex = 8;
            // 
            // tbOpCond
            // 
            this.tbOpCond.Controls.Add(this.tabGeneral);
            this.tbOpCond.Controls.Add(this.tabOilSupply);
            this.tbOpCond.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOpCond.Location = new System.Drawing.Point(7, 11);
            this.tbOpCond.Name = "tbOpCond";
            this.tbOpCond.SelectedIndex = 0;
            this.tbOpCond.Size = new System.Drawing.Size(397, 266);
            this.tbOpCond.TabIndex = 9;
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabGeneral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabGeneral.Controls.Add(this.grpStaticLoad);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Controls.Add(this.txtSpeed_Design);
            this.tabGeneral.Controls.Add(this.label8);
            this.tabGeneral.Controls.Add(this.grpRot_Directionality);
            this.tabGeneral.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(389, 240);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // grpStaticLoad
            // 
            this.grpStaticLoad.Controls.Add(this.label6);
            this.grpStaticLoad.Controls.Add(this.lblStaticLoad_Thrust_Back);
            this.grpStaticLoad.Controls.Add(this.lblStaticLoad_Thrust_Front);
            this.grpStaticLoad.Controls.Add(this.txtThrust_Load_Front);
            this.grpStaticLoad.Controls.Add(this.txtThrust_Load_Back);
            this.grpStaticLoad.Controls.Add(this.lblStaticLoad_Thrust);
            this.grpStaticLoad.Controls.Add(this.label2);
            this.grpStaticLoad.Controls.Add(this.txtRadial_Load);
            this.grpStaticLoad.Controls.Add(this.label18);
            this.grpStaticLoad.Controls.Add(this.label17);
            this.grpStaticLoad.Controls.Add(this.txtRadial_LoadAng);
            this.grpStaticLoad.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpStaticLoad.Location = new System.Drawing.Point(12, 62);
            this.grpStaticLoad.Name = "grpStaticLoad";
            this.grpStaticLoad.Size = new System.Drawing.Size(360, 163);
            this.grpStaticLoad.TabIndex = 280;
            this.grpStaticLoad.TabStop = false;
            this.grpStaticLoad.Text = "Static Load:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(184, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 13);
            this.label6.TabIndex = 601;
            this.label6.Text = "(from Casing S/L)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStaticLoad_Thrust_Back
            // 
            this.lblStaticLoad_Thrust_Back.AutoSize = true;
            this.lblStaticLoad_Thrust_Back.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblStaticLoad_Thrust_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaticLoad_Thrust_Back.Location = new System.Drawing.Point(153, 75);
            this.lblStaticLoad_Thrust_Back.Name = "lblStaticLoad_Thrust_Back";
            this.lblStaticLoad_Thrust_Back.Size = new System.Drawing.Size(35, 13);
            this.lblStaticLoad_Thrust_Back.TabIndex = 336;
            this.lblStaticLoad_Thrust_Back.Text = "Back";
            this.lblStaticLoad_Thrust_Back.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStaticLoad_Thrust_Back.Visible = false;
            // 
            // lblStaticLoad_Thrust_Front
            // 
            this.lblStaticLoad_Thrust_Front.AutoSize = true;
            this.lblStaticLoad_Thrust_Front.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblStaticLoad_Thrust_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaticLoad_Thrust_Front.Location = new System.Drawing.Point(67, 75);
            this.lblStaticLoad_Thrust_Front.Name = "lblStaticLoad_Thrust_Front";
            this.lblStaticLoad_Thrust_Front.Size = new System.Drawing.Size(36, 13);
            this.lblStaticLoad_Thrust_Front.TabIndex = 335;
            this.lblStaticLoad_Thrust_Front.Text = "Front";
            this.lblStaticLoad_Thrust_Front.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStaticLoad_Thrust_Front.Visible = false;
            // 
            // txtThrust_Load_Front
            // 
            this.txtThrust_Load_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThrust_Load_Front.ForeColor = System.Drawing.Color.Black;
            this.txtThrust_Load_Front.Location = new System.Drawing.Point(60, 92);
            this.txtThrust_Load_Front.Name = "txtThrust_Load_Front";
            this.txtThrust_Load_Front.Size = new System.Drawing.Size(50, 21);
            this.txtThrust_Load_Front.TabIndex = 334;
            this.txtThrust_Load_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtThrust_Load_Front.Visible = false;
            // 
            // txtThrust_Load_Back
            // 
            this.txtThrust_Load_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThrust_Load_Back.ForeColor = System.Drawing.Color.Black;
            this.txtThrust_Load_Back.Location = new System.Drawing.Point(145, 92);
            this.txtThrust_Load_Back.Name = "txtThrust_Load_Back";
            this.txtThrust_Load_Back.Size = new System.Drawing.Size(50, 21);
            this.txtThrust_Load_Back.TabIndex = 332;
            this.txtThrust_Load_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtThrust_Load_Back.Visible = false;
            // 
            // lblStaticLoad_Thrust
            // 
            this.lblStaticLoad_Thrust.AutoSize = true;
            this.lblStaticLoad_Thrust.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblStaticLoad_Thrust.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaticLoad_Thrust.Location = new System.Drawing.Point(7, 96);
            this.lblStaticLoad_Thrust.Name = "lblStaticLoad_Thrust";
            this.lblStaticLoad_Thrust.Size = new System.Drawing.Size(43, 13);
            this.lblStaticLoad_Thrust.TabIndex = 333;
            this.lblStaticLoad_Thrust.Text = "Thrust";
            this.lblStaticLoad_Thrust.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStaticLoad_Thrust.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 327;
            this.label2.Text = "Radial";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRadial_Load
            // 
            this.txtRadial_Load.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRadial_Load.ForeColor = System.Drawing.Color.Black;
            this.txtRadial_Load.Location = new System.Drawing.Point(60, 26);
            this.txtRadial_Load.Name = "txtRadial_Load";
            this.txtRadial_Load.ReadOnly = true;
            this.txtRadial_Load.Size = new System.Drawing.Size(50, 21);
            this.txtRadial_Load.TabIndex = 325;
            this.txtRadial_Load.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label18.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(322, 30);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(32, 13);
            this.label18.TabIndex = 329;
            this.label18.Text = "deg.";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label17.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(223, 30);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(39, 13);
            this.label17.TabIndex = 328;
            this.label17.Text = "Angle";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRadial_LoadAng
            // 
            this.txtRadial_LoadAng.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRadial_LoadAng.Location = new System.Drawing.Point(268, 26);
            this.txtRadial_LoadAng.Name = "txtRadial_LoadAng";
            this.txtRadial_LoadAng.ReadOnly = true;
            this.txtRadial_LoadAng.Size = new System.Drawing.Size(50, 21);
            this.txtRadial_LoadAng.TabIndex = 326;
            this.txtRadial_LoadAng.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRadial_LoadAng.TextChanged += new System.EventHandler(this.txtLoadAngle_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 259;
            this.label1.Text = "Speed";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSpeed_Design
            // 
            this.txtSpeed_Design.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpeed_Design.ForeColor = System.Drawing.Color.Black;
            this.txtSpeed_Design.Location = new System.Drawing.Point(72, 18);
            this.txtSpeed_Design.Name = "txtSpeed_Design";
            this.txtSpeed_Design.ReadOnly = true;
            this.txtSpeed_Design.Size = new System.Drawing.Size(50, 21);
            this.txtSpeed_Design.TabIndex = 1;
            this.txtSpeed_Design.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(126, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 279;
            this.label8.Text = "RPM";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpRot_Directionality
            // 
            this.grpRot_Directionality.Controls.Add(this.optRot_Directionality_Bi);
            this.grpRot_Directionality.Controls.Add(this.optRot_Directionality_Uni);
            this.grpRot_Directionality.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRot_Directionality.ForeColor = System.Drawing.Color.Black;
            this.grpRot_Directionality.Location = new System.Drawing.Point(194, 9);
            this.grpRot_Directionality.Name = "grpRot_Directionality";
            this.grpRot_Directionality.Size = new System.Drawing.Size(125, 45);
            this.grpRot_Directionality.TabIndex = 11;
            this.grpRot_Directionality.TabStop = false;
            this.grpRot_Directionality.Text = "Directionality:";
            this.grpRot_Directionality.Visible = false;
            // 
            // optRot_Directionality_Bi
            // 
            this.optRot_Directionality_Bi.AutoSize = true;
            this.optRot_Directionality_Bi.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optRot_Directionality_Bi.Location = new System.Drawing.Point(78, 19);
            this.optRot_Directionality_Bi.Name = "optRot_Directionality_Bi";
            this.optRot_Directionality_Bi.Size = new System.Drawing.Size(36, 17);
            this.optRot_Directionality_Bi.TabIndex = 13;
            this.optRot_Directionality_Bi.Text = "Bi";
            this.optRot_Directionality_Bi.UseVisualStyleBackColor = true;
            this.optRot_Directionality_Bi.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // optRot_Directionality_Uni
            // 
            this.optRot_Directionality_Uni.AutoSize = true;
            this.optRot_Directionality_Uni.Checked = true;
            this.optRot_Directionality_Uni.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optRot_Directionality_Uni.Location = new System.Drawing.Point(11, 19);
            this.optRot_Directionality_Uni.Name = "optRot_Directionality_Uni";
            this.optRot_Directionality_Uni.Size = new System.Drawing.Size(43, 17);
            this.optRot_Directionality_Uni.TabIndex = 12;
            this.optRot_Directionality_Uni.TabStop = true;
            this.optRot_Directionality_Uni.Text = "Uni";
            this.optRot_Directionality_Uni.UseVisualStyleBackColor = true;
            this.optRot_Directionality_Uni.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // tabOilSupply
            // 
            this.tabOilSupply.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabOilSupply.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabOilSupply.Controls.Add(this.lblFlowReqd_Unit);
            this.tabOilSupply.Controls.Add(this.txtFlowReqd);
            this.tabOilSupply.Controls.Add(this.label95);
            this.tabOilSupply.Controls.Add(this.label12);
            this.tabOilSupply.Controls.Add(this.txtOilSupply_Temp);
            this.tabOilSupply.Controls.Add(this.lblPress);
            this.tabOilSupply.Controls.Add(this.lblPressUnit);
            this.tabOilSupply.Controls.Add(this.lblTemp);
            this.tabOilSupply.Controls.Add(this.label7);
            this.tabOilSupply.Controls.Add(this.lblTempUnit);
            this.tabOilSupply.Controls.Add(this.lblTempDegF);
            this.tabOilSupply.Controls.Add(this.txtOilSupply_Press);
            this.tabOilSupply.Controls.Add(this.lblTempDegC);
            this.tabOilSupply.Controls.Add(this.txtLube_Type);
            this.tabOilSupply.Location = new System.Drawing.Point(4, 22);
            this.tabOilSupply.Name = "tabOilSupply";
            this.tabOilSupply.Padding = new System.Windows.Forms.Padding(3);
            this.tabOilSupply.Size = new System.Drawing.Size(389, 240);
            this.tabOilSupply.TabIndex = 1;
            this.tabOilSupply.Text = "Oil Supply";
            // 
            // lblFlowReqd_Unit
            // 
            this.lblFlowReqd_Unit.AutoSize = true;
            this.lblFlowReqd_Unit.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblFlowReqd_Unit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlowReqd_Unit.Location = new System.Drawing.Point(140, 71);
            this.lblFlowReqd_Unit.Name = "lblFlowReqd_Unit";
            this.lblFlowReqd_Unit.Size = new System.Drawing.Size(32, 13);
            this.lblFlowReqd_Unit.TabIndex = 489;
            this.lblFlowReqd_Unit.Text = "gpm";
            this.lblFlowReqd_Unit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFlowReqd
            // 
            this.txtFlowReqd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFlowReqd.Location = new System.Drawing.Point(86, 67);
            this.txtFlowReqd.Name = "txtFlowReqd";
            this.txtFlowReqd.ReadOnly = true;
            this.txtFlowReqd.Size = new System.Drawing.Size(50, 21);
            this.txtFlowReqd.TabIndex = 487;
            this.txtFlowReqd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label95.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label95.Location = new System.Drawing.Point(11, 71);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(69, 13);
            this.label95.TabIndex = 488;
            this.label95.Text = "Reqd. Flow";
            this.label95.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(21, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 13);
            this.label12.TabIndex = 321;
            this.label12.Text = "Lubricant";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOilSupply_Temp
            // 
            this.txtOilSupply_Temp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOilSupply_Temp.Location = new System.Drawing.Point(242, 102);
            this.txtOilSupply_Temp.Name = "txtOilSupply_Temp";
            this.txtOilSupply_Temp.ReadOnly = true;
            this.txtOilSupply_Temp.Size = new System.Drawing.Size(50, 21);
            this.txtOilSupply_Temp.TabIndex = 314;
            this.txtOilSupply_Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPress
            // 
            this.lblPress.AutoSize = true;
            this.lblPress.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblPress.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPress.Location = new System.Drawing.Point(23, 106);
            this.lblPress.Name = "lblPress";
            this.lblPress.Size = new System.Drawing.Size(57, 13);
            this.lblPress.TabIndex = 317;
            this.lblPress.Text = "Pressure";
            this.lblPress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPressUnit
            // 
            this.lblPressUnit.AutoSize = true;
            this.lblPressUnit.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblPressUnit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPressUnit.Location = new System.Drawing.Point(140, 106);
            this.lblPressUnit.Name = "lblPressUnit";
            this.lblPressUnit.Size = new System.Drawing.Size(30, 13);
            this.lblPressUnit.TabIndex = 318;
            this.lblPressUnit.Text = "psig";
            this.lblPressUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTemp
            // 
            this.lblTemp.AutoSize = true;
            this.lblTemp.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblTemp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemp.Location = new System.Drawing.Point(198, 106);
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Size = new System.Drawing.Size(38, 13);
            this.lblTemp.TabIndex = 320;
            this.lblTemp.Text = "Temp";
            this.lblTemp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(158, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 327;
            this.label7.Text = "Type";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTempUnit
            // 
            this.lblTempUnit.AutoSize = true;
            this.lblTempUnit.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblTempUnit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempUnit.Location = new System.Drawing.Point(296, 106);
            this.lblTempUnit.Name = "lblTempUnit";
            this.lblTempUnit.Size = new System.Drawing.Size(19, 13);
            this.lblTempUnit.TabIndex = 322;
            this.lblTempUnit.Text = "°F";
            this.lblTempUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTempDegF
            // 
            this.lblTempDegF.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblTempDegF.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempDegF.Location = new System.Drawing.Point(296, 106);
            this.lblTempDegF.Name = "lblTempDegF";
            this.lblTempDegF.Size = new System.Drawing.Size(8, 8);
            this.lblTempDegF.TabIndex = 323;
            this.lblTempDegF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOilSupply_Press
            // 
            this.txtOilSupply_Press.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOilSupply_Press.Location = new System.Drawing.Point(86, 102);
            this.txtOilSupply_Press.Name = "txtOilSupply_Press";
            this.txtOilSupply_Press.ReadOnly = true;
            this.txtOilSupply_Press.Size = new System.Drawing.Size(50, 21);
            this.txtOilSupply_Press.TabIndex = 312;
            this.txtOilSupply_Press.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTempDegC
            // 
            this.lblTempDegC.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblTempDegC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempDegC.Location = new System.Drawing.Point(422, 103);
            this.lblTempDegC.Name = "lblTempDegC";
            this.lblTempDegC.Size = new System.Drawing.Size(8, 8);
            this.lblTempDegC.TabIndex = 325;
            this.lblTempDegC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLube_Type
            // 
            this.txtLube_Type.BackColor = System.Drawing.SystemColors.Control;
            this.txtLube_Type.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLube_Type.Location = new System.Drawing.Point(86, 32);
            this.txtLube_Type.Name = "txtLube_Type";
            this.txtLube_Type.ReadOnly = true;
            this.txtLube_Type.Size = new System.Drawing.Size(178, 21);
            this.txtLube_Type.TabIndex = 333;
            this.txtLube_Type.TextChanged += new System.EventHandler(this.txtLube_Type_TextChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(324, 284);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 21;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(235, 284);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 20;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cmdClose
            // 
            this.cmdClose.BackColor = System.Drawing.Color.Silver;
            this.cmdClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdClose.Image = ((System.Drawing.Image)(resources.GetObject("cmdClose.Image")));
            this.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdClose.Location = new System.Drawing.Point(12, 284);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(80, 32);
            this.cmdClose.TabIndex = 747;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = false;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // frmOpCond
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(422, 334);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmOpCond";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operating Conditions";
            this.Load += new System.EventHandler(this.frmOperCond_Load);
            this.panel1.ResumeLayout(false);
            this.tbOpCond.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.grpStaticLoad.ResumeLayout(false);
            this.grpStaticLoad.PerformLayout();
            this.grpRot_Directionality.ResumeLayout(false);
            this.grpRot_Directionality.PerformLayout();
            this.tabOilSupply.ResumeLayout(false);
            this.tabOilSupply.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSpeed_Design;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox grpStaticLoad;
        internal System.Windows.Forms.Label lblStaticLoad_Thrust_Back;
        internal System.Windows.Forms.Label lblStaticLoad_Thrust_Front;
        private System.Windows.Forms.TextBox txtThrust_Load_Front;
        private System.Windows.Forms.TextBox txtThrust_Load_Back;
        internal System.Windows.Forms.Label lblStaticLoad_Thrust;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRadial_Load;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtRadial_LoadAng;
        private System.Windows.Forms.TabControl tbOpCond;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabOilSupply;
        internal System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtOilSupply_Temp;
        internal System.Windows.Forms.Label lblPress;
        internal System.Windows.Forms.Label lblPressUnit;
        internal System.Windows.Forms.Label lblTemp;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label lblTempUnit;
        internal System.Windows.Forms.Label lblTempDegF;
        private System.Windows.Forms.TextBox txtOilSupply_Press;
        internal System.Windows.Forms.Label lblTempDegC;
        private System.Windows.Forms.TextBox txtLube_Type;
        internal System.Windows.Forms.Label lblFlowReqd_Unit;
        private System.Windows.Forms.TextBox txtFlowReqd;
        internal System.Windows.Forms.Label label95;
        internal System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpRot_Directionality;
        private System.Windows.Forms.RadioButton optRot_Directionality_Bi;
        private System.Windows.Forms.RadioButton optRot_Directionality_Uni;
        private System.Windows.Forms.Button cmdClose;
    }
}