//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsRadB_ARP                            '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  19NOV18                                '
//                                                                              '
//===============================================================================

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
        public class clsARP
        {
            #region "ENUMERATION TYPES:"
            //==========================
                public enum eInsertedOn { BearingOD, Flange };
            #endregion


            #region "MEMBER VARIABLES:"
            //==========================
                private clsJBearing mCurrentBearing;

                private Double mLoc_Back;
                private Double mAng_Casing_SL;
                private Double mOffset;
                private String mOffset_Direction;
                private clsPin mDowel;
                private eInsertedOn mInsertedOn;

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================

                public Double Loc_Back
                {
                    get { return mLoc_Back; }
                    set { mLoc_Back = value; }
                }

                public Double Ang_Casing_SL
                {
                    get { return mAng_Casing_SL; }
                    set { mAng_Casing_SL = value; }
                }

                public Double Offset
                {
                    get { return mOffset; }
                    set { mOffset = value; }
                }

                public String Offset_Direction
                {
                    get { return mOffset_Direction; }
                    set { mOffset_Direction = value; }
                }
       
                public clsPin Dowel
                {
                    get { return mDowel; }
                    set { mDowel = value; }
                }

                public eInsertedOn InsertedOn
                {
                    get { return mInsertedOn; }
                    set { mInsertedOn = value; }
                }

            #endregion


            #region "CONSTRUCTOR:"

                public clsARP(clsJBearing CurrentBearing_In)
                //==========================================
                {
                    mCurrentBearing = CurrentBearing_In;

                    mDowel = new clsPin(mCurrentBearing.Unit.System);         
                    mDowel.Spec_Type = "P";
                    mDowel.Spec_Mat = "STEEL";
                    mOffset_Direction = "CCW";
                    mInsertedOn = eInsertedOn.BearingOD;
                }

            #endregion


            #region "CLASS METHODS":
 
                public double Ang_Casing_SL_Horz()            
                //================================
                {
                    double pPivot_AngStart = mCurrentBearing.RadB.Pad.Pivot.AngStart_Casing_SL;     //....w.r.t Casing S/L
                    double pPadAng = mCurrentBearing.RadB.Pad.Angle;
                    int pPadCount = mCurrentBearing.RadB.Pad.Count;
                    double pPivotOffset = mCurrentBearing.RadB.Pad.Pivot.Offset;

                    //double pPad_AngStart_Casing_SL = pPivot_AngStart - 0.5 * pPadAng;     // PB 29OCT18, BG Replace  0.5 *pPadAng ==> (pPivotOffset/ 100) * pPadAng ;
                    double pPad_AngStart_Casing_SL = pPivot_AngStart - (pPivotOffset / 100) * pPadAng;
                    double pPad_AngBet = 360 / pPadCount - pPadAng;

                    //....Calculate Pad Start Angle w.r.t. Horizontal 
                    double pPad_AngStart_Horz = 0;

                    if (mCurrentBearing.RadB.OilInlet.Orifice.StartPos == clsOilInlet.eOrificeStartPos.Below)
                    {
                        pPad_AngStart_Horz = 0;
                    }
                    else if (mCurrentBearing.RadB.OilInlet.Orifice.StartPos == clsOilInlet.eOrificeStartPos.On)
                    {
                        pPad_AngStart_Horz = 0.5 * pPad_AngBet;
                    }
                    else if (mCurrentBearing.RadB.OilInlet.Orifice.StartPos == clsOilInlet.eOrificeStartPos.Above)
                    {
                        pPad_AngStart_Horz = pPad_AngBet;
                    }

                    //....Angle - Casing S/L w.r.t. Horizontal.
                    double pAng_Casing_SL_Horz = pPad_AngStart_Horz - pPad_AngStart_Casing_SL;
                    return pAng_Casing_SL_Horz;
                }


                public double Ang_Horz()          
                //======================
                {
                    double pAngle_Horz = 0;
                    pAngle_Horz = Ang_Casing_SL_Horz() + mAng_Casing_SL;
                    return pAngle_Horz;
                }

                //private Double Depth_DefVal ()              // PB 12OCT18a. 
                ////============================
                //{
                //    //....Ref. Radial_Rev11_27OCT11: Col. GI
                //    return mDowel.D();
                //}

                public Double Stickout(Double L_In)       
                //=================================
                { 
                    Double pStickout = 0.0F;
                    pStickout = L_In - mDowel.Hole.Depth_Low;

                    return pStickout;
                }

            #endregion
        }
    }
}
