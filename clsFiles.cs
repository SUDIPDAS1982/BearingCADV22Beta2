
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsFiles                               '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  20DEC18                                '
//                                                                              '
//===============================================================================


//    FILE NAMING CONVENTIONS:
//    -----------------------
//    ....FileName  ==>  Path, File, Extn
//    ....FileTitle ==>        File, Extn
//    ....File      ==>        File

//    *******************************************************************************
//    *          CLASS FOR  FILE MANIPULATION - READ & WRITE AND DELETE.            *
//    *******************************************************************************

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Xml;
using System.Linq;
using System.Configuration;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Word = Microsoft.Office.Interop.Word;
using EXCEL = Microsoft.Office.Interop.Excel;
using System.Collections.Specialized;

namespace BearingCAD22
{
    public class clsFiles
    {
        #region "FILE DEFINITIONS"
            //=====================
            private const int mcObjFile_Count = 1;

            //File Directories & Names:
            //=========================
            private const string mcDriveRoot_Client = "C:";                                       

            //  Installation:
            //  -------------
            //
            //....Root Directory of Client Machine:  
            private const string mcDirRoot = "\\BearingCAD\\";
                   
            //....Config File Name of Client Machine.               
            private const string mcConfigFile_Client ="BearingCAD22_Client.config";

            //....Config File Name of Client Machine.
            private const string mcConfigFile_Server = "BearingCAD22_Server.config";
                    
            //....LogoFile.     
            private const string mcLogo_Title = "Waukesha Logo.bmp"; 

        #endregion


        #region "MEMBER VARIABLE DECLARATIONS"
            //================================

            //....DriveRoot
            private string mDriveRoot;

            //....DB FileName and Type
            private static string mDBFileName, mDBServerName;   
        
            //XLRadial Sheet
            private string mXLRadial_SheetName = "";           
           
            //....Program Data File
            private string mFilePath_ProgramDataFile_EXCEL;
            private string mFileTitle_EXCEL_MatData;            
            private string mFileTitle_EXCEL_StdPartsData;
            private string mFileTitle_EXCEL_StdToolData;
        

            //....Design Tables.

            //....Directory of Design Table Template.
            private string mFilePath_Template_EXCEL;
            private string mFileTitle_Template_EXCEL_Parameter_Complete;
            

            //....Inventor Files.  
            //....Directory of Design Table Template.
            private string mFilePath_Template_Inventor;

            //....Project Dependent Files.
            private string mFileTitle_Template_Inventor_Radial;
            private string mFileTitle_Template_Inventor_Seal_Front;
            private string mFileTitle_Template_Inventor_Seal_Back;
            private string mFileTitle_Template_Inventor_Thrust_Front;
            private string mFileTitle_Template_Inventor_Thrust_Back;
            private string mFileTitle_Template_Inventor_Complete;
           
            private string mFileName_BearingCAD = "";

           
         #endregion


        #region "CLASS PROPERTY ROUTINES"
            //===========================

            public string FileName_BearingCAD
            {
                get { return mFileName_BearingCAD; }
                set { mFileName_BearingCAD = value; }
            }

            //READ-ONLY PROPERTIES:
            //=====================

            public string Logo
            //=================
            {
                get
                { 
                    return mcDriveRoot_Client + mcDirRoot + "Images\\" + mcLogo_Title; 
                }   
            }

            public static string DBFileName
            //==============================
            {
                get { return mDBFileName; }
            }


            public static string DBServerName
            //===============================
            {
                get { return mDBServerName; }
            }

            public string XLRadial_SheetName
            //==============================
            {
                get { return mXLRadial_SheetName; }
                set { mXLRadial_SheetName = value; }
            }

            //....Program Data File
            public string FileTitle_EXCEL_MatData
            //===================================    
            {
                get
                {
                    return mDriveRoot + "\\" + mFilePath_ProgramDataFile_EXCEL + "\\" + mFileTitle_EXCEL_MatData;
                }
            }

            public string File_InputPath
            //==========================   
            {
                get
                {
                    return mcDriveRoot_Client + mcDirRoot  +"Projects\\V22";
                }
            }


            public string FileTitle_EXCEL_StdPartsData
            //===========================================    
            {
                get
                {
                    return mDriveRoot + "\\" + mFilePath_ProgramDataFile_EXCEL + "\\" + mFileTitle_EXCEL_StdPartsData;
                }
            }

            public string FileTitle_EXCEL_StdToolData
            //===========================================    
            {
                get
                {
                    return mDriveRoot + "\\" + mFilePath_ProgramDataFile_EXCEL + "\\" + mFileTitle_EXCEL_StdToolData;
                }
            }


                //public string FileTitle_Template_DDR
                ////==================================
                //{
                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_WORD + "\\" + mFileTitle_Template_DDR; 
                //    }
                //}


