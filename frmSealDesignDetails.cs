
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmSealDesignDetails                   '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  19DEC18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//================================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Drawing.Printing;
using System.Data.OleDb;

namespace BearingCAD22
{
    public partial class frmSealDesignDetails : Form
    {

        #region "MEMBER VARIABLE DECLARATION:"
        //***********************************
            
            //....Local Class Object
            //private clsSeal[] mEndPlate= new clsSeal[2];
            private clsEndPlate[] mEndPlate = new clsEndPlate[2];

            private TextBox[] mTxtDrainHoles_Annulus_D;
            private Double [] mDrainHoles_Annulus_D_Calc;
            private TextBox[] mTxtDrainHoles_AngStart;
            
            private ComboBox[] mCmbDrainHoles_Annulus_Ratio_L_H;
            private ComboBox[] mCmbDrainHoles_D_Desig;
            private TextBox [] mTxtDrainHoles_Count;
            private TextBox [] mTxtDrainHoles_V;
            private ComboBox[] mCmbDrainHoles_AngBet;
            //private Label   [] mLblDrainHoles_AngBet_ULim;
            private Label   [] mLblDrainHoles_AngBet_LLim;
            private ComboBox[] mCmbDrainHoles_AngExit;
            private Label   [] mLblDrainHoles_Notes; 
       
            private Double [] mWireClipHole_AngOther = new Double[5];
            private TextBox[] mTxtBoxWireClipHole_Front;
            private TextBox[] mTxtBoxWireClipHole_Back;
                   
            private Boolean mblnDrainHoles_Annulus_Ratio_L_H_ManuallyChanged = false;
            private Boolean mblnDrainHoles_Annulus_D_ManuallyChanged = false;

            private Boolean mblnDrainHoles_D_Front_ManuallyChanged = false;
            private Boolean mblnDrainHoles_AngBet_Front_ManuallyChanged = false;
            private Boolean mblnDrainHoles_AngExit_Front_ManuallyChanged = false;
            private Boolean mblnTab_ManuallyChanged = false;

            private Boolean mblnDrainHoleCount_Front_ManuallyChanged = false;
            private Boolean mblnDrainHoleCount_Back_ManuallyChanged = false;

            private Label[] mlblMetric;

        #endregion


        #region "FORM CONSTRUCTOR RELATED ROUTINE:"
        //****************************************

            public frmSealDesignDetails()
            //==========================
            {
                InitializeComponent();

                //...Drain Holes
                mCmbDrainHoles_Annulus_Ratio_L_H = new[] {cmbDrainHoles_Annulus_Ratio_L_H_Front,
                                                          cmbDrainHoles_Annulus_Ratio_L_H_Back};

                mTxtDrainHoles_Annulus_D =         new[] {txtDrainHoles_Annulus_D_Front,
                                                          txtDrainHoles_Annulus_D_Back};

                mCmbDrainHoles_D_Desig =           new[] {cmbDrainHoles_D_Desig_Front, 
                                                          cmbDrainHoles_D_Desig_Back };

                mTxtDrainHoles_Count =             new[] {txtDrainHoles_Count_Front,
                                                          txtDrainHoles_Count_Back};

                mTxtDrainHoles_V =                 new[] {txtDrainHoles_V_Front, 
                                                          txtDrainHoles_V_Back };

                mCmbDrainHoles_AngBet =            new[] {cmbDrainHoles_AngBet_Front,
                                                          cmbDrainHoles_AngBet_Back};

                //mLblDrainHoles_AngBet_ULim =       new[] {lblDrainHoles_AngBet_ULim_Front,
                                                          //lblDrainHoles_AngBet_ULim_Back};

                mLblDrainHoles_AngBet_LLim =       new[] {lblDrainHoles_AngBet_LLim_Front,
                                                          lblDrainHoles_AngBet_LLim_Back};

                mTxtDrainHoles_AngStart =          new[] {txtDrainHoles_AngStart_Front,
                                                          txtDrainHoles_AngStart_Back};

                mCmbDrainHoles_AngExit =           new[] {cmbDrainHoles_AngExit_Front,
                                                          cmbDrainHoles_AngExit_Back};

                mLblDrainHoles_Notes = new[] {lblDrainHoles_Notes_Front, 
                                                    lblDrainHoles_Notes_Back };

                mlblMetric = new Label[] { lblDrainHoles_D_Desig_Front_Unit, lblDrainHoles_D_Desig_Front_MM, lblDrainHoles_D_Desig_Back_Unit, lblDrainHoles_D_Desig_Back_MM };


                object[] mAngExit = new object[4] { 30, 35, 40, 45 };

                for (int i = 0; i < 2; i++)
                {
                    mCmbDrainHoles_AngExit[i].Items.Clear();
                    mCmbDrainHoles_AngExit[i].Items.AddRange(mAngExit);
                }

                for (int i = 0; i < 2; i++)
                {
                    mLblDrainHoles_Notes[i].Text = "Note: Drain hole array is crossing the Bearing S/L." + Environment.NewLine +
                                                         "      An extra drain hole has been added at the end.";
                }


                //....Wire Clip Holes
                mTxtBoxWireClipHole_Front = new[] { txtWireClipHoles_AngStart_Front, 
                                                    txtWireClipHoles_AngOther1_Front,
                                                    txtWireClipHoles_AngOther2_Front  };

                mTxtBoxWireClipHole_Back = new[] { txtWireClipHoles_AngStart_Back, 
                                                   txtWireClipHoles_AngOther1_Back,
                                                   txtWireClipHoles_AngOther2_Back  };
            }

        #endregion


        #region "FORM LOAD RELATED ROUTINES:"
        //**********************************

