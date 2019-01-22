//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsRadB_SL                             '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
// PB 22OCT18

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using EXCEL = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Linq;

namespace BearingCAD22
{
    public partial class clsRadB
    {     
        [Serializable]
        public class clsSL
        {
            #region "USER-DEFINED STRUCTURES:"
            //================================
                [Serializable]
                public struct sLoc
                {
                    public Double Center;
                    public Double Back;
                }
             #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private clsJBearing mCurrentBearing;

                #region "Screw:" 
                    private clsScrew mScrew;
                    private sLoc mLScrew;      
                    private sLoc mRScrew;
                  
                #endregion

                #region "Dowel:"
                    public clsPin mDowel;
                    public sLoc mLDowel;
                    public sLoc mRDowel;       
                #endregion

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================

                #region "Screw:"
                //--------------
                    public clsScrew Screw
                    {
                        get { return mScrew; }
                        set { mScrew = value; }
                    }

                    #region "Locations:"
                    //-----------------
                        #region "LScrew:"
                        //---------------
                            public sLoc LScrew
                            {
                                get{return mLScrew;}
                            }

                            public Double LScrew_Center
                            {   set { mLScrew.Center = value; }}       
                            

                            public Double LScrew_Back
                            {   set { mLScrew.Back = value; }}

                        #endregion
            
                        #region "RScrew:"
                        //---------------

                            public sLoc RScrew
                            {
                                get{return mRScrew;}
                            }
            
                            public Double RScrew_Center
                            {
                                set { mRScrew.Center = value; }
                            }

                            public Double RScrew_Back
                            {
                                set { mRScrew.Back = value; }
                            }

                        #endregion

                    #endregion
            
                #endregion


                #region "Dowel:"
                //-------------- 
                       
                    public clsPin Dowel
                    {
                        get { return mDowel; }
                        set { mDowel = value; }
                    }
                    

                    #region "Locations":
                    //-----------------

                        #region "LDowel:"
                        //---------------
                            public sLoc LDowel_Loc
                            {
                                get
                                {
                                    //if (mLDowel_Loc.Center < modMain.gcEPS)
                                    //    mLDowel_Loc.Center = Calc_LDowel_Loc_Center();

                                    //if (mLDowel_Loc.Back < modMain.gcEPS)
                                    //    mLDowel_Loc.Back = Calc_LDowel_Loc_Front();

                                    return mLDowel;
                                }
                            }


                            public Double LDowel_Loc_Center
                            {
                                set { mLDowel.Center = value; }
                            }

                            public Double LDowel_Loc_Back
                            {
                                set { mLDowel.Back = value; }
                            }

                        #endregion


                        #region "RDowel:"
                        //---------------
                            public sLoc RDowel_Loc
                            {
                                get
                                {
                                    //if (mRDowel_Loc.Center < modMain.gcEPS)
                                    //    mRDowel_Loc.Center = Calc_RDowel_Loc_Center();

                                    //if (mRDowel_Loc.Back < modMain.gcEPS)
                                    //    mRDowel_Loc.Back = Calc_RDowel_Loc_Front();

                                    return mRDowel;
                                }
                            }

                            public Double RDowel_Loc_Center
                            {
                                set { mRDowel.Center = value; }
                            }

                            public Double RDowel_Loc_Back
                            {
                                set { mRDowel.Back = value; }
                            }

                        #endregion

                    #endregion


                    #region "Depths":
                    //--------------
                        public Double Dowel_Depth                   //....Read-only.
                        {
                            get {  return Calc_Dowel_Depth(); }
                        }

                    #endregion

                #endregion

            #endregion


            #region "CONSTRUCTOR:"

                public clsSL(clsJBearing CurrentBearing_In)
                //=========================================
                {
                    mCurrentBearing = CurrentBearing_In;

                    //....Screw.
                    //
                        mScrew = new clsScrew(mCurrentBearing.Unit.System);

                        mScrew.Spec_Type = "SHCS";
                        mScrew.Spec_Mat  = "STEEL";

                    //....Dowel.
                    //
                        mDowel = new clsPin(mCurrentBearing.Unit.System);

                        mDowel.Spec_Type = "P";
                        mDowel.Spec_Mat  = "STEEL";
                }

            #endregion


            #region "CLASS METHODS":

                #region "SCREW:"
                //-------------

                    #region "Locations:"
                    //------------------

                        //public Double Calc_Screw_Loc_Center()
                        ////=====================================
                        //{
                        //    //....Commmon for RScrew & LScrew.
                        //    //
                        //    Double pDFit = mCurrent_Bearing_Radial_FP.OD();
                        //    Double pD_CBore = mScrew.Hole.CBore.D;               
                        //    Double pLoc = 0.5F * (pDFit - pD_CBore) - 0.1;

