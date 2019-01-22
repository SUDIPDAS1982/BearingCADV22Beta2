
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmSeal                                '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  20DEC18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//       Private Sub       frmSeal_Load                        ()
//       Private Sub       DisplayData                         ()

//       Private Sub       cmdClose_Click                      ()
//       Private Sub       SaveData                            ()
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace BearingCAD22
{
    public partial class frmSeal : Form
    {
        #region "MEMBER VARIABLE DECLARATION:"
        //************************************
            //private const string mcBladeThickness = "0.060";   // Move to clsSeal SG 06MAR12
            //private const string mcLiningThickness = "0.030"; 

            //....Local Class Object
           // private clsEndPlate[] mBearing.EndPlate = new clsEndPlate[2];
             private clsJBearing mBearing;

            private ComboBox[] mcmbSealType;
            private TextBox[] mtxtDBore;
            private TextBox[] mtxtLength;
            private ComboBox[] mcmbBladeCount;
            private TextBox[] mtxtBladeT;
            private ComboBox[] mcmbBladeAngleTaper;
            private ComboBox[] mcmbMatBase_WCode;
            private TextBox[] mtxtMatBase_Name;
            private TextBox[] mtxtMatLining_Name;
            private ComboBox[] mcmbMatLining;
            private TextBox[] mtxtMatLiningT;
            private CheckBox[] mchkMat_LiningExists;
            private Label[] mlblMetric;

            private Boolean mblnDBore_Min_Front_Changed = false;
            private Boolean mblnDBore_Max_Front_Changed = false;

            private Boolean mblnDBore_Min_Back_Changed = false;
            private Boolean mblnDBore_Max_Back_Changed = false;

            private Boolean mblnFornt_Back_Copied = false;
            private Boolean mIsFront_BladeCount_2 = false;
            private Boolean mIsBack_BladeCount_2 = false;
            private Boolean mIsDataValidated = false;

        #endregion


        #region" FORM CONSTRUCTOR & RELATED ROUTINES:"
        //********************************************

        public frmSeal()
        //===============
        {
            //mBearing = new clsJBearing(clsUnit.eSystem.English, clsBearing.eType.JBearing);
            InitializeComponent();

            //....Set Local Object.
            //SetLocalObject();

            //....Set Seal Type
            mcmbSealType = new ComboBox[] { cmbType_Front, cmbType_Back };
            //.....Set BoreD TextBox.
            mtxtDBore = new TextBox[] { txtDBore_Range_Min_Front, txtDBore_Range_Max_Front, txtDBore_Range_Min_Back, txtDBore_Range_Max_Back };

            //....Length
            mtxtLength = new TextBox[] { txtL_Front, txtL_Back };

            //....Blade Count
            mcmbBladeCount = new ComboBox[] { cmbBlade_Count_Front, cmbBlade_Count_Back };
            //....Blade Thickness
            mtxtBladeT = new TextBox[] { txtBlade_T_Front, txtBlade_T_Back };
            //....Blade Angle Taper
            mcmbBladeAngleTaper = new ComboBox[] { cmbBlade_AngTaper_Front, cmbBlade_AngTaper_Back };
            //....Material Base
            mcmbMatBase_WCode = new ComboBox[] { cmbMat_Base_WCode_Front, cmbMat_Base_WCode_Back };
            mtxtMatBase_Name = new TextBox[] { txtMat_Base_Name_Front, txtMat_Base_Name_Back };
            //....Material Lining 
            //mchkMatLining = new CheckBox[] { chkMat_Lining_Front, chkMat_Lining_Back };
            mcmbMatLining = new ComboBox[] { cmbMat_Lining_WCode_Front, cmbMat_Lining_WCode_Back };
            mtxtMatLining_Name = new TextBox[] { txtMat_Lining_Name_Front, txtMat_Lining_Name_Back };
            //....Material Thick
            mtxtMatLiningT = new TextBox[] { txtMat_LiningT_Front, txtMat_LiningT_Back };

            mchkMat_LiningExists = new CheckBox[] { chkMat_LiningExists_Front, chkMat_LiningExists_Back };

            mlblMetric = new Label[] {  lblMat_LiningT_Front_Metric,  lblMat_LiningT_Back_Metric };
        
            //.....Load Seal Type.
            LoadSealType();

            //....Taper Angle
            LoadTaperAngle();

            //.....Populate Base and Lining Material.
            PopulateMaterial();
           
        }


        private void LoadSealType()
        //=========================         
        {
            for (int i = 0; i < 2; i++)
            {
                mcmbSealType[i].DataSource = Enum.GetValues(typeof(clsEndPlate.clsSeal.eDesign));
                mcmbSealType[i].SelectedIndex = 0;
            }
        }

        private void LoadTaperAngle()
        //============================
        {
            int[] pAngle = new int[] { 45, 50, 55, 60 };

            for (int i = 0; i < 2; i++)
            {
                mcmbBladeAngleTaper[i].Items.Clear();

                for (int j = 0; j < pAngle.Length; j++)
                {
                    mcmbBladeAngleTaper[i].Items.Add(pAngle[j].ToString());
                }
            }
        }


        private void PopulateMaterial()
        //==============================
        {
            ////for (int i = 0; i < 2; i++)
            ////{
            ////    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            ////    var pQryBase = (from pRec in pBearingDBEntities.tblData_Mat
            ////                    where pRec.fldLining == false && pRec.fldCode_Waukesha != null && pRec.fldEndPlate == true
            ////                    orderby pRec.fldCode_Waukesha ascending
            ////                    select pRec).ToList();

            ////    if (pQryBase.Count() > 0)
            ////    {
            ////        mcmbMatBase_WCode[i].Items.Clear();
            ////        for (int j = 0; j < pQryBase.Count; j++)
            ////        {
            ////            mcmbMatBase_WCode[i].Items.Add(pQryBase[j].fldCode_Waukesha);
            ////        }
            ////        mcmbMatBase_WCode[i].Items.Add("Other");
            ////        mcmbMatBase_WCode[i].SelectedIndex = 0;
            ////    }

            ////    //....Lining Material.
            ////    var pQryLiningMat = (from pRec in pBearingDBEntities.tblData_Mat
            ////                         where pRec.fldLining == true
            ////                         orderby pRec.fldName ascending
            ////                         select pRec).ToList();
            ////    mcmbMatLining[i].Items.Clear();
            ////    if (pQryLiningMat.Count() > 0)
            ////    {
            ////        for (int k = 0; k < pQryLiningMat.Count; k++)
            ////        {
            ////            mcmbMatLining[i].Items.Add(pQryLiningMat[k].fldCode_Waukesha);
            ////        }
            ////        mcmbMatLining[i].Items.Add("Other");
            ////        mcmbMatLining[i].SelectedIndex = 0;
            ////    }
            ////}

            for (int i = 0; i < 2; i++)
            {
                //....Base Material.
                //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                mcmbMatBase_WCode[i].Items.Clear();
                string pWHERE = " WHERE Lining = false and Code_Waukesha is not null and EndPlate = true";                
                int pMat_Base_WCode_RecCount = modMain.gDB.PopulateCmbBox(mcmbMatBase_WCode[i], modMain.gFiles.FileTitle_EXCEL_MatData, "[Mat$]", "Code_Waukesha", pWHERE, true);

                
                if (pMat_Base_WCode_RecCount > 0)
                {
                    mcmbMatBase_WCode[i].Items.Add("Other");
                    mcmbMatBase_WCode[i].SelectedIndex = 0;
                }

                //....Lining Material. 
                mcmbMatLining[i].Items.Clear();
                pWHERE = " WHERE Lining = true and Code_Waukesha is not null";
                int pMat_Lining_WCode_RecCount = modMain.gDB.PopulateCmbBox(mcmbMatLining[i], modMain.gFiles.FileTitle_EXCEL_MatData, "[Mat$]", "Code_Waukesha", pWHERE, true);
                
                if (pMat_Lining_WCode_RecCount > 0)
                {
                    mcmbMatLining[i].Items.Add("Other");
                    mcmbMatLining[i].SelectedIndex = 0;
                }

            }

        }

        #endregion


        #region" FORM RELATED ROUTINES:"
        //******************************

        private void frmSeal_Load(object sender, EventArgs e)
        //===================================================
        {
            InitializeControls();

            //....Taper Angle
            LoadTaperAngle();

            //....Set Local Object.
            SetLocalObject();

            //....Set Tab Pages.
            SetTabPages();

            DisplayLblMetric(txtMat_LiningT_Front, lblMat_LiningT_Front_Metric, chkMat_LiningExists_Front.Checked);
            DisplayLblMetric(txtMat_LiningT_Back,  lblMat_LiningT_Back_Metric, chkMat_LiningExists_Back.Checked);

            //....Diaplay Data.
            DisplayData();

            //....Set Control.         
            SetControl_MatLining();
        }

        private void InitializeControls()
        //===============================
        {
            //....Show Labels for Metric 
            for (int i = 0; i < mlblMetric.Length - 1; i++)
            {
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    mlblMetric[i].Visible = true;
                }
                else
                {
                    mlblMetric[i].Visible = false;
                }
            }
        }


        private void SetLocalObject()
        //===========================
        {
           // mBearing.EndPlate = new clsSeal[2];
            mBearing = (clsJBearing)(modMain.gProject.PNR.Bearing).Clone();

            for (int i = 0; i < 2; i++)
            {
               mBearing.EndPlate[i] = (clsEndPlate)((clsJBearing)(modMain.gProject.PNR.Bearing)).EndPlate[i].Clone();
                //if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                //{
                //    mBearing.EndPlate[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                //}
            }
        }


        private void SetTabPages()
        //========================
        {
            TabPage[] pTabPages = new TabPage[] { tabFront, tabBack };

            tbEndSealData.TabPages.Clear();

            for (int i = 0; i < 2; i++)
            {
                tbEndSealData.TabPages.Add(pTabPages[i]);
                //if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                //    tbEndSealData.TabPages.Add(pTabPages[i]);
            }

        }

        private void DisplayData()
        //========================
        {
            for (int i = 0; i < 2; i++)
            {
                //if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                //{

                    //....Seal Type
                    mcmbSealType[i].Text = mBearing.EndPlate[i].Seal.Design.ToString();

                    //....Bore Dia
                    int k = 0;
                    if (i == 1) k = 2;

                    for (int j = 0; j < 2; j++, k++)
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            mtxtDBore[k].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[i].DBore_Range[j]));
                        }
                        else
                        {
                            mtxtDBore[k].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[i].DBore_Range[j]);
                        }
                    }


                    if (mBearing.EndPlate[i].L < modMain.gcEPS)
                    {
                        double pDepth = ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.EndPlateCB_Depth_Def();
                        mBearing.EndPlate[i].L = pDepth;
                        mtxtLength[i].ForeColor = Color.Blue;
                    }
                    else
                    {
                        mtxtLength[i].ForeColor = Color.Black;
                    }

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mtxtLength[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[i].L));
                    }
                    else
                    {
                        mtxtLength[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[i].L);
                    }

                    //  Blade
                    //  -----

                    //....Count.
                    mcmbBladeCount[i].Text = modMain.ConvIntToStr(mBearing.EndPlate[i].Seal.Blade.Count);

                    if (mBearing.EndPlate[i].Seal.Blade.Count != 0)
                    {
                        //....Thick
                        //if(mEndSeal[i].Blade.Count > 1)
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            mtxtBladeT[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[i].Seal.Blade.T));
                        }
                        else
                        {
                            mtxtBladeT[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[i].Seal.Blade.T);
                        }

                        //....Blade Taper Angle.
                        //else if(mEndSeal[i].Blade.Count == 1)
                        if (mBearing.EndPlate[i].Seal.Blade.Count == 1)
                        {
                            if (mcmbBladeAngleTaper[i].Items.Contains(mBearing.EndPlate[i].Seal.Blade.AngTaper.ToString()))
                            {
                                mcmbBladeAngleTaper[i].SelectedIndex = mcmbBladeAngleTaper[i].Items.IndexOf(mBearing.EndPlate[i].Seal.Blade.AngTaper.ToString());
                            }

                            else
                            {
                                mcmbBladeAngleTaper[i].Text = modMain.ConvDoubleToStr(mBearing.EndPlate[i].Seal.Blade.AngTaper, "");
                            }
                        }

                    }


                    //  Material
                    //  --------
                    mcmbMatBase_WCode[i].Text = mBearing.EndPlate[i].Mat.WCode.Base;
                    mtxtMatBase_Name[i].Text = mBearing.EndPlate[i].Mat.Base;
                    string pMatLining = mBearing.EndPlate[i].Mat.Lining;                   
                 
                    mchkMat_LiningExists[i].Checked = mBearing.EndPlate[i].Mat.LiningExists;

                    mcmbMatLining[i].Text = mBearing.EndPlate[i].Mat.WCode.Lining;
                    mBearing.EndPlate[i].Mat.Lining = pMatLining;
                    mtxtMatLining_Name[i].Text = mBearing.EndPlate[i].Mat.Lining;

                    if (mBearing.EndPlate[i].Mat.LiningExists)
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            mtxtMatLiningT[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[i].Mat_LiningT));
                        }
                        else
                        {
                            mtxtMatLiningT[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[i].Mat_LiningT);
                        }
                       // mtxtMatLiningT[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[i].Mat_LiningT);
                    }
                    else
                    {
                        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //{
                            //lblMat_LiningT_Front_Unit.Visible = false;
                            //lblMat_LiningT_Front_Metric.Visible = false;
                            //lblMat_LiningT_Back_Unit.Visible = false;
                            //lblMat_LiningT_Back_Metric.Visible = false;
                        //}
                    }

                    ////if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    ////{
                    ////    mtxtMatLiningT[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndSeal[i].Mat_LiningT));
                    ////}
                    ////else
                    ////{
                    ////    mtxtMatLiningT[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndSeal[i].Mat_LiningT);
                    ////}
                    ////SetMatLiningT(mcmbMatLining[i].Text);
                //}
            }

        }


        private void SetTxtForeColorAndDefVal(Double T_In, TextBox TxtBox_In, Double DefVal_In)
        //======================================================================================     
        {
            if (T_In != 0.000)
            {
                if (Math.Abs(T_In - DefVal_In) < modMain.gcEPS)
                    TxtBox_In.ForeColor = Color.Magenta;
                else
                    TxtBox_In.ForeColor = Color.Black;
            }
            else
            {
                TxtBox_In.ForeColor = Color.Magenta;
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    TxtBox_In.Text = modMain.gProject.PNR.Unit.CEng_Met(DefVal_In).ToString("#0.000");
                }
                else
                {
                    TxtBox_In.Text = DefVal_In.ToString("#0.000");
                }
            }
        }

        private void SetTxtForeColor(TextBox TxtBox_In, Double DefVal_In)
        //===============================================================  
        {
            if (Math.Abs(modMain.ConvTextToDouble(TxtBox_In.Text) - DefVal_In) < modMain.gcEPS)
            {
                TxtBox_In.ForeColor = Color.Magenta;
            }
            else
            {
                TxtBox_In.ForeColor = Color.Black;
            }
        }


        private void SetControl_MatLining()
        //=================================
        {
            ////Boolean[] pblnVisible = new Boolean[2];

            ////for (int i = 0; i < 2; i++)
            ////{
            ////    if (mEndSeal[i] != null)
            ////    {
            ////        if (mEndSeal[i].Design == clsSeal.eDesign.Fixed)
            ////        {
            ////            pblnVisible[i] = true;
            ////        }
            ////        else
            ////        {
            ////            pblnVisible[i] = false;
            ////        }
            ////    }
            ////}

            ////lblMat_Lining_Front.Visible = !pblnVisible[0];
            ////cmbMat_Lining_Front.Visible = !pblnVisible[0];

            ////lblMat_LiningT_Front.Visible = !pblnVisible[0];
            ////txtMat_LiningT_Front.Visible = !pblnVisible[0];

            ////lblMat_Lining_Back.Visible = !pblnVisible[1];
            ////lblMat_LiningT_Back.Visible = !pblnVisible[1];

            ////txtMat_LiningT_Back.Visible = !pblnVisible[1];
            ////cmbMat_Lining_Back.Visible = !pblnVisible[1];
        }



        private void SetMatLiningT(string MatLining_In)
        //====================================================
        {
            if (MatLining_In != "None")
            {
                lblMat_LiningT_Front.Visible = true;
                txtMat_LiningT_Front.Visible = true;
            }
            else
            {
                lblMat_LiningT_Front.Visible = false;
                txtMat_LiningT_Front.Visible = false;
            }
        }

        private void DisplayLblMetric(TextBox TextBox_In, Label Lbl_Metric_Val_In, Boolean Reqd_In = true)
        //================================================================================================
        {
            double pVal = 0.0;
            if (TextBox_In.Text != "")
                pVal = Convert.ToDouble(TextBox_In.Text);

            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
            {
                if (Reqd_In)
                {
                    //Lbl_Unit_In.Visible = true;
                    if (pVal > modMain.gcEPS)
                    {
                        Lbl_Metric_Val_In.Visible = true;
                        Lbl_Metric_Val_In.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(modMain.gProject.PNR.Unit.CMet_Eng(pVal)) + "]";
                    }
                    else
                    {
                        Lbl_Metric_Val_In.Visible = false;
                    }
                }
                else
                {
                   // Lbl_Unit_In.Visible = false;
                    Lbl_Metric_Val_In.Visible = false;
                }
            }
            else
            {
                //Lbl_Unit_In.Visible = false;
                Lbl_Metric_Val_In.Visible = false;
            }
        }


        #endregion


        #region" CONTROL EVENT RELATED ROUTINE:"
        //***********************************

        #region "COMBOBOX RELATED ROUTINE:"
        //---------------------------------

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //====================================================================
        {
            ComboBox pCmbBox = (ComboBox)sender;

            switch (pCmbBox.Name)
            {
                case "cmbType_Front":
                    //======================
                    cmbType_Front.SelectedIndex = 0;
                    cmbBlade_Count_Front.Refresh();
                    cmbBlade_Count_Front.Items.Clear();                  

                    if (cmbType_Front.SelectedIndex == 0)
                    {
                        for (int i = 1; i < 3; i++)
                        {
                            cmbBlade_Count_Front.Items.Add(i.ToString());
                        }
                        //cmbBlade_Count_Front.SelectedIndex = 1;
                        cmbBlade_Count_Front.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbBlade_Count_Front.Items.Add("1");
                        cmbBlade_Count_Front.SelectedIndex = 0;
                    }
                    

                    if (cmbType_Front.Text != "")
                    {
                        if (mBearing != null)
                        {
                            if (mBearing.EndPlate[0] != null)
                            {
                                mBearing.EndPlate[0].Seal.Design = (clsEndPlate.clsSeal.eDesign)
                                                Enum.Parse(typeof(clsEndPlate.clsSeal.eDesign), cmbType_Front.Text);
                                SetControl_MatLining();

                                //if (mEndSeal[0].Mat_LiningT < modMain.gcEPS)
                                //{
                                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //    {
                                //        txtMat_LiningT_Front.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mEndSeal[0].DESIGN_LINING_THICK), "#0.000");
                                //    }
                                //    else
                                //    {
                                //        txtMat_LiningT_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].DESIGN_LINING_THICK, "#0.000");
                                //    }
                                //}
                            }
                        }
                    }
                    break;


                case "cmbType_Back":
                    //==================
                    cmbType_Back.SelectedIndex = 0;
                    cmbBlade_Count_Back.Refresh();
                    cmbBlade_Count_Back.Items.Clear();                    

                    if (cmbType_Back.SelectedIndex == 0)
                    {
                        for (int i = 1; i < 3; i++)
                        {
                            cmbBlade_Count_Back.Items.Add(i.ToString());
                        }
                        //cmbBlade_Count_Back.SelectedIndex = 1;
                        cmbBlade_Count_Back.SelectedIndex = 0;

                    }
                    else
                    {
                        cmbBlade_Count_Back.Items.Add("1");
                        cmbBlade_Count_Back.SelectedIndex = 0;
                    }                   

                    if (cmbType_Back.Text != "")
                    {
                        if (mBearing != null)
                        {
                            if (mBearing.EndPlate[1] != null)
                            {
                                mBearing.EndPlate[1].Seal.Design = (clsEndPlate.clsSeal.eDesign)
                                                   Enum.Parse(typeof(clsEndPlate.clsSeal.eDesign), cmbType_Back.Text);
                                SetControl_MatLining();

                                //if (mEndSeal[1].Mat_LiningT < modMain.gcEPS)
                                //{
                                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //    {
                                //        txtMat_LiningT_Back.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mEndSeal[1].DESIGN_LINING_THICK), "#0.000");
                                //    }
                                //    else
                                //    {
                                //        txtMat_LiningT_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].DESIGN_LINING_THICK, "#0.000");
                                //    }
                                //}
                            }
                        }
                    }
                    break;

                case "cmbBlade_Count_Front":
                    //=====================
                    //cmbBlade_Count_Front.SelectedIndex = 0;
                    if (cmbBlade_Count_Front.SelectedItem.ToString() == "1")
                    {                        
                        lblBladeThick_Front.Text = "Land L";

                        lblBlade_TapAng_Front.Visible = true;
                        cmbBlade_AngTaper_Front.Visible = true;
                        lblDeg_Front.Visible = true;

                        if (mBearing != null)
                        {
                            if (mBearing.EndPlate[0] != null)
                            {
                                if (mBearing.EndPlate[0].Seal.Blade.AngTaper < modMain.gcEPS)
                                    cmbBlade_AngTaper_Front.SelectedIndex = 0;
                                else
                                    cmbBlade_AngTaper_Front.Text = modMain.ConvDoubleToStr(mBearing.EndPlate[0].Seal.Blade.AngTaper, "");
                            }
                        }
                    }
                    else
                    {

                        lblBladeThick_Front.Text = "Thick";

                        lblBlade_TapAng_Front.Visible = false;
                        cmbBlade_AngTaper_Front.Visible = false;
                        lblDeg_Front.Visible = false;
                    }

                    if (cmbBlade_Count_Front.Text != "")
                    {
                        if (mBearing != null)
                        {
                            if (mBearing.EndPlate[0] != null)
                                mBearing.EndPlate[0].Seal.Blade.Count = modMain.ConvTextToInt(cmbBlade_Count_Front.Text);
                        }
                    }

                    if (mBearing != null)
                    {
                        if (mBearing.EndPlate[0] != null)
                        {
                            //if (mBearing.EndPlate[0].Seal.Blade.Count == 2)
                            //{
                            //    modMain.gblnSealDesignDetails = true;
                            //}
                            //else
                            //{
                            //    modMain.gblnSealDesignDetails = false;
                            //}
                            if (mBearing.EndPlate[0].Seal.Blade.Count == 2)
                            {
                                mIsFront_BladeCount_2 = true;
                            }
                            else
                            {
                                mIsFront_BladeCount_2 = false;
                            }
                        }
                    }
                    break;

                case "cmbBlade_Count_Back":
                    //=====================
                    //cmbBlade_Count_Back.SelectedIndex = 0;
                    if (cmbBlade_Count_Back.SelectedItem.ToString() == "1")
                    {
                        lblBladeThick_Back.Text = "Land L";

                        lblBlade_TapAng_Back.Visible = true;
                        cmbBlade_AngTaper_Back.Visible = true;
                        lblDeg_Back.Visible = true;
                        if (mBearing != null)
                        {
                            if (mBearing.EndPlate[1] != null)
                            {
                                if (mBearing.EndPlate[1].Seal.Blade.AngTaper < modMain.gcEPS)
                                    cmbBlade_AngTaper_Back.SelectedIndex = 0;
                                else
                                    cmbBlade_AngTaper_Back.Text = modMain.ConvDoubleToStr(mBearing.EndPlate[1].Seal.Blade.AngTaper, "");
                            }
                        }
                       
                    }
                    else
                    {

                        lblBladeThick_Back.Text = "Thick";
                        txtBlade_T_Back.Visible = true;

                        lblBlade_TapAng_Back.Visible = false;
                        cmbBlade_AngTaper_Back.Visible = false;
                        lblDeg_Back.Visible = false;
                    }

                    if (cmbBlade_Count_Back.Text != "")
                    {
                        if (mBearing != null)
                        {
                            if (mBearing.EndPlate[1] != null)
                                mBearing.EndPlate[1].Seal.Blade.Count = modMain.ConvTextToInt(cmbBlade_Count_Back.Text);
                        }
                    }

                    if (mBearing != null)
                    {
                        if (mBearing.EndPlate[1] != null)
                        {
                            if (mBearing.EndPlate[1].Seal.Blade.Count == 2)
                            {
                                mIsBack_BladeCount_2 = true;
                            }
                            else
                            {
                                mIsBack_BladeCount_2 = false;
                            }
                        }
                    }
                    break;

                case "cmbBlade_AngTaper_Front":
                    //========================
                    if (mBearing != null)
                    {
                        if (mBearing.EndPlate[0] != null)
                            mBearing.EndPlate[0].Seal.Blade.AngTaper = modMain.ConvTextToDouble(cmbBlade_AngTaper_Front.Text);
                    }
                    break;

                case "cmbBlade_AngTaper_Back":
                    //========================
                    if (mBearing != null)
                    {
                        if (mBearing.EndPlate[1] != null)
                            mBearing.EndPlate[1].Seal.Blade.AngTaper = modMain.ConvTextToDouble(cmbBlade_AngTaper_Back.Text);
                    }
                    break;

                ////case "cmbMat_Base_Front":
                ////    //==================
                ////    if (mEndSeal[0] != null)
                ////        mEndSeal[0].Mat.Base = pCmbBox.Text;
                ////    break;

                ////case "cmbMat_Base_Back":
                ////    //=================
                ////    if (mEndSeal[1] != null)
                ////        mEndSeal[1].Mat.Base = pCmbBox.Text;
                ////    break;

                case "cmbMat_Base_WCode_Front":
                    //-----------------------
                    if (mBearing != null)
                    {
                        if (mBearing.EndPlate[0] != null)
                        {
                            mBearing.EndPlate[0].Mat.WCode_Base = pCmbBox.Text;
                            if (pCmbBox.Text == "Other")
                            {
                                txtMat_Base_Name_Front.Text = "";
                                txtMat_Base_Name_Front.ReadOnly = false;
                                txtMat_Base_Name_Front.BackColor = Color.White;
                                chkMat_LiningExists_Front.Checked = false;
                            }
                            else
                            {
                                txtMat_Base_Name_Front.ReadOnly = true;
                                //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                                txtMat_Base_Name_Front.Text = mBearing.EndPlate[0].Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
                                txtMat_Base_Name_Front.BackColor = Color.LightGray;

                                if (txtMat_Base_Name_Front.Text == "STEEL")
                                {
                                    chkMat_LiningExists_Front.Checked = true;
                                }
                                else
                                {
                                    chkMat_LiningExists_Front.Checked = false;
                                }
                            }
                            mBearing.EndPlate[0].Mat.Base = txtMat_Base_Name_Front.Text;

                        }
                    }

                    break;

                case "cmbMat_Base_WCode_Back":
                    //---------------------
                    if (mBearing != null)
                    {
                        if (mBearing.EndPlate[1] != null)
                        {
                            mBearing.EndPlate[1].Mat.WCode_Base = pCmbBox.Text;
                            if (pCmbBox.Text == "Other")
                            {
                                txtMat_Base_Name_Back.Text = "";
                                txtMat_Base_Name_Back.ReadOnly = false;
                                txtMat_Base_Name_Back.BackColor = Color.White;
                                chkMat_LiningExists_Back.Checked = false;
                            }
                            else
                            {
                                txtMat_Base_Name_Back.ReadOnly = true;
                                //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                                txtMat_Base_Name_Back.Text = mBearing.EndPlate[1].Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
                                txtMat_Base_Name_Back.BackColor = Color.LightGray;
                                if (txtMat_Base_Name_Back.Text == "STEEL")
                                {
                                    chkMat_LiningExists_Back.Checked = true;
                                }
                                else
                                {
                                    chkMat_LiningExists_Back.Checked = false;
                                }
                            }
                            mBearing.EndPlate[1].Mat.Base = txtMat_Base_Name_Back.Text;
                        }
                    }
                    break;

                case "cmbMat_Lining_WCode_Front":
                    //=====================
                    ////SetMatLiningT(cmbMat_Lining_Front.Text);  
                    if (mBearing != null)
                    {
                        if (mBearing.EndPlate[0] != null)
                        {
                            mBearing.EndPlate[0].Mat.WCode_Lining = pCmbBox.Text;
                            if (pCmbBox.Text == "Other")
                            {
                                txtMat_Lining_Name_Front.Text = "";
                                txtMat_Lining_Name_Front.ReadOnly = false;
                                txtMat_Lining_Name_Front.BackColor = Color.White;
                            }
                            else
                            {
                                txtMat_Lining_Name_Front.ReadOnly = true;
                                //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                                txtMat_Lining_Name_Front.Text = mBearing.EndPlate[0].Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
                                txtMat_Lining_Name_Front.BackColor = Color.LightGray;
                            }
                            mBearing.EndPlate[0].Mat.Lining = txtMat_Lining_Name_Front.Text;
                        }
                    }
                    
                    break;

                case "cmbMat_Lining_WCode_Back":
                    //==========================
                    ////SetMatLiningT(cmbMat_Lining_Back.Text);
                    //if (mEndSeal[1] != null)
                    //    mEndSeal[1].Mat.Lining = pCmbBox.Text;
                    if (mBearing != null)
                    {
                        if (mBearing.EndPlate[1] != null)
                        {
                            mBearing.EndPlate[1].Mat.WCode_Lining = pCmbBox.Text;
                            if (pCmbBox.Text == "Other")
                            {
                                txtMat_Lining_Name_Back.Text = "";
                                txtMat_Lining_Name_Back.ReadOnly = false;
                                txtMat_Lining_Name_Back.BackColor = Color.White;
                            }
                            else
                            {
                                txtMat_Lining_Name_Back.ReadOnly = true;
                                //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                                txtMat_Lining_Name_Back.Text = mBearing.EndPlate[1].Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
                                txtMat_Lining_Name_Back.BackColor = Color.LightGray;
                            }
                            mBearing.EndPlate[1].Mat.Lining = txtMat_Lining_Name_Back.Text;
                        }
                    }
                    break;
            }

        }

        #endregion


        #region "TEXTBOX RELATED ROUTINE"
        //-------------------------------

        private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
        //================================================================
        {
            TextBox pTxtBox = (TextBox)sender;

            switch (pTxtBox.Name)
            {
                case "txtDBore_Range_Min_Front":
                    mblnDBore_Min_Front_Changed = true;
                    break;

                case "txtDBore_Range_Max_Front":
                    mblnDBore_Max_Front_Changed = true;
                    break;

                case "txtDBore_Range_Min_Back":
                    mblnDBore_Min_Back_Changed = true;
                    break;

                case "txtDBore_Range_Max_Back":
                    mblnDBore_Max_Back_Changed = true;
                    break;

            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        //==========================================================
        {
            TextBox pTxtBox = (TextBox)sender;

            switch (pTxtBox.Name)
            {
                case "txtDBore_Range_Min_Front":
                    //=========================
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[0].DBore_Range[0] =modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                         mBearing.EndPlate[0].DBore_Range[0] = modMain.ConvTextToDouble(pTxtBox.Text);                    
                    }
                    txtDBore_Range_Max_Front.ForeColor = Color.Black;

                    //if (mblnDBore_Min_Front_Changed)
                    //{
                    //    if (((clsSeal)modMain.gProject.Product.EndPlate[0]).DBore_Range[1] < modMain.gcEPS)
                    //    {
                    //        if (mEndSeal[0].DBore_Range[0] > modMain.gcEPS)
                    //        {
                    //            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //            {
                    //                txtDBore_Range_Max_Front.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mEndSeal[0].Calc_DBore_Limit(0)), "#0.0000");
                    //            }
                    //            else
                    //            {
                    //                txtDBore_Range_Max_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].Calc_DBore_Limit(0), "#0.0000");
                    //            }
                    //            txtDBore_Range_Max_Front.ForeColor = Color.Blue;
                    //        }
                    //        else
                    //        {
                    //            txtDBore_Range_Max_Front.Text = "";

                    //        }
                    //        mblnDBore_Min_Front_Changed = false;
                    //    }
                    //}
                    break;

                case "txtDBore_Range_Max_Front":
                    //=========================
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[0].DBore_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mBearing.EndPlate[0].DBore_Range[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                    }

                    if (mblnDBore_Max_Front_Changed)
                    {
                        txtDBore_Range_Max_Front.ForeColor = Color.Black;

                    }
                    break;

                case "txtL_Front":
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[0].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mBearing.EndPlate[0].L = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    break;

                case "txtL_Back":
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[1].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mBearing.EndPlate[1].L = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    break;

                case "txtBlade_T_Front":
                    //==================  
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[0].Seal.Blade.T = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        SetTxtForeColor(pTxtBox, modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[0].Seal.Blade.DESIGN_BLADE_THICK));
                    }
                    else
                    {
                        mBearing.EndPlate[0].Seal.Blade.T = modMain.ConvTextToDouble(pTxtBox.Text);
                        SetTxtForeColor(pTxtBox, mBearing.EndPlate[0].Seal.Blade.DESIGN_BLADE_THICK);
                    }
                    break;

                case "txtMat_LiningT_Front":
                    //====================== 
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[0].Mat_LiningT =modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mBearing.EndPlate[0].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                    }

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        if (mBearing.EndPlate[0].Mat_LiningT > modMain.gcEPS)
                        {
                            lblMat_LiningT_Front_Metric.Visible = true;
                            lblMat_LiningT_Front_Metric.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing.EndPlate[0].Mat_LiningT) + "]";
                        }
                        else
                        {
                            lblMat_LiningT_Front_Metric.Visible = false;
                        }
                    }

                   
                    break;

                case "txtDBore_Range_Min_Back":
                    //========================
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[1].DBore_Range[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mBearing.EndPlate[1].DBore_Range[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    txtDBore_Range_Max_Back.ForeColor = Color.Black;

                    //if (mblnDBore_Min_Back_Changed)
                    //{
                    //    if (((clsSeal)modMain.gProject.Product.EndPlate[1]).DBore_Range[1] < modMain.gcEPS)
                    //    {
                    //        if (mEndSeal[1].DBore_Range[0] > modMain.gcEPS)
                    //        {
                    //            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //            {
                    //                txtDBore_Range_Max_Back.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mEndSeal[1].Calc_DBore_Limit(0)), "#0.0000");
                    //            }
                    //            else
                    //            {
                    //                txtDBore_Range_Max_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].Calc_DBore_Limit(0), "#0.0000");
                    //            }

                    //            txtDBore_Range_Max_Back.ForeColor = Color.Blue;
                    //        }
                    //        else
                    //        {
                    //            txtDBore_Range_Max_Back.Text = "";
                    //        }
                    //        mblnDBore_Min_Front_Changed = false;
                    //    }
                    //}
                    break;

                case "txtDBore_Range_Max_Back":
                    //=========================
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[1].DBore_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mBearing.EndPlate[1].DBore_Range[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                    }

                    if (mblnDBore_Max_Back_Changed)
                    {
                        txtDBore_Range_Max_Back.ForeColor = Color.Black;
                    }
                    break;

                case "txtBlade_T_Back":
                    //==================
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[1].Seal.Blade.T = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        SetTxtForeColor(pTxtBox, modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[1].Seal.Blade.DESIGN_BLADE_THICK));
                    }
                    else
                    {
                        mBearing.EndPlate[1].Seal.Blade.T = modMain.ConvTextToDouble(pTxtBox.Text);
                        SetTxtForeColor(pTxtBox, mBearing.EndPlate[1].Seal.Blade.DESIGN_BLADE_THICK);
                    }
                    break;

                case "txtMat_LiningT_Back":
                    //======================
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[1].Mat_LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mBearing.EndPlate[1].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        if (mBearing.EndPlate[1].Mat_LiningT > modMain.gcEPS)
                        {
                            lblMat_LiningT_Back_Metric.Visible = true;
                            lblMat_LiningT_Back_Metric.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing.EndPlate[1].Mat_LiningT) + "]";
                        }
                        else
                        {
                            lblMat_LiningT_Back_Metric.Visible = false;
                        }
                    }
                                       
                    break;

            }
        }

        

        //BG 01APR13  As per HK's instruction in email dated 27MAR13.
        private void TextBox_Validating(object sender, CancelEventArgs e)
        //================================================================
        {
            TextBox pTxtBox = (TextBox)sender;

            switch (pTxtBox.Name)
            {
                case "txtBlade_T_Front":
                    //--------------------
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[0].Seal.Blade.T = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));                        
                    }
                    else
                    {
                        mBearing.EndPlate[0].Seal.Blade.T = modMain.ConvTextToDouble(pTxtBox.Text);
                       
                    }
                    if (mBearing.EndPlate[0].Seal.Blade.T < modMain.gcEPS)
                    {
                        SetTxtForeColorAndDefVal(mBearing.EndPlate[0].Seal.Blade.T, txtBlade_T_Front, mBearing.EndPlate[0].Seal.Blade.DESIGN_BLADE_THICK);
                        e.Cancel = true;
                    }
                    break;

                case "txtBlade_T_Back":
                    //-----------------
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mBearing.EndPlate[1].Seal.Blade.T = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mBearing.EndPlate[1].Seal.Blade.T = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    if (mBearing.EndPlate[1].Seal.Blade.T < modMain.gcEPS)
                    {
                        SetTxtForeColorAndDefVal(mBearing.EndPlate[1].Seal.Blade.T, txtBlade_T_Back, mBearing.EndPlate[1].Seal.Blade.DESIGN_BLADE_THICK);
                        e.Cancel = true;
                    }
                    break;

                //case "txtMat_LiningT_Front":
                //    //----------------------
                //    //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    //{
                //    //    mEndSeal[0].Mat_LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //    //}
                //    //else
                //    //{
                //    //    mEndSeal[0].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                //    //}

                //    mBearing.EndPlate[0].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);

                //    ////if (mEndSeal[0].Mat_LiningT < modMain.gcEPS)
                //    ////{
                //    ////    SetTxtForeColorAndDefVal(mEndSeal[0].Mat_LiningT, txtMat_LiningT_Front, mEndSeal[0].DESIGN_LINING_THICK);
                //    ////    e.Cancel = true;
                //    ////}
                //    break;

                //case "txtMat_LiningT_Back":
                //    //----------------------
                //    //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    //{
                //    //    mEndSeal[1].Mat_LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //    //}
                //    //else
                //    //{
                //    //    mEndSeal[1].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                //    //}

                //    mBearing.EndPlate[1].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);

                //    ////if (mEndSeal[1].Mat_LiningT < modMain.gcEPS)
                //    ////{
                //    ////    SetTxtForeColorAndDefVal(mEndSeal[1].Mat_LiningT, txtMat_LiningT_Back, mEndSeal[1].DESIGN_LINING_THICK);
                //    ////}
                //    break;
            }
        }

        #endregion


        #region "COMMAND BUTTON RELATED ROUTINE"
        //------------------------------------------------

        


        private void cmdOK_Click(object sender, EventArgs e)
        //==================================================
        {
             Boolean pIsInputValid = ValidateInput();
             if (pIsInputValid)
             {
                 CloseForm();
             }
        }

        private Boolean ValidateInput()
        //=============================
        {
            Boolean pFlag = true;

            if (Math.Round(mBearing.L_Tot(), 4) > Math.Round(mBearing.L_Available,4))
            {
                mIsDataValidated = true;
                pFlag = false;
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    MessageBox.Show("Assy. Total Length (" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot())) + ") cann't be greater than \nAvailable Env. Length ("
                                    + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Available)) + ")", "Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Assy. Total Length (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot()) + ") cann't be greater than \nAvailable Env. Length ("
                                    + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Available) + ")", "Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (mBearing.EndPlate[1].L > mBearing.EndPlate[0].L)
                {
                    tbEndSealData.SelectedIndex = 1;
                    txtL_Back.Focus();
                }
                else
                {
                    tbEndSealData.SelectedIndex = 0;
                    txtL_Front.Focus();
                }
            }

            return pFlag;
        }



        private void CloseForm()
        //======================
        {
            //string pMsg = "Bore Dia entry is not valid." + System.Environment.NewLine +
            //                    "Seal Clearance should be greater than Bearing Clearance: " +
            //                    modMain.ConvDoubleToStr(modMain.gRadialBearing.Clearance(), "#0.000");
            ////string pMsg = "Bore Dia entry is not valid." + System.Environment.NewLine +
            ////                    "Seal Clearance should be greater than Bearing Clearance: " +
            ////                    modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Clearance(), "#0.000");
            ////string pCaption = "Error in record entry";

            //////if (tbEndSealData.SelectedTab.Text == "Front")

            //////if(modMain.gRadialBearing.EndConfig_Front == clsBearing_Radial_FP.eEndConfig.Seal)
            ////if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
            ////{
            ////    if (ValidateSealData(mEndSeal[0]))
            ////    {

            ////        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

            ////        txtDBore_Range_Min_Front.Text = "";
            ////        txtDBore_Range_Max_Front.Text = "";

            ////        txtDBore_Range_Min_Front.Focus();
            ////        return;
            ////    }
            ////}
            //else if(tbEndSealData.SelectedTab.Text == "Back")

            //if (modMain.gRadialBearing.EndConfig_Back == clsBearing_Radial_FP.eEndConfig.Seal)
            ////if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            ////{
            ////    if (ValidateSealData(mEndSeal[1]))
            ////    {

            ////        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

            ////        txtDBore_Range_Min_Back.Text = "";
            ////        txtDBore_Range_Max_Back.Text = "";

            ////        txtDBore_Range_Min_Back.Focus();
            ////        return;
            ////    }
            ////}

            SaveData();

            if (!mblnFornt_Back_Copied)
            {
                mBearing.EndPlate[1] = (clsEndPlate)((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Clone();
            }
            SaveData();

            //Cursor = Cursors.WaitCursor;

            //modMain.gDB.UpdateRecord(modMain.gProject, modMain.gOpCond);
            //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
            //{
            //    //modMain.gEndSeal[0].UpdateRec_Seal(modMain.gProject.No, modMain.gDB, "Seal", "Front"); 
            //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).UpdateRec_Seal(modMain.gProject.No, modMain.gDB, "Seal", "Front");
            //}

            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            //{
            //    //modMain.gEndSeal[1].UpdateRec_Seal(modMain.gProject.No, modMain.gDB, "Seal", "Back");
            //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).UpdateRec_Seal(modMain.gProject.No, modMain.gDB, "Seal", "Front");
            //}

            //Cursor = Cursors.Default;

           

            if (mIsFront_BladeCount_2 || mIsBack_BladeCount_2)
            {
                modMain.gblnSealDesignDetails = true;
            }
            else
            {
                modMain.gblnSealDesignDetails = false;
            }

            modMain.gfrmMain.UpdateDisplay(modMain.gfrmMain);

            this.Hide();

            if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].TLTB.Exists == true ||
                 ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].TLTB.Exists == true)
            {
                modMain.gfrmThrustBearing.ShowDialog();
            }
            else
            {
                ////modMain.gfrmPerformDataBearing.ShowDialog();
                modMain.gfrmBearingDesignDetails.ShowDialog();
            }
        }


        private void cmdCancel_Click(object sender, EventArgs e)
        //=======================================================
        {
            //if (modMain.gRadialBearing.EndConfig_Front == clsBearing_Radial_FP.eEndConfig.Seal)
            //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
            //{
            //    //if (modMain.gEndSeal[0].Compare(mEndSeal[0], "Seal"))
            //    if (((clsSeal)modMain.gProject.Product.EndConfig[0]).Compare(mEndSeal[0], "Seal"))
            //    {
            //        SetMessage_SaveData();
            //    }
            //    else
            //    {
            //        this.Hide();
            //    }
            //}

            ////if (modMain.gRadialBearing.EndConfig_Back == clsBearing_Radial_FP.eEndConfig.Seal)
            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            //{
            //    //if (modMain.gEndSeal[1].Compare(mEndSeal[1], "Seal"))
            //    if (((clsSeal)modMain.gProject.Product.EndConfig[1]).Compare(mEndSeal[1], "Seal"))
            //    {
            //        SetMessage_SaveData();
            //    }
            //    else
            //    {
            //        this.Hide();
            //    }
            //}

            SaveData();
            this.Hide();
        }


        private void SetMessage_SaveData()
        //================================
        {
            int pAns = (int)MessageBox.Show("Data has been modified in this form." +
                           System.Environment.NewLine + "Do you want to save before exit?"
                           , "Save Record", MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question);

            const int pAnsY = 6;    //....Integer value of MessageBoxButtons.Yes.

            if (pAns == pAnsY)
            {
                CloseForm();
            }
            else
            {
                this.Hide();
            }
        }


        private void SaveData()
        //=====================
        {
            //....Front
            //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
            //{

                if (cmbType_Front.Text != "")
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.Design = (clsEndPlate.clsSeal.eDesign)
                      Enum.Parse(typeof(clsEndPlate.clsSeal.eDesign), cmbType_Front.Text);
                }

                Double[] pDBore_Range_Front = new Double[2];
                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pDBore_Range_Front[i] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mtxtDBore[i].Text));
                    }
                    else
                    {
                        pDBore_Range_Front[i] = modMain.ConvTextToDouble(mtxtDBore[i].Text);
                    }
                }

                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].DBore_Range = pDBore_Range_Front;

                //....Length
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Front.Text));
                }
                else
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].L = modMain.ConvTextToDouble(txtL_Front.Text);
                }

                //modMain.gProject.Product.Bearing = (clsBearing_Radial_FP)mBearing_Radial_FP.Clone();

                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.DrainHoles.UpdateCurrentSeal((clsJBearing)modMain.gProject.PNR.Bearing);

                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.Blade.Count = modMain.ConvTextToInt(cmbBlade_Count_Front.Text);
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.Blade.T = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtBlade_T_Front.Text));
                }
                else
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.Blade.T = modMain.ConvTextToDouble(txtBlade_T_Front.Text);
                }

                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.Blade.AngTaper = modMain.ConvTextToDouble(cmbBlade_AngTaper_Front.Text);

                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat.WCode_Base = cmbMat_Base_WCode_Front.Text;
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat.WCode_Lining = cmbMat_Lining_WCode_Front.Text;
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat.Base = txtMat_Base_Name_Front.Text;
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat.Lining = txtMat_Lining_Name_Front.Text;
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat.LiningExists = chkMat_LiningExists_Front.Checked;

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat_LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMat_LiningT_Front.Text));
                }
                else
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Front.Text);
                }
               

                //((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Front.Text); //BG 01APR13

                //BG 01APR13
                //if (((clsSeal)modMain.gProject.Product.EndConfig[0]).Design != clsSeal.eDesign.Fixed)
                //{
                    //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //{
                    //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat_LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMat_LiningT_Front.Text));
                    //}
                    //else
                    //{
                    //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Front.Text);
                    //}
                //}
                //else
                //{
                //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat_LiningT = 0.0F;
                //}

            //}


            //....Back 
            //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
            //{
                if (cmbType_Front.Text != "")
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.Design = (clsEndPlate.clsSeal.eDesign)
                       Enum.Parse(typeof(clsEndPlate.clsSeal.eDesign), cmbType_Back.Text);
                }

                Double[] pDBore_Range_Back = new Double[2];

                for (int i = 0, j = 2; j < 4; i++, j++)
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pDBore_Range_Back[i] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mtxtDBore[j].Text));
                    }
                    else
                    {
                        pDBore_Range_Back[i] = modMain.ConvTextToDouble(mtxtDBore[j].Text);
                    }
                }
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].DBore_Range = pDBore_Range_Back;

                //....Length
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Back.Text));
                }
                else
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].L = modMain.ConvTextToDouble(txtL_Back.Text);
                }

                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.Blade.Count = modMain.ConvTextToInt(cmbBlade_Count_Back.Text);

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.Blade.T = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtBlade_T_Back.Text));
                }
                else
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.Blade.T = modMain.ConvTextToDouble(txtBlade_T_Back.Text);
                }
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.Blade.AngTaper = modMain.ConvTextToDouble(cmbBlade_AngTaper_Back.Text);

                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat.WCode_Base = cmbMat_Base_WCode_Back.Text;
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat.Base = txtMat_Base_Name_Back.Text;
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat.Lining = txtMat_Lining_Name_Back.Text;
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat.WCode_Lining = cmbMat_Lining_WCode_Back.Text;
                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat.LiningExists = chkMat_LiningExists_Back.Checked;

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat_LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMat_LiningT_Back.Text));
                }
                else
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Back.Text);
                }
            //((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Back.Text);

                //((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Back.Text);    //BG 01APR13

                //BG 01APR13
                //if (((clsSeal)modMain.gProject.Product.EndConfig[1]).Design != clsSeal.eDesign.Fixed)
                //{
                    //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //{
                    //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat_LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMat_LiningT_Back.Text));
                    //}
                    //else
                    //{
                    //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Back.Text);
                    //}
                //}
                //else
                //{
                //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat_LiningT = 0.0F;
                //}

            //}
        }


        //private bool ValidateSealData(clsSeal Seal_In)
        ////=============================================
        //{
        //    bool pbln = false;

        //    if (Seal_In.Clearance() < ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Clearance())
        //    {
        //        pbln = true;
        //    }

        //    return pbln;
        //}

        #endregion

        private void chkMat_LiningExists_Front_CheckedChanged(object sender, EventArgs e)
        //=================================================================================
        {
            Set_LiningMat_Design_Front();

            mBearing.EndPlate[0].Mat.LiningExists = chkMat_LiningExists_Front.Checked;


            if (!chkMat_LiningExists_Front.Checked)
            {
                mBearing.EndPlate[0].Mat.Lining = "None";
                mBearing.EndPlate[0].Mat_LiningT = 0.0F;
                txtMat_LiningT_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(0.0);
            }
            else
            {
                ////cmbMat_Lining.Text = "Babbitt";
                cmbMat_Lining_WCode_Front.SelectedIndex = 0;
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    txtMat_LiningT_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[0].Mat_LiningT));
                }
                else
                {
                    txtMat_LiningT_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[0].Mat_LiningT);
                }
            }

            DisplayLblMetric(txtMat_LiningT_Front,  lblMat_LiningT_Front_Metric, chkMat_LiningExists_Front.Checked);
        }

       

        private void chkMat_LiningExists_Back_CheckedChanged(object sender, EventArgs e)
        //================================================================================
        {
            Set_LiningMat_Design_Back();

            mBearing.EndPlate[1].Mat.LiningExists = chkMat_LiningExists_Back.Checked;


            if (!chkMat_LiningExists_Back.Checked)
            {
                mBearing.EndPlate[1].Mat.Lining = "None";
                mBearing.EndPlate[1].Mat_LiningT = 0.0F;
                txtMat_LiningT_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(0.0);
            }
            else
            {
                ////cmbMat_Lining.Text = "Babbitt";
                cmbMat_Lining_WCode_Back.SelectedIndex = 0;
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    txtMat_LiningT_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[1].Mat_LiningT));
                }
                else
                {
                    txtMat_LiningT_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[1].Mat_LiningT);
                }
               // txtMat_LiningT_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[1].Mat_LiningT);
            }

            DisplayLblMetric(txtMat_LiningT_Back, lblMat_LiningT_Back_Metric, chkMat_LiningExists_Back.Checked);
        }

        private void Set_LiningMat_Design_Front()
        //=======================================   
        {
            cmbMat_Lining_WCode_Front.Visible = chkMat_LiningExists_Front.Checked;
            txtMat_Lining_Name_Front.Visible = chkMat_LiningExists_Front.Checked;
            lblMat_LiningT_Front.Visible = chkMat_LiningExists_Front.Checked;
            txtMat_LiningT_Front.Visible = chkMat_LiningExists_Front.Checked;
        }

        private void Set_LiningMat_Design_Back()
        //=======================================   
        {
            cmbMat_Lining_WCode_Back.Visible = chkMat_LiningExists_Back.Checked;
            txtMat_Lining_Name_Back.Visible = chkMat_LiningExists_Back.Checked;
            lblMat_LiningT_Back.Visible = chkMat_LiningExists_Back.Checked;
            txtMat_LiningT_Back.Visible = chkMat_LiningExists_Back.Checked;
        }

        #endregion       

        private void tbEndSealData_SelectedIndexChanged(object sender, EventArgs e)
        //==========================================================================
        {
            DisplayData();
            SaveData();

            if (!mIsDataValidated)
            {
                if (tbEndSealData.SelectedIndex == 1)
                {
                    int pAns = (int)MessageBox.Show("Do you want Front Seal Data to be copied on to Back Seal Data?", "Seal Data Copying",
                                               MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    const int pAnsY = 6;    //....Integer value of MessageBoxButtons.Yes.

                    if (pAns == pAnsY)
                    {
                        if (tbEndSealData.SelectedIndex == 1)
                        {
                            mBearing.EndPlate[1] = (clsEndPlate)((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Clone();
                        }
                        mblnFornt_Back_Copied = true;

                        DisplayData();
                    }
                }
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        //=====================================================
        {
            Boolean pIsInputValid = ValidateInput();
            if (pIsInputValid)
            {
                SaveData();

                if (!mblnFornt_Back_Copied)
                {
                    mBearing.EndPlate[1] = (clsEndPlate)((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Clone();
                }
                SaveData();

                if (mIsFront_BladeCount_2 || mIsBack_BladeCount_2)
                {
                    modMain.gblnSealDesignDetails = true;
                }
                else
                {
                    modMain.gblnSealDesignDetails = false;
                }

                modMain.gfrmMain.UpdateDisplay(modMain.gfrmMain);

                this.Hide();
            }
        }

    }
}
