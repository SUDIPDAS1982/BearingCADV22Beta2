
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmOperCond                            '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  21NOV18                                '
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
//.....Designer changed.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Data.SqlClient;
using EXCEL = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace BearingCAD22
{
    public partial class frmOpCond : Form
    {
        #region "MEMBER VARIABLE"
        //***********************

            private clsOpCond mOpCond = new clsOpCond();

            Boolean mblntxtPressEng_Entered = false;
            Boolean mblntxtPressMet_Entered = false;

            Boolean mblntxtTempEng_Entered = false;
            Boolean mblntxtTempMet_Entered = false;

            //GroupBox Array
            //-------------         

            private TextBox[] mTxtThrust_Load_Design;
            private Label[] mLblStaticLoad_Thrust;
           
        #endregion


        #region "FORM CONSTRUCTOR & RELATED ROUTINES:"
        //********************************************

            public frmOpCond()
            //====================
            {
                InitializeComponent();

                //.....Initialize Lube type.
                //LoadOilSupply_Lube_Type();

                //.....Initialize Oilsupply Type.
                //LoadOilSupply_Type();

                //////.....Initialize Temp Labels
                ////lblTempDegF.Text = Convert.ToString((char)176);
                ////lblTempDegC.Text = Convert.ToString((char)176);

                mTxtThrust_Load_Design = new TextBox[] { txtThrust_Load_Front, txtThrust_Load_Back };
                mLblStaticLoad_Thrust = new Label[] { lblStaticLoad_Thrust_Front, lblStaticLoad_Thrust_Back };
            }

            private void LoadOilSupply_Lube_Type()
            //====================================
            {
                ////BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                //////....Lube
                ////var pQryLube = (from pRec in pBearingDBEntities.tblData_Lube orderby pRec.fldType ascending select pRec).ToList();
                ////cmbLube_Type.Items.Clear();
                ////if (pQryLube.Count() > 0)
                ////{
                ////    for (int i = 0; i < pQryLube.Count; i++)
                ////    {
                ////        cmbLube_Type.Items.Add(pQryLube[i].fldType);
                ////    }
                ////    cmbLube_Type.SelectedIndex = -1;
                ////}
                //String pFileName = "D:\\BearingCAD\\Program Data Files\\Oil_Data_03OCT18.xlsx";

                //int pLubeType_RecCount = modMain.gDB.PopulateCmbBox(cmbLube_Type, modMain.gFiles.FileTitle_EXCEL_OilData, "[Lube$]", "Type", "", true);
                          
            }

            //private void LoadOilSupply_Type()
            ////===============================
            //{
            //    cmbOilSupply_Type.Items.Clear();
            //    cmbOilSupply_Type.Items.Add("Pressurized");
            //    cmbOilSupply_Type.Items.Add("Flooded Bath");
            //    //cmbOilSupply_Type.SelectedIndex = -1;
            //    cmbOilSupply_Type.SelectedIndex = 0;        //BG 04OCT12
            //}
            

        #endregion


        #region "FORM EVENT ROUTINES: "
        //*****************************

            private void frmOperCond_Load(object sender, EventArgs e)   
            //========================================================
            {

                //....Reset Diff Control value.
                ResetControlVal();

                //....Set Local Object.                    
                SetLocalObject();

                //....DisplayData.
                DisplayData();

                //....Set Controls
                SetControl();
               
            }

            private void SetControl()
            //=======================
            {     
                if (modMain.gProject != null)
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
                    {
                        grpStaticLoad.Text = "Static Load (" + "lbf):";
                        lblPressUnit.Text = "psig";
                        lblTempUnit.Text = Convert.ToString((char)176) + "F";
                        //lblFlowReqd_Unit.Text = "gpm";
                    }
                    else
                    {
                        grpStaticLoad.Text = "Static Load (" + "kN):";
                        lblPressUnit.Text = "MPa";
                        lblTempUnit.Text = Convert.ToString((char)176) + "C";
                        //lblFlowReqd_Unit.Text = "LPM";
                    }                    

                    lblStaticLoad_Thrust.Enabled = false;
                    lblStaticLoad_Thrust_Front.Enabled = false;
                    lblStaticLoad_Thrust_Back.Enabled = false;

                    for (int i = 0; i < 2; i++)
                    {
                        if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[i].TLTB.Exists == true)
                        {
                            lblStaticLoad_Thrust.Enabled = true;
                            mTxtThrust_Load_Design[i].Enabled = true;
                            mLblStaticLoad_Thrust[i].Enabled = true;
                        }
                        else
                        {
                            mTxtThrust_Load_Design[i].Enabled = false;
                        }
                    }
                }
              
            }  
            

            private void SetLocalObject()
            //===========================
            {
                mOpCond = (clsOpCond)((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.Clone();
            }

            private void ResetControlVal()      
            //============================
            {
                const string pcBlank = "";  

                //  Speed
                //  -----                    
                    txtSpeed_Design.Text = pcBlank;               

   
                //  Load
                //  ----
                    txtRadial_Load.Text = pcBlank;
              

                //  Load Angle
                //  ----------
                    txtRadial_LoadAng.Text = pcBlank;

                //  Load: Thrust
                //  --------------

                    //....Front       
                    txtThrust_Load_Front.Text = pcBlank;
            
                    //....Back
                    txtThrust_Load_Back.Text = pcBlank;  

            }

            

            private void DisplayData()
            //========================
            {
                txtSpeed_Design.Text = modMain.ConvIntToStr(mOpCond.Speed);

                //  Directionality
                //  --------------
                //if (mOpCond.Rot_Directionality.ToString() == "Bi")
                //    optRot_Directionality_Bi.Checked = true;
                //else if (mOpCond.Rot_Directionality.ToString() == "Uni")
                //    optRot_Directionality_Uni.Checked = true;

                //  Load: Radial
                //  -------------
                txtRadial_Load.Text = modMain.ConvDoubleToStr(mOpCond.Radial_Load, "#0.#");


                //  Load Angle
                //  ----------
                txtRadial_LoadAng.Text = modMain.ConvDoubleToStr(mOpCond.Radial_LoadAng_Casing_SL, "#0.#");

                ////  Load: Thrust
                ////  ------------    
                //if (modMain.gProject.Product.EndConfig[0].Type == clsEndPlate.eType.TL_TB)
                //{
                //    mTxtThrust_Load_Design[0].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range[0], "#0.00");
                  
                //}

                //if (modMain.gProject.Product.EndConfig[1].Type == clsEndPlate.eType.TL_TB)
                //{
                //    mTxtThrust_Load_Design[1].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range[1], "#0.00");
                //}

                txtLube_Type.Text = mOpCond.OilSupply.Lube_Type;

                //....Flow Reqd
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    //txtFlowReqd_gpm_Radial.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CFac_GPM_EngToMet(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd), "#0.00");
                    txtFlowReqd.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CFac_GPM_EngToMet(((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd_Unit, ((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd), "#0.00");
                }
                else
                {
                    //txtFlowReqd_gpm_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd, "#0.00");
                    txtFlowReqd.Text = modMain.ConvDoubleToStr(((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd, "#0.00");
                }
                lblFlowReqd_Unit.Text = ((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd_Unit;
                
                //cmbOilSupply_Type.Text = mOpCond.OilSupply.Flow_Type;
                txtOilSupply_Press.Text = modMain.ConvDoubleToStr(mOpCond.OilSupply.Press, "#0.#");
                txtOilSupply_Temp.Text = modMain.ConvDoubleToStr(mOpCond.OilSupply.Temp, "#0.#");
            }

        #endregion


        #region "CONTROL EVENT RELATED ROUTINE:"
            //**********************************

            #region"TEXT BOX RELATED ROUTINE:"
            //--------------------------------

                #region "LOAD RELATED ROUTINE"  

                    private void txtLoadAngle_TextChanged(object sender, EventArgs e)
                    //================================================================
                    {
                        mOpCond.Radial_LoadAng_Casing_SL = modMain.ConvTextToDouble(txtRadial_LoadAng.Text);
                    }


                    private void OptionButton_CheckedChanged(object sender, EventArgs e)
                    //===================================================================   //BG 23APR09
                    {
                        RadioButton pOptButton = (RadioButton)sender;

                        switch (pOptButton.Name)
                        {
                            //case "optDirection_Uni":
                            //    //=================
                            //    if (pOptButton.Checked)
                            //        modMain.gOpCond.Rot_Directionality =
                            //                (clsOpCond.eRotDirectionality)Enum.Parse
                            //                    (typeof(clsOpCond.eRotDirectionality), "Uni");
                            //    break;

                            //case "optDirection_Bi":
                            //    //=================
                            //    if (pOptButton.Checked)
                            //        modMain.gOpCond.Rot_Directionality =
                            //                (clsOpCond.eRotDirectionality)Enum.Parse
                            //                (typeof(clsOpCond.eRotDirectionality), "Bi");
                            //    break;

                        }
                    }
                  

                #endregion              
                

            #endregion

            #region "COMBO BOX RELATED ROUTINES"

                //private void cmbLube_Type_SelectedIndexChanged(object sender, EventArgs e)
                ////==========================================================================
                //{
                //    mOpCond.OilSupply_Lube_Type = cmbLube_Type.Text; 
                //}

                //private void cmbOilSupply_Type_SelectedIndexChanged(object sender, EventArgs e)
                ////==============================================================================
                //{
                //    cmbOilSupply_Type.SelectedIndex = 0;
                //    modMain.gOpCond.OilSupply_Flow_Type = cmbOilSupply_Type.Text;  
                //}

            #endregion

            #region "COMMAND BUTTON EVENT ROUTINES"
                    //-------------------------------------

                private void cmdButtons_Click(object sender, System.EventArgs e)
                //==============================================================
                {
                    Button pcmdButton = (Button)sender;

                    switch (pcmdButton.Name)
                    {
                        case "cmdOK":
                            //-------
                            CloseForm();
                            break;

                        case "cmdCancel":
                            //----------
                            this.Hide(); 
                            break;
                    }
                }
                
                private void CloseForm()
                //======================
                {
                    SaveData();                   
                    this.Hide();

                    modMain.gfrmPerformDataBearing.ShowDialog();                   
                }

                private void SaveData()
                //=====================
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.Speed = modMain.ConvTextToInt(txtSpeed_Design.Text);
                   

                    //if (optRot_Directionality_Bi.Checked)
                    //{
                    //    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.Rot_Directionality =
                    //         (clsOpCond.eRotDirectionality)Enum.Parse
                    //            (typeof(clsOpCond.eRotDirectionality), "Bi");
                    //}
                    //else
                    //{
                    //    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.Rot_Directionality =
                    //         (clsOpCond.eRotDirectionality)Enum.Parse
                    //            (typeof(clsOpCond.eRotDirectionality), "Uni");
                    //}

                    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.Radial_Load = modMain.ConvTextToDouble(txtRadial_Load.Text);
                    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.Radial_LoadAng_Casing_SL = modMain.ConvTextToDouble(txtRadial_LoadAng.Text);

                    ////  Load: Thrust
                    ////  ------------    
                    //if (modMain.gProject.Product.EndConfig[0].Type == clsEndPlate.eType.TL_TB)
                    //{
                    //    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.Thrust_Load_Range[0] = modMain.ConvTextToDouble(txtThrust_Load_Front.Text);                       

                    //}

                    //if (modMain.gProject.Product.EndConfig[1].Type == clsEndPlate.eType.TL_TB)
                    //{
                    //    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.Thrust_Load_Range[1] = modMain.ConvTextToDouble(txtThrust_Load_Back.Text);  
                    //}

                    //((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.OilSupply_Lube_Type = cmbLube_Type.Text;
                    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.OilSupply_Lube_Type = txtLube_Type.Text;
                    //((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.OilSupply_Flow_Type = cmbOilSupply_Type.Text;
                    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.OilSupply_Press = modMain.ConvTextToDouble(txtOilSupply_Press.Text); 
                    ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.OilSupply_Temp = modMain.ConvTextToDouble(txtOilSupply_Temp.Text);

                   
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        if (((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd_Unit == "LPM")
                        {
                            ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.OilSupply_Flow_Reqd = modMain.gProject.PNR.Unit.CFac_LPM_MetToEng(modMain.ConvTextToDouble(txtFlowReqd.Text));
                            ((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd = modMain.gProject.PNR.Unit.CFac_LPM_MetToEng(modMain.ConvTextToDouble(txtFlowReqd.Text));
                        }
                        else if (((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd_Unit == "LPS")
                        {
                            ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.OilSupply_Flow_Reqd = modMain.gProject.PNR.Unit.CFac_LPS_MetToEng(modMain.ConvTextToDouble(txtFlowReqd.Text));
                            ((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd = modMain.gProject.PNR.Unit.CFac_LPS_MetToEng(modMain.ConvTextToDouble(txtFlowReqd.Text));
                        }
                    }
                    else
                    {
                        ((clsJBearing)modMain.gProject.PNR.Bearing).OpCond.OilSupply_Flow_Reqd = modMain.ConvTextToDouble(txtFlowReqd.Text);
                        ((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd = modMain.ConvTextToDouble(txtFlowReqd.Text);
                    }

                }

                //private void SaveToDB_OpCond(clsProject Project_In, clsOpCond OpCond_In)
                ////======================================================================        
                //{
                //    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                   
                //    //OpCond_In.OilSupply_Type = "Pressurized";
                //    //OpCond_In.OilSupply_Lube_Type = "ISO 32";
                //    int pProjectCount = (from pRec in pBearingDBEntities.tblProject_OpCond where pRec.fldPartNo == Project_In.PNR.No select pRec).Count();

                //    if (pProjectCount > 0)
                //    {
                //        //....Record already exists Update record
                //        var pProject_OpCond = (from pRec in pBearingDBEntities.tblProject_OpCond where pRec.fldPartNo == Project_In.PNR.No select pRec).First();
                //        pProject_OpCond.fldPartNo = Project_In.PNR.No;
                       
                //        pProject_OpCond.fldSpeed = OpCond_In.Speed;                        
                //        pProject_OpCond.fldRot_Directionality = OpCond_In.Rot_Directionality.ToString();
                //        pProject_OpCond.fldRadial_Load = (Decimal)OpCond_In.Radial_Load;                       
                //        pProject_OpCond.fldRadial_LoadAng = (Decimal)OpCond_In.Radial_LoadAng;
                //        pProject_OpCond.fldThrust_Load_Front = (Decimal)OpCond_In.Thrust_Load_Range[0];
                //        pProject_OpCond.fldThrust_Load_Back = (Decimal)OpCond_In.Thrust_Load_Range[1];
                //        pProject_OpCond.fldOilSupply_Lube_Type = OpCond_In.OilSupply.Lube_Type;
                //        pProject_OpCond.fldFlowReqd_gpm = (Decimal)OpCond_In.OilSupply.Reqd_Flow;

                //        pProject_OpCond.fldOilSupply_Type = OpCond_In.OilSupply.Type;
                //        pProject_OpCond.fldOilSupply_Press = (Decimal)OpCond_In.OilSupply.Press;
                //        pProject_OpCond.fldOilSupply_Temp = (Decimal)OpCond_In.OilSupply.Temp;

                //        pBearingDBEntities.SaveChanges();
                //    }
                //    else
                //    {
                //        //....New Record
                //        tblProject_OpCond pProject_OpCond = new tblProject_OpCond();
                //        pProject_OpCond.fldSpeed = OpCond_In.Speed;
                //        pProject_OpCond.fldRot_Directionality = OpCond_In.Rot_Directionality.ToString();
                //        pProject_OpCond.fldRadial_Load = (Decimal)OpCond_In.Radial_Load;
                //        pProject_OpCond.fldRadial_LoadAng = (Decimal)OpCond_In.Radial_LoadAng;
                //        pProject_OpCond.fldThrust_Load_Front = (Decimal)OpCond_In.Thrust_Load_Range[0];
                //        pProject_OpCond.fldThrust_Load_Back = (Decimal)OpCond_In.Thrust_Load_Range[1];
                //        pProject_OpCond.fldOilSupply_Lube_Type = OpCond_In.OilSupply.Lube_Type;
                //        pProject_OpCond.fldFlowReqd_gpm = (Decimal)OpCond_In.OilSupply.Reqd_Flow;

                //        pProject_OpCond.fldOilSupply_Type = OpCond_In.OilSupply.Type;
                //        pProject_OpCond.fldOilSupply_Press = (Decimal)OpCond_In.OilSupply.Press;
                //        pProject_OpCond.fldOilSupply_Temp = (Decimal)OpCond_In.OilSupply.Temp;

                //        pBearingDBEntities.AddTotblProject_OpCond(pProject_OpCond);
                //        pBearingDBEntities.SaveChanges();
                //    }
                //}

      
            #endregion

                //private void cmdImport_XLKMC_Click(object sender, EventArgs e)
                ////============================================================
                //{
                //    Import_Analytical_Data();
                //}

                //private void Import_Analytical_Data()
                ////===================================
                //{
                //    string pExcelFileName = "";
                //    openFileDialog1.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
                //    openFileDialog1.FilterIndex = 1;
                //    openFileDialog1.InitialDirectory = "C:\\";
                //    openFileDialog1.Title = "Open";
                //    openFileDialog1.FileName = " ";

                //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                //    {
                //        Cursor = Cursors.WaitCursor;
                //        pExcelFileName = openFileDialog1.FileName;

                //        //....EndConfig: Seal
                //        clsSeal[] mEndSeal = new clsSeal[2];
                //        for (int i = 0; i < 2; i++)
                //        {
                //            if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                //            {
                //                mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                //            }
                //        }

                //        ////modMain.gEXCEL_Analysis.Retrieve_Params_XLRadial(pExcelFileName, mOpCond, (clsBearing_Radial_FP)modMain.gProject.Product.Bearing, mEndSeal);

                //        for (int i = 0; i < 2; i++)
                //        {
                //            if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                //            {
                //                modMain.gProject.Product.EndPlate[i] = (clsSeal)mEndSeal[i].Clone();
                //            }
                //        }

                //        Cursor = Cursors.Default;
                        

                //        //SqlConnection pConnection = new SqlConnection();
                //        //string pstrQuery = "Select * from " + TableName_In;
                //        //SqlDataReader pDR = modMain.gDB.GetDataReader(pstrQuery, ref pConnection);

                //        //List<string> pParameterName = new List<string>();
                //        //List<string> pExcelSheetName = new List<string>();
                //        //List<string> pCellName = new List<string>();
                //        //List<string> pCellRange_Start = new List<string>();
                //        //List<string> pCellRange_End = new List<string>();

                //        //while (pDR.Read())
                //        //{
                //        //    pParameterName.Add(Convert.ToString(pDR["fldParameter"]));
                //        //    pExcelSheetName.Add(Convert.ToString(pDR["fldWorkSheet"]));
                //        //    pCellName.Add(Convert.ToString(pDR["fldCellNo"]));
                //        //    pCellRange_Start.Add(Convert.ToString(pDR["fldCellNo_Start"]));
                //        //    pCellRange_End.Add(Convert.ToString(pDR["fldCellNo_End"]));

                //        //}
                //        //pDR.Close();

                //        //if (mblnCmd_XLKMC)
                //        //{
                //        //    modMain.gEXCEL_Analysis.Retrieve_Params_XLKMC(pExcelFileName, pParameterName, pExcelSheetName, pCellName, pCellRange_Start, pCellRange_End,
                //        //                                                  mOpCond, (clsBearing_Radial_FP)modMain.gProject.Product.Bearing);
                //        //}

                //        //int pEndConfig_Pos = 0;
                //        //if (mblnCmd_XLThrust_Front)
                //        //{
                //        //    pEndConfig_Pos = 0;
                //        //    modMain.gEXCEL_Analysis.Retrieve_Params_XLTHRUST(pExcelFileName, pParameterName, pExcelSheetName, pCellName, pCellRange_Start, pCellRange_End,
                //        //                                                     mOpCond, pEndConfig_Pos, (clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[pEndConfig_Pos]);
                //        //}
                //        //else if (mblnCmd_XLThrust_Back)
                //        //{
                //        //    pEndConfig_Pos = 1;
                //        //    modMain.gEXCEL_Analysis.Retrieve_Params_XLTHRUST(pExcelFileName, pParameterName, pExcelSheetName, pCellName, pCellRange_Start, pCellRange_End,
                //        //                                                     mOpCond, pEndConfig_Pos, (clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[pEndConfig_Pos]);
                //        //}

                //        DisplayData();
                //    }
                //}
                        
                        
        #endregion

                private void txtLube_Type_TextChanged(object sender, EventArgs e)
                //================================================================
                {
                    mOpCond.OilSupply_Lube_Type = txtLube_Type.Text; 
                }

                private void cmdClose_Click(object sender, EventArgs e)
                //=====================================================
                {
                    SaveData();
                    this.Hide();
                }

    }
}
