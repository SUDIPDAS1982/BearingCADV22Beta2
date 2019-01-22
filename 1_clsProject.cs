
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsProject                             '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  19NOV18                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data.Sql;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace BearingCAD22
{
    [Serializable]
    public class clsProject  
    {
        #region "MEMBER VARIABLE DECLARATIONS:"
        //=====================================
            private clsSOL mSOL = new clsSOL();
            private clsPNR mPNR = new clsPNR();

            private string mStatus;
        #endregion
        
        #region "CLASS PROPERTY ROUTINES:"
        //============================
            public clsSOL SOL
            {
                get { return mSOL; }
                set { mSOL = value; }
            }

            public clsPNR PNR
            {
                get { return mPNR; }
                set { mPNR = value; }
            }

            public string Status
            {
                get { return mStatus; }
                set { mStatus = value; }
            }

        #endregion
        
        #region "CLASS CONSTRUCTOR:"

            public clsProject(clsUnit.eSystem UnitSystem_In, clsBearing.eType Type_In)
            //========================================================================
            {
                //  Initialize.                
                mPNR.Unit.System = UnitSystem_In;   //....Default unit = English (automatically).

                if (Type_In == clsBearing.eType.JBearing)
                {
                    mPNR.Bearing = new clsJBearing(UnitSystem_In, Type_In);
                }
                else if (Type_In == clsBearing.eType.TBearing)
                {
                    mPNR.Bearing = new clsTBearing(UnitSystem_In, Type_In);
                }
            }

        #endregion

        #region "NESTED CLASS:"

            [Serializable]
            public class clsSOL
            {

                #region "ENUMERATION TYPES:"
                //==========================
                    public enum eType { Order, Proposal };

                #endregion

                private string mSONo;
                private string mLineNo;
                private string mRelatedNo;
                private eType mType;
                private clsCustomer mCustomer = new clsCustomer();

                #region "CLASS PROPERTY ROUTINES:"
                //==============================
                    public string SONo
                    {
                        get { return mSONo; }
                        set { mSONo = value; }
                    }

                    public string LineNo
                    {
                        get { return mLineNo; }
                        set { mLineNo = value; }
                    }

                    public string RelatedNo
                    {
                        get { return mRelatedNo; }
                        set { mRelatedNo = value; }
                    }

                    public eType Type
                    {
                        get { return mType; }
                        set { mType = value; }
                    }

                    public clsCustomer Customer
                    {
                        get { return mCustomer; }
                        set { mCustomer = value; }
                    }

                #endregion

                #region "NESTED CLASS:"

                [Serializable]
                public class clsCustomer
                {
                    private string mName;
                    private string mOrderNo;
                    private string mMachineName;

                    #region "CLASS PROPERTY ROUTINES:"
                    //-------------------------------

                        public string Name
                        {
                            get { return mName; }
                            set { mName = value; }
                        }

                        public string OrderNo
                        {
                            get { return mOrderNo; }
                            set { mOrderNo = value; }
                        }

                        public string MachineName
                        {
                            get { return mMachineName; }
                            set { mMachineName = value; }
                        }

                    #endregion
                }

                #endregion
            }


            [Serializable]
            public class clsPNR
            {
                private string mNo;
                private string mRev;
                private clsUnit mUnit = new clsUnit();
                private clsBearing mBearing;

                #region "CLASS PROPERTY ROUTINES:"
                //-------------------------------

                    public string No
                    {
                        get { return mNo; }
                        set { mNo = value; }
                    }

                    public string Rev
                    {
                        get { return mRev; }
                        set { mRev = value; }
                    }

                    public clsUnit Unit
                    {
                        get { return mUnit; }

                        set { mUnit = value; }
                    }

                    public clsBearing Bearing
                    {
                        get { return mBearing; }
                        set { mBearing = value; }
                    }

                #endregion
            }

        #endregion

        #region "SERIALIZE-DESERIALIZE:"
        //-------------------------

            public Boolean Serialize(string FilePath_In)
            //==========================================
            {
                try
                {
                    IFormatter serializer = new BinaryFormatter();
                    string pFileName = FilePath_In + "1.BearingCAD";

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
                string pFileName = FilePath_In + "1.BearingCAD";
                FileStream openFile = new FileStream(pFileName, FileMode.Open, FileAccess.Read);
                object pObj;
                pObj = serializer.Deserialize(openFile);

                openFile.Close();

                return pObj;
            }

        #endregion

        #region "ICLONEABLE MEMBERS"

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
       
    }
}
