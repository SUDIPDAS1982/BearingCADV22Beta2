//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsEndPlate                            '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  20NOV18                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Windows.Forms;  

namespace BearingCAD22
{
     [Serializable]
    public class clsEndPlate
    {
        #region "Main Class:"
         
            #region "MEMBER VARIABLES:"
            //========================
                protected clsJBearing mCurrentBearing;
                private clsSeal mSeal;
                private clsTLTB mTLTB; 

                private clsUnit mUnit = new clsUnit();              
                //private eType mType;
                private clsMaterial mMat = new clsMaterial();       //...Materials - Base & Lining.
                private Double mLiningT;                            

                //....Envelope Geometry:
                private Double mOD;
                private Double[] mDBore_Range = new Double[2];
                private Double mL;
            #endregion


            #region "PROPERTY ROUTINES:"
            //=========================

                public clsSeal Seal
                {
                    get { return mSeal; }
                    set { mSeal = value; }
                }

                public clsTLTB TLTB
                {
                    get { return mTLTB; }
                    set { mTLTB = value; }
                }

                public clsUnit Unit
                {
                    get { return mUnit; }
                }
               
                // Base & Lining Materials:
                //------------------------
                public clsMaterial Mat
                {
                    get { return mMat; }
                    set { mMat = value; }
                }

                public Double Mat_LiningT
                {
                    get { return mLiningT; }
                    set { mLiningT = value; }
                }

                #region "Envelope Geometry:"

                    //....OD  
                    public Double OD
                    {
                        get { return mOD; }
                        set { mOD = value; }
                    }


                    //....Bore Dia 
                    public Double[] DBore_Range
                    {
                        get { return mDBore_Range; }
                        set { mDBore_Range = value;}
                    }

                    //....Length                   
                    public Double L
                    {
                        get
                        {
                            //if (mL < modMain.gcEPS)
                            //{
                            //    mL = ((clsJBearing)mCurrentBearing).EndPlate_L_Def();
                            //}
                            return mL;
                        }
                        set { mL = value; }
                    }

                #endregion
         
            #endregion


            #region "Constructor:"

                    public clsEndPlate(clsUnit.eSystem UnitSystem_In, clsJBearing CurrentBearing_In)
                    //=============================================================================
                    {
                        mUnit.System = UnitSystem_In;
                       
                        mMat.LiningExists = false;
                        mMat.Lining = "None";          

                        mCurrentBearing = CurrentBearing_In;
                        mSeal = new clsSeal(UnitSystem_In, CurrentBearing_In, this);
                        mTLTB = new clsTLTB(UnitSystem_In, CurrentBearing_In, this);
                    }

            #endregion


            #region "CLASS METHODS:"
            //---------------------

                public Double OD_ULimit(clsJBearing Bearing_In)
                //============================================
                {
                    mCurrentBearing = Bearing_In;
                    double pD_CB_Max = ((clsJBearing)mCurrentBearing).RadB.EndPlateCB_DMax();
                    double pDESIGN_DCLEAR = ((clsJBearing)mCurrentBearing).RadB.DESIGN_DCLEAR;
                    return (pD_CB_Max - pDESIGN_DCLEAR);
                }

                public Double OD_LLimit(clsJBearing Bearing_In, int Indx_In)
                //=========================================================
                {
                    mCurrentBearing = Bearing_In;
                    double pScrew_Hole_DBC_Min = ((clsJBearing)mCurrentBearing).Mount[Indx_In].DBC_LLimit(mCurrentBearing);
                    double pHole_CBore_D = ((clsJBearing)mCurrentBearing).Mount[Indx_In].Screw.Hole.CBore.D;
                    return (pScrew_Hole_DBC_Min + 2 * modMain.gcSep_Min + pHole_CBore_D);
                }

                public Double DBore()
                //===================
                {
                    return modMain.Nom_Val(mDBore_Range);
                }

                public Double Clearance()
                //=======================              
                {
                    //....Diametral Clearance.
                    Double pClear;
                    pClear = DBore() - ((clsJBearing)mCurrentBearing).RadB.DShaft();
                    return pClear;
                }

            #endregion


            #region "NESTED CLASS:"

                #region "Class Seal":
                //--------------------
                    [Serializable]
                    public class clsSeal : ICloneable
                    {
                        #region "NAMED CONSTANTS:"
                        //=======================
                        //private const Double mc_DESIGN_LINING_THICK = 0.030F;  // PB 07OCT18. May not be needed
                        #endregion

                        #region "ENUMERATION TYPES:"
                        //==========================                           
                            [Serializable]
                            public enum eDesign { Fixed };
                        #endregion

                        #region "MEMBER VARIABLES:"
                        //=========================
                            private eDesign mDesign;
                            private clsBlade mBlade;
                            private clsDrainHoles mDrainHoles;

                            private clsEndPlate mCurrentEndPlate;
                            //private clsWireClipHoles mWireClipHoles;     
                            //private Double mTempSensor_D_ExitHole;   
                        #endregion


                        #region "PROPERTY ROUTINES:"
                        //=========================
                            //....Design:
                            public eDesign Design
                            {
                                get { return mDesign; }
                                set { mDesign = value; }
                            }