            private void frmSealDesignDetails_Load(object sender, EventArgs e)
            //================================================================
            {
                mblnDrainHoles_Annulus_Ratio_L_H_ManuallyChanged = false;
                mblnDrainHoles_Annulus_D_ManuallyChanged = false;                               

                //....Set Local Object.
                SetLocalObject();

                clsEndPlate[] pEndPlate = new clsEndPlate[2];
                for (int i = 0; i < 2; i++)
                {
                    pEndPlate[i] = (clsEndPlate)((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[i].Clone();                    
                }

                Boolean pblnDrainHoles = true;

                Boolean pblnDrainHoles_Front = false;
                Boolean pblnDrainHoles_Back = false;

                TabPage[] pTabPages_DesignDetails = new TabPage[] { tabFront, tabBack };

                tbEndSealDesignDetails.TabPages.Clear();
                tbEndSealDesignDetails.TabPages.AddRange(pTabPages_DesignDetails);

                if (mEndPlate[0].Seal.Blade.Count > 1 && mEndPlate[1].Seal.Blade.Count == 1)
                {
                    pblnDrainHoles_Front = true;
                    tbEndSealDesignDetails.TabPages.Remove(tabBack);
                }
                else if (mEndPlate[0].Seal.Blade.Count == 1 && mEndPlate[1].Seal.Blade.Count > 1)
                {
                    pblnDrainHoles_Back = true;
                    tbEndSealDesignDetails.TabPages.Remove(tabFront);
                }
                else if (mEndPlate[0].Seal.Blade.Count > 1 && mEndPlate[1].Seal.Blade.Count > 1)
                {
                    pblnDrainHoles_Front = true;
                    pblnDrainHoles_Back = true;
                }
                else if (mEndPlate[0].Seal.Blade.Count == 1 && mEndPlate[1].Seal.Blade.Count == 1)
                {
                    pblnDrainHoles_Front = true;
                    tbEndSealDesignDetails.TabPages.Remove(tabFront);
                    tbEndSealDesignDetails.TabPages.Remove(tabBack);
                }

                //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                //{
                    //  FRONT
                    //  -----
                    //
                    //TabPage[] pTabPages_Front = new TabPage[] { tabMounting_Front, tabDrain_Front, tabTempSensor_Front, tabWC_Front };

                    //if (tbEndSealDesignDetails_Front.TabPages.Count < 4)
                    //{
                    //    tbEndSealDesignDetails_Front.TabPages.Clear();
                    //    tbEndSealDesignDetails_Front.TabPages.AddRange(pTabPages_Front);
                    //}

                if (pblnDrainHoles_Front)
                {

                    TabPage[] pTabPages_Front = new TabPage[] { tabMounting_Front, tabDrain_Front, tabTempSensor_Front, tabWC_Front };


                    if (tbEndSealDesignDetails_Front.TabPages.Count < 4)
                    {
                        tbEndSealDesignDetails_Front.TabPages.Clear();
                        tbEndSealDesignDetails_Front.TabPages.AddRange(pTabPages_Front);
                    }

                    tbEndSealDesignDetails_Front.TabPages.Remove(tabMounting_Front);
                    tbEndSealDesignDetails_Front.TabPages.Remove(tabTempSensor_Front);
                    tbEndSealDesignDetails_Front.TabPages.Remove(tabWC_Front);

                    if (mEndPlate[0].Seal.Blade.Count > 1)
                        pblnDrainHoles = true;
                    else
                        pblnDrainHoles = false;

                    //SetTabPages(pblnDrainHoles, tabDrain_Front, 0);

                    //if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Exists)
                    //{
                    //    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eBolting.Front)
                    //    {
                    //        SetTabPages(true, tabTempSensor_Front, 0);
                    //        SetTabPages(true, tabWC_Front, 0);

                    //        ////SetTabPages(false, tabTempSensor_Front, 0);
                    //        ////SetTabPages(false, tabWC_Front, 0);
                    //    }
                    //    else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eBolting.Back)
                    //    {
                    //        SetTabPages(false, tabTempSensor_Front, 0);
                    //        SetTabPages(false, tabWC_Front, 0);
                    //    }
                    //}
                    //else
                    //{
                    //    SetTabPages(false, tabTempSensor_Front, 0);
                    //    SetTabPages(false, tabWC_Front, 0);
                    //}

                    //....Load Drain Hole Annulus_LH_Ratio                    
                    Load_DrainHole_Annulus_LH_Ratio(mEndPlate[0], mCmbDrainHoles_Annulus_Ratio_L_H[0]);

                    //....Load Drain Hole Dia.
                    Load_DrainHole_D_Desig(mEndPlate[0], mCmbDrainHoles_D_Desig[0]);

                    //....Load WC Holes Dia.
                    //LoadWireClipHoles_D(cmbWireClipHoles_Thread_Dia_Desig_Front);                  //BG 02JUL13
                    LoadWireClipHoles_D(mEndPlate[0], cmbWireClipHoles_Thread_Dia_Desig_Front);        //BG 02JUL13
                    LoadWireClipHoles_Count(cmbWireClipHoles_Count_Front);

                    LoadUnit(mEndPlate[0], cmbWireClipHoles_UnitSystem_Front);       //BG 02JUL13
                }
                //}


                //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                //{
                    //  BACK
                    //  -----
                    //
                    mCmbDrainHoles_Annulus_Ratio_L_H[1].Enabled = true;
                    mTxtDrainHoles_Annulus_D[1].ReadOnly = false;
                    mTxtDrainHoles_Annulus_D[1].BackColor = Color.White;
                    mCmbDrainHoles_D_Desig[1].Enabled = true;
                    mTxtDrainHoles_AngStart[1].ReadOnly = false;
                    mCmbDrainHoles_AngBet[1].Enabled = true;
                    mCmbDrainHoles_AngExit[1].Enabled = true;

                    //TabPage[] pTabPages_Back = new TabPage[] { tabMounting_Back, tabDrain_Back, tabTempSensor_Back, tabWC_Back };

                    //if (tbEndSealDesignDetails_Back.TabPages.Count < 4)
                    //{
                    //    tbEndSealDesignDetails_Back.TabPages.Clear();
                    //    tbEndSealDesignDetails_Back.TabPages.AddRange(pTabPages_Back);
                    //}

                    if (pblnDrainHoles_Back)
                    {
                        TabPage[] pTabPages_Back = new TabPage[] { tabDrain_Back };

                        if (tbEndSealDesignDetails_Back.TabPages.Count < 1)
                        {
                            tbEndSealDesignDetails_Back.TabPages.Clear();
                            tbEndSealDesignDetails_Back.TabPages.AddRange(pTabPages_Back);
                        }

                        tbEndSealDesignDetails_Back.TabPages.Remove(tabMounting_Back);
                        tbEndSealDesignDetails_Back.TabPages.Remove(tabTempSensor_Back);
                        tbEndSealDesignDetails_Back.TabPages.Remove(tabWC_Back);

                        if (mEndPlate[1].Seal.Blade.Count > 1)
                            pblnDrainHoles = true;
                        else
                            pblnDrainHoles = false;

                        //SetTabPages(pblnDrainHoles, tabDrain_Back, 1);

                        //if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Exists)
                        //{
                        //    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eBolting.Front)
                        //    {
                        //        SetTabPages(false, tabTempSensor_Back, 1);
                        //        SetTabPages(false, tabWC_Back, 1);
                        //    }
                        //    else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eBolting.Back)
                        //    {
                        //        ////SetTabPages(false, tabTempSensor_Back, 1);
                        //        ////SetTabPages(false, tabWC_Back, 1);

                        //        SetTabPages(true, tabTempSensor_Back, 1);
                        //        SetTabPages(true, tabWC_Back, 1);
                        //    }
                        //}
                        //else
                        //{
                        //    SetTabPages(false, tabTempSensor_Back, 1);
                        //    SetTabPages(false, tabWC_Back, 1);
                        //}

                        //....Load Drain Hole Annulus_LH_Ratio                    
                        Load_DrainHole_Annulus_LH_Ratio(mEndPlate[1], mCmbDrainHoles_Annulus_Ratio_L_H[1]);

                        //....Load Drain Hole Dia.
                        Load_DrainHole_D_Desig(mEndPlate[1], mCmbDrainHoles_D_Desig[1]);

                        //....Load WC Holes Dia.                      
                        LoadWireClipHoles_D(mEndPlate[1], cmbWireClipHoles_Thread_Dia_Desig_Back);       
                        LoadWireClipHoles_Count(cmbWireClipHoles_Count_Back);

                        LoadUnit(mEndPlate[1], cmbWireClipHoles_UnitSystem_Back);      
                    }
                //}

                ////CheckNullParams();

                //....Set Control.
                SetControl();

                if (pblnDrainHoles_Front && pblnDrainHoles_Back)
                {
                    mCmbDrainHoles_Annulus_Ratio_L_H[1].Enabled = false;

                    mTxtDrainHoles_Annulus_D[1].ReadOnly = true;
                    mTxtDrainHoles_Annulus_D[1].BackColor = mTxtDrainHoles_Count[1].BackColor;

                    mCmbDrainHoles_D_Desig[1].Enabled = false;

                    mTxtDrainHoles_AngStart[1].ReadOnly = true;
                    mTxtDrainHoles_AngStart[1].BackColor = mTxtDrainHoles_Count[1].BackColor;
                    mCmbDrainHoles_AngBet[1].Enabled = false;
                    mCmbDrainHoles_AngExit[1].Enabled = false;
                }
                else if (pblnDrainHoles_Back)
                {
                    mCmbDrainHoles_Annulus_Ratio_L_H[1].Enabled = true;

                    mTxtDrainHoles_Annulus_D[1].ReadOnly = false;
                    mTxtDrainHoles_Annulus_D[1].BackColor = Color.White;

                    mCmbDrainHoles_D_Desig[1].Enabled = true;

                    mTxtDrainHoles_AngStart[1].ReadOnly = false;
                    mTxtDrainHoles_AngStart[1].BackColor = Color.White;
                    mCmbDrainHoles_AngBet[1].Enabled = true;
                    mCmbDrainHoles_AngExit[1].Enabled = true;
                }

                for (int i = 0; i < 2; i++)
                {
                    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[i] = (clsEndPlate)pEndPlate[i].Clone();
                }

                //....Set Local Object.
                SetLocalObject();

                //....Display Data.
                DisplayData();

                tbEndSealDesignDetails.SelectedIndex = 0;
               
            }


            #region "Helper Routines:"
            //-----------------------

                private void SetLocalObject()
                //===========================
                {
                    //....Initialize Local Variable.               
                    for (int i = 0; i < 2; i++)
                    {
                        mEndPlate[i] = (clsEndPlate)((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[i].Clone();
                        mEndPlate[i].Seal.DrainHoles.UpdateCurrentSeal((clsJBearing)modMain.gProject.PNR.Bearing);

                        //if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                        //{
                        //    mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                        //    mEndSeal[i].DrainHoles.UpdateCurrentSeal(modMain.gProject.Product);     //AES 25OCT18
                        //}
                    }

                }


                private void SetTabPages(Boolean Checked_In, TabPage TabPage_In, int Indx_In)
                //============================================================================
                {
                    TabPage[] pTabPages = new TabPage[] { tabFront, tabBack };

                    tbEndSealDesignDetails.TabPages.Clear();

                    for (int i = 0; i < 2; i++)
                    {
                        //if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                        //{
                            tbEndSealDesignDetails.TabPages.Add(pTabPages[i]);
                        //}
                    }

                    TabPage[] pTabPages_Front = new TabPage[] { tabMounting_Front, tabDrain_Front, tabTempSensor_Front, tabWC_Front };
                    TabPage[] pTabPages_Back = new TabPage[] { tabMounting_Back, tabDrain_Back, tabTempSensor_Back, tabWC_Back };

                    Boolean pTab_Exists = false;

                    switch (Indx_In)
                    {

                        case 0:
                        //-----
                            //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                            //{
                                if (!Checked_In)
                                {
                                    tbEndSealDesignDetails_Front.TabPages.Remove(TabPage_In);
                                }

                                foreach (TabPage pTp in tbEndSealDesignDetails_Front.TabPages)
                                {
                                    if (pTp.Text == TabPage_In.Text)
                                    {
                                        pTab_Exists = true;
                                    }
                                }

                                if ((Checked_In) && (!pTab_Exists))
                                {
                                    tbEndSealDesignDetails_Front.TabPages.Clear();
                                    tbEndSealDesignDetails_Front.TabPages.AddRange(pTabPages_Front);
                                }

                                tbEndSealDesignDetails_Front.Refresh();
                            //}

                            break;


                        case 1:
                        //-----

                            //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                            //{
                                if (!Checked_In)
                                {
                                    tbEndSealDesignDetails_Back.TabPages.Remove(TabPage_In);
                                    tbEndSealDesignDetails_Back.Refresh();
                                }

                                foreach (TabPage pTp in tbEndSealDesignDetails_Back.TabPages)
                                {
                                    if (pTp.Text == TabPage_In.Text)
                                    {
                                        pTab_Exists = true;
                                    }
                                }

                                if ((Checked_In) && (!pTab_Exists))
                                {
                                    tbEndSealDesignDetails_Back.TabPages.Clear();
                                    tbEndSealDesignDetails_Back.TabPages.AddRange(pTabPages_Back);
                                }

                                tbEndSealDesignDetails_Back.Refresh();
                            //}

                            break;
                    }
                }

                private void Load_DrainHole_Annulus_LH_Ratio(clsEndPlate EndPlate_In, ComboBox CmbBox_In)
                //===============================================================================    
                {
                    int pLH_Ratio_Min = 2, pLH_Ratio_Max = 6;

                    mDrainHoles_Annulus_D_Calc = new Double[pLH_Ratio_Max + 1];
                    clsEndPlate pEndPlate = (clsEndPlate)EndPlate_In.Clone(); 
                    //clsEndPlate.clsSeal pSeal = (clsEndPlate.clsSeal)EndPlate_In.Clone(); 
   
                    CmbBox_In.Items.Clear();

                    while (pLH_Ratio_Min <= pLH_Ratio_Max)
                    {
                        CmbBox_In.Items.Add(pLH_Ratio_Min);
                        pEndPlate.Seal.DrainHoles.Annulus_Ratio_L_H = pLH_Ratio_Min;
                        mDrainHoles_Annulus_D_Calc[pLH_Ratio_Min] = pEndPlate.Seal.DrainHoles.Calc_Annulus_D();
                        pLH_Ratio_Min++;
                    }                    
                }


                private void Load_DrainHole_D_Desig(clsEndPlate EndPlate_In, ComboBox CmbBox_In)
                //=======================================================================
                {
                    StringCollection pDrainHole_D = new StringCollection();

                   

                    //....EXCEL File: StdToolData
                    string pstrFIELDS, pstrFROM, pstrWHERE, pstrORDERBY, pstrSQL;
                    OleDbDataReader pobjDR = null;
                    OleDbConnection pConnection = null;
                    pstrFIELDS = "Select D_Desig";
                    pstrFROM = " FROM [Drill$]";
                    pstrWHERE = " WHERE DrainHole = 'Y' or DrainHole = 'YP'";
                    pstrORDERBY = " Order by D_Desig ASC";

                    pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE + pstrORDERBY;
                    pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdToolData, ref pConnection);

                    while (pobjDR.Read())
                    {
                        pDrainHole_D.Add(pobjDR["D_Desig"].ToString());
                    }
                    pobjDR.Dispose();
                    pConnection.Close();

                    StringCollection pDrainHole_DwoIn = new StringCollection();
                    Double pNumerator, pDenominator;
                    Double pFinal;

                    for (int i = 0; i < pDrainHole_D.Count; i++)
                        pDrainHole_D[i] = pDrainHole_D[i].Remove(pDrainHole_D[i].Length - 1);

                    for (int i = 0; i < pDrainHole_D.Count; i++)
                        if (pDrainHole_D[i].Contains("/"))
                        {
                            if (pDrainHole_D[i].ToString() != "1")
                            {
                                pNumerator = Convert.ToInt32(modMain.ExtractPreData(pDrainHole_D[i], "/"));
                                pDenominator = Convert.ToInt32(modMain.ExtractPostData(pDrainHole_D[i], "/"));
                                pFinal = Convert.ToDouble(pNumerator / pDenominator);

                                pDrainHole_DwoIn.Add(pFinal.ToString());
                                
                            }
                            else
                            {
                                
                                pFinal = Convert.ToDouble(pDrainHole_D[i]);
                                pDrainHole_DwoIn.Add(pFinal.ToString());
                             }
                        }

                    modMain.SortNumberwoHash(ref pDrainHole_DwoIn, true);

                    pDrainHole_D.Clear();
                    for (int i = 0; i < pDrainHole_DwoIn.Count; i++)
                        pDrainHole_D.Add(pDrainHole_DwoIn[i] + "\"");

                    CmbBox_In.Items.Clear();
                    clsEndPlate pEndPlate = (clsEndPlate)EndPlate_In.Clone(); 
                    //clsEndPlate.clsSeal pSeal = (clsEndPlate.clsSeal)EndPlate_In.Clone();             
                    if (pDrainHole_D.Count > 0)
                    {                      
                        for (int i = 0; i < pDrainHole_D.Count; i++)
                        {
                            if (pEndPlate.Seal.DrainHoles.Count <= 10)
                            {
                                CmbBox_In.Items.Add(pDrainHole_D[i]);
                            }
                        }

                        CmbBox_In.SelectedIndex = 0;
                    }

                  


                    ////BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    ////StringCollection pD_Desig = new StringCollection();

                    ////var pQryD_Desig = (from pRec in pBearingDBEntities.tblManf_Drill where pRec.fldCons_DrainHole == "Y" || pRec.fldCons_DrainHole == "YP" select pRec.fldD_Desig).ToList();

                    ////if (pQryD_Desig.Count() > 0)
                    ////{
                    ////    for (int i = 0; i < pQryD_Desig.Count; i++)
                    ////    {
                    ////        pD_Desig.Add(pQryD_Desig[i]);
                    ////    }
                    ////}

                    ////StringCollection pD_Desig_woIn = new StringCollection();  //....D_Desig w/o the inch symbol '"'.

                    ////Double pNum, pDen;
                    ////string pDia;

                    //////....PB 14JAN13. The following logic assumes that each pD_Desig item contains the inch symbol '"'.
                    //////........This assumption is ok so far for the Drain Hole drill sizes but may be in violation in general.
                    //////
                    ////for (int i = 0; i < pD_Desig.Count; i++)
                    ////    pD_Desig[i] = pD_Desig[i].Remove(pD_Desig[i].Length - 1);   //....Removes the last character being '"'.


                    ////for (int i = 0; i < pD_Desig.Count; i++)

                    ////    if (pD_Desig[i].Contains("/"))
                    ////    {
                    ////        //if (pD_Desig[i].ToString() != "1")        //....PB 14JAN13. This logic doesn't make sense. 
                    ////        //{                                         //........If pD_Desig contains "/", then it cannot be 1. 
                    ////        pNum = Convert.ToInt32(modMain.ExtractPreData(pD_Desig[i], "/"));
                    ////        pDen = Convert.ToInt32(modMain.ExtractPostData(pD_Desig[i], "/"));

                    ////        pDia = Convert.ToDouble(pNum / pDen).ToString();

                    ////        pD_Desig_woIn.Add(pDia);
                    ////        //}

                    ////        //else
                    ////        //{
                    ////        //    pD_Desig_woIn.Add(pD_Desig[i]);
                    ////        //}
                    ////    }


                    //////....Change the dia value to fractional format e.g. 5/16, 3/8 and the like. 
                    //////........The 2nd argument = TRUE.
                    //////
                    ////modMain.SortNumberwoHash(ref pD_Desig_woIn, true);

                    ////pD_Desig.Clear();

                    ////for (int i = 0; i < pD_Desig_woIn.Count; i++)
                    ////    pD_Desig.Add(pD_Desig_woIn[i] + "\"");                      //....Now, re-add the inch symbol '"'.

                    ////CmbBox_In.Items.Clear();

                    ////clsSeal pSeal = (clsSeal)Seal_In.Clone();               //SG 25JAN13

                    ////if (pD_Desig.Count > 0)
                    ////{
                    ////    for (int i = 0; i < pD_Desig.Count; i++)
                    ////    {
                    ////        //....Per Harout K.'s advice: Include only those D_Desig that yields
                    ////        //........Count < 10. Implemented in V1.1.
                    ////        //
                    ////        pSeal.DrainHoles.D_Desig = pD_Desig[i];

                    ////        if (pSeal.DrainHoles.Count <= 10)
                    ////        {
                    ////            CmbBox_In.Items.Add(pD_Desig[i]);
                    ////        }
                    ////    }
                    ////}
                }


                private void LoadWireClipHoles_D(clsEndPlate EndPlate_In, ComboBox CmbBox_In)
                //=====================================================================    AES 04JUL18                                     
                {
                    ////BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    ////string pUnitSystem = null;

                    ////if (Seal_In.Unit.System.ToString() != "")
                    ////    pUnitSystem = Seal_In.Unit.System.ToString().Substring(0, 1);


                    ////if (pUnitSystem == "E")
                    ////{
                    ////    //....Populate Dia Desig.
                    ////    StringCollection pDia_Desig = new StringCollection();
                    ////    var pQryManf_Screw = (from pRec in pBearingDBEntities.tblManf_Screw where pRec.fldUnit == pUnitSystem select pRec.fldD_Desig).Distinct().ToList();

                    ////    if (pQryManf_Screw.Count() > 0)
                    ////    {
                    ////        for (int i = 0; i < pQryManf_Screw.Count; i++)
                    ////        {
                    ////            pDia_Desig.Add(pQryManf_Screw[i].ToString().Trim());
                    ////        }
                    ////    }
                    ////    //....Initialize String Collection.
                    ////    StringCollection pDia_DwHash = new StringCollection();      //....Dia_Desig with # symbol.
                    ////    StringCollection pDia_DwoHash = new StringCollection();     //....Dia_Desig without # symbol. 

                    ////    Double pNumerator, pDenominator;
                    ////    String pFinal;

                    ////    for (int i = 0; i < pDia_Desig.Count; i++)
                    ////    {
                    ////        if (pDia_Desig[i].Contains("#"))
                    ////        {
                    ////            pDia_DwHash.Add(pDia_Desig[i].Remove(0, 1));

                    ////        }
                    ////        else
                    ////        {
                    ////            if (pDia_Desig[i].ToString() != "1")
                    ////            {
                    ////                pNumerator = Convert.ToInt32(modMain.ExtractPreData(pDia_Desig[i], "/"));
                    ////                pDenominator = Convert.ToInt32(modMain.ExtractPostData(pDia_Desig[i], "/"));
                    ////                pFinal = Convert.ToDouble(pNumerator / pDenominator).ToString();
                    ////                pDia_DwoHash.Add(pFinal);
                    ////            }
                    ////            else
                    ////                pDia_DwoHash.Add(pDia_Desig[i]);
                    ////        }
                    ////    }

                    ////    //....Sort Dia_Desig with # symbol.
                    ////    modMain.SortNumberwHash(ref pDia_DwHash);

                    ////    //....Sort Dia_Desig without # symbol.
                    ////    modMain.SortNumberwoHash(ref pDia_DwoHash, true);

                    ////    //....Concatinate # symbol with pDia_DwHash.
                    ////    for (int i = 0; i < pDia_DwHash.Count; i++)
                    ////    {
                    ////        pDia_DwHash[i] = "#" + pDia_DwHash[i];
                    ////    }

                    ////    CmbBox_In.Items.Clear();

                    ////    for (int i = 0; i < pDia_DwHash.Count; i++)
                    ////        CmbBox_In.Items.Add(pDia_DwHash[i]);

                    ////    for (int i = 0; i < pDia_DwoHash.Count; i++)
                    ////        CmbBox_In.Items.Add(pDia_DwoHash[i]);
                    ////}

                    ////else if (pUnitSystem == "M")
                    ////{
                    ////    //....Populate Dia Desig.
                    ////    StringCollection pDia_Desig = new StringCollection();
                    ////    var pQryManf_Screw = (from pRec in pBearingDBEntities.tblManf_Screw where pRec.fldUnit == pUnitSystem select pRec.fldD_Desig).Distinct().ToList();

                    ////    if (pQryManf_Screw.Count() > 0)
                    ////    {
                    ////        for (int i = 0; i < pQryManf_Screw.Count; i++)
                    ////        {
                    ////            pDia_Desig.Add(pQryManf_Screw[i].ToString().Trim());
                    ////        }
                    ////    }
                    ////    //....Initialize String Collection.
                    ////    StringCollection pDia_D = new StringCollection();  //....Dia_Desig with # symbol.

                    ////    for (int i = 0; i < pDia_Desig.Count; i++)
                    ////    {
                    ////        if (pDia_Desig[i].Contains("M"))
                    ////        {
                    ////            pDia_D.Add(pDia_Desig[i].Remove(0, 1));
                    ////        }
                    ////    }

                    ////    modMain.SortNumberwoHash(ref pDia_D, false);
                    ////    CmbBox_In.Items.Clear();

                    ////    for (int i = 0; i < pDia_D.Count; i++)
                    ////        pDia_D[i] = "M" + pDia_D[i];

                    ////    for (int i = 0; i < pDia_D.Count; i++)
                    ////        CmbBox_In.Items.Add(pDia_D[i]);
                    ////}

                    ////if (Seal_In.WireClipHoles.Screw_Spec.D_Desig != null)
                    ////{
                    ////    int pIndx = CmbBox_In.Items.IndexOf(Seal_In.WireClipHoles.Screw_Spec.D_Desig);

                    ////    if (pIndx != -1)
                    ////    {
                    ////        CmbBox_In.Text = Seal_In.WireClipHoles.Screw_Spec.D_Desig;
                    ////    }
                    ////    else
                    ////    {
                    ////        CmbBox_In.SelectedIndex = 0;
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    CmbBox_In.SelectedIndex = 0;
                    ////}
                }                


                private void LoadWireClipHoles_Count(ComboBox CmbBox_In)
                //======================================================
                {
                    //CmbBox_In.Items.Clear();

                    //int pCount = 0;
                    //if (mEndPlate[0] != null)
                    //    pCount = mEndPlate[0].WireClipHoles.COUNT_WIRE_CLIP_HOLES_MAX;
                    //else if (mEndPlate[1] != null)
                    //    pCount = mEndPlate[1].WireClipHoles.COUNT_WIRE_CLIP_HOLES_MAX;

                    //for (int i = 0; i < pCount; i++)
                    //{
                    //    CmbBox_In.Items.Add(i + 1);
                    //}

                    ////if (modMain.gProject.Product.Accessories.WireClip.Count > 0)
                    ////    CmbBox_In.Text = modMain.gProject.Product.Accessories.WireClip.Count.ToString();
                    ////else if (mEndPlate[0] != null && mEndPlate[0].WireClipHoles.Count > 0)
                    ////    CmbBox_In.Text = mEndPlate[0].WireClipHoles.Count.ToString();
                    ////else if (mEndPlate[1] != null && mEndPlate[1].WireClipHoles.Count > 0)
                    ////    CmbBox_In.Text = mEndPlate[1].WireClipHoles.Count.ToString();
                    ////else
                    ////    CmbBox_In.SelectedIndex = 0;

                    //if (mEndPlate[0] != null && mEndPlate[0].WireClipHoles.Count > 0)
                    //    CmbBox_In.Text = mEndPlate[0].WireClipHoles.Count.ToString();
                    //else if (mEndPlate[1] != null && mEndPlate[1].WireClipHoles.Count > 0)
                    //    CmbBox_In.Text = mEndPlate[1].WireClipHoles.Count.ToString();
                    //else
                    //    CmbBox_In.SelectedIndex = 0;
                }

                //BG 02JUL13
                private void LoadUnit(clsEndPlate EndPlate_In, ComboBox CmbBox_In)
                //=========================================================
                {
                     if (CmbBox_In.Items.Count <= 0)
                    {
                        CmbBox_In.Items.Clear();
                        CmbBox_In.Items.Add(clsUnit.eSystem.English.ToString());
                        CmbBox_In.Items.Add(clsUnit.eSystem.Metric.ToString());

                        //if (Seal_In.WireClipHoles.Unit.System.ToString() != "")
                        //    CmbBox_In.Text = Seal_In.WireClipHoles.Unit.System.ToString();
                        //else
                        //    CmbBox_In.SelectedIndex = 0;
                    }
                }

        
                private void CheckNullParams()
                //=============================
                {
                    string pMsg = "";
                    string pCaption = "";

                    if (!IsMountThread_NULL() && !IsFlow_GPM_NULL() && !IsSealDO_Null()
                        && !IsDShaft_NULL())
                    //------------------------------------------------------------------
                    {
                        ;
                    }

                    else if (IsDShaft_NULL())
                    {
                        pMsg = "Enter DShaft Value." + System.Environment.NewLine +
                                "Please open form 'Bearing Geometry & Materials'.";
                        pCaption = "DShaft Error";

                        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                        this.Close();
                        return;
                    }

                    else if (IsFlow_GPM_NULL())
                    {
                        pMsg = "Enter Flow GPM."
                                + System.Environment.NewLine +
                                "Please open form 'Performance Data'.";
                        pCaption = "GPM Error";

                        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                        this.Close();
                        return;
                    }

                    else if (IsMountThread_NULL())
                    {
                        //pMsg = "Select an appropriate Seal Mount Fixture thread first."
                        //        + System.Environment.NewLine +
                        //        "Please open form 'Bearing Design Details'.";
                        //pCaption = "Seal Thread Error";

                        //MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK,
                        //                MessageBoxIcon.Error);

                        //this.Close();
                        //return;
                    }

                    else if (IsSealDO_Null())
                    {
                        pMsg = "Select an appropriate DFinish Seal Mount Fixture thread first."
                                + System.Environment.NewLine +
                                "Please open form 'Bearing Design Details'.";
                        pCaption = "Seal DO Error";

                        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }


                private void SetControl()
                //=======================                           
                {
                    SetControls_MountHoles();

                    Boolean pEnabled;
                  
                    pEnabled = true;
                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    //{
                        txtMountHoles_CBore_Depth_Front.BackColor = Color.White;
                    //}
                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    //{
                        txtMountHoles_CBore_Depth_Back.BackColor = Color.White;
                    //}

                    SetControls_Status(pEnabled);
                   

                    ////else if (modMain.gProject.Status == "Closed" ||
                    ////         (modMain.gUser.Role != "Engineer" || modMain.gUser.Role != "Designer"))
                    ////{
                    ////    pEnabled = false;
                    ////    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    ////    {
                    ////        txtMountHoles_CBore_Depth_Front.BackColor = txtMountHoles_D_CBore_Front.BackColor;
                    ////    }
                    ////    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    ////    {
                    ////        txtMountHoles_CBore_Depth_Back.BackColor = txtMountHoles_D_CBore_Front.BackColor;
                    ////    }

                    ////    SetControls_Status(pEnabled);
                    ////}

                    mLblDrainHoles_Notes[0].Visible = false;
                    mLblDrainHoles_Notes[1].Visible = false;

                    //....Show Labels for Metric 
                    for (int i = 0; i < mlblMetric.Length ; i++)
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

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
                    {
                        cmbDrainHoles_D_Desig_Front.Left = 143;
                        cmbDrainHoles_D_Desig_Front.Top = 63;

                        cmbDrainHoles_D_Desig_Back.Left = 143;
                        cmbDrainHoles_D_Desig_Back.Top = 63;
                    }
                    else
                    {
                        cmbDrainHoles_D_Desig_Front.Left = 206;
                        cmbDrainHoles_D_Desig_Front.Top = 63;

                        cmbDrainHoles_D_Desig_Back.Left = 206;
                        cmbDrainHoles_D_Desig_Back.Top = 63;
                    }

                }


                #region "Sub-Helper Routines:"
                //***************************

                    private void SetControls_MountHoles()
                    //===================================
                    {
                        ////int pT_Pos_Left = 12;
                        ////int pC_Pos_Left = 100;

                        ////optMountHoles_Type_CBore_Front.Checked = false;
                        ////optMountHoles_Type_Thru_Front.Checked = false;
                        ////optMountHoles_Type_Thread_Front.Checked = false;
                        ////chkMountHoles_Thread_Thru_Front.Checked = false;

                        ////optMountHoles_Type_CBore_Back.Checked = false;
                        ////optMountHoles_Type_Thru_Back.Checked = false;
                        ////optMountHoles_Type_Thread_Back.Checked = false;
                        ////chkMountHoles_Thread_Thru_Back.Checked = false;

                        //////if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_GoThru)
                        //////{
                        //////    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                        //////    {
                        //////        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                        //////        {
                        //////            optMountHoles_Type_CBore_Front.Visible = true;
                        //////            optMountHoles_Type_Thru_Front.Visible = true;
                        //////            if (mEndPlate[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                        //////                optMountHoles_Type_CBore_Front.Checked = true;

                        //////            optMountHoles_Type_CBore_Front.Left = pT_Pos_Left;
                        //////            optMountHoles_Type_Thru_Front.Left = pC_Pos_Left;

                        //////            optMountHoles_Type_Thread_Front.Visible = false;
                        //////            optMountHoles_Type_Thread_Front.Visible = false;

                        //////            grpMountHoles_Type_Front.Width = 185;
                        //////            lblMountHoles_Thread_Depth_Front.Top = 141;
                        //////            txtMountHoles_Thread_Depth_Front.Top = lblMountHoles_Thread_Depth_Front.Top - 2;
                        //////        }

                        //////        if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                        //////        {
                        //////            optMountHoles_Type_CBore_Back.Visible = false;
                        //////            optMountHoles_Type_Thru_Back.Visible = false;

                        //////            optMountHoles_Type_Thread_Back.Visible = true;
                        //////            optMountHoles_Type_Thread_Back.Checked = true;
                        //////            chkMountHoles_Thread_Thru_Back.Visible = true;

                        //////            grpMountHoles_Type_Back.Width = 100;
                        //////            chkMountHoles_Thread_Thru_Back.Left = 120;
                        //////            lblMountHoles_Thread_Depth_Back.Top = 75;
                        //////            txtMountHoles_Thread_Depth_Back.Top = lblMountHoles_Thread_Depth_Back.Top - 2;
                        //////        }
                        //////    }

                        //////    else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                        //////    {
                        //////        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                        //////        {
                        //////            optMountHoles_Type_CBore_Front.Visible = false;
                        //////            optMountHoles_Type_Thru_Front.Visible = false;

                        //////            optMountHoles_Type_Thread_Front.Visible = true;
                        //////            optMountHoles_Type_Thread_Front.Checked = true;
                        //////            chkMountHoles_Thread_Thru_Front.Visible = true;

                        //////            grpMountHoles_Type_Front.Width = 100;
                        //////            chkMountHoles_Thread_Thru_Front.Left = 120;
                        //////            lblMountHoles_Thread_Depth_Front.Top = 75;
                        //////            txtMountHoles_Thread_Depth_Front.Top = lblMountHoles_Thread_Depth_Front.Top - 2;
                        //////        }

                        //////        if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                        //////        {
                        //////            optMountHoles_Type_CBore_Back.Visible = true;
                        //////            optMountHoles_Type_Thru_Back.Visible = true;
                        //////            if (mEndPlate[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                        //////                optMountHoles_Type_CBore_Back.Checked = true;

                        //////            optMountHoles_Type_CBore_Back.Left = pT_Pos_Left;
                        //////            optMountHoles_Type_Thru_Back.Left = pC_Pos_Left;

                        //////            optMountHoles_Type_Thread_Back.Visible = false;
                        //////            chkMountHoles_Thread_Thru_Back.Visible = false;

                        //////            grpMountHoles_Type_Back.Width = 185;
                        //////            lblMountHoles_Thread_Depth_Back.Top = 141;
                        //////            txtMountHoles_Thread_Depth_Back.Top = lblMountHoles_Thread_Depth_Back.Top - 2;
                        //////        }
                        //////    }
                        //////}

                        //////else
                        //////{
                        ////    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
                        ////    {
                        ////        if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                        ////        {
                        ////            optMountHoles_Type_CBore_Front.Visible = true;
                        ////            optMountHoles_Type_Thru_Front.Visible = true;
                        ////            optMountHoles_Type_Thru_Front.Enabled = true;      

                        ////            optMountHoles_Type_CBore_Front.Left = pT_Pos_Left;
                        ////            optMountHoles_Type_Thru_Front.Left = pC_Pos_Left;

                        ////            optMountHoles_Type_Thread_Front.Visible = false;
                        ////            chkMountHoles_Thread_Thru_Front.Visible = false;

                        ////            if (modMain.gProject.Product.EndPlate[0].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.C)
                        ////            {
                        ////                optMountHoles_Type_CBore_Front.Checked = true;
                        ////            }

                        ////            else if (modMain.gProject.Product.EndPlate[0].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.H)
                        ////            {
                        ////                optMountHoles_Type_Thru_Front.Checked = true;
                        ////            }

                        ////            grpMountHoles_Type_Front.Width = 185;
                        ////        }

                        ////        if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                        ////        {
                        ////            optMountHoles_Type_CBore_Back.Visible = true;
                        ////            optMountHoles_Type_Thru_Back.Visible = true;
                        ////            optMountHoles_Type_Thru_Back.Enabled = false;   //AES 03AUG18

                        ////            optMountHoles_Type_CBore_Back.Left = pT_Pos_Left;
                        ////            optMountHoles_Type_Thru_Back.Left = pC_Pos_Left;

                        ////            optMountHoles_Type_Thread_Back.Visible = false;
                        ////            chkMountHoles_Thread_Thru_Back.Visible = false;

                        ////            if (modMain.gProject.Product.EndPlate[1].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.C)
                        ////            {
                        ////                optMountHoles_Type_CBore_Back.Checked = true;
                        ////            }

                        ////            else if (modMain.gProject.Product.EndPlate[1].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.H)
                        ////            {
                        ////                optMountHoles_Type_Thru_Back.Checked = true;
                        ////            }

                        ////            grpMountHoles_Type_Back.Width = 185;
                        ////        }
                        ////    }
                        //////}
                    }


                    private void SetControls_Status(Boolean Enabled_In)
                    //================================================
                    {
                        //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                        //{
                            //  FRONT
                            //  ----- 
                            //
                            //txtL_Front.ReadOnly = !Enabled_In;

                            //....Mounting Holes.
                            grpMountHoles_Type_Front.Enabled = Enabled_In;

                            //if (modMain.gProject.Product.EndPlate[0].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.C)
                            //    txtMountHoles_CBore_Depth_Front.ReadOnly = !Enabled_In;
                            //else if (modMain.gProject.Product.EndPlate[0].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.T)
                            //{
                            //    txtMountHoles_Thread_Depth_Front.ReadOnly = !Enabled_In;
                            //    chkMountHoles_Thread_Thru_Front.Enabled = Enabled_In;
                            //}


                            //....Drain Holes.
                            mCmbDrainHoles_Annulus_Ratio_L_H[0].Enabled = Enabled_In;
                            mTxtDrainHoles_Annulus_D[0].ReadOnly = !Enabled_In;

                            if (!Enabled_In)
                                mTxtDrainHoles_Annulus_D[0].BackColor = mTxtDrainHoles_Count[0].BackColor;
                            else
                                mTxtDrainHoles_Annulus_D[0].BackColor = Color.White;

                            mCmbDrainHoles_D_Desig[0].Enabled = Enabled_In;
                            mTxtDrainHoles_AngStart[0].ReadOnly = !Enabled_In;
                            mCmbDrainHoles_AngBet[0].Enabled = Enabled_In;
                            mCmbDrainHoles_AngExit[0].Enabled = Enabled_In;

                            //....Temp. Sensor.
                            txtTempSensor_D_ExitHole_Front.ReadOnly = !Enabled_In;

                            //////....Wire Clip Holes.
                            ////if (modMain.gProject.Status == "Closed" ||
                            ////    modMain.gUser.Role != "Engineer")
                            ////{
                            ////    chkWireClipHoles_Front.Enabled = Enabled_In;
                            ////    cmbWireClipHoles_Count_Front.Enabled = Enabled_In;
                            ////}
                            txtWireClipHoles_DBC_Front.ReadOnly = !Enabled_In;
                            cmbWireClipHoles_UnitSystem_Front.Enabled = Enabled_In;     //BG 02JUL13

                            //........Thread.
                            cmbWireClipHoles_Thread_Dia_Desig_Front.Enabled = Enabled_In;
                            cmbWireClipHoles_Thread_Pitch_Front.Enabled = Enabled_In;
                            txtWireClipHoles_Thread_Depth_Front.ReadOnly = !Enabled_In;

                            txtWireClipHoles_AngStart_Front.ReadOnly = !Enabled_In;
                            txtWireClipHoles_AngOther1_Front.ReadOnly = !Enabled_In;
                            txtWireClipHoles_AngOther2_Front.ReadOnly = !Enabled_In;
                        //}

                        //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                        //{
                            //  BACK
                            //  ----     
                            //
                            //txtL_Back.ReadOnly = !Enabled_In;

                            //....Mounting Holes.
                            grpMountHoles_Type_Back.Enabled = Enabled_In;

                            //if (modMain.gProject.Product.EndPlate[1].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.C)
                            //    txtMountHoles_CBore_Depth_Back.ReadOnly = !Enabled_In;
                            //else if (modMain.gProject.Product.EndPlate[1].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.T)
                            //{
                            //    txtMountHoles_Thread_Depth_Back.ReadOnly = !Enabled_In;
                            //    chkMountHoles_Thread_Thru_Back.Enabled = Enabled_In;
                            //}

                            //....Drain Holes.
                            mCmbDrainHoles_Annulus_Ratio_L_H[1].Enabled = Enabled_In;
                            mTxtDrainHoles_Annulus_D[1].ReadOnly = !Enabled_In;

                            if (!Enabled_In)
                                mTxtDrainHoles_Annulus_D[1].BackColor = mTxtDrainHoles_Count[1].BackColor;
                            else
                                mTxtDrainHoles_Annulus_D[1].BackColor = Color.White;

                            mCmbDrainHoles_D_Desig[1].Enabled = Enabled_In;
                            mTxtDrainHoles_AngStart[1].ReadOnly = !Enabled_In;
                            mCmbDrainHoles_AngBet[1].Enabled = Enabled_In;
                            mCmbDrainHoles_AngExit[1].Enabled = Enabled_In;

                            //....Temp. Sensor.
                            txtTempSensor_D_ExitHole_Back.ReadOnly = !Enabled_In;

                            //////....Wire Clip Holes.
                            ////if (modMain.gProject.Status == "Closed" ||
                            ////    modMain.gUser.Role != "Engineer")
                            ////{
                            ////    chkWireClipHoles_Back.Enabled = Enabled_In;
                            ////    cmbWireClipHoles_Count_Back.Enabled = Enabled_In;
                            ////}
                            txtWireClipHoles_DBC_Back.ReadOnly = !Enabled_In;

                            cmbWireClipHoles_UnitSystem_Back.Enabled = Enabled_In;     //BG 02JUL13

                            //........Thread.
                            cmbWireClipHoles_Thread_Dia_Desig_Back.Enabled = Enabled_In;
                            cmbWireClipHoles_Thread_Pitch_Back.Enabled = Enabled_In;
                            txtWireClipHoles_Thread_Depth_Back.ReadOnly = !Enabled_In;

                            txtWireClipHoles_AngStart_Back.ReadOnly = !Enabled_In;
                            txtWireClipHoles_AngOther1_Back.ReadOnly = !Enabled_In;
                            txtWireClipHoles_AngOther2_Back.ReadOnly = !Enabled_In;
                        //}
                    }

                #endregion

            #endregion

        #endregion


        #region"DISPLAY DATA:"
        //********************

            private void DisplayData()
            //========================      
            {
                int pIndex = 0;
                int pCount = 0;

                #region "FRONT:"
                //--------------

                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    //{
                if (mEndPlate[0].Seal.Blade.Count > 1)
                {
                    pIndex = 0;

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtOD_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].OD));
                        lblOD_Front_Eng.Visible = true;
                        lblOD_Front_Eng.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate[0].OD) +"]";

                        txtDBore_Nom_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].DBore()));
                        lblDBore_Nom_Front_Eng.Visible = true;
                        lblDBore_Nom_Front_Eng.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate[0].DBore()) + "]";

                        txtL_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].L));
                    }
                    else
                    {
                        lblOD_Front_Eng.Visible = false;
                        txtOD_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].OD);
                        txtDBore_Nom_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].DBore());
                        lblDBore_Nom_Front_Eng.Visible = false;
                        txtL_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].L);
                    }

                    //#region  "Mounting Holes:"
                    ////------------------------ 

                    //    if (mEndPlate[0].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.C)
                    //    {
                    //        optMountHoles_Type_CBore_Front.Checked = true;

                    //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //        {
                    //            txtMountHoles_D_ThruHole_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].MountHoles.Screw.D_Thru));
                    //            txtMountHoles_D_CBore_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].MountHoles.Screw.D_CBore));
                    //            txtMountHoles_CBore_Depth_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].MountHoles.Depth_CBore));

                    //            lblMountHoles_CBoreDepth_LLim_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].MountHoles.CBore_Depth_LowerLimit()));
                    //            lblMountHoles_CBoreDepth_ULim_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].MountHoles.CBore_Depth_UpperLimit()));
                    //        }
                    //        else
                    //        {
                    //            txtMountHoles_D_ThruHole_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].MountHoles.Screw.D_Thru);
                    //            txtMountHoles_D_CBore_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].MountHoles.Screw.D_CBore);
                    //            txtMountHoles_CBore_Depth_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].MountHoles.Depth_CBore);

                    //            lblMountHoles_CBoreDepth_LLim_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].MountHoles.CBore_Depth_LowerLimit());
                    //            lblMountHoles_CBoreDepth_ULim_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].MountHoles.CBore_Depth_UpperLimit());
                    //        }
                    //    }

                    //    else if (mEndPlate[0].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.H)
                    //    {
                    //        optMountHoles_Type_Thru_Front.Checked = true;
                    //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //        {
                    //            txtMountHoles_D_ThruHole_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].MountHoles.Screw.D_Thru));
                    //        }
                    //        else
                    //        {
                    //            txtMountHoles_D_ThruHole_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].MountHoles.Screw.D_Thru);
                    //        }
                    //    }

                    //    else if (mEndPlate[0].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.T)
                    //    {
                    //        optMountHoles_Type_Thread_Front.Checked = true;
                    //        chkMountHoles_Thread_Thru_Front.Checked = mEndPlate[0].MountHoles.Thread_Thru;
                    //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //        {
                    //            txtMountHoles_Thread_Depth_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].MountHoles.Depth_Thread));
                    //        }
                    //        else
                    //        {
                    //            txtMountHoles_Thread_Depth_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].MountHoles.Depth_Thread);
                    //        }
                    //    }
                    //    else
                    //        optMountHoles_Type_CBore_Front.Checked = true;

                    //#endregion


                    #region "Drain:"
                    //--------------

                    if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig != null)
                    {
                        pCount = mEndPlate[pIndex].Seal.DrainHoles.Count;
                    }

                    double pAngBet = mEndPlate[pIndex].Seal.DrainHoles.AngBet;

                    //  Annulus
                    //  -------
                    if (mEndPlate[pIndex].Seal.DrainHoles.Annulus.Ratio_L_H > modMain.gcEPS)
                    {
                        mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.Annulus.Ratio_L_H, "");
                    }
                    else
                    {
                        mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].SelectedIndex = 0;
                    }                    

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[pIndex].Seal.DrainHoles.Annulus.D));
                    }
                    else
                    {
                        mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[pIndex].Seal.DrainHoles.Annulus.D);
                    }

                    IsAnnulusDCalc(mEndPlate[pIndex], ref mTxtDrainHoles_Annulus_D[pIndex]);

                    //  Drain Holes                                                                 
                    //  ----------- 
                    if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig != null)
                    {   
                        mCmbDrainHoles_D_Desig[pIndex].Text = mEndPlate[pIndex].Seal.DrainHoles.D_Desig;
                        //mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndPlate[pIndex].Seal.DrainHoles.Count);
                        //txtDrainHoles_V_Front.Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.V(), "#0.000");
                    }
                    else
                    {
                        mCmbDrainHoles_D_Desig[pIndex].SelectedIndex = -1;
                        mCmbDrainHoles_D_Desig[pIndex].SelectedIndex = 0;
                    }

                    //  Angles:
                    //  ------
                    //
                    ////....Upper & Lower Limits:
                    Populate_DrainHolesAng_Bet(mEndPlate[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mCmbDrainHoles_AngBet[pIndex]);

                    if (pAngBet > modMain.gcEPS)
                    {
                        mEndPlate[pIndex].Seal.DrainHoles.AngBet = pAngBet;
                    }

                    if (mEndPlate[pIndex].Seal.DrainHoles.AngBet != 0.0F)
                    {

                        mCmbDrainHoles_AngBet[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngBet, "#0");
                        mTxtDrainHoles_AngStart[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngStart_Horz, "#0.0");
                    }

                    if (mEndPlate[pIndex].Seal.DrainHoles.AngExit < modMain.gcEPS)
                    {
                        mEndPlate[pIndex].Seal.DrainHoles.AngExit = 45;
                    }

                    if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig != null)
                    {
                        if (pCount > 0)
                        {
                            mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(pCount); //modMain.ConvIntToStr(mEndPlate[pIndex].Seal.DrainHoles.Count);
                            mEndPlate[pIndex].Seal.DrainHoles.Count = pCount;
                        }
                        else
                        {
                            mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndPlate[pIndex].Seal.DrainHoles.Count);
                        }
                        txtDrainHoles_V_Front.Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.V(), "#0.000");
                    }

                    mCmbDrainHoles_AngExit[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngExit, "#0");
                   

                    #endregion


                    //#region  "Temp Sensor:"
                    ////--------------------- 

                    //    ////txtTempSensor_D_ExitHole_Front.Text = modMain.ConvDoubleToStr(mEndPlate[0].TempSensor_D_ExitHole, modMain.gUnit.MFormat);
                    //    ////txtTempSensor_Hole_DBC_Front.Text = modMain.ConvDoubleToStr(mEndPlate[0].TempSensor_DBC_Hole(), "#0.000");

                    //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //    {
                    //        txtTempSensor_D_ExitHole_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].TempSensor_D_ExitHole));
                    //        txtTempSensor_Hole_DBC_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].TempSensor_DBC_Hole()));
                    //    }
                    //    else
                    //    {
                    //        txtTempSensor_D_ExitHole_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].TempSensor_D_ExitHole);
                    //        txtTempSensor_Hole_DBC_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].TempSensor_DBC_Hole());
                    //    }

                    //#endregion


                    //#region  "Wire Clip Holes:"
                    ////-------------------------
                    //    //if (modMain.gProject.Product.Accessories.WireClip.Supplied)
                    //    //{
                    //    //    chkWireClipHoles_Front.Checked = true;
                    //    //    chkWireClipHoles_Front.Enabled = false;
                    //    //    SetControl_WireClipHoles_Front();
                    //    //    cmbWireClipHoles_Count_Front.Text = modMain.ConvIntToStr(modMain.gProject.Product.Accessories.WireClip.Count);
                    //    //    cmbWireClipHoles_Count_Front.Enabled = false;
                    //    //}
                    //    //else
                    //    //{
                    //    //    chkWireClipHoles_Front.Enabled = true;
                    //    //    chkWireClipHoles_Front.Checked = (mEndPlate[0]).WireClipHoles.Exists;
                    //    //    SetControl_WireClipHoles_Front();
                    //    //    cmbWireClipHoles_Count_Front.Text = modMain.ConvIntToStr(mEndPlate[0].WireClipHoles.Count);
                    //    //    cmbWireClipHoles_Count_Front.Enabled = true;
                    //    //}

                    //    chkWireClipHoles_Front.Enabled = true;
                    //    chkWireClipHoles_Front.Checked = (mEndPlate[0]).WireClipHoles.Exists;
                    //    SetControl_WireClipHoles_Front();
                    //    cmbWireClipHoles_Count_Front.Text = modMain.ConvIntToStr(mEndPlate[0].WireClipHoles.Count);
                    //    cmbWireClipHoles_Count_Front.Enabled = true;

                    //    //  Thread:
                    //    //  -------
                    //    //....Dia Desig.
                    //    cmbWireClipHoles_Thread_Dia_Desig_Front.Text = mEndPlate[0].WireClipHoles.Screw_Spec.D_Desig;

                    //    //....Unit System
                    //    cmbWireClipHoles_UnitSystem_Front.Text = mEndPlate[0].WireClipHoles.Unit.System.ToString();      //BG 03JUL13

                    //    //....Pitch.
                    //    cmbWireClipHoles_Thread_Pitch_Front.Text = modMain.ConvDoubleToStr(mEndPlate[0].WireClipHoles.Screw_Spec.Pitch, "#0.000");

                    //    //....Depth.                           
                    //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //    {
                    //        txtWireClipHoles_Thread_Depth_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].WireClipHoles.ThreadDepth));
                    //    }
                    //    else
                    //    {
                    //        txtWireClipHoles_Thread_Depth_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].WireClipHoles.ThreadDepth);
                    //    }


                    //    if (mEndPlate[0].Unit.System == clsUnit.eSystem.English)
                    //        lblWireClipHoles_LUnit_Front.Text = "in";
                    //    else if (mEndPlate[0].Unit.System == clsUnit.eSystem.Metric)
                    //        lblWireClipHoles_LUnit_Front.Text = "mm";


                    //    //  Angle
                    //    //  ------
                    //    txtWireClipHoles_AngStart_Front.Text = modMain.ConvDoubleToStr(mEndPlate[0].WireClipHoles.AngStart, "#0");
                    //    txtWireClipHoles_AngOther1_Front.Text = modMain.ConvDoubleToStr(mEndPlate[0].WireClipHoles.AngOther[0], "#0");
                    //    txtWireClipHoles_AngOther2_Front.Text = modMain.ConvDoubleToStr(mEndPlate[0].WireClipHoles.AngOther[1], "#0");

                    //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //    {
                    //        txtWireClipHoles_DBC_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].WireClipHoles.DBC));
                    //    }
                    //    else
                    //    {
                    //        txtWireClipHoles_DBC_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].WireClipHoles.DBC);
                    //    }

                    //    DisplayOtherAngle(mEndPlate[0], lblWireClipHole_AngOther_Front, mTxtBoxWireClipHole_Front);

                    //#endregion
                    //}
                }
                #endregion


                #region "BACK:"
                //-------------
                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    //{
                if (mEndPlate[1].Seal.Blade.Count > 1)
                {
                    pIndex = 1;

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtOD_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].OD));
                        lblOD_Back_Eng.Visible = true;
                        lblOD_Back_Eng.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate[1].OD) + "]";
                        txtL_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].L));

                        txtDBore_Nom_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].DBore()));
                        lblDBore_Nom_Back_Eng.Visible = true;
                        lblDBore_Nom_Back_Eng.Text = "[" + modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate[1].DBore()) + "]";
                    }
                    else
                    {
                        txtOD_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].OD);
                        lblOD_Back_Eng.Visible = false;
                        txtL_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].L);
                        txtDBore_Nom_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].DBore());
                        lblDBore_Nom_Back_Eng.Visible = false;
                    }

                    ////#region  "Mounting Holes:"
                    //////------------------------ 

                    ////    if (mEndPlate[1].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.C)
                    ////    {
                    ////        optMountHoles_Type_CBore_Back.Checked = true;
                    ////        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    ////        {
                    ////            txtMountHoles_D_ThruHole_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].MountHoles.Screw.D_Thru));
                    ////            txtMountHoles_D_CBore_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].MountHoles.Screw.D_CBore));
                    ////            txtMountHoles_CBore_Depth_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].MountHoles.Depth_CBore));

                    ////            lblMountHoles_CBoreDepth_LLim_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].MountHoles.CBore_Depth_LowerLimit()));
                    ////            lblMountHoles_CBoreDepth_ULim_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].MountHoles.CBore_Depth_UpperLimit()));
                    ////        }
                    ////        else
                    ////        {
                    ////            txtMountHoles_D_ThruHole_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].MountHoles.Screw.D_Thru);
                    ////            txtMountHoles_D_CBore_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].MountHoles.Screw.D_CBore);
                    ////            txtMountHoles_CBore_Depth_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].MountHoles.Depth_CBore);

                    ////            lblMountHoles_CBoreDepth_LLim_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].MountHoles.CBore_Depth_LowerLimit());
                    ////            lblMountHoles_CBoreDepth_ULim_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].MountHoles.CBore_Depth_UpperLimit());
                    ////        }
                    ////    }

                    ////    else if (mEndPlate[1].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.H)
                    ////    {
                    ////        optMountHoles_Type_Thru_Back.Checked = true;
                    ////        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    ////        {
                    ////            txtMountHoles_D_ThruHole_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].MountHoles.Screw.D_Thru));
                    ////        }
                    ////        else
                    ////        {
                    ////            txtMountHoles_D_ThruHole_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].MountHoles.Screw.D_Thru);
                    ////        }
                    ////    }

                    ////    else if (mEndPlate[1].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.T)
                    ////    {
                    ////        optMountHoles_Type_Thread_Back.Checked = true;
                    ////        chkMountHoles_Thread_Thru_Back.Checked = mEndPlate[1].MountHoles.Thread_Thru;
                    ////        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    ////        {
                    ////            txtMountHoles_Thread_Depth_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].MountHoles.Depth_Thread));
                    ////        }
                    ////        else
                    ////        {
                    ////            txtMountHoles_Thread_Depth_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].MountHoles.Depth_Thread);
                    ////        }
                    ////    }
                    ////    else
                    ////        optMountHoles_Type_CBore_Back.Checked = true;

                    ////#endregion


                    #region "Drain:"
                    //-----------------

                    pCount = 0;
                    if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig != null)
                    {
                        pCount = mEndPlate[pIndex].Seal.DrainHoles.Count;
                    }
                    double pAngBet = mEndPlate[pIndex].Seal.DrainHoles.AngBet;

                    //  Annulus
                    //  -------
                    if (mEndPlate[pIndex].Seal.DrainHoles.Annulus.Ratio_L_H > modMain.gcEPS)
                    {
                        mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.Annulus.Ratio_L_H, "");
                    }
                    else
                    {
                        mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].SelectedIndex = 0;
                    }  
                                        
                    //mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.Annulus.D, "#0.000");
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[pIndex].Seal.DrainHoles.Annulus.D));
                    }
                    else
                    {
                        mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[pIndex].Seal.DrainHoles.Annulus.D);
                    }

                    IsAnnulusDCalc(mEndPlate[pIndex], ref mTxtDrainHoles_Annulus_D[pIndex]);


                    //  Drain Holes                                                                 
                    //  -----------
                    if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig != null)
                    {                       
                        mCmbDrainHoles_D_Desig[pIndex].Text = mEndPlate[pIndex].Seal.DrainHoles.D_Desig;
                        //mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndPlate[pIndex].Seal.DrainHoles.Count);
                        //txtDrainHoles_V_Back.Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.V(), "#0.000");
                    }
                    else
                    {
                        mCmbDrainHoles_D_Desig[pIndex].SelectedIndex = -1;
                        mCmbDrainHoles_D_Desig[pIndex].SelectedIndex = 0;
                    }


                    //  Angles:
                    //  ------

                    Populate_DrainHolesAng_Bet(mEndPlate[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mCmbDrainHoles_AngBet[pIndex]);

                    if (pAngBet > modMain.gcEPS)
                    {
                        mEndPlate[pIndex].Seal.DrainHoles.AngBet = pAngBet;
                    }

                    if (mEndPlate[pIndex].Seal.DrainHoles.AngBet != 0.0F)
                    {
                        mCmbDrainHoles_AngBet[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngBet, "#0");
                        mTxtDrainHoles_AngStart[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngStart_Horz, "#0.0");
                    }

                    if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig != null)
                    {
                        if (pCount > 0)
                        {
                            mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(pCount); //modMain.ConvIntToStr(mEndPlate[pIndex].Seal.DrainHoles.Count);
                            mEndPlate[pIndex].Seal.DrainHoles.Count = pCount;
                        }
                        else
                        {
                            mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndPlate[pIndex].Seal.DrainHoles.Count);
                        }

                        txtDrainHoles_V_Back.Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.V(), "#0.000");
                    }

                    mCmbDrainHoles_AngExit[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngExit, "#0");

                    #endregion


                    //#region  "Temp Sensor:"
                    ////---------------------

                    //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //    {
                    //        txtTempSensor_D_ExitHole_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].TempSensor_D_ExitHole));
                    //        txtTempSensor_Hole_DBC_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].TempSensor_DBC_Hole()));
                    //    }
                    //    else
                    //    {
                    //        txtTempSensor_D_ExitHole_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].TempSensor_D_ExitHole);
                    //        txtTempSensor_Hole_DBC_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].TempSensor_DBC_Hole());
                    //    }


                    //#endregion


                    //#region "Wire Clip Holes:"
                    ////  ----------------------
                    //    //if (modMain.gProject.Product.Accessories.WireClip.Supplied)
                    //    //{
                    //    //    chkWireClipHoles_Back.Checked = true;
                    //    //    chkWireClipHoles_Back.Enabled = false;
                    //    //    SetControl_WireClipHoles_Back();
                    //    //    cmbWireClipHoles_Count_Back.Text = modMain.ConvIntToStr(modMain.gProject.Product.Accessories.WireClip.Count);
                    //    //    cmbWireClipHoles_Count_Back.Enabled = false;
                    //    //}
                    //    //else
                    //    //{
                    //    //    chkWireClipHoles_Back.Enabled = true;
                    //    //    chkWireClipHoles_Back.Checked = mEndPlate[1].WireClipHoles.Exists;
                    //    //    SetControl_WireClipHoles_Back();
                    //    //    cmbWireClipHoles_Count_Back.Text = modMain.ConvIntToStr(mEndPlate[1].WireClipHoles.Count);
                    //    //    cmbWireClipHoles_Count_Back.Enabled = true;
                    //    //}

                    //    chkWireClipHoles_Back.Enabled = true;
                    //    chkWireClipHoles_Back.Checked = mEndPlate[1].WireClipHoles.Exists;
                    //    SetControl_WireClipHoles_Back();
                    //    cmbWireClipHoles_Count_Back.Text = modMain.ConvIntToStr(mEndPlate[1].WireClipHoles.Count);
                    //    cmbWireClipHoles_Count_Back.Enabled = true;

                    //    //  Thread:
                    //    //  -------
                    //    //....Dia Desig.
                    //    cmbWireClipHoles_Thread_Dia_Desig_Back.Text = mEndPlate[1].WireClipHoles.Screw_Spec.D_Desig;


                    //    //....Unit System
                    //    cmbWireClipHoles_UnitSystem_Back.Text = mEndPlate[1].WireClipHoles.Unit.System.ToString();       //BG 03JUL13

                    //    //....Pitch.
                    //    cmbWireClipHoles_Thread_Pitch_Back.Text = modMain.ConvDoubleToStr(mEndPlate[1].WireClipHoles.Screw_Spec.Pitch, "");

                    //    //....Depth.

                    //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //    {
                    //        txtWireClipHoles_Thread_Depth_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].WireClipHoles.ThreadDepth));
                    //    }
                    //    else
                    //    {
                    //        txtWireClipHoles_Thread_Depth_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].WireClipHoles.ThreadDepth);
                    //    }


                    //    if (mEndPlate[1].Unit.System == clsUnit.eSystem.English)
                    //        lblWireClipHoles_LUnit_Back.Text = "in";
                    //    else if (mEndPlate[1].Unit.System == clsUnit.eSystem.Metric)
                    //        lblWireClipHoles_LUnit_Back.Text = "mm";


                    //    //  Angle
                    //    //  ------
                    //    txtWireClipHoles_AngStart_Back.Text = modMain.ConvDoubleToStr(mEndPlate[1].WireClipHoles.AngStart, "#0");
                    //    txtWireClipHoles_AngOther1_Back.Text = modMain.ConvDoubleToStr(mEndPlate[1].WireClipHoles.AngOther[0], "#0");
                    //    txtWireClipHoles_AngOther2_Back.Text = modMain.ConvDoubleToStr(mEndPlate[1].WireClipHoles.AngOther[1], "#0");

                    //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //    {
                    //        txtWireClipHoles_DBC_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].WireClipHoles.DBC));
                    //    }
                    //    else
                    //    {
                    //        txtWireClipHoles_DBC_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].WireClipHoles.DBC);
                    //    }

                    //    DisplayOtherAngle(mEndPlate[1], lblWireClipHole_AngOther_Back, mTxtBoxWireClipHole_Back);

                    //#endregion
                    //}
                }

                #endregion
            }


            #region "Helper Routines:"
            //************************

                #region "CHECK CALCULATED FIELD:"
                //------------------------------

                    //PB 28JAN13.
                    //private void IsAngStartCalc(ref TextBox TxtBox_In, clsSeal Seal_In, int Indx_In)
                    ////==============================================================================     
                    //{
                    //    Double pAngStart = modMain.ConvTextToDouble(TxtBox_In.Text);

                    //    clsSeal pTempSeal = null;
                    //    pTempSeal = (clsSeal)Seal_In.Clone();

                    //    ////....Assign Annulus ratio to get calculated Annulus D. 
                    //    //pTempSeal.DrainHoles.AngBet = Seal_In.DrainHoles.AngBet;


                    //    int pRet = 0;

                    //    if (Indx_In == 0)
                    //    {
                    //        pTempSeal.DrainHoles.AngStart = pTempSeal.DrainHoles.Calc_AngStart();
                    //        if (modMain.CompareVar(pTempSeal.DrainHoles.AngStart, pAngStart, 0, pRet) > 0)
                    //        {
                    //            TxtBox_In.ForeColor = Color.Black;
                    //            return;
                    //        }
                    //    }

                    //    else if (Indx_In == 1)
                    //    {
                    //        pTempSeal.DrainHoles.AngStart_OtherSide = pTempSeal.DrainHoles.Calc_AngStart_OtherSide();
                    //        if (modMain.CompareVar(pTempSeal.DrainHoles.AngStart_OtherSide, pAngStart, 0, pRet) > 0)
                    //        {
                    //            TxtBox_In.ForeColor = Color.Black;
                    //            return;
                    //        }
                    //    }

                    //    TxtBox_In.ForeColor = Color.Blue;
                    //    pTempSeal = null;
                    //}

                    //private void IsAnnulusDCalc(ref TextBox TxtBox_In, clsSeal Seal_In)
                    ////=================================================================    
                    //{
                    //    Double pAnnulusD = modMain.ConvTextToDouble(TxtBox_In.Text);
                    //    clsSeal pTempSeal = null;
                    //    pTempSeal = (clsSeal)Seal_In.Clone();

                    //    //....Assign Annulus ratio to get calculated Annulus D. 
                    //    pTempSeal.DrainHoles.Annulus_Ratio_L_H = Seal_In.DrainHoles.Annulus.Ratio_L_H;
                    //    pTempSeal.DrainHoles.Annulus_D = pTempSeal.DrainHoles.Calc_Annulus_D();

                    //    int pRet = 0;

                    //    if (modMain.CompareVar(pTempSeal.DrainHoles.Annulus.D, pAnnulusD, 3, pRet) > 0)
                    //    {
                    //        TxtBox_In.ForeColor = Color.Black;
                    //        return;
                    //    }

                    //    TxtBox_In.ForeColor = Color.Blue;
                    //    pTempSeal = null;
                    //}


                    private void IsAnnulusDCalc(clsEndPlate EndPlate_In, ref TextBox TxtBox_In)
                    //=================================================================    
                    {
                        Double pAnnulusD = modMain.ConvTextToDouble(TxtBox_In.Text);

                        clsEndPlate pEndPlate = (clsEndPlate)EndPlate_In.Clone(); 

                        //clsEndPlate.clsSeal pTempSeal = null;
                        //pTempSeal = (clsEndPlate.clsSeal)EndPlate_In.Clone();

                        //....Assign Annulus ratio to get calculated Annulus D. 
                        pEndPlate.Seal.DrainHoles.Annulus_Ratio_L_H = EndPlate_In.Seal.DrainHoles.Annulus.Ratio_L_H;
                        pEndPlate.Seal.DrainHoles.Annulus_D = pEndPlate.Seal.DrainHoles.Annulus.D;


                        for (int i = 2; i < mDrainHoles_Annulus_D_Calc.Length; i++)
                        {
                            if (Math.Abs(pEndPlate.Seal.DrainHoles.Annulus.D - mDrainHoles_Annulus_D_Calc[i]) <= modMain.gcEPS)
                            {
                                TxtBox_In.ForeColor = Color.Blue;
                                return;
                            }
                        }

                        TxtBox_In.ForeColor = Color.Black;
                        pEndPlate = null;
                    }

                #endregion


                //private void DisplayOtherAngle(clsSeal Seal_In, Label LblBox_In, TextBox[] TxtBox_In)
                ////===================================================================================
                //{
                //    if (Seal_In.WireClipHoles.Count == 1)
                //        LblBox_In.Visible = false;
                //    else if (Seal_In.WireClipHoles.Count > 0)
                //        LblBox_In.Visible = true;

                //    for (int i = 1; i < TxtBox_In.Length; i++)
                //        TxtBox_In[i].Visible = false;

                //    for (int i = 1; i < Seal_In.WireClipHoles.Count; i++)
                //        TxtBox_In[i].Visible = true;
                //}

            #endregion

        #endregion


        #region "CONTROL EVENT RELATED ROUTINE:"
        //*************************************

            private void tbEndSealDesignDetails_SelectedIndexChanged(object sender, EventArgs e)
            //==================================================================================
            {
                if (mblnTab_ManuallyChanged)
                {
                    SaveData();
                    mblnTab_ManuallyChanged = false;
                }

                if (tbEndSealDesignDetails.SelectedIndex == 1)// && modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                {
                    //....When the "Back" tab is clicked and the End Config - FRONT = SEAL, mimic the display on the Front tab to 
                    //........the "Back" tab.
                    //
                    mCmbDrainHoles_Annulus_Ratio_L_H[1].Text = modMain.ConvDoubleToStr(mEndPlate[0].Seal.DrainHoles.Annulus.Ratio_L_H, "");

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mTxtDrainHoles_Annulus_D[1].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].Seal.DrainHoles.Annulus.D));
                    }
                    else
                    {
                        mTxtDrainHoles_Annulus_D[1].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].Seal.DrainHoles.Annulus.D);
                    }

                    //mTxtDrainHoles_Annulus_D[1].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].Seal.DrainHoles.Annulus.D);
                    mTxtDrainHoles_Annulus_D[1].ForeColor    = mTxtDrainHoles_Annulus_D[0].ForeColor;

                    mCmbDrainHoles_D_Desig[1].Text           = mEndPlate[0].Seal.DrainHoles.D_Desig;                   
                    mTxtDrainHoles_V[1].Text                 = modMain.ConvDoubleToStr(mEndPlate[0].Seal.DrainHoles.V(), "#0.000");
                    lblDrainHoles_D_Desig_Back_MM.Text       = lblDrainHoles_D_Desig_Front_MM.Text;

                    //AES 06DEC18
                    //Populate_DrainHolesAng_Bet(mEndPlate[1], mLblDrainHoles_AngBet_LLim[1], mLblDrainHoles_AngBet_ULim[1],
                    //                           mCmbDrainHoles_AngBet[1]);

                    Populate_DrainHolesAng_Bet(mEndPlate[0], mLblDrainHoles_AngBet_LLim[0], mCmbDrainHoles_AngBet[1]);                    

                    if (mEndPlate[0].Seal.DrainHoles.AngBet != 0.0F)
                    {                      
                        mCmbDrainHoles_AngBet[1].Text        = modMain.ConvDoubleToStr(mEndPlate[0].Seal.DrainHoles.AngBet, "#0");
                        //mLblDrainHoles_AngBet_ULim[1].Text = mLblDrainHoles_AngBet_ULim[0].Text;
                        mLblDrainHoles_AngBet_LLim[1].Text = mLblDrainHoles_AngBet_LLim[0].Text;
                        mTxtDrainHoles_AngStart[1].Text      = modMain.ConvDoubleToStr(mEndPlate[0].Seal.DrainHoles.AngStart_OtherSide(), "#0.0");
                        //mTxtDrainHoles_AngStart[1].Text = modMain.ConvDoubleToStr(mEndPlate[1].Seal.DrainHoles.AngStart_Horz, "#0.0");        //AES 06DEC18
                        mTxtDrainHoles_AngStart[1].ForeColor = mTxtDrainHoles_AngStart[0].ForeColor;
                    }

                    mTxtDrainHoles_Count[1].Text = mTxtDrainHoles_Count[0].Text; //modMain.ConvIntToStr(mEndPlate[0].Seal.DrainHoles.Count);

                    //mCmbDrainHoles_AngExit[1].Text           = modMain.ConvDoubleToStr(mEndPlate[1].Seal.DrainHoles.AngExit, "#0");
                    mCmbDrainHoles_AngExit[1].Text = modMain.ConvDoubleToStr(mEndPlate[0].Seal.DrainHoles.AngExit, "#0");               //AES 06DEC18

                    //CheckAndAct_DrainHoles_Crossing_180BearingSL(1);
                }
            }


            #region "OPTION BUTTON RELATED ROUTINE:"
            //--------------------------------------

                private void optButton_CheckedChanged(object sender, EventArgs e)
                //================================================================
                {
                    RadioButton pOptButton = (RadioButton)sender;

                    switch (pOptButton.Name)
                    {
                        //case "optMountHoles_Type_CBore_Front":
                        //    //--------------------------------
                        //    //....CBore 
                        //    if (pOptButton.Checked)
                        //        mEndPlate[0].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.C;

                        //    //....Thru'
                        //    chkMountHoles_Thread_Thru_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;

                        //    //....Thru' D
                        //    lblMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                        //    txtMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                        //    //....CBore D
                        //    lblMountHoles_D_CBore_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                        //    txtMountHoles_D_CBore_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                        //    //....CBore Depth
                        //    lblMountHoles_Depth_CBore_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                        //    txtMountHoles_CBore_Depth_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                        //    //....CBore Depth Limits
                        //    lblMountHoles_Limits_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                        //    lblMountHoles_CBoreDepth_ULim_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                        //    lblMountHoles_CBoreDepthULim_Front_Upper.Visible = optMountHoles_Type_CBore_Front.Checked;
                        //    lblMountHoles_CBoreDepth_LLim_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                        //    lblMountHoles_CBoreDepthLLim_Front_Lower.Visible = optMountHoles_Type_CBore_Front.Checked;

                        //    //....Thread Depth
                        //    lblMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;
                        //    txtMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;

                        //    break;


                        //case "optMountHoles_Type_Thru_Front":
                        //    //-------------------------------
                        //    //....Thru'
                        //    if (pOptButton.Checked)
                        //        mEndPlate[0].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.H;

                        //    //....Thru'
                        //    chkMountHoles_Thread_Thru_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                        //    //....Thru' D
                        //    lblMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_Thru_Front.Checked;
                        //    txtMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_Thru_Front.Checked;

                        //    //....CBore D
                        //    lblMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                        //    txtMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                        //    //....CBore Depth
                        //    lblMountHoles_Depth_CBore_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                        //    txtMountHoles_CBore_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                        //    //....CBore Depth Limits
                        //    lblMountHoles_Limits_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                        //    lblMountHoles_CBoreDepth_ULim_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                        //    lblMountHoles_CBoreDepthULim_Front_Upper.Visible = !optMountHoles_Type_Thru_Front.Checked;
                        //    lblMountHoles_CBoreDepth_LLim_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                        //    lblMountHoles_CBoreDepthLLim_Front_Lower.Visible = !optMountHoles_Type_Thru_Front.Checked;

                        //    //....Thread Depth
                        //    lblMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                        //    txtMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                        //    break;


                        //case "optMountHoles_Type_Thread_Front":
                        //    //---------------------------------
                        //    //....Thread
                        //    if (pOptButton.Checked)
                        //        mEndPlate[0].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.T;

                        //    //....Thru'
                        //    chkMountHoles_Thread_Thru_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                        //    //....Thru' D
                        //    lblMountHoles_D_ThruHole_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                        //    txtMountHoles_D_ThruHole_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                        //    //....CBore D
                        //    lblMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                        //    txtMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                        //    //....CBore Depth
                        //    lblMountHoles_Depth_CBore_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                        //    txtMountHoles_CBore_Depth_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                        //    //....CBore Depth Limits
                        //    lblMountHoles_Limits_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                        //    lblMountHoles_CBoreDepth_ULim_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                        //    lblMountHoles_CBoreDepthULim_Front_Upper.Visible = !optMountHoles_Type_Thread_Front.Checked;
                        //    lblMountHoles_CBoreDepth_LLim_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                        //    lblMountHoles_CBoreDepthLLim_Front_Lower.Visible = !optMountHoles_Type_Thread_Front.Checked;

                        //    //....Thread Depth
                        //    lblMountHoles_Thread_Depth_Front.Visible = optMountHoles_Type_Thread_Front.Checked;
                        //    txtMountHoles_Thread_Depth_Front.Visible = optMountHoles_Type_Thread_Front.Checked;

                        //    break;


                        //case "optMountHoles_Type_CBore_Back":
                        //    //-------------------------------
                        //    //....CBore 

                        //    if (pOptButton.Checked)
                        //        mEndPlate[1].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.C;

                        //    //....Thru'
                        //    chkMountHoles_Thread_Thru_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;

                        //    //....Thru' D
                        //    lblMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                        //    txtMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_CBore_Back.Checked;

                        //    //....CBore D
                        //    lblMountHoles_D_CBore_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                        //    txtMountHoles_D_CBore_Back.Visible = optMountHoles_Type_CBore_Back.Checked;

                        //    //....CBore Depth
                        //    lblMountHoles_Depth_CBore_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                        //    txtMountHoles_CBore_Depth_Back.Visible = optMountHoles_Type_CBore_Back.Checked;

                        //    //....CBore Depth Limits
                        //    lblMountHoles_Limits_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                        //    lblMountHoles_CBoreDepth_ULim_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                        //    lblMountHoles_CBoreDepthULim_Back_Upper.Visible = optMountHoles_Type_CBore_Back.Checked;
                        //    lblMountHoles_CBoreDepth_LLim_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                        //    lblMountHoles_CBoreDepthLLim_Back_Lower.Visible = optMountHoles_Type_CBore_Back.Checked;

                        //    //....Thread Depth
                        //    lblMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;
                        //    txtMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;

                        //    break;


                        //case "optMountHoles_Type_Thru_Back":
                        //    //------------------------------
                        //    //....Thru'
                        //    if (pOptButton.Checked)
                        //        mEndPlate[1].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.H;

                        //    //....Thru'
                        //    chkMountHoles_Thread_Thru_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                        //    //....Thru' D
                        //    lblMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_Thru_Back.Checked;
                        //    txtMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_Thru_Back.Checked;

                        //    //....CBore D
                        //    lblMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                        //    txtMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                        //    //....CBore Depth
                        //    lblMountHoles_Depth_CBore_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                        //    txtMountHoles_CBore_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                        //    //....CBore Depth Limits
                        //    lblMountHoles_Limits_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                        //    lblMountHoles_CBoreDepth_ULim_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                        //    lblMountHoles_CBoreDepthULim_Back_Upper.Visible = !optMountHoles_Type_Thru_Back.Checked;
                        //    lblMountHoles_CBoreDepth_LLim_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                        //    lblMountHoles_CBoreDepthLLim_Back_Lower.Visible = !optMountHoles_Type_Thru_Back.Checked;

                        //    //....Thread Depth
                        //    lblMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                        //    txtMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                        //    break;


                        //case "optMountHoles_Type_Thread_Back":
                        //    //--------------------------------
                        //    //....Thread
                        //    if (pOptButton.Checked)
                        //        mEndPlate[1].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.T;

                        //    //....Thru'
                        //    chkMountHoles_Thread_Thru_Back.Visible = optMountHoles_Type_Thread_Back.Checked;

                        //    //....Thru' D
                        //    lblMountHoles_D_ThruHole_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                        //    txtMountHoles_D_ThruHole_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                        //    //....CBore D
                        //    lblMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                        //    txtMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                        //    //....CBore Depth
                        //    lblMountHoles_Depth_CBore_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                        //    txtMountHoles_CBore_Depth_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                        //    //....CBore Depth Limits
                        //    lblMountHoles_Limits_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                        //    lblMountHoles_CBoreDepth_ULim_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                        //    lblMountHoles_CBoreDepthULim_Back_Upper.Visible = !optMountHoles_Type_Thread_Back.Checked;
                        //    lblMountHoles_CBoreDepth_LLim_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                        //    lblMountHoles_CBoreDepthLLim_Back_Lower.Visible = !optMountHoles_Type_Thread_Back.Checked;

                        //    //....Thread Depth
                        //    lblMountHoles_Thread_Depth_Back.Visible = optMountHoles_Type_Thread_Back.Checked;
                        //    txtMountHoles_Thread_Depth_Back.Visible = optMountHoles_Type_Thread_Back.Checked;

                        //    break;
                    }
                }

            #endregion


            #region "CHECKBOX RELATED ROUTINE:"
            //--------------------------------
                     
                private void chkBox_CheckedChanged(object sender, EventArgs e)
                //=============================================================
                {
                    CheckBox pChkBox = (CheckBox)sender;

                    switch (pChkBox.Name)
                     {
                        // case "chkWireClipHoles_Front":
                        ////----------------------------
                        //  mEndPlate[0].WireClipHoles.Exists = pChkBox.Checked;
                        //  SetControl_WireClipHoles_Front();
                        //  break;

                        // case "chkWireClipHoles_Back":
                        ////-----------------------------
                        //  mEndPlate[1].WireClipHoles.Exists = pChkBox.Checked;
                        //  SetControl_WireClipHoles_Back();
                        //  break;

                         case "chkMountHoles_Thread_Thru_Front":
                          //------------------------------------
                          lblMountHoles_Thread_Depth_Front.Visible = !chkMountHoles_Thread_Thru_Front.Checked;
                          txtMountHoles_Thread_Depth_Front.Visible = !chkMountHoles_Thread_Thru_Front.Checked;
                          break;

                         case "chkMountHoles_Thread_Thru_Back":
                          //-----------------------------------
                          lblMountHoles_Thread_Depth_Back.Visible = !chkMountHoles_Thread_Thru_Back.Checked;
                          txtMountHoles_Thread_Depth_Back.Visible = !chkMountHoles_Thread_Thru_Back.Checked;
                          break;
                     }
                }


                #region "Helper Routines:"
                //************************
               
                    private void SetControl_WireClipHoles_Front()
                    //===========================================
                    {
                        lblWireClipHole_Count_Front.Visible = chkWireClipHoles_Front.Checked;
                        cmbWireClipHoles_Count_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_Thread_Front.Visible = chkWireClipHoles_Front.Checked;
                        lblWireClipHole_Thread_Size_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_ThreadDia_Desig_Front.Visible = chkWireClipHoles_Front.Checked;
                        cmbWireClipHoles_Thread_Dia_Desig_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_Thread_Pitch_Front.Visible = chkWireClipHoles_Front.Checked;
                        cmbWireClipHoles_Thread_Pitch_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblUnit_Front.Visible = chkWireClipHoles_Front.Checked;
                        lblWireClipHoles_LUnit_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_Thread_Depth_Front.Visible = chkWireClipHoles_Front.Checked;
                        txtWireClipHoles_Thread_Depth_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_wrt_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_DBC_Front.Visible = chkWireClipHoles_Front.Checked;
                        txtWireClipHoles_DBC_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_Angles_Front.Visible = chkWireClipHoles_Front.Checked;
                        lblWireClipHole_Ang_Start_Front.Visible = chkWireClipHoles_Front.Checked;

                        txtWireClipHoles_AngStart_Front.Visible = chkWireClipHoles_Front.Checked;

                        //if (mEndPlate[0].WireClipHoles.Count > 1)
                        //{
                        //    lblWireClipHole_AngOther_Front.Visible = chkWireClipHoles_Front.Checked;
                        //    txtWireClipHoles_AngOther1_Front.Visible = chkWireClipHoles_Front.Checked;
                        //}

                        //if (mEndPlate[0].WireClipHoles.Count > 2)
                        //    txtWireClipHoles_AngOther2_Front.Visible = chkWireClipHoles_Front.Checked;
                    }


                    private void SetControl_WireClipHoles_Back()
                    //==========================================
                    {
                        lblWireClipHole_Count_Back.Visible = chkWireClipHoles_Back.Checked;
                        cmbWireClipHoles_Count_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_Thread_Back.Visible = chkWireClipHoles_Back.Checked;
                        lblWireClipHole_Thread_Size_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_ThreadDia_Desig_Back.Visible = chkWireClipHoles_Back.Checked;
                        cmbWireClipHoles_Thread_Dia_Desig_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_Thread_Pitch_Back.Visible = chkWireClipHoles_Back.Checked;
                        cmbWireClipHoles_Thread_Pitch_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblUnit_Back.Visible = chkWireClipHoles_Back.Checked;
                        lblWireClipHoles_LUnit_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_Thread_Depth_Back.Visible = chkWireClipHoles_Back.Checked;
                        txtWireClipHoles_Thread_Depth_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_wrt_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_DBC_Back.Visible = chkWireClipHoles_Back.Checked;
                        txtWireClipHoles_DBC_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_Angles_Back.Visible = chkWireClipHoles_Back.Checked;
                        lblWireClipHole_Ang_Start_Back.Visible = chkWireClipHoles_Back.Checked;
                        txtWireClipHoles_AngStart_Back.Visible = chkWireClipHoles_Back.Checked;
                    

                        //if (mEndPlate[1].WireClipHoles.Count > 1)//|| mEndPlate[1].WireClipHoles.Count == 0
                        //{
                        //    lblWireClipHole_AngOther_Back.Visible = chkWireClipHoles_Back.Checked;
                        //    txtWireClipHoles_AngOther1_Back.Visible = chkWireClipHoles_Back.Checked;
                        //}


                        //if (mEndPlate[1].WireClipHoles.Count > 2)//|| mEndPlate[1].WireClipHoles.Count == 0
                        //    txtWireClipHoles_AngOther2_Back.Visible = chkWireClipHoles_Back.Checked;                   
                    }

                #endregion

            #endregion


            #region "TEXTBOX RELATED ROUTINES:"
            //---------------------------------

                private void TextBox_KeyDown(object sender, KeyEventArgs e)
                //=========================================================
                {
                    TextBox pTxtBox = (TextBox)sender;
                    pTxtBox.ForeColor = Color.Black;

                    switch (pTxtBox.Name)                                   
                    {
                        case "txtDrainHoles_Annulus_D_Front":
                        case "txtDrainHoles_Annulus_D_Back":
                            mblnDrainHoles_Annulus_D_ManuallyChanged = true;
                            break;
                    }
                }


                //....PB 29JAN13. Not needed. 
                //private void TextBox_MouseDown(object sender, MouseEventArgs e)
                ////=========================================================
                //{
                //    TextBox pTxtBox = (TextBox)sender;
                //    pTxtBox.ForeColor = Color.Black;
                //}


                private void TxtBox_TextChanged(object sender, EventArgs e)
                //==========================================================   
                {
                    TextBox pTxtBox = (TextBox)sender;
                    int pIndx = 0;

                    double pVal = 0.0;


                    switch (pTxtBox.Name)
                    {
                        case "txtL_Front":
                            //------------
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mEndPlate[0].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mEndPlate[0].L = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            SetTxtForeColor_L(txtL_Front, ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.EndPlateCB_Depth_Def());
                            break;


                        case "txtL_Back":
                            //-----------
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mEndPlate[1].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mEndPlate[1].L = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            SetTxtForeColor_L(txtL_Back, ((clsJBearing)modMain.gProject.PNR.Bearing).RadB.EndPlateCB_Depth_Def());
                            break;


                        ////case "txtMountHole_Depth_CBore_Front":
                        ////    //--------------------------------
                        ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////    {
                        ////        Double pMountHole_Depth_CBore_Front = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        ////        mEndPlate[0].MountHoles.Depth_CBore = pMountHole_Depth_CBore_Front;
                        ////    }
                        ////    else
                        ////    {
                        ////        Double pMountHole_Depth_CBore_Front = modMain.ConvTextToDouble(pTxtBox.Text);
                        ////        mEndPlate[0].MountHoles.Depth_CBore = pMountHole_Depth_CBore_Front;
                        ////    }
                        ////    break;


                        ////case "txtMountHole_Depth_CBore_Back":
                        ////    //-------------------------------
                        ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////    {
                        ////        Double pMountHole_Depth_CBore_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        ////        mEndPlate[1].MountHoles.Depth_CBore = pMountHole_Depth_CBore_Back;
                        ////    }
                        ////    else
                        ////    {
                        ////        Double pMountHole_Depth_CBore_Back = modMain.ConvTextToDouble(pTxtBox.Text);
                        ////        mEndPlate[1].MountHoles.Depth_CBore = pMountHole_Depth_CBore_Back;
                        ////    }
                        ////    break;


                        ////case "txtMountHoles_Thread_Depth_Front":
                        ////    //----------------------------------
                        ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////    {
                        ////        mEndPlate[0].MountHoles.Depth_Thread = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        ////    }
                        ////    else
                        ////    {
                        ////        mEndPlate[0].MountHoles.Depth_Thread = modMain.ConvTextToDouble(pTxtBox.Text);
                        ////    }

                        ////    pVal = Math.Round(2 * ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Screw_Spec.Spec_D, 3);

                        ////    if (Math.Abs(mEndPlate[0].MountHoles.Depth_Thread - pVal) < modMain.gcEPS)
                        ////        txtMountHoles_Thread_Depth_Front.ForeColor = Color.Magenta;
                        ////    else
                        ////        txtMountHoles_Thread_Depth_Front.ForeColor = Color.Black;
                        ////    break;


                        ////case "txtMountHoles_Thread_Depth_Back":
                        ////    //---------------------------------
                        ////    //mEndPlate[1].MountHoles.Depth_Thread = modMain.ConvTextToDouble(pTxtBox.Text);
                        ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////    {
                        ////        mEndPlate[1].MountHoles.Depth_Thread = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        ////    }
                        ////    else
                        ////    {
                        ////        mEndPlate[1].MountHoles.Depth_Thread = modMain.ConvTextToDouble(pTxtBox.Text);
                        ////    }

                        ////    pVal = Math.Round(2 * ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Screw_Spec.Spec_D, 3);

                        ////    if (Math.Abs(mEndPlate[1].MountHoles.Depth_Thread - pVal) < modMain.gcEPS)
                        ////        txtMountHoles_Thread_Depth_Back.ForeColor = Color.Magenta;
                        ////    else
                        ////        txtMountHoles_Thread_Depth_Back.ForeColor = Color.Black;
                        ////    break;


                        case "txtDrainHoles_Annulus_D_Front":
                            //-------------------------------
                            pIndx = 0;

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                pVal = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                            mEndPlate[pIndx].Seal.DrainHoles.Annulus_D = pVal;

                            Populate_DrainHolesAng_Bet(mEndPlate[pIndx], mLblDrainHoles_AngBet_LLim[pIndx], mCmbDrainHoles_AngBet[pIndx]);


                            if (mblnDrainHoles_Annulus_D_ManuallyChanged)
                            {
                                //....The forecolor has already been changed to black in the MouseDown event.
                                //
                                mCmbDrainHoles_Annulus_Ratio_L_H[pIndx].Text =
                                                 modMain.ConvDoubleToStr(mEndPlate[pIndx].Seal.DrainHoles.Calc_Annulus_Ratio_L_H(), "#0.000");
                                mCmbDrainHoles_Annulus_Ratio_L_H[pIndx].ForeColor = Color.Blue;
                                mCmbDrainHoles_Annulus_Ratio_L_H[1].ForeColor = Color.Blue;

                                mblnDrainHoles_Annulus_D_ManuallyChanged = false;
                            }

                            else
                            {
                                //....Annulus D is programmatically written (not manually changed).
                                if (Math.Abs(pVal - Math.Round(mEndPlate[pIndx].Seal.DrainHoles.Calc_Annulus_D(),3)) <= modMain.gcEPS)
                                {
                                    pTxtBox.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    pTxtBox.ForeColor = Color.Black;
                                }
                            }

                            //....Back Seal: Display the same Annulus D.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //{
                            //    mTxtDrainHoles_Annulus_D[1].Text = modMain.ConvDoubleToStr(mEndPlate[pIndx].Seal.DrainHoles.Annulus.D, "#0.000");
                            //}
                            break;


                        case "txtDrainHoles_Annulus_D_Back":
                            //------------------------------

                            //if (modMain.gProject.Product.EndPlate[0].Type != clsEndPlate.eType.Seal)
                            //{
                                //....This event will be fired only when the Back Seal Controls are enabled.
                                pIndx = 1;

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    pVal = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                                }
                                else
                                {
                                    pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                                }
                                //pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                                mEndPlate[pIndx].Seal.DrainHoles.Annulus_D = pVal;

                                Populate_DrainHolesAng_Bet(mEndPlate[pIndx], mLblDrainHoles_AngBet_LLim[pIndx], mCmbDrainHoles_AngBet[pIndx]);

                                if (mblnDrainHoles_Annulus_D_ManuallyChanged)
                                {
                                    mCmbDrainHoles_Annulus_Ratio_L_H[pIndx].Text =
                                                        modMain.ConvDoubleToStr(mEndPlate[pIndx].Seal.DrainHoles.Calc_Annulus_Ratio_L_H(), "#0.000");
                                    mCmbDrainHoles_Annulus_Ratio_L_H[pIndx].ForeColor = Color.Blue;

                                    mblnDrainHoles_Annulus_D_ManuallyChanged = false;
                                }
                                else
                                {
                                    //....Annulus D is programmatically written (not manually changed).
                                    if (Math.Abs(pVal - Math.Round(mEndPlate[pIndx].Seal.DrainHoles.Calc_Annulus_D(), 3)) <= modMain.gcEPS)
                                    {
                                        pTxtBox.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        pTxtBox.ForeColor = Color.Black;
                                    }
                                }
                            //}

                            break;


                        case "txtDrainHoles_AngStart_Front":
                            //------------------------------
                            pIndx = 0;

                            pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndPlate[pIndx].Seal.DrainHoles.AngStart_Horz = pVal;

                            if (Math.Abs(pVal - mEndPlate[pIndx].Seal.DrainHoles.Calc_AngStart_Horz()) <= modMain.gcEPS)
                            {
                                //....Most likely programmatically changed.
                                pTxtBox.ForeColor = Color.Blue;
                            }
                            else
                            {   //....Manually changed.
                                //........The drain holes array is not symmetric about the Casing SL vertical.

                                pVal = mEndPlate[pIndx].Seal.DrainHoles.AngBet_ULim_NonSym();
                                //mLblDrainHoles_AngBet_ULim[pIndx].Text = modMain.ConvDoubleToStr(pVal, "#0");

                                pTxtBox.ForeColor = Color.Black;
                            }

                            CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndx);

                            //....Back Seal: Display the corresponding start angle.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //{
                            //    mTxtDrainHoles_AngStart[1].Text = modMain.ConvDoubleToStr(mEndPlate[pIndx].Seal.DrainHoles.AngStart_OtherSide(), "#0.0");
                            //}

                            break;


                        case "txtDrainHoles_AngStart_Back":
                            //-----------------------------

                            //if (modMain.gProject.Product.EndPlate[0].Type != clsEndPlate.eType.Seal)
                            //{
                                pIndx = 1;

                                pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                                mEndPlate[pIndx].Seal.DrainHoles.AngStart_Horz = pVal;

                                if (Math.Abs(pVal - mEndPlate[pIndx].Seal.DrainHoles.Calc_AngStart_Horz()) <= modMain.gcEPS)
                                {
                                    //....Most likely programmatically changed.
                                    pTxtBox.ForeColor = Color.Blue;
                                }
                                else
                                {   //....Manually changed.
                                    //........The drain holes array is not symmetric about the Casing SL vertical.

                                    pVal = mEndPlate[pIndx].Seal.DrainHoles.AngBet_ULim_NonSym();
                                    //mLblDrainHoles_AngBet_ULim[pIndx].Text = modMain.ConvDoubleToStr(pVal, "#0");

                                    pTxtBox.ForeColor = Color.Black;

                                    //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    //{
                                    //    pTxtBox.ForeColor = Color.Blue;
                                    //    pTxtBox.BackColor = mTxtDrainHoles_Count[pIndx].BackColor;
                                    //}
                                    //else
                                    //{
                                    //    pTxtBox.ForeColor = Color.Black;
                                    //}
                                }

                                CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndx);
                            //}

                            break;


                        //case "txtTempSensor_ExitHole_D_Front":
                        //    //--------------------------------
                        //    mEndPlate[0].TempSensor_D_ExitHole = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;


                        //case "txtTempSensor_ExitHole_D_Back":
                        //    //-------------------------------
                        //    mEndPlate[1].TempSensor_D_ExitHole = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;


                        //case "txtWireClipHole_Thread_Depth_Front":
                        //    //------------------------------------
                        //    mEndPlate[0].WireClipHoles.ThreadDepth = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;


                        //case "txtWireClipHole_Thread_Depth_Back":
                        //    //-----------------------------------
                        //    mEndPlate[1].WireClipHoles.ThreadDepth = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;


                        //case "txtWireClipHole_Ang_Start_Front":
                        //    //---------------------------------
                        //    mEndPlate[0].WireClipHoles.AngStart = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;


                        //case "txtWireClipHole_Ang_Start_Back":
                        //    //--------------------------------
                        //    mEndPlate[1].WireClipHoles.AngStart = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;


                        //case "txtWireClipHole_AngOther1_Front":
                        //    //--------------------------------
                        //    mWireClipHole_AngOther[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    mEndPlate[0].WireClipHoles.AngOther = mWireClipHole_AngOther;
                        //    break;


                        //case "txtWireClipHole_AngOther1_Back":
                        //    //--------------------------------
                        //    mWireClipHole_AngOther[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    mEndPlate[1].WireClipHoles.AngOther = mWireClipHole_AngOther;
                        //    break;


                        //case "txtWireClipHole_AngOther2_Front":
                        //    //---------------------------------
                        //    mWireClipHole_AngOther[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    mEndPlate[0].WireClipHoles.AngOther = mWireClipHole_AngOther;
                        //    break;


                        //case "txtWireClipHole_AngOther2_Back":
                        //    //--------------------------------
                        //    mWireClipHole_AngOther[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    mEndPlate[1].WireClipHoles.AngOther = mWireClipHole_AngOther;
                        //    break;


                        //case "txtWireClipHole_DBC_Front":
                        //    //---------------------------
                        //    mEndPlate[0].WireClipHoles.DBC = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;


                        //case "txtWireClipHole_DBC_Back":
                        //    //--------------------------
                        //    mEndPlate[1].WireClipHoles.DBC = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    break;
                    }
                }


                //private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
                ////===============================================================
                //{
                //    mblnDrainHoles_Annulus_D_Front_ManuallyChanged = true;
                //}


                #region "Helper Routines:"
                //------------------------

                    private void SetTxtForeColor_L(TextBox TxtBox_In, Double ActulVal_In)
                    //====================================================================       
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            if (System.Math.Abs(modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(TxtBox_In.Text)) - ActulVal_In) <= modMain.gcEPS)
                            {
                                TxtBox_In.ForeColor = Color.Blue;
                            }
                            else
                            {
                                TxtBox_In.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            if (System.Math.Abs(modMain.ConvTextToDouble(TxtBox_In.Text) - ActulVal_In) <= modMain.gcEPS)
                            {
                                TxtBox_In.ForeColor = Color.Blue;
                            }
                            else
                            {
                                TxtBox_In.ForeColor = Color.Black;
                            }
                        }
                        
                    }

                #endregion


            #endregion


            #region "COMBOBOX RELATED ROUTINE:"
            //--------------------------------

                private void ComboBox_SelectedIndex_Txt_Changed(object sender, EventArgs e)
                //========================================================================= 
                {
                    ComboBox pCmbBox = (ComboBox)sender;
                    int pIndex = 0;

                    Double pVal = 0.0;

                    //if (pCmbBox.Text != "")
                    //{
                    //    mBearing_Radial_FP.SL.Screw_Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);       //BG 26MAR12                          
                    //    Populate_SL_Details(cmbSL_Screw_Spec_Type);
                    //}

                    switch (pCmbBox.Name)
                    {
                        case "cmbDrainHoles_Annulus_Ratio_L_H_Front":
                            //=======================================
                            pIndex = 0;

                            pVal = modMain.ConvTextToDouble(pCmbBox.Text);
                            mEndPlate[pIndex].Seal.DrainHoles.Annulus_Ratio_L_H = pVal;

                            if (!mblnDrainHoles_Annulus_D_ManuallyChanged)
                            {
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    pVal =modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[pIndex].Seal.DrainHoles.Annulus.D);
                                }
                                else
                                {
                                    pVal = mEndPlate[pIndex].Seal.DrainHoles.Annulus.D;
                                }
                                
                                mTxtDrainHoles_Annulus_D[pIndex].Text =modMain.gProject.PNR.Unit.WriteInUserL(pVal);
                            }

                            Populate_DrainHolesAng_Bet(mEndPlate[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mCmbDrainHoles_AngBet[pIndex]);

                            //....Back Seal: Display the same.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //{                                
                            //    mCmbDrainHoles_Annulus_Ratio_L_H[1].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.Annulus.Ratio_L_H, "");                                
                            //}

                            break;


                        case "cmbDrainHoles_Annulus_Ratio_L_H_Back":
                            //====================================

                            //if (modMain.gProject.Product.EndPlate[0].Type != clsEndPlate.eType.Seal)
                            //{
                                pIndex = 1;

                                pVal = modMain.ConvTextToDouble(pCmbBox.Text);
                                mEndPlate[pIndex].Seal.DrainHoles.Annulus_Ratio_L_H = pVal;

                                if (!mblnDrainHoles_Annulus_D_ManuallyChanged)
                                {
                                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pVal = modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[pIndex].Seal.DrainHoles.Annulus.D);
                                    }
                                    else
                                    {
                                        pVal = mEndPlate[pIndex].Seal.DrainHoles.Annulus.D;
                                    }
                                    mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.gProject.PNR.Unit.WriteInUserL(pVal);
                                   // mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.ConvDoubleToStr(pVal, "#0.000");
                                }

                                Populate_DrainHolesAng_Bet(mEndPlate[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mCmbDrainHoles_AngBet[pIndex]);
                           // }

                            break;


                        case "cmbDrainHoles_D_Desig_Front":
                            //=============================
                            pIndex = 0;
                            mEndPlate[pIndex].Seal.DrainHoles.D_Desig = pCmbBox.Text;

                            Cursor = Cursors.WaitCursor;
                            mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndPlate[pIndex].Seal.DrainHoles.Count);
                            mTxtDrainHoles_Count[pIndex].ForeColor = Color.Blue;

                            txtDrainHoles_V_Front.Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.V(), "#0.000");
                          

                            Populate_DrainHolesAng_Bet(mEndPlate[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mCmbDrainHoles_AngBet[pIndex]);

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig != "")
                                {
                                    Double pNumerator, pDenominator;
                                    Double pFinal = 0.0;
                                    if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig.Contains("/"))
                                    {
                                        if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig.ToString() != "1")
                                        {
                                            String pD_Desig = mEndPlate[pIndex].Seal.DrainHoles.D_Desig.Remove(mEndPlate[pIndex].Seal.DrainHoles.D_Desig.Length - 1);
                                            pNumerator = Convert.ToInt32(modMain.ExtractPreData(pD_Desig, "/"));
                                            pDenominator = Convert.ToInt32(modMain.ExtractPostData(pD_Desig, "/"));
                                            pFinal = Convert.ToDouble(pNumerator / pDenominator);
                                        }
                                        else
                                        {                                           
                                            pFinal = Convert.ToDouble(mEndPlate[pIndex].Seal.DrainHoles.D_Desig);
                                        }
                                    }
                                    if (pFinal > modMain.gcEPS)
                                    {
                                        lblDrainHoles_D_Desig_Front_MM.Visible = true;
                                        lblDrainHoles_D_Desig_Front_MM.Text =  modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(pFinal)) ;
                                    }
                                    else
                                    {
                                        lblDrainHoles_D_Desig_Front_MM.Visible = false;
                                    }
                                }
                            }

                            Cursor = Cursors.Default;

                            //CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndex);           //PB 29JAN13. Not needed.

                            //....Back Seal: Display the same D_Desig.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //    mCmbDrainHoles_D_Desig[1].Text = mEndPlate[pIndex].Seal.DrainHoles.D_Desig;

                            break;


                        case "cmbDrainHoles_D_Desig_Back":
                            //============================

                            //if (modMain.gProject.Product.EndPlate[0].Type != clsEndPlate.eType.Seal)
                            //{
                                pIndex = 1;
                                mEndPlate[pIndex].Seal.DrainHoles.D_Desig = pCmbBox.Text;

                                mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndPlate[pIndex].Seal.DrainHoles.Count);
                                mTxtDrainHoles_Count[pIndex].ForeColor = Color.Blue;

                                txtDrainHoles_V_Back.Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.V(), "#0.000");


                                Populate_DrainHolesAng_Bet(mEndPlate[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mCmbDrainHoles_AngBet[pIndex]);

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig != "")
                                    {
                                        Double pNumerator, pDenominator;
                                        Double pFinal = 0.0;
                                        if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig.Contains("/"))
                                        {
                                            if (mEndPlate[pIndex].Seal.DrainHoles.D_Desig.ToString() != "1")
                                            {
                                                String pD_Desig = mEndPlate[pIndex].Seal.DrainHoles.D_Desig.Remove(mEndPlate[pIndex].Seal.DrainHoles.D_Desig.Length - 1);
                                                pNumerator = Convert.ToInt32(modMain.ExtractPreData(pD_Desig, "/"));
                                                pDenominator = Convert.ToInt32(modMain.ExtractPostData(pD_Desig, "/"));
                                                pFinal = Convert.ToDouble(pNumerator / pDenominator);
                                            }
                                            else
                                            {
                                                pFinal = Convert.ToDouble(mEndPlate[pIndex].Seal.DrainHoles.D_Desig);
                                            }
                                        }
                                        if (pFinal > modMain.gcEPS)
                                        {
                                            lblDrainHoles_D_Desig_Back_MM.Visible = true;
                                            lblDrainHoles_D_Desig_Back_MM.Text =  modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(pFinal)) ;
                                        }
                                        else
                                        {
                                            lblDrainHoles_D_Desig_Back_MM.Visible = false;
                                        }
                                    }
                                }
                           // }

                            break;


                        case "cmbDrainHoles_AngBet_Front":
                            //============================                     
                            pIndex = 0;

                            pVal = modMain.ConvTextToDouble(pCmbBox.Text);
                            mEndPlate[pIndex].Seal.DrainHoles.AngBet = pVal;

                            mTxtDrainHoles_AngStart[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngStart_Horz, "#0.0");
                            mTxtDrainHoles_AngStart[pIndex].ForeColor = Color.Blue;

                            CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndex);

                            //....Back Seal: Display the same.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //    mCmbDrainHoles_AngBet[1].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngBet, "#0");

                            break;


                        case "cmbDrainHoles_AngBet_Back":
                            //===========================

                            //if (modMain.gProject.Product.EndPlate[0].Type != clsEndPlate.eType.Seal)
                            //{
                                pIndex = 1;

                                pVal = modMain.ConvTextToDouble(pCmbBox.Text);                             
                                mEndPlate[pIndex].Seal.DrainHoles.AngBet = pVal;

                                //if (modMain.gProject.Product.EndPlate[0].Type != clsEndPlate.eType.Seal)
                                //{
                                    mTxtDrainHoles_AngStart[pIndex].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngStart_Horz, "#0.0");
                                    mTxtDrainHoles_AngStart[pIndex].ForeColor = Color.Blue;
                                //}

                                CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndex);
                            //}

                            break;


                        case "cmbDrainHoles_AngExit_Front":
                            //=============================
                            pIndex = 0;
                            mEndPlate[pIndex].Seal.DrainHoles.AngExit = modMain.ConvTextToDouble(pCmbBox.Text);

                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //    mCmbDrainHoles_AngExit[1].Text = modMain.ConvDoubleToStr(mEndPlate[pIndex].Seal.DrainHoles.AngExit, "#0");

                            break;


                        case "cmbDrainHoles_AngExit_Back":
                            //============================

                            //if (modMain.gProject.Product.EndPlate[0].Type != clsEndPlate.eType.Seal)
                            //{
                                pIndex = 1;
                                mEndPlate[pIndex].Seal.DrainHoles.AngExit = modMain.ConvTextToDouble(pCmbBox.Text);
                            //}

                            break;


                        //case "cmbWireClipHoles_Count_Front":
                        //    //=============================
                        //    mEndPlate[0].WireClipHoles.Count = modMain.ConvTextToInt(pCmbBox.Text);
                        //    DisplayOtherAngle(mEndPlate[0], lblWireClipHole_AngOther_Front, mTxtBoxWireClipHole_Front);
                        //    break;


                        //case "cmbWireClipHoles_Count_Back":
                        //    //============================
                        //    mEndPlate[1].WireClipHoles.Count = modMain.ConvTextToInt(pCmbBox.Text);
                        //    DisplayOtherAngle(mEndPlate[1], lblWireClipHole_AngOther_Back, mTxtBoxWireClipHole_Back);
                        //    break;

                        //case "cmbWireClipHoles_UnitSystem_Front":
                        //    //===================================
                        //     mEndPlate[0].Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);       //BG 02JUL13                          
                        //     LoadWireClipHoles_D(mEndPlate[0], cmbWireClipHoles_Thread_Dia_Desig_Front);
                        //     break;

                        //case "cmbWireClipHoles_UnitSystem_Back":
                        //    //==================================
                        //    mEndPlate[1].Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);       //BG 02JUL13                          
                        //    LoadWireClipHoles_D(mEndPlate[1], cmbWireClipHoles_Thread_Dia_Desig_Back);
                        //    break;
                            
                        //case "cmbWireClipHoles_Thread_Dia_Desig_Front":
                        //    //=======================================
                        //    mEndPlate[0].WireClipHoles.Screw_Spec.D_Desig = pCmbBox.Text;
                        //    PopulateWCThreadPitch(((clsSeal)modMain.gProject.Product.EndConfig[0]), cmbWireClipHoles_Thread_Dia_Desig_Front.Text, cmbWireClipHoles_Thread_Pitch_Front);
                        //    break;

                        //case "cmbWireClipHoles_Thread_Dia_Desig_Back":
                        //    //======================================
                        //    mEndPlate[1].WireClipHoles.Screw_Spec.D_Desig = pCmbBox.Text;
                        //    PopulateWCThreadPitch(((clsSeal)modMain.gProject.Product.EndConfig[1]), cmbWireClipHoles_Thread_Dia_Desig_Back.Text, cmbWireClipHoles_Thread_Pitch_Back);
                        //    break;


                        //case "cmbWireClipHoles_Thread_Pitch_Front":
                        //    //====================================
                        //    mEndPlate[0].WireClipHoles.Screw_Spec.Pitch = modMain.ConvTextToDouble(pCmbBox.Text);
                        //    break;


                        //case "cmbWireClipHoles_Thread_Pitch_Back":
                        //    //===================================
                        //    mEndPlate[1].WireClipHoles.Screw_Spec.Pitch = modMain.ConvTextToDouble(pCmbBox.Text);
                        //    break;
                    }
                }


                private void ComboBox_MouseDown(object sender, MouseEventArgs e)
                //==============================================================
                {
                    ComboBox pcmbBox = (ComboBox)sender;
                    pcmbBox.ForeColor = Color.Black;

                    switch (pcmbBox.Name)
                    {
                        case "cmbDrainHoles_Annulus_Ratio_L_H_Front":
                        case "cmbDrainHoles_Annulus_Ratio_L_H_Back":
                            //---------------------------------------
                            mblnDrainHoles_Annulus_Ratio_L_H_ManuallyChanged = true;
                            break;

                        case "cmbDrainHoles_D_Front":
                            //-----------------------
                            mblnDrainHoles_D_Front_ManuallyChanged = true;
                            break;

                        case "cmbDrainHoles_AngBet_Front":
                            //----------------------------
                            mblnDrainHoles_AngBet_Front_ManuallyChanged = true;
                            break;

                        case "cmbDrainHoles_AngExit_Front":
                            //-----------------------------
                            mblnDrainHoles_AngExit_Front_ManuallyChanged = true;
                            break;
                    }
                }


                // PB 29JAN13. It is difficult to implement this rule. may be reviewed later.
                //private void cmbDrainHoles_AngBet_DrawItem(object sender, DrawItemEventArgs e)
                ////============================================================================ 
                //{
                //    if (e.Index < 0) return;
                //    ComboBox pCmbBox = (ComboBox)sender;

                //    int pIndex = 0;

                //    switch (pCmbBox.Name)
                //    {
                //        case "cmbDrainHoles_AngBet_Front":
                //            //============================
                //            pIndex = 0;
                //            Double pAng_ULim_Front = Math.Floor(mEndPlate[pIndex].Seal.DrainHoles.AngBet_ULim_Sym());

                //            e.DrawBackground();
                //            Brush pBrush_Front = Brushes.Black;
                //            Double pDrainHoles_AngBet_Front = Convert.ToDouble(pCmbBox.Items[e.Index]);

                //            if (pDrainHoles_AngBet_Front >= pAng_ULim_Front)
                //            {
                //                pBrush_Front = Brushes.Orange;
                //            }

                //            e.Graphics.DrawString(pCmbBox.Items[e.Index].ToString(),
                //                                  e.Font, pBrush_Front, e.Bounds, StringFormat.GenericDefault);

                //            e.DrawFocusRectangle();
                //            break;

                //        case "cmbDrainHoles_AngBet_Back":
                //            //===========================
                //            pIndex = 1;
                //            Double pAng_ULim_Back = Math.Floor(mEndPlate[pIndex].Seal.DrainHoles.AngBet_ULim_Sym());

                //            e.DrawBackground();
                //            Brush pBrush_Back = Brushes.Black;
                //            Double pDrainHoles_AngBet_Back = Convert.ToDouble(pCmbBox.Items[e.Index]);

                //            if (pDrainHoles_AngBet_Back >= pAng_ULim_Back)
                //            {
                //                pBrush_Back = Brushes.Orange;
                //            }

                //            e.Graphics.DrawString(pCmbBox.Items[e.Index].ToString(),
                //                                  e.Font, pBrush_Back, e.Bounds, StringFormat.GenericDefault);

                //            e.DrawFocusRectangle();
                //            break;
                //    }
                //}


                #region "Helper Routines:"
                //------------------------

                    private void PopulateWCThreadPitch(clsEndPlate EndPlate_In, string Text_In, ComboBox CmbBox_In)
                    //======================================================================================
                    {
                        //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                        //StringCollection pWC_ThreadPitch = new StringCollection();
                        //var pQry = (from pRec in pBearingDBEntities.tblManf_Screw where pRec.fldD_Desig == Text_In orderby pRec.fldPitch ascending select pRec.fldPitch).Distinct().ToList();

                        //if (pQry.Count() > 0)
                        //{
                        //    for (int i = 0; i < pQry.Count; i++)
                        //    {
                        //        pWC_ThreadPitch.Add(pQry[i].ToString());
                        //    }
                        //}
                        //if (pWC_ThreadPitch.Count > 0)
                        //{
                        //    CmbBox_In.Items.Clear();
                        //    for (int i = 0; i < pWC_ThreadPitch.Count; i++)
                        //    {
                        //        Double pVal = Convert.ToDouble(pWC_ThreadPitch[i]);
                        //        CmbBox_In.Items.Add(modMain.ConvDoubleToStr(pVal, "#0.000"));
                        //    }
                        //}

                        //if (CmbBox_In.Items.Count > 0)
                        //{
                        //    if (Seal_In.WireClipHoles.Screw_Spec.Pitch > modMain.gcEPS)
                        //    {
                        //        if (CmbBox_In.Items.Contains(Seal_In.WireClipHoles.Screw_Spec.Pitch.ToString("#0.000")))
                        //            CmbBox_In.SelectedIndex = CmbBox_In.Items.IndexOf(Seal_In.WireClipHoles.Screw_Spec.Pitch.ToString("#0.000"));
                        //        else
                        //            CmbBox_In.SelectedIndex = 0;
                        //    }
                        //    else
                        //        CmbBox_In.SelectedIndex = 0;
                        //}
                    }

                #endregion


            #endregion


            #region "COMMAND BUTTON RELATED ROUTINE:"
            //---------------------------------------

                private void cmdPrint_Click(object sender, EventArgs e)
                //======================================================        
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                    modMain.CaptureScreen(this);
                    pd.Print();
                }

                private void cmdOK_Click(object sender, EventArgs e)
                //===================================================
                {
                    CloseForm();
                }

                private void CloseForm()
                //======================    
                {
                    ////.....Validate C'Bore Depth.
                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    //{
                    //    //if (mEndPlate[0].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.C)
                    //    //{
                    //    //    if (!ValidateCBoreDepth(txtMountHoles_CBore_Depth_Front, mEndPlate[0], tbEndSealDesignDetails_Front))
                    //    //        return;
                    //    //}

                    //    if (((clsSeal)modMain.gProject.Product.EndPlate[0]).Blade.Count != 1)        
                    //    {
                    //        ////.....Validate Angle Between.
                    //        //if (!ValidateAngBet(mCmbDrainHoles_AngBet[0], mEndPlate[0], tbEndSealDesignDetails_Front))
                    //        //    return;

                    //        ////.....Validate Angle Start.
                    //        //if (!ValidateAngStart())
                    //        //    return;
                    //    }

                    //    //BG 28MAR13. As per HK's instruction in email dated 27MAR13.
                    //    //if (!ValidateExitHoleDia(txtTempSensor_D_ExitHole_Front, tbEndSealDesignDetails_Front)) 
                    //    //    return;
                    //}


                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    //{
                    //    ////if (mEndPlate[1].MountHoles.Type == clsEndPlate.clsMountHoles.eMountHolesType.C)
                    //    ////{

                    //    ////    if (!ValidateCBoreDepth(txtMountHoles_CBore_Depth_Back, mEndPlate[1], tbEndSealDesignDetails_Back))
                    //    ////        return;
                    //    ////}

                    //    if (((clsSeal)modMain.gProject.Product.EndPlate[1]).Blade.Count != 1)        
                    //    {
                    //        ////.....Validate Angle Between.
                    //        //if (!ValidateAngBet(mCmbDrainHoles_AngBet[1], mEndPlate[1], tbEndSealDesignDetails_Back))
                    //        //    return;

                    //        ////.....Validate Angle Start.
                    //        //if (!ValidateAngStart())
                    //        //    return;
                    //    }

                    //    //BG 28MAR13  As per HK's instruction in email dated 27MAR13.
                    //    //if (!ValidateExitHoleDia(txtTempSensor_D_ExitHole_Back, tbEndSealDesignDetails_Back)) 
                    //    //    return;
                    //}

                    SaveData();
                   
                   // this.Close();
                    this.Hide();

                    modMain.gfrmCreateDataSet.ShowDialog();

                    ////if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                    ////{
                    ////    modMain.gfrmThrustBearingDesignDetails.ShowDialog();
                    ////}
                }

                
                private void SaveData()
                //======================    
                {
                    int pIndex = 0;
                    
                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    //{
                        pIndex = 0;

                        ////  FRONT
                        ////  -----
                        ////
                        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //{
                        //    modMain.gProject.Product.EndConfig[0].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Front.Text));
                        //}
                        //else
                        //{
                        //    modMain.gProject.Product.EndConfig[0].L = modMain.ConvTextToDouble(txtL_Front.Text);
                        //}

                        ////  Mounting Hole
                        ////  --------------                      
                        //if (optMountHoles_Type_CBore_Front.Checked)
                        //{
                        //    modMain.gProject.Product.EndPlate[0].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.C;
                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        modMain.gProject.Product.EndPlate[0].MountHoles.Depth_CBore = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text));
                        //    }
                        //    else
                        //    {
                        //        modMain.gProject.Product.EndPlate[0].MountHoles.Depth_CBore = modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text);
                        //    }
                        //}

                        //else if (optMountHoles_Type_Thru_Front.Checked)
                        //    modMain.gProject.Product.EndPlate[0].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.H;

                        //else if (optMountHoles_Type_Thread_Front.Checked)
                        //{
                        //    modMain.gProject.Product.EndPlate[0].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.T;
                            
                        //    ((clsSeal)modMain.gProject.Product.EndPlate[0]).MountHoles.Thread_Thru = chkMountHoles_Thread_Thru_Front.Checked;

                           
                        //        if (chkMountHoles_Thread_Thru_Front.Checked)
                        //            ((clsSeal)modMain.gProject.Product.EndPlate[0]).MountHoles.Depth_Thread =
                        //                                                                               ((clsSeal)modMain.gProject.Product.EndPlate[0]).L;
                        //        else
                        //            ((clsSeal)modMain.gProject.Product.EndPlate[0]).MountHoles.Depth_Thread =
                        //                                                                                modMain.ConvTextToDouble(txtMountHoles_Thread_Depth_Front.Text);
                            
                        //}                       
                        if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.Blade.Count == 2)
                        {

                            //  Drain Hole
                            //  ------------ 
                            //....Annulus
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_Ratio_L_H = modMain.ConvTextToDouble(mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].Text);

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtDrainHoles_Annulus_D[pIndex].Text));
                            }
                            else
                            {
                                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_D = modMain.ConvTextToDouble(mTxtDrainHoles_Annulus_D[pIndex].Text);
                            }
                                                        
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.D_Desig = mCmbDrainHoles_D_Desig[pIndex].Text;
                            

                            //....Angle
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngBet = modMain.ConvTextToDouble(mCmbDrainHoles_AngBet[pIndex].Text);
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngStart_Horz = modMain.ConvTextToDouble(mTxtDrainHoles_AngStart[pIndex].Text);

                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[pIndex].Text);
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngExit = modMain.ConvTextToDouble(mCmbDrainHoles_AngExit[pIndex].Text);
                        }


                        ////  Temp. Sensor
                        ////  -------------
                        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //{
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).TempSensor_D_ExitHole =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtTempSensor_D_ExitHole_Front.Text));
                        //}
                        //else
                        //{
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).TempSensor_D_ExitHole = modMain.ConvTextToDouble(txtTempSensor_D_ExitHole_Front.Text);
                        //}


                        ////  Wire Clip Holes
                        ////  ----------------
                        //((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Exists = chkWireClipHoles_Front.Checked;

                        //if (((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Exists)
                        //{
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Count = modMain.ConvTextToInt(cmbWireClipHoles_Count_Front.Text);
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Screw_Spec.D_Desig = cmbWireClipHoles_Thread_Dia_Desig_Front.Text;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Screw_Spec.Pitch = modMain.ConvTextToDouble(cmbWireClipHoles_Thread_Pitch_Front.Text);

                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.ThreadDepth =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtWireClipHoles_Thread_Depth_Front.Text));
                        //    }
                        //    else
                        //    {
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.ThreadDepth = modMain.ConvTextToDouble(txtWireClipHoles_Thread_Depth_Front.Text);
                        //    }
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.AngStart = modMain.ConvTextToDouble(txtWireClipHoles_AngStart_Front.Text);

                        //    Double[] pWireClipHole_AngOther = new Double[5];
                        //    pWireClipHole_AngOther[0] = modMain.ConvTextToDouble(txtWireClipHoles_AngOther1_Front.Text);
                        //    pWireClipHole_AngOther[1] = modMain.ConvTextToDouble(txtWireClipHoles_AngOther2_Front.Text);
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.AngOther = pWireClipHole_AngOther;

                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.DBC = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtWireClipHoles_DBC_Front.Text));
                        //    }
                        //    else
                        //    {
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.DBC = modMain.ConvTextToDouble(txtWireClipHoles_DBC_Front.Text);
                        //    }
                            
                        //    if (cmbWireClipHoles_UnitSystem_Front.Text != "")
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Unit.System =
                        //                                        (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbWireClipHoles_UnitSystem_Front.Text);          //BG 03JUL13

                        //}
                        //else
                        //{
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Count = 0;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Screw_Spec.D_Desig = "";
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Screw_Spec.Pitch = 0.0F;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.ThreadDepth = 0.0F;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.AngStart = 0.0F;

                        //    Double[] pWireClipHole_AngOther = new Double[5];
                        //    pWireClipHole_AngOther[0] = 0.0F;
                        //    pWireClipHole_AngOther[1] = 0.0F;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.AngOther = pWireClipHole_AngOther;

                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.DBC = 0.0F;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Unit.System = modMain.gProject.Product.Unit.System;
                        //}
                    //}

                    
                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    //{
                        pIndex = 1;
                        //////  BACK
                        //////  -----
                        //////
                        ////if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////{
                        ////    modMain.gProject.Product.EndConfig[1].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Back.Text));
                        ////}
                        ////else
                        ////{
                        ////    modMain.gProject.Product.EndConfig[1].L = modMain.ConvTextToDouble(txtL_Back.Text);
                        ////}
                        
                        //////  Mounting Hole
                        //////  --------------
                        ////if (optMountHoles_Type_CBore_Back.Checked)
                        ////{
                        ////    modMain.gProject.Product.EndPlate[1].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.C;
                        ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        ////    {
                        ////        modMain.gProject.Product.EndPlate[1].MountHoles.Depth_CBore =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text));
                        ////    }
                        ////    else
                        ////    {
                        ////        modMain.gProject.Product.EndPlate[1].MountHoles.Depth_CBore = modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text);
                        ////    }
                        ////}

                        ////else if (optMountHoles_Type_Thru_Back.Checked)
                        ////    modMain.gProject.Product.EndPlate[1].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.H;

                        ////else if (optMountHoles_Type_Thread_Back.Checked)
                        ////{
                        ////    modMain.gProject.Product.EndPlate[1].MountHoles.Type = clsEndPlate.clsMountHoles.eMountHolesType.T;
                            
                        ////    ((clsSeal)modMain.gProject.Product.EndPlate[1]).MountHoles.Thread_Thru = chkMountHoles_Thread_Thru_Back.Checked;

                        ////    if (chkMountHoles_Thread_Thru_Back.Checked)
                        ////        //((clsSeal)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_Thread =
                        ////        //                                                                   ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).L;
                        ////        ((clsSeal)modMain.gProject.Product.EndPlate[1]).MountHoles.Depth_Thread =
                        ////                                                                           ((clsSeal)modMain.gProject.Product.EndPlate[1]).L;
                        ////    else
                        ////        ((clsSeal)modMain.gProject.Product.EndPlate[1]).MountHoles.Depth_Thread =
                        ////                                                                            modMain.ConvTextToDouble(txtMountHoles_Thread_Depth_Back.Text);
                        ////}


                        //  Drain Hole
                        //  ------------

                        if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.Blade.Count == 2)
                        {
                            //....Annulus
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_Ratio_L_H = modMain.ConvTextToDouble(mCmbDrainHoles_Annulus_Ratio_L_H[0].Text);

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtDrainHoles_Annulus_D[0].Text));
                            }
                            else
                            {
                                ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_D = modMain.ConvTextToDouble(mTxtDrainHoles_Annulus_D[0].Text);
                            }

                            ////((clsSeal)modMain.gProject.Product.EndPlate[pIndex]).DrainHoles.D_Desig = mCmbDrainHoles_D_Desig[pIndex].Text;
                            ////((clsSeal)modMain.gProject.Product.EndPlate[pIndex]).DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[pIndex].Text);

                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.D_Desig = mCmbDrainHoles_D_Desig[0].Text;

                            //....Angle
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngBet = modMain.ConvTextToDouble(mCmbDrainHoles_AngBet[0].Text);
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngStart_Horz = modMain.ConvTextToDouble(mTxtDrainHoles_AngStart[0].Text);

                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[0].Text);   
                            ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngExit = modMain.ConvTextToDouble(mCmbDrainHoles_AngExit[0].Text);
                        }
                        //else
                        //{
                        //    //....Annulus
                        //    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_Ratio_L_H = modMain.ConvTextToDouble(mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].Text);

                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtDrainHoles_Annulus_D[pIndex].Text));
                        //    }
                        //    else
                        //    {
                        //        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Annulus_D = modMain.ConvTextToDouble(mTxtDrainHoles_Annulus_D[pIndex].Text);
                        //    }

                        //    ////((clsSeal)modMain.gProject.Product.EndPlate[pIndex]).DrainHoles.D_Desig = mCmbDrainHoles_D_Desig[pIndex].Text;
                        //    ////((clsSeal)modMain.gProject.Product.EndPlate[pIndex]).DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[pIndex].Text);

                        //    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[pIndex].Text);
                        //    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.D_Desig = mCmbDrainHoles_D_Desig[pIndex].Text;
                            
                        //    //....Angle
                        //    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngBet = modMain.ConvTextToDouble(mCmbDrainHoles_AngBet[pIndex].Text);
                        //    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngStart_Horz = modMain.ConvTextToDouble(mTxtDrainHoles_AngStart[pIndex].Text);
                        //    ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[pIndex].Seal.DrainHoles.AngExit = modMain.ConvTextToDouble(mCmbDrainHoles_AngExit[pIndex].Text);
                        //}

                        ////  Temp. Sensor
                        ////  -------------
                        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //{
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).TempSensor_D_ExitHole =  modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtTempSensor_D_ExitHole_Back.Text));
                        //}
                        //else
                        //{
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).TempSensor_D_ExitHole = modMain.ConvTextToDouble(txtTempSensor_D_ExitHole_Back.Text);
                        //}
                        

                        ////  Wire Clip Holes
                        ////  ----------------
                        //((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Exists = chkWireClipHoles_Back.Checked;

                        //if (((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Exists)
                        //{
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Count = modMain.ConvTextToInt(cmbWireClipHoles_Count_Back.Text);
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Screw_Spec.D_Desig = cmbWireClipHoles_Thread_Dia_Desig_Back.Text;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Screw_Spec.Pitch = modMain.ConvTextToDouble(cmbWireClipHoles_Thread_Pitch_Back.Text);
                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.ThreadDepth =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtWireClipHoles_Thread_Depth_Back.Text));
                        //    }
                        //    else
                        //    {
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.ThreadDepth = modMain.ConvTextToDouble(txtWireClipHoles_Thread_Depth_Back.Text);
                        //    }
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.AngStart = modMain.ConvTextToDouble(txtWireClipHoles_AngStart_Back.Text);

                        //    Double[] pWireClipHole_AngOther = new Double[5];
                        //    pWireClipHole_AngOther[0] = modMain.ConvTextToDouble(txtWireClipHoles_AngOther1_Back.Text);
                        //    pWireClipHole_AngOther[1] = modMain.ConvTextToDouble(txtWireClipHoles_AngOther2_Back.Text);
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.AngOther = pWireClipHole_AngOther;

                        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //    {
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.DBC =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtWireClipHoles_DBC_Back.Text));
                        //    }
                        //    else
                        //    {
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.DBC = modMain.ConvTextToDouble(txtWireClipHoles_DBC_Back.Text);
                        //    }

                        //    if (cmbWireClipHoles_UnitSystem_Back.Text != "")
                        //        ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Unit.System =
                        //                                        (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbWireClipHoles_UnitSystem_Back.Text);          
                        //}
                        //else
                        //{
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Count = 0;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Screw_Spec.D_Desig = "";
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Screw_Spec.Pitch = 0.0F;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.ThreadDepth = 0.0F;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.AngStart = 0.0F;

                        //    Double[] pWireClipHole_AngOther = new Double[5];
                        //    pWireClipHole_AngOther[0] = 0.0F;
                        //    pWireClipHole_AngOther[1] = 0.0F;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.AngOther = pWireClipHole_AngOther;

                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.DBC = 0.0F;
                        //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Unit.System = modMain.gProject.Product.Unit.System;
                        //}
                    //}
                  
                }

              
                private void cmdCancel_Click(object sender, EventArgs e)
                //======================================================    
                {
                    this.Hide();
                }

            #endregion

        #endregion
        

        #region "UTILITY ROUTINE:"
        //************************

            private void SetBackColor_Drain_V(ref TextBox TxtBox_In)
            //======================================================
            {                
                Double pV_ULimit = 5.0;

                Double pDrain_V = modMain.ConvTextToDouble(TxtBox_In.Text);

                if (pDrain_V > pV_ULimit)
                {
                    TxtBox_In.BackColor = Color.Red;
                }
                else
                {
                    TxtBox_In.BackColor = txtDBore_Nom_Front.BackColor;
                }
            }

            private void Populate_DrainHolesAng_Bet(clsEndPlate EndPlate_In, Label lblAng_Bet_LLim_In, ComboBox CmbBox_In)
            //============================================================================================================ 
            {
                Double pAng_LLim = Math.Ceiling((Double)EndPlate_In.Seal.DrainHoles.AngBet_LLim());
                lblAng_Bet_LLim_In.Text = modMain.ConvDoubleToStr(pAng_LLim, "#0");


                Double pAng_ULim = 0.0;

                if (EndPlate_In.Seal.DrainHoles.Sym_CasingSL_Vert())
                {
                    pAng_ULim = Math.Floor((Double)EndPlate_In.Seal.DrainHoles.AngBet_ULim_Sym());  
                }
                else
                {
                    pAng_ULim = Math.Floor((Double)EndPlate_In.Seal.DrainHoles.AngBet_ULim_NonSym());
                }
               
             
                //....Make Ang_Bet list items even nos.
                //
                Double pAng_LLim_Even = 0, pAng_ULim_Even = 0;

                if (pAng_LLim % 2 != 0.0F)
                    pAng_LLim_Even = pAng_LLim + 1.0F;
                else
                    pAng_LLim_Even = pAng_LLim;


                if (pAng_ULim % 2 != 0.0F)
                    pAng_ULim_Even = pAng_ULim - 1.0F;
                else
                    pAng_ULim_Even = pAng_ULim;


                //....Calculate the AngStart_OtherSide corresponding to AngBet = AngBet_LLim to see if 
                //........it is < 0 i.e. AngEnd crossing the 180 deg. Bearing S/L.
                //
                clsEndPlate pEndPlate = (clsEndPlate)EndPlate_In.Clone(); 
                //clsEndPlate.clsSeal pSeal = (clsEndPlate.clsSeal)EndPlate_In.Clone();                       //....Create a local Seal object.
                pEndPlate.Seal.DrainHoles.AngBet = pEndPlate.Seal.DrainHoles.AngBet_LLim();
       

                CmbBox_In.Items.Clear();

                if (pAng_LLim_Even < pAng_ULim_Even)
                {
                    //....Usual case:
                    //
                    //lblAng_Bet_ULim_In.Text = modMain.ConvDoubleToStr(pAng_ULim, "#0");
                    //lblAng_Bet_ULim_In.ForeColor = Color.Blue;

                    //for (int i = Convert.ToInt32(pAng_LLim_Even); i <= pAng_ULim_Even; i = i + 2)
                    //    CmbBox_In.Items.Add(i);
                }

                //else if (pSeal.DrainHoles.AngStart_OtherSide() < 0 || pAng_LLim_Even >= pAng_ULim_Even)
                else if (pEndPlate.Seal.DrainHoles.AngStart_Horz < 0 || pAng_LLim_Even >= pAng_ULim_Even)        //AES 25OCT18
                {
                    //....Unusual case:
                    //....The following fix has been done to accommodate request by HK, KMC, 17OCT12 
                    //........and telephone discussion with PB on 17OCT12.
                    //
                    //lblAng_Bet_ULim_In.Text = "-";
                    //lblAng_Bet_ULim_In.ForeColor = Color.Orange;
   
                    //int pAng_ULim_Selected = Convert.ToInt32(pAng_LLim_Even) + 10 * 2;      //.....Upper limit is arbitrarily chosen as 
                    //                                                                        //........20 deg more than the lower limit. 
                    //for (int i = Convert.ToInt32(pAng_LLim_Even); i <= pAng_ULim_Selected; i = i + 2)
                    //    CmbBox_In.Items.Add(i);
                }


                //....Populate the combo box.
                int pAng_ULim_Selected = Convert.ToInt32(pAng_LLim_Even) + 10 * 2;      //.....Upper limit is arbitrarily chosen as 
                                                                                        //........20 deg more than the lower limit. 
                for (int i = Convert.ToInt32(pAng_LLim_Even); i <= pAng_ULim_Selected; i = i + 2)
                    CmbBox_In.Items.Add(i);


                if (CmbBox_In.Items.Count > 0)
                {
                    CmbBox_In.SelectedIndex = 0;
                }
            }


            //PB 14JAN13.
            //
            //private Boolean IsAnyDrainHolesOnBearingSL (int Indx_In)          //PB 11JAN12. This routine may be moved to clsSeal.
            ////======================================================         
            //{
            //    Boolean pbln = false;
            //    Double pAngi = 0.0F;
            //    Double pAng_LLim = 0.0F;

            //    for (int i = 1; i <= mEndPlate[Indx_In].DrainHoles.Count; i++)
            //    {
            //        if (Indx_In == 0)
            //        {
            //            pAngi = mEndPlate[Indx_In].DrainHoles.AngStart + ((i - 1) * mEndPlate[Indx_In].DrainHoles.AngBet);
            //        }

            //        else
            //        {
            //            pAngi = mEndPlate[Indx_In].DrainHoles.AngStart_OtherSide + ((i - 1) * mEndPlate[Indx_In].DrainHoles.AngBet);
            //        }

            //        pAng_LLim = Math.Ceiling(mEndPlate[Indx_In].DrainHoles.AngBet_LLim());
  
            //        if (pAngi > (180 - (0.5 * pAng_LLim)) && pAngi < (180 + (0.5 * pAng_LLim)))
            //        {
            //            pbln = true;
            //            break;
            //        }
            //    }

            //    return pbln;
            //}


            private void CheckAndAct_DrainHoles_Crossing_180BearingSL(int Index_In)
            //======================================================================
            {
                //if (mEndPlate[Index_In].Seal.DrainHoles.AngStart_OtherSide () > 0)
                if (mEndPlate[Index_In].Seal.DrainHoles.AngStart_Horz > 0)            //AES 25OCT18
                {
                    //....Usual case: Drain holes array does not cross the 180 deg Bearing S/L.
                    //
                    mTxtDrainHoles_Count[Index_In].Text = Convert.ToString(mEndPlate[Index_In].Seal.DrainHoles.Calc_Count());
                    mTxtDrainHoles_Count[Index_In].ForeColor = Color.Blue;
                    mLblDrainHoles_Notes[Index_In].Visible = false;
                }

                else
                {
                    //....Unusual case: Drain holes array crosses the 180 deg Bearing S/L.
                    //
                    mEndPlate[Index_In].Seal.DrainHoles.Count = mEndPlate[Index_In].Seal.DrainHoles.Calc_Count() + 1;
                    mTxtDrainHoles_Count[Index_In].Text = Convert.ToString(mEndPlate[Index_In].Seal.DrainHoles.Count);
                    mTxtDrainHoles_Count[Index_In].ForeColor = Color.Orange;

                    mLblDrainHoles_Notes[Index_In].Visible = true;
                    mLblDrainHoles_Notes[Index_In].ForeColor = Color.Orange;
                }
            }


            #region "VALIDATION FOR NULL OBJECT:"
            //----------------------------------

                public bool IsMountThread_NULL()
                //==============================
                {
                    bool pBln = false;

                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    //{
                    if (((clsJBearing)modMain.gProject.PNR.Bearing).Mount[0].Screw.Spec.Type == ""
                        || ((clsJBearing)modMain.gProject.PNR.Bearing).Mount[0].Screw.Spec.D_Desig == ""
                        || ((clsJBearing)modMain.gProject.PNR.Bearing).Mount[0].Screw.Spec.Pitch == 0.0F
                        || ((clsJBearing)modMain.gProject.PNR.Bearing).Mount[0].Screw.Spec.L == 0.0F
                        || ((clsJBearing)modMain.gProject.PNR.Bearing).Mount[0].Screw.Spec.Mat == "")
                        {
                            pBln = true;
                        }
                    //}

                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    //{
                    if (((clsJBearing)modMain.gProject.PNR.Bearing).Mount[1].Screw.Spec.Type == ""
                      || ((clsJBearing)modMain.gProject.PNR.Bearing).Mount[1].Screw.Spec.D_Desig == ""
                      || ((clsJBearing)modMain.gProject.PNR.Bearing).Mount[1].Screw.Spec.Pitch == 0.0F
                      || ((clsJBearing)modMain.gProject.PNR.Bearing).Mount[1].Screw.Spec.L == 0.0F
                      || ((clsJBearing)modMain.gProject.PNR.Bearing).Mount[1].Screw.Spec.Mat == "")
                        {
                            pBln = true;
                        }
                    //}

                    return pBln;
                }
                       

                public bool IsFlow_GPM_NULL()
                //============================
                {
                    bool pBln = false;

                    //if (Seal_In.DrainHole.Flow_gpm == 0.0F)
                    if (((clsJBearing)modMain.gProject.PNR.Bearing).PerformData.FlowReqd == 0.0F)
                        pBln = true;

                    return pBln;
                  
                }


                public bool IsSealDO_Null()
                //===========================
                {
                    bool pBln = false;

                    if (((clsJBearing)modMain.gProject.PNR.Bearing).BoltingType() == clsJBearing.eBoltingType.Front)
                    {
                        if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].OD < modMain.gcEPS)
                        {
                            pBln = true;
                        }
                    }
                    else if (((clsJBearing)modMain.gProject.PNR.Bearing).BoltingType() == clsJBearing.eBoltingType.Back)
                    {
                        if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].OD < modMain.gcEPS)
                        {
                            pBln = true;
                        }
                    }
                    else if (((clsJBearing)modMain.gProject.PNR.Bearing).BoltingType() == clsJBearing.eBoltingType.Both)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[i].OD < modMain.gcEPS)
                            {
                                pBln = true;
                            }
                        }
                    }

                    return pBln;
                }


                public bool IsDShaft_NULL()
                //=========================
                {
                    bool pBln = false;

                    //if (modMain.gRadialBearing.DShaft() == 0.0F)
                    if (((clsJBearing)modMain.gProject.PNR.Bearing).RadB.DShaft() == 0.0F)
                        pBln = true;

                    return pBln;
                }

            #endregion

        #endregion


        #region "VALIDATION ROUTINE:"
        //***************************

            private bool ValidateCBoreDepth(TextBox TxtBox_In, clsEndPlate EndPlate_In, TabControl TbCtrl_In)
            //========================================================================================
            {
                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //{
                //    Double pCBoreDepth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(TxtBox_In.Text));

                //    if (pCBoreDepth <= Seal_In.MountHoles.CBore_Depth_UpperLimit()
                //        && pCBoreDepth >= Seal_In.MountHoles.CBore_Depth_LowerLimit())
                //        return true;

                //    String pMSg = "Enter value between Lower Limit: " +
                //                   modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(Seal_In.MountHoles.CBore_Depth_LowerLimit()), "#0.000") +
                //                   " to Upper Limit: " +
                //                   modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(Seal_In.MountHoles.CBore_Depth_UpperLimit()), "#0.000") + ".";

                //    String pCaption = "CBore Depth Data Input Error";
                //    MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //    TbCtrl_In.SelectedIndex = 0;
                //    TxtBox_In.Focus();
                //}
                //else
                //{
                //    Double pCBoreDepth = modMain.ConvTextToDouble(TxtBox_In.Text);

                //    if (pCBoreDepth <= Seal_In.MountHoles.CBore_Depth_UpperLimit()
                //        && pCBoreDepth >= Seal_In.MountHoles.CBore_Depth_LowerLimit())
                //        return true;

                //    String pMSg = "Enter value between Lower Limit: " +
                //                   modMain.ConvDoubleToStr(Seal_In.MountHoles.CBore_Depth_LowerLimit(), "#0.000") +
                //                   " to Upper Limit: " +
                //                   modMain.ConvDoubleToStr(Seal_In.MountHoles.CBore_Depth_UpperLimit(), "#0.000") + ".";

                //    String pCaption = "CBore Depth Data Input Error";
                //    MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //    TbCtrl_In.SelectedIndex = 0;
                //    TxtBox_In.Focus();
                //}

                return false;
            }


            private bool ValidateAngBet(ComboBox CmbBox_In, clsEndPlate EndPlate_In, TabControl tbCtrl_In)
            //=====================================================================================
            {
                Double pAngBet = modMain.ConvTextToDouble(CmbBox_In.Text);
                Double pAng_LLim = (Double)Math.Ceiling((Double)EndPlate_In.Seal.DrainHoles.AngBet_LLim());
                Double pAng_ULim = (Double)Math.Floor((Double)EndPlate_In.Seal.DrainHoles.AngBet_ULim_Sym());

                if (pAngBet >= pAng_LLim && pAngBet <= pAng_ULim)
                    return true;

                String pMSg = "Enter value between Lower Limit: " + pAng_LLim + " to Upper Limit: " + pAng_ULim;
                String pCaption = "Angle Between Data Input Error";             
                MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                tbCtrl_In.SelectedIndex = 1;
                if (CmbBox_In.Items.Count > 0)
                    CmbBox_In.SelectedIndex = 0;
                CmbBox_In.Focus();

                return false;
            }

           
            public bool ValidateAngStart()
            //=============================
            {
                //clsEndPlate.clsSeal pTempSeal;// = new clsSeal();                

                int pRet = 0;

                String pMSg = "The given input value of Angle Start may not" +
                               System.Environment.NewLine +
                               "gurantee symmetrical positioning of the drain holes" +
                               System.Environment.NewLine +
                               "about the casing vertical." +
                               "Do you want to proceed?";

                String pCaption = "Angle Start Data Input Warning";

                //....Answer = YES
                int pAnsY = 6;


                //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                //{
                //pTempSeal = (clsEndPlate.clsSeal)mEndPlate[0].Clone();
                clsEndPlate pEndPlate = (clsEndPlate)mEndPlate[0].Clone(); 

                if ((modMain.CompareVar(mEndPlate[0].Seal.DrainHoles.AngBet, ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.DrainHoles.AngBet, 0, pRet) > 0)
                    || (modMain.CompareVar(mEndPlate[0].Seal.DrainHoles.AngStart_Horz, ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Seal.DrainHoles.AngStart_Horz, 0, pRet) > 0))   
                    {
                        pEndPlate.Seal.DrainHoles.AngBet = mEndPlate[0].Seal.DrainHoles.AngBet;
                        pEndPlate.Seal.DrainHoles.Calc_AngStart_Horz();
                        //mEndPlate.Calc_DrainHole_Angle_Start_FrontSeal();

                        pRet = 0;
                        if (modMain.CompareVar(pEndPlate.Seal.DrainHoles.AngStart_Horz, mEndPlate[0].Seal.DrainHoles.AngStart_Horz, 0, pRet) > 0)
                        {
                            int pAns = (int)MessageBox.Show(pMSg, pCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                            bool pbln = false;

                            if (pAns == pAnsY)
                                pbln = true;
                            else
                            {
                                pbln = false;
                                tbEndSealDesignDetails_Front.SelectedIndex = 1;
                                mTxtDrainHoles_AngStart[0].Text = modMain.ConvDoubleToStr(pEndPlate.Seal.DrainHoles.AngStart_Horz, "#0.0");
                                mTxtDrainHoles_AngStart[0].ForeColor = Color.Blue;
                                mTxtDrainHoles_AngStart[0].Focus();
                            }

                            pEndPlate = null;
                            return pbln;
                        }
                    }
                //}

                pRet = 0;
                //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                //{
                pEndPlate = (clsEndPlate)mEndPlate[1].Clone(); 

                if ((modMain.CompareVar(mEndPlate[1].Seal.DrainHoles.AngBet, ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.DrainHoles.AngBet, 0, pRet) > 0)
                || (modMain.CompareVar(mEndPlate[1].Seal.DrainHoles.AngStart_Horz, ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Seal.DrainHoles.AngStart_Horz, 0, pRet) > 0))   
                    {
                        pEndPlate.Seal.DrainHoles.AngBet = mEndPlate[1].Seal.DrainHoles.AngBet;
                        pEndPlate.Seal.DrainHoles.Calc_AngStart_Horz();
                        //mEndPlate.Calc_DrainHole_Angle_Start_FrontSeal();

                        pRet = 0;
                        if (modMain.CompareVar(pEndPlate.Seal.DrainHoles.AngStart_Horz, mEndPlate[1].Seal.DrainHoles.AngStart_Horz, 0, pRet) > 0)
                        {                            
                            int pAns = (int)MessageBox.Show(pMSg, pCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                            bool pbln = false;

                            if (pAns == pAnsY)
                                pbln = true;
                            else
                            {
                                pbln = false;
                                tbEndSealDesignDetails_Back.SelectedIndex = 1;
                                mTxtDrainHoles_AngStart[1].Text = modMain.ConvDoubleToStr(pEndPlate.Seal.DrainHoles.AngStart_Horz, "#0");
                                mTxtDrainHoles_AngStart[1].ForeColor = Color.Blue;
                                mTxtDrainHoles_AngStart[1].Focus();
                            }

                            pEndPlate = null;
                            return pbln;
                        }
                    }
                //}
             
                return true;
            }

            //private void tbEndSealDesignDetails_KeyDown(object sender, KeyEventArgs e)
            //{

            //}

            //private void tbEndSealDesignDetails_Enter(object sender, EventArgs e)
            //{

            //}

            //private void tbEndSealDesignDetails_MouseDown(object sender, MouseEventArgs e)
            //{

            //}

            private void tbEndSealDesignDetails_MouseEnter(object sender, EventArgs e)
            {
                mblnTab_ManuallyChanged = true;
            }

            private void txtDrainHoles_Count_Front_TextChanged(object sender, EventArgs e)
            //============================================================================
            {
                if (mblnDrainHoleCount_Front_ManuallyChanged)
                {
                    mEndPlate[0].Seal.DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[0].Text);
                    txtDrainHoles_V_Front.Text = modMain.ConvDoubleToStr(mEndPlate[0].Seal.DrainHoles.V(), "#0.000");
                    mblnDrainHoleCount_Front_ManuallyChanged = false;
                }
            }

            private void txtDrainHoles_Count_Front_KeyDown(object sender, KeyEventArgs e)
            //===========================================================================
            {
                mblnDrainHoleCount_Front_ManuallyChanged = true;
            }

            private void txtDrainHoles_Count_Back_TextChanged(object sender, EventArgs e)
            //===========================================================================
            {
                if (mblnDrainHoleCount_Back_ManuallyChanged)
                {
                    mEndPlate[1].Seal.DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[1].Text);
                    txtDrainHoles_V_Back.Text = modMain.ConvDoubleToStr(mEndPlate[1].Seal.DrainHoles.V(), "#0.000");
                    mblnDrainHoleCount_Back_ManuallyChanged = false;
                }
            }

            private void txtDrainHoles_Count_Back_KeyDown(object sender, KeyEventArgs e)
            //==========================================================================
            {
                mblnDrainHoleCount_Back_ManuallyChanged = true;
            }

            private void cmdClose_Click(object sender, EventArgs e)
            //=====================================================
            {
                SaveData();
                this.Hide();
            }

            private void txtDrainHoles_V_Front_TextChanged(object sender, EventArgs e)
            //========================================================================
            {
                SetBackColor_Drain_V(ref txtDrainHoles_V_Front);
            }

            private void txtDrainHoles_V_Back_TextChanged(object sender, EventArgs e)
            //=======================================================================
            {
                SetBackColor_Drain_V(ref txtDrainHoles_V_Back);
            }

                
            //private bool ValidateExitHoleDia(TextBox TxtBox_In, TabControl tbCtrl_In)
            ////=======================================================================              
            //{
            //    Double pExitHoleDia = modMain.ConvTextToDouble(TxtBox_In.Text);

            //    if (pExitHoleDia <= ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.D)
            //        return true;

            //    String pMSg = "The value should be less than the" + "\"\"" + " Temp Sensor Hole Dia : " + "\"\"" +
            //                   modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.D, 
            //                   modMain.gUnit.MFormat) + ".";         


            //    String pCaption = "Exit Hole Dia Data Input Error";
            //    MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

            //    tbCtrl_In.SelectedIndex = 2;
            //    TxtBox_In.Focus();

            //    return false;
            //}


        #endregion

            

    }
}
