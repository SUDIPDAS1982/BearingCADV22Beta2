
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsJBearing                            '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  20DEC18                                '
//                                                                              '
//===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace BearingCAD22
{
    [Serializable]
    public class clsJBearing: clsBearing, ICloneable
    {
        #region "ENUMERATION TYPES:"
        //==========================            
            public enum eEndPlatePos { Inside = 0, Overhang = 1 };      //....Inside = 0 (includes Flush), Overhung = 1     
            public enum eBoltingType { Front, Back, Both };
        #endregion


        #region "MEMBER VARIABLES:"
        //=========================             
            private clsUnit mUnit = new clsUnit();
            private clsOpCond mOpCond = new clsOpCond();
            private clsPerformData mPerformData = new clsPerformData();

            private clsRadB mRadB;

            //....Front: 0, Back:1.
            private clsEndPlate[] mEndPlate;
            private clsMount[] mMount;

            private Double mL_Available;                //....Constraint - Total available envelope length.

        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //================================

            public clsUnit Unit
            {
                get { return mUnit; }
                set
                {
                    mUnit = value;
                    mEndPlate[0].Unit.System = mUnit.System;
                    mEndPlate[1].Unit.System = mUnit.System;
                }
            }

            #region "OPERATING CONDITION:"
            //========================

                public clsOpCond OpCond
                {
                    get { return mOpCond; }
                    set { mOpCond = value; }
                }

            #endregion

            #region "PERFORMANCE DATA:"
            //========================

                public clsPerformData PerformData
                {
                    get { return mPerformData; }
                    set { mPerformData = value; }
                }

            #endregion

            public clsRadB RadB
            {
                get { return mRadB; }
                set { mRadB = value; }
            }

            public clsEndPlate[] EndPlate
            {
                get { return mEndPlate; }
                set { mEndPlate = value; }
            }

            public clsMount[] Mount
            {
                get { return mMount; }
                set { mMount = value; }
            }

            public Double L_Available
            {
                get { return mL_Available; }
                set { mL_Available = value; }
            }

        #endregion


        //....Class Constructor
        public clsJBearing(clsUnit.eSystem UnitSystem_In, clsBearing.eType BearingType_In)
            : base(UnitSystem_In, BearingType_In)
        //===============================================================================
        {
            mUnit.System = UnitSystem_In;

            mRadB = new clsRadB(UnitSystem_In, clsRadB.eDesign.Flexure_Pivot, this);

            //....End Plates:
            mEndPlate = new clsEndPlate[2];

            //....Mount
            mMount = new clsMount[2];

            for (int i = 0; i < 2; i++)
            {
                mEndPlate[i] = new clsEndPlate(mUnit.System, this);
                mMount[i] = new clsMount(this);
            }
        }

        #region "CLASS METHODS:"
        //*********************

            public Double L_Tot()
            //-------------------
            {
                //....Relevant Radial Bearing Parameters:
                //....Keep the following commented lines for the sake of history.
                //double pEDM_Relief = ((clsBearing_Radial_FP)mBearing).DESIGN_EDM_RELIEF;
                //double pEDM_Relief_Tot = ((clsBearing_Radial_FP)mBearing).EDM_Relief[0] + 
                //                         ((clsBearing_Radial_FP)mBearing).EDM_Relief[1];     

                double pAxialSealGap_Tot = mRadB.AxialSealGap[0] +
                                           mRadB.AxialSealGap[1];

                double pBearing_Pad_L = mRadB.Pad.L;

                double pL_Tot = 0;


                //....Store End Configs Depth & Lengths in local variables:
                //
                double[] pDepth_EndConfig = new double[2];
                double[] pL_EndConfig = new double[2];

                for (int i = 0; i < 2; i++)
                {
                    pDepth_EndConfig[i] = mRadB.EndPlateCB[i].Depth;
                    pL_EndConfig[i] = mEndPlate[i].L;
                }

                //Calculate Total Length of the Product Assembly.
                //-----------------------------------------------
                //
                //....Case 1: Both End Configs are overhung. 
                //
                if (EndPlatePos(0) == eEndPlatePos.Overhang &&
                    EndPlatePos(1) == eEndPlatePos.Overhang)
                {
                    pL_Tot = pL_EndConfig[0] + pBearing_Pad_L + pAxialSealGap_Tot + pL_EndConfig[1];
                }

                //....Case 2: Both End Configs are Flush / Inside. 
                //
                else if (EndPlatePos(0) == eEndPlatePos.Inside &&
                    EndPlatePos(1) == eEndPlatePos.Inside)
                {
                    pL_Tot = pDepth_EndConfig[0] + pBearing_Pad_L + pAxialSealGap_Tot + pDepth_EndConfig[1];
                }

                //....Case 3: Front End Config = Inside & Back = Overhung.
                //
                else if (EndPlatePos(0) == eEndPlatePos.Inside &&
                    EndPlatePos(1) == eEndPlatePos.Overhang)
                {
                    pL_Tot = pDepth_EndConfig[0] + pBearing_Pad_L + pAxialSealGap_Tot + pL_EndConfig[1];
                }

                //....Case 4: Front End Config = Overhung & Back = Flush/Inside.
                else if (EndPlatePos(0) == eEndPlatePos.Overhang &&
                    EndPlatePos(1) == eEndPlatePos.Inside)
                {
                    pL_Tot = pL_EndConfig[0] + pBearing_Pad_L + pAxialSealGap_Tot + pDepth_EndConfig[1];
                }

                return pL_Tot;
            }


            public Double EndPlate_L_Def()         
            //-----------------------------      
            {
                //....Default Case: Both End Configs' are of equal length.
                double pAxialSealGap_Tot = mRadB.AxialSealGap[0] +
                                         mRadB.AxialSealGap[1];
                double pBearing_Pad_L = mRadB.Pad.L;

                Double pL = 0.0;
                pL = 0.5 * (mL_Available - (pBearing_Pad_L + pAxialSealGap_Tot));

                return pL;
            }


            public eEndPlatePos EndPlatePos(int Index_In)
            //===========================================
            {
                eEndPlatePos pEndPlatePos = eEndPlatePos.Inside;

                //....Store End Configs Depth & Lengths in local variables:
                //
                double pDepth;
                double pL;

                pDepth = mRadB.EndPlateCB[Index_In].Depth;
                pL = mEndPlate[Index_In].L;


                //....Determine End Configs' State:   Overhang, Flush/Inside.
                //
                if (pL > pDepth)
                    pEndPlatePos = eEndPlatePos.Overhang;
                else
                    pEndPlatePos = eEndPlatePos.Inside;    //....Also include Flush. 

                return pEndPlatePos;
            }


            public eBoltingType BoltingType()
            //===============================
            {
                eBoltingType pBolting = eBoltingType.Both;

                if (mMount[0].Bolting == true && mMount[1].Bolting == true)
                {
                    pBolting = eBoltingType.Both;
                }
                else if (mMount[0].Bolting == true && mMount[1].Bolting == false)
                {
                    pBolting = eBoltingType.Front;
                }
                else if (mMount[0].Bolting == false && mMount[1].Bolting == true)
                {
                    pBolting = eBoltingType.Back;
                }

                return pBolting;
            }


            public double MountFalse_ScrewEngagement()
            //========================================
            {
                double pVal = 0;
                if (mMount[0].Bolting == false)
                {
                    pVal = mMount[1].Screw.Hole.Depth.Min_Engagement;   //will be a calculation
                }
                else if (mMount[1].Bolting == false)
                {
                    pVal = mMount[0].Screw.Hole.Depth.Min_Engagement;   //will be a calculation
                }
                return pVal;
            }

        #endregion

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
       
    }
}
