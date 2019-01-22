//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmThrustBearing                       '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  21NOV18                                '
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
using System.Drawing.Printing;
using System.Data.SqlClient;

namespace BearingCAD22
{
    public partial class frmThrustBearing : Form
    {

        #region "MEMBER VARIABLES"
        //***********************

       // private clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];

        private ComboBox[] mCmbDirectionType;
        private TextBox[] mTxtRotation;

        private TextBox[] mTxtPad_ID;
        private TextBox[] mTxtPad_OD;

        private TextBox[] mTxtDBore_Min;
        private TextBox[] mTxtDBore_Max;

        private TextBox[] mTxtLandL;

        //  Material 
        //  ========
        private ComboBox[] mCmbMatBase_WCode;    
        private ComboBox[] mCmbMatLining_WCode;
        private TextBox[] mTxtMat_Base;
        private TextBox[] mTxtMat_Lining;

        private TextBox[] mTxtLiningT_Face;
        private TextBox[] mTxtLiningT_ID;

        private TextBox[] mTxtPad_Count;
        private NumericUpDown[] mUpdPad_Count;

        //  Feed Groove
        //  ===========

        private TextBox[] mTxtFeedGroove_Wid;
        private TextBox[] mTxtFeedGroove_Depth;


        //  Weep Slot
        //  =========
        private ComboBox[] mCmbWeepSlot_Type;
        private TextBox[] mTxtWeepSlot_Wid;
        private TextBox[] mTxtWeepSlot_Depth;


        //  Taper
        //  =====
        private TextBox[] mTxtTaper_Depth_OD;
        private TextBox[] mTxtTaper_Depth_ID;
        private TextBox[] mTxtTaper_Angle;


        //  Shroud
        //  ======
        //private GroupBox[] mGrpToggle_Shroud;
        private TextBox[] mTxtShroud_Ro;
        private TextBox[] mTxtShroud_Ri;
        private Label[] mLblShroud_Ro;
        private Label[] mLblShroud_Ri;
        //private Button[] mcmdShroud_RadiusToDia;
        //private RadioButton[] mOptShroud_Radius;
        //private RadioButton[] mOptShroud_Diameter;

        private Boolean mUpdPadCount_Front_Changed = false;
        private Boolean mUpdPadCount_Back_Changed = false;

        private Boolean mblnDBore_Min_Front_Changed = false;
        private Boolean mblnDBore_Max_Front_Changed = false;

        private Boolean mblnDBore_Min_Back_Changed = false;
        private Boolean mblnDBore_Max_Back_Changed = false;

        private double[] mShroud_ID;

        #endregion


        #region "FORM CONSTRUCTOR & RELATED ROUTINES:"
        //********************************************

        public frmThrustBearing()
        //=======================
        {
            InitializeComponent();

            mCmbDirectionType = new ComboBox[] { cmbDirectionType_Front, cmbDirectionType_Back };
            mTxtRotation = new TextBox[] { txtRotation_Front, txtRotation_Back };

            //....Set TextBoxes
            mTxtPad_ID = new TextBox[] { txtPad_ID_Front, txtPad_ID_Back };
            mTxtPad_OD = new TextBox[] { txtPad_OD_Front, txtPad_OD_Back };

            mTxtDBore_Min = new TextBox[] { txtDBore_Range_Min_Front, txtDBore_Range_Min_Back };
            mTxtDBore_Max = new TextBox[] { txtDBore_Range_Max_Front, txtDBore_Range_Max_Back };

            mTxtLandL = new TextBox[] { txtLandL_Front, txtLandL_Back };


            //  Material 
            //  ========
            //....Base
            mCmbMatBase_WCode = new ComboBox[] { cmbBaseMat_WCode_Front, cmbBaseMat_WCode_Back };
            mTxtMat_Base = new TextBox[] { txtMat_Base_Name_Front, txtMat_Base_Name_Back };

            //....Lining 
            mCmbMatLining_WCode = new ComboBox[] { cmbLining_WCode_Front, cmbLining_WCode_Back };
            mTxtMat_Lining = new TextBox[] { txtMat_Lining_Name_Front, txtMat_Lining_Name_Back };

            //....LiningT
            mTxtLiningT_Face = new TextBox[] { txtLiningT_Face_Front, txtLiningT_Face_Back };
            mTxtLiningT_ID = new TextBox[] { txtLiningT_ID_Front, txtLiningT_ID_Back };

            //
            mTxtPad_Count = new TextBox[] { txtPad_Count_Front, txtPad_Count_Back };
            mUpdPad_Count = new NumericUpDown[] { updPad_Count_Front, updPad_Count_Back };


            //  Feed Groove
            //  ===========
            mTxtFeedGroove_Wid = new TextBox[] { txtFeedGroove_Wid_Front, txtFeedGroove_Wid_Back };
            mTxtFeedGroove_Depth = new TextBox[] { txtFeedGroove_Depth_Front, txtFeedGroove_Depth_Back };


            //  Weep Slot
            //  =========
            mCmbWeepSlot_Type = new ComboBox[] { cmbWeepSlot_Type_Front, cmbWeepSlot_Type_Back };
            mTxtWeepSlot_Wid = new TextBox[] { txtWeepSlot_Wid_Front, txtWeepSlot_Wid_Back };
            mTxtWeepSlot_Depth = new TextBox[] { txtWeepSlot_Depth_Front, txtWeepSlot_Depth_Back };


            //  Taper
            //  =====
            mTxtTaper_Depth_OD = new TextBox[] { txtTaper_Depth_OD_Front, txtTaper_Depth_OD_Back };
            mTxtTaper_Depth_ID = new TextBox[] { txtTaper_Depth_ID_Front, txtTaper_Depth_ID_Back };
            mTxtTaper_Angle = new TextBox[] { txtTaper_Angle_Front, txtTaper_Angle_Back };


            //  Shroud
            //  ======
            //mGrpToggle_Shroud = new GroupBox     [] { grpToggle_Shroud_Front, grpToggle_Shroud_Back };
            mTxtShroud_Ro = new TextBox[] { txtShroud_Ro_Front, txtShroud_Ro_Back };
            mTxtShroud_Ri = new TextBox[] { txtShroud_Ri_Front, txtShroud_Ri_Back };
            mLblShroud_Ro = new Label[] { lblShroud_Ro_Front, lblShroud_Ro_Back };
            mLblShroud_Ri = new Label[] { lblShroud_Ri_Front, lblShroud_Ri_Back };
            ////mOptShroud_Radius = new RadioButton[] { optShroud_Radius_Front, optShroud_Radius_Back };
            ////mOptShroud_Diameter = new RadioButton[] { optShroud_Diameter_Front, optShroud_Diameter_Back };
            mShroud_ID = new double[2];


            //.....Populate Woukesha Code for Base and Lining Material.           
            for (int i = 0; i < 2; i++)
            {
                ////BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                ////var pQryBase = (from pRec in pBearingDBEntities.tblData_Mat where pRec.fldLining == false && pRec.fldCode_Waukesha != null && pRec.fldEndPlate == true orderby pRec.fldCode_Waukesha ascending select pRec).ToList();
                
                ////if (pQryBase.Count() > 0)
                ////{
                ////    mCmbMatBase_WCode[i].Items.Clear();
                ////    for (int j = 0; j < pQryBase.Count; j++)
                ////    {
                ////        mCmbMatBase_WCode[i].Items.Add(pQryBase[j].fldCode_Waukesha);
                ////    }
                ////    mCmbMatBase_WCode[i].Items.Add("Other");
                ////    mCmbMatBase_WCode[i].SelectedIndex = 0;
                ////}

                ////var pQryLining = (from pRec in pBearingDBEntities.tblData_Mat where pRec.fldLining == true && pRec.fldCode_Waukesha != null orderby pRec.fldCode_Waukesha ascending select pRec).ToList();

                ////if (pQryLining.Count() > 0)
                ////{
                ////    mCmbMatLining_WCode[i].Items.Clear();
                ////    for (int j = 0; j < pQryLining.Count; j++)
                ////    {
                ////        mCmbMatLining_WCode[i].Items.Add(pQryLining[j].fldCode_Waukesha);
                ////    }
                ////    mCmbMatLining_WCode[i].Items.Add("Other");
                ////    mCmbMatLining_WCode[i].SelectedIndex = 0;
                ////}
              
                //....Base Material.
                //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
                string pWHERE = " WHERE Lining = false and Code_Waukesha is not null and EndPlate = true";
                int pMat_Base_WCode_RecCount = modMain.gDB.PopulateCmbBox(mCmbMatBase_WCode[i], modMain.gFiles.FileTitle_EXCEL_MatData, "[Mat$]", "Code_Waukesha", pWHERE, true);

                if (pMat_Base_WCode_RecCount > 0)
                {
                    mCmbMatBase_WCode[i].Items.Add("Other");
                    mCmbMatBase_WCode[i].SelectedIndex = 0;
                }

                //....Lining Material.               
                pWHERE = " WHERE Lining = true and Code_Waukesha is not null";
                int pMat_Lining_WCode_RecCount = modMain.gDB.PopulateCmbBox(mCmbMatLining_WCode[i], modMain.gFiles.FileTitle_EXCEL_MatData, "[Mat$]", "Code_Waukesha", pWHERE, true);

                if (pMat_Lining_WCode_RecCount > 0)
                {
                    mCmbMatLining_WCode[i].Items.Add("Other");
                    mCmbMatLining_WCode[i].SelectedIndex = 0;
                }               

                PopulateWeepSlotType(mCmbWeepSlot_Type[i]);
                PopulateDirectionType(mCmbDirectionType[i]);
            }

            mUpdPadCount_Front_Changed = false;
            mUpdPadCount_Back_Changed = false;
        }        