                        //    return modMain.MRound(pLoc, 0.05);
                        //}

                    // PB 22OCT18. BG, write a new method: Screw_Loc_Center_ULimit (see scanned docu for formula)

                        public Double Screw_Loc_Center_ULimit()
                        //=====================================
                        {
                            Double pResult = 0;
                            Double pBearing_DFit = mCurrentBearing.RadB.OD();
                            Double pScrew_CBore_D = mScrew.Hole.CBore.D;
                            Double pScrew_CBore_Depth = mScrew.Hole.CBore.Depth;
                            Double pScrew_D = mScrew.D();

                            Double pX1 = Math.Pow((pScrew_CBore_Depth + pScrew_CBore_D + 0.030), 2);
                            Double pX2 = Math.Pow((0.5 * pBearing_DFit), 2);
                            Double pX3 = Math.Sqrt(1 - (pX1 / pX2));
                            pResult = 0.5 * (pBearing_DFit * pX3 - pScrew_CBore_D);
                            return pResult;
                        }
                       
                        // public Double Calc_Screw_Loc_Front(string Screw_In)
                        ////==================================================        
                        //{
                        //    Double pVal = 0.0F;
                            //////....Used for both RScrew & LScrew.
                            //////
                            //////....Ref. Radial_Rev11_27OCT11: Col. N & Col. P.
                            //////
                            ////Double pAR_Pin_Loc_Dist_Front = mCurrent_Bearing_Radial_FP.AntiRotPin.Loc_Dist_Front;               //....Col. FY.
                            ////string pAR_Pin_Loc_Bearing_V  = mCurrent_Bearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert.ToString();  //....Col. FV.

                            ////Double pDimStart     = mCurrent_Bearing_Radial_FP.DimStart_FrontFace;                               //....Col. CQ.
                            ////Double pAnnulus_LocB = mCurrent_Bearing_Radial_FP.OilInlet.Annulus.Loc_Back;                        //....Col. CU

                            ////Double pDepthF = mCurrent_Bearing_Radial_FP.Depth_EndConfig[0];                                     //....Col. CZ.
                            ////Double pDepthB = mCurrent_Bearing_Radial_FP.Depth_EndConfig[1];                                     //....Col. DA.

                            ////Double pBearingL = mCurrent_Bearing_Radial_FP.L;

                            ////Double pVal1 = modMain.MRound(pDimStart + 0.5 * (pAnnulus_LocB + pDepthF), 0.01F);
                            ////Double pVal2 = modMain.MRound(pDimStart + pBearingL - 0.5 * (pAnnulus_LocB + pDepthB), 0.01F);

                            ////Double pVal = 0.0F;


                            ////switch (Screw_In)
                            ////{
                            ////    case "RScrew":
                            ////    //------------

                            ////        if (((0.5F * pBearingL) > pAR_Pin_Loc_Dist_Front && pAR_Pin_Loc_Bearing_V == "L") ||
                            ////            ((0.5F * pBearingL) < pAR_Pin_Loc_Dist_Front && pAR_Pin_Loc_Bearing_V == "R"))
                            ////        {
                            ////            pVal = pVal1;
                            ////        }
                            ////        else
                            ////        {
                            ////            pVal = pVal2;
                            ////        }
                                
                            ////        break;


                            ////    case "LScrew":
                            ////    //------------

                            ////        if (((0.5F * pBearingL) > pAR_Pin_Loc_Dist_Front && pAR_Pin_Loc_Bearing_V == "R") ||
                            ////            ((0.5F * pBearingL) < pAR_Pin_Loc_Dist_Front && pAR_Pin_Loc_Bearing_V == "L"))
                            ////        {
                            ////            pVal = pVal1;
                            ////        }
                            ////        else
                            ////        {
                            ////            pVal = pVal2;
                            ////        }
                                
                            ////        break;
                            ////}              

                        //    return pVal;
                        //}
            
                    #endregion


                    #region "DEPTHS:"
                    //---------------

                        #region "Thread:"
                        //--------------

                            private Double Calc_Thread_Depth()
                            //=================================
                            {
                                //....Ref. Radial_Rev11_27OCT11: Col. AL.
                                Double pD = mScrew.D();                      //....Col. X
                                return modMain.MRound(2.0 * pD, 0.0625);
                            }

                        #endregion


                        #region "CBore:"
                        //--------------

