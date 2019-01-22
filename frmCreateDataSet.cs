
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  frmCreatedataSet                       '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  11DEC18                                '
//                                                                              '
//===============================================================================
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EXCEL = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using System.IO;
//using Inventor;
//using System.Runtime.InteropServices;

namespace BearingCAD22
{
    public partial class frmCreateDataSet : Form
    {
        public frmCreateDataSet()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        //===================================================
        {
            this.Close();
        }

        //private void cmdCancel_Click(object sender, EventArgs e)
        ////======================================================
        //{
        //    this.Close();
        //}

        private void cmdBrowse_FilePath_Project_Click(object sender, EventArgs e)
        //========================================================================
        {
            folderBrowserDialog1.SelectedPath = modMain.gFiles.File_InputPath;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFilePath_Project.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void cmdCreateParameterList_Click(object sender, EventArgs e)
        //===================================================================
        {
            if (txtFilePath_Project.Text != "")
            {
                //MessageBox.Show("All open Excel files will be closed automatically.\nPlase save before proceeding.", "Warning: Excel Files!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CloseInventor();
                CloseExcelFiles();
                string pFileName = txtFilePath_Project.Text;

                Cursor = Cursors.WaitCursor;
                //CreateParameter_Driver();
                modMain.gFiles.Write_Parameter_Complete(modMain.gProject, pFileName, true);
                Copy_Inventor_Model_Files(modMain.gProject, modMain.gFiles, txtFilePath_Project.Text);
                Cursor = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Please set proper Output File Path", "Error: Output File Path!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFilePath_Project.Focus();
            }
        }

       

       

        
        private void Write_Parameter_Complete_Thrust_Back(clsProject Project_In, clsEndPlate EndPlate_In, EXCEL.Worksheet WorkSheet_In)
        //========================================================================================================================================
        {
            //EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

            //int pRowCount = pExcelCellRange.Rows.Count;
            //string pVarName = "";
            //Double pConvF = 1;
            //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
            //{
            //    pConvF = 25.4;
            //}

            //for (int i = 3; i <= pRowCount; i++)
            //{
            //    if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
            //    {
            //        pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

            //        switch (pVarName)
            //        {
            //            case "EndPlate[1].Mat.Base":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Mat.Base;
            //                break;

            //            case "EndPlate[1].Mat.Lining":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Mat.Lining;
            //                break;

            //            case "EndPlate[1].LiningT.Face":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.LiningT.Face * pConvF;
            //                break;

            //            case "EndPlate[1].LiningT.ID":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LiningT.ID * pConvF);
            //                break;

            //            case "EndPlate[1].DirectionType":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.DirectionType.ToString();
            //                break;

            //            case "EndPlate[1].DBore()":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.DBore() * pConvF);
            //                break;

            //            case "EndPlate[1].LandL":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LAND_L * pConvF);
            //                break;

            //            case "EndPlate[1].L":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.L * pConvF);
            //                break;

            //            case "EndPlate[1].LFlange":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LFlange * pConvF);
            //                break;

            //            case "EndPlate[1].DimStart()":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.DimStart() * pConvF);
            //                break;

            //            case "EndPlate[1].FaceOff_Assy":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FaceOff_Assy * pConvF);
            //                break;

            //            case "EndPlate[1].BackRelief.Reqd":
            //                String pReqd = "N";
            //                if (ThrustTL_In.BackRelief.Reqd)
            //                {
            //                    pReqd = "Y";
            //                }

            //                WorkSheet_In.Cells[i, 4] = pReqd;
            //                break;

            //            case "EndPlate[1].BackRelief.D":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.D * pConvF);
            //                break;

            //            case "EndPlate[1].BackRelief.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.Depth * pConvF);
            //                break;

            //            case "EndPlate[1].BackRelief.Fillet":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.Fillet * pConvF);
            //                break;

            //            case "EndPlate[1].Pad_Count":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Pad_Count;
            //                break;

            //            case "EndPlate[1].PadD[1]":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.PadD[1] * pConvF);
            //                break;

            //            case "EndPlate[1].PadD[0]":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.PadD[0] * pConvF);
            //                break;

            //            case "EndPlate[1].Taper.Depth_OD":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Taper.Depth_OD * pConvF);
            //                break;

            //            case "EndPlate[1].Taper.Depth_ID":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Taper.Depth_ID * pConvF);
            //                break;

            //            case "EndPlate[1].Taper.Angle":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Taper.Angle;
            //                break;

            //            case "EndPlate[1].FeedGroove.Type":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.FeedGroove.Type;
            //                break;

            //            case "EndPlate[1].FeedGroove.Wid":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Wid * pConvF);
            //                break;

            //            case "EndPlate[1].FeedGroove.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Depth * pConvF);
            //                break;

            //            case "EndPlate[1].FeedGroove.DBC":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.DBC * pConvF);
            //                break;

            //            case "EndPlate[1].FeedGroove.Dist_Chamf":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Dist_Chamf * pConvF);
            //                break;

            //            case "EndPlate[1].WeepSlot.Type":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.WeepSlot.Type;
            //                break;

            //            case "EndPlate[1].WeepSlot.Wid":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.WeepSlot.Wid * pConvF);
            //                break;

            //            case "EndPlate[1].WeepSlot.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.WeepSlot.Depth * pConvF);
            //                break;

            //            case "EndPlate[1].Shroud.Ro":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Shroud.Ro * pConvF);
            //                break;

            //            case "EndPlate[1].Shroud.Ri":
            //                WorkSheet_In.Cells[i, 4] = (ThrustTL_In.Shroud.Ri * pConvF);
            //                break;

            //        }
            //    }
            //}
        }


        //private void Write_Parameter_Complete_Accessories(clsProject Project_In, clsAccessories Accessories_In,
        //                                                  EXCEL.Worksheet WorkSheet_In)
        ////======================================================================================================
        //{
        //    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

        //    int pRowCount = pExcelCellRange.Rows.Count;
        //    string pVarName = "";
        //    Double pConvF = 1;
        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
        //    {
        //        pConvF = 25.4;
        //    }

        //    //....EndPlate: Seal
        //    clsSeal[] mEndSeal = new clsSeal[2];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
        //        {
        //            mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
        //        }
        //    }

        //    for (int i = 3; i <= pRowCount; i++)
        //    {
        //        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
        //        {
        //            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

        //            switch (pVarName)
        //            {
        //                case "Bearing.TempSensor.Exists":
        //                    String pTemp_Exists = "";
        //                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
        //                    {
        //                        pTemp_Exists = "Y";
        //                    }
        //                    else
        //                    {
        //                        pTemp_Exists = "N";
        //                    }
        //                    WorkSheet_In.Cells[i, 4] = pTemp_Exists;
        //                    break;

        //                case "Bearing.TempSensor.Count":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count;
        //                    break;

        //                case "Accessories.TempSensor.Name":
        //                    WorkSheet_In.Cells[i, 4] = Accessories_In.TempSensor.Name.ToString();
        //                    break;

        //                case "Accessories.TempSensor.Type":
        //                    WorkSheet_In.Cells[i, 4] = Accessories_In.TempSensor.Type.ToString();
        //                    break;

        //                case "Bearing.TempSensor.D":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D * pConvF;
        //                    break;

        //                case "Bearing.TempSensor.CanLength":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength * pConvF;
        //                    break;

        //                case "Bearing.TempSensor.Loc":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc.ToString();
        //                    break;

        //                case "Bearing.TempSensor.Depth":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth * pConvF;
        //                    break;

        //                case "Bearing.TempSensor.AngStart":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart;
        //                    break;

        //                case "Bearing.Pad.AngBetween()":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.AngBetween();
        //                    break;

        //                case "EndSeal[0].TempSensor_D_ExitHole":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].TempSensor_D_ExitHole * pConvF;
        //                    }

        //                    break;

        //                case "EndSeal[0].TempSensor_DBC_Hole()":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].TempSensor_DBC_Hole() * pConvF;
        //                    }
        //                    break;

        //                case "EndSeal[1].TempSensor_D_ExitHole":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].TempSensor_D_ExitHole * pConvF;
        //                    }
        //                    break;

        //                case "EndSeal[1].TempSensor_DBC_Hole()":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].TempSensor_DBC_Hole() * pConvF;
        //                    }
        //                    break;

        //                case "Accessories.WireClip.Supplied":
        //                    String pSupplied = "";
        //                    if (Accessories_In.WireClip.Supplied)
        //                    {
        //                        pSupplied = "Y";
        //                    }
        //                    else
        //                    {
        //                        pSupplied = "N";
        //                    }
        //                    WorkSheet_In.Cells[i, 4] = pSupplied;
        //                    break;

        //                case "Accessories.WireClip.Count":
        //                    WorkSheet_In.Cells[i, 4] = Accessories_In.WireClip.Count;
        //                    break;

        //                case "Accessories.WireClip.Size":
        //                    WorkSheet_In.Cells[i, 4] = Accessories_In.WireClip.Size.ToString();
        //                    break;

        //                //....Front
        //                case "EndSeal[0].WireClipHoles.DBC":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.DBC * pConvF;
        //                    }
        //                    break;

        //                case "EndSeal[0].Unit.System":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].Unit.System.ToString();
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.Screw_Spec.D_Desig":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.Screw_Spec.D_Desig;
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.Screw_Spec.Pitch":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.Screw_Spec.Pitch;
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.ThreadDepth":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.ThreadDepth * pConvF;
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.AngStart":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.AngStart;
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.AngOther(0)":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[0].WireClipHoles.AngOther.Length > 0)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.AngOther[0];
        //                        }
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.AngOther(1)":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[0].WireClipHoles.AngOther.Length > 1)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.AngOther[1];
        //                        }
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.AngOther(2)":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[0].WireClipHoles.AngOther.Length > 2)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.AngOther[2];
        //                        }
        //                    }
        //                    break;

        //                //....Back
        //                case "EndSeal[1].WireClipHoles.DBC":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.DBC * pConvF;
        //                    }
        //                    break;

        //                case "EndSeal[1].Unit.System":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].Unit.System.ToString();
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.Screw_Spec.D_Desig":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.Screw_Spec.D_Desig;
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.Screw_Spec.Pitch":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.Screw_Spec.Pitch;
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.ThreadDepth":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.ThreadDepth * pConvF;
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.AngStart":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.AngStart;
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.AngOther(0)":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[1].WireClipHoles.AngOther.Length > 0)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.AngOther[0];
        //                        }
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.AngOther(1)":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[1].WireClipHoles.AngOther.Length > 1)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.AngOther[1];
        //                        }
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.AngOther(2)":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[1].WireClipHoles.AngOther.Length > 2)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.AngOther[2];
        //                        }
        //                    }
        //                    break;

        //            }
        //        }
        //    }
        //}        


        private void Copy_Inventor_Model_Files(clsProject Project_In, clsFiles Files_In, String FilePath_In)
        //===================================================================================================
        {
            try
            {
                //  MODEL FILES.
                //  -----------
                //
                ////....Complete Assy: 
                ////
                //string pFileName_CompleteAssy = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Complete);

                //if (System.IO.File.Exists(pFileName_CompleteAssy))
                //    System.IO.File.Delete(pFileName_CompleteAssy);

                //System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Complete, pFileName_CompleteAssy);


                ////....Radial Assy:
                ////
                //string pFileName_RadialAssy = FilePath_In + "\\" + Path.GetFileName(Files_In.FileTitle_Template_Inventor_Radial_Assy);

                //if (File.Exists(pFileName_RadialAssy))
                //    File.Delete(pFileName_RadialAssy);

                //File.Copy(Files_In.FileTitle_Template_Inventor_Radial_Assy, pFileName_RadialAssy);


                //....Radial:
                //
                string pFileName_Radial = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Radial);

                if (System.IO.File.Exists(pFileName_Radial))
                    System.IO.File.Delete(pFileName_Radial);
                System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Radial, pFileName_Radial);


                ////....Seal Front:
                ////
                //if (Project_In.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                //{
                //    string pFileName_Seal_Front = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Seal_Front);
                //    if (System.IO.File.Exists(pFileName_Seal_Front))
                //        System.IO.File.Delete(pFileName_Seal_Front);
                //    System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Seal_Front, pFileName_Seal_Front);
                //}

                ////....Seal Back:
                ////
                //if (Project_In.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                //{
                //    string pFileName_Seal_Back = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Seal_Back);
                //    if (System.IO.File.Exists(pFileName_Seal_Back))
                //        System.IO.File.Delete(pFileName_Seal_Back);
                //    System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Seal_Back, pFileName_Seal_Back);
                //}

                ////....Thrust Bearing Front: 
                ////
                //if (Project_In.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
                //{
                //    string pFileName_Thrust_Front = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Thrust_Front);
                //    if (System.IO.File.Exists(pFileName_Thrust_Front))
                //        System.IO.File.Delete(pFileName_Thrust_Front);
                //    System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Thrust_Front, pFileName_Thrust_Front);
                //}

                ////....Thrust Bearing Back: 
                ////
                //if (Project_In.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                //{
                //    string pFileName_Thrust_Back = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Thrust_Back);
                //    if (System.IO.File.Exists(pFileName_Thrust_Back))
                //        System.IO.File.Delete(pFileName_Thrust_Back);
                //    System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Thrust_Back, pFileName_Thrust_Back);
                //}
            }

