
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsRadB_1_Pad                          '
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
using System.Linq;

namespace BearingCAD22
{
    public partial class clsRadB
    {
        [Serializable]
        public class clsPad
        //==================
        {
            #region "NAMED CONSTANTS:"
            //=======================
                private const int mc_COUNT_PAD_MAX = 6;
            #endregion


            //#region "ENUMERATION TYPES:"
            ////==========================
            //    public enum eLoadOrient { LBP, LOP, Other };
            //#endregion


            #region "USER-DEFINED STRUCTURES:"
            //================================

                #region "Pivot:"
                    [Serializable]
                    public struct sPivot
                    {
                        //....If Bidirectional, Offset = 50%. If not 50%, it refers to the pad angular extent 
                        //........from the leading edge to the pivot as a % of the Pad angle.
                        public Double Offset;      //....stored and displayed in %.  
                        public Double AngStart_Casing_SL;
                   }
                #endregion

                #region "Thick:"
                //........If Pivot input checkbox is checked, user will input the corresponding thickness. 
                //............In that case, the thicknesses @ Lead & Trail  will be the same as that @ Pivot. 
                //........If unchecked, user will independently input all three thicknesses. 
                //
                    [Serializable]
                    public struct sT
                    {
                        public Double Lead;
                        public Double Pivot;
                        public Boolean Pivot_Checked;           
                        public Double Trail;
                    }
                #endregion

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private clsRadB mCurrent_RadB;
            
                private string mLoadOrient = "";         
                private int mCount;
                private Double mL;
                private Double mAngle;
                //....Angle;                    //....Method   
                //....Offset                    //....Method.
                //....RFillet_ID;               //....Method.

                private sPivot mPivot;
                private sT mT;
                private Double mRFillet;        //....AES 12SEP18
              
            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //================================  

                public int Count_Max
                {
                    get { return mc_COUNT_PAD_MAX; }
                }


                public string LoadOrient
                {
                    get { return mLoadOrient; }
                    set { mLoadOrient = value; }
                }
                       
                public int Count
                {
                    get { return mCount; }
                    set { mCount = value;}
                }

                public Double L
                {
                    get { return mL; }
                    set { mL = value; }
                }

                public Double Angle
                {
                    get { return mAngle; }
                    set { mAngle = value; }
                }
            

                #region "PIVOT:"
                //--------------

                    public sPivot Pivot
                    {
                        get { return mPivot; }
                    }


                    public Double Pivot_Offset
                    {
                        set { mPivot.Offset = value; }
                    }


                    public Double Pivot_AngStart_Casing_SL
                    {
                        set { mPivot.AngStart_Casing_SL = value; }
                    }

                #endregion


                #region "T:"
                //---------

                    public sT T
                    {
                        get { return mT; }
                    }

                    public Double T_Lead
                    {
                        set { mT.Lead = value; }
                    }

                    public Double T_Pivot
                    {
                        set { mT.Pivot = value; }
                    }

                    public Boolean T_Pivot_Checked         
                    {
                        set { mT.Pivot_Checked = value; }
                    }

                    public Double T_Trail
                    {
                        set{ mT.Trail = value; }
                    }

                #endregion

                public Double RFillet
                {
                    get { return mRFillet; }
                    set { mRFillet = value; }
                }
            
            #endregion


            #region "CONSTRUCTOR:"

                public clsPad(clsRadB Current_RadB_In)
                //====================================
                {
                    mCurrent_RadB = Current_RadB_In;

                    //....Default Values: 
                    mCount = 4;
                    mPivot.Offset = 50.0F;
                }

            #endregion


            #region "CLASS METHODS":

                public Double Offset()
                //--------------------
                {
                    return (0.5F * (mCurrent_RadB.PadBore() - mCurrent_RadB.Bore()));
                }

                //PB 09AUG11. Eventually, there should be a method: Pad_RBack (i as int). 
                // i = 0 ==> at Pivot
                // i = 1 ==> at Leading Edge. 
                // i = 2 ==> at Trailing Edge.
             

                //public Double TDef()                            
                ////------------------       
                //{
                //    Double pDShaft = mCurrent_Bearing_Radial_FP.DShaft();
                //    Double pT = 0.0F;

                //    if (pDShaft > modMain.gcEPS)
                //        pT = (0.15F * pDShaft);

                //    return pT;
                //}


                public Double AngBet()              
                //========================
                {
                    //....Angle between:
                    //          1. Pivot Locations. 
                    //          2. Temp. Sensor Holes.

                    return (360.0F / mCount);
                }

            
                public Double[] Pivot_OtherAng()
                //==============================    
                {
                    Double[] pOtherAng = new Double[mCount];

                    for (int i = 0; i < mCount; i++)
                    {
                        pOtherAng[i] = mPivot.AngStart_Casing_SL + (i * AngBet());
                    }
                    return pOtherAng;
                }

            #endregion
        }
    
    }
}