                            public clsBlade Blade
                            {
                                get { return mBlade; }
                                set { mBlade = value; }
                            }

                            public clsDrainHoles DrainHoles
                            {
                                get { return mDrainHoles; }
                                set { mDrainHoles = value; }
                            }

                            //#region "Wire Clip Holes:"

                            //    public clsWireClipHoles WireClipHoles
                            //    {
                            //        get { return mWireClipHoles; }
                            //        set { mWireClipHoles = value; }
                            //    }

                            //#endregion

                            //#region "Temp Sensor Exit Holes:"

                            //    public Double TempSensor_D_ExitHole
                            //    {
                            //        get { return mTempSensor_D_ExitHole; }
                            //        set { mTempSensor_D_ExitHole = value; }
                            //    }

                            //#endregion

                        #endregion


                        #region "CONSTRUCTOR:"

                            public clsSeal(clsUnit.eSystem UnitSystem_In, clsJBearing CurrentBearing_In, clsEndPlate CurrentEndPlate_In)
                            //==========================================================================================================
                            {
                                //....Default Values:      
                                mDesign = eDesign.Fixed;

                                //....Instantiate member object variables:
                                mCurrentEndPlate = CurrentEndPlate_In;
                                mBlade = new clsBlade();
                                mDrainHoles = new clsDrainHoles(mCurrentEndPlate, CurrentBearing_In);
                            }

                        #endregion


                        #region "CLASS METHODS:"
                        //======================

                            //#region "TEMP. SENSOR"

                            //    public Double TempSensor_DBC_Hole()
                            //    //=================================
                            //    {
                            //        //....Ref. Seal_Rev9_27OCT11: Col. BG                                               //Check RA Page: 34 
                            //        Double pDBC = ((clsBearing_Radial_FP)mCurrentProduct.Bearing).Bore() + (.08 * 2)  + 
                            //                      ((clsBearing_Radial_FP)mCurrentProduct.Bearing).TempSensor.D;
                            //        return pDBC;
                            //    }

                            //#endregion

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


                        #region "NESTED CLASSES:"

                            #region "Class Blade":
                            //--------------------

                                [Serializable]
                                public class clsBlade
                                //====================
                                {
                                    #region "NAMED CONSTANTS:"
                                    //=======================
                                        private const Double mc_DESIGN_BLADE_THICK = 0.060F;
                                    #endregion

                                    #region "MEMBER VARIABLES:"
                                    //=========================
                                        private int mCount;
                                        private Double mT;
                                        private Double mAngTaper;
                                    #endregion

                                    #region "CLASS PROPERTY ROUTINES:"
                                    //================================  

                                        public Double DESIGN_BLADE_THICK
                                        {
                                            get { return mc_DESIGN_BLADE_THICK; }
                                        }

                                        public int Count
                                        {
                                            get { return mCount; }
                                            set
                                            {
                                                int pCount = mCount;
                                                mCount = value;
                                            }
                                        }

                                        public Double T
                                        {
                                            get { return mT; }
                                            set { mT = value; }        //....When mCount = 1, this is Land L.       
                                        }

                                        public Double AngTaper
                                        {
                                            get { return mAngTaper; }
                                            set
                                            {
                                                if (mCount == 1)
                                                { mAngTaper = value; }
                                            }
                                        }

                                    #endregion

                                    #region "CONSTRUCTOR:"

                                        public clsBlade()
                                        //==================
                                        {
                                            mCount = 1;
                                            mT = mc_DESIGN_BLADE_THICK;
                                            mAngTaper = 45;
                                        }

                                    #endregion
                                }

                            #endregion

                            #region "Class DrainHoles":
                            //-------------------------
                                [Serializable]
                                public class clsDrainHoles
                                {
                                    //#region "NAMED CONSTANTS:"        // PB 24OCT18 Commented out
                                    ////=======================
                                    //    //....Drain Holes: Minimum seperation distance between the end & begin points 
                                    //    //........of two consecutive holes. 
                                    //    private const Double mc_DESIGN_DRAINHOLE_SEP_DIST = 0.030F;
                                    //#endregion

                                    #region "USER-DEFINED STRUCTURES:"
                                    //================================
                                        //....Annulus
                                        [Serializable]                                       
                                        public struct sAnnulus
                                        {
                                            public Double Ratio_L_H;
                                            public Double D;                 //....Derived/User Input
                                        }
                                    #endregion

                                    #region "MEMBER VARIABLES:"
                                    //=========================
                                        protected clsJBearing mCurrentBearing;       //PB 25OCT18, to be passed thru' clsSeal constructor
                                        private clsEndPlate mCurrentEndPlate;

                                        private sAnnulus mAnnulus;
                                        private String mD_Desig;
                                        //....D;                        //....Method.  

                                        private int mCount;     //....Usually it is calculated. However, when the drain holes array crosses the Bearing S/L, 
                                        //.......the count is to be programmatically increased by 1 per HK's (KMC) instruction (DEC12).
                                        //....V                 //....Method.         

                                        private Double mAngBet;
                                        private Double mAngStart_Horz;               //....Calculated but can be overridable.               
                                        //private Double mAngStart_OtherSide;
                                        private Double mAngExit;

                                    #endregion