                            private Double Calc_CBore_Depth()
                            //================================
                            {
                                //....Ref. Radial_Rev11_27OCT11: Col. AO
                                Double pL = 0.0F;

                                if (mScrew.Spec.Unit.System == clsUnit.eSystem.Metric)                      
                                    pL = mScrew.Spec.L / 25.4;                  //....mm ==> in
                                else
                                    pL = mScrew.Spec.L;                         //....Col. AP

                                return (pL - (1.5 * mScrew.D()));
                            }

                        #endregion

                    #endregion


                    #region "Length - Lower Limit:"
                    //-----------------------------

                            public Double Thread_L_LowerLimit()
                            //=====================================   
                            {
                                Double pL_LowerLimit = 0.0F;
                                ////....The routine output unit:      //PB 09JUN09. SB, change elsewhere.
                                ////........UnitSystem = English: in.
                                ////........           = Metric:  mm.
                                //BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                                

                                //Double pDThread = 0.0F;

                                ////....If     Unit = English, DThread = in.
                                ////....ElseIf Unit = Metric,  DThread = mm.
                                ////
                                //var pProject = (from pRec in pBearingDBEntities.tblManf_Screw_D where pRec.fldD_Desig == mScrew_Spec.D_Desig select pRec).ToList();

                                //if (pProject.Count > 0)
                                //    pDThread = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble(pProject[0].fldD);


                                //if (mScrew_Spec.Unit.System == clsUnit.eSystem.English)
                                //{
                                //    //SB 31JUL09

                                //    //...To be done later. 
                                //    //....Refer to "Radial_Rev7_10JUL09.xls", column AR (English). 

                                //    //....MROUND((AN3+0.25)-Z3,0.125)

                                //    Double pZ3, pAN3;
                                //    pZ3 = pDThread / 25.4;                          //....Convert to in. 
                                //    pAN3 = modMain.MRound(2.5 * pZ3, 0.0625);

                                //    pL_LowerLimit = modMain.MRound(pAN3 + 0.25 - pZ3, 0.125);
                                //}

                                //else if (mScrew_Spec.Unit.System == clsUnit.eSystem.Metric)
                                //{

                                //    //....Refer to "Radial_Rev5_30MAR05", column AS (Metric). 
                                //    Double pZ3, pAN3, pAR3, pAQ3;
                                //    pZ3 = pDThread / 25.4;                          //....Convert to in. 

                                //    pAN3 = modMain.MRound(2.5 * pZ3, 0.0625);
                                //    pAR3 = modMain.MRound(pAN3 + 0.25 - pZ3, 0.125);

                                //    pAQ3 = pAR3 - pAN3 + pZ3;

                                //    pL_LowerLimit = (pAN3 + pAQ3 - pZ3) * 25.4;    //....Convert back to mm.            
                                //    pL_LowerLimit = modMain.MRound(pL_LowerLimit, 1);
                                //}
                                return pL_LowerLimit;
                            }

                        ////public Double Thread_L_LowerLimit()
                        //////==================================
                        ////{
                        ////    //....The routine output unit:      //PB 09JUN09. SB, change elsewhere.
                        ////    //........UnitSystem = English: in.
                        ////    //........           = Metric:  mm.
                        ////    BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                        ////    Double pL_LowerLimit = 0.0F;

                        ////    Double pDThread = 0.0F;

                        ////    //....If     Unit = English, DThread = in.
                        ////    //....ElseIf Unit = Metric,  DThread = mm.
                        ////    //
                        ////    var pProject = (from pRec in pBearingDBEntities.tblManf_Screw_D where pRec.fldD_Desig == mScrew_Spec.D_Desig select pRec).ToList();

                        ////    if (pProject.Count > 0)
                        ////        pDThread = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble((Double)pProject[0].fldD);


                        ////    if (mScrew_Spec.Unit.System == clsUnit.eSystem.English)
                        ////    {
                        ////        //SB 31JUL09

                        ////        //...To be done later. 
                        ////        //....Refer to "Radial_Rev7_10JUL09.xls", column AR (English). 

                        ////        //....MROUND((AN3+0.25)-Z3,0.125)

                        ////        Double pZ3, pAN3;
                        ////        pZ3 = pDThread / 25.4;                          //....Convert to in. 
                        ////        pAN3 = modMain.MRound(2.5 * pZ3, 0.0625);

                        ////        pL_LowerLimit = modMain.MRound(pAN3 + 0.25 - pZ3, 0.125);
                        ////    }

                        ////    else if (mScrew_Spec.Unit.System == clsUnit.eSystem.Metric)
                        ////    {