            catch (Exception ex)
            {
                MessageBox.Show("Unable to copy Inventor File.Please close Inventor Files.");
            }
        }

        public void CloseExcelFiles()
        //===========================      
        {
            Process[] pProcesses = Process.GetProcesses();

            try
            {
                foreach (Process p in pProcesses)
                    if (p.ProcessName == "EXCEL")
                        p.Kill();
            }
            catch (Exception pEXP)
            {

            }
        }

        private void cmdOpen_CompleteAssy_Click(object sender, EventArgs e)
        //==================================================================
        {
            if (txtFilePath_Project.Text != "")
            {
                CloseInventor();
                CloseExcelFiles();
                string pFileName = txtFilePath_Project.Text;

                Cursor = Cursors.WaitCursor;
                //CreateParameter_Driver();
                modMain.gFiles.Write_Parameter_Complete(modMain.gProject, pFileName, false);
                Copy_Inventor_Model_Files(modMain.gProject, modMain.gFiles, txtFilePath_Project.Text);                

                Process.Start(txtFilePath_Project.Text);
                Cursor = Cursors.Default;                
            }
            else
            {
                MessageBox.Show("Please set proper Output File Path", "Error: Output File Path!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFilePath_Project.Focus();
            }
        }

        public void CloseInventor()
        //===========================      
        {
            Process[] pProcesses = Process.GetProcesses();

            try
            {
                foreach (Process p in pProcesses)
                    if (p.ProcessName == "Inventor")
                        p.Kill();
            }
            catch (Exception pEXP)
            {

            }
        }

        
    }
}
