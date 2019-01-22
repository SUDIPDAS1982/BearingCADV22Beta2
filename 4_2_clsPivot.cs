//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsPivot                               '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  20NOV18                                '
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
    [Serializable]
    public abstract class  clsPivot
    {
        #region "ENUMERATION TYPES:"
        //==========================

        #endregion

        #region "MEMBER VARIABLES:"
        //=========================
            private clsRadB.eDesign mDesign;

        #endregion

        #region "PROPERTIES:"
        //===================
            public clsRadB.eDesign Design
            {
                get { return mDesign; }
                set { mDesign = value; }
            }

        #endregion

        #region "CONSTRUCTOR:"

            public clsPivot(clsRadB.eDesign Design_In)
            //======================================
            {
                mDesign = Design_In;
            }

        #endregion

        #region "NESTED CLASS:"

            #region "CLASS - FP:"
                [Serializable]
                public partial class clsFP : clsPivot, ICloneable
                {

                    #region "USER-DEFINED STRUCTURES:"
                    //================================
                        [Serializable]
                        //.....Web
                        public struct sWeb
                        {
                            public Double T;
                            public Double H;
                            public Double RFillet;
                        }

                    #endregion


                    #region "MEMBER VARIABLES:"
                    //========================
                        protected clsJBearing mCurrentBearing;

                        private sWeb mWeb;
                        private Double mGapEDM;

                        #region "Detailed Design Data:"
                        private clsEDM_Pad mEDM_Pad;

                    #endregion

                    #endregion


                    #region "CLASS PROPERTY ROUTINES:"
                    //==============================

                    #region "Web:"
                    //------------
                        public sWeb Web
                        {
                            get
                            {
                                if (mWeb.RFillet < modMain.gcEPS)
                                {
                                    mWeb.RFillet = Calc_Web_RFillet();
                                }
                                return mWeb;
                            }
                        }
            
                        public Double Web_T
                        {
                            set { mWeb.T = value; }
                        }

                        public Double Web_H
                        {
                            set { mWeb.H = value; }
                        }

                        public Double Web_RFillet
                        {
                            set { mWeb.RFillet = value; }
                        }

                    #endregion

                    public Double GapEDM
                    {
                        get { return mGapEDM; }
                        set { mGapEDM = value; }
                    }

                    #region "TEMP SENSOR HOLES:"
                    //=========================

                    //public clsTempSensor TempSensor
                    //{
                    //    get { return mTempSensor; }
                    //    set { mTempSensor = value; }
                    //}

                    #endregion


                    #region "EDM Pad:"
                    //===============

                        public clsEDM_Pad EDM_Pad
                        {
                            get { return mEDM_Pad; }
                            set { mEDM_Pad = value; }
                        }

                    #endregion


                    #endregion


                    #region "CONSTRUCTOR:"

                        public clsFP(clsUnit.eSystem UnitSystem_In, clsRadB.eDesign Design_In, clsJBearing CurrentBearing_In)
                            : base(Design_In)
                        //===============================================================================================
                        {
                            //....Instantiate member class objects:                   
                            mEDM_Pad = new clsEDM_Pad(CurrentBearing_In);

                            mCurrentBearing = CurrentBearing_In;
                        }

                    #endregion


                    #region "CLASS METHODS:"
                    //*********************

                        public Double Calc_Web_RFillet()
                        //=================================
                        {
                            Double pWeb_RFillet = modMain.MRound(mWeb.T, 0.005);
                            return pWeb_RFillet;
                        }


                    #region "ICLONEABLE MEMBERS:"
                    //==========================

                        public object Clone()
                        //===================
                        {
                            //return this.MemberwiseClone();

                            BinaryFormatter pBinSerializer;
                            StreamingContext pStreamContext;

                            pStreamContext = new StreamingContext(StreamingContextStates.Clone);
                            pBinSerializer = new BinaryFormatter(null, pStreamContext);

                            MemoryStream pMemBuffer;
                            pMemBuffer = new MemoryStream();

                            //....Serialize the object into the memory stream
                            pBinSerializer.Serialize(pMemBuffer, this);

                            //....Move the stream pointer to the beginning of the memory stream
                            pMemBuffer.Seek(0, SeekOrigin.Begin);


                            //....Get the serialized object from the memory stream
                            Object pobjClone;
                            pobjClone = pBinSerializer.Deserialize(pMemBuffer);
                            pMemBuffer.Close();   //....Release the memory stream.

                            return pobjClone;    //.... Return the deeply cloned object.
                        }

                    #endregion


                    #endregion

                    [Serializable]
                    public class clsEDM_Pad
                    //======================
                    {
                        #region "NAMED CONSTANTS:"
                        //========================
                        //private const Double mc_PAD_RFILLET = 0.030F;           //PB 13FEB12. To be checked later.

                        #endregion


                        #region "USER-DEFINED STRUCTURE:"
                        //================================
                            [Serializable]
                            public struct sFillet
                            {
                                public Double ID;
                                public Double Back;         // PB 12OCT18b Not needed
                            }

                        #endregion


                        #region "MEMBER VARIABLES:"
                        //=========================
                            private clsJBearing mCurrent_Bearing;

                            private Double mAng_Offset;
                            private Double mRFillet_Back;
                            private Double mAngStart_Web;

                        #endregion


                        #region "CLASS PROPERTY ROUTINES:"
                        //================================ 

                        public Double Ang_Offset
                        {
                            get
                            {
                                return mAng_Offset;
                            }

                            set { mAng_Offset = value; }
                        }

                        public Double RFillet_Back
                        {
                            get
                            {
                                if (mRFillet_Back < modMain.gcEPS)
                                {
                                    mRFillet_Back = mCurrent_Bearing.RadB.Pad.RFillet;
                                }

                                return mRFillet_Back;
                            }

                            set { mRFillet_Back = value; }
                        }


                        public Double AngStart_Web
                        {
                            get
                            {
                                //if (mAngStart_Web < modMain.gcEPS)
                                mAngStart_Web = Calc_AngStart_Web();
                                return mAngStart_Web;
                            }

                            set { mAngStart_Web = value; }
                        }

                        #endregion


                        #region "CONSTRUCTOR:"

                        public clsEDM_Pad(clsJBearing Current_Bearing_In)
                        //===============================================
                        {
                            mCurrent_Bearing = Current_Bearing_In;
                        }

                        #endregion


                        #region "CLASS METHODS":

                        //public Double Ang_Offset(clsOpCond OpCond_In)
                        ////===========================================   
                        //{
                        //    //PB 25JAN12. This routine has a few conflicting/confusing relationship. 
                        //    //....Ref. Radial_Rev11_27OCT11: Col. IW

                        //    Double pVal = 0.0F;
                        //    Double pPadAngle    = mCurrent_Bearing_Radial_FP.Pad.Angle;
                        //    Double pPivotOffset = 0.0F;

                        //    if (OpCond_In.Rot_Directionality == clsOpCond.eRotDirectionality.Uni)
                        //    {
                        //        pPivotOffset = mCurrent_Bearing_Radial_FP.Pad.Pivot.Offset;
                        //        pVal = ((100 - pPivotOffset) / 100) * pPadAngle;
                        //    }

                        //    else if (OpCond_In.Rot_Directionality == clsOpCond.eRotDirectionality.Bi)
                        //        pVal = 0.5 * mCurrent_Bearing_Radial_FP.Pad.Angle;

                        //    return pVal;    //BG 07MAY12
                        //    //return modMain.MRound(pVal, 1);
                        //}


                        private Double Calc_AngStart_Web()
                        //================================       
                        {
                            //PB 25JAN12. This routine has a few conflicting/confusing relationship. To be discussed with HK. 
                            //....Ref. Radial_Rev11_27OCT11: Col. IY.

                            Double pPadCount = mCurrent_Bearing.RadB.Pad.Count;
                            Double pPivotOffset = mCurrent_Bearing.RadB.Pad.Pivot.Offset;
                            Double pPadAngle = mCurrent_Bearing.RadB.Pad.Angle;

                            Double pVal = 0.0F;

                            pVal = 0.5 * (360 / pPadCount) + ((pPivotOffset - 50) / 100) * pPadAngle;

                            //if (OpCond_In.Rot_Directionality == clsOpCond.eRotDirectionality.Uni)
                            //{
                            //    pVal = 0.5 * (360 / pPadCount) + ((pPivotOffset - 50) / 100) * pPadAngle;
                            //}

                            //else if (OpCond_In.Rot_Directionality == clsOpCond.eRotDirectionality.Bi)
                            //    pVal = 0.5 * (360 / pPadCount);

                            return pVal;
                        }

                        #endregion
                    }

                }

            #endregion

            #region "CLASS - TP:"

                [Serializable]
                public class clsTP : clsPivot
                {
                    #region "MEMBER VARIABLES:"
                    //========================

                    #endregion

                    #region "CONSTRUCTOR:"

                        public clsTP(clsUnit.eSystem UnitSystem_In, clsRadB.eDesign Design_In, clsJBearing CurrentBearing_In)
                            : base(Design_In)
                        //======================================================================================
                        {
                        }

                    #endregion
                }
            #endregion

        #endregion
    }
}
