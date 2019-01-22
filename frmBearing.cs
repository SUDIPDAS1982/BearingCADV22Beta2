
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmBearing                             '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  18DEC18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
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
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Collections.Specialized;

namespace BearingCAD22
{
   public partial class frmBearing : Form
   {
       #region "MEMBER VARIABLE DECLARATION:"
       //************************************

            private TextBox[] mtxtOD_Range;           
            private TextBox[] mtxtBore_Range;         
            private TextBox[] mtxtDShaft_Range;       
            private TextBox[] mtxtPadBore_Range;
            private TextBox[] mtxtPivot_Loc;
            //public TextBox[] mtxtEDM_Relief;          //....Moved to BearingDesignDetails form.

            private Label[] mlblMetric;
            

            //....Local Objects:
            private clsJBearing mBearing ;           

            private Boolean mblnDepth_EndPlate_F_ManuallyChanged = false;
            private Boolean mblnDepth_EndPlate_B_ManuallyChanged = false;
            private Boolean mblnDShaft_ManuallyChanged = false;     
            private Boolean mblnDSet_ManuallyChanged   = false;
            private Boolean mblnPad_T_Pivot_ManuallyChanged = false;
            private Boolean mblnWeb_T_ManuallyChanged  = false;


            private const double mcDepth_EPCBore_Min_English = 0.2;
            private const double mcDepth_EPCBore_Min_Metric = 5.0;


       #endregion


       #region "FORM CONSTRUCTOR & RELATED ROUTINES:"
       //********************************************

            public frmBearing()
            //=================
            {
                InitializeComponent();               
           
                //....Initialize TextBoxes.
                mtxtOD_Range   = new TextBox[] { txtBearingOD_Range_Min, txtBearingOD_Range_Max };
                mtxtBore_Range   = new TextBox[] { txtBearingBore_Range_Min, txtBearingBore_Range_Max };
                mtxtDShaft_Range = new TextBox[] { txtDShaft_Range_Min, txtDShaft_Range_Max };
                mtxtPadBore_Range   = new TextBox[] { txtPadBore_Range_Min, txtPadBore_Range_Max };
                mtxtPivot_Loc    = new TextBox[] { txtPad_Pivot_AngStart, txtPad_Pivot_AngOther2, 
                                                   txtPad_Pivot_AngOther3, txtPad_Pivot_AngOther4, 
                                                   txtPad_Pivot_AngOther5, txtPad_Pivot_AngOther6 };

                mlblMetric = new Label[] { lblPad_RFillet_ID, lblFlexPivot_Web_RFillet, 
                                           lblFlexPivot_GapEDM, lblLiningT, lblFlexPivot_GapEDM_Unit };

                //....Populate Split Configuration.
                cmbSplitConfig.Items.Clear();
                cmbSplitConfig.Items.Add("Y");
                cmbSplitConfig.Items.Add("N");
                cmbSplitConfig.SelectedIndex = 0;

                //....Populate Pad Type.
                //LoadPadType();

                ////....Populate EDM Gap        
                //LoadEDMGap();
                
                //....Populate Base & Lining  Material.
                ////LoadMat();
                Load_WaukeshaCode();
            }


            //private void LoadPadType()
            ////========================      
            //{
            //    cmbLoadOrient.DataSource = Enum.GetValues(typeof(clsRadB.clsPad.eLoadOrient));
            //    cmbLoadOrient.SelectedIndex = 0;  
            //}


            private void LoadEDMGap()
            //======================
            {

                //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                //StringCollection pEDMGap = new StringCollection();

                //var pQry = (from pRec in pBearingDBEntities.tblManf_EDM orderby pRec.fldGap ascending select pRec).Distinct().ToList();

                //if (pQry.Count() > 0)
                //{
                //    for (int i = 0; i < pQry.Count; i++)
                //    {
                //        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //        //{
                //        //    double pVal = (double)pQry[i].fldGap;
                //        //    pEDMGap.Add(modMain.gProject.PNR.Unit.CEng_Met(pVal).ToString());
                //        //}
                //        //else
                //        //{
                //        //    pEDMGap.Add(pQry[i].fldGap.ToString());
                //        //}

                //        pEDMGap.Add(pQry[i].fldGap.ToString());

                //    }
                //}

                //cmbFlexPivot_GapEDM.Items.Clear();

                //for (int i = 0; i < pEDMGap.Count; i++)
                //{
                //    Double pVal = Convert.ToDouble(pEDMGap[i]);
                //    cmbFlexPivot_GapEDM.Items.Add(modMain.gProject.PNR.Unit.WriteInUserL_Eng(pVal));
                //}

                //if (cmbFlexPivot_GapEDM.Items.Count > 0)
                //    cmbFlexPivot_GapEDM.SelectedIndex = 0;

                cmbFlexPivot_GapEDM.Items.Clear();
                cmbFlexPivot_GapEDM.Items.Add("0.0140");
                cmbFlexPivot_GapEDM.Items.Add("0.0160");

                if (((clsPivot.clsFP)mBearing.RadB.Pivot).GapEDM == 0)
                {
                    cmbFlexPivot_GapEDM.SelectedIndex = 0;
                }
                
            }


            ////private void LoadMat()
            //////=====================    
            ////{
            ////    BearingDBEntities pBearingDBEntities = new BearingDBEntities();

            ////    //....Base Material.
            ////    var pQryBaseMat = (from pRec in pBearingDBEntities.tblData_Mat
            ////                       where
            ////                           pRec.fldBase == true
            ////                       orderby pRec.fldName ascending
            ////                       select pRec).ToList();
            ////    cmbMat_Base.Items.Clear();
            ////    if (pQryBaseMat.Count() > 0)
            ////    {
            ////        for (int i = 0; i < pQryBaseMat.Count; i++)
            ////        {
            ////            cmbMat_Base.Items.Add(pQryBaseMat[i].fldName);
            ////        }
            ////        cmbMat_Base.SelectedIndex = 3;
            ////    }

            ////    //....Lining Material.
            ////    var pQryLiningMat = (from pRec in pBearingDBEntities.tblData_Mat
            ////                         where
            ////                             pRec.fldLining == true
            ////                         orderby pRec.fldName ascending
            ////                         select pRec).ToList();
            ////    cmbMat_Lining.Items.Clear();
            ////    if (pQryBaseMat.Count() > 0)
            ////    {
            ////        for (int i = 0; i < pQryLiningMat.Count; i++)
            ////        {
            ////            cmbMat_Lining.Items.Add(pQryLiningMat[i].fldName);
            ////        }
            ////        cmbMat_Lining.SelectedIndex = 0;
            ////    }
            ////}

            private void Load_WaukeshaCode()
            //==============================    
            {
                ////BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                //....Base Material.
                ////var pQryBaseMat = (from pRec in pBearingDBEntities.tblData_Mat
                ////                   where pRec.fldLining == false && pRec.fldCode_Waukesha != null && pRec.fldBearing == true
                ////                   orderby pRec.fldCode_Waukesha ascending
                ////                   select pRec).ToList();
                ////cmbMat_Base_WCode.Items.Clear();
                ////if (pQryBaseMat.Count() > 0)
                ////{
                ////    for (int i = 0; i < pQryBaseMat.Count; i++)
                ////    {
                ////        cmbMat_Base_WCode.Items.Add(pQryBaseMat[i].fldCode_Waukesha);
                ////    }
                ////    cmbMat_Base_WCode.Items.Add("Other");
                ////    cmbMat_Base_WCode.SelectedIndex = 0;
                ////}

                //////....Lining Material.
                ////var pQryLiningMat = (from pRec in pBearingDBEntities.tblData_Mat
                ////                     where pRec.fldLining == true && pRec.fldCode_Waukesha != null
                ////                     orderby pRec.fldCode_Waukesha ascending
                ////                     select pRec).ToList();
                ////cmbMat_Lining_WCode.Items.Clear();
                ////if (pQryLiningMat.Count() > 0)
                ////{
                ////    for (int i = 0; i < pQryLiningMat.Count; i++)
                ////    {
                ////        cmbMat_Lining_WCode.Items.Add(pQryLiningMat[i].fldCode_Waukesha);
                ////    }
                ////    cmbMat_Lining_WCode.Items.Add("Other");
                ////    cmbMat_Lining_WCode.SelectedIndex = 0;
                ////}

                //....Base Material.
                //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                string pWHERE = " WHERE Lining = false and Code_Waukesha is not null and Bearing = true";
                int pMat_Base_WCode_RecCount = modMain.gDB.PopulateCmbBox(cmbMat_Base_WCode, modMain.gFiles.FileTitle_EXCEL_MatData, "[Mat$]", "Code_Waukesha", pWHERE, true);

                if (pMat_Base_WCode_RecCount > 0)
                {
                    cmbMat_Base_WCode.Items.Add("Other");
                    cmbMat_Base_WCode.SelectedIndex = 0;
                }

                //....Lining Material.               
                pWHERE = " WHERE Lining = true and Code_Waukesha is not null";
                int pMat_Lining_WCode_RecCount = modMain.gDB.PopulateCmbBox(cmbMat_Lining_WCode, modMain.gFiles.FileTitle_EXCEL_MatData, "[Mat$]", "Code_Waukesha", pWHERE, true);

                if (pMat_Lining_WCode_RecCount > 0)
                {
                    cmbMat_Lining_WCode.Items.Add("Other");
                    cmbMat_Lining_WCode.SelectedIndex = 0;
                }

            }

       #endregion


       #region "FORM EVENT ROUTINES:"txtPad_Count
            //***************************

            private void frmBearing_Load(object sender, EventArgs e)
            //======================================================
            {
                mblnDepth_EndPlate_F_ManuallyChanged = false;
                mblnDepth_EndPlate_B_ManuallyChanged = false;
               
                //....Set Locl Object.
                SetLocalObject();

                //....Initialize control.
                InitializeControls();

                //....Set Locl Object.
                SetLocalObject();

                //....Populate EDM Gap        
                LoadEDMGap();

                //....Display data.
                DisplayData();              

                ////....Set Control for diff privilege & Project status.
                //SetControls();                                    
            }


            private void InitializeControls()
            //===============================
            {
                const string pcBlank = "";

                //....DShaft,DFit,DSet,DPad
                for (int i = 0; i < 2; i++)
                {
                    mtxtDShaft_Range[i].Text = pcBlank;
                    mtxtOD_Range[i].Text = pcBlank;
                    mtxtBore_Range[i].Text = pcBlank;
                    mtxtPadBore_Range[i].Text = pcBlank;
                }

                //....Clearence & PreLoad
                //txtClearance.Text = pcBlank;
                //txtPreLoad.Text = pcBlank;

                //  Pad
                //  ===
                    txtPad_L.Text = pcBlank;
                    txtPad_Pivot_Offset.Text = pcBlank;
            
                    for (int i = 0; i < 6; i++)
                    {
                        mtxtPivot_Loc[i].Text = pcBlank;
                    }

                    txtPad_T_Lead.Text = pcBlank;
                    txtPad_T_Pivot.Text = pcBlank;
                    txtPad_T_Trail.Text = pcBlank;
                    //txtPad_RFillet_ID.Text = pcBlank;  

                //  Flexure Pivot
                //  =============
                    txtFlexPivot_Web_RFillet.Text = pcBlank;
                    txtFlexPivot_Web_T.Text = pcBlank;
                    txtFlexPivot_Web_RFillet.Text = pcBlank;
                    txtFlexPivot_Web_H.Text = pcBlank;
                    //txtFlexPivot_Rot_Stiff.Text = pcBlank;

                

                //  Material
                //  ========
                    txtLiningT.Text = pcBlank;

                //....Show Labels for Metric 
                for (int i = 0; i < mlblMetric.Length ; i++)
                {
                    //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric && mBearing_Radial_FP.Mat.LiningExists)
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mlblMetric[i].Visible = true;
                    }
                    else
                    {
                        mlblMetric[i].Visible = false;
                    }
                }

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
                {
                    cmbFlexPivot_GapEDM.Left = 318;
                    cmbFlexPivot_GapEDM.Top = 639;
                }
                else
                {
                    cmbFlexPivot_GapEDM.Left = 387;
                    cmbFlexPivot_GapEDM.Top = 639;
                }
                //cmbFlexPivot_GapEDM
            }


            private void SetLocalObject()
            //===========================
            {
                mBearing = (clsJBearing)(modMain.gProject.PNR.Bearing).Clone();
            }