                                    #region "CLASS PROPERTY ROUTINES:"
                                    //================================ 

                                        #region "ANNULUS:"
                                        //-----------------
                                            public sAnnulus Annulus
                                            {
                                                get
                                                {
                                                    if (Math.Abs(mAnnulus.D) < modMain.gcEPS)
                                                        mAnnulus.D = Calc_Annulus_D();
                                                    return mAnnulus;
                                                }
                                            }

                                            public Double Annulus_Ratio_L_H
                                            {
                                                set
                                                {
                                                    Double pPrevVal = mAnnulus.Ratio_L_H;
                                                    mAnnulus.Ratio_L_H = value;

                                                    if (Math.Abs(mAnnulus.Ratio_L_H - pPrevVal) > modMain.gcEPS)
                                                    {
                                                        //....The current Ratio is different from the previous value.
                                                        //........Recalculate the D.
                                                        mAnnulus.D = Calc_Annulus_D();
                                                    }
                                                }
                                            }

                                            public Double Annulus_D
                                            {
                                                set { mAnnulus.D = value; }
                                            }

                                        #endregion


                                        public String D_Desig
                                        {
                                            get { return mD_Desig; }

                                            set
                                            {
                                                mD_Desig = value;

                                                //....Recalculate: 
                                                mCount = Calc_Count();
                                                mAngStart_Horz = Calc_AngStart_Horz();
                                            }
                                        }

                                        public int Count
                                        {
                                            get
                                            {
                                                if (mCount < modMain.gcEPS)
                                                    mCount = Calc_Count();

                                                return mCount;
                                            }

                                            set { mCount = value; }
                                        }

                                        #region "ANGLES:"
                                        //---------------

                                            public Double AngBet
                                            {
                                                get { return mAngBet; }

                                                set
                                                {
                                                    mAngBet = value;
                                                    mCount = Calc_Count();              //....Reset, in case it has been increased by 1 earlier. //PB 25OCT18. May need to be suppressed
                                                    mAngStart_Horz = Calc_AngStart_Horz();        //....Recalculate.   
                                                }
                                            }


                                            public Double AngStart_Horz
                                            {
                                                get
                                                {
                                                    if (mAngStart_Horz < modMain.gcEPS)
                                                        mAngStart_Horz = Calc_AngStart_Horz();        //....Used only for the very first case.

                                                    return mAngStart_Horz;
                                                }

                                                set { mAngStart_Horz = value; }
                                            }


                                            //public Double AngStart_OtherSide                 //PB 28JAN13. 
                                            //{
                                            //    get {   if (mAngStart_OtherSide < modMain.gcEPS)
                                            //                mAngStart_OtherSide = Calc_AngStart_OtherSide();

                                            //              return mAngStart_OtherSide; }

                                            //    set { mAngStart_OtherSide = value; }
                                            //}


                                            public Double AngExit
                                            {
                                                get { return mAngExit; }
                                                set { mAngExit = value; }
                                            }

                                        #endregion

                                    #endregion


                                    #region "CONSTRUCTOR:"

                                        public clsDrainHoles(clsEndPlate CurrentEndPlate_In, clsJBearing CurrentBearing_In)
                                        //=================================================================================
                                        {
                                            mCurrentBearing = CurrentBearing_In;
                                            mCurrentEndPlate = CurrentEndPlate_In;

                                            //....Initialize: Default Values.
                                            mAnnulus.Ratio_L_H = 2.0F;
                                            mAngExit = 45.0F;
                                        }

                                    #endregion

                                    #region "CLASS METHODS":

                                        public Double Calc_Annulus_Ratio_L_H()
                                        //=====================================          
                                        {
                                            Double pAnnulus_L = mCurrentEndPlate.L - (mCurrentEndPlate.Seal.Blade.Count * mCurrentEndPlate.Seal.Blade.T);
                                            Double pH = 0.5 * (mAnnulus.D - mCurrentEndPlate.DBore());

                                            Double pAnnulus_L_H;
                                            pAnnulus_L_H = pAnnulus_L / pH;

                                            return pAnnulus_L_H;
                                        }

                                        public Double Calc_Annulus_D()
                                        //=============================
                                        {
                                            Double pAnnulus_L;
                                            pAnnulus_L = mCurrentEndPlate.L - (mCurrentEndPlate.Seal.Blade.Count * mCurrentEndPlate.Seal.Blade.T);

                                            Double pH = 0.0F;
                                            if (mAnnulus.Ratio_L_H != 0.0F)
                                                pH = pAnnulus_L / mAnnulus.Ratio_L_H;

                                            Double pD;
                                            pD = mCurrentEndPlate.DBore() + 2 * pH;

                                            return pD;
                                        }

                                        public Double D()
                                        //=================
                                        {
                                            if (mD_Desig != null && mD_Desig != "")
                                            {
                                                return modMain.DVal(mD_Desig);
                                            }
                                            else
                                                return 0;
                                        }