                public string FileTitle_Template_EXCEL_Parameter_Complete
                //========================================================    
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_EXCEL + "\\" + mFileTitle_Template_EXCEL_Parameter_Complete;
                    }
                }
        

                //....Inventor Files        
                public string FileTitle_Template_Inventor_Radial
                //===============================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Radial;
                    }
                }

                public string FileTitle_Template_Inventor_Seal_Front
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Seal_Front;
                    }
                }

                public string FileTitle_Template_Inventor_Seal_Back
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Seal_Back;
                    }
                }

                public string FileTitle_Template_Inventor_Thrust_Front
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Thrust_Front;
                    }
                }

                public string FileTitle_Template_Inventor_Thrust_Back
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Thrust_Back;
                    }
                }

                public string FileTitle_Template_Inventor_Complete
                //=================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Complete;
                    }
                }

        #endregion

        public clsFiles()
        //===============
        {
            //....Reads Configuration File.
            ReadConfigFile();
            mXLRadial_SheetName = "XLRadial SI";
        }

        #region "CLASS METHODS"

            //---------------------------------------------------------------------------
            //                      UTILITY ROUTINES - BEGIN                             '
            //---------------------------------------------------------------------------                 

            private void ReadConfigFile()
            //==========================
            {
                try      
                {
                    //  READ CLIENT CONFIGURATION FILE:
                    //  -------------------------------

                        string pConfigFileName_Client = mcDriveRoot_Client + mcDirRoot + mcConfigFile_Client;

                        FileStream pSW = new FileStream(pConfigFileName_Client, FileMode.Open,
                                                        FileAccess.Read, FileShare.ReadWrite);

                        //....Create the xmldocument
                            System.Xml.XmlDocument pXML = new System.Xml.XmlDocument();

                        //....Root Node of XML.
                            XmlNode pRoot;
                            pXML.Load(pSW);
                            pRoot = pXML.DocumentElement;

                        //....Child Node.
                            XmlNode pRootChild = pRoot.FirstChild;

                        //.....Get Installation Directory Of Server Configuration.
                            mDriveRoot = pRootChild.InnerText;
                            pXML = null;
                            pSW.Close();

                    //  READ SERVER CONFIGURATION FILE:
                    //  -------------------------------

                        string pConfigFileName_Server = mDriveRoot + mcDirRoot + mcConfigFile_Server;

                        if (!File.Exists(pConfigFileName_Server))
                        {
                            MessageBox.Show("Please Specify Proper Root Installation Directory in Client configuration file.", "Error");
                            System.Environment.Exit(0);
                        }

                        pSW = new FileStream(pConfigFileName_Server, FileMode.Open,
                                                            FileAccess.Read, FileShare.ReadWrite);

                        //....Create the xmldocument
                            pXML = new System.Xml.XmlDocument();

                        //....Root Node of XML.
                            pXML.Load(pSW);
                            pRoot = pXML.DocumentElement;

                            foreach (XmlNode pRChild in pRoot.ChildNodes)       
                            {
                                //.....Mapping Rules Implementation.
                                switch (pRChild.Name)
                                {
                                    case "SEREVERName":
                                        //-----------------
                                        mDBServerName = pRChild.InnerText;
                                        break;

                                    case "DataBaseName":
                                        //--------------
                                        mDBFileName = pRChild.InnerText;
                                        break;


                                    case "FilePath_ProgramDataFile_EXCEL":
                                        //-------------------------
                                        mFilePath_ProgramDataFile_EXCEL = pRChild.InnerText;
                                        break;

                                    case "FileTitle_EXCEL_MatData":
                                        //-------------------------
                                        mFileTitle_EXCEL_MatData = pRChild.InnerText;
                                        break;


                                    case "FileTitle_EXCEL_StdPartsData":
                                        //------------------------------
                                        mFileTitle_EXCEL_StdPartsData = pRChild.InnerText;
                                        break;

                                    case "FileTitle_EXCEL_StdToolData":
                                        //-------------------------
                                        mFileTitle_EXCEL_StdToolData = pRChild.InnerText;
                                        break;

                                    case "FilePath_Template_EXCEL":
                                        //-------------------------
                                        mFilePath_Template_EXCEL = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_EXCEL_Parameter_Complete":
                                        mFileTitle_Template_EXCEL_Parameter_Complete = pRChild.InnerText;
                                        break;


                                    case "FilePath_Template_Inventor":
                                        //----------------------
                                        mFilePath_Template_Inventor = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Radial":
                                        //------------------------------
                                        mFileTitle_Template_Inventor_Radial = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Seal_Front":
                                        //----------------------------
                                        mFileTitle_Template_Inventor_Seal_Front = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Seal_Back":
                                        //---------------------------------
                                        mFileTitle_Template_Inventor_Seal_Back = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Thrust_Front":
                                        //----------------------------
                                        mFileTitle_Template_Inventor_Thrust_Front = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Thrust_Back":
                                        //---------------------------------
                                        mFileTitle_Template_Inventor_Thrust_Back = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Complete":
                                        //------------------------------------
                                        mFileTitle_Template_Inventor_Complete = pRChild.InnerText;
                                        break;
                                }
                            }
                            pXML = null;
                            pSW.Close();

                           // UpdateAppConfig(mDBServerName);

                }

                catch (FileNotFoundException pEXP)      //BG 13JUL09
                {
                    MessageBox.Show(pEXP.Message, "File Error");        
                }

            }

            #region "INPUT DATA:"
            //--------------------

            public Boolean Import_DDR_Data(string FileName_In, ref clsProject Project_In)
            //===========================================================================
            {
                //MessageBox.Show("All open Word files will be closed automatically.\nPlase save before proceeding.", "Warning: Word Files!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                CloseWordFiles();
                string pWordFileName = FileName_In;
               
                Word.Document pDoc = null;
                Word.ContentControls pContentControls = null;

                Word.Application pApp = new Word.Application();
                pApp.Documents.Open(FileName_In, Missing.Value, Missing.Value, false, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value);

                int pText6_Occur = 0, pText2_Occur = 0, pDropDown1_Occur = 0, pText7_Occur = 0, pText8_Occur = 0, pText5_Occur = 0;
                string pSO_No = "";
                string pQuoteNo = "";
                string pRNo = "";
                try
                {
                    pDoc = pApp.ActiveDocument;
                    pContentControls = pDoc.ContentControls;
                    Boolean pBookMark = false;

                    foreach (Word.FormField pField in pDoc.FormFields)
                    {
                        pBookMark = true;
                        string pVal = pField.Name + " " + pField.Result;

                        switch (pField.Name)
                        {
                            case "Text6":
                                //------------
                                pText6_Occur++;
                                if (pText6_Occur == 1)
                                {
                                    //txtCustName.Text = pField.Result;
                                    Project_In.SOL.Customer.Name = pField.Result;
                                }
                                break;

                            case "Text2":
                                //------------
                                pText2_Occur++;
                                if (pText2_Occur == 1)
                                {
                                    pQuoteNo = pField.Result;
                                }
                                break;

                            case "Dropdown1":
                                //------------
                                pDropDown1_Occur++;
                                if (pDropDown1_Occur == 1)
                                {
                                    if (pField.Result == "Order")
                                    {
                                        Project_In.SOL.Type = clsProject.clsSOL.eType.Order;
                                    }
                                    else
                                    {
                                        Project_In.SOL.Type = clsProject.clsSOL.eType.Proposal;
                                    }
                                }
                                break;

                            case "Text7":
                                //------------
                                pText7_Occur++;
                                if (pText7_Occur == 1)
                                {
                                    //txtPartNo.Text = pField.Result;
                                    Project_In.PNR.No = pField.Result;
                                }
                                break;

                            case "Text8":
                                //------------
                                pText8_Occur++;
                                if (pText8_Occur == 2)
                                {
                                    if (pField.Result.Contains((char)13))
                                    {
                                        string pCustoOrderNo = "";
                                        string[] pLines = pField.Result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                                        for (int i = 0; i < pLines.Length; i++)
                                        {
                                            if (i == pLines.Length - 1)
                                            {
                                                pCustoOrderNo = pCustoOrderNo + pLines[i].Trim();
                                            }
                                            else
                                            {
                                                pCustoOrderNo = pCustoOrderNo + pLines[i].Trim() + ", ";
                                            }
                                        }
                                        //txtCustOrderNo.Text = pCustoOrderNo;
                                        Project_In.SOL.Customer.OrderNo = pCustoOrderNo;
                                    }
                                    else
                                    {
                                        //txtCustOrderNo.Text = pField.Result;
                                        Project_In.SOL.Customer.OrderNo = pField.Result;
                                    }
                                }
                                else if (pText8_Occur == 3)
                                {
                                    //txtCustMachineName.Text = pField.Result;
                                    Project_In.SOL.Customer.MachineName = pField.Result;
                                }

                                break;

                            case "Text5":
                                //------------
                                pText5_Occur++;
                                if (pText5_Occur == 1)
                                {
                                    pSO_No = pField.Result;
                                    string[] pTemp_SO_First_Array = null;
                                    string[] pTemp_SO_Sub_Array = null;
                                    StringCollection pRelatedSO_No = new StringCollection();

                                    if (pSO_No.Contains("&"))
                                    {
                                        pTemp_SO_First_Array = pSO_No.Split('&');

                                        for (int i = 0; i < pTemp_SO_First_Array.Length; i++)
                                        {
                                            pTemp_SO_First_Array[i] = pTemp_SO_First_Array[i].Trim();
                                            if (pTemp_SO_First_Array[i].Contains(","))
                                            {
                                                pTemp_SO_Sub_Array = pTemp_SO_First_Array[i].Split(',');
                                                pSO_No = pTemp_SO_Sub_Array[0].Trim();

                                                for (int j = 0; j < pTemp_SO_Sub_Array.Length; j++)
                                                {
                                                    if (j > 0)
                                                    {
                                                        if (pTemp_SO_Sub_Array[j] != "")
                                                        {
                                                            if (!pTemp_SO_Sub_Array[j].Contains("-"))
                                                            {
                                                                string pSO_Val = modMain.ExtractPreData(pSO_No, "-") + "-" + pTemp_SO_Sub_Array[j].Trim();
                                                                pRelatedSO_No.Add(pSO_Val);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                pRelatedSO_No.Add(pTemp_SO_First_Array[i]);
                                            }

                                        }
                                    }

                                    Boolean pFlag = false;

                                    if (pSO_No == "" || pSO_No == "N/A")
                                    {
                                        pSO_No = pQuoteNo;
                                        pFlag = true;
                                    }

                                    if (pFlag == false && pQuoteNo != "" && pQuoteNo != "N/A")
                                    {
                                        pRelatedSO_No.Add(pQuoteNo);
                                    }

                                    for (int j = 0; j < pRelatedSO_No.Count; j++)
                                    {
                                        if (j == pRelatedSO_No.Count - 1)
                                        {
                                            pRNo = pRNo + pRelatedSO_No[j];
                                        }
                                        else
                                        {
                                            pRNo = pRNo + pRelatedSO_No[j] + ", ";
                                        }
                                    }
                                }

                                break;
                        }

                    }

                    if (!pBookMark)
                    {
                        MessageBox.Show("'" + Path.GetFileName(FileName_In) + "' is not in correct format. Please check before import.", "Error - Import Data - DDR", MessageBoxButtons.OK);
                        return false;
                    }
                    else
                    {
                        if (pSO_No != "")
                        {
                            Project_In.SOL.SONo = pSO_No;
                            Project_In.SOL.RelatedNo = pRNo;
                            MessageBox.Show("Data have been imported successfully from \n '" + Path.GetFileName(pWordFileName) + "'.", "Data Import from DDR", MessageBoxButtons.OK);
                        }
                        
                    }
                    ////if (pSO_No != "")
                    ////{
                    ////    cmbSONo_Part1.Text = pSO_No.Substring(0, 2);
                    ////    if (pSO_No.Contains("-"))
                    ////    {
                    ////        txtSONo_Part2.Text = modMain.ExtractMidData(pSO_No, " ", "-");
                    ////    }
                    ////    else
                    ////    {
                    ////        txtSONo_Part2.Text = pSO_No.Substring(3);
                    ////        //txtSONo_Part2.Text = modMain.ExtractPostData(pSO_No, " ");
                    ////    }

                    ////    string pTemp = modMain.ExtractPostData(pSO_No, "-");

                    ////    Boolean pIsNumeric = false;
                    ////    foreach (char value in pTemp)
                    ////    {
                    ////        pIsNumeric = char.IsDigit(value);
                    ////    }

                    ////    if (pIsNumeric)
                    ////    {
                    ////        txtSONo_Part3.Text = Convert.ToString(System.Text.RegularExpressions.Regex.Replace(pTemp, "[^0-9]+", string.Empty));
                    ////    }

                    ////    txtRelatedSONo.Text = pRNo;
                    ////}
                }
                catch (Exception pExp)
                {
                    MessageBox.Show("Input Data is not in correct format.", "Error - Import Data - DDR", MessageBoxButtons.OK);
                }
                finally
                {
                    pDoc.Close();
                    pApp = null;
                    
                }
                return true;
                //Cursor = Cursors.Default;
                //}
            }

            public Boolean Retrieve_XLRadial_SheetName(string ExcelFileName_In,ref List<string> SheetName_Out)
            //==============================================================================================
            {
                CloseExcelFiles();
                EXCEL.Application pApp = null;
                pApp = new EXCEL.Application();
                

                Boolean pIsDefaultSheetFound = false;

                try
                {
                    SheetName_Out.Clear();

                    //....Open Load.xls WorkBook.
                    EXCEL.Workbook pWkbOrg = null;
                    pWkbOrg = pApp.Workbooks.Open(ExcelFileName_In, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value,
                                                  Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                  Missing.Value, Missing.Value, Missing.Value);

                    Boolean pFound = false;
                    foreach (EXCEL.Worksheet sheet in pWkbOrg.Sheets)
                    {
                        // Check the name of the current sheet
                        if (sheet.Name.ToUpper() == "XLRADIAL SI")
                        {
                            pIsDefaultSheetFound = true;
                            pFound = true;
                            break; // Exit the loop now
                        }
                    }

                    foreach (EXCEL.Worksheet sheet in pWkbOrg.Sheets)
                    {
                        SheetName_Out.Add(sheet.Name);
                    }

                    //if (pFound == false)
                    //{                        
                    //    foreach (EXCEL.Worksheet sheet in pWkbOrg.Sheets)
                    //    {
                    //        SheetName_Out.Add(sheet.Name);
                    //    }
                    //}

                    pApp.DisplayAlerts = false;
                    pWkbOrg.Close();
                    pApp.Quit();
                    return pIsDefaultSheetFound;
                }
                catch (Exception pEx)
                {
                    MessageBox.Show(pEx.ToString(), "XLRadial Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    pApp.Quit();
                }
            }


            public Boolean Retrieve_Unit_XLRadial(string ExcelFileName_In, clsUnit.eSystem UnitSystem_In)
            //========================================================================================
            {
                CloseExcelFiles();
                EXCEL.Application pApp = null;
                pApp = new EXCEL.Application();

                Boolean pIsRetrieved = false;

                try
                {
                    //....Open Load.xls WorkBook.
                    EXCEL.Workbook pWkbOrg = null;
                    pWkbOrg = pApp.Workbooks.Open(ExcelFileName_In, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value,
                                                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                    Missing.Value, Missing.Value, Missing.Value);

                    string pVal = "";

                    //EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["XLRadial SI"];
                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets[mXLRadial_SheetName];

                    //....Unit
                    pVal = Convert.ToString(pWkSheet.Cells[4, 13].value);

                    if (pVal.Trim() == "mm")
                    {
                        pVal = "Metric";
                    }
                    else
                    {
                        pVal = "English";
                    }                    

                    if (pVal != "")
                    {
                        pIsRetrieved = true;
                        if (UnitSystem_In != (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pVal))
                        {
                            modMain.gProject.PNR.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pVal.Trim());
                        }
                    }
                    pApp.DisplayAlerts = false;
                    pWkbOrg.Close();
                    return pIsRetrieved;
                }
                catch (Exception pEx)
                {
                    MessageBox.Show("XLRadial file is not in correct format.", "XLRadial Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    pApp.Quit();
                }

                //MessageBox.Show("Data have been imported successfully from '" + Path.GetFileName(ExcelFileName_In) + "'", "Data Import from XLRadial", MessageBoxButtons.OK);
            }

            public void Retrieve_Params_XLRadial(string ExcelFileName_In, clsUnit.eSystem UnitSystem_In,
                                                 clsJBearing Bearing_In)
            //==========================================================================================
            {
               
                CloseExcelFiles();

                EXCEL.Application pApp = null;
                pApp = new EXCEL.Application();

                try
                {

                    pApp.DisplayAlerts = false; //Don't want Excel to display error messageboxes

                    //....Open Load.xls WorkBook.
                    EXCEL.Workbook pWkbOrg = null;
                    pWkbOrg = pApp.Workbooks.Open(ExcelFileName_In, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value,
                                                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                    Missing.Value, Missing.Value, Missing.Value);

                    string pVal = "";
                    double pVal_Out = 0.0F;

                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets[mXLRadial_SheetName];
                    EXCEL.Range pExcelCellRange = null;

                    pVal = Convert.ToString(pWkSheet.Cells[3, 12].value);

                    if (pVal != "")
                    {
                        Bearing_In.OpCond.Speed = Convert.ToInt32(pVal);
                    }

                    //....Unit
                    pVal = Convert.ToString(pWkSheet.Cells[4, 13].value);

                    if (pVal.Trim() == "mm")
                    {
                        pVal = "Metric";
                    }
                    else
                    {
                        pVal = "English";
                    }

                    if (pVal != "")
                    {
                        if (UnitSystem_In != (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pVal))
                        {
                            modMain.gProject.PNR.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pVal.Trim());
                        }
                    }

                    //....Conversion Factor
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[5, 12].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Pad.L = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[6, 12].value);

                    if (pVal != "")
                    {
                        Bearing_In.OpCond.Radial_Load = Convert.ToDouble(pVal) / 1000;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[7, 12].value);

                    if (pVal != "")
                    {
                        Bearing_In.OpCond.Radial_LoadAng_Casing_SL = Convert.ToDouble(pVal);
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[8, 12].value);
                    Bearing_In.RadB.Pad.LoadOrient = pVal;
                    //if (pVal != "")
                    //{
                    //    Bearing_In.RadB.Pad.LoadOrient = (clsRadB.clsPad.eLoadOrient)
                    //                                                       Enum.Parse(typeof(clsRadB.clsPad.eLoadOrient), pVal);
                    //}

                    pVal = Convert.ToString(pWkSheet.Cells[9, 12].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Pad.Count = Convert.ToInt32(pVal);
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[10, 12].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Pad.Angle = Convert.ToInt32(pVal);
                    }


                    pVal = Convert.ToString(pWkSheet.Cells[11, 12].value);

                    if (pVal != "")
                    {
                        double pPivot_Offset = Convert.ToDouble(pVal) * 100;
                        Bearing_In.RadB.Pad.Pivot_Offset = pPivot_Offset;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[12, 12].value);

                    if (pVal != "")
                    {
                        if (pVal == "Split")
                        {
                            Bearing_In.RadB.SplitConfig = true;
                        }
                        else
                        {
                            Bearing_In.RadB.SplitConfig = false;
                        }
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[13, 12].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.ARP.Ang_Casing_SL = Convert.ToDouble(pVal);
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[14, 12].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Pad.Pivot_AngStart_Casing_SL = Convert.ToDouble(pVal);
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[18, 12].value);

                    if (pVal != "")
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            Bearing_In.PerformData.Power = modMain.gUnit.CFac_Power_MetToEng(Convert.ToDouble(pVal));
                        }
                        else
                        {
                            Bearing_In.PerformData.Power = Convert.ToDouble(pVal);
                        }
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[19, 12].value);

                    if (pVal != "")
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            Bearing_In.PerformData.TempRise = modMain.gUnit.CFac_Temp_MetToEng(Convert.ToDouble(pVal));
                        }
                        else
                        {
                            Bearing_In.PerformData.TempRise = Convert.ToDouble(pVal);
                        }
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[20, 13].value);
                    if (pVal != "")
                    {
                        Bearing_In.PerformData.FlowReqd_Unit = pVal.ToString().ToUpper();
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[20, 12].value);

                    if (pVal != "")
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            if (Bearing_In.PerformData.FlowReqd_Unit == "LPM")
                            {
                                Bearing_In.PerformData.FlowReqd = modMain.gUnit.CFac_LPM_MetToEng(Convert.ToDouble(pVal));
                            }
                            else if (Bearing_In.PerformData.FlowReqd_Unit == "LPS")
                            {
                                Bearing_In.PerformData.FlowReqd = modMain.gUnit.CFac_LPS_MetToEng(Convert.ToDouble(pVal));
                            }
                        }
                        else
                        {
                            Bearing_In.PerformData.FlowReqd = Convert.ToDouble(pVal);
                        }
                    }

                    


                    //....OilInlet Rerd Area
                    pVal = Convert.ToString(pWkSheet.Cells[21, 12].value);

                    if (pVal != "")
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            Bearing_In.RadB.OilInlet.Annulus_Area = modMain.gUnit.CFac_Area_MetToEng(Convert.ToDouble(pVal));
                        }
                        else
                        {
                            Bearing_In.RadB.OilInlet.Annulus_Area = Convert.ToDouble(pVal);
                        }
                    }

                    //...Shaft Dia
                    pVal = Convert.ToString(pWkSheet.Cells[12, 15].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.DShaft_Range[0] = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[12, 16].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.DShaft_Range[1] = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Bearing Bore
                    pVal = Convert.ToString(pWkSheet.Cells[13, 15].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Bore_Range[0] = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[13, 16].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Bore_Range[1] = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Pad Bore
                    pVal = Convert.ToString(pWkSheet.Cells[14, 15].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.PadBore_Range[0] = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[14, 16].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.PadBore_Range[1] = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Seal Bore - Front
                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    //{
                    pVal = Convert.ToString(pWkSheet.Cells[19, 15].value);

                    if (pVal != "")
                    {
                        Bearing_In.EndPlate[0].DBore_Range[0] = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[19, 16].value);

                    if (pVal != "")
                    {
                        Bearing_In.EndPlate[0].DBore_Range[1] = Convert.ToDouble(pVal) / pConvF;
                    }
                    //}

                    //....Seal Bore - Back
                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    //{
                    pVal = Convert.ToString(pWkSheet.Cells[20, 15].value);

                    if (pVal != "")
                    {
                        Bearing_In.EndPlate[1].DBore_Range[0] = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[20, 16].value);

                    if (pVal != "")
                    {
                        Bearing_In.EndPlate[1].DBore_Range[1] = Convert.ToDouble(pVal) / pConvF;
                    }
                    //}

                    //....Bearing OD
                    pVal = Convert.ToString(pWkSheet.Cells[21, 15].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.OD_Range[0] = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[21, 16].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.OD_Range[1] = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Oil Supply Type
                    pVal = Convert.ToString(pWkSheet.Cells[4, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.OpCond.OilSupply_Lube_Type = pVal;
                    }

                    //....Oil Supply Pressure
                    pVal = Convert.ToString(pWkSheet.Cells[5, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.OpCond.OilSupply_Press = Convert.ToDouble(pVal);
                    }

                    //....Oil Supply Temp
                    pVal = Convert.ToString(pWkSheet.Cells[6, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.OpCond.OilSupply_Temp = Convert.ToDouble(pVal);
                    }

                    //....Oil Noozle Dia
                    pVal = Convert.ToString(pWkSheet.Cells[7, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.OilInlet.Orifice_D = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Number of Nozzle
                    pVal = Convert.ToString(pWkSheet.Cells[8, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.OilInlet.Orifice_Count = Convert.ToInt32(pVal);
                    }

                    //....Pad Shape
                    pVal = Convert.ToString(pWkSheet.Cells[11, 21].value);

                    if (pVal != "")
                    {
                        if (pVal.Contains("Uniform"))
                        {
                            Bearing_In.RadB.Pad.T_Pivot_Checked = true;
                        }
                        else
                        {
                            Bearing_In.RadB.Pad.T_Pivot_Checked = false;
                        }
                    }

                    //....Pad Thick (Leading)
                    pVal = Convert.ToString(pWkSheet.Cells[12, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Pad.T_Lead = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Pad Thick (Pivot)
                    pVal = Convert.ToString(pWkSheet.Cells[13, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Pad.T_Pivot = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Pad Thick (Trailing)
                    pVal = Convert.ToString(pWkSheet.Cells[14, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Pad.T_Trail = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Lining T
                    pVal = Convert.ToString(pWkSheet.Cells[15, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.LiningT = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Pad RFillet
                    pVal = Convert.ToString(pWkSheet.Cells[16, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.Pad.RFillet = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Axial Seal Gap
                    pVal = Convert.ToString(pWkSheet.Cells[17, 21].value);

                    if (pVal != "")
                    {
                        Bearing_In.RadB.AxialSealGap[0] = Convert.ToDouble(pVal) / pConvF;
                        Bearing_In.RadB.AxialSealGap[1] = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Seal Blade Thickness   
                    pVal = Convert.ToString(pWkSheet.Cells[18, 21].value);

                    //....Seal Bore - Front
                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    //{

                    if (pVal != "")
                    {
                        Bearing_In.EndPlate[0].Seal.Blade.T = Convert.ToDouble(pVal) / pConvF;
                    }
                    //}

                    //....Seal Bore - Back
                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    //{

                    if (pVal != "")
                    {
                        Bearing_In.EndPlate[1].Seal.Blade.T = Convert.ToDouble(pVal) / pConvF;
                    }
                    //}

                    //....Web Thickness
                    pVal = Convert.ToString(pWkSheet.Cells[19, 21].value);
                    if (pVal != "")
                    {
                        ((clsPivot.clsFP)Bearing_In.RadB.Pivot).Web_T = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Web Height
                    pVal = Convert.ToString(pWkSheet.Cells[20, 21].value);
                    if (pVal != "")
                    {
                        ((clsPivot.clsFP)Bearing_In.RadB.Pivot).Web_H = Convert.ToDouble(pVal) / pConvF;
                    }

                    //....Web Fillet
                    pVal = Convert.ToString(pWkSheet.Cells[21, 21].value);
                    if (pVal != "")
                    {
                        ((clsPivot.clsFP)Bearing_In.RadB.Pivot).Web_RFillet = Convert.ToDouble(pVal) / pConvF;
                    }

                    pWkbOrg.Close();

                    MessageBox.Show("Data have been imported successfully from '" + Path.GetFileName(ExcelFileName_In) + "'", "Data Import from XLRadial", MessageBoxButtons.OK);
                }
                catch (Exception pExp)
                {
                    MessageBox.Show(pExp.ToString(), "XLRadial Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    pApp.Quit();
                }
            }

            #endregion

            #region "INPUT DATA:"

            public void Read_Parameter_Complete(ref clsProject Project_In, string FileName_In, Boolean Visible_Status_In)
            //============================================================================================================
            {
                try
                {
                    object mobjMissing = Missing.Value;              //....Missing object.
                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();                    

                    //....Open Original WorkBook.
                    EXCEL.Workbook pWkbOrg = null;

                    pWkbOrg = pApp.Workbooks.Open(FileName_In, mobjMissing, false,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing);

                    //....Open WorkSheet - 'Complete ASSY'            
                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Complete Assy"];
                    Boolean pUnit = Read_Parameter_Complete_Assy(Project_In, pWkSheet);


                    if (pUnit)
                    {
                        //....Open WorkSheet - 'Radial Bearing'            
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Radial Bearing"];
                        Read_Parameter_Complete_Radial(Project_In, pWkSheet);

                        //....Open WorkSheet - 'Mounting'    
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Mounting"];
                        Read_Parameter_Complete_Mounting(Project_In, pWkSheet);

                        //....EndPlate: Seal
                        //clsSeal[] mEndSeal = new clsSeal[2];
                        //for (int i = 0; i < 2; i++)
                        //{
                        //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                        //    {
                        //        mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                        //    }
                        //}

                        //....Open WorkSheet - 'Front Config - Seal' 
                        //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                        //{
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Seal"];
                        Read_Parameter_Complete_Seal_Front(Project_In, ((clsJBearing)Project_In.PNR.Bearing).EndPlate[0], pWkSheet);

                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].DBore_Range = ((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].DBore_Range;
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat = (clsMaterial)((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].Mat.Clone();
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[0].Mat_LiningT = ((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].Mat_LiningT;
                        //}
                        //else
                        //{
                        //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Seal"];
                        //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                        //}


                        //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                        //{
                        //....Open WorkSheet - 'Back Config - Seal'    
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Seal"];
                        Read_Parameter_Complete_Seal_Back(Project_In, ((clsJBearing)Project_In.PNR.Bearing).EndPlate[1], pWkSheet);
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].DBore_Range = ((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].DBore_Range;
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat = (clsMaterial)((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].Mat.Clone();
                        ((clsJBearing)modMain.gProject.PNR.Bearing).EndPlate[1].Mat_LiningT = ((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].Mat_LiningT;
                        //}
                        //else
                        //{
                        //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Seal"];
                        //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                        //}


                        ////.............
                        ////....EndPlate: Thurst Bearing
                        //clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];
                        //for (int i = 0; i < 2; i++)
                        //{
                        //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
                        //    {
                        //        mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(modMain.gProject.Product.EndPlate[i])).Clone();
                        //    }
                        //}

                        ////....Open WorkSheet - 'Front TL Thurst Bearing' 
                        //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
                        //{
                        //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Thurst Bearing TL"];
                        //    Write_Parameter_Complete_Thrust_Front(modMain.gProject, mEndTB[0], pWkSheet);
                        //}
                        //else
                        //{
                        //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Thurst Bearing TL"];
                        //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                        //}

                        ////....Open WorkSheet - 'Back TL Thurst Bearing' 
                        //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                        //{
                        //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Thurst Bearing TL"];
                        //    Write_Parameter_Complete_Thrust_Back(modMain.gProject, mEndTB[1], pWkSheet);
                        //}
                        //else
                        //{
                        //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Thurst Bearing TL"];
                        //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                        //}

                        //..............

                        //pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Accessories"];
                        //Write_Parameter_Complete_Accessories(modMain.gProject, modMain.gProject.Product.Accessories, pWkSheet);

                        //DateTime pDate = DateTime.Now;
                        ////String pFileName = FileName_In + "\\CAD Neutral Data Set_" + pDate.ToString("ddMMMyyyy").ToUpper() + ".xlsx";
                        //String pFileName = FileName_In + "\\CAD Neutral Data Set_RevA.xlsx";

                        //EXCEL.XlSaveAsAccessMode pAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive;
                        //pWkbOrg.SaveAs(pFileName, mobjMissing, mobjMissing,
                        //                    mobjMissing, mobjMissing, mobjMissing, pAccessMode,
                        //                    mobjMissing, mobjMissing, mobjMissing,
                        //                    mobjMissing, mobjMissing);

                        pApp.Visible = Visible_Status_In;
                        if (!Visible_Status_In)
                        {
                            pApp.DisplayAlerts = false;
                            pWkbOrg.Close();
                            pWkbOrg = null;
                            pApp = null;
                        }

                        MessageBox.Show("Data have been imported successfully from '" + Path.GetFileName(FileName_In) + "'", "Data Import from Dataset", MessageBoxButtons.OK);
                    }
                }
                catch
                {
                }
            }

            private Boolean Read_Parameter_Complete_Assy(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
            //============================================================================================
            {
                Boolean pVal = false; 
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    string pUnitSystem = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }
                    if (((EXCEL.Range)pExcelCellRange.Cells[4, 6]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[4, 6]).Value2) != "")
                    {
                        pUnitSystem = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[4, 6]).Value2);
                        pUnitSystem = pUnitSystem.Substring(pUnitSystem.LastIndexOf(":") + 1).Trim(); 
                        Project_In.PNR.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pUnitSystem);
                        pVal = true;
                    }

                    if (pUnitSystem != "English" && pUnitSystem != "Metric")
                    {
                        MessageBox.Show("CAD Neutral Data Set File Version Mismatched.", "File Version Mismatch!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        pVal = false;
                        return pVal;
                    }

                    for (int i = 3; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                case "SalesOrder.Customer.Name":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Project_In.SOL.Customer.Name = Convert.ToString(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "SalesOrder.Customer.OrderNo":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Project_In.SOL.Customer.OrderNo = Convert.ToString(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "SalesOrder.Customer.MachineName":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Project_In.SOL.Customer.MachineName = Convert.ToString(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "SalesOrder.No":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Project_In.SOL.SONo = Convert.ToString(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "SalesOrder.RelatedNo":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Project_In.SOL.RelatedNo = Convert.ToString(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "PNR.No":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Project_In.PNR.No = Convert.ToString(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "Bearing.Design":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Design = (clsRadB.eDesign)Enum.Parse(typeof(clsRadB.eDesign), WorkSheet_In.Cells[i, 4].value.ToString());
                                    }
                                    break;

                                case "Bearing.SplitConfig":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        String pSplitConfig = "";
                                        pSplitConfig = WorkSheet_In.Cells[i, 4].value;
                                        if (pSplitConfig == "Y")
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SplitConfig = true;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SplitConfig = false;
                                        }
                                    }
                                    break;

                                //case "EndPlate[0].Type":
                                //    if (WorkSheet_In.Cells[i, 4].value != null)
                                //    {
                                //        ((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].Type = (clsEndPlate.eType)Enum.Parse(typeof(clsEndPlate.eType), WorkSheet_In.Cells[i, 4].value.ToString().Replace(" ", "_"));
                                //    }
                                //    break;

                                //case "EndPlate[1].Type":
                                //    if (WorkSheet_In.Cells[i, 4].value != null)
                                //    {
                                //        ((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].Type = (clsEndPlate.eType)Enum.Parse(typeof(clsEndPlate.eType), WorkSheet_In.Cells[i, 4].value.ToString().Replace(" ", "_"));
                                //    }
                                //    break;

                                case "OpCond.Speed":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).OpCond.Speed = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "OpCond.Rot_Directionality":
                                    //WorkSheet_In.Cells[i, 4] = "";
                                    break;

                                case "OpCond.Radial_Load":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).OpCond.Radial_Load = Convert.ToDouble(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "OpCond.Radial_LoadAng_Casing_SL":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).OpCond.Radial_LoadAng_Casing_SL = Convert.ToDouble(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;


                                case "OpCond.OilSupply.Lube_Type":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply_Lube_Type = Convert.ToString(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "OpCond.OilSupply.Reqd_Flow":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                        {
                                            //Double pVal = WorkSheet_In.Cells[i, 4].value;
                                            ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply_Flow_Reqd = Project_In.PNR.Unit.CFac_LPM_MetToEng(Convert.ToDouble(WorkSheet_In.Cells[i, 4].value));
                                            ((clsJBearing)Project_In.PNR.Bearing).PerformData.FlowReqd = ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply.Flow_Reqd;
                                            ((clsJBearing)Project_In.PNR.Bearing).PerformData.FlowReqd_Unit = "LPM";

                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply_Flow_Reqd = Convert.ToDouble(WorkSheet_In.Cells[i, 4].value);
                                            ((clsJBearing)Project_In.PNR.Bearing).PerformData.FlowReqd = ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply.Flow_Reqd;
                                        }
                                    }
                                    break;

                                case "OpCond.OilSupply.Press":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply_Press = Convert.ToDouble(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "OpCond.OilSupply.Temp":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply_Temp = Convert.ToDouble(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "Bearing.PerformData.Power":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).PerformData.Power = Project_In.PNR.Unit.CFac_Power_MetToEng(Convert.ToDouble(WorkSheet_In.Cells[i, 4].value));
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).PerformData.Power = Convert.ToDouble(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.PerformData.TempRise":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).PerformData.TempRise = modMain.gProject.PNR.Unit.CFac_Temp_MetToEng(Convert.ToDouble(WorkSheet_In.Cells[i, 4].value));
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).PerformData.TempRise = Convert.ToDouble(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }
                return pVal;
            }

            private void Read_Parameter_Complete_Radial(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
            //==================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }
                    for (int i = 3; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                //....Material:
                                case "Bearing.Mat.Base":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        string pMat = WorkSheet_In.Cells[i, 4].value;
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.Base = modMain.ExtractPreData(pMat, ":");
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.WCode_Base = modMain.ExtractPostData(pMat, ":").Replace("WBM", "").Trim();
                                    }
                                    break;

                                //....Geometry:

                                //....Diameter:
                                case "Bearing.Mat.Lining":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.Lining = WorkSheet_In.Cells[i, 4].value;
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.WCode_Lining = WorkSheet_In.Cells[i, 6].value;
                                    }
                                    break;

                                case "Bearing.LiningT":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.LiningT = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.LiningT = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.OD()":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Double pOD_Range = 0;
                                        pOD_Range = WorkSheet_In.Cells[i, 4].value;

                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OD_Range[0] = pOD_Range;
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OD_Range[1] = pOD_Range;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OD_Range[0] = Project_In.PNR.Unit.CMet_Eng(pOD_Range);
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OD_Range[1] = Project_In.PNR.Unit.CMet_Eng(pOD_Range);
                                        }
                                    }
                                    break;


                                case "Bearing.PadBore()":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Double pPadBore_Range = 0;
                                        pPadBore_Range = WorkSheet_In.Cells[i, 4].value;

                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.PadBore_Range[0] = pPadBore_Range;
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.PadBore_Range[1] = pPadBore_Range;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.PadBore_Range[0] = Project_In.PNR.Unit.CMet_Eng(pPadBore_Range);
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.PadBore_Range[1] = Project_In.PNR.Unit.CMet_Eng(pPadBore_Range);
                                        }
                                    }
                                    break;

                                case "Bearing.Bore()":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Double pBore_Range = 0;
                                        pBore_Range = WorkSheet_In.Cells[i, 4].value;

                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Bore_Range[0] = pBore_Range;
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Bore_Range[1] = pBore_Range;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Bore_Range[0] = Project_In.PNR.Unit.CMet_Eng(pBore_Range);
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Bore_Range[1] = Project_In.PNR.Unit.CMet_Eng(pBore_Range);
                                        }
                                    }
                                    break;

                                case "Bearing.DShaft()":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Double pDShaft_Range = 0;
                                        pDShaft_Range = WorkSheet_In.Cells[i, 4].value;

                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft_Range[0] = pDShaft_Range;
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft_Range[1] = pDShaft_Range;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft_Range[0] = Project_In.PNR.Unit.CMet_Eng(pDShaft_Range);
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft_Range[1] = Project_In.PNR.Unit.CMet_Eng(pDShaft_Range);
                                        }
                                    }
                                    break;

                                //....Length:
                                case "L_Available":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).L_Available = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).L_Available = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "L_Tot()":
                                    //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.L_Tot() * pConvF);
                                    break;                               

                                //....Pad:
                                case "Bearing.Pad.Type":                                   
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.LoadOrient = WorkSheet_In.Cells[i, 4].value;//(clsRadB.clsPad.eLoadOrient)Enum.Parse(typeof(clsRadB.clsPad.eLoadOrient), WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "Bearing.Pad.Count":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Count = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "Bearing.Pad.L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.L = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.L = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Pad.Angle":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Angle = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                //....Pad Pivot:
                                case "Bearing.Pad.Pivot.Offset":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Pivot_Offset = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Pad.Pivot.AngStart":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Pivot_AngStart_Casing_SL = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                //....Pad Thickness:
                                case "Bearing.Pad.T.Lead":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T_Lead = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T_Lead = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                     break;

                                case "Bearing.Pad.T.Pivot":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T_Pivot = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T_Pivot = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                    break;

                                case "Bearing.Pad.T.Trail":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T_Trail = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T_Trail = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                     break;

                                case "Bearing.Pad.Rfillet":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.RFillet = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.RFillet =Project_In.PNR.Unit.CMet_Eng( WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                //....Flexure Pivot:
                                case "Bearing.FlexurePivot.Web.T":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web_T = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web_T = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                case "Bearing.FlexurePivot.Web.RFillet":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web_RFillet = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web_RFillet = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.FlexurePivot.Web.H":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web_H = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web_H = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                case "Bearing.FlexurePivot.GapEDM":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).GapEDM = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).GapEDM = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                      break;

                                case "Bearing.MillRelief.D_PadRelief()":
                                    //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief.D_PadRelief() * pConvF);
                                    break;

                                case "Bearing.MillRelief.AxialSealGap[0]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.AxialSealGap[0] = WorkSheet_In.Cells[i, 4].value;
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.AxialSealGap[1] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.AxialSealGap[0] = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.AxialSealGap[1] = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.MillRelief.Exists":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        String pVal = "";
                                        pVal = WorkSheet_In.Cells[i, 4].value;
                                        if (pVal == "Y")
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief_Exists = true;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief_Exists = false;
                                        }
                                    }
                                    break;

                                case "Bearing.MillRelief.D":
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief_Exists)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.mMillRelief_D_Desig = WorkSheet_In.Cells[i, 6].value;
                                        //((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief.D() * pConvF);
                                    }
                                    break;

                                //....DESIGN DETAILS:
                                case "Bearing.L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.L = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.L = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Depth_EndPlate[0]":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.EndPlateCB[0].Depth = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.EndPlateCB[0].Depth = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                        
                                     }
                                     break;

                                case "Bearing.Depth_EndPlate[1]":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.EndPlateCB[1].Depth = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.EndPlateCB[1].Depth = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                    break;

                                case "EndPlate[0].L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].L = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].L = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "EndPlate[1].L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].L = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].L = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                //....Oil inlet:
                                case "Bearing.OilInlet.Count_MainOilSupply":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Count_MainOilSupply = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                //....Orifice:
                                case "Bearing.OilInlet.Orifice.Count":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        int pCount = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                        if (pCount > 1)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_Count = ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Count * 2;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_Count = ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Count;
                                        }
                                    }
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Count_MainOilSupply;
                                    break;

                                case "Bearing.OilInlet.Orifice.D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_D = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_D = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Orifice.StartPos":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Double pAng_Start_Pos = 0;
                                        pAng_Start_Pos = WorkSheet_In.Cells[i, 4].value;

                                        if (pAng_Start_Pos > 0)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_StartPos = clsRadB.clsOilInlet.eOrificeStartPos.Above;
                                        }
                                        else if (pAng_Start_Pos < 0)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_StartPos = clsRadB.clsOilInlet.eOrificeStartPos.Below;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_StartPos = clsRadB.clsOilInlet.eOrificeStartPos.On;
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Orifice.D_Cbore":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_CBore_D = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_CBore_D = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Orifice.Loc_Back":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_Loc_Back = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_Loc_Back = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Orifice.L":
                                    //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Calc_Orifice_L() * pConvF);
                                    break;

                                //....Annulus:     

                                case "Bearing.OilInlet.Annulus.Exists":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        String pAnnulus_Exists = "";
                                        pAnnulus_Exists = WorkSheet_In.Cells[i, 4].value;
                                        if (pAnnulus_Exists == "Y")
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Exists = true;

                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Exists = false;
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Annulus.Area_Reqd":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Area = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Area = modMain.gProject.PNR.Unit.CFac_Area_MetToEng(Convert.ToDouble(WorkSheet_In.Cells[i, 4].value));
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Annulus.Wid":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Wid = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Wid = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Annulus.Depth":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Depth = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Depth = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Annulus.D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_D = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_D = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.OilInlet.Annulus.Loc_Back":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Loc_Back = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_Loc_Back = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                     break;

                                //....Anti-Rotation Pin:      

                                //....Hardware:  
                                case "Bearing.ARP.Spec.Unit.System":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), WorkSheet_In.Cells[i, 4].value);
                                     }
                                    break;

                                case "Bearing.ARP.Spec.Type":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec_Type = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.ARP.Spec.Mat":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec_Mat = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.ARP.Spec.D":
                                    if (WorkSheet_In.Cells[i, 6].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec_D_Desig = WorkSheet_In.Cells[i, 6].value;
                                    }  
                                    //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.D() * pConvF);
                                    break;

                                case "Bearing.ARP.Spec.L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec_L = WorkSheet_In.Cells[i, 4].value;
                                        //if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.English)
                                        //{
                                        //    ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec_L = WorkSheet_In.Cells[i, 4].value;
                                        //}
                                        //else
                                        //{
                                        //    ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec_L = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.CMet_Eng( WorkSheet_In.Cells[i, 4].value);
                                        //}

                                    }
                                     break;

                                case "Bearing.ARP.PN":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.PN;
                                     }
                                    break;

                                case "Bearing.ARP.Hole.Depth_Low":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Hole_Depth_Low = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Hole_Depth_Low = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.ARP.Stickout":
                                    //Double pL = 0.0;
                                    //if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    //{
                                    //    pL = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.L / pConvF;
                                    //}
                                    //else
                                    //{
                                    //    pL = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.L;
                                    //}
                                    //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Stickout(pL) * pConvF);
                                    break;

                                case "Bearing.ARP.Loc_Back":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Loc_Back = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Loc_Back = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.ARP.Ang_Casing_SL":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Ang_Casing_SL = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.ARP.InsertedOn":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.InsertedOn = (clsRadB.clsARP.eInsertedOn)Enum.Parse(typeof(clsRadB.clsARP.eInsertedOn), WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "Bearing.ARP.Offset":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Offset = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Offset = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                case "Bearing.ARP.Offset_Direction":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Offset_Direction = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.ARP.Angle_Horz":
                                    ////WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Ang_Horz();
                                    break;


                                //....S/L Hardware:      

                                //....Screw:  

                                case "Bearing.SL.Screw.Spec.Unit.System":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), WorkSheet_In.Cells[i, 4].value);
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), WorkSheet_In.Cells[i, 4].value);
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), WorkSheet_In.Cells[i, 4].value);
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), WorkSheet_In.Cells[i, 4].value);

                                    }
                                    break;

                                case "Bearing.SL.Screw.Spec.Type":
                                    //String pSpec_Type = "";
                                    //if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    //{
                                    //    pSpec_Type = "Antigo ISO Metric Profile";
                                    //}
                                    //else
                                    //{
                                    //    pSpec_Type = "ANSI Unified Screw Threads";
                                    //}
                                    //WorkSheet_In.Cells[i, 4] = pSpec_Type;//((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Type;
                                    break;

                                case "Bearing.SL.Screw.Spec.Mat":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec_Mat = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.SL.Screw.D":
                                    if (WorkSheet_In.Cells[i, 6].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec_D_Desig = WorkSheet_In.Cells[i, 6].value;
                                    }
                                    //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.D() * pConvF);
                                    break;

                                case "Bearing.SL.Screw.Spec.Pitch":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec_Pitch = WorkSheet_In.Cells[i, 4].value;
                                        //if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        //{
                                        //    ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec_Pitch = WorkSheet_In.Cells[i, 4].value;
                                        //}
                                        //else
                                        //{
                                        //    ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec_Pitch = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        //}
                                    }
                                    break;

                                case "Bearing.SL.Screw.Spec.L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec_L = WorkSheet_In.Cells[i, 4].value;
                                        //if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        //{
                                        //    ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec_L = WorkSheet_In.Cells[i, 4].value;
                                        //}
                                        //else
                                        //{
                                        //    ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec_L = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        //}
                                    }
                                    break;

                                case "Bearing.SL.Screw.PN":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.PN = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.SL.Screw.Hole.CBore.D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_CBore_D = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_CBore_D = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.SL.Screw.Hole.CBore.D_Drill":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_D_Drill = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_D_Drill = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                     break;

                                case "Bearing.SL.Screw.Hole.CBore.Depth":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_CBore_Depth = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_CBore_Depth = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     
                                     break;

                                case "Bearing.SL.Screw.Hole.Depth.TapDrill":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_Depth_TapDrill = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_Depth_TapDrill = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                case "Bearing.SL.Screw.Hole.Depth.Tap":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_Depth_Tap = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_Depth_Tap = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                case "Bearing.SL.Screw.Hole.Depth.Engagement":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {                                        
                                         if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_Depth_Engagement = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole_Depth_Engagement = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;


                                //....Left Location:     

                                case "Bearing.SL.LScrew.Center":
                                      if (WorkSheet_In.Cells[i, 4].value != null)
                                      {
                                          if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LScrew_Center = WorkSheet_In.Cells[i, 4].value;
                                          }
                                          else
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LScrew_Center = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                          }
                                      }
                                      break;

                                case "Bearing.SL.LScrew.Back":
                                      if (WorkSheet_In.Cells[i, 4].value != null)
                                      {
                                          if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LScrew_Back = WorkSheet_In.Cells[i, 4].value;
                                          }
                                          else
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LScrew_Back = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                          }
                                      }
                                      break;

                                //....Right Location:     

                                case "Bearing.SL.RScrew.Center":
                                      if (WorkSheet_In.Cells[i, 4].value != null)
                                      {
                                          if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RScrew_Center = WorkSheet_In.Cells[i, 4].value;
                                          }
                                          else
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RScrew_Center = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                          }
                                      }
                                      break;

                                case "Bearing.SL.RScrew.Back":
                                      if (WorkSheet_In.Cells[i, 4].value != null)
                                      {
                                          if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RScrew_Back = WorkSheet_In.Cells[i, 4].value;
                                          }
                                          else
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RScrew_Back = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                          }
                                      }
                                      break;

                                //....Dowel:      

                                case "Bearing.SL.Dowel.Spec.Type":
                                       if (WorkSheet_In.Cells[i, 4].value != null)
                                       {
                                           ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec_Type = WorkSheet_In.Cells[i, 4].value;
                                       }
                                       break;

                                case "Bearing.SL.Dowel.Spec.Mat":
                                      if (WorkSheet_In.Cells[i, 4].value != null)
                                      {
                                          ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec_Mat = WorkSheet_In.Cells[i, 4].value;
                                      }
                                      break;

                                case "Bearing.SL.Dowel.D":
                                      if (WorkSheet_In.Cells[i, 6].value != null)
                                      {
                                          ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec_D_Desig = WorkSheet_In.Cells[i, 6].value;
                                      }
                                    //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.D() * pConvF);
                                    break;

                                case "Bearing.SL.Dowel.Spec.L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec_L = WorkSheet_In.Cells[i, 4].value;
                                        //if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        //{
                                        //    ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec_L = WorkSheet_In.Cells[i, 4].value;
                                        //}
                                        //else
                                        //{
                                        //    ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec_L = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        //}
                                        
                                    }
                                    break;

                                case "Bearing.SL.Dowel.PN":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.PN = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.SL.Dowel.Hole.Depth_Up":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Hole_Depth_Up = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Hole_Depth_Up = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.SL.Dowel.Hole.Depth_Low":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Hole_Depth_Low = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Hole_Depth_Low = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;


                                //....Left Location:     

                                case "Bearing.SL.Ldowel_Loc.Center":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LDowel_Loc_Center = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LDowel_Loc_Center = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                case "Bearing.SL.Ldowel_Loc.Back":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LDowel_Loc_Back = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LDowel_Loc_Back = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                //....Right Location:      

                                case "Bearing.SL.Rdowel_Loc.Center":
                                     if (WorkSheet_In.Cells[i, 4].value != null)
                                     {
                                         if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RDowel_Loc_Center = WorkSheet_In.Cells[i, 4].value;
                                         }
                                         else
                                         {
                                             ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RDowel_Loc_Center = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                         }
                                     }
                                     break;

                                case "Bearing.SL.Rdowel_Loc.Back":
                                      if (WorkSheet_In.Cells[i, 4].value != null)
                                      {
                                          if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RDowel_Loc_Back = WorkSheet_In.Cells[i, 4].value;
                                          }
                                          else
                                          {
                                              ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RDowel_Loc_Back = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                          }
                                      }
                                      break;
                            }
                        }
                    }
                }
                catch
                {
                }

            }

            private void Read_Parameter_Complete_Mounting(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
            //================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }
                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                case "Bearing.Mount_Bolting":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        string pVal = Convert.ToString(WorkSheet_In.Cells[i, 4].value);
                                        if (pVal == "Both")
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Bolting = true;
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Bolting = true;
                                        }
                                        else if (pVal == "Front")
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Bolting = true;
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Bolting = false;
                                        }
                                        else if (pVal == "Back")
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Bolting = false;
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Bolting = true;
                                        }
                                        //((clsJBearing)Project_In.PNR.Bearing).RadB.Mount.Bolting = (clsBearing_Radial_FP.eBolting)Enum.Parse(typeof(clsBearing_Radial_FP.eBolting), WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "Bearing.EndPlate[0].OD":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].OD = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].OD = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }                                
                                    }
                                    break;

                                case "Bearing.TWall_CB_EndPlate(0)":
                                    //if (WorkSheet_In.Cells[i, 4].value != null)
                                    //{
                                    //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.TWall_CB_EndPlate(0) * pConvF);
                                    //}
                                    break;
                         

                                case "Bearing.Mount.BC[0].D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].DBC = WorkSheet_In.Cells[i, 4].value; 
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].DBC = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value); 
                                        }                                
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Type":
                                    //String pSpec_Type = "";
                                    //if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    //{
                                    //    pSpec_Type = "Antigo ISO Metric Profile";
                                    //}
                                    //else
                                    //{
                                    //    pSpec_Type = "ANSI Unified Screw Threads";
                                    //}
                                    ////WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.Type;
                                    //WorkSheet_In.Cells[i, 4] = pSpec_Type;
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Mat":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        string pMat = WorkSheet_In.Cells[i, 4].value;
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec_Mat = modMain.ExtractPreData(pMat, ":");
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.WCode_Base = modMain.ExtractPostData(pMat, ":").Replace("WBM", "").Trim();
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.D":
                                    //if (WorkSheet_In.Cells[i, 4].value != null)
                                    //{
                                    //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.D() * pConvF);
                                    //}
                                    if (WorkSheet_In.Cells[i, 6].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec_D_Desig = WorkSheet_In.Cells[i, 6].value;
                                    }  
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Pitch":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec_Pitch = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec_L = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].Count":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                                                             
                                        int pCount = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count - 1;
                                        Double[] pMount_HolesAngBet = new Double[pCount];
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet = pMount_HolesAngBet;
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngStart":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngStart = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[0]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 1)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[1]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 2)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[1] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[2]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 3)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[2] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[3]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 4)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[3] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[4]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 5)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[4] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[5]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 6)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[5] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[6]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 7)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[6] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Mounting.Type":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec_Type = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.D_Drill":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_D_Drill = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_D_Drill = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.CBore.D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_CBore_D = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_CBore_D = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.CBore.Depth":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_CBore_Depth = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_CBore_Depth = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.TapDrill":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_Depth_TapDrill = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_Depth_TapDrill = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.Tap":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_Depth_Tap = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_Depth_Tap = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.Engagement":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_Depth_Engagement = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole_Depth_Engagement = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;


                                case "Bearing.EndPlate[1].OD":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].OD = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].OD = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.TWall_CB_EndPlate(1)":
                                    //if (WorkSheet_In.Cells[i, 4].value != null)
                                    //{
                                    //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.TWall_CB_EndPlate(0) * pConvF);
                                    //}
                                    break;


                                case "Bearing.Mount.BC[1].D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (Project_In.PNR.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].DBC = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].DBC = Project_In.PNR.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }                                       
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Type":
                                    //String pSpec_Type = "";
                                    //if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    //{
                                    //    pSpec_Type = "Antigo ISO Metric Profile";
                                    //}
                                    //else
                                    //{
                                    //    pSpec_Type = "ANSI Unified Screw Threads";
                                    //}
                                    ////WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.Type;
                                    //WorkSheet_In.Cells[i, 4] = pSpec_Type;
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Mat":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        string pMat = WorkSheet_In.Cells[i, 4].value;
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec_Mat = modMain.ExtractPreData(pMat, ":");
                                        ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.WCode_Base = modMain.ExtractPostData(pMat, ":").Replace("WBM", "").Trim();
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.D":
                                    //if (WorkSheet_In.Cells[i, 4].value != null)
                                    //{
                                    //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.D() * pConvF);
                                    //}
                                    if (WorkSheet_In.Cells[i, 6].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec_D_Desig = WorkSheet_In.Cells[i, 6].value;
                                    }  
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Pitch":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec_Pitch = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec_L = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].Count":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);

                                        int pCount = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count - 1;
                                        Double[] pMount_HolesAngBet = new Double[pCount];
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet = pMount_HolesAngBet;
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngStart":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngStart = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[0]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 1)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[1]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 2)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[1] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[2]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 3)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[2] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[3]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 4)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[3] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[4]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 5)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[4] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[5]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 6)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[5] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[6]":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 7)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[6] = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Mounting.Type":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec_Type = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.D_Drill":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_D_Drill = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_D_Drill = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.CBore.D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_CBore_D = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_CBore_D = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.CBore.Depth":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_CBore_Depth = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_CBore_Depth = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.TapDrill":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_Depth_TapDrill = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_Depth_TapDrill = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.Tap":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_Depth_Tap = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_Depth_Tap = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.Engagement":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_Depth_Min_Engagement = WorkSheet_In.Cells[i, 4].value;
                                        }
                                        else
                                        {
                                            ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole_Depth_Engagement = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.CMet_Eng(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;
                            }   

                        }
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < ((clsJBearing)Project_In.PNR.Bearing).Mount[j].Count-1; k++)
                        {
                            if (((clsJBearing)Project_In.PNR.Bearing).Mount[j].AngBet[0] == ((clsJBearing)Project_In.PNR.Bearing).Mount[j].AngBet[k])
                            {
                                ((clsJBearing)Project_In.PNR.Bearing).Mount[j].EquiSpaced = true;
                            }
                            else
                            {
                                ((clsJBearing)Project_In.PNR.Bearing).Mount[j].EquiSpaced = false;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            private void Read_Parameter_Complete_Seal_Front(clsProject Project_In, clsEndPlate EndPlate_In, EXCEL.Worksheet WorkSheet_In)
            //====================================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }

                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {

                                case "EndPlate[0].Mat.Base":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        string pMat = WorkSheet_In.Cells[i, 4].value;
                                        EndPlate_In.Mat.Base = modMain.ExtractPreData(pMat, ":");
                                        EndPlate_In.Mat.WCode_Base = modMain.ExtractPostData(pMat, ":").Replace("WBM", "").Trim();
                                    }
                                    break;

                                case "EndPlate[0].Mat.Lining":
                                    if (WorkSheet_In.Cells[i, 4].value != null && WorkSheet_In.Cells[i, 4].value != "")
                                    {
                                        EndPlate_In.Mat.Lining = WorkSheet_In.Cells[i, 4].value;
                                        EndPlate_In.Mat.WCode_Lining = WorkSheet_In.Cells[i, 6].value;
                                        EndPlate_In.Mat.LiningExists = true;
                                        //Seal_In.Mat.LiningExists = true;
                                    }
                                    else
                                    {
                                        EndPlate_In.Mat.LiningExists = false;
                                    }
                                    
                                    break;

                                case "EndPlate[0].LiningT":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {                                        
                                        EndPlate_In.Mat_LiningT = WorkSheet_In.Cells[i, 4].value / pConvF;

                                        if (EndPlate_In.Mat_LiningT > modMain.gcEPS)
                                        {
                                            EndPlate_In.Mat.LiningExists = true;
                                        }
                                    }
                                    break;

                                case "EndPlate[0].Design":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.Seal.Design = (clsEndPlate.clsSeal.eDesign)Enum.Parse(typeof(clsEndPlate.clsSeal.eDesign), WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "EndPlate[0].OD":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.OD = WorkSheet_In.Cells[i, 4].value / pConvF;
                                    }
                                    break;

                                case "EndPlate[0].DBore":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Double pDBore = WorkSheet_In.Cells[i, 4].value / pConvF;
                                        EndPlate_In.DBore_Range[0] = pDBore;
                                        EndPlate_In.DBore_Range[1] = pDBore;
                                    }
                                    break;

                                case "EndPlate[0].L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.L = WorkSheet_In.Cells[i, 4].value / pConvF;
                                    }
                                    break;

                                case "EndPlate[0].Blade.Count":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.Seal.Blade.Count = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "EndPlate[0].Blade.T":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count == 1)
                                        {
                                            EndPlate_In.Seal.Blade.T = WorkSheet_In.Cells[i, 4].value / pConvF;
                                        }
                                    }
                                    break;

                                case "EndPlate[0].Blade.AngTaper":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.Seal.Blade.AngTaper = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                //case "EndPlate[0].Blade_T":
                                //    if (Seal_In.Blade.Count > 1)
                                //    {
                                //        Seal_In.Blade.T = WorkSheet_In.Cells[i, 4].value / pConvF;  
                                //    }
                                //    break;

                                case "EndPlate[0].DrainHoles.Annulus.Ratio_L_H":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.Annulus_Ratio_L_H = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.Annulus.D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.Annulus_D = WorkSheet_In.Cells[i, 4].value / pConvF;
                                        }
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.D()":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        EndPlate_In.Seal.DrainHoles.D_Desig = WorkSheet_In.Cells[i, 6].value;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.Count":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.Count = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngBet":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.AngBet = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngStart_Horz":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.AngStart_Horz = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngExit":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.AngExit = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            private void Read_Parameter_Complete_Seal_Back(clsProject Project_In, clsEndPlate EndPlate_In, EXCEL.Worksheet WorkSheet_In)
            //===================================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }

                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {

                                case "EndPlate[1].Mat.Base":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        string pMat = WorkSheet_In.Cells[i, 4].value;
                                        EndPlate_In.Mat.Base = modMain.ExtractPreData(pMat, ":");
                                        EndPlate_In.Mat.WCode_Base = modMain.ExtractPostData(pMat, ":").Replace("WBM", "").Trim();
                                    }
                                    break;

                                case "EndPlate[1].Mat.Lining":                                    
                                    if (WorkSheet_In.Cells[i, 4].value != null && WorkSheet_In.Cells[i, 4].value != "")
                                    {
                                        EndPlate_In.Mat.Lining = WorkSheet_In.Cells[i, 4].value;
                                        EndPlate_In.Mat.WCode_Lining = WorkSheet_In.Cells[i, 6].value;
                                        EndPlate_In.Mat.LiningExists = true;
                                        //Seal_In.Mat.LiningExists = true;
                                    }
                                    else
                                    {
                                        EndPlate_In.Mat.LiningExists = false;
                                    }
                                    break;

                                case "EndPlate[1].LiningT":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.Mat_LiningT = WorkSheet_In.Cells[i, 4].value / pConvF;

                                        if (EndPlate_In.Mat_LiningT > modMain.gcEPS)
                                        {
                                            EndPlate_In.Mat.LiningExists = true;
                                        }
                                    }
                                    break;

                                case "EndPlate[1].Design":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.Seal.Design = (clsEndPlate.clsSeal.eDesign)Enum.Parse(typeof(clsEndPlate.clsSeal.eDesign), WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "EndPlate[1].OD":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.OD = WorkSheet_In.Cells[i, 4].value / pConvF;
                                    }
                                    break;

                                case "EndPlate[1].DBore":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        Double pDBore = WorkSheet_In.Cells[i, 4].value / pConvF;
                                        EndPlate_In.DBore_Range[0] = pDBore;
                                        EndPlate_In.DBore_Range[1] = pDBore;
                                    }
                                    break;

                                case "EndPlate[1].L":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.L = WorkSheet_In.Cells[i, 4].value / pConvF;
                                    }
                                    break;

                                case "EndPlate[1].Blade.Count":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.Seal.Blade.Count = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                    }
                                    break;

                                case "EndPlate[1].Blade.T":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count == 1)
                                        {
                                            EndPlate_In.Seal.Blade.T = WorkSheet_In.Cells[i, 4].value / pConvF;
                                        }
                                    }
                                    break;

                                case "EndPlate[1].Blade.AngTaper":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        EndPlate_In.Seal.Blade.AngTaper = WorkSheet_In.Cells[i, 4].value;
                                    }
                                    break;

                                //case "EndPlate[0].Blade_T":
                                //    if (Seal_In.Blade.Count > 1)
                                //    {
                                //        Seal_In.Blade.T = WorkSheet_In.Cells[i, 4].value / pConvF;  
                                //    }
                                //    break;

                                case "EndPlate[1].DrainHoles.Annulus.Ratio_L_H":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.Annulus_Ratio_L_H = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.Annulus.D":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.Annulus_D = WorkSheet_In.Cells[i, 4].value / pConvF;
                                        }
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.D()":
                                    //if (Seal_In.Blade.Count > 1)
                                    //{
                                    //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DrainHoles.D() * pConvF);
                                    //}

                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        EndPlate_In.Seal.DrainHoles.D_Desig = WorkSheet_In.Cells[i, 6].value;
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.Count":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.Count = Convert.ToInt32(WorkSheet_In.Cells[i, 4].value);
                                        }
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.AngBet":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.AngBet = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.AngStart_Horz":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.AngStart_Horz = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.AngExit":
                                    if (WorkSheet_In.Cells[i, 4].value != null)
                                    {
                                        if (EndPlate_In.Seal.Blade.Count > 1)
                                        {
                                            EndPlate_In.Seal.DrainHoles.AngExit = WorkSheet_In.Cells[i, 4].value;
                                        }
                                    }
                                    break;

                            }
                        }
                    }
                }
                catch
                {
                }
            }

            #endregion


            #region "OUTPUT DATA:"

            public void Write_Parameter_Complete(clsProject Project_In, string FileName_In, Boolean Visible_Status_In)
            //========================================================================================================
            {
                try
                {
                    object mobjMissing = Missing.Value;              //....Missing object.
                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();


                    //....Open Original WorkBook.
                    EXCEL.Workbook pWkbOrg = null;

                    pWkbOrg = pApp.Workbooks.Open(modMain.gFiles.FileTitle_Template_EXCEL_Parameter_Complete, mobjMissing, false,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing);

                    //....Open WorkSheet - 'Complete ASSY'            
                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Complete Assy"];
                    Write_Parameter_Complete_Assy(Project_In, pWkSheet);

                    //....Open WorkSheet - 'Radial Bearing'            
                    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Radial Bearing"];
                    Write_Parameter_Complete_Radial(Project_In, pWkSheet);

                    //....Open WorkSheet - 'Mounting'    
                    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Mounting"];
                    Writer_Parameter_Complete_Mounting(Project_In, pWkSheet);

                    ////....EndPlate: Seal
                    //clsSeal[] mEndSeal = new clsSeal[2];
                    //for (int i = 0; i < 2; i++)
                    //{
                    //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                    //    {
                    //        mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                    //    }
                    //}


                    ////....Open WorkSheet - 'Front Config - Seal' 
                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    //{
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Seal"];
                        Write_Parameter_Complete_Seal_Front(Project_In,((clsJBearing)Project_In.PNR.Bearing).EndPlate[0], pWkSheet);
                    //}
                    //else
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Seal"];
                    //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                    //}

                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    //{
                        //....Open WorkSheet - 'Back Config - Seal'    
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Seal"];
                        Write_Parameter_Complete_Seal_Back(Project_In,((clsJBearing)Project_In.PNR.Bearing).EndPlate[1], pWkSheet);
                    //}
                    //else
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Seal"];
                    //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                    //}

                    ////.............
                    ////....EndPlate: Thurst Bearing
                    //clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];
                    //for (int i = 0; i < 2; i++)
                    //{
                    //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
                    //    {
                    //        mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(modMain.gProject.Product.EndPlate[i])).Clone();
                    //    }
                    //}

                    ////....Open WorkSheet - 'Front TL Thurst Bearing' 
                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Thurst Bearing TL"];
                    //    Write_Parameter_Complete_Thrust_Front(modMain.gProject, mEndTB[0], pWkSheet);
                    //}
                    //else
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Thurst Bearing TL"];
                    //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                    //}

                    ////....Open WorkSheet - 'Back TL Thurst Bearing' 
                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Thurst Bearing TL"];
                    //    Write_Parameter_Complete_Thrust_Back(modMain.gProject, mEndTB[1], pWkSheet);
                    //}
                    //else
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Thurst Bearing TL"];
                    //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                    //}

                    //..............

                    //pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Accessories"];
                    //Write_Parameter_Complete_Accessories(modMain.gProject, modMain.gProject.Product.Accessories, pWkSheet);
               

                    DateTime pDate = DateTime.Now;
                    //String pFileName = FileName_In + "\\CAD Neutral Data Set_" + pDate.ToString("ddMMMyyyy").ToUpper() + ".xlsx";
                    String pFileName = FileName_In + "\\CAD Neutral Data Set_RevA.xlsx";

                    EXCEL.XlSaveAsAccessMode pAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive;
                    pWkbOrg.SaveAs(pFileName, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing, mobjMissing, pAccessMode,
                                        mobjMissing, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing);

                    pApp.Visible = Visible_Status_In;
                    if (!Visible_Status_In)
                    {
                        pWkbOrg.Close();
                        pWkbOrg = null;
                        pApp = null;
                    }
                }
                catch
                {
                }
            }

            private void Write_Parameter_Complete_Assy(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
            //======================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";

                    WorkSheet_In.Cells[4, 6] = "Unit System: " + Project_In.PNR.Unit.System.ToString().Trim();

                    for (int i = 3; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                case "SalesOrder.Customer.Name":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SOL.Customer.Name;
                                    break;

                                case "SalesOrder.Customer.OrderNo":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SOL.Customer.OrderNo;
                                    break;

                                case "SalesOrder.Customer.MachineName":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SOL.Customer.MachineName;
                                    break;

                                case "SalesOrder.No":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SOL.SONo;
                                    break;

                                case "SalesOrder.RelatedNo":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SOL.RelatedNo;
                                    break;

                                case "PNR.No":
                                    WorkSheet_In.Cells[i, 4] = Project_In.PNR.No;
                                    break;

                                case "Bearing.Design":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.Design.ToString();
                                    break;

                                case "Bearing.SplitConfig":
                                    String pSplitConfig = "";
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.SplitConfig)
                                    {
                                        pSplitConfig = "Y";
                                    }
                                    else
                                    {
                                        pSplitConfig = "N";
                                    }
                                    WorkSheet_In.Cells[i, 4] = pSplitConfig;
                                    break;

                                //case "Bearing.DShaft_Range[0], Bearing.DShaft_Range[1]":
                                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //    {
                                //        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft_Range[0] * pConv_InchToMM) + ", " +
                                //                                  modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft_Range[1] * pConv_InchToMM);
                                //    }
                                //    else
                                //    {
                                //        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft_Range[0]) + ", " +
                                //                                    modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft_Range[1]);
                                //    }
                                //    break;

                                case "EndPlate[0].Type":
                                    WorkSheet_In.Cells[i, 4] = "Seal"; //((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].Type.ToString().Replace("_", " ");
                                    break;

                                case "EndPlate[1].Type":
                                    WorkSheet_In.Cells[i, 4] = "Seal"; //((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].Type.ToString().Replace("_", " ");
                                    break;

                                case "OpCond.Speed":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).OpCond.Speed;
                                    break;

                                case "OpCond.Rot_Directionality":
                                    WorkSheet_In.Cells[i, 4] = "";
                                    break;

                                case "OpCond.Radial_Load":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).OpCond.Radial_Load;
                                    break;

                                case "OpCond.Radial_LoadAng_Casing_SL":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).OpCond.Radial_LoadAng_Casing_SL;
                                    break;

                                //case "OpCond.Thrust_Load_Range[0]":
                                //    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
                                //    {
                                //        WorkSheet_In.Cells[i, 4] = OpCond_In.Thrust_Load_Range[0];
                                //    }
                                //    break;

                                //case "OpCond.Thrust_Load_Range[1]":
                                //    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                                //    {
                                //        WorkSheet_In.Cells[i, 4] = OpCond_In.Thrust_Load_Range[1];
                                //    }
                                //    break;

                                case "OpCond.OilSupply.Lube_Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply.Lube_Type;
                                    break;

                                case "OpCond.OilSupply.Reqd_Flow":
                                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        //WorkSheet_In.Cells[i, 4] = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CFac_GPM_EngToMet(((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply.Flow_Reqd), "#0.00");
                                        WorkSheet_In.Cells[i, 4] = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CFac_GPM_EngToMet(((clsJBearing)Project_In.PNR.Bearing).PerformData.FlowReqd_Unit, ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply.Flow_Reqd), "#0.00");
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply.Flow_Reqd;
                                    }
                                    break;

                                case "OpCond.OilSupply.Press":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply.Press;
                                    break;

                                case "OpCond.OilSupply.Temp":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).OpCond.OilSupply.Temp;
                                    break;

                                case "Bearing.PerformData.Power":
                                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CFac_Power_EngToMet(((clsJBearing)Project_In.PNR.Bearing).PerformData.Power), "##0.00"); ;
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).PerformData.Power;
                                    }
                                    break;

                                case "Bearing.PerformData.TempRise":
                                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CFac_Temp_EngToMet(((clsJBearing)Project_In.PNR.Bearing).PerformData.TempRise), "#0.0");
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).PerformData.TempRise;
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }
            }


            private void Write_Parameter_Complete_Radial(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
            //================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    Double pConvF_ARP = 1;
                    Double pConvF_SL = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }
                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF_ARP = 25.4;
                    }
                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF_SL = 25.4;
                    }

                    for (int i = 3; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                //....Material:
                                case "Bearing.Mat.Base":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.Base + ": WBM " + ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.WCode.Base; 
                                    break;

                                //....Geometry:

                                //....Diameter:
                                case "Bearing.Mat.Lining":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.Lining;
                                    WorkSheet_In.Cells[i, 6] = ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.WCode.Lining;
                                    break;

                                case "Bearing.LiningT":                                   
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.LiningT * pConvF);
                                    break;

                                case "Bearing.OD()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OD()* pConvF);
                                    break;

                                case "Bearing.PadBore()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.PadBore() * pConvF);
                                    break;

                                case "Bearing.Bore()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.Bore() * pConvF);
                                    break;

                                case "Bearing.DShaft()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.DShaft() * pConvF);
                                    break;

                                //....Length:
                                case "L_Available":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).L_Available * pConvF);
                                    break;

                                case "L_Tot()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).L_Tot() * pConvF);
                                    break;

                                //....Pad:
                                case "Bearing.Pad.Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.LoadOrient.ToString();
                                    break;

                                case "Bearing.Pad.Count":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Count;
                                    break;

                                case "Bearing.Pad.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.L * pConvF);
                                    break;

                                case "Bearing.Pad.Angle":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Angle;
                                    break;

                                //....Pad Pivot:
                                case "Bearing.Pad.Pivot.Offset":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Pivot.Offset);
                                    break;

                                case "Bearing.Pad.Pivot.AngStart":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Pivot.AngStart_Casing_SL;
                                    break;

                                //....Pad Thickness:
                                case "Bearing.Pad.T.Lead":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T.Lead * pConvF);
                                    break;

                                case "Bearing.Pad.T.Pivot":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T.Pivot * pConvF);
                                    break;

                                case "Bearing.Pad.T.Trail":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.T.Trail * pConvF);
                                    break;

                                case "Bearing.Pad.Rfillet":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.RFillet * pConvF);
                                    break;

                                //....Flexure Pivot:
                                case "Bearing.FlexurePivot.Web.T":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web.T * pConvF);
                                    break;

                                case "Bearing.FlexurePivot.Web.RFillet":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web.RFillet * pConvF);
                                    break;

                                case "Bearing.FlexurePivot.Web.H":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).Web.H * pConvF);
                                    break;

                                case "Bearing.FlexurePivot.GapEDM":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsPivot.clsFP)((clsJBearing)Project_In.PNR.Bearing).RadB.Pivot).GapEDM * pConvF);
                                    break;


                                case "Bearing.MillRelief.D_PadRelief()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.PadRelief_D() * pConvF);
                                    break;

                                case "Bearing.MillRelief.AxialSealGap[0]":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.AxialSealGap[0] * pConvF);
                                    break;

                                case "Bearing.MillRelief.Exists":
                                    String pVal = "";
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief_Exists)
                                    {
                                        pVal = "Y";
                                    }
                                    else
                                    {
                                        pVal = "N";
                                    }
                                    WorkSheet_In.Cells[i, 4] = pVal;
                                    break;

                                case "Bearing.MillRelief.D":
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief_Exists)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief_D() * pConvF);
                                        WorkSheet_In.Cells[i, 6] = ((clsJBearing)Project_In.PNR.Bearing).RadB.MillRelief_D_Desig;
                                    }
                                    break;

                                //....DESIGN DETAILS:
                                case "Bearing.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.L * pConvF);
                                    break;

                                case "Bearing.Depth_EndPlate[0]":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.EndPlateCB[0].Depth * pConvF);
                                    break;

                                case "Bearing.Depth_EndPlate[1]":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.EndPlateCB[1].Depth * pConvF);
                                    break;

                                case "EndPlate[0].L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].L * pConvF);
                                    break;

                                case "EndPlate[1].L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].L * pConvF);
                                    break;

                                //....Oil inlet:
                                case "Bearing.OilInlet.Count_MainOilSupply":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Count_MainOilSupply;
                                    break;

                                //....Orifice:
                                case "Bearing.OilInlet.Orifice.Count":
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice.Count;
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Count_MainOilSupply;
                                    break;

                                case "Bearing.OilInlet.Orifice.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice.D * pConvF);
                                    break;

                                case "Bearing.OilInlet.Orifice.StartPos":

                                    Double pAng_Start_Pos = 0;
                                    int pPad_Count = ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Count;
                                    Double pPad_Angle = ((clsJBearing)Project_In.PNR.Bearing).RadB.Pad.Angle;
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice.StartPos == clsRadB.clsOilInlet.eOrificeStartPos.Below)
                                    {

                                        pAng_Start_Pos = -(360 / pPad_Count - pPad_Angle) / 2;
                                    }
                                    else if (((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice.StartPos == clsRadB.clsOilInlet.eOrificeStartPos.On)
                                    {

                                        pAng_Start_Pos = 0;
                                    }
                                    else if (((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice.StartPos == clsRadB.clsOilInlet.eOrificeStartPos.Above)
                                    {

                                        pAng_Start_Pos = (360 / pPad_Count - pPad_Angle) / 2;
                                    }

                                    WorkSheet_In.Cells[i, 4] = pAng_Start_Pos;
                                    break;

                                case "Bearing.OilInlet.Orifice.D_Cbore":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice.CBore_D * pConvF);
                                    break;

                                case "Bearing.OilInlet.Orifice.Loc_Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice.Loc_Back * pConvF);
                                    break;

                                case "Bearing.OilInlet.Orifice.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Orifice_L() * pConvF);
                                    break;

                                //....Annulus:     

                                case "Bearing.OilInlet.Annulus.Exists":
                                    String pAnnulus_Exists = "";
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.Exists)
                                    {
                                        pAnnulus_Exists = "Y";
                                    }
                                    else
                                    {
                                        pAnnulus_Exists = "N";
                                    }
                                    WorkSheet_In.Cells[i, 4] = pAnnulus_Exists;
                                    break;

                                case "Bearing.OilInlet.Annulus.Area_Reqd":                                    
                                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CFac_Area_EngToMet(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.Area));
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.Area);
                                    }
                                    break;

                                case "Bearing.OilInlet.Annulus.Wid":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.Wid * pConvF);
                                    break;

                                case "Bearing.OilInlet.Annulus.Depth":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.Depth * pConvF);
                                    break;

                                case "Bearing.OilInlet.Annulus.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.D * pConvF);
                                    break;

                                case "Bearing.OilInlet.Annulus.Loc_Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.Loc_Back * pConvF);
                                    break;

                                //case "Bearing.OilInlet.Annulus_V()":
                                //    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus_V(((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.D , ((clsJBearing)Project_In.PNR.Bearing).RadB.OilInlet.Annulus.Wid );
                                //    break;

                                //....Flange:      

                                //case "Bearing.Flange.Exists":
                                //    String pFlange_Exists = "";
                                //    if (((clsJBearing)Project_In.PNR.Bearing).RadB.Flange.Exists)
                                //    {
                                //        pFlange_Exists = "Y";
                                //    }
                                //    else
                                //    {
                                //        pFlange_Exists = "N";
                                //    }

                                //    WorkSheet_In.Cells[i, 4] = pFlange_Exists;
                                //    break;

                                //case "Bearing.Flange.D":
                                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.Flange.D * pConvF);
                                //    break;

                                //case "Bearing.Flange.Wid":
                                //    WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsJBearing)Project_In.PNR.Bearing).RadB.Flange.Wid * pConvF);
                                //    break;

                                //case "Bearing.Flange.DimStart_Front":
                                //    WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsJBearing)Project_In.PNR.Bearing).RadB.Flange.DimStart_Back * pConvF);
                                //    break;

                                //....Anti-Rotation Pin:      

                                //....Hardware:  
                                case "Bearing.ARP.Spec.Unit.System":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System.ToString();
                                    break;

                                case "Bearing.ARP.Spec.Type": 
                                    WorkSheet_In.Cells[i, 4] =((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Type;
                                    break;

                                case "Bearing.ARP.Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Mat;
                                    break;

                                case "Bearing.ARP.Spec.D":
                                    WorkSheet_In.Cells[i, 6] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.D_Desig;
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.D() * pConvF_ARP);
                                    break;

                                case "Bearing.ARP.Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.L);
                                    break;

                                case "Bearing.ARP.PN":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.PN;
                                    break;

                                case "Bearing.ARP.Hole.Depth_Low":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Hole.Depth_Low * pConvF_ARP);
                                    break;

                                case "Bearing.ARP.Stickout":
                                    Double pL = 0.0;
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pL = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.L / pConvF_ARP;
                                    }
                                    else
                                    {
                                        pL = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Dowel.Spec.L;
                                    }
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Stickout(pL) * pConvF_ARP);
                                    break;

                                case "Bearing.ARP.Loc_Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Loc_Back * pConvF);
                                    break;

                                case "Bearing.ARP.Ang_Casing_SL":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Ang_Casing_SL;
                                    break;

                                case "Bearing.ARP.InsertedOn":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.InsertedOn.ToString();
                                    break;

                                case "Bearing.ARP.Offset":
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Offset > modMain.gcEPS)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Offset * pConvF);
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = 0;
                                    }
                                    break;

                                case "Bearing.ARP.Offset_Direction":
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Offset > modMain.gcEPS)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Offset_Direction.ToString();
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = "None";
                                    }
                                    break;

                                case "Bearing.ARP.Angle_Horz":
                                    double pVal1 = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Ang_Horz();
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.ARP.Ang_Horz();
                                    break;


                                //....S/L Hardware:      

                                //....Screw:  

                                case "Bearing.SL.Screw.Spec.Unit.System":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System.ToString();
                                    break;

                                case "Bearing.SL.Screw.Spec.Type":
                                    String pSpec_Type = "";
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pSpec_Type = "Antigo ISO Metric Profile";
                                    }
                                    else
                                    {
                                        pSpec_Type = "ANSI Unified Screw Threads";
                                    }
                                    WorkSheet_In.Cells[i, 4] = pSpec_Type;//((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Type;
                                    break;

                                case "Bearing.SL.Screw.Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Mat;
                                    break;

                                case "Bearing.SL.Screw.D":
                                    WorkSheet_In.Cells[i, 6] = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.D_Desig;
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.D() * pConvF_SL);
                                    break;

                                case "Bearing.SL.Screw.Spec.Pitch":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Pitch);
                                    break;

                                case "Bearing.SL.Screw.Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.L);
                                    break;

                                case "Bearing.SL.Screw.PN":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.PN;
                                    break;

                                case "Bearing.SL.Screw.Hole.CBore.D":
                                    //String pD = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole.CBore.D);
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole.CBore.D * pConvF_SL);
                                    break;

                                case "Bearing.SL.Screw.Hole.CBore.D_Drill":
                                    //String pD_Drill = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole.D_Drill);
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole.D_Drill * pConvF_SL);
                                    break;

                                case "Bearing.SL.Screw.Hole.CBore.Depth":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole.CBore.Depth * pConvF_SL);
                                    break;

                                case "Bearing.SL.Screw.Hole.Depth.TapDrill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole.Depth.TapDrill * pConvF_SL);
                                    break;

                                case "Bearing.SL.Screw.Hole.Depth.Tap":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole.Depth.Tap * pConvF_SL);
                                    break;

                                case "Bearing.SL.Screw.Hole.Depth.Engagement":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Hole.Depth.Engagement * pConvF_SL);
                                    break;


                                //....Left Location:     

                                case "Bearing.SL.LScrew.Center":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LScrew.Center * pConvF);
                                    break;

                                case "Bearing.SL.LScrew.Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LScrew.Back * pConvF);
                                    break;

                                //....Right Location:     

                                case "Bearing.SL.RScrew.Center":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RScrew.Center * pConvF);
                                    break;

                                case "Bearing.SL.RScrew.Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RScrew.Back * pConvF);
                                    break;

                                //....Dowel:      

                                case "Bearing.SL.Dowel.Spec.Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.Type;
                                    break;

                                case "Bearing.SL.Dowel.Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.Mat;
                                    break;

                                case "Bearing.SL.Dowel.D":
                                    WorkSheet_In.Cells[i, 6] = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.D_Desig;
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.D() * pConvF_SL);
                                    break;

                                case "Bearing.SL.Dowel.Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Spec.L);
                                    break;

                                case "Bearing.SL.Dowel.PN":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.PN;
                                    break;

                                case "Bearing.SL.Dowel.Hole.Depth_Up":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Hole.Depth_Up * pConvF_SL);
                                    break;

                                case "Bearing.SL.Dowel.Hole.Depth_Low":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Dowel.Hole.Depth_Low * pConvF_SL);
                                    break;


                                //....Left Location:     

                                case "Bearing.SL.Ldowel_Loc.Center":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LDowel_Loc.Center * pConvF);
                                    break;

                                case "Bearing.SL.Ldowel_Loc.Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.LDowel_Loc.Back * pConvF);
                                    break;

                                //....Right Location:      

                                case "Bearing.SL.Rdowel_Loc.Center":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RDowel_Loc.Center * pConvF);
                                    break;

                                case "Bearing.SL.Rdowel_Loc.Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.SL.RDowel_Loc.Back * pConvF);
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }

            }


            private void Writer_Parameter_Complete_Mounting(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
            //=============================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    Double pConvF_SL = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }
                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF_SL = 25.4;
                    }
                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                case "Bearing.Mount_Bolting":
                                    WorkSheet_In.Cells[i, 4] = "Both";//((clsJBearing)Project_In.PNR.Bearing).RadB.Mount.Bolting.ToString();
                                    break;

                                case "Bearing.EndPlate[0].OD":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).EndPlate[0].OD * pConvF);
                                    break;

                                case "Bearing.TWall_CB_EndPlate(0)":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.EndPlateCB_TWall(0) * pConvF);
                                    break;

                                //....Front End Config:
                                //case "Bearing_In.TWall_BearingCB(0)":
                                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.TWall_BearingCB(0)*pConvF);
                                //    break;

                                case "Bearing.Mount.BC[0].D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].DBC * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Type":
                                    String pSpec_Type = "";
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pSpec_Type = "Antigo ISO Metric Profile";
                                    }
                                    else
                                    {
                                        pSpec_Type = "ANSI Unified Screw Threads";
                                    }
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.Type;
                                    WorkSheet_In.Cells[i, 4] = pSpec_Type;
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.Mat + ": WBM" + ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.WCode.Base;
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.D":
                                    WorkSheet_In.Cells[i, 6] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.D_Desig;
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.D() * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Pitch":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.Pitch );
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.L);                                   
                                    break;

                                case "Bearing.Mount.BC[0].Count"://Bearing.Mount.Screw[0].Hole.Count
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count;
                                    break;

                                case "Bearing.Mount.BC[0].AngStart":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngStart;
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[0]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.HolesAngOther[0];
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 1)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[1]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.HolesAngOther[1];
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[1];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 2)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[2]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.HolesAngOther[2];
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[2];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 3)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[3]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.HolesAngOther[3];
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[3];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 4)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[4]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.HolesAngOther[4];
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[4];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 5)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[5]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.HolesAngOther[5];
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[5];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 6)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[6]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.HolesAngOther[6];
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[6];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 7)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                
                                case "Bearing.Mount.Screw[0].Hole.Mounting.Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.Type;
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.D_Drill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole.D_Drill * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.CBore.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole.CBore.D * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.CBore.Depth":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole.CBore.Depth * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.TapDrill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole.Depth.TapDrill * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.Tap":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole.Depth.Tap * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.Engagement":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Hole.Depth.Engagement * pConvF_SL);
                                    break;

                                case "Bearing.EndPlate[1].OD":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].OD * pConvF);
                                    break;

                                case "Bearing.TWall_CB_EndPlate(1)":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.EndPlateCB_TWall(1) * pConvF);
                                    break;

                                //....Front End Config:
                                //case "Bearing_In.TWall_BearingCB(0)":
                                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).RadB.TWall_BearingCB(0)*pConvF);
                                //    break;

                                case "Bearing.Mount.BC[1].D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].DBC * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Type":
                                    pSpec_Type = "";
                                    if (((clsJBearing)Project_In.PNR.Bearing).RadB.SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pSpec_Type = "Antigo ISO Metric Profile";
                                    }
                                    else
                                    {
                                        pSpec_Type = "ANSI Unified Screw Threads";
                                    }
                                    //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[0].Screw.Spec.Type;
                                    WorkSheet_In.Cells[i, 4] = pSpec_Type;    
                                //WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec.Type;
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec.Mat + ": WBM" + ((clsJBearing)Project_In.PNR.Bearing).RadB.Mat.WCode.Base;
                                    break;

                                case "Bearing.Mount.Screw[1].D":
                                    WorkSheet_In.Cells[i, 6] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec.D_Desig;
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.D() * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Pitch":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec.Pitch );
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec.L );
                                    break;

                                case "Bearing.Mount.BC[1].Count":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count;
                                    break;

                                case "Bearing.Mount.BC[1].AngStart":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngStart;
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[0]":
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0];
                                    }
                                    else
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 1)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[1]":
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[1];
                                    }
                                    else
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 2)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[2]":
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[2];
                                    }
                                    else
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 3)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[3]":
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[3];
                                    }
                                    else
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 4)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[4]":
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[4];
                                    }
                                    else
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[0].Count > 5)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[5]":
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[5];
                                    }
                                    else
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 6)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[6]":
                                    if (!((clsJBearing)Project_In.PNR.Bearing).Mount[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[6];
                                    }
                                    else
                                    {
                                        if (((clsJBearing)Project_In.PNR.Bearing).Mount[1].Count > 7)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;
                            
                                case "Bearing.Mount.Screw[1].Hole.Mounting.Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Spec.Type;
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.D_Drill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole.D_Drill * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.CBore.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole.CBore.D * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.CBore.Depth":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole.CBore.Depth * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.TapDrill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole.Depth.TapDrill * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.Tap":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole.Depth.Tap * pConvF_SL);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.Engagement":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsJBearing)Project_In.PNR.Bearing).Mount[1].Screw.Hole.Depth.Engagement * pConvF_SL);
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            private void Write_Parameter_Complete_Seal_Front(clsProject Project_In, clsEndPlate EndPlate_In, EXCEL.Worksheet WorkSheet_In)
            //============================================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }

                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {

                                case "EndPlate[0].Mat.Base":
                                    WorkSheet_In.Cells[i, 4] = EndPlate_In.Mat.Base + ": WBM " +EndPlate_In.Mat.WCode.Base;
                                    break;

                                case "EndPlate[0].Mat.Lining":
                                    WorkSheet_In.Cells[i, 4] = EndPlate_In.Mat.Lining;
                                    WorkSheet_In.Cells[i, 6] = EndPlate_In.Mat.WCode.Lining;
                                    break;

                                case "EndPlate[0].LiningT":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Mat_LiningT * pConvF);
                                    break;

                                case "EndPlate[0].Design":
                                    WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.Design.ToString();
                                    break;

                                case "EndPlate[0].OD":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.OD * pConvF);
                                    break;

                                case "EndPlate[0].DBore":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.DBore() * pConvF);
                                    break;

                                case "EndPlate[0].L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.L * pConvF);
                                    break;

                                case "EndPlate[0].Blade.Count":
                                    WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.Blade.Count;
                                    break;

                                case "EndPlate[0].Blade.T":
                                    if (EndPlate_In.Seal.Blade.Count == 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.Blade.T * pConvF);
                                    }
                                    break;

                                case "EndPlate[0].Blade.AngTaper":
                                    if (EndPlate_In.Seal.Blade.Count == 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.Blade.AngTaper);
                                    }
                                    break;

                                case "EndPlate[0].Blade_T":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.Blade.T * pConvF);
                                    }                                    
                                    break;

                                case "EndPlate[0].DrainHoles.Annulus.Ratio_L_H":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.Annulus.Ratio_L_H;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.Annulus.D":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.DrainHoles.Annulus.D * pConvF);
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.D()":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.DrainHoles.D() * pConvF);
                                        WorkSheet_In.Cells[i, 6] = EndPlate_In.Seal.DrainHoles.D_Desig;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.Count":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.Count;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngBet":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.AngBet;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngStart_Horz":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.AngStart_Horz;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngExit":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.AngExit;
                                    }
                                    break;

                            }
                        }
                    }
                }
                catch
                {
                }
            }

            private void Write_Parameter_Complete_Seal_Back(clsProject Project_In, clsEndPlate EndPlate_In, EXCEL.Worksheet WorkSheet_In)
            //============================================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }

                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null && Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                case "EndPlate[1].Mat.Base":
                                    WorkSheet_In.Cells[i, 4] = EndPlate_In.Mat.Base + ": WBM " + EndPlate_In.Mat.WCode.Base;
                                    break;

                                case "EndPlate[1].Mat.Lining":
                                    WorkSheet_In.Cells[i, 4] = EndPlate_In.Mat.Lining;
                                    WorkSheet_In.Cells[i, 6] = EndPlate_In.Mat.WCode.Lining;
                                    break;

                                case "EndPlate[1].LiningT":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Mat_LiningT * pConvF);
                                    break;

                                case "EndPlate[1].Design":
                                    WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.Design.ToString();
                                    break;

                                case "EndPlate[1].OD":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.OD * pConvF);
                                    break;

                                case "EndPlate[1].DBore":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.DBore() * pConvF);
                                    break;

                                case "EndPlate[1].L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.L * pConvF);
                                    break;

                                case "EndPlate[1].Blade.Count":
                                    WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.Blade.Count;
                                    break;

                                case "EndPlate[1].Blade.T":
                                    if (((clsJBearing)Project_In.PNR.Bearing).EndPlate[1].Seal.Blade.Count == 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.Blade.T * pConvF);
                                    }
                                    break;

                                case "EndPlate[1].Blade.AngTaper":
                                    if (EndPlate_In.Seal.Blade.Count == 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.Blade.AngTaper);
                                    }
                                    break;

                                case "EndPlate[1].Blade_T":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.Blade.T * pConvF);
                                    }
                                    break;
                                    
                                case "EndPlate[1].DrainHoles.Annulus.Ratio_L_H":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.Annulus.Ratio_L_H;
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.Annulus.D":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.DrainHoles.Annulus.D * pConvF);
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.D()":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(EndPlate_In.Seal.DrainHoles.D() * pConvF);
                                        WorkSheet_In.Cells[i, 6] = EndPlate_In.Seal.DrainHoles.D_Desig;
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.Count":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.Count;
                                    }
                                    break;


                                case "EndPlate[1].DrainHoles.AngBet":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.AngBet;
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.AngStart_Horz":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.AngStart_Horz;
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.AngExit":
                                    if (EndPlate_In.Seal.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = EndPlate_In.Seal.DrainHoles.AngExit;
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }
            }


            #endregion

            private void UpdateAppConfig(String DataSource_In)
            //================================================
            {
                Configuration pConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // ....First Connection String
                // ........Because it's an EF connection string it's not a normal connection string
                // ........so we pull it into the EntityConnectionStringBuilder instead
                EntityConnectionStringBuilder pEFB = new EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings["BearingDBEntities"].ConnectionString);

                // ....Then we extract the actual underlying provider connection string
                SqlConnectionStringBuilder pSQB = new SqlConnectionStringBuilder(pEFB.ProviderConnectionString);

                // ....Now we can set the datasource
                pSQB.DataSource = DataSource_In;

                // ....Pop it back into the EntityConnectionStringBuilder 
                pEFB.ProviderConnectionString = pSQB.ConnectionString;

                // ....And update
                pConfig.ConnectionStrings.ConnectionStrings["BearingDBEntities"].ConnectionString = pEFB.ConnectionString;

                pConfig.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
       
            //---------------------------------------------------------------------------
            //                      UTILITY ROUTINES - END                              '
            //---------------------------------------------------------------------------
        #endregion


        #region "SESSION SAVE/RESTORE RELATED ROUTINES:"
            //-----------------------------------------

            #region "SAVE SESSION:"
            //--------------------
            public void Save_SessionData(clsProject Project_In)
            //=================================================
            {
                try
                {
                    string pFilePath = mFileName_BearingCAD.Remove(mFileName_BearingCAD.Length - 11);// mFileName_BearingCAD;

                    Boolean pProject =
                    Project_In.Serialize(pFilePath);

                    //....Merge Binary files.
                    Merge_ObjFiles(pFilePath);

                    //....Delete two Binary files.
                    Delete_ObjFiles(pFilePath);
                }
                catch (Exception pEXP)
                {

                }
            }


            private void Merge_ObjFiles(string FilePath_In)
            //=============================================
            {
                byte[] pHeader;
                byte[] buffer;
                int count = 0;
                string pFileHeader = null;
                FileStream OpenFile = null;

                string pFileName_Out = FilePath_In + ".BearingCAD";
                FileStream OutputFile = new FileStream(pFileName_Out, FileMode.Create, FileAccess.Write);

                for (int index = 1; index <= mcObjFile_Count; index++)
                {
                    string pFileName = FilePath_In + index + ".BearingCAD";

                    OpenFile = new FileStream(pFileName, FileMode.Open, FileAccess.Read, FileShare.Read);

                    //....Initialize the buffer by the total byte length of the file.
                    buffer = new byte[OpenFile.Length];

                    //....Read the file and store it into the buffer.
                    OpenFile.Read(buffer, 0, buffer.Length);
                    count = OpenFile.Read(buffer, 0, buffer.Length);

                    //....Create a header for each file.
                    pFileHeader = "BeginFile" + index + "," + buffer.Length.ToString();

                    //....Transfer the header string into bytes.
                    pHeader = Encoding.Default.GetBytes(pFileHeader);

                    //....Write the header info. into file.
                    OutputFile.Write(pHeader, 0, pHeader.Length);

                    //....Write a Linefeed into file for seperating header info and file info.
                    OutputFile.WriteByte(10); // linefeed

                    //....Write buffer data into file.
                    OutputFile.Write(buffer, 0, buffer.Length);
                    OpenFile.Close();
                }

                OutputFile.Close();
            }

            private void Delete_ObjFiles(string FilePath_In)
            //==========================================
            {
                string pFileName = null;

                for (int index = 1; index <= mcObjFile_Count; index++)
                {
                    pFileName = FilePath_In + index + ".BearingCAD";
                    File.Delete(pFileName);
                }
            }

            #endregion


            #region "RESTORE SESSION:"
            //------------------------

            public void Restore_SessionData(ref clsProject Project_In, string FilePath_In)
            //============================================================================
            {
                try
                {
                    Split_SessionFile();
                    Project_In = (clsProject)modMain.gProject.Deserialize(FilePath_In);                    
                    Delete_ObjFiles(FilePath_In);
                }
                catch (Exception pEXP)
                {

                }
            }

            private void Split_SessionFile()
            //==============================
            {
                string line = null;
                Int32 pLength = 0;
                int pIndex = 1;

                FileStream OpenFile = null;
                OpenFile = new FileStream(mFileName_BearingCAD, FileMode.Open, FileAccess.Read, FileShare.Read);

                while (OpenFile.Position != OpenFile.Length)
                {
                    line = null;
                    while (string.IsNullOrEmpty(line) && OpenFile.Position != OpenFile.Length)
                    {
                        //....Read the header info.
                        line = ReadLine(OpenFile);
                    }

                    if (!string.IsNullOrEmpty(line) && OpenFile.Position != OpenFile.Length)
                    {
                        //....Store the total byte length of the file stored into the header.
                        pLength = GetLength(line);
                    }
                    if (!string.IsNullOrEmpty(line))
                    {
                        //....Write bin files from the marged file.
                        Write_ObjFiles(OpenFile, pLength, pIndex);
                        pIndex++;
                    }
                }
                OpenFile.Close();
            }


            private string ReadLine(FileStream fs)
            //===================================
            {
                string line = string.Empty;

                const int bufferSize = 4096;
                byte[] buffer = new byte[bufferSize];
                byte b = 0;
                byte lf = 10;
                int i = 0;

                while (b != lf)
                {
                    b = (byte)fs.ReadByte();
                    buffer[i] = b;
                    i++;
                }

                line = System.Text.Encoding.Default.GetString(buffer, 0, i - 1);

                return line;
            }


            private Int32 GetLength(string fileInfo)
            //=====================================
            {
                Int32 pLength = 0;
                if (!string.IsNullOrEmpty(fileInfo))
                {
                    //....get the file information
                    string[] info = fileInfo.Split(',');
                    if (info != null && info.Length == 2)
                    {
                        pLength = Convert.ToInt32(info[1]);
                    }
                }
                return pLength;
            }


            private void Write_ObjFiles(FileStream fs, int fileLength, int Index_In)
            //=====================================================================
            {
                FileStream fsFile = null;
                string pFilePath = "";
                if (mFileName_BearingCAD != "")
                {
                    pFilePath =  mFileName_BearingCAD.Remove(mFileName_BearingCAD.Length - 11);
                }

                try
                {
                    string pFileName_Out = pFilePath + Index_In + ".BearingCAD";

                    byte[] buffer = new byte[fileLength];
                    int count = fs.Read(buffer, 0, fileLength);
                    fsFile = new FileStream(pFileName_Out, FileMode.Create, FileAccess.Write, FileShare.None);
                    fsFile.Write(buffer, 0, buffer.Length);
                    fsFile.Write(buffer, 0, count);
                }
                catch (Exception ex1)
                {
                    // handle or display the error
                    throw ex1;
                }
                finally
                {
                    if (fsFile != null)
                    {
                        fsFile.Flush();
                        fsFile.Close();
                        fsFile = null;
                    }
                }
            }

            #endregion

            #region "UTILITY ROUTINES:"

            public void CloseWordFiles()
            //===========================     
            {
                Process[] pProcesses = Process.GetProcesses();

                try
                {
                    foreach (Process p in pProcesses)
                        if (p.ProcessName == "WINWORD")
                            p.Kill();
                }
                catch (Exception pEXP)
                {

                }
            }

                public void CloseExcelFiles()
                //===========================      
                {
                    Process[] pProcesses = Process.GetProcesses();

                    try
                    {
                        foreach (Process p in pProcesses)
                            if (p.ProcessName == "EXCEL")
                                p.Kill();
                    }
                    catch (Exception pEXP)
                    {

                    }
                }

            #endregion

            #endregion

    }
        
}