        private void PopulateWeepSlotType(ComboBox cmbBox_In)
        //===================================================
        {
            //cmbBox_In.Items.Clear();
            //cmbBox_In.DataSource = Enum.GetValues(typeof(clsBearing_Thrust_TL.clsWeepSlot.eType));
            //cmbBox_In.SelectedIndex = 0;
        }


        private void PopulateDirectionType(ComboBox cmbBox_In)
        //===================================================
        {
            //cmbBox_In.DataSource = Enum.GetValues(typeof(clsBearing_Thrust_TL.eDirectionType));

            //if (cmbBox_In.Items.Count > 0)
            //    cmbBox_In.SelectedIndex = 0;
        }


        #endregion


        #region "FORM EVENT ROUTINES: "
        //*****************************

        private void frmThrustBearing_Load(object sender, EventArgs e)
        //==============================================================
        {
            //....Set Local Object.
            SetLocalObject();

            //....Set Tab Pages.
            SetTabPages();

            //....Set Control.
            SetControl();

            //....Diaplay Data.
            DisplayData();
        }


        private void SetLocalObject()
        //===========================
        {
            //mEndTB = new clsBearing_Thrust_TL[2];

            //for (int i = 0; i < 2; i++)
            //{
            //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
            //    {
            //        mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(modMain.gProject.Product.EndPlate[i])).Clone();
            //    }
            //}
        }


        private void SetTabPages()
        //===========================
        {
            //TabPage[] pTabPages = new TabPage[] { tabFront, tabBack };

            //tbThrustBearingData.TabPages.Clear();

            //for (int i = 0; i < 2; i++)
            //{
            //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
            //        tbThrustBearingData.TabPages.Add(pTabPages[i]);
            //}
        }

        private void SetControl()
        //=======================                           
        {
            SetControls_Status(true);          
           
        }