            private void DisplayData()  
            //========================
            {
                //....Type Radial.                
                txtRadialType.Text = mBearing.RadB.Pivot.Design.ToString(); 

                //....Split Config.                
                if (mBearing.RadB.SplitConfig)
                {
                    cmbSplitConfig.SelectedIndex = -1;
                    cmbSplitConfig.SelectedIndex = 0;
                }
                else
                {
                    cmbSplitConfig.SelectedIndex = -1;
                    cmbSplitConfig.SelectedIndex = 0;
                }

                //  Bearing Length:
                //if (mBearing.RadB.L < modMain.gcEPS)
                //{
                //    mBearing.RadB.L = mBearing.RadB.Pad.L + mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1];
                //}

                double pENDPlateCB_Depth_Front = 0; 
                double pENDPlateCB_Depth_Back = 0; 
                if (mBearing.RadB.EndPlateCB[0].Depth > modMain.gcEPS)
                {
                    pENDPlateCB_Depth_Front = mBearing.RadB.EndPlateCB[0].Depth;
                }
                if (mBearing.RadB.EndPlateCB[1].Depth > modMain.gcEPS)
                {
                    pENDPlateCB_Depth_Back = mBearing.RadB.EndPlateCB[1].Depth;
                }

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)      // PB 21OCT18. Have English Unit first and then Metric to maintain consistency. 
                {
                    if (mBearing.RadB.L < modMain.gcEPS)
                    {
                        mBearing.RadB.L = mBearing.RadB.Pad.L + mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1] + 2*modMain.gProject.PNR.Unit.CMet_Eng(mcDepth_EPCBore_Min_Metric) ;
                    }
                    txtHousingL.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.L));
                }
                else
                {
                    if (mBearing.RadB.L < modMain.gcEPS)
                    {
                        mBearing.RadB.L = mBearing.RadB.Pad.L + mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1] + 2 * mcDepth_EPCBore_Min_English;
                    }
                    txtHousingL.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.L);
                }

                if (pENDPlateCB_Depth_Front > modMain.gcEPS)
                {
                    mBearing.RadB.EndPlateCB[0].Depth = pENDPlateCB_Depth_Front;
                }

                if (pENDPlateCB_Depth_Back > modMain.gcEPS)
                {
                    mBearing.RadB.EndPlateCB[1].Depth = pENDPlateCB_Depth_Back;
                }

                //  Depths:
                //  -------
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    if (mBearing.RadB.EndPlateCB[0].Depth != 0.0)
                    {
                        if (Math.Abs(mBearing.RadB.EndPlateCB[0].Depth - mBearing.RadB.EndPlateCB_Depth_Def()) > modMain.gcEPS)
                        {
                            if (mBearing.RadB.EndPlateCB[0].Depth > modMain.gcEPS)
                            {
                                txtDepth_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB[0].Depth));
                            }
                            else
                            {
                                txtDepth_EndConfig_Front.Text = "";
                            }
                        }
                        else
                        {
                            if (mBearing.RadB.EndPlateCB_Depth_Def() > modMain.gcEPS)
                            {
                                txtDepth_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB_Depth_Def()));
                            }
                            else
                            {
                                txtDepth_EndConfig_Front.Text = "";
                            }
                        }
                    }

                    if (mBearing.RadB.EndPlateCB[1].Depth != 0.0)
                    {
                        if (Math.Abs(mBearing.RadB.EndPlateCB[1].Depth - mBearing.RadB.EndPlateCB_Depth_Def()) > modMain.gcEPS)
                        {
                            if (mBearing.RadB.EndPlateCB[1].Depth > modMain.gcEPS)
                            {
                                txtDepth_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB[1].Depth));
                            }
                            else
                            {
                                txtDepth_EndConfig_Back.Text = "";
                            }
                        }
                        else
                        {
                            if (mBearing.RadB.EndPlateCB_Depth_Def() > modMain.gcEPS)
                            {
                                txtDepth_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB_Depth_Def()));
                            }
                            else
                            {
                                txtDepth_EndConfig_Back.Text = "";
                            }
                        }
                    }
                }
                else
                {
                    if (mBearing.RadB.EndPlateCB[0].Depth != 0.0)
                    {
                        if (Math.Abs(mBearing.RadB.EndPlateCB[0].Depth - mBearing.RadB.EndPlateCB_Depth_Def()) > modMain.gcEPS)
                        {
                            if (mBearing.RadB.EndPlateCB[0].Depth > modMain.gcEPS)
                            {
                                txtDepth_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.EndPlateCB[0].Depth);
                            }
                            else
                            {
                                txtDepth_EndConfig_Front.Text = "";
                            }
                        }
                        else
                        {
                            if (mBearing.RadB.EndPlateCB_Depth_Def() > modMain.gcEPS)
                            {
                                txtDepth_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.EndPlateCB_Depth_Def());
                            }
                            else
                            {
                                txtDepth_EndConfig_Front.Text = "";
                            }
                        }
                    }

                    if (mBearing.RadB.EndPlateCB[1].Depth != 0.0)
                    {
                        if (Math.Abs(mBearing.RadB.EndPlateCB[1].Depth - mBearing.RadB.EndPlateCB_Depth_Def()) > modMain.gcEPS)
                        {
                            if (mBearing.RadB.EndPlateCB[1].Depth > modMain.gcEPS)
                            {
                                txtDepth_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.EndPlateCB[1].Depth);
                            }
                            else
                            {
                                txtDepth_EndConfig_Back.Text = "";
                            }
                        }
                        else
                        {
                            if (mBearing.RadB.EndPlateCB_Depth_Def() > modMain.gcEPS)
                            {
                                txtDepth_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.EndPlateCB_Depth_Def());
                            }
                            else
                            {
                                txtDepth_EndConfig_Back.Text = "";
                            }
                        }
                    }
                }

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    if (mBearing.EndPlate[0].L > modMain.gcEPS)
                    {
                        txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[0].L));
                    }
                    else
                    {
                        txtLength_EndConfig_Front.Text = "";
                    }

                    if (mBearing.EndPlate[1].L > modMain.gcEPS)
                    {
                        txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.EndPlate[1].L));
                    }
                    else
                    {
                        txtLength_EndConfig_Back.Text = "";
                    }                   
                    
                }
                else
                {
                    if (mBearing.EndPlate[0].L > modMain.gcEPS)
                    {
                        txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[0].L);
                    }
                    else
                    {
                        txtLength_EndConfig_Front.Text = "";
                    }

                    if (mBearing.EndPlate[1].L > modMain.gcEPS)
                    {
                        txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[1].L);
                    }
                    else
                    {
                        txtLength_EndConfig_Back.Text = "";
                    }  

                    //txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[0].L);
                    //txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.EndPlate[1].L);
                }

                //....Set Length Unit.                      //....Not Used Now
                //string pUnit = "in"; 
                //lblLengthUnit.Text = pUnit; 

                //....DSet,DShaft,DFit & DPad.
                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mtxtDShaft_Range[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.DShaft_Range[i]));
                        mtxtOD_Range[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.OD_Range[i]));
                        mtxtBore_Range[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.Bore_Range[i]));
                        mtxtPadBore_Range[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.PadBore_Range[i]));
                    }
                    else
                    {
                        mtxtDShaft_Range[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.DShaft_Range[i]);
                        mtxtOD_Range[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.OD_Range[i]);
                        mtxtBore_Range[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.Bore_Range[i]);
                        mtxtPadBore_Range[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.PadBore_Range[i]);
                    }
                }

                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //{
                //    //.....PreLoad.
                //    //if (IsDiaNotNull())
                //    //    txtPreLoad.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()));
                //    if (IsDiaNotNull())
                //        txtPreLoad.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.PreLoad());


                //    //.....Clearence.                
                //    txtClearance.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Clearance()));
                //}
                //else
                //{
                //    //.....PreLoad.
                //    if (IsDiaNotNull())
                //        txtPreLoad.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.PreLoad());

                //    //.....Clearence.                
                //    txtClearance.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Clearance());
                //}
                    
                //....Lengths
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    if (mBearing.L_Available < modMain.gcEPS)
                    {
                        mBearing.L_Available = mBearing.RadB.Pad.L + mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1] + 2 * modMain.gProject.PNR.Unit.CMet_Eng(mcDepth_EPCBore_Min_Metric);
                    }
                    txtL_Available.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Available));

                    if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                    {
                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                    }
                    else
                    {
                        txtL_Tot.Text = "";
                    }
                }
                else
                {
                    if (mBearing.L_Available < modMain.gcEPS)
                    {
                        mBearing.L_Available = mBearing.RadB.Pad.L + mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1] + 2 * mcDepth_EPCBore_Min_English;
                    }
                    txtL_Available.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Available);
                    if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                    {
                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                    }
                    else
                    {
                        txtL_Tot.Text = "";
                    }
                }
                   
              
                //  Pad:
                //  ====                  
                txtLoadOrient.Text = mBearing.RadB.Pad.LoadOrient.ToString();

                    //...Count
                txtPad_Count.Text = modMain.ConvIntToStr(mBearing.RadB.Pad.Count);
                //updPad_Count.Value = mBearing.RadB.Pad.Count;
                
                    //...Length  
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtPad_L.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.Pad.L));
                    }
                    else
                    {
                        txtPad_L.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.Pad.L);
                    }

                    //....Angle.
                    txtPad_Ang.Text = modMain.ConvDoubleToStr(mBearing.RadB.Pad.Angle, "#0");

                    txtPad_Pivot_Offset.Text = modMain.ConvDoubleToStr(mBearing.RadB.Pad.Pivot.Offset, "#0.0");  
                                      
                    ////....Pivot Offset
                    //if (modMain.gOpCond.Rot_Directionality == clsOpCond.eRotDirectionality.Bi)
                    //{
                    //    txtPad_Pivot_Offset.Text = "50";                       
                    //    txtPad_Pivot_Offset.ReadOnly = true;
                    //    txtPad_Pivot_Offset.BackColor = txtRadialType.BackColor;  //txtClearance.BackColor;
                    //    txtPad_Pivot_Offset.ForeColor = Color.Black;   
                    //}

                    //else if (modMain.gOpCond.Rot_Directionality == clsOpCond.eRotDirectionality.Uni)
                    //{                       
                    //    txtPad_Pivot_Offset.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.Pivot.Offset, "#0.0");  
                    //    txtPad_Pivot_Offset.ReadOnly = false;
                    //    txtPad_Pivot_Offset.BackColor = Color.White;
                    //    txtPad_Pivot_Offset.ForeColor = Color.Black;
                    //}

                    //....Location.     
                    mtxtPivot_Loc[0].Text = modMain.ConvDoubleToStr(mBearing.RadB.Pad.Pivot.AngStart_Casing_SL, "#0");

                    //....Thick  
                    //if (Math.Abs(mBearing_Radial_FP.Pad.T.Lead -  mBearing_Radial_FP.Pad.T.Pivot) > modMain.gcEPS
                    //    || Math.Abs(mBearing_Radial_FP.Pad.T.Trail - mBearing_Radial_FP.Pad.T.Pivot) > modMain.gcEPS
                    //    || Math.Abs(mBearing_Radial_FP.Pad.T.Lead - mBearing_Radial_FP.Pad.T.Trail) > modMain.gcEPS)  
                    //{
                    //    chkThick_Pivot.Checked = false;
                    //}
                    //else
                    //{
                    //    chkThick_Pivot.Checked = true;
                    //}

                    //BG 26MAR13
                    if (!((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T.Pivot_Checked)
                    {
                        if (Math.Abs(mBearing.RadB.Pad.T.Lead - mBearing.RadB.Pad.T.Pivot) > modMain.gcEPS
                          || Math.Abs(mBearing.RadB.Pad.T.Trail - mBearing.RadB.Pad.T.Pivot) > modMain.gcEPS
                          || Math.Abs(mBearing.RadB.Pad.T.Lead - mBearing.RadB.Pad.T.Trail) > modMain.gcEPS)
                        {
                            chkThick_Pivot.Checked = false;
                        }
                        else
                        {
                            chkThick_Pivot.Checked = true;
                        }
                    }
                    else
                        chkThick_Pivot.Checked = ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T.Pivot_Checked;

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtPad_T_Lead.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.Pad.T.Lead));
                        txtPad_T_Pivot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.Pad.T.Pivot));
                        txtPad_T_Trail.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.Pad.T.Trail));

                        //....RFillet_ID
                        txtPad_RFillet_ID.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.Pad.RFillet));        

                        //....RFillet_ID
                        //lblPad_RFillet_ID_Unit.Visible = true;
                        //lblPad_RFillet_ID.Visible = true;
                        //lblPad_RFillet_ID.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Pad.RFillet)) + ")";
                        //txtPad_RFillet_ID.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Pad.RFillet);
                        ////txtPad_RFillet_ID.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Pad.RFillet));
                        
                    }
                    else
                    {
                        txtPad_T_Lead.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.Pad.T.Lead);
                        txtPad_T_Pivot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.Pad.T.Pivot);
                        txtPad_T_Trail.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.Pad.T.Trail);

                        //....RFillet_ID
                        txtPad_RFillet_ID.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.Pad.RFillet);

                        //....RFillet_ID
                        //lblPad_RFillet_ID_Unit.Visible = false;
                        //lblPad_RFillet_ID.Visible = false;
                        //txtPad_RFillet_ID.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Pad.RFillet);
                    }
                    //txtPad_RFillet_ID.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing.RadB.Pad.RFillet);        //AES 28SEP18

                    ////....EDM Relief       //BG 06DEC12
                    //for(int i = 0; i < 2; i++)
                    //{
                    //    mtxtEDM_Relief[i].Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.EDM_Relief[i], "#0.000");
                    //}
  
                //  Flexure Pivot
                //  =============

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtFlexPivot_Web_T.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.T));
                        //txtFlexPivot_Web_RFillet.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.FlexurePivot.Web.RFillet));
                        txtFlexPivot_Web_H.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.H));

                        txtFlexPivot_Web_RFillet.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.RFillet)); 
                    }
                    else
                    {
                        txtFlexPivot_Web_T.Text = modMain.gProject.PNR.Unit.WriteInUserL(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.T);
                        //txtFlexPivot_Web_RFillet.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.FlexurePivot.Web.RFillet);
                        txtFlexPivot_Web_H.Text = modMain.gProject.PNR.Unit.WriteInUserL(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.H);

                        txtFlexPivot_Web_RFillet.Text = modMain.gProject.PNR.Unit.WriteInUserL(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.RFillet);
                    }
                   // txtFlexPivot_Web_RFillet.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.RFillet);        //AES 28SEP18


                    string pEDMGap =modMain.gProject.PNR.Unit.WriteInUserL_Eng( ((clsPivot.clsFP)mBearing.RadB.Pivot).GapEDM);

                    Boolean pValExists = false;

                    for (int i = 0; i < cmbFlexPivot_GapEDM.Items.Count; i++)
                    {
                        if (cmbFlexPivot_GapEDM.Items[i].ToString() == pEDMGap)
                        {
                            pValExists = true;
                            break;
                        }
                    }
                    if (!pValExists)
                    {
                        cmbFlexPivot_GapEDM.Items.Add(pEDMGap);
                    }

                    if (((clsPivot.clsFP)mBearing.RadB.Pivot).GapEDM != 0.0)
                    {
                        int pIndx;
                        pIndx = cmbFlexPivot_GapEDM.Items.IndexOf(modMain.gProject.PNR.Unit.WriteInUserL_Eng(((clsPivot.clsFP)mBearing.RadB.Pivot).GapEDM));
                        //cmbFlexPivot_GapEDM.SelectedIndex = pIndx;
                        if (pIndx != -1)
                        {
                            cmbFlexPivot_GapEDM.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(((clsPivot.clsFP)mBearing.RadB.Pivot).GapEDM);  //AES 28SEP18
                        }
                        else
                        {
                            CalcEDMGap();
                        }
                    }
                    else
                        cmbFlexPivot_GapEDM.Text = "";
                   

                    //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //{
                    //    txtFlexPivot_Rot_Stiff.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.FlexurePivot.Rot_Stiff), "#0");
                    //}
                    //else
                    //{
                    //    txtFlexPivot_Rot_Stiff.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.FlexurePivot.Rot_Stiff, "#0");
                    //}


                //  End Thrust Bearing related.
                //  ==========================

                    //if (mProduct.EndConfig[0].Type == clsEndConfig.eType.TL_TB ||
                    //    mProduct.EndConfig[1].Type == clsEndConfig.eType.TL_TB)
                    //{
                    //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //    {
                    //        //....Dist_ThrustFace
                    //        if ((mProduct.Dist_ThrustFace[0] > modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] < modMain.gcEPS))
                    //        {
                    //            txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mProduct.Dist_ThrustFace[0]));
                    //            optEndTBPos_Front.Checked = true;
                    //        }

                    //        else if ((mProduct.Dist_ThrustFace[0] < modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] > modMain.gcEPS))
                    //        {
                    //            txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mProduct.Dist_ThrustFace[1]));
                    //            optEndTBPos_Back.Checked = true;
                    //        }

                    //        else if ((mProduct.Dist_ThrustFace[0] > modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] > modMain.gcEPS))
                    //        {
                    //            txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mProduct.Dist_ThrustFace[0]));
                    //            optEndTBPos_Front.Checked = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //....Dist_ThrustFace
                    //        if ((mProduct.Dist_ThrustFace[0] > modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] < modMain.gcEPS))
                    //        {
                    //            txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(mProduct.Dist_ThrustFace[0]);
                    //            optEndTBPos_Front.Checked = true;
                    //        }

                    //        else if ((mProduct.Dist_ThrustFace[0] < modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] > modMain.gcEPS))
                    //        {
                    //            txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(mProduct.Dist_ThrustFace[1]);
                    //            optEndTBPos_Back.Checked = true;
                    //        }

                    //        else if ((mProduct.Dist_ThrustFace[0] > modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] > modMain.gcEPS))
                    //        {
                    //            txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(mProduct.Dist_ThrustFace[0]);
                    //            optEndTBPos_Front.Checked = true;
                    //        }
                    //    }
                    //}


                //  OilInlet
                //  ========                  

                    ////if (mBearing_Radial_FP.Pad.L > mBearing_Radial_FP.PAD_L_THRESHOLD)
                    ////    cmbOilInlet_Orifice_Count.Text = modMain.ConvIntToStr(mBearing_Radial_FP.OilInlet.Orifice.Count);
                    ////else
                    ////    txtOilInlet_Orifice_Count.Text = modMain.ConvIntToStr(mBearing_Radial_FP.Pad.Count);

                    //AES 12SEP18
                    //txtOilInlet_Orifice_Count.Text = modMain.ConvIntToStr(mBearing_Radial_FP.OilInlet.Orifice.Count);

                    

                    //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //{
                    //    txtOilInlet_Orifice_D.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Orifice.D));
                    //}
                    //else
                    //{
                    //    txtOilInlet_Orifice_D.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Orifice.D);
                    //}
                    


                //  Material
                //  ========
                    Double pLiningT = 0;
                    if (mBearing != null)
                    {
                        ////cmbMat_Base_WCode.Text = mBearing_Radial_FP.Mat.WCode.Base;
                       
                        int pIndex = 0;
                        Boolean pLiningExists = mBearing.RadB.Mat.LiningExists;
                        if (pLiningExists)
                        {
                            pLiningT = mBearing.RadB.LiningT;
                        }

                        if (mBearing.RadB.Mat.WCode.Base != "")
                        {
                            pIndex = cmbMat_Base_WCode.Items.IndexOf(mBearing.RadB.Mat.WCode.Base);
                        }

                        int pLiningCode_Indx = cmbMat_Lining_WCode.Items.IndexOf(mBearing.RadB.Mat.WCode.Lining);

                        cmbMat_Base_WCode.SelectedIndex = -1;
                        cmbMat_Base_WCode.SelectedIndex = pIndex;
                        txtMat_Base_Name.Text = mBearing.RadB.Mat.Base;


                        //txtMat_Base_Name.Text = mBearing_Radial_FP.Mat.Base;
                        mBearing.RadB.Mat.LiningExists = pLiningExists;
                        chkMat_LiningExists.Checked = mBearing.RadB.Mat.LiningExists;
                        pIndex = 0;

                        if (mBearing.RadB.Mat.LiningExists)
                        {
                            if (pLiningCode_Indx != -1)
                            {
                                cmbMat_Lining_WCode.SelectedIndex = pLiningCode_Indx;
                            }

                            if (mBearing.RadB.Mat.WCode.Lining != "")
                            {
                                pIndex = cmbMat_Lining_WCode.Items.IndexOf(mBearing.RadB.Mat.WCode.Lining);

                                cmbMat_Lining_WCode.SelectedIndex = -1;
                                cmbMat_Lining_WCode.SelectedIndex = pIndex;
                                txtMat_Lining_Name.Text = mBearing.RadB.Mat.Lining;
                            }
                            ////cmbMat_Lining_WCode.Text = mBearing_Radial_FP.Mat.WCode.Lining;
                            ////txtMat_Lining_Name.Text = mBearing_Radial_FP.Mat.Lining;
                        }

                        Set_LiningMat_Design();

                        ////cmbMat_Lining.Text = mBearing_Radial_FP.Mat.Lining;
                        //cmbMat_Lining_WCode.Text = mBearing_Radial_FP.Mat.WCode.Lining;
                        //txtMat_Lining_Name.Text = mBearing_Radial_FP.Mat.Lining;
                    }

                    if (mBearing.RadB.Mat.LiningExists)
                    {
                        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //{
                        //    ////if (Math.Abs(mBearing_Radial_FP.LiningT - mBearing_Radial_FP.Mat_Lining_T()) < modMain.gcEPS)
                        //    ////{
                        //    ////    txtLiningT.ForeColor = Color.Magenta; //Color.Purple;
                        //    ////    txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mat_Lining_T()));
                        //    ////}
                        //    ////else
                        //    ////{
                        //    ////    txtLiningT.ForeColor = Color.Black;
                        //    ////    txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.LiningT));
                        //    ////}

                        //    txtLiningT.ForeColor = Color.Black;
                        //    txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.LiningT));                           
                        //}
                        //else
                        //{
                        //    ////if (Math.Abs(mBearing_Radial_FP.LiningT - mBearing_Radial_FP.Mat_Lining_T()) < modMain.gcEPS)
                        //    ////{
                        //    ////    txtLiningT.ForeColor = Color.Magenta; //Color.Purple;
                        //    ////    txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mat_Lining_T());
                        //    ////}
                        //    ////else
                        //    ////{
                        //    ////    txtLiningT.ForeColor = Color.Black;
                        //    ////    txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.LiningT);
                        //    ////}

                        //    txtLiningT.ForeColor = Color.Black;
                        //    txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.LiningT);
                        //}

                        txtLiningT.ForeColor = Color.Black;
                        mBearing.RadB.LiningT = pLiningT;
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.LiningT));
                        }
                        else
                        {
                            txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.LiningT);
                        }
                    }
                    else
                    {
                        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //{
                        //    lblLiningT_Unit.Visible = false;
                        //    lblLiningT.Visible = false;
                        //}
                    }        
            }

            private void CalcEDMGap()
            //========================
            {
                if (mBearing.RadB.Pad.L + (mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1]) < 6 ||
                               Math.Abs(mBearing.RadB.Pad.L + (mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1]) - 6) < modMain.gcEPS)
                {
                    cmbFlexPivot_GapEDM.SelectedIndex = 0;
                    cmbFlexPivot_GapEDM.BackColor = Color.White;

                }
                else if (mBearing.RadB.Pad.L + (mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1]) > 6 &&
                                mBearing.RadB.Pad.L + (mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1]) < 10)
                {
                    cmbFlexPivot_GapEDM.SelectedIndex = 1;
                    cmbFlexPivot_GapEDM.BackColor = Color.White;
                }
                else
                {
                    cmbFlexPivot_GapEDM.SelectedIndex = 1;
                    cmbFlexPivot_GapEDM.BackColor = Color.Red;
                }
            }
       

            //private void SetControls()
            ////=======================                           
            //{
            //    Boolean pEnabled = false;
            //    Color pControlColor = txtRadialType.BackColor;
            //    SetControls_Status(pEnabled);       //BG 28JUN13

            //    //if (modMain.gProject.Status == "Open" &&
            //    //     (modMain.gUser.Privilege == "Engineering"))  //BG 28JUN13

            //    ////if (modMain.gProject.Status == "Open" &&
            //    ////    modMain.gUser.Role == "Engineer")   //BG 28JUN13
            //    ////{
            //        pEnabled = true;
            //        //txtPad_T_Pivot.BackColor = Color.White;
            //        txtPad_Count.BackColor = Color.White;

            //        SetControls_Status(pEnabled);
            //    ////}
            //    //else if (modMain.gProject.Status == "Closed" ||
            //    //         modMain.gUser.Privilege == "Manufacturing" ||
            //    //         modMain.gUser.Privilege == "Designer" ||
            //    //         modMain.gUser.Privilege == "General" ||
            //    //         modMain.gUser.Role != "Engineer")      //BG 28JUN13
            //    ////else
            //    ////{
            //    ////    //pEnabled = false;     //BG 28JUN13

            //    ////    txtPad_Ang.BackColor = pControlColor;   
            //    ////    txtPad_Pivot_Offset.BackColor = pControlColor;
            //    ////    txtPad_T_Pivot.BackColor = pControlColor;
            //    ////    txtPad_Count.BackColor = pControlColor;
            //    ////    txtPad_Pivot_AngStart.BackColor = pControlColor;
            //    ////    txtPad_T_Lead.BackColor = pControlColor;
            //    ////    txtPad_T_Trail.BackColor = pControlColor;

            //    ////    //SetControls_Status(pEnabled);     //BG 28JUN13
            //    ////}

               
            //    //....End Thrust Bearing related.
            //    //Boolean pblnEndTB = false;

            //    //if (mBearing.EndPlate[0].TLTB.Exists == true ||
            //    //    mBearing.EndPlate[1].TLTB.Exists == true)
            //    //{
            //    //    pblnEndTB = true;
            //    //    grpPos_TB.Refresh();
            //    //    grpPos_TB.Visible = pblnEndTB;

            //    //    //....For Both T/B
            //    //    if (mBearing.EndPlate[0].TLTB.Exists == true &&
            //    //        mBearing.EndPlate[1].TLTB.Exists == true)
            //    //    {
            //    //        optEndTBPos_Front.Visible = pblnEndTB;
            //    //        optEndTBPos_Front.Checked = pblnEndTB;
            //    //        optEndTBPos_Back.Visible = pblnEndTB;                        
            //    //    }

            //    //   //....For Front T/B
            //    //    else if (mBearing.EndPlate[0].TLTB.Exists == true)
            //    //    {
            //    //        optEndTBPos_Front.Visible = pblnEndTB;
            //    //        optEndTBPos_Front.Checked = pblnEndTB;
            //    //        optEndTBPos_Back.Visible = !pblnEndTB;                       
            //    //    }

            //    //   //....For Back T/B
            //    //    else if (mBearing.EndPlate[1].TLTB.Exists == true)
            //    //    {
            //    //        optEndTBPos_Front.Visible = !pblnEndTB;
            //    //        optEndTBPos_Back.Visible = pblnEndTB;
            //    //        optEndTBPos_Back.Checked = pblnEndTB;                       
            //    //    }                   
            //    //}
            //    //else
            //    //{
            //    //    pblnEndTB = false;
            //    //    grpPos_TB.Refresh();
            //    //    grpPos_TB.Visible = pblnEndTB;                    
            //    //}

            //    //lblEndConfig_Thrust_Sep.Visible = pblnEndTB;
            //    //lblAxialDist_PadMidPt_ThrustFace.Visible = pblnEndTB;
            //    //txtAxialDist_PadMidPt_ThrustFace.Visible = pblnEndTB;
            //}


            //private void SetControls_Status(Boolean Enable_In)
            ////==================================================
            //{
            //    //cmbSplitConfig.Enabled = Enable_In;
            //    //....Shaft Dia.
            //    txtDShaft_Range_Min.ReadOnly = !Enable_In;
            //    txtDShaft_Range_Max.ReadOnly = !Enable_In;

            //    //....Outer Dia.
            //    txtBearingOD_Range_Min.ReadOnly = !Enable_In;
            //    txtBearingOD_Range_Max.ReadOnly = !Enable_In;

            //    //....Pad Dia.
            //    txtPadBore_Range_Min.ReadOnly = !Enable_In;
            //    txtPadBore_Range_Max.ReadOnly = !Enable_In;

            //    //....Set Dia.
            //    txtBearingBore_Range_Min.ReadOnly = !Enable_In;
            //    txtBearingBore_Range_Max.ReadOnly = !Enable_In;

            //    //....Length.
            //    txtL_Available.ReadOnly = !Enable_In;
            //    //txtL_Tot.ReadOnly = !pEnable_In;

            //    //  Pad:
            //    //  ----
            //    txtLoadOrient.Enabled = Enable_In;
            //    //updPad_Count.Enabled = Enable_In;

            //    //txtPad_Count.ReadOnly = !Enable_In;
            //    txtPad_L.ReadOnly = !Enable_In;
            //    //if(modMain.gOpCond.Rot_Directionality== clsOpCond.eRotDirectionality.Uni)
            //    //    txtPad_Ang.ReadOnly = !Enable_In;                             
            //    txtPad_Pivot_Offset.ReadOnly = !Enable_In;
            //    txtPad_Pivot_AngStart.ReadOnly = !Enable_In;
            //    //chkRound.Enabled = pEnable_In;                                  

            //    //....Thickness.
            //    chkThick_Pivot.Enabled = Enable_In;

            //    ////if (modMain.gProject.Status == "Closed" ||
            //    ////    modMain.gUser.Role != "Engineer")                      
            //    ////{
            //    ////    txtPad_T_Lead.ReadOnly = !Enable_In;
            //    ////    txtPad_T_Pivot.ReadOnly = !Enable_In;
            //    ////    txtPad_T_Trail.ReadOnly = !Enable_In;
            //    ////}

            //    //txtPad_RFillet_ID.ReadOnly = !pEnable_In;

            //    //  Web
            //    //  ----
            //    txtFlexPivot_Web_T.ReadOnly = !Enable_In;
            //    txtFlexPivot_Web_RFillet.ReadOnly = !Enable_In;
            //    txtFlexPivot_Web_H.ReadOnly = !Enable_In;
            //    //txtFlexPivot_GapEDM.Enabled = Enable_In;
            //    //txtFlexPivot_Rot_Stiff.ReadOnly = !Enable_In;

                

            //    ////chkMat_LiningExists.Enabled = Enable_In;
            //    ////cmbMat_Base.Enabled = Enable_In;
            //    ////cmbMat_Lining.Enabled = Enable_In;
            //    ////txtLiningT.ReadOnly = !Enable_In;


            //    //if (mBearing.EndPlate[0].TLTB.Exists == true ||
            //    //       mBearing.EndPlate[1].TLTB.Exists == true)
            //    //{
            //    //    txtAxialDist_PadMidPt_ThrustFace.ReadOnly = !Enable_In;
            //    //}
            //}            

        #endregion   
                

       #region "CONTROL EVENT ROUTINES:" 
        //*****************************

            #region "COMMAND BUTTON RELATED ROUTINE"
            //--------------------------------------

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
                    Double pLHousingInner = mBearing.RadB.AxialSealGap[0] + mBearing.RadB.AxialSealGap[1] + mBearing.RadB.Pad.L;
                    Double pEndPlateCB_DepthMin = 0.0;

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pEndPlateCB_DepthMin = modMain.gProject.PNR.Unit.CMet_Eng(mBearing.RadB.DEPTH_END_CONFIG_MIN_METRIC);
                    }
                    else
                    {
                        pEndPlateCB_DepthMin = mBearing.RadB.DEPTH_END_CONFIG_MIN_ENGLISH;
                    }


                    if (Math.Round(mBearing.RadB.L, 4) < Math.Round(pLHousingInner))
                    {
                        pFlag = false;
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            MessageBox.Show("Housing Length (" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.L)) + ") cann't be less than \n 'Pad_L + 2 x AxialSealGap' ("
                                            + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(pLHousingInner)) + ")", "Housing Length Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Housing Length (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.L) + ") cann't be less than \n 'Pad_L + 2 x AxialSealGap' ("
                                            + modMain.gProject.PNR.Unit.WriteInUserL(pLHousingInner) + ")", "Housing Length Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        txtHousingL.Focus();
                    }
                    else if (Math.Round(mBearing.RadB.L,4) >Math.Round( mBearing.L_Available,4))
                    {
                        pFlag = false;
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            MessageBox.Show("Housing Length (" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.L)) + ") cann't be greater than \nAvailable Env. Length ("
                                            + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Available)) + ")", "Housing Length Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Housing Length (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.L) + ") cann't be greater than \nAvailable Env. Length ("
                                            + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Available) + ")", "Housing Length Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        txtHousingL.Focus();
                    }
                    else if (Math.Round(mBearing.L_Tot(), 4) > Math.Round(mBearing.L_Available,4))
                    {
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
                    }
                    else if (mBearing.RadB.EndPlateCB[0].Depth < pEndPlateCB_DepthMin)
                    {
                        pFlag = false;
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            MessageBox.Show("End Plate C'Bore Depth Front (" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB[0].Depth)) + ") cann't be less than Min. Depth Value (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.DEPTH_END_CONFIG_MIN_METRIC) + ")", "Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("End Plate C'Bore Depth Front (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.EndPlateCB[0].Depth) + ") cann't be less than Min. Depth Value (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.DEPTH_END_CONFIG_MIN_ENGLISH) + ")", "Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        txtDepth_EndConfig_Front.Focus();
                    }
                    else if (mBearing.RadB.EndPlateCB[1].Depth < pEndPlateCB_DepthMin)
                    {
                        pFlag = false;
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            MessageBox.Show("End Plate C'Bore Depth Back (" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB[1].Depth)) + ") cann't be less than Min. Depth Value (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.DEPTH_END_CONFIG_MIN_METRIC) + ")", "Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("End Plate C'Bore Depth Back (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.EndPlateCB[1].Depth) + ") cann't be less than Min. Depth Value (" + modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.DEPTH_END_CONFIG_MIN_ENGLISH) + ")", "Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        txtDepth_EndConfig_Back.Focus();
                    }

                    return pFlag;
                }

                private void CloseForm()    
                //======================
                {
                    SaveData(); 

                    this.Hide();

                    modMain.gfrmSeal.ShowDialog();

                    if (mBearing.EndPlate[0].TLTB.Exists == true || mBearing.EndPlate[1].TLTB.Exists == true)
                    {
                        modMain.gfrmThrustBearing.ShowDialog();
                    }
                }


                private void SaveData()
                //=====================
                {
                    //....SplitConfig.
                    if (cmbSplitConfig.SelectedIndex == 0)
                       ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.SplitConfig = true;
                    else
                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.SplitConfig = false;

                    //....Bearing L
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtHousingL.Text));
                    }
                    else
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.L = modMain.ConvTextToDouble(txtHousingL.Text);
                    }

                    //  Depth End Plates
                    //  -----------------
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.EndPlateCB[0].Depth =
                                                                    modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text));
                        ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.EndPlateCB[1].Depth =
                                                                    modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text));
                    }
                    else
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.EndPlateCB[0].Depth =
                                                                    modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);
                        ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.EndPlateCB[1].Depth =
                                                                    modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);
                    }

                    //  Length End Plates
                    //  ------------------
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text));
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text));
                    }
                    else
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].L = modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text);
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].L = modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text);
                    }

                    //....Save DFit,DSet,DShaft & DPad
                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.OD_Range[i] =modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mtxtOD_Range[i].Text));
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.PadBore_Range[i] =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(mtxtPadBore_Range[i].Text));
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Bore_Range[i] =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(mtxtBore_Range[i].Text));
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.DShaft_Range[i] =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(mtxtDShaft_Range[i].Text));
                        }
                        else
                        {
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.OD_Range[i] = modMain.ConvTextToDouble(mtxtOD_Range[i].Text);
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.PadBore_Range[i] = modMain.ConvTextToDouble(mtxtPadBore_Range[i].Text);
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Bore_Range[i] = modMain.ConvTextToDouble(mtxtBore_Range[i].Text);
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.DShaft_Range[i] = modMain.ConvTextToDouble(mtxtDShaft_Range[i].Text);
                        }
                    }

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).L_Available = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Available.Text));
                    }
                    else
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).L_Available = modMain.ConvTextToDouble(txtL_Available.Text);
                    }


                    #region "Pad:"
                    //  ---------                        
                        ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pad.LoadOrient =  txtLoadOrient.Text;
                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.Count = modMain.ConvTextToInt(txtPad_Count.Text);
                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.Angle = modMain.ConvTextToInt(txtPad_Ang.Text); 

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.L =modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_L.Text));
                        }
                        else
                        {
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.L = modMain.ConvTextToDouble(txtPad_L.Text);
                        }

                        //....Pivot
                        if (mtxtPivot_Loc[0].Text != "")
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.Pivot_AngStart_Casing_SL = modMain.ConvTextToDouble(mtxtPivot_Loc[0].Text);

                        Calc_Pad_Pivot_Locations(((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.Count);
                     
                        //modMain.gRadialBearing.PadAng = modMain.ConvTextToDouble(txtPad_Ang.Text);

                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.Pivot_Offset = modMain.ConvTextToDouble(txtPad_Pivot_Offset.Text);

                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Pivot_Checked = chkThick_Pivot.Checked;

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            if (mBearing.RadB.Pad.T.Lead == mBearing.RadB.Pad.T.Pivot && mBearing.RadB.Pad.T.Trail == mBearing.RadB.Pad.T.Pivot)
                            {
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Lead =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtPad_T_Pivot.Text));
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Pivot = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Pivot.Text));
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Trail = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Pivot.Text));
                            }
                            else
                            {
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Lead =modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Lead.Text));
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Pivot = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Pivot.Text));
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Trail = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Trail.Text));
                            }

                            ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pad.RFillet =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtPad_RFillet_ID.Text)); 
                        }
                        else
                        {
                            if (mBearing.RadB.Pad.T.Lead == mBearing.RadB.Pad.T.Pivot && mBearing.RadB.Pad.T.Trail == mBearing.RadB.Pad.T.Pivot)
                            {
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Lead = modMain.ConvTextToDouble(txtPad_T_Pivot.Text);
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Pivot = modMain.ConvTextToDouble(txtPad_T_Pivot.Text);
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Trail = modMain.ConvTextToDouble(txtPad_T_Pivot.Text);
                            }
                            else
                            {
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Lead = modMain.ConvTextToDouble(txtPad_T_Lead.Text);
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Pivot = modMain.ConvTextToDouble(txtPad_T_Pivot.Text);
                                ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Trail = modMain.ConvTextToDouble(txtPad_T_Trail.Text);
                            }

                            ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pad.RFillet = modMain.ConvTextToDouble(txtPad_RFillet_ID.Text); 
                        }
                       //modMain.gRadialBearing.PadRFillet = modMain.ConvTextToDouble(txtPad_RFillet_ID.Text);
                        //((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.RFillet = modMain.ConvTextToDouble(txtPad_RFillet_ID.Text);        //AES 28SEP18

                    #endregion


                    //#region "EDM Relief:"
                        ////  ----------------       //BG 06DEC12
                    //    for (int i = 0; i < 2; i++)
                    //    {
                    //        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.EDM_Relief[i]  = modMain.ConvTextToDouble(mtxtEDM_Relief[i].Text);
                    //    }
                        
                    //#endregion


                    #region"Flexure Pivot:"
                    //  ------------------
                        //....Web
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).Web_T = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFlexPivot_Web_T.Text));
                            //((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).Web_RFillet = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFlexPivot_Web_RFillet.Text));
                            ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).Web_H = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFlexPivot_Web_H.Text));

                            ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).Web_RFillet = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFlexPivot_Web_RFillet.Text));        //AES 28SEP18
                            //((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).GapEDM =  modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFlexPivot_GapEDM.Text));
                            ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).GapEDM = modMain.ConvTextToDouble(cmbFlexPivot_GapEDM.Text);
                                //modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(cmbFlexPivot_GapEDM.Text));
                            ////((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).Rot_Stiff = modMain.ConvTextToDouble(txtFlexPivot_Rot_Stiff.Text);
                        }
                        else
                        {
                            ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).Web_T = modMain.ConvTextToDouble(txtFlexPivot_Web_T.Text);
                            //((clsJBearing) modMain.gProject.PNR.Bearing).RadB.FlexurePivot.Web_RFillet = modMain.ConvTextToDouble(txtFlexPivot_Web_RFillet.Text);
                            ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).Web_H = modMain.ConvTextToDouble(txtFlexPivot_Web_H.Text);
                            ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).Web_RFillet = modMain.ConvTextToDouble(txtFlexPivot_Web_RFillet.Text);        //AES 28SEP18
                            ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).GapEDM = modMain.ConvTextToDouble(cmbFlexPivot_GapEDM.Text);
                           // ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.FlexurePivot.GapEDM = modMain.ConvTextToDouble(cmbFlexPivot_GapEDM.Text);
                        }

                        
                        

                    #endregion


                    //....End Thrust Bearing Related
                        
                            //if (mProduct.EndConfig[0].Type == clsEndConfig.eType.TL_TB ||
                            //    mProduct.EndConfig[1].Type == clsEndConfig.eType.TL_TB)
                            //{
                            //    if (optEndTBPos_Front.Checked)
                            //    {
                            //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            //        {
                            //            modMain.gProject.Product.Dist_ThrustFace[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtAxialDist_PadMidPt_ThrustFace.Text));
                            //        }
                            //        else
                            //        {
                            //            modMain.gProject.Product.Dist_ThrustFace[0] = modMain.ConvTextToDouble(txtAxialDist_PadMidPt_ThrustFace.Text);
                            //        }
                            //    }
                            //    else if (optEndTBPos_Back.Checked)
                            //    {
                            //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            //        {
                            //            modMain.gProject.Product.Dist_ThrustFace[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtAxialDist_PadMidPt_ThrustFace.Text));
                            //        }
                            //        else
                            //        {
                            //            modMain.gProject.Product.Dist_ThrustFace[1] = modMain.ConvTextToDouble(txtAxialDist_PadMidPt_ThrustFace.Text);
                            //        }
                            //    }
                            //}
                       
                      

                    #region "OilInlet:"
                    //  --------------
                        //if (mBearing_Radial_FP.Pad.L > mBearing_Radial_FP.PAD_L_THRESHOLD)
                        //    ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.OilInlet.Orifice_Count = modMain.ConvTextToInt(cmbOilInlet_Orifice_Count.Text);
                        //else
                        //    ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.OilInlet.Orifice_Count = modMain.ConvTextToInt(txtOilInlet_Orifice_Count.Text);

                        
                       
                      
                       //updPad_Count.Value = 4;     //Back to the default value

                        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //{
                        //    ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.OilInlet.Orifice_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Orifice_D.Text));
                        //}
                        //else
                        //{
                        //    ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.OilInlet.Orifice_D = modMain.ConvTextToDouble(txtOilInlet_Orifice_D.Text);
                        //}

                         

                    #endregion


                    #region "Material:"
                    // --------------
                        ////((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.Base = cmbMat_Base.Text;
                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.WCode_Base = cmbMat_Base_WCode.Text;
                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.WCode_Lining = cmbMat_Lining_WCode.Text;
                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.Base = txtMat_Base_Name.Text;
                        ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.LiningExists = chkMat_LiningExists.Checked;

                        if (((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.LiningExists)                   
                        {
                            ////((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.Lining = cmbMat_Lining.Text;
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.Lining = txtMat_Lining_Name.Text;
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLiningT.Text));
                            }
                            else
                            {
                                ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.LiningT = modMain.ConvTextToDouble(txtLiningT.Text);
                            }
                            //((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT = modMain.ConvTextToDouble(txtLiningT.Text);       //AES 28SEP18
                        }
                        else
                        {
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.Lining = "None";
                            ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT = 0.0F;
                        }

                    #endregion     
                }


                private void Update_Depth_EndConfig(Double pPrev_Pad_L_In, Double pCurr_Pad_L_In)
                {
                    if (pPrev_Pad_L_In != pCurr_Pad_L_In)
                    {
                        MessageBox.Show("Not Exact Value");
                    }
                }

                private void cmdAccessories_Click(object sender, EventArgs e)
                //===========================================================
                {
                    //modMain.gfrmAccessories.TempSensor_Count = mBearing_Radial_FP.Pad.Count;  
                    //modMain.gfrmAccessories.ShowDialog();                           
                }


                private void cmdCancel_Click(object sender, EventArgs e)
                //=======================================================
                {                    
                    this.Close();
                }


                private void cmdPrint_Click(object sender, EventArgs e)
                //=======================================================   
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                    modMain.CaptureScreen(this);
                    pd.Print();
                }

            #endregion


            #region "TEXTBOX RELATED ROUTINES:"
            //--------------------------------
       
            //....KeyDown is raised as soon as the user presses a key on the keyboard, while they're still holding 
            //........it down. USE THIS ONE.

            //....KeyPress is raised for character keys (unlike KeyDown and KeyUp, which are also raised for 
            //........noncharacter keys) while the key is pressed. This is a "higher-level" event than either 
            //........KeyDown or KeyUp, and as such, different data is available in the EventArgs.

            //....KeyUp is raised after the user releases a key on the keyboard.


            //private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
            //==============================================================
            //{
            //    ...."Key Press" event is called when a 
            //    TextBox pTxtBox = (TextBox)sender;

            //    switch (pTxtBox.Name)
            //    {
            //        case "txtDShaft_Range_Min":
            //                mblnDShaft_ManuallyChanged = true; 
            //            break;

            //        case "txtDShaft_Range_Max":
            //                mblnDShaft_ManuallyChanged = true; 
            //            break;

            //        case "txtDSet_Range_Min":
            //            mblnDSet_Changed = true;
            //            break;

            //        case "txtDSet_Range_Max":
            //            mblnDSet_Changed = true;
            //            break;

            //        case "txtFlexPivot_Web_T":
            //            mblnFlexPivot_Web_T = true;
            //            break;

            //    }
            //}


                private void TxtBox_KeyDown(object sender, KeyEventArgs e)
                //========================================================
                {       
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtDShaft_Range_Min":
                        case "txtDShaft_Range_Max":

                            mblnDShaft_ManuallyChanged = true;
                            break;

                        case "txtDSet_Range_Min":
                        case "txtDSet_Range_Max":

                            mblnDSet_ManuallyChanged = true;
                            break;

                        case "txtPad_T_Pivot":
                            mblnPad_T_Pivot_ManuallyChanged = true;
                            break;

                        case "txtFlexPivot_Web_T":
                            mblnWeb_T_ManuallyChanged = true;
                            break;
                    
                    }
                }


                private void TextBox_TextChanged(object sender, EventArgs e)
                //==========================================================    
                {
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {

                        case "txtL_Available":
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.L_Available =  modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Available.Text));
                            }
                            else
                            {
                                mBearing.L_Available = modMain.ConvTextToDouble(txtL_Available.Text);
                            }

                            if (mBearing.EndPlate[0].L < modMain.gcEPS)
                            {
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                  double pVal = mBearing.EndPlate_L_Def();
                                  if (pVal > modMain.gcEPS)
                                  {
                                      txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(pVal));
                                      txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                                  }
                                }
                                else
                                {
                                    double pVal = mBearing.EndPlate_L_Def();
                                    if (pVal > modMain.gcEPS)
                                    {
                                        txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(pVal);
                                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                                    }
                                }
                            }

                            if (mBearing.EndPlate[1].L < modMain.gcEPS)
                            {
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    double pVal = mBearing.EndPlate_L_Def();
                                    if (pVal > modMain.gcEPS)
                                    {
                                        txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(pVal));
                                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                                    }
                                }
                                else
                                {
                                    double pVal = mBearing.EndPlate_L_Def();
                                    if (pVal > modMain.gcEPS)
                                    {
                                        txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(pVal);
                                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                                    }
                                }
                            }

                            if (modMain.ConvTextToDouble(txtL_Available.Text) <modMain.ConvTextToDouble(txtL_Tot.Text))
                            {
                                txtL_Tot.BackColor = Color.Red;
                            }
                            else
                            {
                                txtL_Tot.BackColor = Color.White;
                            }

                            break;

                        case "txtDShaft_Range_Min":
                            //=====================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.DShaft_Range[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDShaft_Range_Min.Text));

                                //Display_OilInlet_Orifice_Count(mBearing.RadB.Pad.Count, mBearing.RadB.Pad.L);
                                
                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.0000"); 
                            }
                            else
                            {
                                mBearing.RadB.DShaft_Range[0] = modMain.ConvTextToDouble(txtDShaft_Range_Min.Text);

                                //Display_OilInlet_Orifice_Count(mBearing.RadB.Pad.Count, mBearing.RadB.Pad.L);

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000"); 
                            }
  
                              

                            
                            ////....Pad pivot thickness is 15% of DShaft.
                            //if (mblnDShaft_ManuallyChanged || ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T.Pivot_Checked)
                            //{
                            //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            //    {
                            //        txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Pad.TDef()), "#0.000");
                            //    }
                            //    else
                            //    {
                            //        txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.TDef(), "#0.000");
                            //    }
                            //   txtPad_T_Pivot.ForeColor = Color.Blue;
                            //   mblnDShaft_ManuallyChanged = false;
                            //}

                            //Check_Pad_T_Pivot(txtDShaft_Range_Min.Text, txtDShaft_Range_Max.Text);

                            
                            
                            break;

                        case "txtHousingL":                                //  Bearing Length:
                            //----------
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtHousingL.Text));

                                //if (mblnL_ManuallyChanged)
                                //{
                                    //....The following special actions to taken when L is manually changed.
                                    //

                                    //....End Plates Depth:        
                                    //
                                    double pDepth = modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB_Depth_Def());      //....Symmetrical Depths.

                                    //....FRONT: 
                                    if (pDepth > modMain.gcEPS)
                                    {
                                        txtDepth_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);                                        
                                        mBearing.RadB.EndPlateCB[0].Depth = modMain.gProject.PNR.Unit.CMet_Eng(pDepth);
                                    }
                                    else
                                    {
                                        txtDepth_EndConfig_Front.Text = "";
                                        mBearing.RadB.EndPlateCB[0].Depth = 0.0;
                                    }
                                    txtDepth_EndConfig_Front.ForeColor = Color.Blue;

                                    if (mBearing.EndPlate[0].L < modMain.gcEPS)
                                    {
                                        if (pDepth > modMain.gcEPS)
                                        {
                                            txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                        }
                                        else
                                        {
                                            txtLength_EndConfig_Front.Text = "";
                                        }
                                        txtLength_EndConfig_Front.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        txtLength_EndConfig_Front.ForeColor = Color.Black;
                                    }

                                    //....BACK:
                                    if (pDepth > modMain.gcEPS)
                                    {
                                        txtDepth_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                        mBearing.RadB.EndPlateCB[1].Depth = modMain.gProject.PNR.Unit.CMet_Eng(pDepth);
                                    }
                                    else
                                    {
                                        txtDepth_EndConfig_Back.Text = "";
                                        mBearing.RadB.EndPlateCB[1].Depth = 0.0;
                                    }
                                    
                                    txtDepth_EndConfig_Back.ForeColor = Color.Blue;

                                    if (mBearing.EndPlate[1].L < modMain.gcEPS)
                                    {                                        
                                        if (pDepth > modMain.gcEPS)
                                        {
                                            txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                        }
                                        else
                                        {
                                            txtLength_EndConfig_Back.Text = "";
                                        }

                                        txtLength_EndConfig_Back.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        txtLength_EndConfig_Back.ForeColor = Color.Black;
                                    }

                                    mBearing.RadB.OilInlet.Orifice_Loc_Back = mBearing.RadB.OilInlet.Orifice_Loc_Back_Def();

                                    if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                                    {
                                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                                    }
                                    else
                                    {
                                        txtL_Tot.Text = "";
                                    }
                                    //txtOilInlet_Orifice_Loc_BackFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mCurrentBearing.RadB.OilInlet.Orifice.Loc_Back));

                                    //  Reset the state. 
                                    //  ---------------
                                    //mblnL_ManuallyChanged = false;
                                //}
                            }
                            else
                            {
                                mBearing.RadB.L = modMain.ConvTextToDouble(txtHousingL.Text);

                                //if (mblnL_ManuallyChanged)
                                //{
                                    //....The following special actions to taken when L is manually changed.
                                    //
                                    //....End-Configs Depth:
                                    //
                                    double pDepth = mBearing.RadB.EndPlateCB_Depth_Def();      //....Symmetrical Depths.

                                    //....FRONT:
                                    //
                                    txtDepth_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                    txtDepth_EndConfig_Front.ForeColor = Color.Blue;
                                    //mblnDepth_EndPlate_F_ManuallyChanged = false;

                                    mBearing.RadB.EndPlateCB[0].Depth = pDepth;

                                    if (mBearing.EndPlate[0].L < modMain.gcEPS)
                                    {
                                        if (pDepth > modMain.gcEPS)
                                        {
                                            txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                        }
                                        else
                                        {
                                            txtLength_EndConfig_Front.Text = "";
                                        }
                                        txtLength_EndConfig_Front.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        txtLength_EndConfig_Front.ForeColor = Color.Black;
                                    }

                                    //....BACK:
                                    //
                                    txtDepth_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                    txtDepth_EndConfig_Back.ForeColor = Color.Blue;
                                    //mblnDepth_EndPlate_B_ManuallyChanged = false;

                                    mBearing.RadB.EndPlateCB[1].Depth = pDepth;

                                    if (mBearing.EndPlate[1].L < modMain.gcEPS)
                                    {
                                        if (pDepth > modMain.gcEPS)
                                        {
                                            txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                        }
                                        else
                                        {
                                            txtLength_EndConfig_Back.Text = "";
                                        }
                                        txtLength_EndConfig_Back.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        txtLength_EndConfig_Back.ForeColor = Color.Black;
                                    }

                                    mBearing.RadB.OilInlet.Orifice_Loc_Back = mBearing.RadB.OilInlet.Orifice_Loc_Back_Def();

                                    if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                                    {
                                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                                    }
                                    else
                                    {
                                        txtL_Tot.Text = "";
                                    }
                                   // txtOilInlet_Orifice_Loc_BackFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.RadB.OilInlet.Orifice.Loc_Back);

                                    //  Reset the state. 
                                    //  ---------------
                                    //mblnL_ManuallyChanged = false;
                                //}
                            }

                            break;


                        case "txtDShaft_Range_Max":
                            //=====================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.DShaft_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDShaft_Range_Max.Text));

                                //Display_OilInlet_Orifice_Count(mBearing.RadB.Pad.Count, mBearing.RadB.Pad.L);

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.00");

                                //txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.00"); 
                            }
                            else
                            {
                                mBearing.RadB.DShaft_Range[1] = modMain.ConvTextToDouble(txtDShaft_Range_Max.Text);

                                //Display_OilInlet_Orifice_Count(mBearing.RadB.Pad.Count, mBearing.RadB.Pad.L);

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000"); 
                            }
                                
                            
                            
                            ////....Pad pivot thickness is 15% of DShaft.
                            //if (mblnDShaft_ManuallyChanged || ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T.Pivot_Checked)
                            //{
                            //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            //    {
                            //        txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Pad.TDef()), "#0.000");
                            //    }
                            //    else
                            //    {
                            //        txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.TDef(), "#0.000");
                            //    }
                            //    mblnDShaft_ManuallyChanged = false;
                            //}

                            //Check_Pad_T_Pivot(txtDShaft_Range_Min.Text, txtDShaft_Range_Max.Text);
                            break;

                        case "txtDFit_Range_Min":
                            //=================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.OD_Range[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing.RadB.OD_Range[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            break;

                        case "txtDFit_Range_Max":
                            //==================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.OD_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing.RadB.OD_Range[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            break;

                        case "txtDSet_Range_Min":
                            //===================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.Bore_Range[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtBearingBore_Range_Min.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.0000");
                            }
                            else
                            {
                                mBearing.RadB.Bore_Range[0] = modMain.ConvTextToDouble(txtBearingBore_Range_Min.Text);

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000");
                            }

                           

                            //....RFillet_ID
                            ////if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            ////{
                            ////    txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Pad.RFillet), "#0.000");
                            ////}
                            ////else
                            ////{
                            ////    txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.RFillet, "#0.000");
                            ////}

                            //txtPad_RFillet_ID.Text = modMain.gUnit.WriteInUserL_Eng(mBearing_Radial_FP.Pad.RFillet);

                            //....Lining T
                            if (mblnDSet_ManuallyChanged)
                            {
                                ((clsPivot.clsFP)((clsJBearing)modMain.gProject.PNR.Bearing).RadB.Pivot).EDM_Pad.RFillet_Back = 0.0;
                                ////txtLiningT.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mat_Lining_T(), "#0.000");
                                mblnDSet_ManuallyChanged = false;
                            }
                          
                            break;

                        case "txtDSet_Range_Max":
                            //===================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.Bore_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtBearingBore_Range_Max.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.0000");

                            }
                            else
                            {
                                mBearing.RadB.Bore_Range[1] = modMain.ConvTextToDouble(txtBearingBore_Range_Max.Text);

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000");

                            }

                           
                            //....RFillet_ID
                            //txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.RFillet_ID(), "#0.000");
                            //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            //{
                            //    txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Pad.RFillet), "#0.000");
                            //}
                            //else
                            //{
                            //    txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.RFillet, "#0.000");
                            //}

                           // txtPad_RFillet_ID.Text = modMain.gUnit.WriteInUserL_Eng(mBearing_Radial_FP.Pad.RFillet);

                            //////....Lining T
                            ////if (mblnDSet_ManuallyChanged)
                            ////{
                            ////    ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.EDM_Pad.RFillet_Back = 0.0;
                            ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            ////    {
                            ////        txtLiningT.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mat_Lining_T()), "#0.000");
                            ////    }
                            ////    else
                            ////    {
                            ////        txtLiningT.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mat_Lining_T(), "#0.000");
                            ////    }
                            ////    mblnDSet_ManuallyChanged = false;
                            ////}                          

                            break;


                        case "txtDPad_Range_Min":
                            //===================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.PadBore_Range[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPadBore_Range_Min.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.00");

                                //txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.00");   
                            }
                            else
                            {
                                mBearing.RadB.PadBore_Range[0] = modMain.ConvTextToDouble(txtPadBore_Range_Min.Text);

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000");   
                            }

                           
                            
                            break;

                        case "txtDPad_Range_Max":
                            //===================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.PadBore_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPadBore_Range_Max.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.0000"); 
                            }
                            else
                            {
                                mBearing.RadB.PadBore_Range[1] = modMain.ConvTextToDouble(txtPadBore_Range_Max.Text);

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                //txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000"); 
                            }
                            
                            
                            break;                      
                       

                        case "txtPad_Count":
                            //==============
                            //....Pad Count.
                            int pCount = modMain.ConvTextToInt(txtPad_Count.Text);
                            Set_Pad_Pivot_Locations(pCount);

                            //....Set Pad Count.
                            mBearing.RadB.Pad.Count = pCount;

                            //....Set Pad Angle.
                            //txtPad_Ang.ForeColor = Color.Purple;               
                            txtPad_Ang.Text = mBearing.RadB.Pad.Angle.ToString();
                            if (txtPad_Pivot_AngStart.Text != "")
                                Calc_Pad_Pivot_Locations(pCount);                            

                            //....Set Orifice Count.
                            //txtOilInlet_Orifice_Count.Text = Convert.ToString(pCount);
                            //Display_OilInlet_Orifice_Count(mBearing.RadB.Pad.Count, mBearing.RadB.Pad.L);

                            break;
                                                   

                        case "txtPad_L":
                            //==========
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.Pad.L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing.RadB.Pad.L = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                            //Display_OilInlet_Orifice_Count(mBearing.RadB.Pad.Count,mBearing.RadB.Pad.L);

                            //....Total L
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                                {
                                    txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                                }
                                else
                                {
                                    txtL_Tot.Text = "";
                                }
                            }
                            else
                            {
                                if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                                {
                                    txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                                }
                                else
                                {
                                    txtL_Tot.Text = "";
                                }
                            }
                            break;

                        
                        case "txtPad_Pivot_Offset":
                            //=====================
                            mBearing.RadB.Pad.Pivot_Offset = modMain.ConvTextToDouble(pTxtBox.Text);

                            if (pTxtBox.Text.Contains("."))
                            {
                                string pNum = modMain.ExtractPostData(pTxtBox.Text, ".");
                                if (pNum.Length > 1)
                                    pTxtBox.Text = modMain.ConvDoubleToStr(Math.Round(mBearing.RadB.Pad.Pivot.Offset, 1),"#0.0");
                            }
                            break;


                        case "txtPad_Pivot_AngStart":
                            //=======================
                            int pPadCount = modMain.ConvTextToInt(txtPad_Count.Text);
                            mBearing.RadB.Pad.Pivot_AngStart_Casing_SL =
                               modMain.ConvTextToDouble(txtPad_Pivot_AngStart.Text);

                            if (txtPad_Pivot_AngStart.Text != "")
                            {
                                Calc_Pad_Pivot_Locations(pPadCount);
                            }
                            else if (txtPad_Pivot_AngStart.Text == "")
                            {
                                for (int i = 1; i < pPadCount; i++)
                                {
                                    mtxtPivot_Loc[i].Text = "";
                                }
                            }
                            break;


                        case "txtPad_T_Lead":
                            //================   
                                  
                            if (!chkThick_Pivot.Checked)
                            {
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing.RadB.Pad.T_Lead = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                                }
                                else
                                {
                                    mBearing.RadB.Pad.T_Lead = modMain.ConvTextToDouble(pTxtBox.Text);
                                }
                            }
                           
                            break;


                        case "txtPad_T_Pivot":
                            //================  

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.Pad.T_Pivot =modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing.RadB.Pad.T_Pivot = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                            if (chkThick_Pivot.Checked)
                            {
                                Set_Pad_T_Lead_AND_Trail (mBearing.RadB.Pad.T.Pivot);
                            }

                            if (mblnPad_T_Pivot_ManuallyChanged)
                            {
                                pTxtBox.ForeColor = Color.Black;
                            }
                            break;


                        case "txtPad_T_Trail":
                            //=================  
       
                            if (!chkThick_Pivot.Checked)
                            {
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing.RadB.Pad.T_Trail = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                                }
                                else
                                {
                                    mBearing.RadB.Pad.T_Trail = modMain.ConvTextToDouble(pTxtBox.Text);
                                }
                            }
                                                        
                            break;


                        case "txtFlexPivot_Web_T":
                            //====================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsPivot.clsFP)mBearing.RadB.Pivot).Web_T = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                ((clsPivot.clsFP)mBearing.RadB.Pivot).Web_T = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                             if (mblnWeb_T_ManuallyChanged)
                             {
                                 Double pWeb_RFillet;
                                 if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                 {
                                     pWeb_RFillet = modMain.MRound(modMain.gProject.PNR.Unit.CEng_Met(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.T), 0.005);
                                 }
                                 else
                                 {
                                     pWeb_RFillet = modMain.MRound(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.T, 0.005);
                                 }
                                 //pWeb_RFillet = modMain.MRound(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.T, 0.005);
                                 txtFlexPivot_Web_RFillet.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(pWeb_RFillet);
                                 txtFlexPivot_Web_RFillet.ForeColor = Color.Blue;
                             }
                             else
                             {
                                 txtFlexPivot_Web_RFillet.ForeColor = Color.Black;
                             }
                             mblnWeb_T_ManuallyChanged = false;
                            break;


                        case "txtFlexPivot_Web_RFillet":
                            //============================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsPivot.clsFP)mBearing.RadB.Pivot).Web_RFillet = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                ((clsPivot.clsFP)mBearing.RadB.Pivot).Web_RFillet = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                           //((clsPivot.clsFP)mBearing.RadB.Pivot).Web_RFillet = modMain.ConvTextToDouble(pTxtBox.Text);

                            Double pPrev_Web_RFillet = modMain.MRound(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.T, 0.005);
                            if (Math.Abs(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.RFillet - pPrev_Web_RFillet) < modMain.gcEPS)
                            {
                                txtFlexPivot_Web_RFillet.ForeColor = Color.Magenta;
                            }
                            else
                            {
                                txtFlexPivot_Web_RFillet.ForeColor = Color.Black;
                            }

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                if (((clsPivot.clsFP)mBearing.RadB.Pivot).Web.RFillet > modMain.gcEPS)
                                {
                                    lblFlexPivot_Web_RFillet.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(((clsPivot.clsFP)mBearing.RadB.Pivot).Web.RFillet) + "]";
                                }
                            }

                            break;


                        case "txtFlexPivot_Web_H":
                            //======================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsPivot.clsFP)mBearing.RadB.Pivot).Web_H = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                ((clsPivot.clsFP)mBearing.RadB.Pivot).Web_H = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            break;


                        //case "txtFlexPivot_Rot_Stiff":
                        //    //=========================
                        //    mBearing_Radial_FP.FlexurePivot.Rot_Stiff = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;


                        //case "txtAxialDist_PadMidPt_ThrustFace":
                        //    //==================================
                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        if (optEndTBPos_Front.Checked)
                        //            mProduct.Dist_ThrustFace[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));

                        //        else if (optEndTBPos_Back.Checked)
                        //            mProduct.Dist_ThrustFace[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        //    }
                        //    else
                        //    {
                        //        if (optEndTBPos_Front.Checked)
                        //            mProduct.Dist_ThrustFace[0] = modMain.ConvTextToDouble(pTxtBox.Text);

                        //        else if (optEndTBPos_Back.Checked)
                        //            mProduct.Dist_ThrustFace[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    }
                        //    break;
                          
  
                        

                        case "txtMat_Lining_Name":
                            //====================
                            if (mBearing != null)
                            {
                                mBearing.RadB.Mat.Lining = pTxtBox.Text;
                                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //{
                                //    txtLiningT.Text = modMain.gProject.PNR.Unit.CEng_Met(((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT).ToString("0.000");
                                //}
                                //else
                                //{
                                //    txtLiningT.Text = ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT.ToString("0.000");
                                //}

                                txtLiningT.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing.RadB.LiningT);

                                ////if (((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.WCode.Lining != mBearing_Radial_FP.Mat.WCode.Lining)
                                ////{
                                ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                ////    {
                                ////        //txtLiningT.Text = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mat_Lining_T()).ToString("0.000");

                                ////        txtLiningT.Text = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.LiningT).ToString("0.000");
                                ////    }
                                ////    else
                                ////    {
                                ////        //txtLiningT.Text = mBearing_Radial_FP.Mat_Lining_T().ToString("0.000");
                                ////        txtLiningT.Text = mBearing_Radial_FP.LiningT.ToString("0.000");
                                ////    }
                                ////}
                                ////else
                                ////{
                                ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                ////    {
                                ////        txtLiningT.Text = modMain.gProject.PNR.Unit.CEng_Met(((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT).ToString("0.000");
                                ////    }
                                ////    else
                                ////    {
                                ////        txtLiningT.Text = ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT.ToString("0.000");
                                ////    }

                                ////}
                            }

                            break;

                        case "txtLiningT":
                            //===========
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.LiningT = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing.RadB.LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                           // mBearing.RadB.LiningT = modMain.ConvTextToDouble(pTxtBox.Text);

                               if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                               {
                                   if (mBearing.RadB.LiningT > modMain.gcEPS)
                                   {
                                       lblLiningT.Visible = true;
                                       lblLiningT.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing.RadB.LiningT) + "]";
                                   }
                                   else
                                   {
                                       lblLiningT.Visible = false;
                                   }
                               }

                            ////if (Math.Abs(mBearing_Radial_FP.LiningT - mBearing_Radial_FP.Mat_Lining_T()) < modMain.gcEPS)
                            ////{
                            ////    txtLiningT.ForeColor = Color.Magenta;
                            ////}
                            ////else
                            ////{
                            ////    txtLiningT.ForeColor = Color.Black;
                            ////}
                           
                            break;

                        case "txtPad_RFillet_ID":
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing.RadB.Pad.RFillet = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing.RadB.Pad.RFillet = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                if (mBearing.RadB.Pad.RFillet > modMain.gcEPS)
                                {
                                    lblPad_RFillet_ID.Text ="["+ modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing.RadB.Pad.RFillet)+ "]";
                                }
                            }

                            break;
                      
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
                            //Lbl_Unit_In.Visible = false;
                            Lbl_Metric_Val_In.Visible = false;
                        }
                    }
                    else
                    {
                        //Lbl_Unit_In.Visible = false;
                        Lbl_Metric_Val_In.Visible = false;
                    }
                }


                //private void Check_Pad_T_Pivot(String DShaft_Min_In, String DShaft_Max_In)
                ////========================================================================
                //{
                //    //PB 16JAN13. To be reviewed. Some adhoc changes made here.
                //    //
                //    if(DShaft_Min_In == "" && DShaft_Max_In == "")
                //    {                       
                //        txtPad_T_Pivot.Text = "";

                //        if (chkThick_Pivot.Checked)
                //        {
                //            txtPad_T_Lead.Text = "";
                //            txtPad_T_Trail.Text = "";
                //        }
                //    }
                //    else if (DShaft_Min_In == "" && DShaft_Max_In != "")
                //    {                       
                //        if (mBearing_Radial_FP.DShaft_Range[0] < modMain.gcEPS)
                //        {                 
                //            mBearing_Radial_FP.DShaft_Range[0] = 0;
                //        }
                //        //txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.TDef(), "#0.000"); //PB 16JAN13
                //    }
                //    else if (DShaft_Min_In != "" && DShaft_Max_In == "")
                //    {
                //       if (mBearing_Radial_FP.DShaft_Range[1] < modMain.gcEPS)
                //        {
                //            mBearing_Radial_FP.DShaft_Range[1] = 0;
                //        }
                //        //txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.TDef(), "#0.000");   //PB 16JAN13
                //    }
                //}


                //private void Display_OilInlet_Orifice_Count(int PadCount_In,double PadL_In)
                ////========================================================================    
                //{ 
                //    //if (PadL_In > mBearing_Radial_FP.PAD_L_THRESHOLD)  //BG 22MAR13
                //    Double pLD = (double)(PadL_In / mBearing.RadB.DShaft());
                //    if (pLD > 0.8)   
                //    {
                //        cmbOilInlet_Orifice_Count.Visible = true;
                //        //txtOilInlet_Orifice_Count.Visible = false;
                        
                //        cmbOilInlet_Orifice_Count.Items.Clear();
                //        cmbOilInlet_Orifice_Count.Items.Add(PadCount_In);
                //        cmbOilInlet_Orifice_Count.Items.Add((PadCount_In * 2));

                //        if (mBearing.RadB.OilInlet.Orifice.Count != 0)
                //        {
                //            cmbOilInlet_Orifice_Count.SelectedIndex = cmbOilInlet_Orifice_Count.Items.
                //                                                          IndexOf(mBearing.RadB.OilInlet.Orifice.Count);

                //            if( cmbOilInlet_Orifice_Count.SelectedIndex==-1)
                //                cmbOilInlet_Orifice_Count.SelectedIndex = 0;
                //        }
                //        else
                //            cmbOilInlet_Orifice_Count.SelectedIndex = 0;
                //    }
                //    else
                //    {
                //        cmbOilInlet_Orifice_Count.Visible = true;
                //        cmbOilInlet_Orifice_Count.Items.Clear();
                //        cmbOilInlet_Orifice_Count.Items.Add(PadCount_In);
                //        cmbOilInlet_Orifice_Count.SelectedIndex = 0;
                //    }
                //}

               
                private void Set_Pad_T_Lead_AND_Trail(double T_Pivot_In)    
                //======================================================    
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        if (T_Pivot_In != 0.0)
                        {
                            txtPad_T_Lead.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(T_Pivot_In));
                            txtPad_T_Trail.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(T_Pivot_In));

                            mBearing.RadB.Pad.T_Lead = T_Pivot_In;
                            mBearing.RadB.Pad.T_Trail = T_Pivot_In;
                        }
                    }
                    else
                    {
                        if (T_Pivot_In != 0.0)
                        {
                            txtPad_T_Lead.Text = modMain.gProject.PNR.Unit.WriteInUserL(T_Pivot_In);
                            txtPad_T_Trail.Text = modMain.gProject.PNR.Unit.WriteInUserL(T_Pivot_In);

                            mBearing.RadB.Pad.T_Lead = T_Pivot_In;
                            mBearing.RadB.Pad.T_Trail = T_Pivot_In;
                        }
                    }
                }


                private void Set_Pad_Pivot_Locations(int PadCount_In)
                //====================================================
                {
                    int pIndx;

                    for (pIndx = 0; pIndx < PadCount_In; pIndx++)
                    {
                        mtxtPivot_Loc[pIndx].Visible = true;
                    }
                    
                    for (; pIndx < mBearing.RadB.Pad.Count_Max; pIndx++)              
                        mtxtPivot_Loc[pIndx].Visible = false;
                }


                private void Calc_Pad_Pivot_Locations(int PadCount_In)
                //====================================================
                {
                    //if (mBearing_Radial_FP.Pad.Pivot.AngStart != 0.0F)        //AES 17SEP18
                    //{
                        Double pLocVal;
                        pLocVal = modMain.ConvTextToDouble(txtPad_Pivot_AngStart.Text);
                        Double psngDeg = 0.0F;

                        if (PadCount_In != 0)
                        {
                            psngDeg = Convert.ToDouble(360 / PadCount_In);

                            for (int i = 1; i < PadCount_In; i++)
                            {
                                mtxtPivot_Loc[i].Text = (pLocVal + (i * psngDeg)).ToString("#0");
                            }
                        }
                    //}
                }


                 //AM 13AUG12
                //private void txtBox_MouseDown(object sender, MouseEventArgs e)
                ////============================================================  
                //{
                //    TextBox pTxtBox = (TextBox)sender;
                //    //pTxtBox.ForeColor = Color.Black;

                //    switch (pTxtBox.Name)
                //    {
                //        case "":

                //            break;

                //    //    case "txtL_Tot":
                //    //    //--------------    //SB 15JUL09
                //    //        mBearing.Clone(ref mTempBearing);

                //    //        //chkRound.Checked = false;

                //    //        mblnL_Tot = true;
                //    //        mblnSeal_L = false;

                //    //        break;

                //    //    case "txtPad_L":
                //    //        //--------------
                //    //        mBearing.Clone(ref mTempBearing);

                //    //        //chkRound.Checked = false;

                //    //        mblnL_Tot = true;
                //    //        mblnSeal_L = false;

                //    //        break;
                //    }
                //}
       
            #endregion


            #region "UPDOWN CONTROL RELATED ROUTINE"
            //--------------------------------------

                //private void updPad_Count_ValueChanged(object sender, EventArgs e)
                ////=================================================================
                //{
                //    txtPad_Count.Text = updPad_Count.Value.ToString();
                //    txtPad_Ang.Text = CalcAngle(modMain.ConvTextToInt(txtPad_Count.Text)).ToString();
                //}

                private int CalcAngle(int PadCount_In)
                //====================================
                {
                    int pPadAngle = 0;

                    if (PadCount_In == 3)
                    {
                        pPadAngle = 100;
                    }
                    else if (PadCount_In == 4)
                    {
                        pPadAngle = 72;
                    }
                    else if (PadCount_In == 5)
                    {
                        pPadAngle = 60;
                    }
                    else if (PadCount_In == 6)
                    {
                        pPadAngle = 50;
                    }
                    return pPadAngle;
                }

                

            #endregion


            #region "COMBO BOX RELATED ROUTINE"
            //---------------------------------

                private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
                //===================================================================== 
                {
                    ComboBox pCmbBox = (ComboBox)sender;

                    switch (pCmbBox.Name)
                    {
                        case "cmbSplitConfig":
                            //==============
                           
                            if (mBearing != null)
                            {
                                pCmbBox.SelectedIndex = 0;

                                if (pCmbBox.SelectedIndex == 0)
                                    mBearing.RadB.SplitConfig = true;
                                else
                                    mBearing.RadB.SplitConfig = false;

                                //if (mBearing.RadB.SplitConfig)
                                //{
                                //    updPad_Count.Minimum = 4;
                                //    updPad_Count.Maximum = 6;
                                //    updPad_Count.Increment = 2;
                                //    updPad_Count.Value = 4;
                                //}
                                //else
                                //{
                                //    updPad_Count.Minimum = 3;
                                //    updPad_Count.Maximum = 6;
                                //    updPad_Count.Increment = 1;
                                //    updPad_Count.Value = 3;
                                //}
                            }
                            break;

                        //case "cmbLoadOrient":
                        //    //====================
                        //    if (pCmbBox.Text != "")
                        //        if (mBearing != null)
                        //        {
                        //            mBearing.RadB.Pad.LoadOrient = (clsRadB.clsPad.eLoadOrient)
                        //                                           Enum.Parse(typeof(clsRadB.clsPad.eLoadOrient), pCmbBox.Text);
                        //            //Set_Pad_Pivot_AngStart(pCmbBox.Text);
                        //            //txtPad_Pivot_AngStart.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.Set_Pivot_AngStart(),"");
                        //        }
                        //    break;

                        case "cmbFlexPivot_GapEDM":
                            //=========================
                            if (mBearing != null)
                            {
                                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //{
                                //    mBearing_Radial_FP.FlexurePivot.GapEDM = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pCmbBox.Text));
                                //}
                                //else
                                //{
                                //    mBearing_Radial_FP.FlexurePivot.GapEDM = modMain.ConvTextToDouble(pCmbBox.Text);
                                //}
                                ((clsPivot.clsFP)mBearing.RadB.Pivot).GapEDM = modMain.ConvTextToDouble(pCmbBox.Text);

                                if(modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    if (((clsPivot.clsFP)mBearing.RadB.Pivot).GapEDM > modMain.gcEPS)
                                    {
                                        lblFlexPivot_GapEDM.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(((clsPivot.clsFP)mBearing.RadB.Pivot).GapEDM)) ;
                                    }
                                }
                            }
                            break;                    

                        ////case "cmbMat_Base":
                        ////    //=============
                        ////    if (mBearing_Radial_FP != null)
                        ////    {
                        ////        mBearing_Radial_FP.Mat.Base = pCmbBox.Text;
                        ////        txtBaseMat_WaukeshaCode.Text = mBearing_Radial_FP.Mat.MatCode(pCmbBox.Text);
                        ////    }
                        ////    break;

                        case "cmbMat_Base_WCode":
                            //=========================
                            if (mBearing != null)
                            {
                                mBearing.RadB.Mat.WCode_Base = pCmbBox.Text;

                                if (pCmbBox.Text == "Other")
                                {
                                    txtMat_Base_Name.Text = "";
                                    txtMat_Base_Name.ReadOnly = false;
                                    txtMat_Base_Name.BackColor = Color.White;
                                    chkMat_LiningExists.Checked = false;
                                }
                                else
                                {
                                    txtMat_Base_Name.ReadOnly = true;
                                    //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                                    txtMat_Base_Name.Text = mBearing.RadB.Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
                                    txtMat_Base_Name.BackColor = Color.LightGray;

                                    if (txtMat_Base_Name.Text == "STEEL")
                                    {
                                        chkMat_LiningExists.Checked = true;
                                    }
                                    else
                                    {
                                        chkMat_LiningExists.Checked = false;
                                    }
                                }
                                mBearing.RadB.Mat.Base = txtMat_Base_Name.Text;
                                
                            }
                               
                            break;

                        ////case "cmbMat_Lining":
                        ////    //===============
                        ////    if (mBearing_Radial_FP != null)
                        ////    {

                        ////        mBearing_Radial_FP.Mat.Lining = pCmbBox.Text;
                        ////        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////        {
                        ////            txtLiningT.Text =modMain.gProject.PNR.Unit.CEng_Met( ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT).ToString("0.000"); 
                        ////        }
                        ////        else
                        ////        {
                        ////            txtLiningT.Text = ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT.ToString("0.000"); 
                        ////        }
                                


                        ////        if (((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Mat.Lining != mBearing_Radial_FP.Mat.Lining)
                        ////        {
                        ////            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////            {
                        ////                txtLiningT.Text = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mat_Lining_T()).ToString("0.000");
                        ////            }
                        ////            else
                        ////            {
                        ////                txtLiningT.Text = mBearing_Radial_FP.Mat_Lining_T().ToString("0.000");
                        ////            }
                        ////        }
                        ////        else
                        ////        {
                        ////            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////            {
                        ////                txtLiningT.Text = modMain.gProject.PNR.Unit.CEng_Met(((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT).ToString("0.000");
                        ////            }
                        ////            else
                        ////            {
                        ////                txtLiningT.Text = ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.LiningT.ToString("0.000");
                        ////            }
                                    
                        ////        }

                        ////        txtLining_WaukeshaCode.Text = mBearing_Radial_FP.Mat.MatCode(pCmbBox.Text);
                        ////    }
  
                        ////    break;

                        case "cmbMat_Lining_WCode":
                            //=====================
                            if (mBearing != null)
                            {
                                mBearing.RadB.Mat.WCode_Lining = pCmbBox.Text;
                                if (pCmbBox.Text == "Other")
                                {
                                    txtMat_Lining_Name.Text = "";
                                    txtMat_Lining_Name.ReadOnly = false;
                                    txtMat_Lining_Name.BackColor = Color.White;
                                }
                                else
                                {
                                    txtMat_Lining_Name.ReadOnly = true;
                                    //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                                    txtMat_Lining_Name.Text = mBearing.RadB.Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
                                    txtMat_Lining_Name.BackColor = Color.LightGray;
                                }

                                mBearing.RadB.Mat.Lining = txtMat_Lining_Name.Text;
                            }

                            break;
                    }
                }

              

            #endregion


            #region "CHECKBOX RELATED ROUTINE"

                private void ChkBox_CheckedChanged(object sender, EventArgs e)
                //============================================================   
                {
                    CheckBox pChkBox = (CheckBox)sender;

                    switch (pChkBox.Name)
                    {
                        case "chkThick_Pivot":
                            //----------------
                               Set_Pad_T_Design();
                             
                               if (chkThick_Pivot.Checked)      //BG 21MAR13
                               {              
                                   Set_Pad_T_Lead_AND_Trail(mBearing.RadB.Pad.T.Pivot);
                               }
                               ((clsJBearing) modMain.gProject.PNR.Bearing).RadB.Pad.T_Pivot_Checked = chkThick_Pivot.Checked;
                           
                            break;


                        case "chkMat_LiningExists":
                            //---------------------
                            mBearing.RadB.Mat.LiningExists = chkMat_LiningExists.Checked;
                           Set_LiningMat_Design();


                            if (!chkMat_LiningExists.Checked)
                            {
                                mBearing.RadB.Mat.Lining = "None";    
                                mBearing.RadB.LiningT = 0.0F;
                                txtLiningT.Text = modMain.ConvDoubleToStr(0.0F, "#0.000");

                                //lblLiningT_Unit.Visible = false;
                                //lblLiningT.Visible = false;
                            }
                            else
                            {
                                ////cmbMat_Lining.Text = "Babbitt";
                                cmbMat_Lining_WCode.SelectedIndex = 0;
                                txtLiningT.Text = modMain.ConvDoubleToStr(mBearing.RadB.LiningT, "#0.000");

                                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //{
                                //    lblLiningT_Unit.Visible = true;
                                //    lblLiningT.Visible = true;
                                //}

                            }

                            DisplayLblMetric(txtLiningT, lblLiningT, chkMat_LiningExists.Checked);

                            break;
                    }
                }

                private void Set_LiningMat_Design()
                //=================================   
                {                
                    cmbMat_Lining_WCode.Visible = chkMat_LiningExists.Checked;
                    txtMat_Lining_Name.Visible = chkMat_LiningExists.Checked;   
                    lblThick.Visible = chkMat_LiningExists.Checked;
                    txtLiningT.Visible = chkMat_LiningExists.Checked;
                    //lblLiningT_Unit.Visible = chkMat_LiningExists.Checked;
                    //lblLiningT.Visible = chkMat_LiningExists.Checked;
                }


                private void Set_Pad_T_Design()
                //===========================                      
                {
                    txtPad_T_Lead.ReadOnly = chkThick_Pivot.Checked;
                    Set_ForeAndBackColor(ref txtPad_T_Lead, chkThick_Pivot.Checked);

                    txtPad_T_Trail.ReadOnly = chkThick_Pivot.Checked;
                    Set_ForeAndBackColor(ref txtPad_T_Trail, chkThick_Pivot.Checked);                    
                }


                private void Set_ForeAndBackColor (ref TextBox TxtBox_In, bool bln_In)
                //====================================================================    
                {
                    if (bln_In)
                    {
                        TxtBox_In.ForeColor = Color.Blue;
                        Color pColor = Color.FromArgb(255, 235, 233, 237);
                        TxtBox_In.BackColor = pColor;
                    }
                    else
                    {
                        TxtBox_In.ForeColor = Color.Black;
                        TxtBox_In.BackColor = Color.White;
                    }
                }

            #endregion


            #region "OPTION BUTTON RELATED ROUTINE:"

                private void optButton_CheckedChanged(object sender, EventArgs e)
                //===============================================================
                {
                    RadioButton pOptBtn = (RadioButton)sender;

                    //switch (pOptBtn.Name)
                    //{
                        //case "optEndTBPos_Front":
                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(modMain.gProject.Product.Dist_ThrustFace[0]), "");
                        //    }
                        //    else
                        //    {
                        //        txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Dist_ThrustFace[0], "");
                        //    }
                        //    break;

                        //case "optEndTBPos_Back":
                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(modMain.gProject.Product.Dist_ThrustFace[1]), "");
                        //    }
                        //    else
                        //    {
                        //        txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Dist_ThrustFace[1], "");
                        //    }
                        //    //txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Dist_ThrustFace[1], "");
                        //    break;
                    //}
                }
           #endregion

       #endregion


       #region "UTILITY ROUTINES:"
       //*************************

            private Boolean IsDiaNotNull()
            //============================
            {
                if ((mBearing.RadB.DShaft_Range[0] != 0.0F && mBearing.RadB.DShaft_Range[1] != 0.0F)
                || (mBearing.RadB.Bore_Range[0] != 0.0F && mBearing.RadB.Bore_Range[1] != 0.0F)
                || (mBearing.RadB.PadBore_Range[0] != 0.0F && mBearing.RadB.PadBore_Range[1] != 0.0F))
     
                    return true;

                else
                    return false;
            }

       #endregion

            private void cmdClose_Click(object sender, EventArgs e)
            //======================================================
            {
                 Boolean pIsInputValid = ValidateInput();
                 if (pIsInputValid)
                 {
                     SaveData();
                     this.Hide();
                 }
            }

            private void txtLength_EndConfig_Front_TextChanged(object sender, EventArgs e)
            //============================================================================
            {
                double pDepth = 0.0;
                Double pVal = 0.0;
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    mBearing.EndPlate[0].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text));
                    pDepth = modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB_Depth_Def());
                    pVal = modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text);

                    if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                    {
                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                    }
                    else
                    {
                        txtL_Tot.Text = "";
                    }
                }
                else
                {
                    mBearing.EndPlate[0].L = modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text);
                    pDepth = mBearing.RadB.EndPlateCB_Depth_Def();
                    pVal = modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text);

                    if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                    {
                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                    }
                    else
                    {
                        txtL_Tot.Text = "";
                    }
                }

                if (Math.Abs(pVal - pDepth) > modMain.gcEPS)
                {
                    txtLength_EndConfig_Front.ForeColor = Color.Black;
                }
                else
                {
                    txtLength_EndConfig_Front.ForeColor = Color.Blue;
                }

                
            }

            private void txtLength_EndConfig_Back_TextChanged(object sender, EventArgs e)
            //===========================================================================
            {
                double pDepth = 0.0;
                Double pVal = 0.0;

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    mBearing.EndPlate[1].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text));
                    pDepth = modMain.gProject.PNR.Unit.CEng_Met(mBearing.RadB.EndPlateCB_Depth_Def());
                    pVal = modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text);

                    if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                    {
                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                    }
                    else
                    {
                        txtL_Tot.Text = "";
                    }
                }
                else
                {
                    mBearing.EndPlate[1].L = modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text);
                    pDepth = mBearing.RadB.EndPlateCB_Depth_Def();
                    pVal = modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text);

                    if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                    {
                        txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                    }
                    else
                    {
                        txtL_Tot.Text = "";
                    }
                }

                if (Math.Abs(pVal - pDepth) > modMain.gcEPS)
                {
                    txtLength_EndConfig_Back.ForeColor = Color.Black;
                }
                else
                {
                    txtLength_EndConfig_Back.ForeColor = Color.Blue;
                }
            }

            private void txtL_Tot_TextChanged(object sender, EventArgs e)
            //===========================================================
            {
                if (Math.Round(mBearing.L_Available, 4) < Math.Round(mBearing.L_Tot(),4))
                {
                    txtL_Tot.BackColor = Color.Red;
                }
                else
                {
                    txtL_Tot.BackColor = Color.White;
                }
            }

            private void txtDepth_EndConfig_Validating(object sender, CancelEventArgs e)
            //==========================================================================
            {
                TextBox pTxtBox = (TextBox)sender;
                Double pVal = 0.0;

                switch (pTxtBox.Name)
                {
                    case "txtDepth_EndConfig_Front":
                        //------------------------------
                        Double pPreVal = mBearing.RadB.EndPlateCB[0].Depth;

                        //if (mblnL_ManuallyChanged)    //PB 17JAN13
                        //{
                        //    mBearing_Radial_FP.Depth_EndConfig[0] = modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);  
                        //}

                        if (mblnDepth_EndPlate_F_ManuallyChanged && txtDepth_EndConfig_Front.Text != "")
                        {
                            if (!mblnDepth_EndPlate_B_ManuallyChanged)
                            {
                                double pDepthF;
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    //....Retrieve from Text Box.
                                    pDepthF = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text));
                                }
                                else
                                {
                                    //....Retrieve from Text Box.
                                    pDepthF = modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);
                                }

                                //....Assign. 
                                mBearing.RadB.EndPlateCB[0].Depth = pDepthF;

                                //....Update the Depth Back.
                                Update_Depth_EndConfig(txtDepth_EndConfig_Front, txtDepth_EndConfig_Back);

                                //  Reset the state. 
                                //  ---------------
                                mblnDepth_EndPlate_F_ManuallyChanged = false;
                            }
                         
                        }
                        break;

                    case "txtDepth_EndConfig_Back":
                        //-------------------------
                        //if (mblnL_ManuallyChanged)    //PB 17JAN13
                        //{
                        //    mBearing_Radial_FP.Depth_EndConfig[1] = modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);
                        //}

                        if (mblnDepth_EndPlate_B_ManuallyChanged && txtDepth_EndConfig_Back.Text != "")
                        {
                            if (!mblnDepth_EndPlate_F_ManuallyChanged)
                            {
                                //....Retrieve from Text Box.
                                double pDepthB;
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    pDepthB = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text));
                                }
                                else
                                {
                                    pDepthB = modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);
                                }

                                //....Assign. 
                                mBearing.RadB.EndPlateCB[1].Depth = pDepthB;

                                //....Update the Depth Front.
                                Update_Depth_EndConfig(txtDepth_EndConfig_Back, txtDepth_EndConfig_Front);
                            }

                            //  Reset the state. 
                            //  ---------------
                            mblnDepth_EndPlate_B_ManuallyChanged = false;
                        }
                        break;
                }
            }

            private void Update_Depth_EndConfig(TextBox Txt_In, TextBox Txt_Out)
            //===================================================================
            {
                Double pDepth_Tot = mBearing.RadB.L - (mBearing.RadB.Pad.L +
                                                                    mBearing.RadB.AxialSealGap[0] +
                                                                    mBearing.RadB.AxialSealGap[1]);

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {

                    Double pDepth_Other = pDepth_Tot - modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(Txt_In.Text));
                    Txt_Out.Text = modMain.gProject.PNR.Unit.CEng_Met(pDepth_Other).ToString("#0.000");

                    //if (modMain.gProject.PNR.Unit.CEng_Met(pDepth_Other) >= mBearing.RadB.DEPTH_END_CONFIG_MIN_METRIC)
                    //{
                    //    Txt_Out.ForeColor = Color.Blue;
                    //}
                    //else
                    //{
                    //    Txt_Out.ForeColor = Color.Red;
                    //}
                }
                else
                {
                    Double pDepth_Other = pDepth_Tot - modMain.ConvTextToDouble(Txt_In.Text);
                    Txt_Out.Text = pDepth_Other.ToString("#0.0000");

                    //if (pDepth_Other >= mBearing.RadB.DEPTH_END_CONFIG_MIN_ENGLISH)
                    //{
                    //    Txt_Out.ForeColor = Color.Blue;
                    //}
                    //else
                    //{
                    //    Txt_Out.ForeColor = Color.Red;
                    //}
                }
            }

            private void txtDepth_EndConfig_Front_KeyDown(object sender, KeyEventArgs e)
            //==========================================================================
            {
                TextBox pTxtBox = (TextBox)sender;

                if (!pTxtBox.ReadOnly)
                    pTxtBox.ForeColor = Color.Black;

                switch (pTxtBox.Name)
                {
                    case "txtDepth_EndConfig_Front":
                        //--------------------------
                        mblnDepth_EndPlate_F_ManuallyChanged = true;

                        pTxtBox.ForeColor = Color.Black;
                        txtDepth_EndConfig_Back.ForeColor = Color.Blue;
                        break;

                    case "txtDepth_EndConfig_Back":
                        //-------------------------
                        mblnDepth_EndPlate_B_ManuallyChanged = true;

                        txtDepth_EndConfig_Front.ForeColor = Color.Blue;
                        pTxtBox.ForeColor = Color.Black;
                        break;
                }
            }

            private void txtDepth_EndConfig_TextChanged(object sender, EventArgs e)
            //======================================================================
            {
                TextBox pTxtBox = (TextBox)sender;               

                switch (pTxtBox.Name)
                {
                    case "txtDepth_EndConfig_Front":
                        //--------------------------
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            mBearing.RadB.EndPlateCB[0].Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));

                            if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                            {
                                txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                            }
                            else
                            {
                                txtL_Tot.Text = "";
                            }
                        }
                        else
                        {
                            mBearing.RadB.EndPlateCB[0].Depth = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                            {
                                txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                            }
                            else
                            {
                                txtL_Tot.Text = "";
                            }
                        }
                        break;

                    case "txtDepth_EndConfig_Back":
                        //-------------------------
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            mBearing.RadB.EndPlateCB[1].Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                            {
                                txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing.L_Tot()));
                            }
                            else
                            {
                                txtL_Tot.Text = "";
                            }
                        }
                        else
                        {
                            mBearing.RadB.EndPlateCB[1].Depth = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (mBearing.EndPlate[0].L > modMain.gcEPS && mBearing.EndPlate[1].L > modMain.gcEPS)
                            {
                                txtL_Tot.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing.L_Tot());
                            }
                            else
                            {
                                txtL_Tot.Text = "";
                            }
                        }
                        break;
                }
            }
          
   }
}