                                        public Int32 Calc_Count()
                                        //=======================                          
                                        {   //....Ref.: Sizing & Qty. of Drain Holes 
                                            //........Depends on FlowReqd_gpm & Drain hole D.

                                            //....Flow reqd. GPM.
                                            Double pFlowReqd_gpm = mCurrentBearing.PerformData.FlowReqd;

                                            Int32 pCount = 0;

                                            Double pATot_Reqd;
                                            pATot_Reqd = (231 * pFlowReqd_gpm * 0.5) / (60 * 12 * 2);

                                            Double pA_Hole;
                                            pA_Hole = 0.25 * Math.PI * Math.Pow(D(), 2);

                                            if (pA_Hole != 0.0F)
                                                pCount = (Int16)Math.Ceiling(pATot_Reqd / pA_Hole);

                                            return pCount;
                                        }

                                        public Double V()
                                        //=================                                 
                                        {
                                            Double pFlowReqd_gpm = mCurrentBearing.PerformData.FlowReqd;

                                            Double pA_Hole;
                                            pA_Hole = 0.25 * Math.PI * Math.Pow(D(), 2);

                                            Double pATot;
                                            pATot = mCount * pA_Hole;     //....Use always the calculated value of the # of holes.                    
                                            //........Don't use the augmented count if the drain holes array crosses
                                            //........Bearing S/L, as the extra hole will be deleted by the user later.
                                            Double pV = 0.0F;
                                            if (pATot != 0.0F)
                                                pV = (0.5F * pFlowReqd_gpm * 231) / (60 * 12 * pATot);

                                            return pV;
                                        }


                                        public Double AngBet_LLim()
                                        //==========================               
                                        {
                                            //....This value depends on the D & Annulus_D.
                                            //
                                            Double pAng_Bet_LLim = 0.0F;

                                            if (D() != 0.0F)
                                            {
                                                Double pAnnulusR = 0.5 * mAnnulus.D;
                                                Double pS = D() + modMain.gcSep_Min;             //....Arc length between the centers of two consecutive holes.

                                                Double pAngBet_Rad = 0.0F;

                                                if (pAnnulusR != 0.0F)
                                                    pAngBet_Rad = (pS / pAnnulusR);                         //....Rad.

                                                pAng_Bet_LLim = (pAngBet_Rad * (180.0F / Math.PI));         //....Deg.. 
                                            }

                                            return pAng_Bet_LLim;
                                        }


                                        public Double Calc_AngStart_Horz()
                                        //================================                                  
                                        {
                                            Double pAngStart = 0;
                                            //....This calculation is based on the assumption of the symmetry of the drain holes array about
                                            //........the Casing S/L vertical. 
                                            //
                                            //........This depends on the AngBet, D_Desig (which in turns triggers Calc_Count) & Anti-Rotation Pin Ang. 


                                            ////Store relevant parameters from Bearing_Radial_FP object in local variables.
                                            ////---------------------------------------------------------------------------
                                            ////
                                            ////....Anti Rotation Pin Location w.r.t. Bearing Datums:
                                            //clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL pAntiRotPinLoc_BS =
                                            //                        ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Bearing_SL;
                                            //clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert pAntiRotPinLoc_BV =
                                            //                        ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Bearing_Vert;

                                            ////....Anti Rotation Pin Angle w.r.t Bearing S.L. 
                                            //Double pAntiRot_Pin_Loc_Ang = ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Angle;

                                            //Determine Angle Start w.r.t Casing S/L:
                                            //-----------------------------------------
                                            //
                                            //...w.r.t Casing Vertical.
                                            //........Valid for either even or odd value of the "Count".
                                            Double pAngStart_Casing_Vert = (0.5 * (mCount - 1)) * mAngBet;

                                            //...w.r.t Casing SplitLine.
                                            Double pAngStart_Casing_SL = 90 - pAngStart_Casing_Vert;

                                            //Convert Angle Start w.r.t Horizontal:
                                            //-------------------------------------
                                            // PB 24OCT18. BG, Bring this method from clsARP ==> Ang_Casing_SL_Horz () + pAngStart_Casing_SL
                                            pAngStart = mCurrentBearing.RadB.ARP.Ang_Casing_SL_Horz() + pAngStart_Casing_SL;

                                            return pAngStart;
                                        }


                                        //public Double Calc_AngStart_OtherSide()
                                        ////=====================================  
                                        //{
                                        //    //...w.r.t Bearing S/L.
                                        //    Double pAng;
                                        //    pAng = 180 - (mAngStart + ((mCount - 1) * mAngBet));

                                        //    return pAng;
                                        //}


                                        public Double AngStart_OtherSide()       
                                        //=================================  
                                        {
                                            //...w.r.t Bearing S/L.
                                            Double pAng;
                                            pAng = 180 - (mAngStart_Horz + ((mCount - 1) * mAngBet));

                                            return pAng;
                                        }
                                        // PB 24OCT18. BG, suppress all ULim calculations. On the form, we will not display ULim now. It is not required now, may be later in the future. Just keep blank. Just check if
                                        //the AngBet is >= than LLim. No need to check <= Ulim