        private void SetControls_Status(Boolean Enable_In)
        //=================================================
        {

            //for (int i = 0; i < 2; i++)
            //{
            //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
            //    {
            //        ////mCmbDirectionType[i].Enabled = Enable_In;

            //        ////mTxtPad_ID[i].ReadOnly = !Enable_In;
            //        ////mTxtPad_OD[i].ReadOnly = !Enable_In;

            //        ////mTxtDBore_Min[i].ReadOnly = !Enable_In;
            //        ////mTxtDBore_Max[i].ReadOnly = !Enable_In;

            //        ////mTxtLandL[i].ReadOnly = !Enable_In;

            //        ////mCmbMatBase_WCode[i].Enabled = Enable_In;
            //        ////mCmbMatLining_WCode[i].Enabled = Enable_In;

            //        ////mTxtLiningT_Face[i].ReadOnly = !Enable_In;
            //        ////mTxtLiningT_ID[i].ReadOnly = !Enable_In;

            //        ////mTxtPad_Count[i].ReadOnly = !Enable_In;
            //        ////mUpdPad_Count[i].Enabled = Enable_In;

            //        ////mTxtFeedGroove_Wid[i].ReadOnly = !Enable_In;
            //        ////mTxtFeedGroove_Depth[i].ReadOnly = !Enable_In;

            //        ////mCmbWeepSlot_Type[i].Enabled = Enable_In;
            //        ////mTxtWeepSlot_Wid[i].ReadOnly = !Enable_In;
            //        ////mTxtWeepSlot_Depth[i].ReadOnly = !Enable_In;

            //        ////mTxtTaper_Depth_OD[i].ReadOnly = !Enable_In;
            //        ////mTxtTaper_Depth_ID[i].ReadOnly = !Enable_In;
            //        ////mTxtTaper_Angle[i].ReadOnly = !Enable_In;

            //        ////mTxtShroud_Ro[i].ReadOnly = !Enable_In;
            //        ////mTxtShroud_Ri[i].ReadOnly = !Enable_In;

            //        ////mOptShroud_Radius[i].Checked = false;
            //        ////mOptShroud_Diameter[i].Checked = false;

            //        //mOptShroud_Diameter[i].Enabled = Enable_In;
            //        //mOptShroud_Diameter[i].Enabled = Enable_In;

            //        if (mEndTB[i].Shroud.ID < modMain.gcEPS)
            //        {
            //            mLblShroud_Ro[i].Text = "Outer R";
            //            mLblShroud_Ri[i].Text = "Inner R";

            //            mTxtShroud_Ro[i].ReadOnly = !Enable_In;
            //            mTxtShroud_Ri[i].ReadOnly = !Enable_In;

            //            mTxtShroud_Ro[i].ForeColor = Color.Black;

            //            if (mEndTB[i].Shroud.Ri == 0.5 * mEndTB[i].PadD[0])
            //                mTxtShroud_Ri[i].ForeColor = Color.Magenta;
            //            else
            //                mTxtShroud_Ri[i].ForeColor = Color.Black;

            //            //mOptShroud_Radius[i].Checked = true;
            //            //mOptShroud_Diameter[i].Checked = false;
            //            ////mOptShroud_Radius[i].Checked = false;
            //            ////mOptShroud_Diameter[i].Checked = true;
            //        }
            //        else
            //        {
            //            mLblShroud_Ro[i].Text = "OD";
            //            mLblShroud_Ri[i].Text = "ID";

            //            mTxtShroud_Ro[i].ReadOnly = Enable_In;
            //            mTxtShroud_Ri[i].ReadOnly = Enable_In;

            //            mTxtShroud_Ro[i].ForeColor = Color.Blue;
            //            mTxtShroud_Ri[i].ForeColor = Color.Blue;

            //            //mOptShroud_Diameter[i].Checked = true;
            //            //mOptShroud_Radius[i].Checked = false;
            //        }
            //    }
            //}

        }

        private void DisplayData()
        //========================
        {

            //for (int i = 0; i < 2; i++)
            //{
            //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
            //    {
            //        mCmbDirectionType[i].Text = mEndTB[i].DirectionType.ToString();

            //        if (i == 0)
            //            mTxtRotation[i].Text = clsBearing_Thrust_TL.eRotation.CCW.ToString();
            //        else
            //            mTxtRotation[i].Text = clsBearing_Thrust_TL.eRotation.CW.ToString();

            //        mTxtRotation[i].ForeColor = Color.Magenta;

            //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
            //        {
            //            int j = 0;
            //            //....Pad D                          
            //            mTxtPad_ID[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].PadD[j]));
            //            j++;
            //            mTxtPad_OD[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].PadD[j]));

            //            //....Bore D
            //            j = 0;
            //            mTxtDBore_Min[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].DBore_Range[j]));
            //            j++;
            //            mTxtDBore_Max[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].DBore_Range[j]));


            //            mTxtLandL[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].LandL));
            //            mCmbMatBase_WCode[i].Text = mEndTB[i].Mat.WCode.Base;
            //            mCmbMatLining_WCode[i].Text = mEndTB[i].Mat.WCode.Lining;
            //            ////mCmbMat_Base[i].Text = mEndTB[i].Mat.Base;
            //            ////mCmbMat_Lining[i].Text = mEndTB[i].Mat.Lining;

            //            //mTxtLiningT_Face[i].Text = modMain.ConvDoubleToStr(mEndTB[i].LiningT.Face, "#0.0000");
            //            //mTxtLiningT_ID[i].Text = modMain.ConvDoubleToStr(mEndTB[i].LiningT.ID, "#0.0000");

            //            mTxtLiningT_Face[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].LiningT.Face));
            //            mTxtLiningT_ID[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].LiningT.ID));

            //            mTxtPad_Count[i].Text = mEndTB[i].Pad_Count.ToString();
            //            mUpdPad_Count[i].Value = mEndTB[i].Pad_Count;

            //            mTxtFeedGroove_Wid[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].FeedGroove.Wid));
            //            mTxtFeedGroove_Depth[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].FeedGroove.Depth));

            //            mCmbWeepSlot_Type[i].Text = mEndTB[i].WeepSlot.Type.ToString();
            //            mTxtWeepSlot_Wid[i].Text = modMain.ConvDoubleToStr(mEndTB[i].WeepSlot.Wid, "#0.000");
            //            mTxtWeepSlot_Depth[i].Text = modMain.ConvDoubleToStr(mEndTB[i].WeepSlot.Depth, "#0.000");

            //            mTxtTaper_Depth_OD[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].Taper.Depth_OD));
            //            mTxtTaper_Depth_ID[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].Taper.Depth_ID));
            //            mTxtTaper_Angle[i].Text = modMain.ConvDoubleToStr(mEndTB[i].Taper.Angle, "#0.#");

            //            if (mEndTB[i].Shroud.OD < modMain.gcEPS)
            //            {
            //                mTxtShroud_Ro[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].Shroud.Ro));
            //            }
            //            if (mEndTB[i].Shroud.ID < modMain.gcEPS)
            //            {
            //                mTxtShroud_Ri[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[i].Shroud.Ri));
            //                mShroud_ID[i] = mEndTB[i].Shroud.ID;
            //            }
            //        }
            //        else
            //        {
            //            int j = 0;
            //            //....Pad D                          
            //            mTxtPad_ID[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].PadD[j]);
            //            j++;
            //            mTxtPad_OD[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].PadD[j]);

            //            //....Bore D
            //            j = 0;
            //            mTxtDBore_Min[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].DBore_Range[j]);
            //            j++;
            //            mTxtDBore_Max[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].DBore_Range[j]);


            //            mTxtLandL[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].LandL);
            //            mCmbMatBase_WCode[i].Text = mEndTB[i].Mat.WCode.Base;
            //            mCmbMatLining_WCode[i].Text = mEndTB[i].Mat.WCode.Lining;
            //            //mCmbMat_Base[i].Text = mEndTB[i].Mat.Base;
            //            //mCmbMat_Lining[i].Text = mEndTB[i].Mat.Lining;

            //            //mTxtLiningT_Face[i].Text = modMain.ConvDoubleToStr(mEndTB[i].LiningT.Face, "#0.0000");
            //            //mTxtLiningT_ID[i].Text = modMain.ConvDoubleToStr(mEndTB[i].LiningT.ID, "#0.0000");

            //            mTxtLiningT_Face[i].Text =modMain.gProject.PNR.Unit.WriteInUserL( mEndTB[i].LiningT.Face);
            //            mTxtLiningT_ID[i].Text =modMain.gProject.PNR.Unit.WriteInUserL( mEndTB[i].LiningT.ID);

            //            mTxtPad_Count[i].Text = mEndTB[i].Pad_Count.ToString();
            //            mUpdPad_Count[i].Value = mEndTB[i].Pad_Count;

            //            mTxtFeedGroove_Wid[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].FeedGroove.Wid);
            //            mTxtFeedGroove_Depth[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].FeedGroove.Depth);

            //            mCmbWeepSlot_Type[i].Text = mEndTB[i].WeepSlot.Type.ToString();
            //            mTxtWeepSlot_Wid[i].Text = modMain.ConvDoubleToStr(mEndTB[i].WeepSlot.Wid, "#0.000");
            //            mTxtWeepSlot_Depth[i].Text = modMain.ConvDoubleToStr(mEndTB[i].WeepSlot.Depth, "#0.000");

            //            mTxtTaper_Depth_OD[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].Taper.Depth_OD);
            //            mTxtTaper_Depth_ID[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].Taper.Depth_ID);
            //            mTxtTaper_Angle[i].Text = modMain.ConvDoubleToStr(mEndTB[i].Taper.Angle, "#0.#");

            //            if (mEndTB[i].Shroud.OD < modMain.gcEPS)
            //            {
            //                mTxtShroud_Ro[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].Shroud.Ro);
            //            }
            //            if (mEndTB[i].Shroud.ID < modMain.gcEPS)
            //            {
            //                mTxtShroud_Ri[i].Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndTB[i].Shroud.Ri);
            //                mShroud_ID[i] = mEndTB[i].Shroud.ID;
            //            }
            //        }

            //    }

            //}

        }


