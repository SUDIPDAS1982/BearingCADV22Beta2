//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsPin                                 '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  08NOV18                                '
//                                                                              '
//===============================================================================
// PB 12OCT18a. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace BearingCAD22
{
    [Serializable]
    public class clsPin             
    {
        [Serializable]
        public struct sSpec
        {
            public clsUnit Unit;
            public string Type;
            public string Mat;
            public string D_Desig;
            public Double L;                      
        }

        [Serializable]
        public struct sHole
        {
            //public Double Depth;
            public Double Depth_Up;
            public Double Depth_Low;
        }

        #region "MEMBER VARIABLES:"
        //=========================
            
            private sSpec mSpec;
            private sHole mHole;
            public string mPN;                  // PB 12OCT18a. Complete code
        #endregion


        #region "PROPERTY ROUTINES:"
        //=========================

            public sSpec Spec
            {
                get {return mSpec;}
            }

            public sHole Hole
            {
                get { return mHole;}
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
                set { mSpec.D_Desig = value; }
            }

            public Double Spec_L         
            {
                set { mSpec.L = value; }
            }

            public clsUnit Spec_Unit
            {
                set { mSpec.Unit = value; }
            }

            //public Double Hole_Depth
            //{
            //    set { mHole.Depth = value; }
            //}

            public Double Hole_Depth_Up
            {
                set { mHole.Depth_Up = value; }
            }

            public Double Hole_Depth_Low
            {
                set { mHole.Depth_Low = value; }
            }

            public string PN
            {
                get { return mPN; }
                set { mPN = value; }
            }

        #endregion

         
        public clsPin(clsUnit.eSystem UnitSystem_In)     
        //==========================================
        {
            mSpec.Unit = new clsUnit();
            mSpec.Unit.System = UnitSystem_In;
        }


        #region "CLASS METHODS:"
        //====================

            public Double D()     // PB 12OCT18a. There may be a generic method D in clsDrill. May be used  
            //--------------
            {
                double pD = 0.0;
                double pNumerator = 0.0;
                double pDenominator = 0.0;
            
                if (mSpec.D_Desig == null || mSpec.D_Desig == "") return pD;

                if (mSpec.Unit.System == clsUnit.eSystem.English)
                {
                    if (mSpec.D_Desig.Contains("/"))
                    {
                        pNumerator = Convert.ToInt32(modMain.ExtractPreData(mSpec.D_Desig, "/"));
                        pDenominator = Convert.ToInt32(modMain.ExtractPostData(mSpec.D_Desig, "/"));
                        pD = Convert.ToDouble(pNumerator / pDenominator);
                    }
                    else
                    {
                        pD = Convert.ToDouble(mSpec.D_Desig);
                    }
                }               
                else if (mSpec.Unit.System == clsUnit.eSystem.Metric && mSpec.D_Desig.Contains("M"))
                {
                    //....Not yet in the database. Parse the D_Desig string (e.g. M3) to get the dia in mm. 
                    pD = Convert.ToDouble(mSpec.D_Desig.Remove(0, 1)) / 25.4;         //....1 in = 25.4 mm.
                }
               

                return pD;
            }
           
        #endregion
    }
}