                                        public bool Sym_CasingSL_Vert()
                                        //=============================
                                        {
                                            if (Math.Abs(mAngStart_Horz - Calc_AngStart_Horz()) <= modMain.gcEPS)
                                            {
                                                return true;
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }


                                        public Double AngBet_ULim_Sym()
                                        //============================                     
                                        {
                                            Double pAng_Bet_ULim = 0.0F;

                                            ////....This routine is valid when the drain holes are symmetric about the Casing S/L vertical 
                                            ////........when Ang_Start is a dependent parameter.

                                            ////Store relevant parameters from Bearing_Radial_FP object in local variables.
                                            ////---------------------------------------------------------------------------
                                            ////
                                            ////....Anti Rotation Pin Location w.r.t. Bearing Datums:
                                            //clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL pPinLoc_BS =
                                            //                        ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Bearing_SL;

                                            //clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert pPinLoc_BV =
                                            //                        ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Bearing_Vert;

                                            ////....Anti Rotation Pin Angle w.r.t Bearing S.L. 
                                            //Double pPinLoc_Ang = ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Angle;


                                            ////....Drain hole Angle upper limit in degree.
                                            //Double pAng_Bet_ULim = 0.0F;

                                            //int pCount = Calc_Count();

                                            //if (pCount > 1)
                                            //{
                                            //    if ((pPinLoc_BS == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Top &&
                                            //            pPinLoc_BV == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.L)
                                            //            ||
                                            //        (pPinLoc_BS == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Bottom &&
                                            //            pPinLoc_BV == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.R))
                                            //        //------------------------------------------------------------------------------
                                            //        pAng_Bet_ULim = (2 * (90 - pPinLoc_Ang) / (pCount - 1));


                                            //    else if ((pPinLoc_BS == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Bottom &&
                                            //            pPinLoc_BV == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.L)
                                            //            ||
                                            //        (pPinLoc_BS == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Top &&
                                            //            pPinLoc_BV == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.R))
                                            //        //------------------------------------------------------------------------------
                                            //        pAng_Bet_ULim = (2 * (90 + pPinLoc_Ang)) / (pCount - 1);
                                            //}

                                            return pAng_Bet_ULim;
                                        }


                                        public Double AngBet_ULim_NonSym()
                                        //================================                     
                                        {
                                            Double pAng_Bet_ULim = 0.0F;
                                            //....This routine is used when Ang_Start is a user input and the drain holes will no longer be 
                                            //.........symmetric about the Casing S/L vertical 

                                            //....This value depends on AngStart_Front & Count. This calculation loses its relevance
                                            //........when the drain holes array crosses the Bearing S/L and hence, it is not used then.

                                            ////....Drain hole Angle upper limit in degree.
                                            //Double pAng_Bet_ULim = 0.0F;                            
                                            //pAng_Bet_ULim        = (180 - mAngStart_Horz) / (Calc_Count () - 1);

                                            return pAng_Bet_ULim;
                                        }

                                        public void UpdateCurrentSeal(clsJBearing CurrentBearing_In)
                                        //=========================================================
                                        {
                                            mCurrentBearing = CurrentBearing_In;
                                        }

                                    #endregion
                                }

                            #endregion

                        #endregion
                    }

                #endregion

                #region "Class TLTB":

                    [Serializable]
                    public class clsTLTB : ICloneable
                    {
                        #region "NAMED CONSTANTS:"
                        //========================
                            private const Double mc_LINING_T = 0.0F;//0.030F;
                            private const Double mc_LAND_L = 0.060F;

                            private const Double mc_FACEOFF_ASSY_DEF = 0.010;           //BG 04APR13

                            private const Double mc_BACK_RELIEF_DEPTH = 0.020;
                            private const Double mc_BACK_RELIEF_FILLET = 0.010;

                            private int mPAD_COUNT = 6;

                        #endregion


                        #region " ENUMERATION TYPES:"
                        //===========================
                            //public enum eDirectionType { Uni, Bi, Bumper };
                            public enum eDirectionType { Uni, Bi };
                            public enum eRotation { CCW, CW };

                        #endregion


                        #region "USER-DEFINED STRUCTURES:"
                        //===============================    

                            [Serializable]
                            public struct sTaper
                            {
                                public Double Depth_ID;
                                public Double Depth_OD;
                                public Double Angle;
                            }

                            [Serializable]
                            public struct sShroud
                            {
                                public Double Ri;
                                public Double Ro;
                                public Double OD;
                                public Double ID;
                            }

                            [Serializable]
                            public struct sLiningT          //.... Lining Thickness.
                            {
                                public Double Face;
                                public Double ID;
                            }

                            [Serializable]
                            public struct sBackRelief          //.... Back Relief.
                            {
                                public Boolean Reqd;
                                public Double D;
                                public Double Depth;
                                public Double Fillet;
                            }

                        #endregion


                        #region "MEMBER VARIABLE DECLARATIONS:"
                        //=====================================
                            private Boolean mExists;

                            private eDirectionType mDirectionType;

                            //....Pad:
                            private int mPad_Count;
                            private Double[] mPadD;                                     // ID: 0  & OD: 1.

                            //....Bore.
                            private Double mLandL;

                            //....Materials:
                            private sLiningT mLiningT;

                            private sTaper mTaper;
                            private sShroud mShroud;

                            //....Back Relief
                            private sBackRelief mBackRelief;

                            //....Miscellaneous
                            private Double mLFlange;
                            private Double mFaceOff_Assy;

                        #endregion