        #endregion


        #region "NUMERIC UPDOWN RELATED:"
        //-------------------------------

        private void updPad_Count_Enter(object sender, EventArgs e)
        //=========================================================
        {
            mUpdPadCount_Front_Changed = true;
            mUpdPadCount_Back_Changed = true;
        }


        private void updPad_Count_ValueChanged(object sender, EventArgs e)
        //================================================================
        {
            NumericUpDown pUpd = (NumericUpDown)sender;

            switch (pUpd.Name)
            {

                case "updPad_Count_Front":
                    txtPad_Count_Front.Text = updPad_Count_Front.Value.ToString();
                    break;

                case "updPad_Count_Back":
                    txtPad_Count_Back.Text = updPad_Count_Back.Value.ToString();
                    break;
            }
        }


        #endregion

        #region "CHECK BOX RELATED:"
        //-------------------------------

            private void chkLining_Front_CheckedChanged(object sender, EventArgs e)
            //=====================================================================
            {
                cmbLining_WCode_Front.Visible = chkLining_Front.Checked;
                txtMat_Lining_Name_Front.Visible = chkLining_Front.Checked;
                lblMat_TLining_Front.Visible = chkLining_Front.Checked;
                lblThick_Face_Front.Visible = chkLining_Front.Checked;
                txtLiningT_Face_Front.Visible = chkLining_Front.Checked;
                lblThick_ID_Front.Visible = chkLining_Front.Checked;
                txtLiningT_ID_Front.Visible = chkLining_Front.Checked;

            }

            private void chkLining_Back_CheckedChanged(object sender, EventArgs e)
            //=====================================================================
            {
                cmbLining_WCode_Back.Visible = chkLining_Back.Checked;
                txtMat_Lining_Name_Back.Visible = chkLining_Back.Checked;
                lblMat_TLining_Back.Visible = chkLining_Back.Checked;
                lblThick_Face_Back.Visible = chkLining_Back.Checked;
                txtLiningT_Face_Back.Visible = chkLining_Back.Checked;
                lblThick_ID_Back.Visible = chkLining_Back.Checked;
                txtLiningT_ID_Back.Visible = chkLining_Back.Checked;
            }

      
        #endregion
        


        #region "COMBO BOX RELATED ROUTINE"
        //---------------------------------

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //====================================================================
        {

            //ComboBox pCmbBox = (ComboBox)sender;

            //switch (pCmbBox.Name)
            //{
            //    case "cmbDirectionType_Front":
            //        //--------------------------------                    
            //        if(cmbDirectionType_Front.SelectedIndex > 0)
            //        {
            //            MessageBox.Show("BearingCAD 2.1 does not Support '" + cmbDirectionType_Front.Text + "'.", "Direction Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            cmbDirectionType_Front.SelectedIndex = 0;
            //        }
                    
            //        ////if (cmbDirectionType_Front.Text == clsBearing_Thrust_TL.eDirectionType.Bumper.ToString())
            //        ////{
            //        ////    grpTaper_Front.Enabled = false;
            //        ////    grpShroud_Front.Enabled = false;
            //        ////}
            //        ////else
            //        ////{
            //        ////    grpTaper_Front.Enabled = true;
            //        ////    grpShroud_Front.Enabled = true;
            //        ////}

            //        break;

            //    case "cmbDirectionType_Back":
            //        //-----------------------
            //        if (cmbDirectionType_Back.SelectedIndex > 0)
            //        {
            //            MessageBox.Show("BearingCAD 2.1 does not Support '" + cmbDirectionType_Back.Text + "'.", "Direction Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            cmbDirectionType_Back.SelectedIndex = 0;
            //        }
            //        ////if (cmbDirectionType_Back.Text == clsBearing_Thrust_TL.eDirectionType.Bumper.ToString())
            //        ////{
            //        ////    grpTaper_Back.Enabled = false;
            //        ////    grpShroud_Back.Enabled = false;
            //        ////}
            //        ////else
            //        ////{
            //        ////    grpTaper_Back.Enabled = true;
            //        ////    grpShroud_Back.Enabled = true;
            //        ////}

            //        break;


            //    case "cmbBaseMat_WCode_Front":
            //        //-----------------------
            //        if (mEndTB[0] != null)
            //        {
            //            mEndTB[0].Mat.WCode_Base = pCmbBox.Text;
            //            //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
            //            txtMat_Base_Name_Front.Text = mEndTB[0].Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
            //            mEndTB[0].Mat.Base = txtMat_Base_Name_Front.Text;
            //        }
            //        break;

            //    case "cmbBaseMat_WCode_Back":
            //        //---------------------
            //        if (mEndTB[1] != null)
            //        {
            //            mEndTB[1].Mat.WCode_Base = pCmbBox.Text;
            //            //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
            //            txtMat_Base_Name_Back.Text = mEndTB[1].Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
            //            mEndTB[1].Mat.Base = txtMat_Base_Name_Back.Text;
            //        }
            //        break;

            //    case "cmbLining_WCode_Front":
            //        //-----------------------
            //        if (mEndTB[0] != null)
            //        {
            //            mEndTB[0].Mat.WCode_Lining = pCmbBox.Text;
            //            //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
            //            txtMat_Lining_Name_Front.Text = mEndTB[0].Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
            //            mEndTB[0].Mat.Lining = txtMat_Lining_Name_Front.Text;
            //        }
            //        break;

            //    case "cmbLining_WCode_Back":
            //        //---------------------
            //        if (mEndTB[1] != null)
            //        {
            //            mEndTB[1].Mat.WCode_Lining = pCmbBox.Text;
            //            //string pMatFileName = "D:\\BearingCAD\\Program Data Files\\Mat_Data_03OCT18.xlsx";
            //            txtMat_Lining_Name_Back.Text = mEndTB[1].Mat.MatName(pCmbBox.Text, modMain.gFiles.FileTitle_EXCEL_MatData);
            //            mEndTB[1].Mat.Lining = txtMat_Lining_Name_Back.Text;
            //        }
            //        break;
            //}
        }

