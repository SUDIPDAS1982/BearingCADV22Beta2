
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmMain                                '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  11DEC18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
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
using System.Diagnostics;
using System.IO;

namespace BearingCAD22
{
    public partial class frmMain : Form
    {       
        int mTop_CmdButton;
        int mHt_cmdButton = 32 + 2;
        Boolean mblnExitFromBtn = false;

        #region "FORM CONSTRUCTOR RELATED ROUTINE"
        //----------------------------------------

            public frmMain()
            //===============
            {
                InitializeComponent();
            }

        #endregion


        #region "FORM RELATED ROUTINE"
        //----------------------------

            private void frmMain_Resize(object sender, EventArgs e)
            //=====================================================
            {
                Form pForm = (Form)sender;
                UpdateDisplay(pForm);
            }

            private void frmMain_Load(object sender, EventArgs e)
            //===================================================
            {             
                mTop_CmdButton = cmdRadialBearingData.Top + mHt_cmdButton;

                Form pForm = (Form)sender;
                UpdateDisplay(pForm);            
            }

            private void frmMain_Activated(object sender, EventArgs e)
            //========================================================
            {
                Form pForm = (Form)sender;
                UpdateDisplay(pForm);            
            }

            public void UpdateDisplay(Form Form_In)
            //======================================   
            {
                //------------------------------------------------------------
                //....Status Bar Panels: 

                Int32 pWidth = Form_In.Width  / 3;
                SBar1.Width = Form_In.Width;

                //SBpanel1.Width = pWidth ;
                SBpanel2.Width = pWidth ;
                SBpanel3.Width = pWidth ;
                //SBpanel4.Width = pWidth ;
                SBpanel5.Width = (Form_In.Width - (2 * pWidth));

                //SBpanel1.Text = modMain.gUser.Name + " (" + modMain.gUser.Initials + ")";
                if (modMain.gProject != null)
                {
                    //SBpanel2.Text = "Project No: " + modMain.gProject.No;     //BG 10MAY13
                    SBpanel2.Text = modMain.gProject.SOL.SONo + "-" + modMain.gProject.SOL.LineNo;
                    SBpanel3.Text = "P/N: " + modMain.gProject.PNR.No;
                } 
                else
                {
                    SBpanel2.Text = "";
                    SBpanel3.Text = "P/N: ";
                }    
                //SBpanel4.Text = modMain.gUser.Role;
                ////SBpanel5.Text = DateTime.Today.DayOfWeek.ToString() + ", " +
                ////                DateTime.Today.ToString(" MMM dd, yyyy");
                SBpanel5.Text = DateTime.Today.ToString(" MMM dd, yyyy");       //AES 14SEP18

                //------------------------------------------------------------------

                //....Form caption.
                this.Text = modMain.gcstrProgramName + " " + modMain.gcstrVersionNo +
                            "                                           Main Form";

                if (modMain.gProject != null)
                {
                    UpdateDisplay_Project();
                }
            }