                        #region "Member Class Objects:"
                            private clsFeedGroove mFeedGroove;
                            private clsWeepSlot mWeepSlot;
                            private clsPerformData mPerformData;
                        #endregion


                        #region "CLASS PROPERTY ROUTINES:"
                        //===============================

                            public Boolean Exists
                            {
                                get { return mExists; }
                                set { mExists = value; }
                            }

                            public Double LINING_T
                            {
                                get { return mc_LINING_T; }
                            }

                            public Double LAND_L
                            {
                                get { return mc_LAND_L; }
                            }

                            public Double FACEOFF_ASSY_DEF
                            {
                                get { return mc_FACEOFF_ASSY_DEF; }
                            }


                            public Double BACK_RELIEF_DEPTH
                            {
                                get { return mc_BACK_RELIEF_DEPTH; }
                            }

                            public Double BACK_RELIEF_FILLET
                            {
                                get { return mc_BACK_RELIEF_FILLET; }
                            }


                            public eDirectionType DirectionType
                            {
                                get { return mDirectionType; }
                                set { mDirectionType = value; }
                            }


                        #region "Pad:"

                            public int Pad_Count
                            //==================
                            {
                                get
                                {
                                    if (mPad_Count < modMain.gcEPS)
                                        return mPAD_COUNT;
                                    else
                                        return mPad_Count;
                                }
                                set { mPad_Count = value; }
                            }


                            public Double[] PadD
                            //===================
                            {
                                get { return mPadD; }
                                set { mPadD = value; }
                            }

                        #endregion

                        #region "Bore:"

                            public Double LandL
                            //==================
                            {
                                get
                                {
                                    if (mLandL < modMain.gcEPS)
                                        return mc_LAND_L;
                                    else
                                        return mLandL;
                                }
                                set { mLandL = value; }
                            }

                        #endregion
                        
                        #region "Face & ID Lining Thicknesses:"

                            public sLiningT LiningT
                            {
                                get
                                {
                                    mLiningT.Face = mc_LINING_T;
                                    mLiningT.ID = mc_LINING_T;

                                    //if (mLiningT.Face < modMain.gcEPS)
                                    //    mLiningT.Face = mc_LINING_T;

                                    //if (mLiningT.ID < modMain.gcEPS)
                                    //    mLiningT.ID = mc_LINING_T;

                                    return mLiningT;
                                }
                            }


                            public Double LiningT_Face
                            {
                                set { mLiningT.Face = value; }
                            }

                            public Double LiningT_ID
                            {
                                set { mLiningT.ID = value; }
                            }

                        #endregion
                        
                        #region "Taper:"

                            public sTaper Taper
                            {
                                get { return mTaper; }
                            }

                            public Double Taper_Depth_OD
                            {
                                set { mTaper.Depth_OD = value; }
                            }

                            public Double Taper_Depth_ID
                            {
                                set { mTaper.Depth_ID = value; }
                            }

                            public Double Taper_Angle
                            {
                                set { mTaper.Angle = value; }
                            }

                        #endregion
                        
                        #region "Shroud:"

                            public sShroud Shroud
                            {
                                get
                                {
                                    if (mShroud.Ri < modMain.gcEPS || mShroud.Ri < 0.5 * mPadD[0])
                                        mShroud.Ri = 0.5 * mPadD[0];

                                    return mShroud;
                                }
                            }

                            public Double Shroud_Ri
                            {
                                set
                                {
                                    mShroud.Ri = value;
                                    //if (value >= 0.5 * mPadD[0])
                                    //    mShroud.Ri = value;
                                    //else
                                    //    mShroud.Ri = 0.5 * mPadD[0];
                                }
                            }

                            public Double Shroud_Ro
                            {
                                set { mShroud.Ro = value; }
                            }

                            public Double Shroud_OD
                            {
                                set { mShroud.OD = value; }
                            }

                            public Double Shroud_ID
                            {
                                set { mShroud.ID = value; }
                            }

                        #endregion
                        
                        #region "Back Relief:"

                            public sBackRelief BackRelief
                            {
                                get
                                {
                                    if (mBackRelief.Depth < modMain.gcEPS)
                                        mBackRelief.Depth = mc_BACK_RELIEF_DEPTH;

                                    return mBackRelief;
                                }
                            }


                            public Boolean BackRelief_Reqd
                            {
                                set { mBackRelief.Reqd = value; }
                            }

                            public Double BackRelief_D
                            {
                                set { mBackRelief.D = value; }
                            }

                            public Double BackRelief_Depth
                            {
                                set { mBackRelief.Depth = value; }
                            }

                            public Double BackRelief_Fillet
                            {
                                set { mBackRelief.Fillet = value; }
                            }

                        #endregion
                        
                        #region "Miscellaneous:"

                            public Double LFlange
                            {
                                get { return mLFlange; }
                                set { mLFlange = value; }
                            }


                            public Double FaceOff_Assy
                            {
                                get { return mFaceOff_Assy; }
                                set { mFaceOff_Assy = value; }
                            }

                        #endregion
                        
                        #region "Feed Groove:"

                            public clsFeedGroove FeedGroove
                            {
                                get { return mFeedGroove; }
                                set { mFeedGroove = value; }
                            }

                        #endregion
                        
                        #region "Weep Slot:"