                        ////        //....Refer to "Radial_Rev5_30MAR05", column AS (Metric). 
                        ////        Double pZ3, pAN3, pAR3, pAQ3;
                        ////        pZ3 = pDThread / 25.4;                          //....Convert to in. 

                        ////        pAN3 = modMain.MRound(2.5 * pZ3, 0.0625);
                        ////        pAR3 = modMain.MRound(pAN3 + 0.25 - pZ3, 0.125);

                        ////        pAQ3 = pAR3 - pAN3 + pZ3;

                        ////        pL_LowerLimit = (pAN3 + pAQ3 - pZ3) * 25.4;    //....Convert back to mm.            
                        ////        pL_LowerLimit = modMain.MRound(pL_LowerLimit, 1);
                        ////    }
                        ////    return pL_LowerLimit;
                        ////}

                    #endregion

                #endregion


                #region "PIN:"
                //-----------    

                    #region "Locations:"
                    //------------------  

                        #region "LDowel:"
                        //---------------  

                            //private Double Calc_LDowel_Loc_Center()
                            // public Double Calc_LDowel_Loc_Center()     //BG 27NOV12
                            ////======================================
                            //{
                            //    Double pVal = 0.0F;

                            //    ////....Ref. Radial_Rev11_27OCT11: Col. BX

                            //    ////........SplitLine_LScrew_Loc.Center: Col. O
                            //    ////........AntiRot_Pin_Spec.D :         Col. GC

                            //    //string pARPin_Loc = mCurrent_Bearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert.ToString();   //....Col. FV.
                            //    //Double pVal = 0.0F;

                            //    //if (pARPin_Loc == "L")
                            //    //    pVal = mLScrew_Loc.Center - mCurrent_Bearing_Radial_FP.AntiRotPin.Spec.D;

                            //    //else if (pARPin_Loc == "R")
                            //    //    pVal = mLScrew_Loc.Center;

                            //    return modMain.MRound(pVal, 0.05);
                            //}


                            //private Double Calc_LDowel_Loc_Front()
                            // public Double Calc_LDowel_Loc_Front()
                            // //=====================================     //BG 27NOV12
                            //{
                            //    //....Ref. Radial_Rev11_27OCT11: Col. BY.
                            //    return Calc_Screw_Loc_Front("RScrew");         //....Col. N
                            //}

                        #endregion


                        #region "RDowel:"
                        //---------------  

                            //private Double Calc_RDowel_Loc_Center()
                            //public Double Calc_RDowel_Loc_Center()         //BG 27NOV12
                            ////======================================
                            //{
                            //    Double pVal = 0.0F;
                            //    ////....Ref. Radial_Rev11_27OCT11: Col. BV   

                            //    ////........SplitLine_RScrew_Loc.Center : Col. M.
                            //    ////........AntiRot_Pin_Spec.D          : Col. GC.

                            //    //string pAR_Pin_Loc = mCurrent_Bearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert.ToString();   //....Col. FV.

                            //    //Double pVal = 0.0F;

                            //    //if (pAR_Pin_Loc == "R")
                            //    //    pVal = mRScrew_Loc.Center - mCurrent_Bearing_Radial_FP.AntiRotPin.Spec.D;

                            //    //else if (pAR_Pin_Loc == "L")
                            //    //    pVal = mRScrew_Loc.Center;

                            //    return modMain.MRound(pVal, 0.05);
                            //}


                            ////private Double Calc_RDowel_Loc_Front()
                            //public Double Calc_RDowel_Loc_Front()          //BG 27NOV12
                            ////=====================================
                            //{
                            //    //....Ref. Radial_Rev11_27OCT11: Col. BW.
                            //    return Calc_Screw_Loc_Front("LScrew");   //....Col. P
                            //}

                        #endregion

                    #endregion


                    #region "Depth:"
                    //---------------

                        private Double Calc_Dowel_Depth()
                        //================================
                        {
                            //....Ref. Radial_Rev11_27OCT11: Col. CI
                            Double pL = 0.0F;

                            if (mDowel.Spec.Unit.System == clsUnit.eSystem.English)
                                pL = mDowel.Spec.L;             //....Col. CF

                            else if (mDowel.Spec.Unit.System == clsUnit.eSystem.Metric)
                                pL = mDowel.Spec.L / 25.4;

                            return modMain.MRound((0.5 * pL + 0.05), 0.01);
                        }

                    #endregion


                    //public Double Pin_L_LowerLimit()
                    ////===============================
                    //{
                    //    return (2.0F * mDowel_Spec.D);
                    //}


                #endregion

            #endregion
        }
    }
}