            private void UpdateDisplay_Project()
            //==================================
            {
                cmdEndSealData.Visible = true;
                cmdEndSealDesgnDetail.Visible = true;
                cmdEndSealData.Top = mTop_CmdButton;

                if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].TLTB.Exists == true ||
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].TLTB.Exists == true)
                {
                    cmdThrustBearingData.Visible = true;
                    cmdThrustBearingDesgnDetail.Visible = true;

                    cmdThrustBearingData.Top = cmdEndSealData.Top + mHt_cmdButton;
                    SetPosition_CmdButtons(cmdThrustBearingData.Top);
                }
                else
                {
                    SetPosition_CmdButtons(cmdEndSealData.Top);
                    cmdThrustBearingData.Visible = false;
                    cmdThrustBearingDesgnDetail.Visible = false;
                }

                ////cmdCreateGCodes.Enabled = true;
                //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal &&
                //    modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                //{
                //    cmdEndSealData.Visible = true;
                //    cmdEndSealDesgnDetail.Visible = true;
                //    cmdThrustBearingData.Visible = true;
                //    cmdThrustBearingDesgnDetail.Visible = true;
                //    //cmdCreateGCodes.Visible = true;

                //    cmdEndSealData.Top = mTop_CmdButton;
                //    cmdThrustBearingData.Top = cmdEndSealData.Top + mHt_cmdButton;

                //    SetPosition_CmdButtons(cmdThrustBearingData.Top);

                //    //if(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                //    //{
                //    //    cmdCreateGCodes.Enabled = false;
                //    //}

                //}

                //else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal &&
                //         modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                //{
                //    cmdThrustBearingData.Visible = false;
                //    cmdThrustBearingDesgnDetail.Visible = false;
                //    cmdEndSealData.Visible = true;
                //    //cmdEndSealDesgnDetail.Visible = false; // true; For interim release
                //    if (modMain.gblnSealDesignDetails)
                //    {
                //        cmdEndSealDesgnDetail.Visible = true; // true; For interim 2nd release
                //    }
                //    else
                //    {
                //        cmdEndSealDesgnDetail.Visible = false;
                //    }
                //    //cmdCreateGCodes.Visible = false;

                //    cmdEndSealData.Top = mTop_CmdButton;
                //    SetPosition_CmdButtons(cmdEndSealData.Top);
                //}

                //else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                //         modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                //{
                //    cmdThrustBearingData.Visible = true;
                //    cmdThrustBearingDesgnDetail.Visible = true;
                //    cmdEndSealData.Visible = true;
                //    cmdEndSealDesgnDetail.Visible = true;
                //    //cmdCreateGCodes.Visible = true;

                //    cmdThrustBearingData.Top = mTop_CmdButton;
                //    cmdEndSealData.Top = cmdThrustBearingData.Top + mHt_cmdButton;

                //    SetPosition_CmdButtons(cmdEndSealData.Top);

                //    //if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                //    //{
                //    //    cmdCreateGCodes.Enabled = false;
                //    //}
                //}

                //else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                //         modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                //{
                //    cmdEndSealData.Visible = false;
                //    cmdEndSealDesgnDetail.Visible = false;
                //    cmdThrustBearingData.Visible = true;
                //    cmdThrustBearingDesgnDetail.Visible = true;
                //    //cmdCreateGCodes.Visible = true;

                //    cmdThrustBearingData.Top = mTop_CmdButton;
                //    SetPosition_CmdButtons(cmdThrustBearingData.Top);

                //    //if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi ||
                //    //   ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                //    //{
                //    //    cmdCreateGCodes.Enabled = false;
                //    //}
                //}
               
            }

            private void SetPosition_CmdButtons(int Top_In)
            //==============================================
            {
                cmdRadialBearingDesgnDetail.Top = Top_In + mHt_cmdButton;

                if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].TLTB.Exists == true ||
                       ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].TLTB.Exists == true)
                {
                    cmdThrustBearingDesgnDetail.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                    cmdCreateFiles.Top = cmdThrustBearingDesgnDetail.Top + mHt_cmdButton;
                    cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;
                }
                else
                {
                    if (modMain.gblnSealDesignDetails)        //AES 23NOV18
                    {
                        cmdEndSealDesgnDetail.Visible = true;
                        cmdEndSealDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                        cmdCreateFiles.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                        cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;
                    }
                    else
                    {
                        cmdEndSealDesgnDetail.Visible = false;
                        cmdCreateFiles.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                        cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;
                    }

                    //AES 23NOV18
                    //cmdEndSealDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                    //cmdCreateFiles.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                    //cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;
                }

                ////cmdPerfData.Top = Top_In + mHt_cmdButton;
                ////cmdRadialBearingDesgnDetail.Top = cmdPerfData.Top + mHt_cmdButton;
                //cmdRadialBearingDesgnDetail.Top = Top_In + mHt_cmdButton;

                //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal &&
                //    modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                //{
                //    cmdEndSealDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                //    cmdThrustBearingDesgnDetail.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                //    cmdCreateFiles.Top = cmdThrustBearingDesgnDetail.Top + mHt_cmdButton;
                //    //cmdCreateGCodes.Top = cmdCreateFiles.Top + mHt_cmdButton;
                //    cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;

                //}
                //else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal &&
                //        modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                //{
                //    //cmdEndSealDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                //    if (modMain.gblnSealDesignDetails)
                //    {
                //        cmdEndSealDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                //        cmdCreateFiles.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                //    }
                //    else
                //    {
                //        cmdCreateFiles.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                //    }
                //    //cmdCreateFiles.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;     //For Interim release.
                //    cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;
                //}

                //else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                //         modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                //{
                //    cmdThrustBearingDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                //    cmdEndSealDesgnDetail.Top = cmdThrustBearingDesgnDetail.Top + mHt_cmdButton;
                //    cmdCreateFiles.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                //    //cmdCreateGCodes.Top = cmdCreateFiles.Top + mHt_cmdButton;
                //    cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;

                //}
                //else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                //         modMain.gProject.Product.EndPlate[1].Type== clsEndPlate.eType.TL_TB)
                //{
                //    cmdThrustBearingDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                //    cmdCreateFiles.Top = cmdThrustBearingDesgnDetail.Top + mHt_cmdButton;
                //    //cmdCreateGCodes.Top = cmdCreateFiles.Top + mHt_cmdButton;
                //    cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;
                //}
                
            }


            private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
            //======================================================================
            {
                if (!mblnExitFromBtn)
                {
                    ExitProgram();
                    e.Cancel = true;    
                }
            }

            private void ExitProgram()
            //=============================
            //....Will be fully implemented later. 04MAR07.
            {
                string pstrPrompt = null;
                string pstrTitle = null;
                DialogResult pAnswer;
                Boolean pReturn = false;

                pstrPrompt = " Do you want to exit from application?";
                pstrTitle = "Exit Application";
                pAnswer = MessageBox.Show(pstrPrompt, pstrTitle, MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                if (pAnswer == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;

                    if (modMain.gProject != null)
                    {  
                        //AES 26JUL18
                        ////if(modMain.gDB.ProjectNo_Exists(modMain.gProject.No, modMain.gProject.No_Suffix, "tblProject_Details"))
                        ////{
                        ////    modMain.gDB.UpdateRecord(modMain.gProject, modMain.gOpCond);
                        ////}
                        ////else
                        ////{
                        ////    modMain.gDB.AddRecord(modMain.gProject,modMain.gOpCond);
                        ////}

                        //modMain.gDB.SaveToDB_ORM(modMain.gProject, modMain.gOpCond);
                    }

                    //modMain.gfrmLogIn.Close();

                    //this.Close();               

                    //System.Environment.Exit(0);
                    mblnExitFromBtn = true;
                    Application.Exit();
                    Cursor = Cursors.Default;       
                }
                else if (pAnswer == DialogResult.No)
                    return;
              
            }

        #endregion


        #region "CONTROL EVENT RELATED ROUTINE"
        //-------------------------------------

            #region "MENU ITEM RELATED ROUTINE"
            //---------------------------------

                private void mnuItem_Click(object sender, EventArgs e)
                //====================================================
                {
                    ToolStripMenuItem pMenuStrip = (ToolStripMenuItem)sender;

                    switch (pMenuStrip.Name)
                    {
                        case "&New":
                            break;

                        case "mnuSession_Restore":
                            Set_OpenFileDialog();
                            break;

                        case "mnuSession_Save":
                            Set_SaveFileDialog();
                            break;

                        case "mnuImportDataSet":
                           string pExcelFileName = "";
                           
                            openFileDialog1.Filter = "XLRadial files|*.xls;*.xlsx";
                            openFileDialog1.FilterIndex = 1;
                            openFileDialog1.InitialDirectory = modMain.gFiles.File_InputPath;
                            openFileDialog1.Title = "Open";
                            openFileDialog1.FileName = " ";

                            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                Cursor = Cursors.WaitCursor;
                                pExcelFileName = openFileDialog1.FileName;
                                modMain.gProject = new clsProject(clsUnit.eSystem.Metric, clsBearing.eType.JBearing);
                                modMain.gFiles.Read_Parameter_Complete(ref modMain.gProject, pExcelFileName, false);

                                if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.Blade.Count == 2 ||
                                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.Blade.Count == 2)
                                {
                                    modMain.gblnSealDesignDetails = true;
                                }
                                modMain.gfrmMain.UpdateDisplay(modMain.gfrmMain); 

                                Cursor = Cursors.Default;
                            }                                                        
                            break;

                        case "mnuFileSave":
                            //Set_SaveFileDialog();
                            break;

                        case "Save &As":
                            break;

                        case "mnuFileExit":
                            ExitProgram();
                            break;
                    }
                }

                private void Set_SaveFileDialog()
                //===============================
                {
                    string pFileName = "DataSet";
                    string pFilePath = modMain.gFiles.File_InputPath;//"D:\\BearingCAD\\";
                    string pFileTitle = "";
                    string pFileName_BearingCAD = "DataSet";
                    DateTime pDate = DateTime.Today;
                   
                    saveFileDialog1.Filter = "BearingCAD Session Files (*.BearingCAD)|*.BearingCAD";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.InitialDirectory = pFilePath;
                    saveFileDialog1.FileName = pFileName_BearingCAD;
                    saveFileDialog1.Title = "Save";

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        modMain.gFiles.FileName_BearingCAD = saveFileDialog1.FileName;
                       
                        modMain.gFiles.Save_SessionData(modMain.gProject);
                        MessageBox.Show("Data have been Saved successfully To '" + Path.GetFileName(modMain.gFiles.FileName_BearingCAD) + "'", "Data Save", MessageBoxButtons.OK);
                    }
                }

                private void Set_OpenFileDialog()
                //===============================
                {
                    string pFileName = "";
                    string pFilePath = modMain.gFiles.File_InputPath; 
                    string pFileName_BearingCAD = "";
                    if (modMain.gFiles.FileName_BearingCAD != "")
                    {
                        pFileName = modMain.gFiles.FileName_BearingCAD.Remove(modMain.gFiles.FileName_BearingCAD.Length - 11);
                        pFilePath = modMain.gFiles.FileName_BearingCAD.Substring(0, modMain.gFiles.FileName_BearingCAD.LastIndexOf("\\"));
                        pFileName_BearingCAD = pFileName.Substring(pFileName.LastIndexOf("\\") + 1);//ExtractPreData(pFileName, "_");
                    }

                    openFileDialog1.Filter = "BearingCAD Session Files (*.BearingCAD)|*.BearingCAD";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.InitialDirectory = pFilePath;
                    openFileDialog1.FileName = pFileName_BearingCAD;
                    openFileDialog1.Title = "Restore";

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {                       
                        modMain.gProject = new clsProject(clsUnit.eSystem.English,clsBearing.eType.JBearing);
                        modMain.gFiles.FileName_BearingCAD = openFileDialog1.FileName;
                        if (modMain.gFiles.FileName_BearingCAD != "" && modMain.gFiles.FileName_BearingCAD != null)
                        {
                            string pFileTitle_Temp = modMain.gFiles.FileName_BearingCAD.Substring(modMain.gFiles.FileName_BearingCAD.LastIndexOf("\\") + 1);

                            string pstrTemp = pFileTitle_Temp;
                            if (pFileTitle_Temp.Contains('_'))
                            {
                                pstrTemp = pFileTitle_Temp.Substring(0, pFileTitle_Temp.LastIndexOf("_"));
                            }
                            pFilePath = modMain.gFiles.FileName_BearingCAD.Remove(modMain.gFiles.FileName_BearingCAD.Length - 11);
                                                        
                            modMain.gFiles.Restore_SessionData(ref modMain.gProject,  pFilePath);

                            if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.Blade.Count == 2 ||
                                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.Blade.Count == 2)
                            {
                                modMain.gblnSealDesignDetails = true;
                            }
                            
                            Cursor.Current = Cursors.WaitCursor;
                            modMain.gfrmMain.UpdateDisplay(modMain.gfrmMain);       //AES 17AUG18
                           
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Data have been Restored successfully from '" + pFileTitle_Temp + "'", "Data Restore", MessageBoxButtons.OK);
                        }
                    }
                }

            #endregion


            #region "TOOL STRIP RELATED ROUTINE"
            //----------------------------------

                private void ToolStrip_ItemClicked(object sender, EventArgs e)
                //============================================================
                {
                    ToolStripItem pToolStripItem = (ToolStripItem)sender;

                    switch (pToolStripItem.Name)
                    {
                        //case "tsNew":
                        //    break;

                        //case "tsOpen":
                        //    Set_OpenFileDialog();
                        //    break;

                        //case "tsSave":
                        //    Set_SaveFileDialog();
                        //    break;

                        //case "tsExit":
                        //   ExitProgram();
                        //    break;
                    }
                }

            #endregion


            #region "COMMAND BUTTON RELATED ROUTINE"
            //--------------------------------------

                private void cmdButtons_Click(object sender, System.EventArgs e)
                //==============================================================
                {
                    Button pcmdButton = (Button)sender;
                    
                    string pMsg = "Please insert a Project Number";
                    string pCaption = "Project Information Sheet";

                    Cursor = Cursors.WaitCursor;        

                    switch (pcmdButton.Name)
                    {

                        case "cmdProject":
                            //------------
                            modMain.gfrmProject.ShowDialog();
                            break;

                        //case "cmdImportAnalyticalData":
                        //    //-------------------------
                            //if (modMain.gProject != null)
                            //    modMain.gfrmImportData.ShowDialog();
                            //else
                            //    MsgBox(pMsg, pCaption);                           
                            //break;

                        case "cmdOpCond":
                            //-----------
                            if (modMain.gProject != null)
                                modMain.gfrmOperCond.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;          
                
                        case "cmdRadialBearingData":
                            //----------------------
                            if (modMain.gProject != null)
                                modMain.gfrmBearing.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;

                        case "cmdThrustBearingData":
                            //----------------------
                            if (modMain.gProject!= null)
                                modMain.gfrmThrustBearing.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;


                        case "cmdEndSealData":
                            //-------------
                            if (modMain.gProject != null)
                                modMain.gfrmSeal.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;


                        case "cmdPerfData":
                            //-------------

                            if (modMain.gProject != null)
                                modMain.gfrmPerformDataBearing.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;

                        case "cmdRadialBearingDesgnDetail":
                            //-----------------------------
                            if (modMain.gProject != null)
                                modMain.gfrmBearingDesignDetails.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);

                            break;

                        case "cmdThrustBearingDesgnDetail":
                            //-----------------------------
                            if (modMain.gProject != null)
                                modMain.gfrmThrustBearingDesignDetails.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);

                            break;

                        case "cmdEndSealDesgnDetail":
                            //---------------------
                            if (modMain.gProject != null)
                            {                                
                                modMain.gfrmSealDesignDetails.ShowDialog();
                               
                            }
                            else
                            {              
                               MsgBox(pMsg, pCaption);
                            }
                            break;

                        //case "cmdCreateGCodes":
                        //    //------------------
                        //    //if (modMain.gProject != null)
                        //    //    modMain.gfrmGCodes.ShowDialog();
                        //    //else
                        //    //    MsgBox(pMsg, pCaption);


                        //    if (modMain.gProject != null)
                        //    {
                        //        if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal &&
                        //            modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                        //        {
                        //            if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[1]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                        //            {
                        //                MessageBox.Show("G-Code generation for Bi-directional TB is not supported in this version.", "G-Code", MessageBoxButtons.OK);
                        //            }
                        //        }
                               
                        //        else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                        //                 modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                        //        {
                        //            if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                        //            {
                        //                MessageBox.Show("G-Code generation for Bi-directional TB is not supported in this version.", "G-Code", MessageBoxButtons.OK);
                        //            }
                        //        }
                        //        else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                        //                 modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                        //        {
                        //            if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi ||
                        //               ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[1]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                        //            {
                        //                MessageBox.Show("G-Code generation for Bi-directional TB is not supported in this version.", "G-Code", MessageBoxButtons.OK);
                        //            }
                        //        }
                            
                        //        //for (int i = 0; i < 2; i++)
                        //        //{
                        //        //    //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.TL_TB ||)
                        //            //{
                        //                //if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                        //                //{
                        //                //    MessageBox.Show("G-Code generation for Bi-directional TB is not supported in this version.", "G-Code", MessageBoxButtons.OK);
                        //                //}
                        //            //}
                        //        //}
                            
                        //        ////modMain.gfrmGCodes.ShowDialog();
                        //    }
                        //    else
                        //        MsgBox(pMsg, pCaption);

                        //    break;

                        case "cmdCreateFiles":                                          
                            //----------------
                            if (modMain.gProject != null)
                                modMain.gfrmCreateDataSet.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);

                            break;

                        case "cmdExit":
                            //---------
                            ExitProgram();
                            break;

                    }

                    Cursor = Cursors.Default;        

                }

                //private void cmdCreateGCodes_MouseHover(object sender, EventArgs e)         //BG 03APR13
                ////=================================================================
                //{
                //    //if(!cmdCreateGCodes.Enabled)
                //    toolTip1.SetToolTip(cmdCreateGCodes, "G-Code generation for Bi-directional TB is not supported in this version.");
                //}


                private void MsgBox(string Msg_In, string Caption_In)                   //SB 09APR09
                //===================================================
                {
                    MessageBox.Show(Msg_In, Caption_In, MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                    modMain.gfrmProject.Show();
                }

            #endregion

                //private void btnbutton1_Click(object sender, EventArgs e)
                //{
                //    modMain.gProject = new clsProject(clsUnit.eSystem.English);
                //    modMain.gFiles.Read_Parameter_Complete(ref modMain.gProject, ref modMain.gOpCond, "C:\\BearingCAD\\Projects\\V22\\Example 3\\CAD Neutral Data Set_RevA.xlsx", false);
                //}

               

                             
        
        #endregion

    }
}