                            public clsWeepSlot WeepSlot
                            {
                                get { return mWeepSlot; }
                                set { mWeepSlot = value; }
                            }

                        #endregion
                        
                        #region "Perform Data:"

                            public clsPerformData PerformData
                            {
                                get { return mPerformData; }
                                set { mPerformData = value; }
                            }

                        #endregion

                        #endregion


                        #region "CLASS CONSTRUCTOR:"

                            public clsTLTB(clsUnit.eSystem UnitSystem_In, clsJBearing CurrentBearing_In, clsEndPlate CurrentEndPlate_In)
                            //=========================================================================
                            {
                                mPadD = new Double[2];

                                mFeedGroove = new clsFeedGroove(CurrentBearing_In, this);
                                mWeepSlot = new clsWeepSlot();
                                mPerformData = new clsPerformData();

                                ////mT1 = new clsEndMill();
                                ////mT2 = new clsEndMill();
                                ////mT3 = new clsEndMill();
                                ////mT4 = new clsEndMill();

                                ////mT2.Type = clsEndMill.eType.Flat;
                                ////mT4.Type = clsEndMill.eType.Chamfer;

                                ////mOverlap_frac = mc_DESIGN_OVERLAP_FRAC;
                                ////mFeedRate.Taperland = mc_FEED_RATE_TL_DEF;
                                ////mDepth_TL_BackLash = mc_DEPTH_TL_BACKLASH;
                                ////mDepth_TL_Dwell_T = mc_DEPTH_TL_DWELL_T;
                                ////mFeedRate.WeepSlot = mc_FEED_RATE_WEEPSLOT_DEF;
                                ////mDepth_WS_Cut_Per_Pass = mc_DEPTH_WS_CUT_PER_PASS_DEF;

                                //Mat.Base = "Steel 4340";
                                //Mat.LiningExists = false;
                                //Mat.Lining = "Babbitt";
                            }

                        #endregion


                        #region "CLASS METHODS:"

                            #region "REF. / DEPENDENT VARIABLES:"

                            public Double DimStart()
                            //------------------------
                            {
                                //....Ref. TL Thrust Bearing_Rev1_27OCT11: Col. AA
                                if (mBackRelief.Reqd)
                                    return (0.15 - mLandL);
                                else
                                    return (0.15 + mLandL);
                            }


                            public Double MountHoles_Depth_Tap_Drill()
                            //=========================================
                            {
                                return 0.0;
                                //if ((MountHoles.Type == clsMountHoles.eMountHolesType.T && MountHoles.Thread_Thru)
                                //|| (MountHoles.Type == clsMountHoles.eMountHolesType.T))
                                //{
                                //    return mLFlange;
                                //}
                                //else
                                //    return (MountHoles.Depth_Thread + 0.0625);
                            }


