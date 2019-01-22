namespace BearingCAD22
{
    partial class frmPerformDataBearing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPerformDataBearing));
            this.lblBorder = new System.Windows.Forms.Label();
            this.tbBearing = new System.Windows.Forms.TabControl();
            this.tabRadialBearing = new System.Windows.Forms.TabPage();
            this.lblPower_HP_Unit = new System.Windows.Forms.Label();
            this.txtTempRise_F_Radial = new System.Windows.Forms.TextBox();
            this.label94 = new System.Windows.Forms.Label();
            this.txtPower_HP_Radial = new System.Windows.Forms.TextBox();
            this.label96 = new System.Windows.Forms.Label();
            this.lblTempRise_F_Radial_Unit = new System.Windows.Forms.Label();
            this.txtPower_Met_Radial = new System.Windows.Forms.TextBox();
            this.txtTempRise_Met_Radial = new System.Windows.Forms.TextBox();
            this.label89 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.tbBearing.SuspendLayout();
            this.tabRadialBearing.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.BackColor = System.Drawing.Color.Black;
            this.lblBorder.Location = new System.Drawing.Point(1, 1);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(275, 178);
            this.lblBorder.TabIndex = 7;
            // 
            // tbBearing
            // 
            this.tbBearing.Controls.Add(this.tabRadialBearing);
            this.tbBearing.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBearing.Location = new System.Drawing.Point(12, 12);
            this.tbBearing.Name = "tbBearing";
            this.tbBearing.SelectedIndex = 0;
            this.tbBearing.Size = new System.Drawing.Size(251, 113);
            this.tbBearing.TabIndex = 9;
            // 
            // tabRadialBearing
            // 
            this.tabRadialBearing.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabRadialBearing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabRadialBearing.Controls.Add(this.lblPower_HP_Unit);
            this.tabRadialBearing.Controls.Add(this.txtTempRise_F_Radial);
            this.tabRadialBearing.Controls.Add(this.label94);
            this.tabRadialBearing.Controls.Add(this.txtPower_HP_Radial);
            this.tabRadialBearing.Controls.Add(this.label96);
            this.tabRadialBearing.Controls.Add(this.lblTempRise_F_Radial_Unit);
            this.tabRadialBearing.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabRadialBearing.Location = new System.Drawing.Point(4, 22);
            this.tabRadialBearing.Name = "tabRadialBearing";
            this.tabRadialBearing.Padding = new System.Windows.Forms.Padding(3);
            this.tabRadialBearing.Size = new System.Drawing.Size(243, 87);
            this.tabRadialBearing.TabIndex = 0;
            this.tabRadialBearing.Text = "Radial";
            // 
            // lblPower_HP_Unit
            // 
            this.lblPower_HP_Unit.AutoSize = true;
            this.lblPower_HP_Unit.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblPower_HP_Unit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPower_HP_Unit.Location = new System.Drawing.Point(170, 19);
            this.lblPower_HP_Unit.Name = "lblPower_HP_Unit";
            this.lblPower_HP_Unit.Size = new System.Drawing.Size(25, 13);
            this.lblPower_HP_Unit.TabIndex = 506;
            this.lblPower_HP_Unit.Text = "kW";
            this.lblPower_HP_Unit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTempRise_F_Radial
            // 
            this.txtTempRise_F_Radial.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTempRise_F_Radial.Location = new System.Drawing.Point(115, 49);
            this.txtTempRise_F_Radial.Name = "txtTempRise_F_Radial";
            this.txtTempRise_F_Radial.ReadOnly = true;
            this.txtTempRise_F_Radial.Size = new System.Drawing.Size(50, 21);
            this.txtTempRise_F_Radial.TabIndex = 471;
            this.txtTempRise_F_Radial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTempRise_F_Radial.TextChanged += new System.EventHandler(this.txtTempRise_TextChanged);
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label94.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label94.Location = new System.Drawing.Point(40, 53);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(70, 13);
            this.label94.TabIndex = 483;
            this.label94.Text = "Temp. Rise";
            this.label94.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPower_HP_Radial
            // 
            this.txtPower_HP_Radial.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPower_HP_Radial.Location = new System.Drawing.Point(116, 15);
            this.txtPower_HP_Radial.Name = "txtPower_HP_Radial";
            this.txtPower_HP_Radial.ReadOnly = true;
            this.txtPower_HP_Radial.Size = new System.Drawing.Size(50, 21);
            this.txtPower_HP_Radial.TabIndex = 467;
            this.txtPower_HP_Radial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPower_HP_Radial.TextChanged += new System.EventHandler(this.txtPower_TextChanged);
            // 
            // label96
            // 
            this.label96.AutoSize = true;
            this.label96.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label96.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label96.Location = new System.Drawing.Point(39, 18);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(71, 13);
            this.label96.TabIndex = 481;
            this.label96.Text = "Power Loss";
            this.label96.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTempRise_F_Radial_Unit
            // 
            this.lblTempRise_F_Radial_Unit.AutoSize = true;
            this.lblTempRise_F_Radial_Unit.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblTempRise_F_Radial_Unit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempRise_F_Radial_Unit.Location = new System.Drawing.Point(170, 53);
            this.lblTempRise_F_Radial_Unit.Name = "lblTempRise_F_Radial_Unit";
            this.lblTempRise_F_Radial_Unit.Size = new System.Drawing.Size(22, 13);
            this.lblTempRise_F_Radial_Unit.TabIndex = 489;
            this.lblTempRise_F_Radial_Unit.Text = "°C";
            this.lblTempRise_F_Radial_Unit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPower_Met_Radial
            // 
            this.txtPower_Met_Radial.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPower_Met_Radial.ForeColor = System.Drawing.Color.Black;
            this.txtPower_Met_Radial.Location = new System.Drawing.Point(155, -5);
            this.txtPower_Met_Radial.Name = "txtPower_Met_Radial";
            this.txtPower_Met_Radial.Size = new System.Drawing.Size(50, 21);
            this.txtPower_Met_Radial.TabIndex = 468;
            this.txtPower_Met_Radial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPower_Met_Radial.Visible = false;
            this.txtPower_Met_Radial.TextChanged += new System.EventHandler(this.txtPower_TextChanged);
            // 
            // txtTempRise_Met_Radial
            // 
            this.txtTempRise_Met_Radial.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTempRise_Met_Radial.ForeColor = System.Drawing.Color.Black;
            this.txtTempRise_Met_Radial.Location = new System.Drawing.Point(99, -15);
            this.txtTempRise_Met_Radial.Name = "txtTempRise_Met_Radial";
            this.txtTempRise_Met_Radial.Size = new System.Drawing.Size(50, 21);
            this.txtTempRise_Met_Radial.TabIndex = 472;
            this.txtTempRise_Met_Radial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTempRise_Met_Radial.Visible = false;
            this.txtTempRise_Met_Radial.TextChanged += new System.EventHandler(this.txtTempRise_TextChanged);
            // 
            // label89
            // 
            this.label89.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label89.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label89.Location = new System.Drawing.Point(82, 153);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(10, 12);
            this.label89.TabIndex = 488;
            this.label89.Text = "0";
            this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label89.Visible = false;
            // 
            // label90
            // 
            this.label90.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label90.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label90.Location = new System.Drawing.Point(228, 153);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(10, 17);
            this.label90.TabIndex = 487;
            this.label90.Text = "F";
            this.label90.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label90.Visible = false;
            // 
            // label92
            // 
            this.label92.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label92.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label92.Location = new System.Drawing.Point(82, 132);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(31, 17);
            this.label92.TabIndex = 485;
            this.label92.Text = "HP";
            this.label92.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label92.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cmdClose);
            this.panel2.Controls.Add(this.tbBearing);
            this.panel2.Controls.Add(this.cmdOK);
            this.panel2.Controls.Add(this.cmdCancel);
            this.panel2.Controls.Add(this.label92);
            this.panel2.Controls.Add(this.label90);
            this.panel2.Controls.Add(this.txtTempRise_Met_Radial);
            this.panel2.Controls.Add(this.txtPower_Met_Radial);
            this.panel2.Controls.Add(this.label89);
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(273, 176);
            this.panel2.TabIndex = 10;
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(121, 137);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(66, 28);
            this.cmdOK.TabIndex = 479;
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
            this.cmdCancel.Location = new System.Drawing.Point(193, 137);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(70, 28);
            this.cmdCancel.TabIndex = 480;
            this.cmdCancel.Text = "   &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.BackColor = System.Drawing.Color.Silver;
            this.cmdClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdClose.Image = ((System.Drawing.Image)(resources.GetObject("cmdClose.Image")));
            this.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdClose.Location = new System.Drawing.Point(12, 137);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(66, 28);
            this.cmdClose.TabIndex = 748;
            this.cmdClose.Text = "  Close";
            this.cmdClose.UseVisualStyleBackColor = false;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // frmPerformDataBearing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(278, 183);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblBorder);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmPerformDataBearing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Performance Data: Bearing";
            this.Load += new System.EventHandler(this.frmPerformance_Load);
            this.tbBearing.ResumeLayout(false);
            this.tabRadialBearing.ResumeLayout(false);
            this.tabRadialBearing.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.TabControl tbBearing;
        private System.Windows.Forms.TabPage tabRadialBearing;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Label lblPower_HP_Unit;
        internal System.Windows.Forms.Label lblTempRise_F_Radial_Unit;
        internal System.Windows.Forms.Label label89;
        internal System.Windows.Forms.Label label90;
        private System.Windows.Forms.TextBox txtTempRise_F_Radial;
        internal System.Windows.Forms.Label label92;
        private System.Windows.Forms.TextBox txtTempRise_Met_Radial;
        internal System.Windows.Forms.Label label94;
        private System.Windows.Forms.TextBox txtPower_Met_Radial;
        internal System.Windows.Forms.Label label96;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TextBox txtPower_HP_Radial;
        private System.Windows.Forms.Button cmdClose;
    }
}