        #endregion


        #region "TEXTBOX RELATED ROUTINE"
        //-------------------------------

        private void TxtBox_KeyDown(object sender, KeyEventArgs e)
        //========================================================
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

                case "txtShroud_Ri_Front":
                    pTxtBox.ForeColor = Color.Black;
                    break;

                case "txtShroud_Ri_Back":
                    pTxtBox.ForeColor = Color.Black;
                    break;
            }
        }



        private void TextBox_TextChanged(object sender, EventArgs e)
        //==========================================================
        {
            TextBox pTxtBox = (TextBox)sender;

            //switch (pTxtBox.Name)
            //{
                //case "txtDBore_Range_Min_Front":
                //    //--------------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[0].DBore_Range[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //        txtDBore_Range_Max_Front.ForeColor = Color.Black;

                //        if (mblnDBore_Min_Front_Changed)
                //        {
                //            if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[0]).DBore_Range[1] < modMain.gcEPS)
                //            {
                //                if (mEndTB[0].DBore_Range[0] > modMain.gcEPS)
                //                {
                //                    txtDBore_Range_Max_Front.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[0].Calc_DBore_Limit(0)), "#0.0000");
                //                    txtDBore_Range_Max_Front.ForeColor = Color.Blue;
                //                }
                //                else
                //                {
                //                    txtDBore_Range_Max_Front.Text = "";
                //                }
                //                mblnDBore_Min_Front_Changed = false;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        mEndTB[0].DBore_Range[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                //        txtDBore_Range_Max_Front.ForeColor = Color.Black;

                //        if (mblnDBore_Min_Front_Changed)
                //        {
                //            if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[0]).DBore_Range[1] < modMain.gcEPS)
                //            {
                //                if (mEndTB[0].DBore_Range[0] > modMain.gcEPS)
                //                {
                //                    txtDBore_Range_Max_Front.Text = modMain.ConvDoubleToStr(mEndTB[0].Calc_DBore_Limit(0), "#0.0000");
                //                    txtDBore_Range_Max_Front.ForeColor = Color.Blue;
                //                }
                //                else
                //                {
                //                    txtDBore_Range_Max_Front.Text = "";
                //                }
                //                mblnDBore_Min_Front_Changed = false;
                //            }
                //        }
                //    }
                //    break;

                //case "txtDBore_Range_Max_Front":
                //    //--------------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[0].DBore_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //    }
                //    else
                //    {
                //        mEndTB[0].DBore_Range[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                //    }

                //    if (mblnDBore_Max_Front_Changed)
                //    {
                //        txtDBore_Range_Max_Front.ForeColor = Color.Black;

                //    }
                //    break;

                //case "txtDBore_Range_Min_Back":
                //    //--------------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[1].DBore_Range[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //        txtDBore_Range_Max_Back.ForeColor = Color.Black;

                //        if (mblnDBore_Min_Back_Changed)
                //        {
                //            if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[1]).DBore_Range[1] < modMain.gcEPS)
                //            {
                //                if (mEndTB[1].DBore_Range[0] > modMain.gcEPS)
                //                {
                //                    txtDBore_Range_Max_Back.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mEndTB[1].Calc_DBore_Limit(0)), "#0.0000");
                //                    txtDBore_Range_Max_Back.ForeColor = Color.Blue;
                //                }
                //                else
                //                {
                //                    txtDBore_Range_Max_Back.Text = "";
                //                }
                //                mblnDBore_Min_Front_Changed = false;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        mEndTB[1].DBore_Range[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                //        txtDBore_Range_Max_Back.ForeColor = Color.Black;

                //        if (mblnDBore_Min_Back_Changed)
                //        {
                //            if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[1]).DBore_Range[1] < modMain.gcEPS)
                //            {
                //                if (mEndTB[1].DBore_Range[0] > modMain.gcEPS)
                //                {
                //                    txtDBore_Range_Max_Back.Text = modMain.ConvDoubleToStr(mEndTB[1].Calc_DBore_Limit(0), "#0.0000");
                //                    txtDBore_Range_Max_Back.ForeColor = Color.Blue;
                //                }
                //                else
                //                {
                //                    txtDBore_Range_Max_Back.Text = "";
                //                }
                //                mblnDBore_Min_Front_Changed = false;
                //            }
                //        }
                //    }
                //    break;

                //case "txtDBore_Range_Max_Back":
                //    //--------------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[1].DBore_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //    }
                //    else
                //    {
                //        mEndTB[1].DBore_Range[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                //    }

                //    if (mblnDBore_Max_Back_Changed)
                //    {
                //        txtDBore_Range_Max_Back.ForeColor = Color.Black;

                //    }
                //    break;

                //case "txtPad_ID_Front":
                //    //-----------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[0].PadD[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_ID_Front.Text));
                //        txtShroud_Ri_Front.Text =modMain.gProject.PNR.Unit.CEng_Met( mEndTB[0].Shroud.Ri).ToString("#0.000");
                //    }
                //    else
                //    {
                //        mEndTB[0].PadD[0] = modMain.ConvTextToDouble(txtPad_ID_Front.Text);
                //        txtShroud_Ri_Front.Text = mEndTB[0].Shroud.Ri.ToString("#0.000");
                //    }
                //    break;

                //case "txtPad_ID_Back":
                //    //----------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[1].PadD[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_ID_Back.Text));
                //        txtShroud_Ri_Back.Text = modMain.gProject.PNR.Unit.CEng_Met(mEndTB[1].Shroud.Ri).ToString("#0.000");
                //    }
                //    else
                //    {
                //        mEndTB[1].PadD[0] = modMain.ConvTextToDouble(txtPad_ID_Back.Text);
                //        txtShroud_Ri_Back.Text = mEndTB[1].Shroud.Ri.ToString("#0.000");
                //    }
                //    break;

                //case "txtLandL_Front":
                //    //----------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[0].LandL = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //    }
                //    else
                //    {
                //        mEndTB[0].LandL = modMain.ConvTextToDouble(pTxtBox.Text);
                //    }
                //    SetForeColor(pTxtBox, mEndTB[0].LAND_L);
                //    break;

                //case "txtLandL_Back":
                //    //----------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[1].LandL = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //    }
                //    else
                //    {
                //        mEndTB[1].LandL = modMain.ConvTextToDouble(pTxtBox.Text);
                //    }
                //    SetForeColor(pTxtBox, mEndTB[1].LAND_L);
                //    break;

                //case "txtLiningT_Face_Front":
                //    //-----------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[0].LiningT_Face =  modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLiningT_Face_Front.Text));
                //    }
                //    else
                //    {
                //        mEndTB[0].LiningT_Face = modMain.ConvTextToDouble(txtLiningT_Face_Front.Text);
                //    }
                //    SetForeColor(txtLiningT_Face_Front, mEndTB[0].LINING_T);
                //    break;

                //case "txtLiningT_ID_Front":
                //    //---------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[0].LiningT_ID = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLiningT_ID_Front.Text));
                //    }
                //    else
                //    {
                //        mEndTB[0].LiningT_ID = modMain.ConvTextToDouble(txtLiningT_ID_Front.Text);
                //    }
                //    SetForeColor(txtLiningT_ID_Front, mEndTB[0].LINING_T);
                //    break;


                //case "txtLiningT_Face_Back":
                //    //----------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[1].LiningT_Face = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLiningT_Face_Back.Text));
                //    }
                //    else
                //    {
                //        mEndTB[1].LiningT_Face = modMain.ConvTextToDouble(txtLiningT_Face_Back.Text);
                //    }
                //    SetForeColor(txtLiningT_Face_Back, mEndTB[1].LINING_T);
                //    break;

                //case "txtLiningT_ID_Back":
                //    //--------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[1].LiningT_ID =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtLiningT_ID_Back.Text));
                //    }
                //    else
                //    {
                //        mEndTB[1].LiningT_ID = modMain.ConvTextToDouble(txtLiningT_ID_Back.Text);
                //    }
                //    SetForeColor(txtLiningT_ID_Back, mEndTB[1].LINING_T);
                //    break;

                //case "txtPad_Count_Front":
                //    //--------------------
                //    mEndTB[0].Pad_Count = modMain.ConvTextToInt(pTxtBox.Text);
                //    mUpdPadCount_Front_Changed = false;
                //    if (!mUpdPadCount_Front_Changed)
                //        updPad_Count_Front.Value = mEndTB[0].Pad_Count;
                //    break;

                //case "txtPad_Count_Back":
                //    //------------------
                //    mEndTB[1].Pad_Count = modMain.ConvTextToInt(pTxtBox.Text);
                //    mUpdPadCount_Back_Changed = false;
                //    if (!mUpdPadCount_Back_Changed)
                //        updPad_Count_Back.Value = mEndTB[1].Pad_Count;
                //    break;

                //case "txtFeedGroove_Depth_Front":
                //    //--------------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[0].FeedGroove.Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFeedGroove_Depth_Front.Text));
                //    }
                //    else
                //    {
                //        mEndTB[0].FeedGroove.Depth = modMain.ConvTextToDouble(txtFeedGroove_Depth_Front.Text);
                //    }
                //    break;

                //case "txtFeedGroove_Depth_Back":
                //    //-------------------------
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        mEndTB[1].FeedGroove.Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFeedGroove_Depth_Back.Text));
                //    }
                //    else
                //    {
                //        mEndTB[1].FeedGroove.Depth = modMain.ConvTextToDouble(txtFeedGroove_Depth_Back.Text);
                //    }
                //    break;

                //case "txtWeepSlot_Depth_Front":
                //    //------------------------- 
                //    mEndTB[0].WeepSlot.Depth = modMain.ConvTextToDouble(txtWeepSlot_Depth_Front.Text);
                //    txtFeedGroove_Depth_Front.Text = mEndTB[0].FeedGroove.Depth.ToString("#0.000");
                //    break;

                //case "txtWeepSlot_Depth_Back":
                //    //------------------------
                //    mEndTB[1].WeepSlot.Depth = modMain.ConvTextToDouble(txtWeepSlot_Depth_Back.Text);
                //    txtFeedGroove_Depth_Back.Text = mEndTB[1].FeedGroove.Depth.ToString("#0.000");
                //    break;

                //case "txtShroud_Ro_Front":
                //    //-------------------- 
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        if (mEndTB[0].Shroud.OD < modMain.gcEPS)
                //            mEndTB[0].Shroud_Ro = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //    }
                //    else
                //    {
                //        if (mEndTB[0].Shroud.OD < modMain.gcEPS)
                //            mEndTB[0].Shroud_Ro = modMain.ConvTextToDouble(pTxtBox.Text);
                //    }
                //    break;

                //case "txtShroud_Ro_Back":
                //    //-------------------- 
                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                //    {
                //        if (mEndTB[1].Shroud.OD < modMain.gcEPS)
                //            mEndTB[1].Shroud_Ro = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                //    }
                //    else
                //    {
                //        if (mEndTB[1].Shroud.OD < modMain.gcEPS)
                //            mEndTB[1].Shroud_Ro = modMain.ConvTextToDouble(pTxtBox.Text);
                //    }

                //    break;
            //}

        }
    
        private void TextBox_Validating(object sender, CancelEventArgs e)
        //================================================================
        {
            //TextBox pTxtBox = (TextBox)sender;

            //switch (pTxtBox.Name)
            //{
            //    case "txtShroud_Ri_Front":
            //        //--------------------                   
            //        if (mShroud_ID[0] < modMain.gcEPS)// && mOptShroud_Diameter[0].Checked == false)
            //        {
            //            Double pShroud_Ri_Front = modMain.ConvTextToDouble(txtShroud_Ri_Front.Text);
            //            txtShroud_Ri_Front.Text = mEndTB[0].Validate_Shroud_Ri(ref pShroud_Ri_Front).ToString("#0.000");
            //            mEndTB[0].Shroud_Ri = modMain.ConvTextToDouble(txtShroud_Ri_Front.Text);
            //            mShroud_ID[0] = mEndTB[0].Shroud.Ri * 2;

            //            if (pShroud_Ri_Front == 0.5 * mEndTB[0].PadD[0])
            //                txtShroud_Ri_Front.ForeColor = Color.Magenta;
            //            else
            //                txtShroud_Ri_Front.ForeColor = Color.Black;

            //            e.Cancel = true;
            //        }
            //        break;

            //    case "txtShroud_Ri_Back":
            //        //--------------------                   
            //        if (mShroud_ID[1] < modMain.gcEPS)// && mOptShroud_Diameter[1].Checked == false)
            //        {
            //            Double pShroud_Ri_Back = modMain.ConvTextToDouble(txtShroud_Ri_Back.Text);
            //            txtShroud_Ri_Back.Text = mEndTB[1].Validate_Shroud_Ri(ref pShroud_Ri_Back).ToString("#0.000");
            //            mEndTB[1].Shroud_Ri = modMain.ConvTextToDouble(txtShroud_Ri_Back.Text);                       
            //            mShroud_ID[1] = mEndTB[1].Shroud.Ri * 2;

            //            if (pShroud_Ri_Back == 0.5 * mEndTB[1].PadD[0])
            //                txtShroud_Ri_Back.ForeColor = Color.Magenta;
            //            else
            //                txtShroud_Ri_Back.ForeColor = Color.Black;

            //            e.Cancel = true;
            //        }
            //        break;
            //}
        }


        private void SetForeColor(TextBox TxtBox_In, Double DefVal_In)
        //==============================================================       
        {
            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
            {
                if (System.Math.Abs(modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(TxtBox_In.Text)) - DefVal_In) <= modMain.gcEPS)
                {
                    TxtBox_In.ForeColor = Color.Magenta;
                }
                else
                {
                    TxtBox_In.ForeColor = Color.Black;
                }
            }
            else
            {
                if (System.Math.Abs(modMain.ConvTextToDouble(TxtBox_In.Text) - DefVal_In) <= modMain.gcEPS)
                {
                    TxtBox_In.ForeColor = Color.Magenta;
                }
                else
                {
                    TxtBox_In.ForeColor = Color.Black;
                }
            }
        }

        #endregion


        #region "OPTION BUTTON RELATED ROUTINE"
        //-------------------------------------

        private void OptionButton_CheckedChanged(object sender, EventArgs e)
        //===================================================================   
        {
            //RadioButton pOptButton = (RadioButton)sender;

            //switch (pOptButton.Name)
            //{
            //    case "optShroud_Radius_Front":
            //        //========================
            //        lblShroud_Ro_Front.Text = "Outer R";
            //        txtShroud_Ro_Front.Text = modMain.ConvDoubleToStr(mEndTB[0].Shroud.Ro, "#0.000");
                                    
            //        txtShroud_Ro_Front.ReadOnly = false;
            //        txtShroud_Ro_Front.BackColor = Color.White;
            //        txtShroud_Ro_Front.ForeColor = Color.Black;                   
               
            //        mEndTB[0].Shroud_OD = 0.0;

            //        lblShroud_Ri_Front.Text = "Inner R";
            //        txtShroud_Ri_Front.Text = modMain.ConvDoubleToStr(mEndTB[0].Shroud.Ri, "#0.000");
                  
            //        txtShroud_Ri_Front.ReadOnly = false;
            //        txtShroud_Ri_Front.BackColor = Color.White;
                
            //        if (mEndTB[0].Shroud.Ri == 0.5 * mEndTB[0].PadD[0])
            //            txtShroud_Ri_Front.ForeColor = Color.Magenta;
            //        else
            //            txtShroud_Ri_Front.ForeColor = Color.Black;

            //        mEndTB[0].Shroud_ID = 0.0;

            //        break;

            //    case "optShroud_Diameter_Front":
            //        //===========================
            //        lblShroud_Ro_Front.Text = "OD";
            //        mEndTB[0].Shroud_OD = mEndTB[0].Shroud.Ro * 2;
            //        txtShroud_Ro_Front.Text = modMain.ConvDoubleToStr(mEndTB[0].Shroud.OD, "#0.000");
            //        txtShroud_Ro_Front.ReadOnly = true;
            //        txtShroud_Ro_Front.BackColor = Color.WhiteSmoke;
            //        txtShroud_Ro_Front.ForeColor = Color.Blue;

            //        lblShroud_Ri_Front.Text = "ID";
            //        mEndTB[0].Shroud_ID = mEndTB[0].Shroud.Ri * 2;
            //        txtShroud_Ri_Front.Text = modMain.ConvDoubleToStr(mEndTB[0].Shroud.ID, "#0.000");
            //        txtShroud_Ri_Front.ReadOnly = true;
            //        txtShroud_Ri_Front.BackColor = Color.WhiteSmoke;
            //        txtShroud_Ri_Front.ForeColor = Color.Blue;

            //        break;

            //    case "optShroud_Radius_Back":
            //        //=======================
            //        lblShroud_Ro_Back.Text = "Outer R";
               
            //        txtShroud_Ro_Back.ReadOnly = false;
            //        txtShroud_Ro_Back.BackColor = Color.White;
            //        txtShroud_Ro_Back.ForeColor = Color.Black;
                   
            //        mEndTB[1].Shroud_OD = 0.0;

            //        lblShroud_Ri_Back.Text = "Inner R";
            //        txtShroud_Ri_Back.Text = modMain.ConvDoubleToStr(mEndTB[1].Shroud.Ri, "#0.000");
                  
            //        txtShroud_Ri_Back.ReadOnly = false;
            //        txtShroud_Ri_Back.BackColor = Color.White;
                    
            //        if (mEndTB[1].Shroud.Ri == 0.5 * mEndTB[1].PadD[0])
            //            txtShroud_Ri_Back.ForeColor = Color.Magenta;
            //        else
            //            txtShroud_Ri_Back.ForeColor = Color.Black;

            //        mEndTB[1].Shroud_ID = 0.0;

            //        break;

            //    case "optShroud_Diameter_Back":
            //        //==========================
            //        lblShroud_Ro_Back.Text = "OD";
            //        mEndTB[1].Shroud_OD = mEndTB[1].Shroud.Ro * 2;
            //        txtShroud_Ro_Back.Text = modMain.ConvDoubleToStr(mEndTB[1].Shroud.OD, "#0.000");
            //        txtShroud_Ro_Back.ReadOnly = true;
            //        txtShroud_Ro_Back.BackColor = Color.WhiteSmoke;
            //        txtShroud_Ro_Back.ForeColor = Color.Blue;

            //        lblShroud_Ri_Back.Text = "ID";
            //        mEndTB[1].Shroud_ID = mEndTB[1].Shroud.Ri * 2;
            //        txtShroud_Ri_Back.Text = modMain.ConvDoubleToStr(mEndTB[1].Shroud.ID, "#0.000");
            //        txtShroud_Ri_Back.ReadOnly = true;
            //        txtShroud_Ri_Back.BackColor = Color.WhiteSmoke;
            //        txtShroud_Ri_Back.ForeColor = Color.Blue;

            //        break;

            //}
        }

        #endregion


        #region "COMMAND BUTTON EVENT ROUTINE:"
        //-------------------------------------
        


        private void cmdOK_Click(object sender, EventArgs e)
        //===================================================
        {
            //SaveData();
            //this.Hide();
           
            //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
            //{
            //    modMain.gfrmSeal.ShowDialog();
            //}
            //else
            //    //modMain.gfrmPerformDataBearing.ShowDialog();
            //    modMain.gfrmBearingDesignDetails.ShowDialog();
        }


        private void cmdCancel_Click(object sender, EventArgs e)
        //=======================================================
        {
            this.Close();
        }


        private void SaveData()
        //=====================
        {
            ////.....Front Tab:   i=0
            ////.....Back Tab:    i=1. 

            //for (int i = 0; i < 2; i++)
            //{
            //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
            //    {
            //        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).DirectionType = (clsBearing_Thrust_TL.eDirectionType)
            //                                            Enum.Parse(typeof(clsBearing_Thrust_TL.eDirectionType), mCmbDirectionType[i].Text);

            //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
            //        {
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).PadD[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtPad_OD[i].Text));
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).PadD[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtPad_ID[i].Text));

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).DBore_Range[0] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtDBore_Min[i].Text));
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).DBore_Range[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtDBore_Max[i].Text));

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).LandL = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtLandL[i].Text));


            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Mat.WCode_Base = mCmbMatBase_WCode[i].Text;
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Mat.WCode_Lining = mCmbMatLining_WCode[i].Text;

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Mat.Base = mTxtMat_Base[i].Text;
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Mat.Lining = mTxtMat_Lining[i].Text;

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).LiningT_Face = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtLiningT_Face[i].Text));
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).LiningT_ID = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtLiningT_ID[i].Text));

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Pad_Count = modMain.ConvTextToInt(mTxtPad_Count[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).FeedGroove.Wid = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtFeedGroove_Wid[i].Text));
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).FeedGroove.Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtFeedGroove_Depth[i].Text));

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).WeepSlot.Type = (clsBearing_Thrust_TL.clsWeepSlot.eType)
            //                                                                                         Enum.Parse(typeof(clsBearing_Thrust_TL.clsWeepSlot.eType), mCmbWeepSlot_Type[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).WeepSlot.Wid = modMain.ConvTextToDouble(mTxtWeepSlot_Wid[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).WeepSlot.Depth = modMain.ConvTextToDouble(mTxtWeepSlot_Depth[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Taper_Depth_ID = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtTaper_Depth_ID[i].Text));
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Taper_Depth_OD = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtTaper_Depth_OD[i].Text));
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Taper_Angle = modMain.ConvTextToDouble(mTxtTaper_Angle[i].Text);

            //            if (mEndTB[i].Shroud.OD > modMain.gcEPS)
            //            {
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_OD = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtShroud_Ro[i].Text));
            //            }
            //            else
            //            {
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_Ro = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtShroud_Ro[i].Text));
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_OD = 0.0;
            //            }

            //            if (mEndTB[i].Shroud.ID > modMain.gcEPS)
            //            {
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_ID = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtShroud_Ri[i].Text));
            //            }
            //            else
            //            {
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_Ri = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mTxtShroud_Ri[i].Text));
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_ID = 0.0;
            //            }
            //        }
            //        else
            //        {
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).PadD[1] = modMain.ConvTextToDouble(mTxtPad_OD[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).PadD[0] = modMain.ConvTextToDouble(mTxtPad_ID[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).DBore_Range[0] = modMain.ConvTextToDouble(mTxtDBore_Min[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).DBore_Range[1] = modMain.ConvTextToDouble(mTxtDBore_Max[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).LandL = modMain.ConvTextToDouble(mTxtLandL[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Mat.WCode_Base = mCmbMatBase_WCode[i].Text;
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Mat.WCode_Lining = mCmbMatLining_WCode[i].Text;

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Mat.Base = mTxtMat_Base[i].Text;
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Mat.Lining = mTxtMat_Lining[i].Text;                    

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).LiningT_Face = modMain.ConvTextToDouble(mTxtLiningT_Face[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).LiningT_ID = modMain.ConvTextToDouble(mTxtLiningT_ID[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Pad_Count = modMain.ConvTextToInt(mTxtPad_Count[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).FeedGroove.Wid = modMain.ConvTextToDouble(mTxtFeedGroove_Wid[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).FeedGroove.Depth = modMain.ConvTextToDouble(mTxtFeedGroove_Depth[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).WeepSlot.Type = (clsBearing_Thrust_TL.clsWeepSlot.eType)
            //                                                                                         Enum.Parse(typeof(clsBearing_Thrust_TL.clsWeepSlot.eType), mCmbWeepSlot_Type[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).WeepSlot.Wid = modMain.ConvTextToDouble(mTxtWeepSlot_Wid[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).WeepSlot.Depth = modMain.ConvTextToDouble(mTxtWeepSlot_Depth[i].Text);

            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Taper_Depth_ID = modMain.ConvTextToDouble(mTxtTaper_Depth_ID[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Taper_Depth_OD = modMain.ConvTextToDouble(mTxtTaper_Depth_OD[i].Text);
            //            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Taper_Angle = modMain.ConvTextToDouble(mTxtTaper_Angle[i].Text);

            //            if (mEndTB[i].Shroud.OD > modMain.gcEPS)
            //            {
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_OD = modMain.ConvTextToDouble(mTxtShroud_Ro[i].Text);
            //            }
            //            else
            //            {
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_Ro = modMain.ConvTextToDouble(mTxtShroud_Ro[i].Text);
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_OD = 0.0;
            //            }

            //            if (mEndTB[i].Shroud.ID > modMain.gcEPS)
            //            {
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_ID = modMain.ConvTextToDouble(mTxtShroud_Ri[i].Text);
            //            }
            //            else
            //            {
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_Ri = modMain.ConvTextToDouble(mTxtShroud_Ri[i].Text);
            //                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndPlate[i]).Shroud_ID = 0.0;
            //            }
            //        }

            //    }

            //}

        }

        #endregion


    }
}
