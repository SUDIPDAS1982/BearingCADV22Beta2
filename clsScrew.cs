
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsScrew                               '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  20DEC18                                '
//                                                                              '
//===============================================================================
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.OleDb;

namespace BearingCAD22
{
    [Serializable]                      
    public class clsScrew : ICloneable
    {
        #region "USER-DEFINED STRUCTURES:"
        //================================

            [Serializable]
            public struct sSpec
            {
                public clsUnit Unit;
                public string Type;                   
            
                public string Mat;                    
                public string D_Desig;  
                public Double Pitch;                  
                public Double L;                     
            }

            [Serializable]
            public struct sHole
            {   
                public Double D_Drill;
                public sCBore CBore;        
                public sDepth Depth;
            }
 
            [Serializable]
            public struct sCBore
            {   
                public Boolean Exists;
                public Double D;
                public Double Depth;
            }

            [Serializable]
            public struct sDepth
            {
                public Boolean Exists;
                public Double TapDrill;
                public Double Tap;                
                public Double Min_Engagement;
                public Double Engagement;
            }

        #endregion

        # region "MEMBER VARIABLE DECLARATIONS:"
        //======================================
            public sSpec mSpec;
            public sHole mHole;
            public string mPN;
            private double mHead_H;          

        #endregion


        #region "PROPERTY ROUTINES:"
        //=========================

            public sSpec Spec
            {
                get {return mSpec;}      
            }

            public sHole Hole
            {
                get {return mHole;}
            }

            public string PN
            {
                get { return mPN; }
                set { mPN = value; }
            }

            public double Head_H
            {
                get { return mHead_H; }
                set { mHead_H = value; }
            }
         
            public clsUnit Spec_Unit
            {
                set { mSpec.Unit = value; }
            }

            public string Spec_Type
            {
                set { mSpec.Type = value; }
            }

            public string Spec_Mat
            {
                set { mSpec.Mat = value; }
            }
           

            public string Spec_D_Desig   
            {   
                set  
                { 
                    mSpec.D_Desig = value;
                }
            }

            public Double Spec_Pitch     
            {
                //....Unit dependent - /in or /mm.
                set { mSpec.Pitch = value; }
            }


            public Double Spec_L
            {   
                //....Unit dependent - in or mm.
                set 
                { 
                    mSpec.L = value;
                }
            }


            //// Diameters
            //// ---------

            //public Double Head_H                       
            //{
            //    //....Unit independent - if Unit = "Metric", has been converted into "in".
            //    get 
            //    {
            //        if(mHead_H < modMain.gcEPS)
            //            Retrieve_Head_Params();

            //        return mHead_H;
            //    } 
            //}


            //public Double Head_D                   
            //{
            //    //....Unit independent - if Unit = "Metric", has been converted into "in".
            //    get 
            //    {
            //        if (mHead_D < modMain.gcEPS)
            //            Retrieve_Head_Params();

            //        return mHead_D; 
            //    }
            //}
          

            public Double Hole_CBore_D
            {
                set { mHole.CBore.D = value; }
            }

            public Double Hole_D_Drill
            {
                set { mHole.D_Drill = value; }
            }

            public Double Hole_CBore_Depth
            {
                set 
                { 
                    mHole.CBore.Depth = value;
                }
            }

            public Boolean Hole_Depth_Exists
            {
                set { mHole.Depth.Exists = value; }
            }

            public Double Hole_Depth_TapDrill
            {
                set { mHole.Depth.TapDrill = value; }
            }

            public Double Hole_Depth_Tap
            {
                set { mHole.Depth.Tap = value; }
            }

            public Double Hole_Depth_Min_Engagement
            {
                set { mHole.Depth.Min_Engagement = value; }
            }

            public Double Hole_Depth_Engagement
            {
                set { mHole.Depth.Engagement = value; }
            }

        #endregion


        //....Class Constructor
         public clsScrew (clsUnit.eSystem UnitSystem_In)           
        //==============================================
        {
            mSpec.Unit = new clsUnit();
            mSpec.Unit.System = UnitSystem_In;  
        }


        # region "CLASS METHODS:"
        //=======================


