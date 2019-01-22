//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsRadB_OilInlet                       '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  19NOV18                                '
//                                                                              '
//===============================================================================
// PB 12OCT18a

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

namespace BearingCAD22
{
    public partial class clsRadB
    {
        [Serializable]
        public class clsOilInlet
        //======================
        {
            #region "ENUMERATION TYPES:"
            //==========================
                public enum eOrificeStartPos {Above, On, Below };      //....w.r.t Bearing S/L (Horizontal)
            #endregion


            #region "USER-DEFINED STRUCTURES:"
            //============================

                #region "Orifice:"
                    [Serializable]
                    public struct sOrifice
                    {
                        public eOrificeStartPos StartPos;
                        public int Count;       //....If Pad L <= 4 in (100 mm), then Orifice_Count = Count_Pad 
                        public Double D;        //    Else                     , then Orifice_Count = Count_Pad or 2*Count_Pad.
                        public Double Ratio_L_D;
                       
                        public double CBore_D;    
                        public Double Loc_Back;   
                        public Double Dist_Holes;   //....If Orifice_Count =   Count_Pad, Dist_FeedHole = 0 (irrelevant).
                                                    //                                                  = 2*Count_Pad, Dist_FeedHole > 0.
                    }
                #endregion


                #region "Annulus:"
                    [Serializable]
                    public struct sAnnulus
                    {
                        public bool Exists;
                        public Double Area;
                        public Double Wid;
                        public Double Depth;
                        public Double D;
                        public Double Ratio_Wid_Depth;     
                        public Double Loc_Back;
                    }
                #endregion

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private clsJBearing mCurrentBearing;
                private sOrifice mOrifice;

                private int mCount_MainOilSupply;

                private sAnnulus mAnnulus;

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //================================ 

                #region "Orifice:"
                //---------------

                    public sOrifice Orifice
                    {
                        get
                        {
                            int pPad_Count = mCurrentBearing.RadB.Pad.Count;

                            if (mOrifice.Count <= 0 && pPad_Count > 0)
                                mOrifice.Count = pPad_Count;

                            if (mOrifice.Loc_Back < modMain.gcEPS)
                                mOrifice.Loc_Back = Orifice_Loc_Back_Def();

                            if (mOrifice.Dist_Holes < modMain.gcEPS)
                                mOrifice.Dist_Holes = Calc_Orifice_Dist_Holes();

                            return mOrifice;
                        }
                    }


                    public int Orifice_Count
                    {
                        set { mOrifice.Count = value; }
                    }

                    public Double Orifice_D
                    {
                        set { mOrifice.D = value; }
                    }

                    public Double Orifice_Ratio_L_D
                    {
                        set { mOrifice.Ratio_L_D = value; }
                    }

                    public Double Orifice_CBore_D
                    {
                        set { mOrifice.CBore_D = value; }
                    }

                    public eOrificeStartPos Orifice_StartPos
                    {
                        set { mOrifice.StartPos = value; }
                    }

                    public Double Orifice_Loc_Back
                    {
                        set { mOrifice.Loc_Back = value; }
                    }

                    public Double Orifice_Dist_Holes
                    {
                        set { mOrifice.Dist_Holes = value; }
                    }       

                #endregion


                #region "Main Members:"
                //---------------------

                    public int Count_MainOilSupply
                    {
                        get { return mCount_MainOilSupply; }
                        set { mCount_MainOilSupply = value; }
                    }

                #endregion


                #region "Annulus:"
                //---------------

                    public sAnnulus Annulus
                    {
                        get
                        {
                            //if (mAnnulus.D < modMain.gcEPS)
                            //    Calc_Annulus_Params();           //....D, L.  

                            //if (mAnnulus.Wid < modMain.gcEPS)
                            //    Calc_Annulus_Params();

                            //if (mAnnulus.Loc_Back < modMain.gcEPS)
                            //    mAnnulus.Loc_Back = Calc_Annulus_Loc_Back();

                            return mAnnulus;
                        }
                    }


                    public bool Annulus_Exists
                    {
                        set { mAnnulus.Exists = value; }
                    }

                    public Double Annulus_Area
                    {
                        set { mAnnulus.Area = value; }
                    }

                    public Double Annulus_Wid
                    {
                        set { mAnnulus.Wid = value; }
                    }

                    public Double Annulus_Depth
                    {
                        set { mAnnulus.Depth = value; }
                    }


                    public Double Annulus_Ratio_Wid_Depth
                    {
                        set { mAnnulus.Ratio_Wid_Depth = value; }
                    }

                    public Double Annulus_D
                    {
                        set { mAnnulus.D = value; }
                    }

                 
                    public Double Annulus_Loc_Back
                    {
                        set { mAnnulus.Loc_Back = value; }
                    }

                #endregion


            #endregion


            #region "CONSTRUCTOR:"

                    public clsOilInlet(clsJBearing CurrentBearing_In)
                //=====================================================================
                {
                    mCurrentBearing = CurrentBearing_In;
                    mCount_MainOilSupply = 1;
                    mOrifice.StartPos = eOrificeStartPos.Below;
                    mOrifice.Ratio_L_D = 1.5;
                }

            #endregion


            #region "CLASS METHODS":

                #region "ORIFICE:"

                    public Double Orifice_CB_DMin()
                    //===================================
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. DK
                        //........Oil Inlet Orifice D: Col. DJ
                        return modMain.MRound(2.5 * mOrifice.D, 0.0625);
                    }
                   
