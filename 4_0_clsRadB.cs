
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial                      '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearingCAD22
{
    [Serializable]
    public partial class clsRadB
    {
        #region "ENUMERATION TYPES:"
        //==========================
           public enum eDesign { Flexure_Pivot, Tilting_Pad, Sleeve };      
        #endregion

        #region "USER-DEFINED STRUCTURES:"
        //================================

           [Serializable]
           public struct sEndPlateCB
           {
               public Double D;
               public Double Depth;
           }
          
        #endregion

        #region "NAMED CONSTANTS:"
        //========================

           //DESIGN PARAMETERS (Commented-out ones are not used internally):
           //------------------
           //....EDM Relief (used in Main Class & clsOilInlet).
           //private const Double mc_DESIGN_EDM_RELIEF = 0.010D;       

           //....Min. End Config. Depth
           private const Double mc_DEPTH_END_CONFIG_MIN_ENGLISH = 0.2;
           private const Double mc_DEPTH_END_CONFIG_MIN_METRIC = 5;

           //........Used in EndConfig_DO_Max (), DFit().
           private const Double mc_DESIGN_DCLEAR = 0.002F;    //....Diametral Clearance between Bearing CB & End Plate OD)
           private const Double mcEndPlateCB_TWall_Min = modMain.gcSep_Min;

           //....EDM Relief (used in Main Class & clsOilInlet).
           private const Double mc_DESIGN_EDM_RELIEF = 0.010D;   

           //OTHERS:
           //------
           //....Others Angle Count 
           private const int mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX = 7;     

        #endregion

        #region "MEMBER VARIABLES:"
        //=========================
           protected clsJBearing mCurrentBearing;
           private eDesign mDesign;

           private bool mSplitConfig;

           #region "Diameters:"
               //....Min.= 0 & Max.= 1:
               //  
               private Double[] mOD_Range = new Double[2];           //....OD  
               private Double[] mPadBore_Range = new Double[2];
               private Double[] mBore_Range = new Double[2];
               private Double[] mDShaft_Range = new Double[2];
           #endregion

           private sEndPlateCB[] mEndPlateCB = new sEndPlateCB[2];

           #region "Lengths:"
               private Double mL;
           #endregion
                       
               private Double[] mAxialSealGap = new Double[2];       

           #region "Materials:"
               private clsMaterial mMat = new clsMaterial();
               private Double mLiningT;                                //....Not included in clsMaterial.
           #endregion

           private clsPad mPad;
           private clsPivot mPivot;
           private bool mMillRelief_Exists;
           public string mMillRelief_D_Desig;

           #region "Detailed Design Data:"
                private clsOilInlet mOilInlet;               
                private clsARP mARP;
                private clsSL mSL;
                private clsFlange mFlange;
           #endregion

        #endregion

        #region "CLASS PROPERTY ROUTINES:"
        //================================
            
            #region "NAMED CONSTANTS:"

                public Double DESIGN_DCLEAR
                //===========================
                {
                    get { return mc_DESIGN_DCLEAR; }
                }

                public Double DESIGN_EDM_RELIEF
                //=============================    
                {
                    get { return mc_DESIGN_EDM_RELIEF; }
                }

                public Double DEPTH_END_CONFIG_MIN_ENGLISH
                //========================================
                {
                    get { return mc_DEPTH_END_CONFIG_MIN_ENGLISH; }
                }

                public Double DEPTH_END_CONFIG_MIN_METRIC
                //=======================================
                {
                    get { return mc_DEPTH_END_CONFIG_MIN_METRIC; }
                } 
 

            #endregion

                public int COUNT_MOUNT_HOLES_ANG_OTHER_MAX
                {
                    get { return mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX; }
                } 

                public eDesign Design
                {
                    get { return mDesign; }
                    set { mDesign = value; }
                }

            #region "Split Configuration:"

                public bool SplitConfig
                {
                    get { return mSplitConfig; }
                    set { mSplitConfig = value; }
                }
            #endregion


            #region "Diameters:"

                //....OD:
                public Double[] OD_Range
                {
                    get { return mOD_Range; }
                    set { mOD_Range = value; }
                }

                //.... Pad Bore:             
                public Double[] PadBore_Range
                {
                    get { return mPadBore_Range; }
                    set { mPadBore_Range = value; }
                }

                //.... Bore:            
                public Double[] Bore_Range
                {
                    get { return mBore_Range; }
                    set { mBore_Range = value; }
                }

                //....Shaft Dia:
                public Double[] DShaft_Range
                {
                    get { return mDShaft_Range; }
                    set
                    {
                        mDShaft_Range = value;
                    }
                }

            #endregion

            //....EndPlateCB:
            public sEndPlateCB[] EndPlateCB
            {
                get 
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (mEndPlateCB[i].Depth < modMain.gcEPS)
                        {
                            mEndPlateCB[i].Depth = EndPlateCB_Depth_Def();
                        }
                    }
                    return mEndPlateCB; 
                }
                set { mEndPlateCB = value; }
            }
                 

            #region "Lengths:"

                public Double L
                {
                    get { return mL; }
                    set { mL = value; }
                }


                //#region "Depth - End Plates:"

                //public double[] EndPlateCB_Depth
                //{
                //    get
                //    {
                //        for (int i = 0; i < 2; i++)
                //        {
                //            if (mEndPlateCB[i].Depth < modMain.gcEPS)
                //            {
                //                mEndPlateCB[i].Depth = Calc_Depth_EndPlate();
                //            }
                //        }
                //        return mEndPlateCB;
                //    }
                //}

                //#endregion

            #endregion

                #region "Mill Relief:"

                    public bool MillRelief_Exists
                    {
                        get { return mMillRelief_Exists; }
                        set { mMillRelief_Exists = value; }
                    }

                    public string MillRelief_D_Desig
                    {
                        get { return mMillRelief_D_Desig; }
                        set { mMillRelief_D_Desig = value; }
                    }

                    public Double[] AxialSealGap
                    {
                        get
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (mAxialSealGap[i] < modMain.gcEPS)
                                {
                                    mAxialSealGap[i] = mc_DESIGN_EDM_RELIEF;
                                }
                            }
                            return mAxialSealGap;
                        }
                    }

                #endregion
         
                #region "Materials:"

                    public clsMaterial Mat
                    {
                        get { return mMat; }
                        set { mMat = value; }
                    }

                    //.... Lining Thickness.
                    public Double LiningT
                    {
                        get
                        {
                            return mLiningT;
                        }
                        set { mLiningT = value; }
                    }

                #endregion

                #region "PAD:"
                //============  

                    public clsPad Pad
                    {
                        get { return mPad; }
                        set { mPad = value; }
                    }

                #endregion

                #region "PIVOT:"

                    public clsPivot Pivot
                    {
                        get { return mPivot; }
                        set { mPivot = value; }
                    } 

                #endregion

                #region "OIL INLET:"

                    public clsOilInlet OilInlet
                    {
                        get { return mOilInlet; }
                        set { mOilInlet = value; }
                    }

                #endregion

           
            #region "ANTI ROTATION PIN:"
            //==========================

                public clsARP ARP
                {
                    get { return mARP; }
                    set { mARP = value; }
                }

            #endregion

            #region "SPLIT LINE HARDWARE:"
            //===========================

                public clsSL SL
                {
                    get { return mSL; }
                    set { mSL = value; }
                }
            #endregion
         
            #region "FLANGE:"
            //===============

                public clsFlange Flange
                {
                    get { return mFlange; }
                    set { mFlange = value; }
                }

            #endregion

        #endregion

            //....Class Constructor
            public clsRadB(clsUnit.eSystem UnitSystem_In, eDesign Design_In, clsJBearing CurrentBearing_In)
            //=============================================================================================
            {
                mCurrentBearing = CurrentBearing_In;

                if (Design_In == eDesign.Flexure_Pivot)
                {
                    mPivot = new clsPivot.clsFP(UnitSystem_In, clsRadB.eDesign.Flexure_Pivot, CurrentBearing_In);
                }
                else if (Design_In == eDesign.Tilting_Pad)
                {
                    mPivot = new clsPivot.clsTP(UnitSystem_In, clsRadB.eDesign.Tilting_Pad, CurrentBearing_In);
                }

                //....Instantiate member class objects: 
                mPad = new clsPad(this);
                mOilInlet = new clsOilInlet(CurrentBearing_In);
                mARP = new clsARP(CurrentBearing_In);
                mSL = new clsSL(CurrentBearing_In);
                mFlange = new clsFlange(this);


                //....Initialize: 
                mSplitConfig = true;

                //........Material.
                mMat.WCode_Base = "1002-107";
                mMat.LiningExists = true;
                mMat.WCode_Lining = "1002-960";
            }


            #region "CLASS METHODS:"
            //*********************

                #region "REF. / DEPENDENT VARIABLES:"

                    #region "LENGTHS:"

                        public Double EndPlateCB_Depth_Def()
                        //----------------------------------
                        {
                            //........Assumes equal depth on both sides as a starting estimate. 
                            double pDepth = 0.0F;
                            pDepth = (mL - (mPad.L + mAxialSealGap[0] + mAxialSealGap[1])) * 0.5F;
                            return pDepth;
                        }

                        public Double EndPlateCB_DMax()
                        //==============================          
                        {
                            Double pDCB_Max = 0;
                            pDCB_Max = OD() - 2 * mcEndPlateCB_TWall_Min;

                            return pDCB_Max;
                        }

                        public Double EndPlateCB_D(int Indx_In)
                        //=====================================                 
                        {
                            Double pDCB = 0.0;
                            pDCB = ((clsJBearing)mCurrentBearing).EndPlate[Indx_In].OD + mc_DESIGN_DCLEAR;
                            return pDCB;
                        }

                        public Double EndPlateCB_TWall(int Indx_In)
                        //==========================================           
                        {
                            Double pTWall = 0;
                            pTWall = 0.5 * (OD() - EndPlateCB_D(Indx_In));

                            return pTWall;
                        }

                    #endregion
         
                    #region "DIAMETERS:"

                        //....Nominal 

                        public Double OD()
                        //----------------
                        {
                            return modMain.Nom_Val(mOD_Range);
                        }

                        public Double PadBore()
                        //---------------------
                        {
                            return modMain.Nom_Val(mPadBore_Range);
                        }

                        public Double Bore()
                        //-------------------
                        {
                            return modMain.Nom_Val(mBore_Range);
                        }

                        public Double DShaft()
                        //---------------------
                        {
                            return modMain.Nom_Val(mDShaft_Range);
                        }

                        //public Double Clearance()  
                        ////-----------------------
                        //{
                        //    return (mBore_Range[0] - mDShaft_Range[0]);
                        //}


                        //public Double PreLoad()
                        ////---------------------
                        //{
                        //    Double pPreLoad = 0.0f;
                        //    pPreLoad = (PadBore() - Bore()) / (PadBore() - DShaft());

                        //    return pPreLoad;
                        //}

                    #endregion


                    public Double PadRelief_D()
                    //==========================
                    {
                        Double pPadReleif_D = 0.0;

                        Double pDBore = Bore();
                        Double pPad_TPivot = mPad.T.Pivot;
                        Double pWeb_H = ((clsPivot.clsFP)mPivot).Web.H;//mFP.FlexurePivot.Web.H;

                        if (!mMillRelief_Exists)
                        {
                            pPadReleif_D = pDBore + 2 * (pPad_TPivot + pWeb_H + 0.020);
                        }
                        else if (mMillRelief_Exists == true)
                        {
                            pPadReleif_D = pDBore + 2 * (pPad_TPivot + mAxialSealGap[0]) + 0.020;
                        }

                        return pPadReleif_D;
                    }

                    public Double MillRelief_D()
                    //==========================
                    {
                        if (mMillRelief_D_Desig != null && mMillRelief_D_Desig != "")
                        {
                            return modMain.DVal(mMillRelief_D_Desig);
                        }
                        else
                            return 0;
                    }

                #endregion

            #endregion

    }
}