                            public Double Validate_Shroud_Ri(ref Double Shroud_Ri)
                            //====================================================
                            {
                                string pMsg = "Shroud ID should not be less than Pad ID.";

                                if (Shroud_Ri < 0.5 * mPadD[0])
                                {
                                    MessageBox.Show(pMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Shroud_Ri = 0.5 * mPadD[0];
                                }

                                return Shroud_Ri;
                            }
                        
                        #endregion

                        #endregion


                        #region "ICLONEABLE MEMBERS:"
                        //==========================

                            public object Clone()
                            //===================
                            {

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


                        #region "NESTED CLASSES:"

                            #region "Class FeedGroove":
                            [Serializable]

                                public class clsFeedGroove
                                //=========================
                                {
                                    #region "NAMED CONSTANTS:"
                                    //========================
                                        private const Double mc_DIST_CHAMF = 0.030F;

                                    #endregion

                                    #region "MEMBER VARIABLES:"
                                    //=========================
                                        private clsJBearing mCurrentBearing;
                                        private clsTLTB mCurrentTLTB;

                                        private String mType;
                                        private Double mWid;
                                        private Double mDepth;
                                        private Double mDBC;
                                        private Double mDist_Chamf;

                                    #endregion


                                    #region "CLASS PROPERTY ROUTINES:"
                                    //================================  

                                        public Double DIST_CHAMF
                                        {
                                            get { return mc_DIST_CHAMF; }
                                        }


                                        public String Type
                                        {
                                            get { return mType; }
                                            set { mType = value; }
                                        }

                                        public Double Wid
                                        {
                                            get { return mWid; }
                                            set { mWid = value; }
                                        }

                                        public Double Depth
                                        {
                                            get
                                            {
                                                if (mDepth < modMain.gcEPS)
                                                    mDepth = (2 * mCurrentTLTB.WeepSlot.Depth);
                                                return mDepth;
                                            }
                                            set { mDepth = value; }
                                        }

                                        public Double DBC
                                        {
                                            get
                                            {
                                                if (mDBC < modMain.gcEPS)
                                                    mDBC = Calc_DBC();

                                                return mDBC;
                                            }

                                            set { mDBC = value; }
                                        }

                                        public Double Dist_Chamf
                                        {
                                            get
                                            {
                                                if (mDist_Chamf < modMain.gcEPS)
                                                    return mc_DIST_CHAMF;
                                                else
                                                    return mDist_Chamf;
                                            }
                                            set { mDist_Chamf = value; }
                                        }

                                    #endregion


                                    #region "CONSTRUCTOR:"

                                        public clsFeedGroove(clsJBearing Current_Bearing_In, clsTLTB CurrentTLTB_In)
                                        //=================================================
                                        {
                                            mCurrentBearing = Current_Bearing_In;
                                            mCurrentTLTB = CurrentTLTB_In;
                                        }
                                    #endregion


                                    #region "CLASS METHODS:"

                                        #region "REF. / DEPENDENT VARIABLES:"

                                            private Double Calc_DBC()
                                            //=======================
                                            {
                                                //....Ref. TL Thrust Bearing_Rev1_27OCT11: Col. AY
                                                return modMain.MRound(1.95 * mCurrentTLTB.Shroud.Ri, 0.01);
                                            }

                                        #endregion

                                    #endregion

                                }

                            #endregion

                            #region "Class WeepSlot":
                            [Serializable]

                                public class clsWeepSlot
                                //========================
                                {
                                    public enum eType { Rectangular, Circular, V_notch };


                                    #region "MEMBER VARIABLES:"
                                    //=========================

                                        private eType mType;
                                        private Double mWid;
                                        private Double mDepth;

                                    #endregion


                                    #region "CLASS PROPERTY ROUTINES:"
                                    //================================  

                                        public eType Type
                                        {
                                            get { return mType; }
                                            set { mType = value; }
                                        }

                                        public Double Wid
                                        {
                                            get { return mWid; }
                                            set { mWid = value; }
                                        }

                                        public Double Depth
                                        {
                                            get { return mDepth; }
                                            set { mDepth = value; }
                                        }

                                    #endregion


                                    #region "CONSTRUCTOR:"

                                        public clsWeepSlot()
                                        {
                                        }

                                    #endregion

                                }

                            #endregion

                            #region "Class Performance Data":
                            [Serializable]

                                public class clsPerformData
                                //=========================
                                {
                                    #region "USER-DEFINED STRUCTURES:"
                                    //================================
                                        [Serializable]
                                        public struct sPadMax
                                        {
                                            public Double Temp;
                                            public Double Press;
                                            //public Double Load;        //....This property has been suppressed as as per HK's instruction on 09JAN13

                                        }

                                    #endregion


                                    #region "MEMBER VARIABLES:"
                                    //=========================

                                        private Double mPower_HP;
                                        private Double mFlowReqd_gpm;
                                        private Double mTempRise_F;
                                        private Double mTFilm_Min;
                                        public Double mUnitLoad;

                                        //....Pad Maximums:
                                        private sPadMax mPadMax;

                                    #endregion


                                    #region "CLASS PROPERTY ROUTINES:"
                                    //================================

                                        //.... Power (Eng Unit).
                                        public Double Power_HP
                                        {
                                            get { return mPower_HP; }
                                            set
                                            {
                                                mPower_HP = value;
                                                mTempRise_F = Calc_TempRise_F();
                                            }
                                        }


                                        //.... Flow Reqd (Eng Unit).
                                        public Double FlowReqd_gpm
                                        {
                                            get { return mFlowReqd_gpm; }
                                            set
                                            {
                                                mFlowReqd_gpm = value;
                                                mTempRise_F = Calc_TempRise_F();
                                            }
                                        }


                                        //.... Temp Rise (Eng Unit).
                                        public Double TempRise_F
                                        {
                                            get
                                            {
                                                if (Math.Abs(mTempRise_F) < modMain.gcEPS)
                                                    mTempRise_F = Calc_TempRise_F();

                                                return mTempRise_F;
                                            }

                                            set { mTempRise_F = value; }
                                        }


                                        //.... TFilm_Min (Eng Unit).
                                        public Double TFilm_Min
                                        {
                                            get { return mTFilm_Min; }
                                            set { mTFilm_Min = value; }
                                        }


                                        //.... UnitLoad
                                        public Double UnitLoad           //....This property has been added as as per HK's instruction on 09JAN13
                                        {
                                            get { return mUnitLoad; }
                                            set { mUnitLoad = value; }
                                        }


                                        //....Pad Maximums:
                                        public sPadMax PadMax
                                        {
                                            get { return mPadMax; }
                                        }


                                        public Double PadMax_Temp
                                        {
                                            set { mPadMax.Temp = value; }
                                        }


                                        public Double PadMax_Press
                                        {
                                            set { mPadMax.Press = value; }
                                        }

                                        //public Double PadMax_Load
                                        //{
                                        //    set { mPadMax.Load = value; }
                                        //}


                                    #endregion


                                    #region "CONSTRUCTOR:"

                                        public clsPerformData()
                                        //=====================
                                        {

                                        }

                                    #endregion

                                    #region "CLASS METHODS:"

                                        public Double Calc_TempRise_F()
                                        //=============================
                                        {
                                            Double pTempRise = 0.0;

                                            if (mFlowReqd_gpm != 0.0)        //BG 30JAN13
                                            {
                                                pTempRise = 12.4 * (mPower_HP / mFlowReqd_gpm);
                                            }

                                            return pTempRise;
                                        }

                                    #endregion

                                }

                            #endregion

                        #endregion
                    }
                #endregion

                #endregion

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

