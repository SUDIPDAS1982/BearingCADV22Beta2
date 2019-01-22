
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsOpCond                              '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  20NOV18                                '
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
using System.Runtime.Serialization.Formatters.Binary;     
using System.IO;                                        
using System.Runtime.Serialization;                     

namespace BearingCAD22
{
    [Serializable]
    public class clsOpCond :ICloneable
    {
        #region "STRUCTURE TYPES:"
        //========================          
            [Serializable]
            public struct sOilSupply
            {
                public string Lube_Type;
                public Double Flow_Reqd;
                //public string Flow_Reqd_Unit; 
                public string Flow_Type;         
                public Double Press;        
                public Double Temp;         
            }

        #endregion
        
        #region " MEMBER VARIABLE DECLARATIONS: "
        //======================================           
            private int mSpeed;
            private Double mRadial_Load;                // PB 08OCT18. May be later moved to clsBearing_Radial.
            private Double mRadial_LoadAng_Casing_SL;   // -do-
            //private Double[] mThrust_Load_Range = new Double[2];  // PB 07OCT18. Comment out

            private sOilSupply mOilSupply;           

        #endregion
        
        #region "CLASS PROPERTY ROUTINES:"
        //===============================

            //Rotation:
            //---------
            public int Speed
            {
                get { return mSpeed; }
                set { mSpeed = value; }
            }

            public Double Radial_Load
            {
                get { return mRadial_Load; }
                set { mRadial_Load = value; }
            }

            public Double Radial_LoadAng_Casing_SL
            {
                get { return mRadial_LoadAng_Casing_SL; }
                set { mRadial_LoadAng_Casing_SL = value; }
            }   

            //public Double[] Thrust_Load_Range
            //{
            //    get { return mThrust_Load_Range; }
            //    set { mThrust_Load_Range = value; }
            //}

            //Oil Supply:
            //-----------

            //GET:
            //----
            public sOilSupply OilSupply
            {
                get { return mOilSupply; }
            }

            //SET:
            //----
            public string OilSupply_Lube_Type
            {
                set { mOilSupply.Lube_Type = value; }
            }

            public Double OilSupply_Flow_Reqd
            {
                set { mOilSupply.Flow_Reqd = value; }
            }

            //public string OilSupply_Flow_Reqd_Unit
            //{
            //    set { mOilSupply.Flow_Reqd_Unit = value; }
            //}

            public string OilSupply_Flow_Type
            {
                set { mOilSupply.Flow_Type = value; }
            }

            public Double OilSupply_Press         
            {
                set { mOilSupply.Press = value; }
            }

            public Double OilSupply_Temp          
            {
                set { mOilSupply.Temp = value; }
            }


        //Class Constructor.
        //==================

            public clsOpCond()
            {
                 
            }

        #endregion     
        
        #region "SERIALIZE-DESERIALIZE:"
        //------------------------------

            public Boolean Serialize(string FilePath_In)
            //==========================================
            {
                try
                {
                    IFormatter serializer = new BinaryFormatter();
                    string pFileName = FilePath_In + "2.BearingCAD";

                    FileStream saveFile = new FileStream(pFileName, FileMode.Create, FileAccess.Write);

                    serializer.Serialize(saveFile, this);

                    saveFile.Close();

                    return true;
                }
                catch
                {
                    return false;
                }
            }


            public object Deserialize(string FilePath_In)
            //===========================================
            {
                IFormatter serializer = new BinaryFormatter();
                string pFileName = FilePath_In + "2.BearingCAD";
                FileStream openFile = new FileStream(pFileName, FileMode.Open, FileAccess.Read);
                object pObj;
                pObj = serializer.Deserialize(openFile);

                openFile.Close();

                return pObj;
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