                    public Double Orifice_Loc_Back_Def()         
                    //========================================  
                    {
                        Double pDepthF = mCurrentBearing.RadB.EndPlateCB[0].Depth;        //....Col. CZ.
                        //Double pDepthB = mCurrent_Bearing_Radial_FP.Depth_EndConfig[1];        //....Col. DA.   PB 18JAN13. Not yet needed.

                        Double pVal = 0.0;

                        if (!Orifice_Exists_2ndSet())
                        {
                            //....# of Orifice Sets = 1.
                            pVal = pDepthF + mCurrentBearing.RadB.AxialSealGap[0] + (0.5 * mCurrentBearing.RadB.Pad.L);
                        }
                        else
                        {
                            //....# of Orifice Sets = 2.
                            pVal = pDepthF + mCurrentBearing.RadB.AxialSealGap[0] + (mCurrentBearing.RadB.Pad.L / 2.5);
                        }

                        return pVal;
                    }

                    public Double Orifice_L()
                    //=============================
                    { 
                        return mOrifice.Ratio_L_D * mOrifice.D; 
                    }

                #endregion


                #region "MAIN:"
     
                    public bool Orifice_Exists_2ndSet()
                    //==================================
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. DQ
                        if (mOrifice.Count == 2 * mCurrentBearing.RadB.Pad.Count)
                            return true;
                        else
                            return false;
                    }


                    private Double Calc_Orifice_Dist_Holes()      
                    //======================================
                    {
                        Double pDepthB = mCurrentBearing.RadB.EndPlateCB[1].Depth;
                        Double pPadL = mCurrentBearing.RadB.Pad.L;

                        Double pVal = 0.0F;

                        if (Orifice_Exists_2ndSet())
                            //....# of Orifice Sets = 2.
                            pVal = 0.2 * pPadL;

                        else
                            //....# of Orifice Sets = 1.
                            //........Design Table cols. requires a non-null value.
                            pVal = 1;

                        return pVal;
                    }

                #endregion


                #region "ANNULUS:"

                    public void Calc_Annulus_Params()
                    //=================================
                    {
                        //....Flow reqd. GPM.
                        Double pFlowReqd_gpm = mCurrentBearing.PerformData.FlowReqd;

                        //....Calculate AMin:
                        Double pUp, pDown;
                        pUp = 231 * pFlowReqd_gpm;
                        pDown = 2 * mCount_MainOilSupply * 60 * 12 * 10;

                        Double pAMin = 0.0F;
                        if (pDown != 0.0F)
                            pAMin = pUp / pDown;

                        //....Calculate H:
                        Double pH = 0.0F;

                        if (mAnnulus.Ratio_Wid_Depth != 0.0F)
                        {
                            double pAny = 0.0F;
                            pAny = (pAMin / mAnnulus.Ratio_Wid_Depth);

                            if (pAny != 0.0F)
                                pH = (Double)Math.Sqrt(pAny);
                        }

                        //....Diameter & Length:
                        //
                        if (pH != 0.0F)
                        {
                            mAnnulus.D = mCurrentBearing.RadB.OD() - 2 * pH;
                            mAnnulus.Wid = pH * mAnnulus.Ratio_Wid_Depth;
                        }
                    }

                    public Double Calc_Annulus_L(Double Annulus_D_In)
                    //================================================
                    {
                        Double pH = 0.5 * (mCurrentBearing.RadB.OD() - Annulus_D_In);
                        Double pL = mAnnulus.Ratio_Wid_Depth * pH;

                        return pL;
                    }

                    public Double Calc_Annulus_Ratio_L_H(Double Annulus_D_In, Double Annulus_L_In)
                    //==============================================================================        
                    {
                        Double pH = 0.5 * (mCurrentBearing.RadB.OD() - Annulus_D_In);
                        Double pRatio_L_H = Annulus_L_In / pH;

                        return pRatio_L_H;
                    }

                    public Double Annulus_V(Double Annulus_D_In, Double Annulus_L_In)
                    //================================================================      
                    {
                        Double pV = 0.0F;

                        //....Annulus Height.
                        Double pH;
                        pH = 0.5F * (mCurrentBearing.RadB.OD() - Annulus_D_In);

                        //....Annulus Area.
                        Double pArea;
                        //pArea = pH * Annulus.L;
                        pArea = pH * Annulus_L_In;

                        //....Velocity
                        Double pFlowReqd_gpm = mCurrentBearing.PerformData.FlowReqd;

                        Double pUp, pDown;
                        pUp = 231 * pFlowReqd_gpm;
                        pDown = 2 * mCount_MainOilSupply * 60 * 12 * pArea;

                        if (pDown != 0.0F)
                            pV = pUp / pDown;

                        return pV;
                    }

                    //public Double Calc_Annulus_Loc_Back()
                    ////=====================================
                    //{
                    //    return (0.5 * (mCurrent_Bearing_Radial_FP.L - mAnnulus.Wid));
                    //}

                #endregion

            #endregion
        }
    }
}
