//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsPerformData                         '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  06DEC18                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
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
    [Serializable]
    public class clsPerformData
    //=========================
    {
        #region "MEMBER VARIABLES:"
        //=========================
            private Double mPower;
            private Double mFlowReqd;
            private string mFlowReqd_Unit;
            private Double mTempRise;
                   
        #endregion
        
        #region "CLASS PROPERTY ROUTINES:"
        //=================================

            //.... Power (Eng Unit).
            public Double Power
            {
                get { return mPower; }
                set { mPower = value;}
            }

            //.... Flow Reqd (Eng Unit).
            public Double FlowReqd
            {
                get { return mFlowReqd; }
                set { mFlowReqd = value;}
            }

            ///.... Flow Reqd Unit.
            public string FlowReqd_Unit
            {
                get { return mFlowReqd_Unit; }
                set { mFlowReqd_Unit = value; }
            }

            //.... Temp Rise (Eng Unit).
            public Double TempRise
            {
                get { return mTempRise; }
                set { mTempRise = value;}
            }


        #endregion
        
        #region "CONSTRUCTOR:"

            public clsPerformData()
            //=====================
            {

            }

        #endregion
        
        #region "CLASS METHODS:"

            //public Double Calc_TempRise_F ()
            ////==============================
            //{
            //    Double pTempRise = 0.0;

            //    if(mFlowReqd_gpm != 0.0)        //BG 29JAN13
            //    {
            //       pTempRise = 12.4 * (mPower_HP / mFlowReqd_gpm);
            //    }
                       
            //    return pTempRise;
            //}

        #endregion
    }

}
