
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing                             '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  19NOV18                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace BearingCAD22
{
     [Serializable]
    public abstract class clsBearing: ICloneable
    {  

        #region "ENUMERATION TYPES:"
        //==========================
            public enum eType { JBearing, TBearing };
        #endregion


        #region "MEMBER VARIABLES:"
        //=========================           
            private eType mType;
        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //================================

            public eType Type
            {
                get { return mType; }
                set { mType = value; }
            }
  

        #endregion


        public clsBearing(clsUnit.eSystem UnitSystem_In, eType Type_In)
        //=============================================================
        {
            mType = Type_In;
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
       
    }
}
