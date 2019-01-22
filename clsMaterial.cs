
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsMaterial                            '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
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
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Linq;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;  
using System.Runtime.Serialization;  
namespace BearingCAD22
{
    [Serializable]
    public class clsMaterial : ICloneable
    {

        #region "MEMBER VARIABLE DECLARATIONS"
            //================================

            private string mBase;
            private string mLining;
            private bool mLiningExists;

            //....WaukeshaCode:     
            private sWCode mWCode;      

            [Serializable]
            public struct sWCode
            {              
                public String Base;
                public String Lining;
            }

        #endregion


        #region "CLASS PROPERTY ROUTINES"

            //....Pad Maximums:
            public sWCode WCode
            {
                get { return mWCode; }
            }


            public String WCode_Base
            {
                set { mWCode.Base = value; }
            }


            public String WCode_Lining
            {
                set { mWCode.Lining = value; }
            }

            public String Base
            {                
                get { return mBase; }
                set { mBase = value; }
            }                   


            public string Lining
            {
                get { return mLining; }
                set { mLining = value; }
            }

           
            public bool LiningExists
            {
                get { return mLiningExists; }
                set { mLiningExists = value; }
            }
            
        #endregion


        #region "CLASS METHODS"
            //====================
            
            //public string MatCode(string Mat_In)
            ////===================================
            //{
            //    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    string pWaukeshaCode = "";
            //    var pProject = (from pRec in pBearingDBEntities.tblData_Mat where pRec.fldName == Mat_In select pRec.fldCode_Waukesha).ToList();

            //    if (pProject.Count > 0)
            //    {
            //        pWaukeshaCode = modMain.gDB.CheckDBString(pProject[0]);
            //    }
            //    return pWaukeshaCode;
       
            //}

            public string MatName(string WCode_In, string MatFileName_In)
            //=============================================================
            {
                string pName = "";

                string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE, pstrORDERBY;
                OleDbConnection pConnection = null;

                pstrFIELDS = "Name ";
                pstrFROM = " FROM [Mat$]";
                pstrWHERE = " WHERE Code_Waukesha = '" + WCode_In + "'";
                pstrORDERBY = " Order by Name ASC";

                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE + pstrORDERBY;

                OleDbDataReader pobjDR = null;
                pobjDR = modMain.gDB.GetDataReader(pstrSQL, MatFileName_In, ref pConnection);

                if(pobjDR.Read())
                {
                    pName = modMain.gDB.CheckDBString(pobjDR["Name"]);
                }
                pobjDR.Dispose();
                pConnection.Close();
                return pName;
            }

        #endregion

        #region " ICLONEABLE MEMBERS: "

            public object Clone()
            //===================           //SB 31MAR09
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
    }





   

    
}