            public void GetPitch(ComboBox CmbBox_In, string DiaDesig_In, string Type_In, string UnitSystem = "")
            //===================================================================================================
            {
                //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                //string pFIELDS = "Select Distinct Pitch";
                //string pWHERE = " WHERE Type = '" + Type_In + "' and Unit = '" + pUnit + "' and D_Desig = '" + DiaDesig_In + "'";
                //string pSQL = pFIELDS + pWHERE;
              

                String pMat = "STEEL";
                String pUnit = mSpec.Unit.System.ToString().Substring(0, 1);     //..."E" or "M".

                if (UnitSystem != "")
                {
                    pUnit = UnitSystem.Substring(0, 1);     //..."E" or "M".
                }
                //....EXCEL File: StdPartsData
                string pWHERE = " WHERE Type = '" + Type_In + "' and D_Desig = '" + DiaDesig_In + "' and Mat = '" + pMat + "'";
               
                string pSheetName = "";
                if (pUnit == "E")
                {
                    pSheetName = "[Screw_English$]"; 
                }
                else
                {
                    pSheetName = "[Screw_Metric$]";
                }

                int pPitch_RecCount = modMain.gDB.PopulateCmbBox(CmbBox_In, modMain.gFiles.FileTitle_EXCEL_StdPartsData, pSheetName, "Pitch", pWHERE, true);

                if (pPitch_RecCount > 0)
                {
                    CmbBox_In.SelectedIndex = 0;
                }
            }
                   
            #region "Reset Dependent Member Variables:"

            //private void Reset_D_Params()
            ////===========================
            //{
            //    mSpec.D = 0.0F;
            //    //mD_Drill = 0.0F;
            //    //mD_Thru = 0.0F;
            //    //mD_CBore = 0.0F;
            //}

                //private void Reset_Head_Params()
                ////==============================
                //{
                //    mHead_D = 0.0F;
                //    mHead_H = 0.0F;
                //}

            #endregion


            #region "Retrieve Dependent Member Variables:"

                public Double D()       
                //=================
                {
                    Double pD = 0.0;
                    double pNumerator = 0.0;
                    double pDenominator = 0.0;

                    if (mSpec.D_Desig == "" || mSpec.D_Desig == null) return pD;

                    if (mSpec.Unit.System == clsUnit.eSystem.English)
                    {
                        if (mSpec.D_Desig.Contains("/"))
                        {
                            pNumerator = Convert.ToInt32(modMain.ExtractPreData(mSpec.D_Desig, "/"));
                            pDenominator = Convert.ToInt32(modMain.ExtractPostData(mSpec.D_Desig, "/"));
                            pD = Convert.ToDouble(pNumerator / pDenominator);
                        }
                        else if (mSpec.D_Desig.Contains("#"))
                        {
                            OleDbDataReader pobjDR = null;
                            OleDbConnection pConnection = null;
                            string pUnit = mSpec.Unit.System.ToString().Substring(0, 1);

                            string pstrFIELDS = "Select D";
                            string pstrFROM = " FROM [Screw_D$]";                          
                            string pstrWHERE = " WHERE  Unit = '" + pUnit + "' and D_Desig = '" + mSpec.D_Desig + "'";                                                  

                            string pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE;
                            pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                            if (pobjDR.Read())
                            {
                                int pColFldName = 0;                           

                                pColFldName = pobjDR.GetOrdinal("D");
                                if (pobjDR.IsDBNull(pColFldName) == false)
                                {
                                    pD = modMain.ConvTextToDouble(pobjDR["D"].ToString());                                    
                                }
                            }
                            pobjDR.Dispose();
                            pConnection.Close();
                        }
                        else
                        {
                            pD = Convert.ToDouble(mSpec.D_Desig);
                        }
                    }          
                    else if (mSpec.Unit.System == clsUnit.eSystem.Metric && mSpec.D_Desig.Contains("M"))
                    {
                       pD = Convert.ToDouble(mSpec.D_Desig.Remove(0, 1));
                    }                  

                    if (mSpec.Unit.System == clsUnit.eSystem.Metric)
                        pD = pD / 25.4;     //....1 in = 25.4 mm.
                    
                    return pD;
                }


                //private void Retrieve_Head_Params()
                ////=================================
                //{
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    //if (mType == null || mD_Desig == null) return;
                    //var pProject = (from pRec in pBearingDBEntities.tblManf_Screw_Head where pRec.fldD_Desig == mD_Desig && pRec.fldType == mType select (new { pRec.fldHead_H, pRec.fldHead_D })).ToList();

                    //if (pProject.Count() > 0)
                    //{
                    //    //....Unit dependent - in or mm. 
                    //    Double pHead_H = mDB.CheckDBDouble(pProject[0].fldHead_H);
                    //    Double pHead_D = mDB.CheckDBDouble(pProject[0].fldHead_D);

                    //    if (mUnit.System == clsUnit.eSystem.Metric)
                    //    {
                    //        mHead_H = pHead_H / 25.4;
                    //        mHead_D = pHead_D / 25.4;
                    //    }
                    //    else                //....English Unit.
                    //    {
                    //        mHead_H = pHead_H;
                    //        mHead_D = pHead_D;
                    //    }
                    //}
                //}

            #endregion           


            #region "ICLONABLE METHOD:"

                public object Clone()
                //===================
                {
                    return this.MemberwiseClone();
                }

            #endregion

        #endregion

    }
}
