
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsDB                                  '
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
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Common;
using System.Windows.Forms;
using System.Data.SqlTypes;
using System.Collections.Specialized;
using System.Globalization;
using System.Data.OleDb;

namespace BearingCAD22
{
     [Serializable]
    public class clsDB
    {

        #region "MEMBER VARIABLE DECLARATIONS:"
        //================================

            private string mDBFileName;
            private string mDBServerName;
            CultureInfo mInvCulture = CultureInfo.InvariantCulture;
            string mstrDefDate = "";

        #endregion


        //....Class Constructor. 

        public clsDB()
        //============ 
        {
            mDBFileName = clsFiles.DBFileName;
            mDBServerName = clsFiles.DBServerName;
            mstrDefDate = DateTime.MinValue.ToString("d", mInvCulture);
        }


        #region "CLASS METHODS:"

            #region "ADO.NET HELPER ROUTINES:"



        public OleDbConnection GetConnection(string strFileName_In, ref OleDbConnection Conn_In)
        //=================================================================================

       //This routine returns OleDbConnection
        //       Input   Parameters      :   TypeName,  FileName
        //       Output  Parameters      :   OleDbConnection
        {

            //OleDbConnection pGetConnection = null;

            string pstrConnectDB = "";
            string pstrFileName = "";
            string pstrPassword = "";

            try
            {             
                pstrFileName = strFileName_In;
                pstrConnectDB = "Provider=Microsoft.Ace.OLEDB.12.0;" +
                                "Data Source=" + pstrFileName + ";" +
                                "Extended Properties='Excel 12.0 Xml;IMEX=1;HDR=YES;'";

                Conn_In = new OleDbConnection(pstrConnectDB);
                Conn_In.Open();
          }

            catch (OleDbException pEXP)
            {
                //....Handles connection-level Errors
                MessageBox.Show(pEXP.Message, "Connection Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (InvalidOperationException pEXP)
            {
                MessageBox.Show(pEXP.Message, "Connection Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            return Conn_In;
        }

        public OleDbDataReader GetDataReader(string strSELECTQry_In, string strFileName_In, ref OleDbConnection Conn_In)
        //========================================================================= 

      //This routine returns DataReader
        //       Input   Parameters      :   SQL Statement,DBTypeName,FileName 
        //       Output  Parameters      :   DataReader
        {
            OleDbDataReader pGetDataReader = null;
            
            OleDbCommand pCmd = new OleDbCommand(strSELECTQry_In, GetConnection(strFileName_In, ref Conn_In));

            OleDbDataReader pDR = null;

            try
            { pDR = pCmd.ExecuteReader(); }

            catch (Exception pEXP)
            {
                MessageBox.Show(pEXP.Message, "Data Read Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            { pGetDataReader = pDR; }

            return pGetDataReader;
        }

      //  public long ExecuteCommand(string strACTIONQry_In, string strFileName_In)
      //  //=====================================================================

      ////This routine returns Number of Records 
      //  //       Input   Parameters      :   SQL Statement,DBTypeName,FileName 
      //  //       Output  Parameters      :   Number of Records 
      //  {
      //      int pExecuteCommand = 0;

      //      OleDbConnection pConn = new OleDbConnection();
      //      pConn = GetConnection(strFileName_In);

      //      OleDbCommand pCmd = new OleDbCommand(strACTIONQry_In, pConn);
      //      int pCountRecords = 0;

      //      try
      //      {
      //          pCountRecords = pCmd.ExecuteNonQuery();

      //      }
      //      catch (Exception pEXP)
      //      {
      //          MessageBox.Show(pEXP.Message + " , " + pEXP.StackTrace, "Command Execution Error",
      //                          MessageBoxButtons.OK, MessageBoxIcon.Error);
      //      }
      //      finally
      //      {
      //          pExecuteCommand = pCountRecords;
      //      }

      //      pConn.Close();

      //      return pExecuteCommand;
      //  }

        public int PopulateCmbBox(ComboBox cmbBox_In, string FileName_In, string strTableName_In, 
                                  string strFldName_In, string strWHERE_In, bool blnOrderBy_In)
        //======================================================================================   
        {
            //....This utility function populates a comboBox and 
            //......returns the # of list items, if any.

            //This routine populate comboboxes
            //       Input   Parameters      :   ComboBoxName, TableName, FieldName
            //       Output  Parameters      :   No of Records

            //....Create the SQL string.   
            //
            int pCountRec = 0;
            try
            {

                OleDbConnection pConnection = null;
                string pstrORDERBY = "";
                if (blnOrderBy_In == true)
                    pstrORDERBY = " ORDER BY " + strFldName_In + " ASC";

                string pstrSQL = "";
                pstrSQL = "SELECT " + " DISTINCT " + strFldName_In + " FROM " +
                            strTableName_In + " " + strWHERE_In + pstrORDERBY;

                //....Get the corresponding data reader object.
                OleDbDataReader pobjDR = null;
                pobjDR = modMain.gDB.GetDataReader(pstrSQL, FileName_In, ref pConnection);

                //....Store the ordinal for the given field for better performance.
                int pColFldName = 0;
                pColFldName = pobjDR.GetOrdinal(strFldName_In);

                //Add list items to the Combo Box
                //-------------------------------
                
                string pRowVal = "";

                cmbBox_In.Items.Clear();
                List<string> pVal_List = new List<string>();
                Boolean pFlag = true;
                while (pobjDR.Read())
                {
                    pFlag = true;
                    pCountRec = pCountRec + 1;
                    if (pobjDR.IsDBNull(pColFldName) == false)
                        pRowVal = Convert.ToString(pobjDR[pColFldName]).Trim();
                    if (strFldName_In == "Pitch")
                    {
                        double pVal = modMain.ConvTextToDouble(pRowVal);
                        pRowVal = pVal.ToString("#0.000");
                        if (!pVal_List.Contains(pRowVal))
                        {
                            pVal_List.Add(pRowVal);
                        }
                        else
                        {
                            pFlag = false;
                        }
                    }
                    else if (strFldName_In == "L")
                    {
                        Double pVal = modMain.ConvTextToDouble(pRowVal);
                        double pDecimalPart = pVal - Math.Truncate(pVal);
                        if (pDecimalPart > modMain.gcEPS)
                        {
                            pRowVal = pVal.ToString("#0.0000");
                        }
                        else
                        {
                            pRowVal = pVal.ToString("#0");
                        }

                    }

                    if (pFlag)
                    {
                        cmbBox_In.Items.Add(pRowVal.Trim());
                    }
                }

                pobjDR.Dispose();
                pConnection.Close();
                return pCountRec;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return pCountRec;
            }
        }


               


                ////private SqlDataAdapter GetDataAdapter(string strQry_In)
                //////=====================================================

                //////This routine returns DataAdapter
                //////       Input   Parameters      :   SQL Statement 
                //////       Output  Parameters      :   DataAdapter

                //////'....Not yet used anywhere. 
                ////{
                ////    SqlConnection pConn = new SqlConnection();
                ////    pConn = GetConnection();

                ////    SqlDataAdapter pDA = new SqlDataAdapter(strQry_In, pConn);

                ////    //....Set SelectCommand Properties:
                ////    pDA.SelectCommand = new SqlCommand();
                ////    pDA.SelectCommand.Connection = pConn;
                ////    pDA.SelectCommand.CommandText = strQry_In;
                ////    pDA.SelectCommand.CommandType = CommandType.Text;

                ////    //....Now execute the command.
                ////    pDA.SelectCommand.ExecuteNonQuery();

                ////    return pDA;
                ////}


                ////public DataSet GetDataSet(string strQry_In, string strTableName_In)                   
                //////=================================================================  

                ////  //This routine returns DataSet
                //////      Input   Parameters      :   SQL Statement , Database Table Name
                //////      Output  Parameters      :   DataSet
                ////{

                ////    //Data Adapter Object:
                ////    //--------------------
                ////    SqlDataAdapter pDA = new SqlDataAdapter();
                ////    pDA.SelectCommand = new SqlCommand(strQry_In, GetConnection());


                ////    //....Set SelectCommand Properties.
                ////    //pDA.SelectCommand.Connection = GetConnection();       //AM 16MAR12
                ////    //pDA.SelectCommand.CommandText = strQry_In;
                ////    pDA.SelectCommand.CommandType = CommandType.Text;

                ////    //....Now execute the command.
                ////    try
                ////    {
                ////        pDA.SelectCommand.ExecuteNonQuery();
                ////    }
                ////    catch (Exception e)
                ////    {
                ////        MessageBox.Show(e.Message);
                ////    }

                ////    //DataSet object: Fill with Data.
                ////    //-------------------------------
                ////    DataSet pDS = new DataSet();
                ////    pDS.Clear();
                ////    pDA.Fill(pDS, strTableName_In);

                ////    return pDS;
                ////}


                ////public DataView GetDataView(string strQry_In, string strTableName_In)                              
                //////===================================================================   

                //////This routine returns DataView
                //////       Input   Parameters      :   SQL Statement , Database Table Name
                //////       Output  Parameters      :   DataView
                ////{
                ////    SqlConnection pConn = new SqlConnection();
                ////    pConn = GetConnection();

                ////    //Data Adapter Object:
                ////    //--------------------
                ////    SqlDataAdapter pDA = new SqlDataAdapter();
                ////    pDA.SelectCommand = new SqlCommand(strQry_In, pConn);

                ////    //....Set SelectCommand Properties:
                ////    pDA.SelectCommand.Connection = pConn;
                ////    pDA.SelectCommand.CommandText = strQry_In;
                ////    pDA.SelectCommand.CommandType = CommandType.Text;
                ////    //pDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                ////    //....Execute the command.
                ////    pDA.SelectCommand.ExecuteNonQuery();

                ////    //DataSet object: Fill with Data.
                ////    //-------------------------------
                ////    DataSet pDS = new DataSet();
                ////    pDS.Clear();
                ////    pDA.Fill(pDS, strTableName_In);

                ////    //....DataView Object.
                ////    return new DataView(pDS.Tables[strTableName_In]);
                ////}

            #endregion


            //#region "DATABASE RELATED ROUTINES:"
            ////---------------------------------

            //    #region "Database Retrieval Routine:"

            //        public void RetrieveRecord(clsProject Project_In, clsOpCond OpCond_In)
            //        //=====================================================================
            //        {
            //            //RetrieveRec_Project(Project_In);
            //            RetrieveRec_Project_ORM(Project_In);
            //            //RetrieveRec_OpCond(Project_In, OpCond_In);
            //            RetrieveRec_OpCond_ORM(Project_In, OpCond_In);
            //            //RetrieveRec_Product(Project_In);
            //            RetrieveRec_Product_ORM(Project_In);
            //            //RetrieveRec_Bearing_Radial(Project_In);
            //            RetrieveRec_Bearing_Radial_ORM(Project_In);
            //            //RetrieveRec_Bearing_Radial_FP_Detail(Project_In);
            //            RetrieveRec_Bearing_Radial_FP_Detail_ORM(Project_In);
            //            //RetrieveRec_EndConfigs(Project_In);
            //            RetrieveRec_EndConfigs_ORM(Project_In);
            //            //RetrieveRec_Accessories(Project_In);
            //            RetrieveRec_Accessories_ORM(Project_In);
            //        }
       

            //        #region "Retrieval: Project"

            //        private void RetrieveRec_Project_ORM(clsProject Project_In)
            //        //=========================================================    //AES 28MAY18 
            //        {
            //            BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                        
            //            string pNoSuffix = "";

            //            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //            {
            //                pNoSuffix = Project_In.No_Suffix;
            //            }
            //            else
            //            {
            //                pNoSuffix = "NULL";
            //            }

            //            var pProject = (from pRec in pBearingDBEntities.tblProject_Details where pRec.fldNo == Project_In.No && pRec.fldNo_Suffix == pNoSuffix select pRec).ToList();

            //            if (pProject.Count > 0)
            //            {
            //                Project_In.Status = CheckDBString(pProject[0].fldStatus);

            //                //....Customer
            //                Project_In.Customer_Name = CheckDBString(pProject[0].fldCustomer_Name);
            //                Project_In.Customer_MachineDesc = CheckDBString(pProject[0].fldCustomer_MachineDesc);
            //                Project_In.Customer_PartNo = CheckDBString(pProject[0].fldCustomer_PartNo);

            //                //....Unit System
            //                Project_In.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem),
            //                                                        pProject[0].fldUnitSystem);

            //                //....AssyDwg
            //                Project_In.AssyDwgNo = CheckDBString(pProject[0].fldAssyDwg_No);
            //                Project_In.AssyDwgNo_Suffix = CheckDBString(pProject[0].fldAssyDwg_No_Suffix);
            //                Project_In.AssyDwgRef = CheckDBString(pProject[0].fldAssyDwg_Ref);

            //                //....Engg.
            //                Project_In.Engg_Name = CheckDBString(pProject[0].fldEngg_Name);
            //                Project_In.Engg_Initials = CheckDBString(pProject[0].fldEngg_Initials);
            //                if (pProject[0].fldEngg_Date.HasValue)
            //                {
            //                    Project_In.Engg_Date = (DateTime)pProject[0].fldEngg_Date;
            //                }
            //                else
            //                {
            //                    Project_In.Engg_Date = DateTime.MinValue;
            //                }
                            
            //                //....Designer.
            //                Project_In.DesignedBy_Name = CheckDBString(pProject[0].fldDesignedBy_Name);
            //                Project_In.DesignedBy_Initials = CheckDBString(pProject[0].fldDesignedBy_Initials);
                                                        
            //                if (pProject[0].fldDesignedBy_Date.HasValue)
            //                {
            //                    Project_In.DesignedBy_Date = (DateTime)pProject[0].fldDesignedBy_Date;
            //                }
            //                else
            //                {
            //                    Project_In.DesignedBy_Date = DateTime.MinValue;
            //                }
                            
            //                //....Checked By.
            //                Project_In.CheckedBy_Name = CheckDBString(pProject[0].fldCheckedBy_Name);
            //                Project_In.CheckedBy_Initials = CheckDBString(pProject[0].fldCheckedBy_Initials);
            //                if (pProject[0].fldCheckedBy_Date.HasValue)
            //                {
            //                    Project_In.CheckedBy_Date = (DateTime)pProject[0].fldCheckedBy_Date;
            //                }
            //                else
            //                {
            //                    Project_In.CheckedBy_Date = DateTime.MinValue;
            //                }
                            
            //                //....Closing Date.
            //                if (pProject[0].fldDate_Closing.HasValue)
            //                {
            //                    Project_In.Date_Closing =(DateTime)pProject[0].fldDate_Closing;
            //                }
            //                else
            //                {
            //                    Project_In.Date_Closing = DateTime.MinValue;
            //                }
                            
            //                //....File Paths    //To be saved in CreateFiles.
            //                Project_In.FilePath_Project = CheckDBString(pProject[0].fldFilePath_Project);
            //                Project_In.FilePath_DesignTbls_SWFiles = CheckDBString(pProject[0].fldFilePath_DesignTbls_SWFiles);

            //                //....SolidWorks Model Files Paths
            //                Project_In.FileModified_CompleteAssy = CheckDBBoolean(pProject[0].fldFileModified_CompleteAssy);

            //                Project_In.FileModified_RadialPart = CheckDBBoolean(pProject[0].fldFileModified_Radial_Part);
            //                Project_In.FileModified_RadialBlankAssy = CheckDBBoolean(pProject[0].fldFileModified_Radial_BlankAssy);

            //                Project_In.FileModified_EndTB_Part = CheckDBBoolean(pProject[0].fldFileModified_EndTB_Part);
            //                Project_In.FileModified_EndTB_Assy = CheckDBBoolean(pProject[0].fldFileModified_EndTB_Assy);

            //                Project_In.FileModified_EndSeal_Part = CheckDBBoolean(pProject[0].fldFileModified_EndSeal_Part);

            //                Project_In.FileModification_Notes = CheckDBString(pProject[0].fldFileModification_Notes);
            //            }
            //        }

            //        #endregion

            //        #region "Retrieval: Operating Condition"

            //            private void RetrieveRec_OpCond_ORM(clsProject Project_In, clsOpCond OpCond_In)
            //            //=============================================================================    //AES 28MAY18  
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_OpCond where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    OpCond_In.Speed_Range[0] = CheckDBInt(pProject[0].fldSpeed_Range_Min);
            //                    OpCond_In.Speed_Range[1] = CheckDBInt(pProject[0].fldSpeed_Range_Max);

            //                    string pRot = pProject[0].fldRot_Directionality.ToString();
            //                    if (pRot != "")
            //                        OpCond_In.Rot_Directionality =
            //                            (clsOpCond.eRotDirectionality)Enum.Parse(typeof(clsOpCond.eRotDirectionality), pRot);

            //                    OpCond_In.Radial_Load_Range[0] = CheckDBDouble(pProject[0].fldRadial_Load_Range_Min);
            //                    OpCond_In.Radial_Load_Range[1] = CheckDBDouble(pProject[0].fldRadial_Load_Range_Max);

            //                    OpCond_In.Radial_LoadAng = CheckDBDouble(pProject[0].fldRadial_LoadAng);

            //                    OpCond_In.Thrust_Load_Range_Front[0] = CheckDBDouble(pProject[0].fldThrust_Load_Range_Front_Min);
            //                    OpCond_In.Thrust_Load_Range_Front[1] = CheckDBDouble(pProject[0].fldThrust_Load_Range_Front_Max);

            //                    OpCond_In.Thrust_Load_Range_Back[0] = CheckDBDouble(pProject[0].fldThrust_Load_Range_Back_Min);
            //                    OpCond_In.Thrust_Load_Range_Back[1] = CheckDBDouble(pProject[0].fldThrust_Load_Range_Back_Max);

            //                    OpCond_In.OilSupply_Press = CheckDBDouble(pProject[0].fldOilSupply_Press);
            //                    OpCond_In.OilSupply_Temp = CheckDBDouble(pProject[0].fldOilSupply_Temp);
            //                    OpCond_In.OilSupply_Type = CheckDBString(pProject[0].fldOilSupply_Type);
            //                    OpCond_In.OilSupply_Lube_Type = CheckDBString(pProject[0].fldOilSupply_Lube_Type);
            //                }
            //            }
        
            //        #endregion


            //        #region "Retrieval: Product"

            //            private void RetrieveRec_Product_ORM(clsProject Project_In)
            //            //=========================================================    //AES 28MAY18     
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Product where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    //....Bearing Type
            //                    Project_In.Product.Type = (clsBearing.eType)Enum.Parse(typeof(clsBearing.eType),
            //                                                            CheckDBString(pProject[0].fldBearing_Type));

            //                    //....EndConfigs                                                                 
            //                    Project_In.Product.L_Available = CheckDBDouble(pProject[0].fldL_Available);
            //                    Project_In.Product.Dist_ThrustFace[0] = CheckDBDouble(pProject[0].fldDist_ThrustFace_Front);
            //                    Project_In.Product.Dist_ThrustFace[1] = CheckDBDouble(pProject[0].fldDist_ThrustFace_Back);
            //                }
            //            }
                       
            //        #endregion


            //        #region "Retrieval: Bearing Radial"

            //            private void RetrieveRec_Bearing_Radial_ORM(clsProject Project_In)
            //            //================================================================      //AES 28MAY18 
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    //....Radial Bearing Type
            //                    ((clsBearing_Radial)Project_In.Product.Bearing).Design = (clsBearing_Radial.eDesign)Enum.Parse(typeof(clsBearing_Radial.eDesign),
            //                                                            CheckDBString(pProject[0].fldDesign));//CheckDBString(pDR, "fldDesign")); 
            //                }
            //            }
                       
            //        #endregion


            //        #region "Retrieval: Bearing Radial FP Detail"


            //            private void RetrieveRec_Bearing_Radial_FP_Detail_ORM(clsProject Project_In)
            //            //==========================================================================      
            //            {
            //                RetrieveRec_Bearing_Radial_FP_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_PerformData_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_Pad_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_FlexurePivot_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_OilInlet_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_MillRelief_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_Flange_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_SL_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_AntiRotPin_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_Mount_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_TempSensor_ORM(Project_In);
            //                RetrieveRec_Bearing_Radial_FP_EDM_Pad_ORM(Project_In);
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_ORM(clsProject Project_In)
            //            //===================================================================      //AES 05JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig = CheckDBBoolean(pProject[0].fldSplitConfig);// CheckDBBoolean(pDR, "fldSplitConfig");

            //                    //Bearing Geometry. 
            //                    //---------------- 

            //                    //....DShaft. 
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0] = CheckDBDouble(pProject[0].fldDShaft_Range_Min);//CheckDBDouble(pDR, "fldDShaft_Range_Min");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1] = CheckDBDouble(pProject[0].fldDShaft_Range_Max);// CheckDBDouble(pDR, "fldDShaft_Range_Max");

            //                    //....DFit.
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0] = CheckDBDouble(pProject[0].fldDFit_Range_Min);// CheckDBDouble(pDR, "fldDFit_Range_Min");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1] = CheckDBDouble(pProject[0].fldDFit_Range_Max);// CheckDBDouble(pDR, "fldDFit_Range_Max");

            //                    //....DSet.
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0] = CheckDBDouble(pProject[0].fldDSet_Range_Min);// CheckDBDouble(pDR, "fldDSet_Range_Min");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1] = CheckDBDouble(pProject[0].fldDSet_Range_Max);// CheckDBDouble(pDR, "fldDSet_Range_Max");

            //                    //....DPad.
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0] = CheckDBDouble(pProject[0].fldDPad_Range_Min);// CheckDBDouble(pDR, "fldDPad_Range_Min");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1] = CheckDBDouble(pProject[0].fldDPad_Range_Max);// CheckDBDouble(pDR, "fldDPad_Range_Max");

            //                    //....Bearing Length, Length Total.
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).L = CheckDBDouble(pProject[0].fldL);// CheckDBDouble(pDR, "fldL");

            //                    RetrieveRec_Bearing_Radial_FP_Pad_ORM(Project_In); //....For Pad L.

            //                    ////....EDM Relief
            //                    //((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[0] = CheckDBDouble(pDR, "fldEDMRelief_Front");
            //                    //((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[1] = CheckDBDouble(pDR, "fldEDMRelief_Back");

            //                    //....EndConfig Depth 
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[0] = CheckDBDouble(pProject[0].fldDepth_EndConfig_Front);// CheckDBDouble(pDR, "fldDepth_EndConfig_Front");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[1] = CheckDBDouble(pProject[0].fldDepth_EndConfig_Back);// CheckDBDouble(pDR, "fldDepth_EndConfig_Back");

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DimStart_FrontFace = CheckDBDouble(pProject[0].fldDimStart_FrontFace);//CheckDBDouble(pDR, "fldDimStart_FrontFace");

            //                    // Material
            //                    // --------                            
            //                    //....Base                            
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base = CheckDBString(pProject[0].fldMat_Base);// CheckDBString(pDR, "fldMat_Base");

            //                    //....Lining    
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.LiningExists = CheckDBBoolean(pProject[0].fldMat_LiningExists);// CheckDBBoolean(pDR, "fldMat_LiningExists");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining = pProject[0].fldMat_Lining.ToString();// pDR["fldMat_Lining"].ToString();

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT = CheckDBDouble(pProject[0].fldLiningT);// CheckDBDouble(pDR, "fldLiningT");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_PerformData_ORM(clsProject Project_In)
            //            //===============================================================================      //AES 05JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_PerformData where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Perfor Data
            //                    // ------------
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP = CheckDBDouble(pProject[0].fldPower_HP);// CheckDBDouble(pDR, "fldPower_HP");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm = CheckDBDouble(pProject[0].fldFlowReqd_gpm);// CheckDBDouble(pDR, "fldFlowReqd_gpm");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F = CheckDBDouble(pProject[0].fldTempRise_F);// CheckDBDouble(pDR, "fldTempRise_F");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TFilm_Min = CheckDBDouble(pProject[0].fldTFilm_Min);// CheckDBDouble(pDR, "fldTFilm_Min");

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Temp = CheckDBDouble(pProject[0].fldPadMax_Temp);// CheckDBDouble(pDR, "fldPadMax_Temp");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Press = CheckDBDouble(pProject[0].fldPadMax_Press);// CheckDBDouble(pDR, "fldPadMax_Press");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Rot = CheckDBDouble(pProject[0].fldPadMax_Rot);// CheckDBDouble(pDR, "fldPadMax_Rot");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Load = CheckDBDouble(pProject[0].fldPadMax_Load);// CheckDBDouble(pDR, "fldPadMax_Load");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Stress = CheckDBDouble(pProject[0].fldPadMax_Stress);// CheckDBDouble(pDR, "fldPadMax_Stress");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_Pad_ORM(clsProject Project_In)
            //            //=======================================================================      //AES 05JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Pad where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Pad
            //                    // ---                             
            //                    //....Type.
            //                    if (CheckDBString(pProject[0].fldType)!="")//CheckDBString(pDR, "fldType") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type =
            //                            (clsBearing_Radial_FP.clsPad.eLoadPos)Enum.Parse(typeof(clsBearing_Radial_FP.clsPad.eLoadPos), CheckDBString(pProject[0].fldType));//CheckDBString(pDR, "fldType"));

            //                    //....Count.
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count = CheckDBInt(pProject[0].fldCount);// CheckDBInt(pDR, "fldCount");

            //                    //....Length.
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L = CheckDBDouble(pProject[0].fldL);// CheckDBDouble(pDR, "fldL");

            //                    //....Pivot.
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot_Offset = CheckDBDouble(pProject[0].fldPivot_Offset);// CheckDBDouble(pDR, "fldPivot_Offset");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot_AngStart = CheckDBDouble(pProject[0].fldPivot_AngStart);// CheckDBDouble(pDR, "fldPivot_AngStart");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T_Lead = CheckDBDouble(pProject[0].fldT_Lead);// CheckDBDouble(pDR, "fldT_Lead");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T_Pivot = CheckDBDouble(pProject[0].fldT_Pivot);// CheckDBDouble(pDR, "fldT_Pivot");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T_Trail = CheckDBDouble(pProject[0].fldT_Trail);// CheckDBDouble(pDR, "fldT_Trail");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_FlexurePivot_ORM(clsProject Project_In)
            //            //================================================================================      //AES 05JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_FlexurePivot where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // FlexPivot
            //                    // ---------
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web_T = CheckDBDouble(pProject[0].fldWeb_T);// CheckDBDouble(pDR, "fldWeb_T");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web_H = CheckDBDouble(pProject[0].fldWeb_H);// CheckDBDouble(pDR, "fldWeb_H");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web_RFillet = CheckDBDouble(pProject[0].fldWeb_RFillet);// CheckDBDouble(pDR, "fldWeb_RFillet");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM = CheckDBDouble(pProject[0].fldGapEDM);// CheckDBDouble(pDR, "fldGapEDM");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Rot_Stiff = CheckDBDouble(pProject[0].fldRot_Stiff);// CheckDBDouble(pDR, "fldRot_Stiff");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_OilInlet_ORM(clsProject Project_In)
            //            //============================================================================      //AES 05JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_OilInlet where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // OilInlet
            //                    // --------
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_Count = CheckDBInt(pProject[0].fldOrifice_Count);// CheckDBInt(pDR, "fldOrifice_Count");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_D = CheckDBDouble(pProject[0].fldOrifice_D);// CheckDBDouble(pDR, "fldOrifice_D");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply = CheckDBInt(pProject[0].fldCount_MainOilSupply);// CheckDBInt(pDR, "fldCount_MainOilSupply");

            //                    if (CheckDBString(pProject[0].fldOrifice_StartPos)!="")//CheckDBString(pDR, "fldOrifice_StartPos") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_StartPos = (clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos)Enum.Parse(typeof(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos),
            //                                                    CheckDBString(pProject[0].fldOrifice_StartPos));//CheckDBString(pDR, "fldOrifice_StartPos"));

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_Loc_FrontFace = CheckDBDouble(pProject[0].fldOrifice_Loc_FrontFace);// CheckDBDouble(pDR, "fldOrifice_Loc_FrontFace");

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_Exists = CheckDBBoolean(pProject[0].fldAnnulus_Exists);// CheckDBBoolean(pDR, "fldAnnulus_Exists");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_D = CheckDBDouble(pProject[0].fldAnnulus_D);// CheckDBDouble(pDR, "fldAnnulus_D");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_Loc_Back = CheckDBDouble(pProject[0].fldAnnulus_Loc_Back);// CheckDBDouble(pDR, "fldAnnulus_Loc_Back");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_L = CheckDBDouble(pProject[0].fldAnnulus_L);// CheckDBDouble(pDR, "fldAnnulus_L");

            //                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D == 0.0F && ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.L == 0.0F)
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Calc_Annulus_Params();

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_Dist_Holes = CheckDBDouble(pProject[0].fldOrifice_Dist_Holes);// CheckDBDouble(pDR, "fldOrifice_Dist_Holes");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_MillRelief_ORM(clsProject Project_In)
            //            //==============================================================================      //AES 05JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_MillRelief where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Mill Relief
            //                    // -----------
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists = CheckDBBoolean(pProject[0].fldExists);//CheckDBBoolean(pDR, "fldExists");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_Desig = CheckDBString(pProject[0].fldD_Desig);// CheckDBString(pDR, "fldD_Desig");
            //                    //....EDM Relief
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[0] = CheckDBDouble(pProject[0].fldEDMRelief_Front);// CheckDBDouble(pDR, "fldEDMRelief_Front");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[1] = CheckDBDouble(pProject[0].fldEDMRelief_Back);// CheckDBDouble(pDR, "fldEDMRelief_Back");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_Flange_ORM(clsProject Project_In)
            //            //==========================================================================      //AES 05JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Flange where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Flange
            //                    // ------
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists = CheckDBBoolean(pProject[0].fldExists);// CheckDBBoolean(pDR, "fldExists");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D = CheckDBDouble(pProject[0].fldD);// CheckDBDouble(pDR, "fldD");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid = CheckDBDouble(pProject[0].fldWid);// CheckDBDouble(pDR, "fldWid");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Front = CheckDBDouble(pProject[0].fldDimStart_Front);// CheckDBDouble(pDR, "fldDimStart_Front");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_SL_ORM(clsProject Project_In)
            //            //======================================================================      //AES 05JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_SL where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Split Line Hardware
            //                    // -------------------

            //                    // Thread:
            //                    // -------                                 
            //                    if (CheckDBString(pProject[0].fldScrew_Spec_UnitSystem)!="")//CheckDBString(pDR, "fldScrew_Spec_UnitSystem") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System = (clsUnit.eSystem)
            //                                         Enum.Parse(typeof(clsUnit.eSystem), CheckDBString(pProject[0].fldScrew_Spec_UnitSystem));//CheckDBString(pDR, "fldScrew_Spec_UnitSystem"));                //BG 26MAR12

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Type = CheckDBString(pProject[0].fldScrew_Spec_Type);// CheckDBString(pDR, "fldScrew_Spec_Type");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig = CheckDBString(pProject[0].fldScrew_Spec_D_Desig);// CheckDBString(pDR, "fldScrew_Spec_D_Desig");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch = CheckDBDouble(pProject[0].fldScrew_Spec_Pitch);// CheckDBDouble(pDR, "fldScrew_Spec_Pitch");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L = CheckDBDouble(pProject[0].fldScrew_Spec_L);// CheckDBDouble(pDR, "fldScrew_Spec_L");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Mat = CheckDBString(pProject[0].fldDowel_Spec_Mat);// CheckDBString(pDR, "fldScrew_Spec_Mat");

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc_Center = CheckDBDouble(pProject[0].fldLScrew_Spec_Loc_Center);// CheckDBDouble(pDR, "fldLScrew_Spec_Loc_Center");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc_Front = CheckDBDouble(pProject[0].fldLScrew_Spec_Loc_Front); // CheckDBDouble(pDR, "fldLScrew_Spec_Loc_Front");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc_Center = CheckDBDouble(pProject[0].fldRScrew_Spec_Loc_Center);// CheckDBDouble(pDR, "fldRScrew_Spec_Loc_Center");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc_Front = CheckDBDouble(pProject[0].fldRScrew_Spec_Loc_Front);// CheckDBDouble(pDR, "fldRScrew_Spec_Loc_Front");

            //                    // Pin:
            //                    // ----                                  
            //                    if (CheckDBString(pProject[0].fldDowel_Spec_UnitSystem)!="")//CheckDBString(pDR, "fldDowel_Spec_UnitSystem") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System = (clsUnit.eSystem)        //BG 26MAR12
            //                                         Enum.Parse(typeof(clsUnit.eSystem), CheckDBString(pProject[0].fldDowel_Spec_UnitSystem));//CheckDBString(pDR, "fldDowel_Spec_UnitSystem"));

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Type = CheckDBString(pProject[0].fldDowel_Spec_Type);// CheckDBString(pDR, "fldDowel_Spec_Type");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig = CheckDBString(pProject[0].fldDowel_Spec_D_Desig);// CheckDBString(pDR, "fldDowel_Spec_D_Desig");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.L = CheckDBDouble(pProject[0].fldDowel_Spec_L);// CheckDBDouble(pDR, "fldDowel_Spec_L");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Mat = CheckDBString(pProject[0].fldDowel_Spec_Mat);// CheckDBString(pDR, "fldDowel_Spec_Mat");

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc_Center = CheckDBDouble(pProject[0].fldLDowel_Spec_Loc_Center);// CheckDBDouble(pDR, "fldLDowel_Spec_Loc_Center");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc_Front = CheckDBDouble(pProject[0].fldLDowel_Spec_Loc_Front);// CheckDBDouble(pDR, "fldLDowel_Spec_Loc_Front");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc_Center = CheckDBDouble(pProject[0].fldRScrew_Spec_Loc_Center);// CheckDBDouble(pDR, "fldRDowel_Spec_Loc_Center");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc_Front = CheckDBDouble(pProject[0].fldRScrew_Spec_Loc_Front);// CheckDBDouble(pDR, "fldRDowel_Spec_Loc_Front");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_AntiRotPin_ORM(clsProject Project_In)
            //            //==============================================================================      //AES 07JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_AntiRotPin where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Anti Rotation Pin
            //                    // ------------------
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Dist_Front = CheckDBDouble(pProject[0].fldLoc_Dist_Front);// CheckDBDouble(pDR, "fldLoc_Dist_Front");

            //                    if (CheckDBString(pProject[0].fldLoc_Casing_SL)!="")//CheckDBString(pDR, "fldLoc_Casing_SL") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL = (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL)
            //                            Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL),
            //                            CheckDBString(pProject[0].fldLoc_Casing_SL));//CheckDBString(pDR, "fldLoc_Casing_SL"));

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Offset = CheckDBDouble(pProject[0].fldLoc_Offset);// CheckDBDouble(pDR, "fldLoc_Offset");

            //                    if (CheckDBString(pProject[0].fldLoc_Bearing_Vert)!="")//CheckDBString(pDR, "fldLoc_Bearing_Vert") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert = (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert)
            //                            Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert),
            //                            CheckDBString(pProject[0].fldLoc_Bearing_Vert));//CheckDBString(pDR, "fldLoc_Bearing_Vert"));

            //                    if (CheckDBString(pProject[0].fldLoc_Bearing_SL)!="")//CheckDBString(pDR, "fldLoc_Bearing_SL") != "")                  //BG 08MAY12
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL = (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL)
            //                            Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL),
            //                            CheckDBString(pProject[0].fldLoc_Bearing_SL));//CheckDBString(pDR, "fldLoc_Bearing_SL"));

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Angle = CheckDBDouble(pProject[0].fldLoc_Angle);// CheckDBDouble(pDR, "fldLoc_Angle");

            //                    // Pin:
            //                    // ----
            //                    if (CheckDBString(pProject[0].fldSpec_UnitSystem)!="")//CheckDBString(pDR, "fldSpec_UnitSystem") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Unit.System = (clsUnit.eSystem)
            //                                         Enum.Parse(typeof(clsUnit.eSystem), CheckDBString(pProject[0].fldSpec_UnitSystem));//CheckDBString(pDR, "fldSpec_UnitSystem"));                      //BG 26MAR12

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Type = CheckDBString(pProject[0].fldSpec_Type);// CheckDBString(pDR, "fldSpec_Type");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig = CheckDBString(pProject[0].fldSpec_D_Desig);// CheckDBString(pDR, "fldSpec_D_Desig");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.L = CheckDBDouble(pProject[0].fldSpec_L);// CheckDBDouble(pDR, "fldSpec_L");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Mat = CheckDBString(pProject[0].fldSpec_Mat);// CheckDBString(pDR, "fldSpec_Mat");

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Depth = CheckDBDouble(pProject[0].fldDepth);// CheckDBDouble(pDR, "fldDepth");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Stickout = CheckDBDouble(pProject[0].fldStickOut);// CheckDBDouble(pDR, "fldStickOut");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_Mount_ORM(clsProject Project_In)
            //            //=========================================================================      //AES 07JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Mount Hole - Front
            //                    // ------------------  
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru = CheckDBBoolean(pProject[0].fldHoles_GoThru);// CheckDBBoolean(pDR, "fldHoles_GoThru");

            //                    if (CheckDBString(pProject[0].fldHoles_Bolting)!="")//CheckDBString(pDR, "fldHoles_Bolting") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting = (clsBearing_Radial_FP.eFaceID)
            //                                                                                Enum.Parse(typeof(clsBearing_Radial_FP.eFaceID),
            //                                                                                        CheckDBString(pProject[0].fldHoles_Bolting));//CheckDBString(pDR, "fldHoles_Bolting"));

            //                    if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
            //                    {

            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0] = CheckDBBool(pProject[0].fldFixture_Candidates_Chosen_Front);// CheckDBBool(pDR, "fldFixture_Candidates_Chosen_Front");
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[1] = CheckDBBool(pProject[0].fldFixture_Candidates_Chosen_Back);// CheckDBBool(pDR, "fldFixture_Candidates_Chosen_Back");

            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0] = CheckDBDouble(pProject[0].fldHoles_Thread_Depth_Front);// CheckDBDouble(pDR, "fldHoles_Thread_Depth_Front");
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1] = CheckDBDouble(pProject[0].fldHoles_Thread_Depth_Back);// CheckDBDouble(pDR, "fldHoles_Thread_Depth_Back");

            //                        RetrieveRec_Bearing_Radial_FP_Mount_Fixture_ORM(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString(), Project_In);
            //                    }
            //                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
            //                    {
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
            //                        {
            //                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0] = CheckDBBool(pProject[0].fldFixture_Candidates_Chosen_Front);// CheckDBBool(pDR, "fldFixture_Candidates_Chosen_Front");
            //                            RetrieveRec_Bearing_Radial_FP_Mount_Fixture_ORM(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString(), Project_In);
            //                        }
            //                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
            //                        {
            //                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[1] = CheckDBBool(pProject[0].fldFixture_Candidates_Chosen_Back);// CheckDBBool(pDR, "fldFixture_Candidates_Chosen_Back");
            //                            RetrieveRec_Bearing_Radial_FP_Mount_Fixture_ORM(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString(), Project_In);
            //                        }
            //                    }
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_TempSensor_ORM(clsProject Project_In)
            //            //==============================================================================      //AES 07JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_TempSensor where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Temp Sensor
            //                    // -----------
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists = CheckDBBoolean(pProject[0].fldExists);// CheckDBBoolean(pDR, "fldExists");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength = CheckDBDouble(pProject[0].fldCanLength);// CheckDBDouble(pDR, "fldCanLength");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count = CheckDBInt(pProject[0].fldCount);// CheckDBInt(pDR, "fldCount");

            //                    if (CheckDBString(pProject[0].fldLoc)!="")//CheckDBString(pDR, "fldLoc") != "")
            //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc =
            //                        (clsBearing_Radial_FP.eFaceID)Enum.Parse(typeof(clsBearing_Radial_FP.eFaceID), CheckDBString(pProject[0].fldLoc));//CheckDBString(pDR, "fldLoc"));

            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D = CheckDBDouble(pProject[0].fldD);// CheckDBDouble(pDR, "fldD");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth = CheckDBDouble(pProject[0].fldDepth);// CheckDBDouble(pDR, "fldDepth");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart = CheckDBDouble(pProject[0].fldAngStart);// CheckDBDouble(pDR, "fldAngStart");
            //                }
            //            }

            //            private void RetrieveRec_Bearing_Radial_FP_EDM_Pad_ORM(clsProject Project_In)
            //            //===========================================================================      //AES 07JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_EDM_Pad where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // EDM Pad
            //                    // -------
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.RFillet_Back = CheckDBDouble(pProject[0].fldRFillet_Back);// CheckDBDouble(pDR, "fldRFillet_Back");
            //                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.AngStart_Web = CheckDBDouble(pProject[0].fldAngStart_Web);// CheckDBDouble(pDR, "fldAngStart_Web");
            //                }
            //            }

                              

            //            #region "Retrieval: Bearing Radial FP - Mount"

                       
            //                #region ""Retrieval: Bearing Radial FP - Mount Fixture"

            //                    private StringCollection Retrieve_Bearing_Radial_FP_Mount_Fixture_Bolting_Pos_ORM(clsProject Project_In)
            //                    //======================================================================================================      //AES 18JUN18
            //                    {
            //                        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                        string pNoSuffix = "";

            //                        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                        {
            //                            pNoSuffix = Project_In.No_Suffix;
            //                        }
            //                        else
            //                        {
            //                            pNoSuffix = "NULL";
            //                        }

            //                        var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount_Fixture where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                        StringCollection pPos = new StringCollection();


            //                        if (pProject.Count > 0)
            //                        {
            //                            for (int i = 0; i < pProject.Count; i++)
            //                            {
            //                                pPos.Add(CheckDBString(pProject[i].fldPosition));
            //                            }
            //                        }

            //                        return pPos;
            //                    }

            //                    private void RetrieveRec_Bearing_Radial_FP_Mount_Fixture_ORM(string Bolting_In, clsProject Project_In)
            //                    //====================================================================================================     //AES 18JUN18
            //                    {
            //                        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                        string pNoSuffix = "";

            //                        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                        {
            //                            pNoSuffix = Project_In.No_Suffix;
            //                        }
            //                        else
            //                        {
            //                            pNoSuffix = "NULL";
            //                        }

            //                        var pQryMounFixture = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount_Fixture where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                        string pPosition = "";

            //                        if (pQryMounFixture.Count > 0)
            //                        {
            //                            for (int i = 0; i < pQryMounFixture.Count; i++)
            //                            {
            //                                pPosition = CheckDBString(pQryMounFixture[i].fldPosition);

            //                                //....Bolting = Front and Both. Position = Front
            //                                if (pPosition == "Front")
            //                                {                                             
            //                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0])
            //                                    {
            //                                        pBearingDBEntities = new BearingDBEntities();
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo = CheckDBString(pQryMounFixture[i].fldPartNo);

            //                                        String pPartNo_Front = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo;

            //                                        var pManf_SpliAndTurn_Front = (from pRec in pBearingDBEntities.tblManf_Fixture_SplitAndTurn where pRec.fldPartNo == pPartNo_Front select pRec).ToList();

            //                                        if (pManf_SpliAndTurn_Front.Count > 0)
            //                                        {
            //                                            for (int j = 0; j < pManf_SpliAndTurn_Front.Count; j++)
            //                                            {                                                            
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldDBC);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldDFinish);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count = CheckDBInt(pManf_SpliAndTurn_Front[j].fldCountHoles);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldAng_Start);

            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0] = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldAng_Other1);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1] = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldAng_Other2);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2] = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldAng_Other3);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3] = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldAng_Other4);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4] = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldAng_Other5);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5] = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldAng_Other6);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6] = CheckDBDouble(pManf_SpliAndTurn_Front[j].fldAng_Other7);

            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced = CheckDBBoolean(pManf_SpliAndTurn_Front[j].fldEqui_Spaced);

            //                                                //....Complimentary Angle start true or false.
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen = CheckDBBoolean(pQryMounFixture[i].fldHolesAngStart_Comp_Chosen);   //SB 23JUN09

            //                                                //.....Selected Thread.
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig = CheckDBString(pManf_SpliAndTurn_Front[j].fldDia_Desig);
            //                                            }
            //                                        }
            //                                    }

            //                                    else
            //                                    {
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC = CheckDBDouble(pQryMounFixture[i].fldDBC);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish = CheckDBDouble(pQryMounFixture[i].fldD_Finish);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count = CheckDBInt(pQryMounFixture[i].fldHolesCount);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced = CheckDBBool(pQryMounFixture[i].fldHolesEquispaced);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart = CheckDBDouble(pQryMounFixture[i].fldHolesAngStart);

            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther1);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther2);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther3);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther4);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther5);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther6);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther7);

            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig = CheckDBString(pQryMounFixture[i].fldScrew_Spec_D_Desig);

            //                                    }

            //                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type = CheckDBString(pQryMounFixture[i].fldScrew_Spec_Type);
            //                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L = CheckDBDouble(pQryMounFixture[i].fldScrew_Spec_L);
            //                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat = CheckDBString(pQryMounFixture[i].fldScrew_Spec_Mat);
            //                                }


            //                                //....Bolting = Back and Both. Position = Back
            //                                else if (pPosition == "Back")
            //                                {                                             

            //                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[1])
            //                                    {
            //                                        pBearingDBEntities = new BearingDBEntities();
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo = CheckDBString(pQryMounFixture[i].fldPartNo);

            //                                        String pPartNo_Back = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo;
            //                                        var pManf_SpliAndTurn_Back = (from pRec in pBearingDBEntities.tblManf_Fixture_SplitAndTurn where pRec.fldPartNo == pPartNo_Back select pRec).ToList();

            //                                        if (pManf_SpliAndTurn_Back.Count > 0)
            //                                        {
            //                                            for (int j = 0; j < pManf_SpliAndTurn_Back.Count; j++)
            //                                            {
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldDBC);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldDFinish);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count = CheckDBInt(pManf_SpliAndTurn_Back[j].fldCountHoles);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldAng_Start);

            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0] = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldAng_Other1);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1] = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldAng_Other2);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2] = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldAng_Other3);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3] = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldAng_Other4);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4] = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldAng_Other5);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5] = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldAng_Other6);
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6] = CheckDBDouble(pManf_SpliAndTurn_Back[j].fldAng_Other7);

            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced = CheckDBBoolean(pManf_SpliAndTurn_Back[j].fldEqui_Spaced);

            //                                                //....Complimentary Angle start true or false.
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen = CheckDBBoolean(pQryMounFixture[i].fldHolesAngStart_Comp_Chosen);   //SB 23JUN09

            //                                                //.....Selected Thread.
            //                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig = CheckDBString(pManf_SpliAndTurn_Back[j].fldDia_Desig);
            //                                            }
            //                                        }
            //                                    }

            //                                    else
            //                                    {
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC = CheckDBDouble(pQryMounFixture[i].fldDBC);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish = CheckDBDouble(pQryMounFixture[i].fldD_Finish);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count = CheckDBInt(pQryMounFixture[i].fldHolesCount);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced = CheckDBBool(pQryMounFixture[i].fldHolesEquispaced);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart = CheckDBDouble(pQryMounFixture[i].fldHolesAngStart);

            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther1);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther2);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther3);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther4);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther5);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther6);
            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6] = CheckDBDouble(pQryMounFixture[i].fldHolesAngOther7);

            //                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig = CheckDBString(pQryMounFixture[i].fldScrew_Spec_D_Desig);

            //                                    }
            //                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type = CheckDBString(pQryMounFixture[i].fldScrew_Spec_Type);
            //                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L = CheckDBDouble(pQryMounFixture[i].fldScrew_Spec_L);
            //                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat = CheckDBString(pQryMounFixture[i].fldScrew_Spec_Mat);
            //                                }
            //                            }
            //                        }
            //                    }
         
            //                #endregion

            //        #endregion

                 
            //        #endregion


            //        #region "Retrieval: End Configs Detail"

            //            public void Retrieve_EndConfigs_Type_ORM(clsProject Project_In, clsEndConfig.eType[] Type_Out)
            //                //=========================================================================================      //AES 28MAY18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";
            //                String pPosition = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfigs where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                for (int i = 0; i < pProject.Count; i++ )
            //                {
            //                    pPosition = CheckDBString(pProject[i].fldPosition);

            //                    int pIndx = 0;
            //                    if (pPosition == "Front")
            //                    {
            //                        pIndx = 0;
            //                    }
            //                    else if (pPosition == "Back")
            //                    {
            //                        pIndx = 1;
            //                    }

            //                    if (CheckDBString(pProject[i].fldType) != "")
            //                        Type_Out[pIndx] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType),
            //                                                                           CheckDBString(pProject[i].fldType));
            //                }
            //            }
                        
         
            //             private void RetrieveRec_EndConfigs_ORM(clsProject Project_In)
            //            //============================================================      //AES 28MAY18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";
            //                String pPosition = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfigs where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                for (int i = 0; i < pProject.Count; i++)
            //                {
            //                    pPosition = CheckDBString(pProject[i].fldPosition);// CheckDBString(pDR, "fldPosition");
            //                    int pIndx = 0;
            //                    if (pPosition == "Front")
            //                    {
            //                        pIndx = 0;
            //                    }
            //                    else if (pPosition == "Back")
            //                    {
            //                        pIndx = 1;
            //                    }

            //                    Project_In.Product.EndConfig[pIndx].Mat.Base = CheckDBString(pProject[i].fldMat_Base);// CheckDBString(pDR, "fldMat_Base");
            //                    Project_In.Product.EndConfig[pIndx].Mat.Lining = CheckDBString(pProject[i].fldMat_Lining);// CheckDBString(pDR, "fldMat_Lining");
            //                    Project_In.Product.EndConfig[pIndx].DO = CheckDBDouble(pProject[i].fldDO);// CheckDBDouble(pDR, "fldDO");
            //                    Project_In.Product.EndConfig[pIndx].DBore_Range[0] = CheckDBDouble(pProject[i].fldDBore_Range_Min);// CheckDBDouble(pDR, "fldDBore_Range_Min");
            //                    Project_In.Product.EndConfig[pIndx].DBore_Range[1] = CheckDBDouble(pProject[i].fldDBore_Range_Max);// CheckDBDouble(pDR, "fldDBore_Range_Max");
            //                    Project_In.Product.EndConfig[pIndx].L = CheckDBDouble(pProject[i].fldL);// CheckDBDouble(pDR, "fldL");

            //                    RetrieveRec_EndConfig_MountHoles_ORM(Project_In, pPosition); //RetrieveRec_EndConfig_MountHoles(Project_In, pPosition);//To be implemented

            //                    if (Project_In.Product.EndConfig[pIndx].Type == clsEndConfig.eType.Seal)
            //                    {
            //                        RetrieveRec_EndConfig_Seal_Detail_ORM(Project_In, pIndx); //RetrieveRec_EndConfig_Seal_Detail(Project_In, pIndx);//To be implemented
            //                    }
            //                    else if (Project_In.Product.EndConfig[pIndx].Type == clsEndConfig.eType.TL_TB)
            //                    {
            //                        RetrieveRec_EndConfig_TB_Detail_ORM(Project_In, pIndx); //RetrieveRec_EndConfig_TB_Detail(Project_In, pIndx);//To be implemented
            //                    }
            //                }
            //            }

                      
            //        #endregion


            //        #region "Retrieval: End Config - Mount Holes "

            //            private void RetrieveRec_EndConfig_MountHoles_ORM(clsProject Project_In, string Position_In)
            //            //==========================================================================================      //AES 30MAY18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";
                            
            //                int pIndx = 0;
            //                if (Position_In == "Front")
            //                {
            //                    pIndx = 0;
            //                }
            //                else if (Position_In == "Back")
            //                {
            //                    pIndx = 1;
            //                }

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_MountHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                for (int i = 0; i < pProject.Count; i++)
            //                {
            //                    if (CheckDBString(pProject[i].fldType)!="")//CheckDBString(pDR, "fldType") != "")
            //                        Project_In.Product.EndConfig[pIndx].MountHoles.Type = (clsEndConfig.clsMountHoles.eMountHolesType)
            //                                                                                    Enum.Parse(typeof(clsEndConfig.clsMountHoles.eMountHolesType),
            //                                                                                    CheckDBString(pProject[i].fldType));//CheckDBString(pDR, "fldType"));

            //                    (Project_In.Product.EndConfig[pIndx]).MountHoles.Depth_CBore = CheckDBDouble(pProject[i].fldDepth_CBore);// CheckDBDouble(pDR, "fldDepth_CBore");
            //                    (Project_In.Product.EndConfig[pIndx]).MountHoles.Thread_Thru = CheckDBBoolean(pProject[i].fldThread_Thru);// CheckDBBoolean(pDR, "fldThread_Thru");

            //                    (Project_In.Product.EndConfig[pIndx]).MountHoles.Depth_Thread = CheckDBDouble(pProject[i].fldDepth_Thread);// CheckDBDouble(pDR, "fldDepth_Thread");
            //                }
            //            }

                      
            //        #endregion


            //        #region "Retrieval: End Config - Seal Detail"

            //            private void RetrieveRec_EndConfig_Seal_Detail_ORM(clsProject Project_In, int PosIndx_In)
            //            //=======================================================================================      //AES 08JUN18
            //            {
            //                RetrieveRec_EndConfig_Seal_ORM(Project_In, PosIndx_In);
            //                RetrieveRec_EndConfig_Seal_Blade_ORM(Project_In, PosIndx_In);
            //                RetrieveRec_EndConfig_Seal_DrainHoles_ORM(Project_In, PosIndx_In);
            //                RetrieveRec_EndConfig_Seal_WireClipHoles_ORM(Project_In, PosIndx_In);
            //            }

                     

            //            #region "Retrieval: End Config - Seal"

            //                private void RetrieveRec_EndConfig_Seal_ORM(clsProject Project_In, int PosIndx_In)
            //                //================================================================================      //AES 08JUN18
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";
                            
            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++ )
            //                    {
            //                        if (CheckDBString(pProject[i].fldType)!="")//CheckDBString(pDR, "fldType") != "")
            //                            ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Design = (clsSeal.eDesign)
            //                                                                                        Enum.Parse(typeof(clsSeal.eDesign),
            //                                                                                        CheckDBString(pProject[i].fldType));//CheckDBString(pDR, "fldType"));

            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Mat_LiningT = CheckDBDouble(pProject[i].fldLiningT);// CheckDBDouble(pDR, "fldLiningT");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).TempSensor_D_ExitHole = CheckDBDouble(pProject[i].fldTempSensor_D_ExitHole);// CheckDBDouble(pDR, "fldTempSensor_D_ExitHole");
            //                    }
            //                }

            //                #endregion


            //            #region "Retrieval: End Config Seal - Blade"

            //                private void RetrieveRec_EndConfig_Seal_Blade_ORM(clsProject Project_In, int PosIndx_In)
            //                //======================================================================================      //AES 08JUN18              
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";

            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_Blade where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++)
            //                    {

            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Blade.Count = CheckDBInt(pProject[i].fldCount);// CheckDBInt(pDR, "fldCount");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Blade.T = CheckDBDouble(pProject[i].fldT);// CheckDBDouble(pDR, "fldT");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Blade.AngTaper = CheckDBDouble(pProject[i].fldAngTaper);// CheckDBDouble(pDR, "fldAngTaper");
            //                    }
            //                }

                           

            //                #endregion


            //            #region "Retrieval: End Config Seal - DrainHoles"

            //                private void RetrieveRec_EndConfig_Seal_DrainHoles_ORM(clsProject Project_In, int PosIndx_In)
            //                //===========================================================================================      //AES 08JUN18
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";

            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_DrainHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++)
            //                    {
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.Annulus_Ratio_L_H = CheckDBDouble(pProject[i].fldAnnulus_Ratio_L_H);// CheckDBDouble(pDR, "fldAnnulus_Ratio_L_H");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.Annulus_D = CheckDBDouble(pProject[i].fldAnnulus_D);// CheckDBDouble(pDR, "fldAnnulus_D");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.D_Desig = CheckDBString(pProject[i].fldD_Desig);// CheckDBString(pDR, "fldD_Desig");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.Count = CheckDBInt(pProject[i].fldCount);// CheckDBInt(pDR, "fldCount");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.AngBet = CheckDBDouble(pProject[i].fldAngBet);// CheckDBDouble(pDR, "fldAngBet");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.AngStart = CheckDBDouble(pProject[i].fldAngStart);// CheckDBDouble(pDR, "fldAngStart");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.AngExit = CheckDBDouble(pProject[i].fldAngExit);// CheckDBDouble(pDR, "fldAngExit");
            //                    }
            //                }

            //            #endregion


            //            #region "Retrieval: End Config Seal - WireClipHoles"

            //                private void RetrieveRec_EndConfig_Seal_WireClipHoles_ORM(clsProject Project_In, int PosIndx_In)
            //                //==============================================================================================      //AES 08JUN18 
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";

            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_WireClipHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++)
            //                    {
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Exists = CheckDBBoolean(pProject[i].fldExists);// CheckDBBoolean(pDR, "fldExists");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Count = CheckDBInt(pProject[i].fldCount);// CheckDBInt(pDR, "fldCount");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.DBC = CheckDBDouble(pProject[i].fldDBC);// CheckDBDouble(pDR, "fldDBC");

            //                        if (CheckDBString(pProject[i].fldUnitSystem)!="")//CheckDBString(pDR, "fldUnitSystem") != "")
            //                            ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Unit.System = (clsUnit.eSystem)
            //                                             Enum.Parse(typeof(clsUnit.eSystem), CheckDBString(pProject[i].fldUnitSystem));//CheckDBString(pDR, "fldUnitSystem"));

            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Screw_Spec.D_Desig = CheckDBString(pProject[i].fldThread_Dia_Desig);//CheckDBString(pDR, "fldThread_Dia_Desig");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Screw_Spec.Pitch = CheckDBDouble(pProject[i].fldThread_Pitch);//CheckDBDouble(pDR, "fldThread_Pitch");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.ThreadDepth = CheckDBDouble(pProject[i].fldThread_Depth);//CheckDBDouble(pDR, "fldThread_Depth");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.AngStart = CheckDBDouble(pProject[i].fldAngStart);//CheckDBDouble(pDR, "fldAngStart");
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.AngOther[0] = CheckDBDouble(pProject[i].fldAngOther1);//CheckDBDouble(pDR, "fldAngOther1"););
            //                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.AngOther[1] = CheckDBDouble(pProject[i].fldAngOther2);// CheckDBDouble(pDR, "fldAngOther2");
            //                    }
            //                }

            //            #endregion                        


            //        #endregion


            //        #region "Retrieval: End Config - TB Detail"

            //            private void RetrieveRec_EndConfig_TB_Detail_ORM(clsProject Project_In, int PosIndx_In)
            //                //=================================================================================      //AES 08JUN18
            //            {
            //                RetrieveRec_Bearing_Thrust_TL_ORM(Project_In, PosIndx_In);
            //                RetrieveRec_Bearing_Thrust_TL_PerformData_ORM(Project_In, PosIndx_In);
            //                RetrieveRec_Bearing_Thrust_TL_FeedGroove_ORM(Project_In, PosIndx_In);
            //                RetrieveRec_Bearing_Thrust_TL_WeepSlot_ORM(Project_In, PosIndx_In);
            //                RetrieveRec_Bearing_Thrust_TL_GCodes_ORM(Project_In, PosIndx_In);
            //            }

            //            #region "Retrieval: End Config - Thrust_TL"

            //                private void RetrieveRec_Bearing_Thrust_TL_ORM(clsProject Project_In, int PosIndx_In)
            //                //===================================================================================      //AES 08JUN18              
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";

            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++)
            //                    {
            //                        if (CheckDBString(pProject[i].fldDirectionType)!="")//CheckDBString(pDR, "fldDirectionType") != "")
            //                            ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).DirectionType = (clsBearing_Thrust_TL.eDirectionType)
            //                                                                                                             Enum.Parse(typeof(clsBearing_Thrust_TL.eDirectionType),
            //                                                                                                             CheckDBString(pProject[i].fldDirectionType));//CheckDBString(pDR, "fldDirectionType"));

            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PadD[0] = CheckDBDouble(pProject[i].fldPad_ID);//CheckDBDouble(pDR, "fldPad_ID");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PadD[1] = CheckDBDouble(pProject[i].fldPad_OD);// CheckDBDouble(pDR, "fldPad_OD");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).LandL = CheckDBDouble(pProject[i].fldLandL);// CheckDBDouble(pDR, "fldLandL");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).LiningT_Face = CheckDBDouble(pProject[i].fldLiningT_Face);// CheckDBDouble(pDR, "fldLiningT_Face");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).LiningT_ID = CheckDBDouble(pProject[i].fldLiningT_ID);// CheckDBDouble(pDR, "fldLiningT_ID");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Pad_Count = CheckDBInt(pProject[i].fldPad_Count);// CheckDBInt(pDR, "fldPad_Count");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Taper_Depth_OD = CheckDBDouble(pProject[i].fldTaper_Depth_OD);// CheckDBDouble(pDR, "fldTaper_Depth_OD");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Taper_Depth_ID = CheckDBDouble(pProject[i].fldTaper_Depth_ID);// CheckDBDouble(pDR, "fldTaper_Depth_ID");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Taper_Angle = CheckDBDouble(pProject[i].fldTaper_Angle);// CheckDBDouble(pDR, "fldTaper_Angle");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Shroud_Ro = CheckDBDouble(pProject[i].fldShroud_Ro);// CheckDBDouble(pDR, "fldShroud_Ro");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Shroud_Ri = CheckDBDouble(pProject[i].fldShroud_Ri);// CheckDBDouble(pDR, "fldShroud_Ri");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).BackRelief_Reqd = CheckDBBoolean(pProject[i].fldBackRelief_Reqd);// CheckDBBoolean(pDR, "fldBackRelief_Reqd");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).BackRelief_D = CheckDBDouble(pProject[i].fldBackRelief_D);// CheckDBDouble(pDR, "fldBackRelief_D");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).BackRelief_Depth = CheckDBDouble(pProject[i].fldBackRelief_Depth);// CheckDBDouble(pDR, "fldBackRelief_Depth");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).BackRelief_Fillet = CheckDBDouble(pProject[i].fldBackRelief_Fillet);// CheckDBDouble(pDR, "fldBackRelief_Fillet");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).LFlange = CheckDBDouble(pProject[i].fldLFlange);// CheckDBDouble(pDR, "fldLFlange");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FaceOff_Assy = CheckDBDouble(pProject[i].fldFaceOff_Assy);// CheckDBDouble(pDR, "fldFaceOff_Assy");

            //                    }
            //                }

            //            #endregion


            //            #region "Retrieval: End Config - Thrust_TL - Perform Data"

            //                private void RetrieveRec_Bearing_Thrust_TL_PerformData_ORM(clsProject Project_In, int PosIndx_In)
            //                //===============================================================================================      //AES 08JUN18
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";

            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_PerformData where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++)
            //                    {
            //                        // Perfor Data
            //                        // ------------
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.Power_HP = CheckDBDouble(pProject[i].fldPower_HP);// CheckDBDouble(pDR, "fldPower_HP");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.FlowReqd_gpm = CheckDBDouble(pProject[i].fldFlowReqd_gpm);// CheckDBDouble(pDR, "fldFlowReqd_gpm");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.TempRise_F = CheckDBDouble(pProject[i].fldTempRise_F);// CheckDBDouble(pDR, "fldTempRise_F");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.TFilm_Min = CheckDBDouble(pProject[i].fldTFilm_Min);// CheckDBDouble(pDR, "fldTFilm_Min");

            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.PadMax_Temp = CheckDBDouble(pProject[i].fldPadMax_Temp);// CheckDBDouble(pDR, "fldPadMax_Temp");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.PadMax_Press = CheckDBDouble(pProject[i].fldPadMax_Press);// CheckDBDouble(pDR, "fldPadMax_Press");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.UnitLoad = CheckDBDouble(pProject[i].fldUnitLoad);// CheckDBDouble(pDR, "fldUnitLoad");
            //                    }
            //                }

            //            #endregion


            //            #region "Retrieval: End Config - Thrust_TL:Feed Groove"

            //                private void RetrieveRec_Bearing_Thrust_TL_FeedGroove_ORM(clsProject Project_In, int PosIndx_In)
            //                //=============================================================================================      //AES 08JUN18 
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";

            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_FeedGroove where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++)
            //                    {
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.Type = CheckDBString(pProject[i].fldType);// CheckDBString(pDR, "fldType");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.Wid = CheckDBDouble(pProject[i].fldWid);// CheckDBDouble(pDR, "fldWid");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.Depth = CheckDBDouble(pProject[i].fldDepth);// CheckDBDouble(pDR, "fldDepth");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.DBC = CheckDBDouble(pProject[i].fldDBC);// CheckDBDouble(pDR, "fldDBC");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.Dist_Chamf = CheckDBDouble(pProject[i].fldDist_Chamf);// CheckDBDouble(pDR, "fldDist_Chamf");
            //                    }
            //                }

            //            #endregion


            //            #region "Retrieval: End Config - Thrust_TL:Weep Slot"

            //                private void RetrieveRec_Bearing_Thrust_TL_WeepSlot_ORM(clsProject Project_In, int PosIndx_In)
            //                //============================================================================================      //AES 08JUN18
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";

            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_WeepSlot where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++)
            //                    {
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).WeepSlot.Type = (clsBearing_Thrust_TL.clsWeepSlot.eType)
            //                                                                                                         Enum.Parse(typeof(clsBearing_Thrust_TL.clsWeepSlot.eType), CheckDBString(pProject[i].fldType));//CheckDBString(pDR, "fldType"));
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).WeepSlot.Wid = CheckDBDouble(pProject[i].fldWid);// CheckDBDouble(pDR, "fldWid");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).WeepSlot.Depth = CheckDBDouble(pProject[i].fldDepth);// CheckDBDouble(pDR, "fldDepth");
            //                    }
            //                }

            //            #endregion


            //            #region "Retrieval: End Config - Thrust_TL:GCodes"

            //                private void RetrieveRec_Bearing_Thrust_TL_GCodes_ORM(clsProject Project_In, int PosIndx_In)
            //                //==========================================================================================      //AES 08JUN18
            //                {
            //                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                    string pNoSuffix = "", pPosition = "";

            //                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                    {
            //                        pNoSuffix = Project_In.No_Suffix;
            //                    }
            //                    else
            //                    {
            //                        pNoSuffix = "NULL";
            //                    }
            //                    if (PosIndx_In == 0)
            //                    {
            //                        pPosition = "Front";
            //                    }
            //                    else
            //                    {
            //                        pPosition = "Back";
            //                    }
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_GCodes where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                    for (int i = 0; i < pProject.Count; i++)
            //                    {
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).T1.D_Desig = CheckDBString(pProject[i].fldD_Desig_T1);// CheckDBString(pDR, "fldD_Desig_T1");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).T2.D_Desig = CheckDBString(pProject[i].fldD_Desig_T2);// CheckDBString(pDR, "fldD_Desig_T2");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).T3.D_Desig = CheckDBString(pProject[i].fldD_Desig_T3);// CheckDBString(pDR, "fldD_Desig_T3");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).T4.D_Desig = CheckDBString(pProject[i].fldD_Desig_T4);// CheckDBString(pDR, "fldD_Desig_T4");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Overlap_frac = CheckDBDouble(pProject[i].fldOverlap_frac);// CheckDBDouble(pDR, "fldOverlap_frac");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedRate_Taperland = CheckDBDouble(pProject[i].fldFeed_Taperland);// CheckDBDouble(pDR, "fldFeed_Taperland");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedRate_WeepSlot = CheckDBDouble(pProject[i].fldFeed_WeepSlot);// CheckDBDouble(pDR, "fldFeed_WeepSlot");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Depth_TL_Backlash = CheckDBDouble(pProject[i].fldDepth_Backlash);// CheckDBDouble(pDR, "fldDepth_Backlash");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Depth_TL_Dwell_T = CheckDBDouble(pProject[i].fldDepth_Dwell_T);// CheckDBDouble(pDR, "fldDepth_Dwell_T");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Depth_WS_Cut_Per_Pass = CheckDBDouble(pProject[i].fldDepth_WeepSlot_Cut);// CheckDBDouble(pDR, "fldDepth_WeepSlot_Cut");
            //                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FilePath_Dir = CheckDBString(pProject[i].fldFilePath_Dir);// CheckDBString(pDR, "fldFilePath_Dir");
            //                    }
            //                }
                           
            //            #endregion

            //        #endregion


            //        #region "Retrieval: Accessories"

            //            private void RetrieveRec_Accessories_ORM(clsProject Project_In)
            //                //=============================================================      //AES 30MAY18              
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";

            //                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //                {
            //                    pNoSuffix = Project_In.No_Suffix;
            //                }
            //                else
            //                {
            //                    pNoSuffix = "NULL";
            //                }

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Product_Accessories where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();

            //                if (pProject.Count > 0)
            //                {
            //                    // Perfor Data
            //                    // ------------
            //                    Project_In.Product.Accessories.TempSensor_Supplied = CheckDBBoolean(pProject[0].fldTempSensor_Supplied);// CheckDBBoolean(pDR, "fldTempSensor_Supplied");
            //                    Project_In.Product.Accessories.TempSensor_Name = (clsAccessories.eTempSensorName)Enum.Parse(typeof(clsAccessories.eTempSensorName),
            //                                                                        CheckDBString(pProject[0].fldTempSensor_Name));//CheckDBString(pDR, "fldTempSensor_Name"));
            //                    Project_In.Product.Accessories.TempSensor_Count = CheckDBInt(pProject[0].fldTempSensor_Count);//CheckDBInt(pDR, "fldTempSensor_Count");
            //                    Project_In.Product.Accessories.TempSensor_Type = (clsAccessories.eTempSensorType)Enum.Parse(typeof(clsAccessories.eTempSensorType),
            //                                                                        CheckDBString(pProject[0].fldTempSensor_Type));//CheckDBString(pDR, "fldTempSensor_Type"));

            //                    Project_In.Product.Accessories.WireClip_Supplied = CheckDBBoolean(pProject[0].fldWireClip_Supplied);// CheckDBBoolean(pDR, "fldWireClip_Supplied");
            //                    Project_In.Product.Accessories.WireClip_Count = CheckDBInt(pProject[0].fldWireClip_Count);// CheckDBInt(pDR, "fldWireClip_Count");
            //                    Project_In.Product.Accessories.WireClip_Size = (clsAccessories.eWireClipSize)Enum.Parse(typeof(clsAccessories.eWireClipSize),
            //                                                                    CheckDBString(pProject[0].fldWireClip_Size));//CheckDBString(pDR, "fldWireClip_Size"));
            //                }
            //            }
                       
            //        #endregion

            //    #endregion


            //    ////#region "Save Global Data"
            //    //////------------------------

            //    ////    public void SaveData_Global ()
            //    ////    //============================
            //    ////    {
            //    ////        //if (modMain.gDB.ProjectNo_Exists(modMain.gProject.No, modMain.gProject.No_Suffix))
            //    ////        //{
            //    ////        //    modMain.gDB.UpdateRecord(modMain.gProject, modMain.gOpCond);
            //    ////        //}
            //    ////        //else
            //    ////        //{
            //    ////        //    modMain.gDB.SaveToDB_ORM(modMain.gProject, modMain.gOpCond);
            //    ////        //}
            //    ////        modMain.gDB.SaveToDB_ORM(modMain.gProject, modMain.gOpCond);
            //    ////    }


            //    ////#endregion


            //    //#region "DATABASE MODIFICATION (INSERTION & UPDATION) ROUTINES:"

            //    //    public void SaveToDB_ORM(clsProject Project_In, clsOpCond OpCond_In)
            //    //    //==================================================================        //AES 31MAY18
            //    //    {
            //    //        SaveToDB_Project_ORM(Project_In);
            //    //        SaveToDB_Product_ORM(Project_In);
            //    //        SaveToDB_OpCond_ORM(Project_In, OpCond_In);
            //    //        SaveToDB_Bearing_Radial_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_Detail_ORM(Project_In);
            //    //        SaveToDB_EndConfigs_ORM(Project_In);
            //    //        SaveToDB_EndConfigs_MountHoles_ORM(Project_In);

            //    //        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
            //    //            Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)

            //    //        SaveToDB_EndConfig_Seal_Detail_ORM(Project_In);

            //    //        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.TL_TB ||
            //    //            Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.TL_TB)

            //    //            SaveToDB_EndConfig_TB_Detail_ORM(Project_In);

            //    //        SaveToDB_Project_Product_Accessories_ORM(Project_In);
            //    //    }


            //    //    //private void SaveToDB_Project_ORM(clsProject Project_In)
            //    //    ////======================================================        
            //    //    //{
            //    //    //    BearingDBEntities pBearingDBEntities = new BearingDBEntities();

            //    //    //    //....Customer
            //    //    //    int pCustCount = (from pRec in pBearingDBEntities.tblCustomer
            //    //    //                         where pRec.fldName == Project_In.Customer.Name select pRec).Count();

            //    //    //    int pCustID = 0;

            //    //    //    if (pCustCount > 0)
            //    //    //    {
            //    //    //        //....Record already exists Update record
            //    //    //        var pCust = (from pRec in pBearingDBEntities.tblCustomer where pRec.fldName == Project_In.Customer.Name  select pRec).First();
            //    //    //        pCustID = pCust.fldID;
            //    //    //        pCust.fldOrderNo = Project_In.Customer.OrderNo;
            //    //    //        pCust.fldMachineName = Project_In.Customer.MachineName;
            //    //    //        pCust.fldUnit = Project_In.Customer.Unit;
            //    //    //        pBearingDBEntities.SaveChanges();
            //    //    //    }
            //    //    //    else
            //    //    //    {
            //    //    //        var pCust = (from pRec in pBearingDBEntities.tblCustomer orderby pRec.fldID descending select pRec).First();
            //    //    //        pCustID = pCust.fldID +1;

            //    //    //        //....New Record
            //    //    //        tblCustomer pCustomer = new tblCustomer();
            //    //    //        pCustomer.fldID = pCustID;
            //    //    //        pCustomer.fldName = Project_In.Customer.Name;
            //    //    //        pCustomer.fldOrderNo = Project_In.Customer.OrderNo;
            //    //    //        pCustomer.fldMachineName = Project_In.Customer.MachineName;
            //    //    //        pCustomer.fldUnit = Project_In.Customer.Unit;

            //    //    //        pBearingDBEntities.AddTotblCustomer(pCustomer);
            //    //    //        pBearingDBEntities.SaveChanges();
            //    //    //    }


            //    //    //    //....Sales Order
            //    //    //    int pSOCount = (from pRec in pBearingDBEntities.tblSalesOrder
            //    //    //                      where pRec.fldNo == Project_In.SalesOrder.No
            //    //    //                      select pRec).Count();


            //    //    //    if (pSOCount > 0)
            //    //    //    {
            //    //    //        //....Record already exists Update record
            //    //    //        var pSONo = (from pRec in pBearingDBEntities.tblSalesOrder where pRec.fldNo == Project_In.SalesOrder.No
            //    //    //                     select pRec).First();

            //    //    //        pSONo.fldRelatedSONo= Project_In.SalesOrder.RelatedNo;
            //    //    //        pSONo.fldType = Project_In.SalesOrder.Type;
                            
            //    //    //        pBearingDBEntities.SaveChanges();

            //    //    //        //LineNo
            //    //    //        int pLineCount = (from pRec in pBearingDBEntities.tblLine
            //    //    //                        where pRec.fldSONo == Project_In.SalesOrder.No && pRec.fldNo = 
            //    //    //                        select pRec).Count();

            //    //    //    }
            //    //    //    else
            //    //    //    {
            //    //    //        //....New Record
            //    //    //        tblSalesOrder pSO = new tblSalesOrder();
            //    //    //        pSO.fldNo = Project_In.SalesOrder.No;
            //    //    //        pSO.fldRelatedSONo = Project_In.SalesOrder.RelatedNo;
            //    //    //        pSO.fldType = Project_In.SalesOrder.Type;

            //    //    //        pBearingDBEntities.AddTotblSalesOrder(pSO);
            //    //    //        pBearingDBEntities.SaveChanges();

            //    //    //        //....LineNo
                            
            //    //    //        tblLine pLine = new tblLine();
            //    //    //        pLine.fldSONo = Project_In.SalesOrder.No;
            //    //    //        pLine.fldNo = Project_In.SalesOrder.LineNo;

            //    //    //        pBearingDBEntities.AddTotblLine(pLine);
            //    //    //        pBearingDBEntities.SaveChanges();
            //    //    //    }



            //    //    //    int pProjectCount = (from pRec in pBearingDBEntities.tblProject where pRec.fldSONo == Project_In.SalesOrder.No && 
            //    //    //                             pRec.fldLineNo == Project_In.SalesOrder.LineNo && pRec.fldPartNo = Project_In.PartNo select pRec).Count();


                       
            //    //    //    int pProjectCount  = (from pRec in pBearingDBEntities.tblProject_Details where pRec.fldNo == Project_In.No && pRec.fldNo_Suffix == pNoSuffix select pRec).Count();

            //    //    //    if (pProjectCount > 0)
            //    //    //    {
            //    //    //        //....Record already exists Update record
            //    //    //        var pProject = (from pRec in pBearingDBEntities.tblProject where pRec.fldNo == Project_In.No && pRec.fldNo_Suffix == pNoSuffix select pRec).First();
            //    //    //        pProject.fldSONo = Project_In.SalesOrder.No;
            //    //    //        pProject.fldLineNo = Project_In.SalesOrder.LineNo;
            //    //    //        pProject.fldStatus = Project_In.Status;
                            
            //    //    //        pProject.fldCustomer_Name = Project_In.Customer.Name;
            //    //    //        pProject.fldCustomer_MachineDesc = Project_In.Customer.MachineName;
            //    //    //        pProject.fldCustomer_PartNo = Project_In.Customer.OrderNo;
            //    //    //        pProject.fldUnitSystem = Project_In.Unit.System.ToString();
            //    //    //        pProject.fldAssyDwg_No = Project_In.AssyDwg.No;
            //    //    //        pProject.fldAssyDwg_No_Suffix = Project_In.AssyDwg.No_Suffix;
            //    //    //        pProject.fldAssyDwg_Ref = Project_In.AssyDwg.Ref;
            //    //    //        pProject.fldEngg_Name = Project_In.Engg.Name;
            //    //    //        pProject.fldEngg_Initials = Project_In.Engg.Initials;
            //    //    //        pProject.fldEngg_Date = pEnggDate;
            //    //    //        pProject.fldDesignedBy_Name = Project_In.DesignedBy.Name;
            //    //    //        pProject.fldDesignedBy_Initials = Project_In.DesignedBy.Initials;
            //    //    //        pProject.fldDesignedBy_Date = pDesignedByDate;
            //    //    //        pProject.fldCheckedBy_Name = Project_In.CheckedBy.Name;
            //    //    //        pProject.fldCheckedBy_Initials = Project_In.CheckedBy.Initials;
            //    //    //        pProject.fldCheckedBy_Date = pCheckedByDate;
            //    //    //        pProject.fldFilePath_Project = Project_In.FilePath_Project;
            //    //    //        pProject.fldFilePath_DesignTbls_SWFiles = Project_In.FilePath_DesignTbls_SWFiles;
            //    //    //        pProject.fldFileModified_CompleteAssy = pFileModified_CompleteAssy;
            //    //    //        pProject.fldFileModified_Radial_Part = pFileModified_RadialPart;
            //    //    //        pProject.fldFileModified_Radial_BlankAssy = pFileModified_RadialBlankAssy;
            //    //    //        pProject.fldFileModified_EndTB_Part = pFileModified_EndTB_Part;
            //    //    //        pProject.fldFileModified_EndTB_Assy = pFileModified_EndTB_Assy;
            //    //    //        pProject.fldFileModified_EndSeal_Part = pFileModified_EndSeal_Part;
            //    //    //        pProject.fldFileModification_Notes = Project_In.FileModification_Notes;

            //    //    //        pBearingDBEntities.SaveChanges();
            //    //    //    }
            //    //    //     else
            //    //    //    {
            //    //    //        //....New Record
            //    //    //        tblProject_Details pProject_Details = new tblProject_Details();
            //    //    //        pProject_Details.fldNo = Project_In.No;
            //    //    //        pProject_Details.fldNo_Suffix = pNoSuffix;
            //    //    //        pProject_Details.fldStatus = Project_In.Status;
            //    //    //        pProject_Details.fldCustomer_Name = Project_In.Customer.Name;
            //    //    //        pProject_Details.fldCustomer_MachineDesc = Project_In.Customer.MachineName;
            //    //    //        pProject_Details.fldCustomer_PartNo = Project_In.Customer.OrderNo;
            //    //    //        pProject_Details.fldUnitSystem = Project_In.Unit.System.ToString();
            //    //    //        pProject_Details.fldAssyDwg_No = Project_In.AssyDwg.No;
            //    //    //        pProject_Details.fldAssyDwg_No_Suffix = Project_In.AssyDwg.No_Suffix;
            //    //    //        pProject_Details.fldAssyDwg_Ref = Project_In.AssyDwg.Ref;
            //    //    //        pProject_Details.fldEngg_Name = Project_In.Engg.Name;
            //    //    //        pProject_Details.fldEngg_Initials = Project_In.Engg.Initials;
            //    //    //        pProject_Details.fldEngg_Date = pEnggDate;
            //    //    //        pProject_Details.fldDesignedBy_Name = Project_In.DesignedBy.Name;
            //    //    //        pProject_Details.fldDesignedBy_Initials = Project_In.DesignedBy.Initials;
            //    //    //        pProject_Details.fldDesignedBy_Date = pDesignedByDate;
            //    //    //        pProject_Details.fldCheckedBy_Name = Project_In.CheckedBy.Name;
            //    //    //        pProject_Details.fldCheckedBy_Initials = Project_In.CheckedBy.Initials;
            //    //    //        pProject_Details.fldCheckedBy_Date = pCheckedByDate;
            //    //    //        pProject_Details.fldFilePath_Project = Project_In.FilePath_Project;
            //    //    //        pProject_Details.fldFilePath_DesignTbls_SWFiles = Project_In.FilePath_DesignTbls_SWFiles;
            //    //    //        pProject_Details.fldFileModified_CompleteAssy = pFileModified_CompleteAssy;
            //    //    //        pProject_Details.fldFileModified_Radial_Part = pFileModified_RadialPart;
            //    //    //        pProject_Details.fldFileModified_Radial_BlankAssy = pFileModified_RadialBlankAssy;
            //    //    //        pProject_Details.fldFileModified_EndTB_Part = pFileModified_EndTB_Part;
            //    //    //        pProject_Details.fldFileModified_EndTB_Assy = pFileModified_EndTB_Assy;
            //    //    //        pProject_Details.fldFileModified_EndSeal_Part = pFileModified_EndSeal_Part;
            //    //    //        pProject_Details.fldFileModification_Notes = Project_In.FileModification_Notes;

            //    //    //        pBearingDBEntities.AddTotblProject_Details(pProject_Details);
            //    //    //        pBearingDBEntities.SaveChanges();
            //    //    //    }   
            //    //    //}

            //    //    private void SaveToDB_Product_ORM(clsProject Project_In)
            //    //    //=====================================================        //AES 31MAY18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Product where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Product = (from pRec in pBearingDBEntities.tblProject_Product where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Product.fldProjectNo = Project_In.No;
            //    //            pProject_Product.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Product.fldBearing_Type = Project_In.Product.Type.ToString();
            //    //            pProject_Product.fldL_Available = (Decimal)Project_In.Product.L_Available;
            //    //            pProject_Product.fldDist_ThrustFace_Front = (Decimal)Project_In.Product.Dist_ThrustFace[0];
            //    //            pProject_Product.fldDist_ThrustFace_Back = (Decimal)Project_In.Product.Dist_ThrustFace[1];
                            

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Product pProject_Product = new tblProject_Product();
            //    //            pProject_Product.fldProjectNo = Project_In.No;
            //    //            pProject_Product.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Product.fldBearing_Type = Project_In.Product.Type.ToString();
            //    //            pProject_Product.fldL_Available = (Decimal)Project_In.Product.L_Available;
            //    //            pProject_Product.fldDist_ThrustFace_Front = (Decimal)Project_In.Product.Dist_ThrustFace[0];
            //    //            pProject_Product.fldDist_ThrustFace_Back = (Decimal)Project_In.Product.Dist_ThrustFace[1];

            //    //            pBearingDBEntities.AddTotblProject_Product(pProject_Product);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }                           
            //    //    }

            //    //    private void SaveToDB_OpCond_ORM(clsProject Project_In, clsOpCond OpCond_In)
            //    //    //==========================================================================        //AES 31MAY18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        OpCond_In.OilSupply_Type = "Pressurized";
            //    //        OpCond_In.OilSupply_Lube_Type = "ISO 32";
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_OpCond where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_OpCond = (from pRec in pBearingDBEntities.tblProject_OpCond where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_OpCond.fldProjectNo = Project_In.No;
            //    //            pProject_OpCond.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_OpCond.fldSpeed_Range_Min = OpCond_In.Speed_Range[0];
            //    //            pProject_OpCond.fldSpeed_Range_Max = OpCond_In.Speed_Range[1];
            //    //            pProject_OpCond.fldRot_Directionality = OpCond_In.Rot_Directionality.ToString();
            //    //            pProject_OpCond.fldRadial_Load_Range_Min = (Decimal)OpCond_In.Radial_Load_Range[0];
            //    //            pProject_OpCond.fldRadial_Load_Range_Max = (Decimal)OpCond_In.Radial_Load_Range[1];
            //    //            pProject_OpCond.fldRadial_LoadAng = (Decimal)OpCond_In.Radial_LoadAng;
            //    //            pProject_OpCond.fldThrust_Load_Range_Front_Min = (Decimal)OpCond_In.Thrust_Load_Range_Front[0];
            //    //            pProject_OpCond.fldThrust_Load_Range_Front_Max = (Decimal)OpCond_In.Thrust_Load_Range_Front[1];
            //    //            pProject_OpCond.fldThrust_Load_Range_Back_Min = (Decimal)OpCond_In.Thrust_Load_Range_Back[0];
            //    //            pProject_OpCond.fldThrust_Load_Range_Back_Max = (Decimal)OpCond_In.Thrust_Load_Range_Back[1];
            //    //            pProject_OpCond.fldOilSupply_Lube_Type = OpCond_In.OilSupply.Lube.Type;
            //    //            pProject_OpCond.fldOilSupply_Type = OpCond_In.OilSupply.Type;
            //    //            pProject_OpCond.fldOilSupply_Press = (Decimal)OpCond_In.OilSupply.Press;
            //    //            pProject_OpCond.fldOilSupply_Temp = (Decimal)OpCond_In.OilSupply.Temp;


            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_OpCond pProject_OpCond = new tblProject_OpCond();
            //    //            pProject_OpCond.fldProjectNo = Project_In.No;
            //    //            pProject_OpCond.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_OpCond.fldSpeed_Range_Min = OpCond_In.Speed_Range[0];
            //    //            pProject_OpCond.fldSpeed_Range_Max = OpCond_In.Speed_Range[1];
            //    //            pProject_OpCond.fldRot_Directionality = OpCond_In.Rot_Directionality.ToString();
            //    //            pProject_OpCond.fldRadial_Load_Range_Min = (Decimal)OpCond_In.Radial_Load_Range[0];
            //    //            pProject_OpCond.fldRadial_Load_Range_Max = (Decimal)OpCond_In.Radial_Load_Range[1];
            //    //            pProject_OpCond.fldRadial_LoadAng = (Decimal)OpCond_In.Radial_LoadAng;
            //    //            pProject_OpCond.fldThrust_Load_Range_Front_Min = (Decimal)OpCond_In.Thrust_Load_Range_Front[0];
            //    //            pProject_OpCond.fldThrust_Load_Range_Front_Max = (Decimal)OpCond_In.Thrust_Load_Range_Front[1];
            //    //            pProject_OpCond.fldThrust_Load_Range_Back_Min = (Decimal)OpCond_In.Thrust_Load_Range_Back[0];
            //    //            pProject_OpCond.fldThrust_Load_Range_Back_Max = (Decimal)OpCond_In.Thrust_Load_Range_Back[1];
            //    //            pProject_OpCond.fldOilSupply_Lube_Type = OpCond_In.OilSupply.Lube.Type;
            //    //            pProject_OpCond.fldOilSupply_Type = OpCond_In.OilSupply.Type;
            //    //            pProject_OpCond.fldOilSupply_Press = (Decimal)OpCond_In.OilSupply.Press;
            //    //            pProject_OpCond.fldOilSupply_Temp = (Decimal)OpCond_In.OilSupply.Temp;

            //    //            pBearingDBEntities.AddTotblProject_OpCond(pProject_OpCond);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_ORM(clsProject Project_In)
            //    //    //=============================================================        //AES 31MAY18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial.fldDesign = ((clsBearing_Radial)Project_In.Product.Bearing).Design.ToString();
                            
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial pProject_Bearing_Radial = new tblProject_Bearing_Radial();
            //    //            pProject_Bearing_Radial.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial.fldDesign = ((clsBearing_Radial)Project_In.Product.Bearing).Design.ToString();

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial(pProject_Bearing_Radial);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_Detail_ORM(clsProject Project_In)
            //    //    //=======================================================================        //AES 01JUN18
            //    //    {
            //    //        SaveToDB_Bearing_Radial_FP_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_PerformData_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_Pad_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_FlexurePivot_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_OilInlet_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_MillRelief_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_Flange_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_SL_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_AntiRotPin_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_Mount_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_TempSensor_ORM(Project_In);
            //    //        SaveToDB_Bearing_Radial_FP_EDM_Pad_ORM(Project_In);
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_ORM(clsProject Project_In)
            //    //    //================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pSplitConfig = false;
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig) pSplitConfig = true;

            //    //            Boolean pMat_LiningExists = false;
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.LiningExists) pMat_LiningExists = true;

            //    //            //  Set Default Value:          
            //    //            //  ------------------
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base == null || ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base == "")
            //    //                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base = "Steel 4340";

            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining == null || ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining == "")
            //    //                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining = "Babbitt";

            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT == 0.00F)
            //    //                ((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT = 0.020F;
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP.fldSplitConfig = pSplitConfig;
            //    //            pProject_Bearing_Radial_FP.fldDShaft_Range_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0];
            //    //            pProject_Bearing_Radial_FP.fldDShaft_Range_Max = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1];
            //    //            pProject_Bearing_Radial_FP.fldDFit_Range_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0];
            //    //            pProject_Bearing_Radial_FP.fldDFit_Range_Max = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1];
            //    //            pProject_Bearing_Radial_FP.fldDSet_Range_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0];
            //    //            pProject_Bearing_Radial_FP.fldDSet_Range_Max = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1];
            //    //            pProject_Bearing_Radial_FP.fldDPad_Range_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0];
            //    //            pProject_Bearing_Radial_FP.fldDPad_Range_Max = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1];
            //    //            pProject_Bearing_Radial_FP.fldL = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).L;
            //    //            pProject_Bearing_Radial_FP.fldDepth_EndConfig_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[0];
            //    //            pProject_Bearing_Radial_FP.fldDepth_EndConfig_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[1];
            //    //            pProject_Bearing_Radial_FP.fldDimStart_FrontFace = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DimStart_FrontFace;
            //    //            pProject_Bearing_Radial_FP.fldMat_Base = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base;
            //    //            pProject_Bearing_Radial_FP.fldMat_LiningExists = pMat_LiningExists;
            //    //            pProject_Bearing_Radial_FP.fldMat_Lining = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining;
            //    //            pProject_Bearing_Radial_FP.fldLiningT = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT;
                            

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP pProject_Bearing_Radial_FP = new tblProject_Bearing_Radial_FP();
            //    //            pProject_Bearing_Radial_FP.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP.fldSplitConfig = pSplitConfig;
            //    //            pProject_Bearing_Radial_FP.fldDShaft_Range_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0];
            //    //            pProject_Bearing_Radial_FP.fldDShaft_Range_Max = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1];
            //    //            pProject_Bearing_Radial_FP.fldDFit_Range_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0];
            //    //            pProject_Bearing_Radial_FP.fldDFit_Range_Max = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1];
            //    //            pProject_Bearing_Radial_FP.fldDSet_Range_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0];
            //    //            pProject_Bearing_Radial_FP.fldDSet_Range_Max = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1];
            //    //            pProject_Bearing_Radial_FP.fldDPad_Range_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0];
            //    //            pProject_Bearing_Radial_FP.fldDPad_Range_Max = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1];
            //    //            pProject_Bearing_Radial_FP.fldL = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).L;
            //    //            pProject_Bearing_Radial_FP.fldDepth_EndConfig_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[0];
            //    //            pProject_Bearing_Radial_FP.fldDepth_EndConfig_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[1];
            //    //            pProject_Bearing_Radial_FP.fldDimStart_FrontFace = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).DimStart_FrontFace;
            //    //            pProject_Bearing_Radial_FP.fldMat_Base = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base;
            //    //            pProject_Bearing_Radial_FP.fldMat_LiningExists = pMat_LiningExists;
            //    //            pProject_Bearing_Radial_FP.fldMat_Lining = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining;
            //    //            pProject_Bearing_Radial_FP.fldLiningT = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP(pProject_Bearing_Radial_FP);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }                           
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_PerformData_ORM(clsProject Project_In)
            //    //    //============================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_PerformData where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_PerformData = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_PerformData where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_PerformData.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPower_HP = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldFlowReqd_gpm = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldTempRise_F = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldTFilm_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TFilm_Min;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Temp = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Temp;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Press = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Press;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Rot = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Rot;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Load = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Load;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Stress = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Stress;
                            
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_PerformData pProject_Bearing_Radial_FP_PerformData = new tblProject_Bearing_Radial_FP_PerformData();
            //    //            pProject_Bearing_Radial_FP_PerformData.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPower_HP = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldFlowReqd_gpm = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldTempRise_F = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldTFilm_Min = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TFilm_Min;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Temp = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Temp;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Press = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Press;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Rot = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Rot;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Load = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Load;
            //    //            pProject_Bearing_Radial_FP_PerformData.fldPadMax_Stress = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Stress;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_PerformData(pProject_Bearing_Radial_FP_PerformData);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_Pad_ORM(clsProject Project_In)
            //    //    //====================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Pad where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_Pad = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Pad where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_Pad.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_Pad.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_Pad.fldType = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type.ToString();
            //    //            pProject_Bearing_Radial_FP_Pad.fldCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count;
            //    //            pProject_Bearing_Radial_FP_Pad.fldL = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L;
            //    //            pProject_Bearing_Radial_FP_Pad.fldPivot_Offset = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset;
            //    //            pProject_Bearing_Radial_FP_Pad.fldPivot_AngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.AngStart;
            //    //            pProject_Bearing_Radial_FP_Pad.fldT_Lead = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead;
            //    //            pProject_Bearing_Radial_FP_Pad.fldT_Pivot = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot;
            //    //            pProject_Bearing_Radial_FP_Pad.fldT_Trail = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail;
                            
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_Pad pProject_Bearing_Radial_FP_Pad = new tblProject_Bearing_Radial_FP_Pad();
            //    //            pProject_Bearing_Radial_FP_Pad.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_Pad.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_Pad.fldType = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type.ToString();
            //    //            pProject_Bearing_Radial_FP_Pad.fldCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count;
            //    //            pProject_Bearing_Radial_FP_Pad.fldL = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L;
            //    //            pProject_Bearing_Radial_FP_Pad.fldPivot_Offset = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset;
            //    //            pProject_Bearing_Radial_FP_Pad.fldPivot_AngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.AngStart;
            //    //            pProject_Bearing_Radial_FP_Pad.fldT_Lead = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead;
            //    //            pProject_Bearing_Radial_FP_Pad.fldT_Pivot = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot;
            //    //            pProject_Bearing_Radial_FP_Pad.fldT_Trail = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_Pad(pProject_Bearing_Radial_FP_Pad);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_FlexurePivot_ORM(clsProject Project_In)
            //    //    //============================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_FlexurePivot where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_FlexurePivot = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_FlexurePivot where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldWeb_T = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldWeb_H = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldWeb_RFillet = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldGapEDM = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldRot_Stiff = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Rot_Stiff;

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_FlexurePivot pProject_Bearing_Radial_FP_FlexurePivot = new tblProject_Bearing_Radial_FP_FlexurePivot();
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldWeb_T = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldWeb_H = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldWeb_RFillet = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldGapEDM = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM;
            //    //            pProject_Bearing_Radial_FP_FlexurePivot.fldRot_Stiff = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Rot_Stiff;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_FlexurePivot(pProject_Bearing_Radial_FP_FlexurePivot);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_OilInlet_ORM(clsProject Project_In)
            //    //    //=========================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pAnnulus_Exists = false;
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Exists)
            //    //                pAnnulus_Exists = true;
            //    //            int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_OilInlet where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_OilInlet = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_OilInlet where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_Count = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_D = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldCount_MainOilSupply = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_StartPos = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos.ToString();
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_DDrill_CBore = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.DDrill_CBore;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_Loc_FrontFace = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Loc_FrontFace;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.L;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_AngStart_BDV = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.AngStart_BDV;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldAnnulus_Exists = pAnnulus_Exists;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldAnnulus_D = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldAnnulus_Loc_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Loc_Back;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldAnnulus_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.L;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_Dist_Holes = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Dist_Holes;
                            
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_OilInlet pProject_Bearing_Radial_FP_OilInlet = new tblProject_Bearing_Radial_FP_OilInlet();
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_Count = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_D = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldCount_MainOilSupply = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_StartPos = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos.ToString();
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_DDrill_CBore = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.DDrill_CBore;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_Loc_FrontFace = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Loc_FrontFace;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.L;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_AngStart_BDV = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.AngStart_BDV;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldAnnulus_Exists = pAnnulus_Exists;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldAnnulus_D = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldAnnulus_Loc_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Loc_Back;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldAnnulus_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.L;
            //    //            pProject_Bearing_Radial_FP_OilInlet.fldOrifice_Dist_Holes = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Dist_Holes;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_OilInlet(pProject_Bearing_Radial_FP_OilInlet);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_MillRelief_ORM(clsProject Project_In)
            //    //    //===========================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pMillRelief = false;
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_MillRelief where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_MillRelief = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_MillRelief where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldExists = pMillRelief;
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldD_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_Desig;
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldEDMRelief_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[0];
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldEDMRelief_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[1];
                            
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_MillRelief pProject_Bearing_Radial_FP_MillRelief = new tblProject_Bearing_Radial_FP_MillRelief();
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldExists = pMillRelief;
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldD_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_Desig;
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldEDMRelief_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[0];
            //    //            pProject_Bearing_Radial_FP_MillRelief.fldEDMRelief_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[1];

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_MillRelief(pProject_Bearing_Radial_FP_MillRelief);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_Flange_ORM(clsProject Project_In)
            //    //    //=======================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pFlange_Exists = false;
            //    //        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists)
            //    //            pFlange_Exists = true;
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Flange where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_Flange = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Flange where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_Flange.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_Flange.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_Flange.fldExists = pFlange_Exists;
            //    //            pProject_Bearing_Radial_FP_Flange.fldD = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D;
            //    //            pProject_Bearing_Radial_FP_Flange.fldWid = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid;
            //    //            pProject_Bearing_Radial_FP_Flange.fldDimStart_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Front;

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_Flange pProject_Bearing_Radial_FP_Flange = new tblProject_Bearing_Radial_FP_Flange();
            //    //            pProject_Bearing_Radial_FP_Flange.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_Flange.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_Flange.fldExists = pFlange_Exists;
            //    //            pProject_Bearing_Radial_FP_Flange.fldD = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D;
            //    //            pProject_Bearing_Radial_FP_Flange.fldWid = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid;
            //    //            pProject_Bearing_Radial_FP_Flange.fldDimStart_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Front;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_Flange(pProject_Bearing_Radial_FP_Flange);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_SL_ORM(clsProject Project_In)
            //    //    //===================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_SL where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_SL = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_SL where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_SL.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_SL.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_UnitSystem = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System.ToString();
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Type;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_Pitch = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Mat;
            //    //            pProject_Bearing_Radial_FP_SL.fldLScrew_Spec_Loc_Center = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Center;
            //    //            pProject_Bearing_Radial_FP_SL.fldLScrew_Spec_Loc_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Front;
            //    //            pProject_Bearing_Radial_FP_SL.fldRScrew_Spec_Loc_Center = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Center;
            //    //            pProject_Bearing_Radial_FP_SL.fldRScrew_Spec_Loc_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Front;
            //    //            pProject_Bearing_Radial_FP_SL.fldThread_Depth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Thread_Depth;
            //    //            pProject_Bearing_Radial_FP_SL.fldCBore_Depth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.CBore_Depth;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_UnitSystem = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System.ToString();
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Type;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.L;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Mat;
            //    //            pProject_Bearing_Radial_FP_SL.fldLDowel_Spec_Loc_Center = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Center;
            //    //            pProject_Bearing_Radial_FP_SL.fldLDowel_Spec_Loc_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Front;
            //    //            pProject_Bearing_Radial_FP_SL.fldRDowel_Spec_Loc_Center = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Center;
            //    //            pProject_Bearing_Radial_FP_SL.fldRDowel_Spec_Loc_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Front;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Depth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Depth;

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_SL pProject_Bearing_Radial_FP_SL = new tblProject_Bearing_Radial_FP_SL();
            //    //            pProject_Bearing_Radial_FP_SL.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_SL.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_UnitSystem = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System.ToString();
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Type;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_Pitch = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L;
            //    //            pProject_Bearing_Radial_FP_SL.fldScrew_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Mat;
            //    //            pProject_Bearing_Radial_FP_SL.fldLScrew_Spec_Loc_Center = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Center;
            //    //            pProject_Bearing_Radial_FP_SL.fldLScrew_Spec_Loc_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Front;
            //    //            pProject_Bearing_Radial_FP_SL.fldRScrew_Spec_Loc_Center = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Center;
            //    //            pProject_Bearing_Radial_FP_SL.fldRScrew_Spec_Loc_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Front;
            //    //            pProject_Bearing_Radial_FP_SL.fldThread_Depth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Thread_Depth;
            //    //            pProject_Bearing_Radial_FP_SL.fldCBore_Depth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.CBore_Depth;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_UnitSystem = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System.ToString();
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Type;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.L;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Mat;
            //    //            pProject_Bearing_Radial_FP_SL.fldLDowel_Spec_Loc_Center = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Center;
            //    //            pProject_Bearing_Radial_FP_SL.fldLDowel_Spec_Loc_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Front;
            //    //            pProject_Bearing_Radial_FP_SL.fldRDowel_Spec_Loc_Center = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Center;
            //    //            pProject_Bearing_Radial_FP_SL.fldRDowel_Spec_Loc_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Front;
            //    //            pProject_Bearing_Radial_FP_SL.fldDowel_Depth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Depth;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_SL(pProject_Bearing_Radial_FP_SL);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_AntiRotPin_ORM(clsProject Project_In)
            //    //    //==========================================================================        //AES 01JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
                        
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_AntiRotPin where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_AntiRotPin = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_AntiRotPin where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Dist_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Dist_Front;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Casing_SL = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL.ToString();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Offset = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Offset;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Bearing_Vert = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert.ToString();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Bearing_SL = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL.ToString();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Angle = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Angle;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_UnitSystem = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Unit.System.ToString();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Type;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.L;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Mat;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldDepth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Depth;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldStickOut = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Stickout;

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_AntiRotPin pProject_Bearing_Radial_FP_AntiRotPin = new tblProject_Bearing_Radial_FP_AntiRotPin();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Dist_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Dist_Front;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Casing_SL = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL.ToString();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Offset = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Offset;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Bearing_Vert = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert.ToString();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Bearing_SL = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL.ToString();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldLoc_Angle = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Angle;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_UnitSystem = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Unit.System.ToString();
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Type;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.L;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldSpec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Mat;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldDepth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Depth;
            //    //            pProject_Bearing_Radial_FP_AntiRotPin.fldStickOut = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Stickout;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_AntiRotPin(pProject_Bearing_Radial_FP_AntiRotPin);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_Mount_ORM(clsProject Project_In)
            //    //    //======================================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pHolesGoThru = false;
            //    //        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru) pHolesGoThru = true;

            //    //        Boolean pFixtureCandidate_Chosen_Front = false;
            //    //        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0]) pFixtureCandidate_Chosen_Front = true;

            //    //        Boolean pFixtureCandidate_Chosen_Back = false;
            //    //        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0]) pFixtureCandidate_Chosen_Back = true;

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_Mount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_Mount.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_Mount.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_Mount.fldHoles_GoThru = pHolesGoThru;
            //    //            pProject_Bearing_Radial_FP_Mount.fldHoles_Bolting = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString();
            //    //            pProject_Bearing_Radial_FP_Mount.fldFixture_Candidates_Chosen_Front = pFixtureCandidate_Chosen_Front;
            //    //            pProject_Bearing_Radial_FP_Mount.fldFixture_Candidates_Chosen_Back = pFixtureCandidate_Chosen_Back;
            //    //            pProject_Bearing_Radial_FP_Mount.fldHoles_Thread_Depth_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0];
            //    //            pProject_Bearing_Radial_FP_Mount.fldHoles_Thread_Depth_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1];
                            
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_Mount pProject_Bearing_Radial_FP_Mount = new tblProject_Bearing_Radial_FP_Mount();
            //    //            pProject_Bearing_Radial_FP_Mount.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_Mount.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_Mount.fldHoles_GoThru = pHolesGoThru;
            //    //            pProject_Bearing_Radial_FP_Mount.fldHoles_Bolting = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString();
            //    //            pProject_Bearing_Radial_FP_Mount.fldFixture_Candidates_Chosen_Front = pFixtureCandidate_Chosen_Front;
            //    //            pProject_Bearing_Radial_FP_Mount.fldFixture_Candidates_Chosen_Back = pFixtureCandidate_Chosen_Back;
            //    //            pProject_Bearing_Radial_FP_Mount.fldHoles_Thread_Depth_Front = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0];
            //    //            pProject_Bearing_Radial_FP_Mount.fldHoles_Thread_Depth_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1];

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_Mount(pProject_Bearing_Radial_FP_Mount);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_Mount_Fixture_ORM(clsProject Project_In)
            //    //    //==============================================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pHolesEquispaced = false;
            //    //        Boolean pHolesAngStart_Comp_Chosen = false;

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount_Fixture where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record

            //    //            StringCollection pBolting = new StringCollection();
            //    //            pBolting = Retrieve_Bearing_Radial_FP_Mount_Fixture_Bolting_Pos_ORM(Project_In);
            //    //            switch (pBolting.Count)
            //    //            {
            //    //                case 1:
            //    //                    //----- 

            //    //                    //....Bolting = Front or Bolting = Back, when user changes Bolting = Both
            //    //                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front ||
            //    //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
            //    //                    {
            //    //                        if (pBolting[0] == ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString())
            //    //                        {
            //    //                            UpdateRec_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In);
            //    //                        }
            //    //                        else
            //    //                        {
            //    //                            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_Mount_Fixture");
            //    //                            SaveToDB_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In);
            //    //                        }
            //    //                    }
            //    //                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
            //    //                    {
            //    //                        DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_Mount_Fixture");
            //    //                        SaveToDB_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In);
            //    //                    }
            //    //                    break;

            //    //                case 2:
            //    //                    //-----

            //    //                    //....Bolting = Both, When user changes Bolting = Front or  Bolting = Back
            //    //                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front ||
            //    //                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
            //    //                    {
            //    //                        for (int i = 0; i < pBolting.Count; i++)
            //    //                        {
            //    //                            if (pBolting[i] == ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString())
            //    //                            {
            //    //                                UpdateRec_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In);
            //    //                            }
            //    //                            else
            //    //                            {
            //    //                                DeleteRecord_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In, pBolting[i]);
            //    //                            }
            //    //                        }
            //    //                    }
            //    //                    else
            //    //                    {
            //    //                        UpdateRec_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In);
            //    //                    }
            //    //                    break;
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
            //    //            {
            //    //                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = true;
            //    //                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = true;

            //    //                tblProject_Bearing_Radial_FP_Mount_Fixture pProject_Bearing_Radial_FP_Mount_Fixture = new tblProject_Bearing_Radial_FP_Mount_Fixture(); pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo = Project_In.No;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo_Suffix = pNoSuffix;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldPosition = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString();
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldPartNo = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldDBC = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldD_Finish = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesEquispaced = pHolesEquispaced;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart_Comp_Chosen = pHolesAngStart_Comp_Chosen;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther1 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther2 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther3 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther4 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther5 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther6 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther7 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat;

            //    //                pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_Mount_Fixture(pProject_Bearing_Radial_FP_Mount_Fixture);
            //    //                pBearingDBEntities.SaveChanges();
            //    //            }
            //    //            else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
            //    //            {
            //    //                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = true;
            //    //                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = true;

            //    //                tblProject_Bearing_Radial_FP_Mount_Fixture pProject_Bearing_Radial_FP_Mount_Fixture = new tblProject_Bearing_Radial_FP_Mount_Fixture(); pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo = Project_In.No;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo = Project_In.No;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo_Suffix = pNoSuffix;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldPosition = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString();
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldPartNo = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldDBC = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldD_Finish = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesEquispaced = pHolesEquispaced;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart_Comp_Chosen = pHolesAngStart_Comp_Chosen;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther1 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther2 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther3 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther4 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther5 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther6 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther7 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat;

            //    //                pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_Mount_Fixture(pProject_Bearing_Radial_FP_Mount_Fixture);
            //    //                pBearingDBEntities.SaveChanges();
            //    //            }
            //    //            else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
            //    //            {
            //    //                string[] pPosition = new string[] { "Front", "Back" };

            //    //                for (int i = 0; i < 2; i++)
            //    //                {
            //    //                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = true;
            //    //                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = true;

            //    //                    tblProject_Bearing_Radial_FP_Mount_Fixture pProject_Bearing_Radial_FP_Mount_Fixture = new tblProject_Bearing_Radial_FP_Mount_Fixture(); pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo = Project_In.No;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo = Project_In.No;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldPosition = pPosition[i];
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldPartNo = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldDBC = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldD_Finish = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesEquispaced = pHolesEquispaced;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart_Comp_Chosen = pHolesAngStart_Comp_Chosen;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther1 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0];
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther2 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1];
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther3 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2];
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther4 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3];
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther5 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4];
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther6 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5];
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther7 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6];
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L;
            //    //                    pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat;

            //    //                    pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_Mount_Fixture(pProject_Bearing_Radial_FP_Mount_Fixture);
            //    //                }
            //    //                pBearingDBEntities.SaveChanges();
            //    //            }
            //    //        }
            //    //    }


            //    //    private void UpdateRec_Bearing_Radial_FP_Mount_Fixture_ORM(clsProject Project_In)
            //    //    //===============================================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pHolesEquispaced = false;
            //    //        Boolean pHolesAngStart_Comp_Chosen = false;
            //    //        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
            //    //        {
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = true;
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = true;

            //    //            var pProject_Bearing_Radial_FP_Mount_Fixture = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount_Fixture where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldPosition = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString();
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldPartNo = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldDBC = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldD_Finish = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesEquispaced = pHolesEquispaced;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart_Comp_Chosen = pHolesAngStart_Comp_Chosen;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther1 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther2 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther3 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther4 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther5 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther6 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther7 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat;

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
            //    //        {
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = true;
            //    //            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = true;

            //    //            var pProject_Bearing_Radial_FP_Mount_Fixture = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount_Fixture where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldPosition = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString();
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldPartNo = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldDBC = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldD_Finish = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesEquispaced = pHolesEquispaced;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart_Comp_Chosen = pHolesAngStart_Comp_Chosen;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther1 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther2 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther3 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther4 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther5 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther6 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther7 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6];
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L;
            //    //            pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat;

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
            //    //        {
            //    //            string[] pPosition = new string[] { "Front", "Back" };

            //    //            for (int i = 0; i < 2; i++)
            //    //            {
            //    //                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = true;
            //    //                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = true;

            //    //                var pProject_Bearing_Radial_FP_Mount_Fixture = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount_Fixture where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo = Project_In.No;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldProjectNo_Suffix = pNoSuffix;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldPosition = pPosition[i];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldPartNo = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldDBC = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldD_Finish = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesEquispaced = pHolesEquispaced;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngStart_Comp_Chosen = pHolesAngStart_Comp_Chosen;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther1 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther2 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther3 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther4 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther5 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther6 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldHolesAngOther7 = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6];
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Type = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_D_Desig = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_L = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L;
            //    //                pProject_Bearing_Radial_FP_Mount_Fixture.fldScrew_Spec_Mat = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat;
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_TempSensor_ORM(clsProject Project_In)
            //    //    //===========================================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pExists = false;
            //    //        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists) pExists = true;

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_TempSensor where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_TempSensor = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_TempSensor where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldExists = pExists;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldCanLength = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldLoc = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc.ToString();
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldD = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldDepth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldAngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart;
                            
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_TempSensor pProject_Bearing_Radial_FP_TempSensor = new tblProject_Bearing_Radial_FP_TempSensor();
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldExists = pExists;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldCanLength = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldCount = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldLoc = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc.ToString();
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldD = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldDepth = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth;
            //    //            pProject_Bearing_Radial_FP_TempSensor.fldAngStart = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_TempSensor(pProject_Bearing_Radial_FP_TempSensor);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Radial_FP_EDM_Pad_ORM(clsProject Project_In)
            //    //    //========================================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_EDM_Pad where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Bearing_Radial_FP_EDM_Pad = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_EDM_Pad where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Bearing_Radial_FP_EDM_Pad.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_EDM_Pad.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_EDM_Pad.fldRFillet_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.RFillet_Back;
            //    //            pProject_Bearing_Radial_FP_EDM_Pad.fldAngStart_Web = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.AngStart_Web;
                            
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Bearing_Radial_FP_EDM_Pad pProject_Bearing_Radial_FP_EDM_Pad = new tblProject_Bearing_Radial_FP_EDM_Pad();
            //    //            pProject_Bearing_Radial_FP_EDM_Pad.fldProjectNo = Project_In.No;
            //    //            pProject_Bearing_Radial_FP_EDM_Pad.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Bearing_Radial_FP_EDM_Pad.fldRFillet_Back = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.RFillet_Back;
            //    //            pProject_Bearing_Radial_FP_EDM_Pad.fldAngStart_Web = (Decimal)((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.AngStart_Web;

            //    //            pBearingDBEntities.AddTotblProject_Bearing_Radial_FP_EDM_Pad(pProject_Bearing_Radial_FP_EDM_Pad);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_EndConfigs_ORM(clsProject Project_In)
            //    //    //=========================================================        //AES 31MAY18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        string[] pPosition = new string[] { "Front", "Back" };
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfigs where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                var pProject_EndConfigs = (from pRec in pBearingDBEntities.tblProject_EndConfigs where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                pProject_EndConfigs.fldProjectNo = Project_In.No;
            //    //                pProject_EndConfigs.fldProjectNo_Suffix = pNoSuffix;
            //    //                pProject_EndConfigs.fldPosition = pPosition[i];
            //    //                pProject_EndConfigs.fldType = Project_In.Product.EndConfig[i].Type.ToString();
            //    //                pProject_EndConfigs.fldMat_Base = Project_In.Product.EndConfig[i].Mat.Base;
            //    //                pProject_EndConfigs.fldMat_Lining = Project_In.Product.EndConfig[i].Mat.Lining;
            //    //                pProject_EndConfigs.fldDO = (Decimal)Project_In.Product.EndConfig[i].DO;
            //    //                pProject_EndConfigs.fldDBore_Range_Min = (Decimal)Project_In.Product.EndConfig[i].DBore_Range[0];
            //    //                pProject_EndConfigs.fldDBore_Range_Max = (Decimal)Project_In.Product.EndConfig[i].DBore_Range[1];
            //    //                pProject_EndConfigs.fldL = (Decimal)Project_In.Product.EndConfig[i].L;                               
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                tblProject_EndConfigs pProject_EndConfigs = new tblProject_EndConfigs();
            //    //                pProject_EndConfigs.fldProjectNo = Project_In.No;
            //    //                pProject_EndConfigs.fldProjectNo_Suffix = pNoSuffix;
            //    //                pProject_EndConfigs.fldPosition = pPosition[i];
            //    //                pProject_EndConfigs.fldType = Project_In.Product.EndConfig[i].Type.ToString();
            //    //                pProject_EndConfigs.fldMat_Base = Project_In.Product.EndConfig[i].Mat.Base;
            //    //                pProject_EndConfigs.fldMat_Lining = Project_In.Product.EndConfig[i].Mat.Lining;
            //    //                pProject_EndConfigs.fldDO = (Decimal)Project_In.Product.EndConfig[i].DO;
            //    //                pProject_EndConfigs.fldDBore_Range_Min = (Decimal)Project_In.Product.EndConfig[i].DBore_Range[0];
            //    //                pProject_EndConfigs.fldDBore_Range_Max = (Decimal)Project_In.Product.EndConfig[i].DBore_Range[1];
            //    //                pProject_EndConfigs.fldL = (Decimal)Project_In.Product.EndConfig[i].L;

            //    //                pBearingDBEntities.AddTotblProject_EndConfigs(pProject_EndConfigs);                               
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_EndConfigs_MountHoles_ORM(clsProject Project_In)
            //    //    //===================================================================        //AES 31MAY18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        string[] pPosition = new string[] { "Front", "Back" };
            //    //        Boolean pThread_Thru = false;
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_MountHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                var pProject_EndConfigs_MountHoles = (from pRec in pBearingDBEntities.tblProject_EndConfig_MountHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                pProject_EndConfigs_MountHoles.fldProjectNo = Project_In.No;
            //    //                pProject_EndConfigs_MountHoles.fldProjectNo_Suffix = pNoSuffix;
            //    //                pProject_EndConfigs_MountHoles.fldPosition = pPosition[i];
            //    //                pProject_EndConfigs_MountHoles.fldType = Project_In.Product.EndConfig[i].MountHoles.Type.ToString();
            //    //                pProject_EndConfigs_MountHoles.fldDepth_CBore = (Decimal)Project_In.Product.EndConfig[i].MountHoles.Depth_CBore;
            //    //                pProject_EndConfigs_MountHoles.fldThread_Thru = pThread_Thru;
            //    //                pProject_EndConfigs_MountHoles.fldDepth_Thread = (Decimal)Project_In.Product.EndConfig[i].MountHoles.Depth_Thread;                      
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                tblProject_EndConfig_MountHoles pProject_EndConfigs_MountHoles = new tblProject_EndConfig_MountHoles();
            //    //                pProject_EndConfigs_MountHoles.fldProjectNo = Project_In.No;
            //    //                pProject_EndConfigs_MountHoles.fldProjectNo_Suffix = pNoSuffix;
            //    //                pProject_EndConfigs_MountHoles.fldPosition = pPosition[i];
            //    //                pProject_EndConfigs_MountHoles.fldType = Project_In.Product.EndConfig[i].MountHoles.Type.ToString();
            //    //                pProject_EndConfigs_MountHoles.fldDepth_CBore = (Decimal)Project_In.Product.EndConfig[i].MountHoles.Depth_CBore;
            //    //                pProject_EndConfigs_MountHoles.fldThread_Thru = pThread_Thru;
            //    //                pProject_EndConfigs_MountHoles.fldDepth_Thread = (Decimal)Project_In.Product.EndConfig[i].MountHoles.Depth_Thread;

            //    //                pBearingDBEntities.AddTotblProject_EndConfig_MountHoles(pProject_EndConfigs_MountHoles);
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_EndConfig_Seal_Detail_ORM(clsProject Project_In)
            //    //    //====================================================================        //AES 04JUN18
            //    //    {
            //    //        SaveToDB_EndConfig_Seal_ORM(Project_In);
            //    //        SaveToDB_EndConfig_Seal_Blade_ORM(Project_In);
            //    //        SaveToDB_EndConfig_Seal_DrailnHoles_ORM(Project_In);
            //    //        SaveToDB_EndConfig_Seal_WireClipHoles_ORM(Project_In);
            //    //    }

            //    //    private void SaveToDB_EndConfig_Seal_ORM(clsProject Project_In)
            //    //    //=============================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    //                {
            //    //                    var pProject_EndConfig_Seal = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_Seal.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_Seal.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_Seal.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_Seal.fldType = ((clsSeal)Project_In.Product.EndConfig[i]).Design.ToString();
            //    //                    pProject_EndConfig_Seal.fldLiningT = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).Mat_LiningT;
            //    //                    pProject_EndConfig_Seal.fldTempSensor_D_ExitHole = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).TempSensor_D_ExitHole;

            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    //                {
            //    //                    tblProject_EndConfig_Seal pProject_EndConfig_Seal = new tblProject_EndConfig_Seal();
            //    //                    pProject_EndConfig_Seal.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_Seal.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_Seal.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_Seal.fldType = ((clsSeal)Project_In.Product.EndConfig[i]).Design.ToString();
            //    //                    pProject_EndConfig_Seal.fldLiningT = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).Mat_LiningT;
            //    //                    pProject_EndConfig_Seal.fldTempSensor_D_ExitHole = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).TempSensor_D_ExitHole;

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_Seal(pProject_EndConfig_Seal);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_EndConfig_Seal_Blade_ORM(clsProject Project_In)
            //    //    //===================================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_Blade where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    //                {
            //    //                    var pProject_EndConfig_Seal_Blade = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_Blade where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_Seal_Blade.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_Seal_Blade.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_Seal_Blade.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_Seal_Blade.fldCount = ((clsSeal)Project_In.Product.EndConfig[i]).Blade.Count;
            //    //                    pProject_EndConfig_Seal_Blade.fldT = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).Blade.T;
            //    //                    pProject_EndConfig_Seal_Blade.fldAngTaper = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).Blade.AngTaper;

            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    //                {
            //    //                    tblProject_EndConfig_Seal_Blade pProject_EndConfig_Seal_Blade = new tblProject_EndConfig_Seal_Blade();
            //    //                    pProject_EndConfig_Seal_Blade.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_Seal_Blade.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_Seal_Blade.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_Seal_Blade.fldCount = ((clsSeal)Project_In.Product.EndConfig[i]).Blade.Count;
            //    //                    pProject_EndConfig_Seal_Blade.fldT = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).Blade.T;
            //    //                    pProject_EndConfig_Seal_Blade.fldAngTaper = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).Blade.AngTaper;

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_Seal_Blade(pProject_EndConfig_Seal_Blade);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_EndConfig_Seal_DrailnHoles_ORM(clsProject Project_In)
            //    //    //=========================================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_DrainHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    //                {
            //    //                    var pProject_EndConfig_Seal_DrainHoles = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_DrainHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAnnulus_Ratio_L_H = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.Ratio_L_H;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAnnulus_D = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.D;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldD_Desig = ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.D_Desig;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldCount = ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Count;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAngStart = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngStart;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAngBet = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngBet;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAngExit = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngExit;

            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    //                {
            //    //                    tblProject_EndConfig_Seal_DrainHoles pProject_EndConfig_Seal_DrainHoles = new tblProject_EndConfig_Seal_DrainHoles();
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAnnulus_Ratio_L_H = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.Ratio_L_H;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAnnulus_D = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.D;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldD_Desig = ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.D_Desig;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldCount = ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Count;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAngStart = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngStart;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAngBet = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngBet;
            //    //                    pProject_EndConfig_Seal_DrainHoles.fldAngExit = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngExit;

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_Seal_DrainHoles(pProject_EndConfig_Seal_DrainHoles);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_EndConfig_Seal_WireClipHoles_ORM(clsProject Project_In)
            //    //    //===========================================================================        //AES 04JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_WireClipHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                Boolean pExists = false;
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    //                {
            //    //                    if (((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Exists) pExists = true;

            //    //                    var pProject_EndConfig_Seal_WireClipHoles = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_WireClipHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldExists = pExists;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldCount = ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Count;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldDBC = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.DBC;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldUnitSystem = ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Unit.System.ToString();
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldThread_Dia_Desig = ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.D_Desig;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldThread_Pitch = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.Pitch;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldThread_Depth = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.ThreadDepth;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldAngStart = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngStart;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldAngOther1 = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[0];
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldAngOther2 = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[1];

            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                Boolean pExists = false;
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    //                {
            //    //                    if (((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Exists) pExists = true;

            //    //                    tblProject_EndConfig_Seal_WireClipHoles pProject_EndConfig_Seal_WireClipHoles = new tblProject_EndConfig_Seal_WireClipHoles();
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldExists = pExists;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldCount = ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Count;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldDBC = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.DBC;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldUnitSystem = ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Unit.System.ToString();
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldThread_Dia_Desig = ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.D_Desig;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldThread_Pitch = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.Pitch;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldThread_Depth = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.ThreadDepth;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldAngStart = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngStart;
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldAngOther1 = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[0];
            //    //                    pProject_EndConfig_Seal_WireClipHoles.fldAngOther2 = (Decimal)((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[1];

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_Seal_WireClipHoles(pProject_EndConfig_Seal_WireClipHoles);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_EndConfig_TB_Detail_ORM(clsProject Project_In)
            //    //    //==================================================================        //AES 05JUN18
            //    //    {
            //    //        SaveToDB_Bearing_Thrust_TL_ORM(Project_In);
            //    //        SaveToDB_Bearing_Thrust_TL_PerformData_ORM(Project_In);
            //    //        SaveToDB_Bearing_Thrust_TL_FeedGroove_ORM(Project_In);
            //    //        SaveToDB_Bearing_Thrust_TL_WeepSlot_ORM(Project_In);
            //    //        SaveToDB_Bearing_Thrust_TL_GCodes_ORM(Project_In);
            //    //    }

            //    //    private void SaveToDB_Bearing_Thrust_TL_ORM(clsProject Project_In)
            //    //    //================================================================        //AES 05JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                Boolean pReqd = false;
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    if (((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Reqd) pReqd = true;

            //    //                    var pProject_EndConfig_TB = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_TB.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB.fldDirectionType = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DirectionType.ToString();
            //    //                    pProject_EndConfig_TB.fldPad_ID = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[0];
            //    //                    pProject_EndConfig_TB.fldPad_OD = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[1];
            //    //                    pProject_EndConfig_TB.fldLandL = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LandL;
            //    //                    pProject_EndConfig_TB.fldLiningT_Face = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.Face;
            //    //                    pProject_EndConfig_TB.fldLiningT_ID = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.ID;
            //    //                    pProject_EndConfig_TB.fldPad_Count = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Pad_Count;
            //    //                    pProject_EndConfig_TB.fldTaper_Depth_OD = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_OD;
            //    //                    pProject_EndConfig_TB.fldTaper_Depth_ID = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_ID;
            //    //                    pProject_EndConfig_TB.fldTaper_Angle = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Angle;
            //    //                    pProject_EndConfig_TB.fldShroud_Ro = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ro;
            //    //                    pProject_EndConfig_TB.fldShroud_Ri = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ri;
            //    //                    pProject_EndConfig_TB.fldBackRelief_Reqd = pReqd;
            //    //                    pProject_EndConfig_TB.fldBackRelief_D = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.D;
            //    //                    pProject_EndConfig_TB.fldBackRelief_Depth = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Depth;
            //    //                    pProject_EndConfig_TB.fldBackRelief_Fillet = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Fillet;
            //    //                    pProject_EndConfig_TB.fldDimStart = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DimStart();
            //    //                    pProject_EndConfig_TB.fldLFlange = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LFlange;
            //    //                    pProject_EndConfig_TB.fldFaceOff_Assy = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FaceOff_Assy;

            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                Boolean pReqd = false;
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    if (((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Reqd) pReqd = true;

            //    //                    tblProject_EndConfig_TB pProject_EndConfig_TB = new tblProject_EndConfig_TB();
            //    //                    pProject_EndConfig_TB.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB.fldDirectionType = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DirectionType.ToString();
            //    //                    pProject_EndConfig_TB.fldPad_ID = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[0];
            //    //                    pProject_EndConfig_TB.fldPad_OD = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[1];
            //    //                    pProject_EndConfig_TB.fldLandL = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LandL;
            //    //                    pProject_EndConfig_TB.fldLiningT_Face = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.Face;
            //    //                    pProject_EndConfig_TB.fldLiningT_ID = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.ID;
            //    //                    pProject_EndConfig_TB.fldPad_Count = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Pad_Count;
            //    //                    pProject_EndConfig_TB.fldTaper_Depth_OD = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_OD;
            //    //                    pProject_EndConfig_TB.fldTaper_Depth_ID = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_ID;
            //    //                    pProject_EndConfig_TB.fldTaper_Angle = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Angle;
            //    //                    pProject_EndConfig_TB.fldShroud_Ro = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ro;
            //    //                    pProject_EndConfig_TB.fldShroud_Ri = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ri;
            //    //                    pProject_EndConfig_TB.fldBackRelief_Reqd = pReqd;
            //    //                    pProject_EndConfig_TB.fldBackRelief_D = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.D;
            //    //                    pProject_EndConfig_TB.fldBackRelief_Depth = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Depth;
            //    //                    pProject_EndConfig_TB.fldBackRelief_Fillet = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Fillet;
            //    //                    pProject_EndConfig_TB.fldDimStart = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DimStart();
            //    //                    pProject_EndConfig_TB.fldLFlange = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LFlange;
            //    //                    pProject_EndConfig_TB.fldFaceOff_Assy = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FaceOff_Assy;

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_TB(pProject_EndConfig_TB);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Thrust_TL_PerformData_ORM(clsProject Project_In)
            //    //    //============================================================================        //AES 05JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_PerformData where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    var pProject_EndConfig_TB_PerformData = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_PerformData where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_TB_PerformData.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB_PerformData.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB_PerformData.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB_PerformData.fldPower_HP = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.Power_HP;
            //    //                    pProject_EndConfig_TB_PerformData.fldFlowReqd_gpm = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.FlowReqd_gpm;
            //    //                    pProject_EndConfig_TB_PerformData.fldTempRise_F = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TempRise_F;
            //    //                    pProject_EndConfig_TB_PerformData.fldTFilm_Min = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TFilm_Min;
            //    //                    pProject_EndConfig_TB_PerformData.fldPadMax_Temp = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Temp;
            //    //                    pProject_EndConfig_TB_PerformData.fldPadMax_Press = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Press;
            //    //                    pProject_EndConfig_TB_PerformData.fldUnitLoad = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.UnitLoad;

            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    tblProject_EndConfig_TB_PerformData pProject_EndConfig_TB_PerformData = new tblProject_EndConfig_TB_PerformData();
            //    //                    pProject_EndConfig_TB_PerformData.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB_PerformData.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB_PerformData.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB_PerformData.fldPower_HP = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.Power_HP;
            //    //                    pProject_EndConfig_TB_PerformData.fldFlowReqd_gpm = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.FlowReqd_gpm;
            //    //                    pProject_EndConfig_TB_PerformData.fldTempRise_F = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TempRise_F;
            //    //                    pProject_EndConfig_TB_PerformData.fldTFilm_Min = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TFilm_Min;
            //    //                    pProject_EndConfig_TB_PerformData.fldPadMax_Temp = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Temp;
            //    //                    pProject_EndConfig_TB_PerformData.fldPadMax_Press = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Press;
            //    //                    pProject_EndConfig_TB_PerformData.fldUnitLoad = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.UnitLoad;

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_TB_PerformData(pProject_EndConfig_TB_PerformData);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Thrust_TL_FeedGroove_ORM(clsProject Project_In)
            //    //    //===========================================================================        //AES 05JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_FeedGroove where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    var pProject_EndConfig_TB_PerformData = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_FeedGroove where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_TB_PerformData.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB_PerformData.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB_PerformData.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB_PerformData.fldType = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Type;
            //    //                    pProject_EndConfig_TB_PerformData.fldWid = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Wid;
            //    //                    pProject_EndConfig_TB_PerformData.fldDepth = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Depth;
            //    //                    pProject_EndConfig_TB_PerformData.fldDBC = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.DBC;
            //    //                    pProject_EndConfig_TB_PerformData.fldDist_Chamf = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Dist_Chamf;
                                    
            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    tblProject_EndConfig_TB_FeedGroove pProject_EndConfig_TB_PerformData = new tblProject_EndConfig_TB_FeedGroove();
            //    //                    pProject_EndConfig_TB_PerformData.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB_PerformData.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB_PerformData.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB_PerformData.fldType = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Type;
            //    //                    pProject_EndConfig_TB_PerformData.fldWid = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Wid;
            //    //                    pProject_EndConfig_TB_PerformData.fldDepth = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Depth;
            //    //                    pProject_EndConfig_TB_PerformData.fldDBC = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.DBC;
            //    //                    pProject_EndConfig_TB_PerformData.fldDist_Chamf = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Dist_Chamf;

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_TB_FeedGroove(pProject_EndConfig_TB_PerformData);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Thrust_TL_WeepSlot_ORM(clsProject Project_In)
            //    //    //=========================================================================        //AES 05JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_WeepSlot where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    var pProject_EndConfig_TB_WeepSlot = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_WeepSlot where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_TB_WeepSlot.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB_WeepSlot.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB_WeepSlot.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB_WeepSlot.fldType = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Type.ToString();
            //    //                    pProject_EndConfig_TB_WeepSlot.fldWid = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Wid;
            //    //                    pProject_EndConfig_TB_WeepSlot.fldDepth = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Depth;

            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    tblProject_EndConfig_TB_WeepSlot pProject_EndConfig_TB_WeepSlot = new tblProject_EndConfig_TB_WeepSlot();
            //    //                    pProject_EndConfig_TB_WeepSlot.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB_WeepSlot.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB_WeepSlot.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB_WeepSlot.fldType = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Type.ToString();
            //    //                    pProject_EndConfig_TB_WeepSlot.fldWid = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Wid;
            //    //                    pProject_EndConfig_TB_WeepSlot.fldDepth = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Depth;

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_TB_WeepSlot(pProject_EndConfig_TB_WeepSlot);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Bearing_Thrust_TL_GCodes_ORM(clsProject Project_In)
            //    //    //=======================================================================        //AES 05JUN18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        string[] pPosition = new string[] { "Front", "Back" };

            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_GCodes where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    var pProject_EndConfig_TB_GCodes = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_GCodes where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //                    pProject_EndConfig_TB_GCodes.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB_GCodes.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB_GCodes.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB_GCodes.fldD_Desig_T1 = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T1.D_Desig;
            //    //                    pProject_EndConfig_TB_GCodes.fldD_Desig_T2 = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T2.D_Desig;
            //    //                    pProject_EndConfig_TB_GCodes.fldD_Desig_T3 = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T3.D_Desig;
            //    //                    pProject_EndConfig_TB_GCodes.fldD_Desig_T4 = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T4.D_Desig;
            //    //                    pProject_EndConfig_TB_GCodes.fldOverlap_frac = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Overlap_frac;
            //    //                    pProject_EndConfig_TB_GCodes.fldFeed_Taperland = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.Taperland;
            //    //                    pProject_EndConfig_TB_GCodes.fldFeed_WeepSlot = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.WeepSlot;
            //    //                    pProject_EndConfig_TB_GCodes.fldDepth_Backlash = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Backlash;
            //    //                    pProject_EndConfig_TB_GCodes.fldDepth_Dwell_T = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Dwell_T;
            //    //                    pProject_EndConfig_TB_GCodes.fldDepth_WeepSlot_Cut = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_WS_Cut_Per_Pass;
            //    //                    pProject_EndConfig_TB_GCodes.fldFilePath_Dir = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FilePath_Dir;

            //    //                    pBearingDBEntities.SaveChanges();
            //    //                }
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            for (int i = 0; i < pPosition.Length; i++)
            //    //            {
            //    //                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    //                {
            //    //                    tblProject_EndConfig_TB_GCodes pProject_EndConfig_TB_GCodes = new tblProject_EndConfig_TB_GCodes();
            //    //                    pProject_EndConfig_TB_GCodes.fldProjectNo = Project_In.No;
            //    //                    pProject_EndConfig_TB_GCodes.fldProjectNo_Suffix = pNoSuffix;
            //    //                    pProject_EndConfig_TB_GCodes.fldPosition = pPosition[i];
            //    //                    pProject_EndConfig_TB_GCodes.fldD_Desig_T1 = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T1.D_Desig;
            //    //                    pProject_EndConfig_TB_GCodes.fldD_Desig_T2 = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T2.D_Desig;
            //    //                    pProject_EndConfig_TB_GCodes.fldD_Desig_T3 = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T3.D_Desig;
            //    //                    pProject_EndConfig_TB_GCodes.fldD_Desig_T4 = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T4.D_Desig;
            //    //                    pProject_EndConfig_TB_GCodes.fldOverlap_frac = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Overlap_frac;
            //    //                    pProject_EndConfig_TB_GCodes.fldFeed_Taperland = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.Taperland;
            //    //                    pProject_EndConfig_TB_GCodes.fldFeed_WeepSlot = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.WeepSlot;
            //    //                    pProject_EndConfig_TB_GCodes.fldDepth_Backlash = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Backlash;
            //    //                    pProject_EndConfig_TB_GCodes.fldDepth_Dwell_T = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Dwell_T;
            //    //                    pProject_EndConfig_TB_GCodes.fldDepth_WeepSlot_Cut = (Decimal)((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_WS_Cut_Per_Pass;
            //    //                    pProject_EndConfig_TB_GCodes.fldFilePath_Dir = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FilePath_Dir;

            //    //                    pBearingDBEntities.AddTotblProject_EndConfig_TB_GCodes(pProject_EndConfig_TB_GCodes);
            //    //                }
            //    //            }
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

            //    //    private void SaveToDB_Project_Product_Accessories_ORM(clsProject Project_In)
            //    //    //==========================================================================        //AES 31MAY18
            //    //    {
            //    //        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //    //        string pNoSuffix = "";
            //    //        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    //        {
            //    //            pNoSuffix = Project_In.No_Suffix;
            //    //        }
            //    //        else
            //    //        {
            //    //            pNoSuffix = "NULL";
            //    //        }
            //    //        Boolean pTempSensor_Supplied = false;
            //    //        if (Project_In.Product.Accessories.TempSensor.Supplied) pTempSensor_Supplied = true;

            //    //        Boolean pWireClip_Supplied = false;
            //    //        if (Project_In.Product.Accessories.WireClip.Supplied) pWireClip_Supplied = true;
            //    //        int pProjectCount = (from pRec in pBearingDBEntities.tblProject_Product_Accessories where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).Count();

            //    //        if (pProjectCount > 0)
            //    //        {
            //    //            //....Record already exists Update record
            //    //            var pProject_Product_Accessories = (from pRec in pBearingDBEntities.tblProject_Product_Accessories where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).First();
            //    //            pProject_Product_Accessories.fldProjectNo = Project_In.No;
            //    //            pProject_Product_Accessories.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Product_Accessories.fldTempSensor_Supplied = pTempSensor_Supplied;
            //    //            pProject_Product_Accessories.fldTempSensor_Name = Project_In.Product.Accessories.TempSensor.Name.ToString();
            //    //            pProject_Product_Accessories.fldTempSensor_Count = Project_In.Product.Accessories.TempSensor.Count;
            //    //            pProject_Product_Accessories.fldTempSensor_Type = Project_In.Product.Accessories.TempSensor.Type.ToString();
            //    //            pProject_Product_Accessories.fldWireClip_Supplied = pWireClip_Supplied;
            //    //            pProject_Product_Accessories.fldWireClip_Count = Project_In.Product.Accessories.WireClip.Count;
            //    //            pProject_Product_Accessories.fldWireClip_Size = Project_In.Product.Accessories.WireClip.Size.ToString();

            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //        else
            //    //        {
            //    //            //....New Record
            //    //            tblProject_Product_Accessories pProject_Product_Accessories = new tblProject_Product_Accessories();
            //    //            pProject_Product_Accessories.fldProjectNo = Project_In.No;
            //    //            pProject_Product_Accessories.fldProjectNo_Suffix = pNoSuffix;
            //    //            pProject_Product_Accessories.fldTempSensor_Supplied = pTempSensor_Supplied;
            //    //            pProject_Product_Accessories.fldTempSensor_Name = Project_In.Product.Accessories.TempSensor.Name.ToString();
            //    //            pProject_Product_Accessories.fldTempSensor_Count = Project_In.Product.Accessories.TempSensor.Count;
            //    //            pProject_Product_Accessories.fldTempSensor_Type = Project_In.Product.Accessories.TempSensor.Type.ToString();
            //    //            pProject_Product_Accessories.fldWireClip_Supplied = pWireClip_Supplied;
            //    //            pProject_Product_Accessories.fldWireClip_Count = Project_In.Product.Accessories.WireClip.Count;
            //    //            pProject_Product_Accessories.fldWireClip_Size = Project_In.Product.Accessories.WireClip.Size.ToString();

            //    //            pBearingDBEntities.AddTotblProject_Product_Accessories(pProject_Product_Accessories);
            //    //            pBearingDBEntities.SaveChanges();
            //    //        }
            //    //    }

         
            //    //#endregion


            //    ////#region "Database Update Routine:"

            //    ////    public void UpdateRecord(clsProject Project_In, clsOpCond OpCond_In)
            //    ////    //==================================================================
            //    ////    {
            //    ////        //UpdateRec_Project(Project_In);
            //    ////        SaveToDB_Project_ORM(Project_In);

            //    ////        if (!Does_EndConfigs_Match_ObjAndDB(Project_In))      
            //    ////        {
            //    ////            //DeleteRecords_Table(Project_In, "tblProject_Product");
            //    ////            DeleteRecords_Table_ORM(Project_In, "tblProject_Product");
            //    ////            //AddRec_Product(Project_In);
            //    ////            SaveToDB_Product_ORM(Project_In);

            //    ////            //DeleteRecords_Table(Project_In, "tblProject_EndConfigs");
            //    ////            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfigs");
            //    ////            //AddRec_EndConfigs(Project_In);
            //    ////            SaveToDB_EndConfigs_ORM(Project_In);

            //    ////            //DeleteRecords_Table(Project_In, "tblProject_EndConfig_MountHoles");
            //    ////            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_MountHoles");
            //    ////            //AddRec_EndConfig_MountHoles(Project_In);
            //    ////            SaveToDB_EndConfigs_MountHoles_ORM(Project_In);

            //    ////            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
            //    ////                Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            //    ////            {
            //    ////                DeleteRecords_EndConfig_Seal_Detail(Project_In);
            //    ////                DeleteRecords_EndConfig_TB_Detail(Project_In);
            //    ////                //AddRec_EndConfig_Seal_Detail(Project_In);
            //    ////                SaveToDB_EndConfig_Seal_Detail_ORM(Project_In);
            //    ////            }

            //    ////            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.TL_TB ||
            //    ////                Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.TL_TB)
            //    ////            {
            //    ////                DeleteRecords_EndConfig_TB_Detail(Project_In);
            //    ////                DeleteRecords_EndConfig_Seal_Detail(Project_In);
            //    ////                //AddRec_EndConfig_TB_Detail(Project_In);
            //    ////                SaveToDB_EndConfig_TB_Detail_ORM(Project_In);
            //    ////            }
            //    ////        }

            //    ////        else
            //    ////        {
            //    ////            UpdateRec_Product(Project_In);
            //    ////            UpdateRec_EndConfigs(Project_In);
            //    ////            UpdateRec_EndConfig_MountHoles(Project_In);

            //    ////            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
            //    ////                Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            //    ////            {
            //    ////                Update_EndConfig_Seal_Detail(Project_In);
            //    ////            }

            //    ////            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.TL_TB ||
            //    ////                Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.TL_TB)
            //    ////            {
            //    ////                Update_EndConfig_TB_Detail(Project_In);
            //    ////            }
            //    ////        }

            //    ////        UpdateRec_OpCond(Project_In, OpCond_In);
            //    ////        UpdateRec_Bearing_Radial(Project_In);
            //    ////        UpdateRec_Bearing_Radial_FP_Detail(Project_In);
            //    ////        Update_Project_Product_Accessories(Project_In);
            //    ////    }


            //    ////    private Boolean Does_EndConfigs_Match_ObjAndDB (clsProject Project_In)
            //    ////    //====================================================================            
            //    ////    {
            //    ////        Boolean pbln = false;
                                               
            //    ////        clsEndConfig.eType[] pType_DB = new clsEndConfig.eType[2];
            //    ////        //Retrieve_EndConfigs_Type(Project_In, pType_DB);
            //    ////        Retrieve_EndConfigs_Type_ORM(Project_In, pType_DB);

            //    ////        if (Project_In.Product.EndConfig[0].Type.ToString() == pType_DB[0].ToString() &&
            //    ////            Project_In.Product.EndConfig[1].Type.ToString() == pType_DB[1].ToString())
            //    ////        {
            //    ////            pbln = true;

            //    ////        }
            //    ////        return pbln;
            //    ////    }
                   

            //    ////    #region "Update: Project"

            //    ////        private void UpdateRec_Project(clsProject Project_In)
            //    ////        //=====================================================
            //    ////        {
            //    ////            string pEnggDate, pCheckedby_Date, pDesignedby_Date;

            //    ////            if (Project_In.Engg.Date.ToString("d", mInvCulture) != mstrDefDate)
            //    ////                pEnggDate = "'" + Project_In.Engg.Date.ToString("d", mInvCulture) + "'";
            //    ////            else
            //    ////                pEnggDate = "NULL";

            //    ////            if (Project_In.CheckedBy.Date.ToString("d", mInvCulture) != mstrDefDate)
            //    ////                pCheckedby_Date = "'" + Project_In.CheckedBy.Date.ToString("d", mInvCulture) + "'";
            //    ////            else
            //    ////                pCheckedby_Date = "NULL";

            //    ////            if (Project_In.DesignedBy.Date.ToString("d", mInvCulture) != mstrDefDate)
            //    ////                pDesignedby_Date = "'" + Project_In.DesignedBy.Date.ToString("d", mInvCulture) + "'";
            //    ////            else
            //    ////                pDesignedby_Date = "NULL";


            //    ////            int pFileModified_RadialPart = 0, pFileModified_RadialBlankAssy = 0, pFileModified_EndTB_Part = 0;
            //    ////            int pFileModified_EndTB_Assy = 0, pFileModified_EndSeal_Part = 0, pFileModified_CompleteAssy = 0; 

            //    ////            if (Project_In.FileModified_RadialPart)
            //    ////                pFileModified_RadialPart = 1;
            //    ////            else
            //    ////                pFileModified_RadialPart = 0;

            //    ////            if (Project_In.FileModified_RadialBlankAssy)
            //    ////                pFileModified_RadialBlankAssy = 1;
            //    ////            else
            //    ////                pFileModified_RadialBlankAssy = 0;
                                                 
            //    ////            if (Project_In.FileModified_EndTB_Part)
            //    ////                pFileModified_EndTB_Part = 1;
            //    ////            else
            //    ////                pFileModified_EndTB_Part = 0;
                          
            //    ////            if (Project_In.FileModified_EndTB_Assy)
            //    ////                pFileModified_EndTB_Assy = 1;
            //    ////            else
            //    ////                pFileModified_EndTB_Assy = 0;
                           
            //    ////            if (Project_In.FileModified_EndSeal_Part)
            //    ////                pFileModified_EndSeal_Part = 1;
            //    ////            else
            //    ////                pFileModified_EndSeal_Part = 0;

            //    ////            if (Project_In.FileModified_CompleteAssy)
            //    ////                pFileModified_CompleteAssy = 1;
            //    ////            else
            //    ////                pFileModified_CompleteAssy = 0;

            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;
 
            //    ////            pstrSET = " SET fldStatus = '" + Project_In.Status +
            //    ////                        "', fldCustomer_Name = '" + Project_In.Customer.Name +
            //    ////                        "', fldCustomer_MachineDesc = '" + Project_In.Customer.MachineDesc +
            //    ////                        "', fldCustomer_PartNo = '" + Project_In.Customer.PartNo +
            //    ////                        "', fldUnitSystem = '" + Project_In.Unit.System.ToString() +
            //    ////                        "', fldAssyDwg_No = '" + Project_In.AssyDwg.No +
            //    ////                        "', fldAssyDwg_No_Suffix = '" + Project_In.AssyDwg.No_Suffix +
            //    ////                        "', fldAssyDwg_Ref = '" + Project_In.AssyDwg.Ref +
            //    ////                        "', fldEngg_Name = '" + Project_In.Engg.Name +
            //    ////                        "', fldEngg_Initials = '" + Project_In.Engg.Initials +
            //    ////                        "', fldEngg_Date = " + pEnggDate +
            //    ////                        ",  fldDesignedBy_Name = '" + Project_In.DesignedBy.Name +
            //    ////                        "', fldDesignedBy_Initials = '" + Project_In.DesignedBy.Initials +
            //    ////                        "', fldDesignedBy_Date = " + pDesignedby_Date +
            //    ////                        ",  fldCheckedby_Name = '" + Project_In.CheckedBy.Name +
            //    ////                        "', fldCheckedby_Initials = '" + Project_In.CheckedBy.Initials +
            //    ////                        "', fldCheckedby_Date = " + pCheckedby_Date + 
            //    ////                        ", fldFilePath_Project = '" + modMain.gProject.FilePath_Project +
            //    ////                        "', fldFilePath_DesignTbls_SWFiles = '" + modMain.gProject.FilePath_DesignTbls_SWFiles +
            //    ////                        "', fldFileModified_CompleteAssy = " + pFileModified_CompleteAssy +             //BG 05DEC12
            //    ////                        ", fldFileModified_Radial_Part = " + pFileModified_RadialPart +
            //    ////                        ", fldFileModified_Radial_BlankAssy = " + pFileModified_RadialBlankAssy +
            //    ////                        ", fldFileModified_EndTB_Part = " + pFileModified_EndTB_Part +
            //    ////                        ", fldFileModified_EndTB_Assy = " + pFileModified_EndTB_Assy +
            //    ////                        ", fldFileModified_EndSeal_Part = " + pFileModified_EndSeal_Part +
            //    ////                        //", fldFileModified_CompleteAssy = " + pFileModified_CompleteAssy +        //BG 05DEC12
            //    ////                        ", fldFileModification_Notes = '" + modMain.gProject.FileModification_Notes + "'";

            //    ////            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                pstrWHERE = " WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////            else
            //    ////                pstrWHERE = " WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix is NULL";

            //    ////            pstrActionSQL = "UPDATE tblProject_Details" + pstrSET + pstrWHERE;
            //    ////            pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: Product"

            //    ////        private void UpdateRec_Product(clsProject Project_In)
            //    ////        //==================================================
            //    ////        {                           
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;
                   
            //    ////            pstrSET = " SET fldBearing_Type = '" + Project_In.Product.Type.ToString() +
            //    ////                //"', fldEndConfig_Front_Type = '" + Project_In.Product.EndConfig[0].Type.ToString() +      //SG 23JAN13
            //    ////                        //"', fldEndConfig_Back_Type = '" + Project_In.Product.EndConfig[1].Type.ToString() +
            //    ////                        "', fldL_Available = " + Project_In.Product.L_Available +
            //    ////                        ", fldDist_ThrustFace_Front = " + Project_In.Product.Dist_ThrustFace[0] +
            //    ////                        ", fldDist_ThrustFace_Back = " + Project_In.Product.Dist_ThrustFace[1];

            //    ////            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////            else
            //    ////                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////            pstrActionSQL = "UPDATE tblProject_Product" + pstrSET + pstrWHERE;
            //    ////            pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: Operating Condition"

            //    ////        private void UpdateRec_OpCond(clsProject Project_In, clsOpCond OpCond_In)    
            //    ////        //=======================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET, pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            pstrSET = " SET fldSpeed_Range_Min = " + OpCond_In.Speed_Range[0] +
            //    ////                     ", fldSpeed_Range_Max = " + OpCond_In.Speed_Range[1] +
            //    ////                     ", fldRadial_Load_Range_Min = " + OpCond_In.Radial_Load_Range[0] +
            //    ////                     ", fldRadial_Load_Range_Max = " + OpCond_In.Radial_Load_Range[1] +
            //    ////                     ", fldRadial_LoadAng = " + OpCond_In.Radial_LoadAng +
            //    ////                     ", fldThrust_Load_Range_Front_Min = " + OpCond_In.Thrust_Load_Range_Front[0] +
            //    ////                     ", fldThrust_Load_Range_Front_Max = " + OpCond_In.Thrust_Load_Range_Front[1] +
            //    ////                     ", fldThrust_Load_Range_Back_Min = " + OpCond_In.Thrust_Load_Range_Back[0] +
            //    ////                     ", fldThrust_Load_Range_Back_Max = " + OpCond_In.Thrust_Load_Range_Back[1] +
            //    ////                     ", fldOilSupply_Lube_Type = '" + OpCond_In.OilSupply.Lube.Type +
            //    ////                     "', fldOilSupply_Type = '" + OpCond_In.OilSupply.Type +
            //    ////                     "', fldOilSupply_Press = " + OpCond_In.OilSupply.Press +
            //    ////                     ", fldOilSupply_Temp = " + OpCond_In.OilSupply.Temp +
            //    ////                     ", fldRot_Directionality = '" + OpCond_In.Rot_Directionality.ToString() + "'";


            //    ////            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////            else
            //    ////                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////            pstrActionSQL = "UPDATE tblProject_OpCond" + pstrSET + pstrWHERE;
            //    ////            pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: Bearing Radial"

            //    ////        private void UpdateRec_Bearing_Radial(clsProject Project_In)
            //    ////        //==========================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            pstrSET = " SET fldDesign = '" + ((clsBearing_Radial)Project_In.Product.Bearing).Design.ToString() + "'";

            //    ////            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////            else
            //    ////                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////            pstrActionSQL = "UPDATE tblProject_Bearing_Radial" + pstrSET + pstrWHERE;
            //    ////            pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: Bearing Radial FP Detail"

            //    ////        private void UpdateRec_Bearing_Radial_FP_Detail(clsProject Project_In)
            //    ////        //=====================================================================
            //    ////        {
            //    ////            UpdateRec_Bearing_Radial_FP(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_PerformData(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_Pad(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_FlexurePivot(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_OilInlet(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_MillRelief(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_Flange(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_SL(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_AntiRotPin(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_Mount(Project_In);
            //    ////            //UpdateRec_Bearing_Radial_FP_Mount_Fixture(Project_In);       
            //    ////            CheckAndUpdate_Bearing_Radial_FP_Mount_Fixture(Project_In);    
            //    ////            UpdateRec_Bearing_Radial_FP_TempSensor(Project_In);
            //    ////            UpdateRec_Bearing_Radial_FP_EDM_Pad(Project_In);      
            //    ////        } 

            //    ////        #region "Update: Bearing Radial FP"

            //    ////            public void UpdateRec_Bearing_Radial_FP(clsProject Project_In)
            //    ////            //===============================================================        
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords, pSplitConfig = 0;

            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
            //    ////                    pSplitConfig = 1;
            //    ////                else
            //    ////                    pSplitConfig = 0;

            //    ////                int pMatLiningExists = 0;
            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.LiningExists) pMatLiningExists = 1;                                                             

            //    ////                pstrSET = " SET fldSplitConfig = " + pSplitConfig +
            //    ////                            ", fldDShaft_Range_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0] +
            //    ////                            ", fldDShaft_Range_Max = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1] +
            //    ////                            ", fldDFit_Range_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0] +
            //    ////                            ", fldDFit_Range_Max = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1] +
            //    ////                            ", fldDSet_Range_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0] +
            //    ////                            ", fldDSet_Range_Max = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1] +
            //    ////                            ", fldDPad_Range_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0] +
            //    ////                            ", fldDPad_Range_Max = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1] +
            //    ////                            ", fldL = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).L +
            //    ////                            //", fldEDMRelief_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[0] +
            //    ////                            //", fldEDMRelief_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[1] +
            //    ////                            ", fldDepth_EndConfig_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[0] +
            //    ////                            ", fldDepth_EndConfig_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[1] +
            //    ////                            ", fldDimStart_FrontFace = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DimStart_FrontFace +
            //    ////                            ", fldMat_Base = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base +
            //    ////                            "',fldMat_LiningExists = " + pMatLiningExists +
            //    ////                            ", fldMat_Lining = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining +
            //    ////                            "',fldLiningT = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT;

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Perform Data"

            //    ////            public void UpdateRec_Bearing_Radial_FP_PerformData(clsProject Project_In)
            //    ////            //=========================================================================       
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                pstrSET = " SET fldPower_HP = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP +
            //    ////                          ", fldFlowReqd_gpm = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm +
            //    ////                          ", fldTempRise_F = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F +
            //    ////                          ", fldTFilm_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TFilm_Min +
            //    ////                          ", fldPadMax_Temp = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Temp +
            //    ////                          ", fldPadMax_Press = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Press +
            //    ////                          ", fldPadMax_Rot = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Rot +
            //    ////                          ", fldPadMax_Load = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Load +
            //    ////                          ", fldPadMax_Stress = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Stress;

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_PerformData" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Pad"

            //    ////            public void UpdateRec_Bearing_Radial_FP_Pad(clsProject Project_In)
            //    ////            //================================================================       
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                pstrSET = " SET fldType = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type.ToString() +
            //    ////                          "', fldCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count +
            //    ////                          ", fldL = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L +
            //    ////                          ", fldPivot_Offset = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset +
            //    ////                          ", fldPivot_AngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.AngStart +
            //    ////                          ", fldT_Lead = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead +
            //    ////                          ", fldT_Pivot = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot +
            //    ////                          ", fldT_Trail = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail;


            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Pad" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Flexure Pivot"

            //    ////            public void UpdateRec_Bearing_Radial_FP_FlexurePivot(clsProject Project_In)
            //    ////            //=========================================================================       
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                pstrSET = " SET fldWeb_T = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T +
            //    ////                          ", fldWeb_RFillet = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet +
            //    ////                          ", fldWeb_H = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H +
            //    ////                          ", fldGapEDM = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM +
            //    ////                          ", fldRot_Stiff = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Rot_Stiff;

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_FlexurePivot" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Oil Inlet"

            //    ////            public void UpdateRec_Bearing_Radial_FP_OilInlet(clsProject Project_In)
            //    ////            //=====================================================================       
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords, pExists = 0;

            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Exists)
            //    ////                    pExists = 1;
            //    ////                else
            //    ////                    pExists = 0;

            //    ////                pstrSET = " SET fldOrifice_Count = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count +
            //    ////                          ", fldOrifice_D = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D +
            //    ////                          ", fldCount_MainOilSupply = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply +
            //    ////                          ", fldOrifice_StartPos = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos +
            //    ////                          "', fldOrifice_DDrill_CBore = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.DDrill_CBore +
            //    ////                          ", fldOrifice_Loc_FrontFace = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Loc_FrontFace +
            //    ////                          ", fldOrifice_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.L +
            //    ////                          ", fldOrifice_AngStart_BDV = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.AngStart_BDV +
            //    ////                          ", fldAnnulus_Exists = " + pExists +
            //    ////                          ", fldAnnulus_D = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D +
            //    ////                          ", fldAnnulus_Loc_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Loc_Back +
            //    ////                          ", fldAnnulus_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.L +
            //    ////                          ", fldOrifice_Dist_Holes = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Dist_Holes;

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_OilInlet" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Mill Relief"

            //    ////            public void UpdateRec_Bearing_Radial_FP_MillRelief(clsProject Project_In)
            //    ////            //=======================================================================       
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                int pExists = 0;
            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists) pExists = 1;

            //    ////                pstrSET = " SET fldExists = " + pExists +
            //    ////                          ", fldD_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_Desig + "'" +
            //    ////                          ", fldEDMRelief_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[0] +
            //    ////                          ", fldEDMRelief_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[1];

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_MillRelief" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Flange"

            //    ////            public void UpdateRec_Bearing_Radial_FP_Flange(clsProject Project_In)
            //    ////            //====================================================================      
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                int pExists = 0;
            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists) pExists = 1;

            //    ////                pstrSET = " SET fldExists = " + pExists +
            //    ////                          ", fldD = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D + "'" +
            //    ////                          ", fldWid = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid + "'" +
            //    ////                          ", fldDimStart_Front = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Front + "'";

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Flange" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - SL"

            //    ////            public void UpdateRec_Bearing_Radial_FP_SL(clsProject Project_In)
            //    ////            //================================================================      
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                pstrSET = " SET fldScrew_Spec_UnitSystem = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System.ToString() +        //BG 26MAR12
            //    ////                          "', fldScrew_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Type +
            //    ////                          "', fldScrew_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig + 
            //    ////                          "', fldScrew_Spec_Pitch = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch + 
            //    ////                          ", fldScrew_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L +
            //    ////                          ", fldScrew_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Mat +
            //    ////                          "', fldLScrew_Spec_Loc_Center = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Center +
            //    ////                          ", fldLScrew_Spec_Loc_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Front + 
            //    ////                          ", fldRScrew_Spec_Loc_Center = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Center +
            //    ////                          ", fldRScrew_Spec_Loc_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Front + 
            //    ////                          ", fldThread_Depth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Thread_Depth + 
            //    ////                          ", fldCBore_Depth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.CBore_Depth +
            //    ////                          ", fldDowel_Spec_UnitSystem = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System.ToString() +       //BG 26MAR12
            //    ////                          "', fldDowel_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Type +
            //    ////                          "', fldDowel_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig +
            //    ////                          "', fldDowel_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.L  +
            //    ////                          ", fldDowel_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Mat +
            //    ////                          "', fldLDowel_Spec_Loc_Center = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Center + 
            //    ////                          ", fldLDowel_Spec_Loc_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Front + 
            //    ////                          ", fldRDowel_Spec_Loc_Center = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Center +
            //    ////                          ", fldRDowel_Spec_Loc_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Front +
            //    ////                          ", fldDowel_Depth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Depth;

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_SL" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - AntiRotPin"

            //    ////            public void UpdateRec_Bearing_Radial_FP_AntiRotPin(clsProject Project_In)
            //    ////            //=======================================================================      
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                pstrSET = " SET fldLoc_Dist_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Dist_Front +
            //    ////                          ", fldLoc_Casing_SL = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL.ToString() +
            //    ////                          "', fldLoc_Offset = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Offset +
            //    ////                          ", fldLoc_Bearing_Vert = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert.ToString() +
            //    ////                          "', fldLoc_Bearing_SL = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL.ToString() +
            //    ////                          "', fldLoc_Angle = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Angle +
            //    ////                          ", fldSpec_UnitSystem = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Unit.System.ToString() +           //BG 26MAR12
            //    ////                          "', fldSpec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Type +
            //    ////                          "', fldSpec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig +
            //    ////                          "', fldSpec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.L +
            //    ////                          ", fldSpec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Mat +
            //    ////                          "', fldDepth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Depth +
            //    ////                          ", fldStickOut = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Stickout;


            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_AntiRotPin" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Mount"

            //    ////            public void UpdateRec_Bearing_Radial_FP_Mount(clsProject Project_In)
            //    ////            //===================================================================      
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                int pHolesGoThru = 0;
            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru) pHolesGoThru = 1;                              
                               

            //    ////                int pFixture_Candidate_Chosen_Front = 0; int pFixture_Candidate_Chosen_Back = 0;

            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0]) pFixture_Candidate_Chosen_Front = 1;
                                
            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[1]) pFixture_Candidate_Chosen_Back = 1;

            //    ////                pstrSET = " SET fldHoles_GoThru = " + pHolesGoThru +
            //    ////                          ", fldHoles_Bolting = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting +
            //    ////                          "',fldFixture_Candidates_Chosen_Front = " + pFixture_Candidate_Chosen_Front +
            //    ////                          ", fldFixture_Candidates_Chosen_Back = " + pFixture_Candidate_Chosen_Back +
            //    ////                          ", fldHoles_Thread_Depth_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0] +
            //    ////                          ", fldHoles_Thread_Depth_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1];

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Mount" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Mount Fixture"

            //    ////            public void CheckAndUpdate_Bearing_Radial_FP_Mount_Fixture(clsProject Project_In)
            //    ////            //===============================================================================   'BG 20MAR13      
            //    ////            {
            //    ////                StringCollection pBolting = new StringCollection();
            //    ////                pBolting = Retrieve_Bearing_Radial_FP_Mount_Fixture_Bolting_Pos_ORM(Project_In);

            //    ////                switch (pBolting.Count)
            //    ////                {
            //    ////                    case 1:
            //    ////                    //----- 

            //    ////                        //....Bolting = Front or Bolting = Back, when user changes Bolting = Both
            //    ////                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front ||
            //    ////                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
            //    ////                        {
            //    ////                            if (pBolting[0] == ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString())
            //    ////                            {
            //    ////                                UpdateRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
            //    ////                            }
            //    ////                            else
            //    ////                            {
            //    ////                                DeleteRecords_Table(Project_In, "tblProject_Bearing_Radial_FP_Mount_Fixture");
            //    ////                                //AddRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
            //    ////                                SaveToDB_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In);
            //    ////                            }
            //    ////                        }
            //    ////                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
            //    ////                        {
            //    ////                            DeleteRecords_Table(Project_In, "tblProject_Bearing_Radial_FP_Mount_Fixture");
            //    ////                            //AddRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
            //    ////                            SaveToDB_Bearing_Radial_FP_Mount_Fixture_ORM(Project_In);
            //    ////                        }
            //    ////                        break;

            //    ////                    case 2:
            //    ////                    //-----

            //    ////                        //....Bolting = Both, When user changes Bolting = Front or  Bolting = Back
            //    ////                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front ||
            //    ////                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
            //    ////                        {
            //    ////                            for (int i = 0; i < pBolting.Count; i++)
            //    ////                            {
            //    ////                                if (pBolting[i] == ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString())
            //    ////                                {
            //    ////                                    UpdateRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
            //    ////                                }
            //    ////                                else
            //    ////                                {
            //    ////                                    DeleteRecord_Bearing_Radial_FP_Mount_Fixture(Project_In, pBolting[i]);
            //    ////                                }
            //    ////                            }
            //    ////                        }
            //    ////                        else
            //    ////                        {
            //    ////                            UpdateRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
            //    ////                        }
            //    ////                        break;
            //    ////                }
            //    ////            }                            

            //    ////            public void UpdateRec_Bearing_Radial_FP_Mount_Fixture(clsProject Project_In)
            //    ////            //===========================================================================      
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting + "'";

            //    ////                int pHolesEquispaced = 0, pHolesAngStart_Comp_Chosen = 0;

            //    ////                //....Front
            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
            //    ////                {
            //    ////                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = 1;
            //    ////                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

            //    ////                    pstrSET = " SET fldPartNo = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo +
            //    ////                                "', fldDBC = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC +
            //    ////                                ", fldD_Finish = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish +
            //    ////                                ", fldHolesCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count +
            //    ////                                ", fldHolesEquispaced = " + pHolesEquispaced +
            //    ////                                ", fldHolesAngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart +
            //    ////                                ", fldHolesAngStart_Comp_Chosen = " + pHolesAngStart_Comp_Chosen +
            //    ////                                ", fldHolesAngOther1 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0] +
            //    ////                                ", fldHolesAngOther2 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1] +
            //    ////                                ", fldHolesAngOther3 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2] +
            //    ////                                ", fldHolesAngOther4 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3] +
            //    ////                                ", fldHolesAngOther5 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4] +
            //    ////                                ", fldHolesAngOther6 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5] +
            //    ////                                ", fldHolesAngOther7 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6] +
            //    ////                                ", fldScrew_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type +
            //    ////                                "', fldScrew_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig +
            //    ////                                "', fldScrew_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L +
            //    ////                                ", fldScrew_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat + "'";

            //    ////                    pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Mount_Fixture" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }

            //    ////                //....Back
            //    ////                else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back )
            //    ////                {
            //    ////                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = 1;
            //    ////                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

            //    ////                    pstrSET = " SET fldPartNo = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo +
            //    ////                                "', fldDBC = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC +
            //    ////                                ", fldD_Finish = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish +
            //    ////                                ", fldHolesCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count +
            //    ////                                ", fldHolesEquispaced = " + pHolesEquispaced +
            //    ////                                ", fldHolesAngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart +
            //    ////                                ", fldHolesAngStart_Comp_Chosen = " + pHolesAngStart_Comp_Chosen +
            //    ////                                ", fldHolesAngOther1 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0] +
            //    ////                                ", fldHolesAngOther2 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1] +
            //    ////                                ", fldHolesAngOther3 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2] +
            //    ////                                ", fldHolesAngOther4 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3] +
            //    ////                                ", fldHolesAngOther5 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4] +
            //    ////                                ", fldHolesAngOther6 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5] +
            //    ////                                ", fldHolesAngOther7 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6] +
            //    ////                                ", fldScrew_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type +
            //    ////                                "', fldScrew_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig +
            //    ////                                "', fldScrew_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L +
            //    ////                                ", fldScrew_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat + "'";

            //    ////                    pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Mount_Fixture" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
                                
            //    ////                //....Both
            //    ////                else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
            //    ////                {
            //    ////                    string[] pPosition = new string[] { "Front", "Back" };

            //    ////                    for (int i = 0; i < 2; i++)
            //    ////                    {
            //    ////                        pHolesEquispaced = 0; pHolesAngStart_Comp_Chosen = 0;
            //    ////                        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                            pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                        else
            //    ////                            pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                        pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.EquiSpaced) pHolesEquispaced = 1;
            //    ////                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

            //    ////                        pstrSET = " SET fldPartNo = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.PartNo +
            //    ////                                  "', fldDBC = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DBC +
            //    ////                                  ", fldD_Finish = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.D_Finish +
            //    ////                                  ", fldHolesCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Hole.Count +
            //    ////                                  ", fldHolesEquispaced = " + pHolesEquispaced +
            //    ////                                  ", fldHolesAngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngStart +
            //    ////                                  ", fldHolesAngStart_Comp_Chosen = " + pHolesAngStart_Comp_Chosen +
            //    ////                                  ", fldHolesAngOther1 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[0] +
            //    ////                                  ", fldHolesAngOther2 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[1] +
            //    ////                                  ", fldHolesAngOther3 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[2] +
            //    ////                                  ", fldHolesAngOther4 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[3] +
            //    ////                                  ", fldHolesAngOther5 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[4] +
            //    ////                                  ", fldHolesAngOther6 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[5] +
            //    ////                                  ", fldHolesAngOther7 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.HolesAngOther[6] +
            //    ////                                  ", fldScrew_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Type +
            //    ////                                  "', fldScrew_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.D_Desig +
            //    ////                                  "', fldScrew_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.L +
            //    ////                                  ", fldScrew_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw_Spec.Mat + "'";

            //    ////                        pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Mount_Fixture" + pstrSET + pstrWHERE;
            //    ////                        pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                    }
            //    ////                }
            //    ////            }

                            
                           
            //    ////        #endregion


            //    ////        #region "Update: Bearing Radial FP - Temp Sensor"

            //    ////            public void UpdateRec_Bearing_Radial_FP_TempSensor(clsProject Project_In)
            //    ////            //========================================================================      
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                int pExists = 0;
            //    ////                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists) pExists = 1;

            //    ////                pstrSET = " SET fldExists = " + pExists +
            //    ////                          ", fldCanLength = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength +
            //    ////                          ", fldCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count +
            //    ////                          ", fldLoc = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc +
            //    ////                          "', fldD = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D +
            //    ////                          ", fldDepth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth +
            //    ////                          ", fldAngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart;

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_TempSensor" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////    #endregion


            //    ////        #region "Update: Bearing Radial FP - EDM Pad"

            //    ////            public void UpdateRec_Bearing_Radial_FP_EDM_Pad(clsProject Project_In)
            //    ////            //====================================================================      
            //    ////            {
            //    ////                //....UPDATE Records.
            //    ////                String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////                int pCountRecords;

            //    ////                pstrSET = " SET fldRFillet_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.RFillet_Back +
            //    ////                          ", fldAngStart_Web = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.AngStart_Web;

            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_EDM_Pad" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }

            //    ////        #endregion

            //    ////#endregion


            //    ////    #region "Update: End Configs"

            //    ////        private void UpdateRec_EndConfigs(clsProject Project_In)
            //    ////        //=======================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;
            //    ////            string[] pPosition = new string[] { "Front", "Back" };  

            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                pstrSET = " SET fldType = '" + Project_In.Product.EndConfig[i].Type +
            //    ////                            "', fldMat_Base = '" + Project_In.Product.EndConfig[i].Mat.Base +
            //    ////                            "', fldMat_Lining = '" + Project_In.Product.EndConfig[i].Mat.Lining +
            //    ////                            "', fldDO = " + Project_In.Product.EndConfig[i].DO +
            //    ////                            ",  fldDBore_Range_Min = " + Project_In.Product.EndConfig[i].DBore_Range[0] +
            //    ////                            ",  fldDBore_Range_Max = " + Project_In.Product.EndConfig[i].DBore_Range[1] +
            //    ////                            ",  fldL = " + Project_In.Product.EndConfig[i].L;
                                         
            //    ////                pstrActionSQL = "UPDATE tblProject_EndConfigs" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }                                                             
                           
            //    ////        }
                    
            //    ////    #endregion


            //    ////    #region "Update: End Config - Mount Holes"

            //    ////        private void UpdateRec_EndConfig_MountHoles(clsProject Project_In)
            //    ////        //=================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;
            //    ////            string[] pPosition = new string[] { "Front", "Back" };
                         
            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                int pThread_Thru = 0; 
            //    ////                if (Project_In.Product.EndConfig[i].MountHoles.Thread_Thru) pThread_Thru = 1;

            //    ////                pstrSET = " SET fldType = '" + Project_In.Product.EndConfig[i].MountHoles.Type.ToString() +
            //    ////                            "', fldDepth_CBore = " + Project_In.Product.EndConfig[i].MountHoles.Depth_CBore +
            //    ////                            ",  fldThread_Thru = " + pThread_Thru + 
            //    ////                            ",  fldDepth_Thread = " + Project_In.Product.EndConfig[i].MountHoles.Depth_Thread;

            //    ////                pstrActionSQL = "UPDATE tblProject_EndConfig_MountHoles" + pstrSET + pstrWHERE;
            //    ////                pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            }
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: End Config - Seal Details"

            //    ////    private void Update_EndConfig_Seal_Detail(clsProject Project_In)
            //    ////    //==============================================================
            //    ////    {
            //    ////        Update_EndConfig_Seal(Project_In);
            //    ////        Update_EndConfig_Seal_Blade(Project_In);
            //    ////        Update_EndConfig_Seal_DrainHoles(Project_In);
            //    ////        Update_EndConfig_Seal_WireClipHoles(Project_In);
            //    ////    }

            //    ////    #region "Update: End Config - Seal"

            //    ////        private void Update_EndConfig_Seal(clsProject Project_In)
            //    ////        //========================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;                       

            //    ////            string[] pPosition = new string[] { "Front", "Back" };

            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    ////                {
            //    ////                    //SG 13DEC12
            //    ////                    pstrSET = " SET fldType = '" + ((clsSeal)Project_In.Product.EndConfig[i]).Design.ToString() +
            //    ////                                "', fldLiningT = " + ((clsSeal)Project_In.Product.EndConfig[i]).Mat_LiningT +
            //    ////                                ", fldTempSensor_D_ExitHole = " + ((clsSeal)Project_In.Product.EndConfig[i]).TempSensor_D_ExitHole;

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_Seal" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }                                                             
            //    ////        }
                        
            //    ////    #endregion

            //    ////    #region "Update: End Config Seal - Blade"

            //    ////        private void Update_EndConfig_Seal_Blade(clsProject Project_In)
            //    ////        //=============================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;
               
            //    ////            string[] pPosition = new string[] { "Front", "Back" };

            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    ////                {
            //    ////                    pstrSET = " SET fldCount = " + ((clsSeal)Project_In.Product.EndConfig[i]).Blade.Count +
            //    ////                                ", fldT = " + ((clsSeal)Project_In.Product.EndConfig[i]).Blade.T +
            //    ////                                ", fldAngTaper = " + ((clsSeal)Project_In.Product.EndConfig[i]).Blade.AngTaper;

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_Seal_Blade" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }
            //    ////        }

            //    ////    #endregion

            //    ////    #region "Update: End Config Seal - Drain Holes"

            //    ////        private void Update_EndConfig_Seal_DrainHoles(clsProject Project_In)
            //    ////        //==================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;
                                                          
            //    ////            string[] pPosition = new string[] { "Front", "Back" };

            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    ////                {
            //    ////                    pstrSET = " SET fldAnnulus_Ratio_L_H = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.Ratio_L_H +
            //    ////                                ", fldAnnulus_D = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.D +
            //    ////                                ", fldD_Desig = '" + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.D_Desig +
            //    ////                                "', fldCount = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Count +
            //    ////                                ", fldAngStart = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngStart +
            //    ////                                ", fldAngBet = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngBet +
            //    ////                                ", fldAngExit = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngExit;

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_Seal_DrainHoles" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }
            //    ////        }

            //    ////    #endregion

            //    ////    #region "Update: End Config Seal - Wire Clip Holes"

            //    ////        private void Update_EndConfig_Seal_WireClipHoles(clsProject Project_In)
            //    ////        //======================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            string[] pPosition = new string[] { "Front", "Back" };

                                
            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                int pExists = 0;
            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
            //    ////                {
            //    ////                    if (((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Exists) pExists = 1;

            //    ////                    pstrSET = " SET fldExists = " + pExists +
            //    ////                                ", fldCount = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Count +
            //    ////                                ", fldDBC = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.DBC +
            //    ////                                ", fldUnitSystem = '" + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Unit.System.ToString() +           //BG 03JUL13
            //    ////                                "', fldThread_Dia_Desig = '" + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.D_Desig +
            //    ////                                "', fldThread_Pitch = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.Pitch +
            //    ////                                ", fldThread_Depth = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.ThreadDepth +
            //    ////                                ", fldAngStart = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngStart +
            //    ////                                ", fldAngOther1 = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[0] +
            //    ////                                ", fldAngOther2 = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[1] ;

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_Seal_WireClipHoles" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }
            //    ////        }

            //    ////    #endregion

            //    ////#endregion


            //    ////    #region "Update: End Config - TB Details"

            //    ////    private void Update_EndConfig_TB_Detail(clsProject Project_In)
            //    ////    //==============================================================
            //    ////    {
            //    ////        Update_EndConfig_Thrust_TL(Project_In);
            //    ////        Update_EndConfig_Thrust_TL_PerformData(Project_In);
            //    ////        Update_EndConfig_Thrust_TL_FeedGroove(Project_In);
            //    ////        Update_EndConfig_Thrust_TL_WeepSlot(Project_In);
            //    ////        Update_EndConfig_Thrust_TL_GCodes(Project_In); 
            //    ////    }

            //    ////    #region "Update: End Config - Thrust_TL"

            //    ////        private void Update_EndConfig_Thrust_TL(clsProject Project_In)
            //    ////        //============================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            string[] pPosition = new string[] { "Front", "Back" };
                                
            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                int pReqd = 0;

            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    ////                {
            //    ////                    if (((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Reqd) pReqd = 1;

            //    ////                    pstrSET = " SET fldDirectionType = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DirectionType.ToString() +
            //    ////                                "', fldPad_ID = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[0] +
            //    ////                                ", fldPad_OD = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[1] +
            //    ////                                ", fldLandL = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LandL +
            //    ////                                ", fldLiningT_Face = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.Face +
            //    ////                                ", fldLiningT_ID = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.ID +
            //    ////                                ", fldPad_Count = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Pad_Count +
            //    ////                                ", fldTaper_Depth_OD = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_OD +
            //    ////                                ", fldTaper_Depth_ID = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_ID +
            //    ////                                ", fldTaper_Angle = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Angle +
            //    ////                                ", fldShroud_Ro = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ro +
            //    ////                                ", fldShroud_Ri = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ri +
            //    ////                                ", fldBackRelief_Reqd = " + pReqd +
            //    ////                                ", fldBackRelief_D = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.D +
            //    ////                                ", fldBackRelief_Depth = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Depth +
            //    ////                                ", fldBackRelief_Fillet = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Fillet +
            //    ////                                ", fldDimStart = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DimStart() +
            //    ////                                ", fldLFlange = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LFlange +
            //    ////                                ", fldFaceOff_Assy = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FaceOff_Assy;

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: End Config Thrust - Perform Data"

            //    ////        private void Update_EndConfig_Thrust_TL_PerformData(clsProject Project_In)
            //    ////        //========================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            string[] pPosition = new string[] { "Front", "Back" };

            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    ////                {
                                       
            //    ////                    pstrSET = " SET fldPower_HP = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.Power_HP +
            //    ////                        ", fldFlowReqd_gpm = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.FlowReqd_gpm +
            //    ////                        ", fldTempRise_F = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TempRise_F +
            //    ////                        ", fldTFilm_Min = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TFilm_Min +
            //    ////                        ", fldPadMax_Temp = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Temp +
            //    ////                        ", fldPadMax_Press = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Press +
            //    ////                        ", fldUnitLoad = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.UnitLoad;

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB_PerformData" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: End Config Thrust - Feed Groove"

            //    ////        private void Update_EndConfig_Thrust_TL_FeedGroove(clsProject Project_In)
            //    ////        //========================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            string[] pPosition = new string[] { "Front", "Back" };

            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    ////                {
            //    ////                    pstrSET = " SET fldType = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Type +
            //    ////                                "', fldWid = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Wid +
            //    ////                                ", fldDepth = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Depth +
            //    ////                                ", fldDBC = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.DBC +
            //    ////                                ", fldDist_Chamf = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Dist_Chamf;

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB_FeedGroove" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: End Config Thrust - Weep Slot"

            //    ////        private void Update_EndConfig_Thrust_TL_WeepSlot(clsProject Project_In)
            //    ////        //======================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            string[] pPosition = new string[] { "Front", "Back" };

            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    ////                {
            //    ////                    pstrSET = " SET fldType = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Type +
            //    ////                                "', fldWid = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Wid +
            //    ////                                ", fldDepth = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Depth;

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB_WeepSlot" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }
            //    ////        }

            //    ////    #endregion


            //    ////    #region "Update: End Config Thrust - GCodes"

            //    ////        private void Update_EndConfig_Thrust_TL_GCodes(clsProject Project_In)
            //    ////        //======================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            string[] pPosition = new string[] { "Front", "Back" };

            //    ////            for (int i = 0; i < pPosition.Length; i++)
            //    ////            {
            //    ////                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////                else
            //    ////                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

            //    ////                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

            //    ////                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.TL_TB)
            //    ////                {                                                   

            //    ////                    pstrSET = " SET fldD_Desig_T1 = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T1.D_Desig +
            //    ////                                "', fldD_Desig_T2 = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T2.D_Desig +
            //    ////                                "', fldD_Desig_T3 = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T3.D_Desig +
            //    ////                                "', fldD_Desig_T4 = '" +  ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T4.D_Desig +
            //    ////                                "', fldOverlap_frac = " +  ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Overlap_frac +
            //    ////                                ", fldFeed_Taperland = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.Taperland +
            //    ////                                ", fldFeed_WeepSlot = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.WeepSlot +
            //    ////                                ", fldDepth_Backlash = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Backlash +
            //    ////                                ", fldDepth_Dwell_T = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Dwell_T +
            //    ////                                //", fldRMargin_WeepSlot = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).RMargin_WeepSlot +       //BG 03APR13
            //    ////                                ", fldDepth_WeepSlot_Cut = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_WS_Cut_Per_Pass +
            //    ////                                //", fldStarting_LineNo = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Starting_LineNo + //BG 03APR13
            //    ////                                ", fldFilePath_Dir = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FilePath_Dir + "'";

            //    ////                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB_GCodes" + pstrSET + pstrWHERE;
            //    ////                    pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////                }
            //    ////            }
            //    ////        }

            //    ////    #endregion

            //    ////#endregion


            //    ////    #region "Update: Product Accessories"

            //    ////        private int Update_Project_Product_Accessories(clsProject Project_In)
            //    ////        //====================================================================
            //    ////        {
            //    ////            //....UPDATE Records.
            //    ////            String pstrSET = "", pstrWHERE, pstrActionSQL;
            //    ////            int pCountRecords;

            //    ////            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //    ////                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //    ////            else
            //    ////                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";
                                                     
            //    ////            int pTempSensor_Supplied = 0;
            //    ////            if (Project_In.Product.Accessories.TempSensor.Supplied) pTempSensor_Supplied = 1;

            //    ////            int pWireClip_Supplied = 0;
            //    ////            if (Project_In.Product.Accessories.WireClip.Supplied) pWireClip_Supplied = 1;

            //    ////            pstrSET = " SET fldTempSensor_Supplied = " + pTempSensor_Supplied + 
            //    ////                        ", fldTempSensor_Name = '" + Project_In.Product.Accessories.TempSensor.Name +
            //    ////                        "', fldTempSensor_Count = " + Project_In.Product.Accessories.TempSensor.Count +
            //    ////                        ", fldTempSensor_Type = '" + Project_In.Product.Accessories.TempSensor.Type +
            //    ////                        "', fldWireClip_Supplied = " + pWireClip_Supplied +
            //    ////                        ", fldWireClip_Count = " + Project_In.Product.Accessories.WireClip.Count +
            //    ////                        ", fldWireClip_Size = '" + Project_In.Product.Accessories.WireClip.Size + "'";                                        

            //    ////            pstrActionSQL = "UPDATE tblProject_Product_Accessories" + pstrSET + pstrWHERE;
            //    ////            pCountRecords = ExecuteCommand(pstrActionSQL);
            //    ////            return pCountRecords;                           
            //    ////        }

            //    ////    #endregion


            //    ////#endregion


            //    #region "Database Deletion Routine:"

                
            //        public void DeleteRecord_ORM(clsProject Project_In)
            //        //==================================================
            //        {
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Details");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Product");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_OpCond");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial");

            //            //DeleteRecords_Bearing_Radial_FP_Detail(Project_In);
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_PerformData");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_Pad");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_FlexurePivot");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_OilInlet");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_MillRelief");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_Flange");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_SL");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_AntiRotPin");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_Mount");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_Mount_Fixtur");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_TempSensor");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Bearing_Radial_FP_EDM_Pad");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfigs");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_MountHoles");

            //            //DeleteRecords_EndConfig_Seal_Detail(Project_In);
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_Seal");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_Seal_Blade");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_Seal_DrainHoles");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_Seal_WireClipHoles");

            //            //DeleteRecords_EndConfig_TB_Detail(Project_In);
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_TB");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_TB_PerformData");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_TB_FeedGroove");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_TB_WeepSlot");
            //            DeleteRecords_Table_ORM(Project_In, "tblProject_EndConfig_TB_GCodes");

            //            DeleteRecords_Table_ORM(Project_In, "tblProject_Product_Accessories");
            //        }

            //        #region "Deletion From Different Tables:"

            //            ////private void DeleteRecords_Table(clsProject Project_In, string TableName_In)
            //            //////=========================================================================
            //            ////{
            //            ////    string pstrWHERE = "";
            //            ////    string pSQL = "";

            //            ////    if (TableName_In == "tblProject_Details")
            //            ////    {
            //            ////        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //            ////            pstrWHERE = " WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix = '" + Project_In.No_Suffix + "'";
            //            ////        else
            //            ////            pstrWHERE = " WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix is NULL";
            //            ////    }                           
            //            ////    else
            //            ////    {
            //            ////        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //            ////            pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
            //            ////        else
            //            ////            pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";
            //            ////    }
                                                        
            //            ////    pSQL = "DELETE FROM " + TableName_In + pstrWHERE;
            //            ////    ExecuteCommand(pSQL);
            //            ////}

            //            private void DeleteRecords_Table_ORM(clsProject Project_In, string TableName_In)
            //            //================================================================================
            //            {
                           
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";
            //                if (Project_In.No_Suffix != null)
            //                    pNoSuffix = Project_In.No_Suffix;
            //                else
            //                    pNoSuffix = "NULL";

            //                if (TableName_In == "tblProject_Details")
            //                {
            //                   var pProject = (from pRec in pBearingDBEntities.tblProject_Details where pRec.fldNo == Project_In.No && pRec.fldNo_Suffix == pNoSuffix select pRec).ToList();
            //                   pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Product")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Product where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_OpCond")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_OpCond where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_PerformData")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_PerformData where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_Pad")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Pad where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_FlexurePivot")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_FlexurePivot where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_OilInlet")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_OilInlet where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_MillRelief")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_MillRelief where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_Flange")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Flange where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_SL")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_SL where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_AntiRotPin")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_AntiRotPin where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_Mount")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_Mount_Fixture")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount_Fixture where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_TempSensor")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_TempSensor where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Bearing_Radial_FP_EDM_Pad")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_EDM_Pad where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfigs")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfigs where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_MountHoles")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_MountHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_Seal")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_Seal_Blade")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_Blade where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_Seal_DrainHoles")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_DrainHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_Seal_WireClipHoles")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_Seal_WireClipHoles where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_TB")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_TB_PerformData")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_PerformData where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_TB_FeedGroove")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_FeedGroove where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_TB_WeepSlot")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_WeepSlot where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_EndConfig_TB_GCodes")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_EndConfig_TB_GCodes where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
            //                else if (TableName_In == "tblProject_Product_Accessories")
            //                {
            //                    var pProject = (from pRec in pBearingDBEntities.tblProject_Product_Accessories where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix select pRec).ToList();
            //                    pBearingDBEntities.DeleteObject(pProject);
            //                }
                           
            //                pBearingDBEntities.SaveChanges();
                           
            //            }


            //            #region "Delete: Bearing Radial FP Detail"

            //                #region "Delete: Bearing Radial FP Mount_Fixture"

            //            ////private void DeleteRecord_Bearing_Radial_FP_Mount_Fixture(clsProject Project_In, string Position_In)
            //            //////================================================================================================== BG 28FEB13
            //            ////{
            //            ////    string pstrWHERE = "";
            //            ////    string pSQL = "";

            //            ////    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
            //            ////        pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No +
            //            ////                    "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix +
            //            ////                    "' AND fldPosition = '" + Position_In + "'";
            //            ////    else
            //            ////        pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + Position_In + "'";


            //            ////    pSQL = "DELETE FROM  tblProject_Bearing_Radial_FP_Mount_Fixture" + pstrWHERE;
            //            ////    ExecuteCommand(pSQL);
            //            ////}

            //            private void DeleteRecord_Bearing_Radial_FP_Mount_Fixture_ORM(clsProject Project_In, string Position_In)
            //            //======================================================================================================= AES 26JUN18
            //            {
            //                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            //                string pNoSuffix = "";
            //                if (Project_In.No_Suffix != null)
            //                    pNoSuffix = Project_In.No_Suffix;
            //                else
            //                    pNoSuffix = "NULL";

            //                var pProject = (from pRec in pBearingDBEntities.tblProject_Bearing_Radial_FP_Mount_Fixture where pRec.fldProjectNo == Project_In.No && pRec.fldProjectNo_Suffix == pNoSuffix && pRec.fldPosition == Position_In select pRec).ToList();
            //                pBearingDBEntities.DeleteObject(pProject);

            //                pBearingDBEntities.SaveChanges();
            //            }
            //                #endregion

            //            ////private void DeleteRecords_Bearing_Radial_FP_Detail(clsProject Project_In)
            //            //////=========================================================================
            //            ////{
            //            ////    string[] pTblName = new string[]{"tblProject_Bearing_Radial_FP", "tblProject_Bearing_Radial_FP_PerformData",
            //            ////                                     "tblProject_Bearing_Radial_FP_Pad", "tblProject_Bearing_Radial_FP_FlexurePivot",
            //            ////                                     "tblProject_Bearing_Radial_FP_OilInlet", "tblProject_Bearing_Radial_FP_MillRelief",
            //            ////                                     "tblProject_Bearing_Radial_FP_Flange", "tblProject_Bearing_Radial_FP_SL",
            //            ////                                     "tblProject_Bearing_Radial_FP_AntiRotPin", "tblProject_Bearing_Radial_FP_Mount",
            //            ////                                     "tblProject_Bearing_Radial_FP_Mount_Fixture", "tblProject_Bearing_Radial_FP_TempSensor",
            //            ////                                     "tblProject_Bearing_Radial_FP_EDM_Pad"};

            //            ////    for (int i = 0; i < pTblName.Length; i++)
            //            ////    {
            //            ////        DeleteRecords_Table(Project_In, pTblName[i]);
            //            ////    }
            //            ////}

            //            #endregion


                       

            //            #region "Delete: End Config Seal Details"

            //                ////private void DeleteRecords_EndConfig_Seal_Detail(clsProject Project_In)
            //                //////=====================================================================
            //                ////{
            //                ////    string[] pTblName = new string[]{"tblProject_EndConfig_Seal", "tblProject_EndConfig_Seal_Blade",
            //                ////                                        "tblProject_EndConfig_Seal_DrainHoles", "tblProject_EndConfig_Seal_WireClipHoles"};

            //                ////    for (int i = 0; i < pTblName.Length; i++)
            //                ////    {
            //                ////        DeleteRecords_Table_ORM(Project_In, pTblName[i]);
            //                ////    }
            //                ////}

            //            #endregion


            //            #region "Delete: End Config TB Details"

            //                ////private void DeleteRecords_EndConfig_TB_Detail(clsProject Project_In)
            //                //////===================================================================
            //                ////{
            //                ////    string[] pTblName = new string[]{"tblProject_EndConfig_TB",  "tblProject_EndConfig_TB_PerformData", 
            //                ////                                     "tblProject_EndConfig_TB_FeedGroove", "tblProject_EndConfig_TB_WeepSlot",
            //                ////                                     "tblProject_EndConfig_TB_GCodes"};

            //                ////    for (int i = 0; i < pTblName.Length; i++)
            //                ////    {
            //                ////        DeleteRecords_Table_ORM(Project_In, pTblName[i]);
            //                ////    }
            //                ////}

            //            #endregion

            //        #endregion

            //    #endregion
        
            //#endregion


            #region "Populate Combo & List Boxes, String Collections:"
            //--------------------------------------------------------
                

                ////public int PopulateCmbBox(ComboBox cmbBox_In,
                ////                            string strTableName_In, string strFldName_In,
                ////                            string strWHERE_In, bool blnOrderBy_In)
                //////============================================================================   
                ////{
                ////    //....This utility function populates a comboBox and 
                ////    //......returns the # of list items, if any.

                ////    //This routine populate comboboxes
                ////    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                ////    //       Output  Parameters      :   No of Records

                ////    //....Create the SQL string.   
                ////    //

                ////    string pstrORDERBY = "";
                ////    if (blnOrderBy_In == true)
                ////        pstrORDERBY = " ORDER BY " + "[" + strFldName_In + "]" + " ASC";

                ////    string pstrSQL = "";

                ////    pstrSQL = "SELECT " + " DISTINCT " + strFldName_In + " FROM " +
                ////                strTableName_In + " " + strWHERE_In + pstrORDERBY;

                ////    //....Get the corresponding data reader object.
                ////    SqlDataReader pobjDR = null;
                ////    SqlConnection pConnection = new SqlConnection();

                ////    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                ////    //....Store the ordinal for the given field for better performance.
                ////    int pColFldName = 0;
                ////    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                ////    //Add list items to the Combo Box
                ////    //-------------------------------
                ////    int pCountRec = 0;
                ////    string pRowVal = "";

                ////    cmbBox_In.Items.Clear();
                ////    while (pobjDR.Read())
                ////    {
                ////        pCountRec = pCountRec + 1;
                ////        if (pobjDR.IsDBNull(pColFldName) == false)
                ////            pRowVal = Convert.ToString(pobjDR[pColFldName]);

                ////        //if (pRowVal != "")
                ////        cmbBox_In.Items.Add(pRowVal);
                ////    }

                ////    pobjDR.Close();
                ////    pConnection.Close();

                ////    return pCountRec;
                ////}

                ////public int PopulateCmbBox(ComboBox cmbBox_In,
                ////                        string strTableName_In, string strFldName_In,
                ////                        int Val_In, string strWHERE_In, bool blnOrderBy_In)
                //////============================================================================   
                ////{
                ////    //....This utility function populates a comboBox and 
                ////    //......returns the # of list items, if any.

                ////    //This routine populate comboboxes
                ////    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                ////    //       Output  Parameters      :   No of Records

                ////    //....Create the SQL string.   
                ////    //

                ////    string pstrORDERBY = "";
                ////    if (blnOrderBy_In == true)
                ////        pstrORDERBY = " ORDER BY " + "[" + strFldName_In + "]" + " ASC";

                ////    string pstrSQL = "";

                ////    pstrSQL = "SELECT " + " DISTINCT " + strFldName_In + " FROM " +
                ////                strTableName_In + " " + strWHERE_In + pstrORDERBY;

                ////    //....Get the corresponding data reader object.
                
                ////    SqlConnection pConnection = new SqlConnection();   
                ////    SqlDataReader pobjDR = null;
                ////    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                ////    //....Store the ordinal for the given field for better performance.
                ////    int pColFldName = 0;
                ////    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                ////    //Add list items to the Combo Box
                ////    //-------------------------------
                ////    int pCountRec = 0;
                ////    string pRowVal = "";
                ////    Single pModVal;

                ////    cmbBox_In.Items.Clear();
                ////    while (pobjDR.Read())
                ////    {
                ////        pCountRec = pCountRec + 1;
                ////        if (pobjDR.IsDBNull(pColFldName) == false)
                ////            pRowVal = Convert.ToString(pobjDR[pColFldName]);
                ////        pModVal = Convert.ToSingle(pRowVal) / Val_In;

                ////        cmbBox_In.Items.Add(pModVal.ToString());
                ////    }

                ////    pConnection.Close();

                ////    pobjDR.Close();
                ////    return pCountRec;
                ////}


                ////public int PopulateLstBox(ListBox listBox_In,string strTableName_In, string strFldName_In,
                ////                          string strWHERE_In, bool blnOrderBy_In)
                //////========================================================================================   
                ////{
                ////    //....This utility function populates a comboBox and 
                ////    //......returns the # of list items, if any.

                ////    //This routine populate comboboxes
                ////    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                ////    //       Output  Parameters      :   No of Records

                ////    //....Create the SQL string.   
                ////    //

                ////    string pstrORDERBY = "";
                ////    if (blnOrderBy_In == true)
                ////        pstrORDERBY = " ORDER BY " + "[" + strFldName_In + "]" + " ASC";

                ////    string pstrSQL = "";

                ////    pstrSQL = "SELECT " + strFldName_In + " FROM " +
                ////                strTableName_In + " " + strWHERE_In + pstrORDERBY;

                ////    //....Get the corresponding data reader object.

                ////    SqlConnection pConnection = new SqlConnection();
                ////    SqlDataReader pobjDR = null;
                ////    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                ////    //....Store the ordinal for the given field for better performance.
                ////    int pColFldName = 0;
                ////    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                ////    //Add list items to the Combo Box
                ////    //-------------------------------
                ////    int pCountRec = 0;
                ////    string pRowVal = "";               

                ////    listBox_In.Items.Clear();
                ////    while (pobjDR.Read())
                ////    {
                ////        pCountRec = pCountRec + 1;
                ////        if (pobjDR.IsDBNull(pColFldName) == false)
                ////            pRowVal = Convert.ToString(pobjDR[pColFldName]);

                ////        listBox_In.Items.Add(pRowVal.ToString());
                ////    }

                ////    pConnection.Close();

                ////    pobjDR.Close();
                ////    return pCountRec;
                ////}

            
                //PB 18JAN12. To be reviewed later.
                ////public int PopulateStringCol(StringCollection strCol_In, 
                ////                         string strTableName_In, string strFldName_In,
                ////                         string strWHERE_In, bool blnOrderBy_In)
                //////============================================================================    
                ////{
                ////    //....This utility function populates a comboBox and 
                ////    //......returns the # of list items, if any.

                ////    //This routine populate comboboxes
                ////    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                ////    //       Output  Parameters      :   No of Records

                ////    //....Create the SQL string.   
                ////    //

                ////    string pstrORDERBY = "";
                ////    if (blnOrderBy_In == true)
                ////        pstrORDERBY = " ORDER BY " + "[" + strFldName_In + "]" + " ASC";

                ////    string pstrSQL = "";

                ////    pstrSQL = "SELECT " + " DISTINCT " + strFldName_In + " FROM " +
                ////                strTableName_In + " " + strWHERE_In + pstrORDERBY;


                ////    //....Get the corresponding data reader object.
                ////    SqlConnection pConnection = new SqlConnection();   //SB 06JUL09

                ////    SqlDataReader pobjDR = null;
                ////    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                ////    //....Store the ordinal for the given field for better performance.
                ////    int pColFldName = 0;
                ////    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                ////    //Add list items to the Combo Box
                ////    //-------------------------------
                ////    int pCountRec = 0;
                ////    string pRowVal = "";

                ////    strCol_In.Clear();
                ////    while (pobjDR.Read())
                ////    {
                ////        pCountRec = pCountRec + 1;
                ////        if (pobjDR.IsDBNull(pColFldName) == false)
                ////            pRowVal = Convert.ToString(pobjDR[pColFldName]);

                ////        //if (pRowVal != "")
                ////        strCol_In.Add(pRowVal);                  
                ////    }

                ////    pConnection.Close();

                ////    pobjDR.Close();
                ////    return pCountRec;
                ////}


                ////public int PopulateStringCol(StringCollection strCol_In,
                ////                                 string strTableName_In, string strFldName_In,
                ////                                 string strWHERE_In)
                //////============================================================================      //SB 27APR09
                ////{
                ////    //....This utility function populates a comboBox and 
                ////    //......returns the # of list items, if any.

                ////    //This routine populate comboboxes
                ////    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                ////    //       Output  Parameters      :   No of Records

                ////    //....Create the SQL string.   
                ////    //

                ////    string pstrSQL = "";

                ////    pstrSQL = "SELECT " + strFldName_In + " FROM " +
                ////                            strTableName_In + " " + strWHERE_In;    //SB 09JUL09

                ////    //....Get the corresponding data reader object.
                ////    SqlConnection pConnection = new SqlConnection();   
                
                ////    SqlDataReader pobjDR = null;
                ////    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                ////    //....Store the ordinal for the given field for better performance.
                ////    int pColFldName = 0;
                ////    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                ////    //Add list items to the Combo Box
                ////    //-------------------------------
                ////    int pCountRec = 0;
                ////    string pRowVal = "";

                ////    strCol_In.Clear();
                ////    while (pobjDR.Read())
                ////    {
                ////        pRowVal = "";   //SB 19JUN09
                ////        pCountRec = pCountRec + 1;
                ////        if (pobjDR.IsDBNull(pColFldName) == false)
                ////            pRowVal = Convert.ToString(pobjDR[pColFldName]);

                ////        //if (pRowVal != "")
                ////        strCol_In.Add(pRowVal);
                ////    }

                ////    pobjDR.Close();
                ////    pConnection.Close();

                ////    return pCountRec;
                ////}

            #endregion


            #region "Data Checking - Retrieval & Insertion:"
            //---------------------------------------------

                public string CheckDBString(SqlDataReader DR_In, String FieldName_In)
                //===================================================================
                {
                      string pStrFldVal;
                      if (Convert.IsDBNull(DR_In[FieldName_In]))
                          pStrFldVal = "";
                      else
                          pStrFldVal = Convert.ToString(DR_In[FieldName_In]);

                      return pStrFldVal;
                }

                public string CheckDBString(Object Val_In)
                //========================================  //AES 14AUG18
                {
                    string pStrFldVal;
                    if (Val_In == null)
                        pStrFldVal = "";
                    else
                        pStrFldVal = Convert.ToString(Val_In);

                    return pStrFldVal;
                }


                ////public DateTime CheckDBDateTime(SqlDataReader DR_In, String FieldName_In)
                //////===================================================================
                ////{
                ////    DateTime pDtFldVal;
                ////    if (Convert.IsDBNull(DR_In[FieldName_In]))
                ////        pDtFldVal = DateTime.MinValue;
                ////    else
                ////        pDtFldVal = Convert.ToDateTime(DR_In[FieldName_In]);

                ////    return pDtFldVal;
                ////}

                //public DateTime CheckDBDateTime(DateTime Val_In)
                ////==============================================  //AES 28MAY18
                //{
                //    DateTime pDtFldVal;
                //    if (Convert.IsDBNull(Val_In))
                //        pDtFldVal = DateTime.MinValue;
                //    else
                //        pDtFldVal = Convert.ToDateTime(Val_In);

                //    return pDtFldVal;
                //}

                ////public Int32 CheckDBInt(SqlDataReader DR_In, String FieldName_In)
                //////===================================================================
                ////{
                ////    Int32 pIntFldVal;
                ////    if (Convert.IsDBNull(DR_In[FieldName_In]))
                ////        pIntFldVal = 0;
                ////    else
                ////        pIntFldVal = Convert.ToInt32(DR_In[FieldName_In]);

                ////    return pIntFldVal;
                ////}

                public Int32 CheckDBInt(Object Val_In)
                //====================================  //AES 14AUG18
                {
                    Int32 pIntFldVal;
                    if (Val_In == null)
                        pIntFldVal = 0;
                    else
                        pIntFldVal = Convert.ToInt32(Val_In);

                    return pIntFldVal;
                }

                ////public Single CheckDBSingle(SqlDataReader DR_In, String FieldName_In)
                //////===================================================================
                ////{
                ////    Single pSngFldVal;
                ////    if (Convert.IsDBNull(DR_In[FieldName_In]))
                ////        pSngFldVal = 0;
                ////    else
                ////        pSngFldVal = Convert.ToSingle(DR_In[FieldName_In]);

                ////    return pSngFldVal;
                ////}

                ////public Single CheckDBSingle(Object Val_In)
                //////========================================  //AES 14AUG18
                ////{
                ////    Single pSngFldVal;
                ////    if (Val_In == null)
                ////        pSngFldVal = 0;
                ////    else
                ////        pSngFldVal = Convert.ToSingle(Val_In);

                ////    return pSngFldVal;
                ////}
                ////public Double CheckDBDouble(SqlDataReader DR_In, String FieldName_In)
                //////===================================================================
                ////{
                ////    Double pDblFldVal;
                ////    if (Convert.IsDBNull(DR_In[FieldName_In]))
                ////        pDblFldVal = 0;
                ////    else
                ////        pDblFldVal = Convert.ToDouble(DR_In[FieldName_In]);

                ////    return pDblFldVal;
                ////}

                public Double CheckDBDouble(Object Val_In)
                //========================================  //AES 14AUG18
                {
                    Double pDblFldVal;
                    if (Val_In == null)
                        pDblFldVal = 0;
                    else
                        pDblFldVal = Convert.ToDouble(Val_In);

                    return pDblFldVal;
                }

                //PB 17DEC12. BG, check.
                ////public Boolean CheckDBBoolean(SqlDataReader DR_In, String FieldName_In)
                //////===================================================================== 
                ////{
                ////    Boolean pFldVal = false;
                ////    if (Convert.IsDBNull(DR_In[FieldName_In]))
                ////        pFldVal = false;
                ////    else
                ////        pFldVal = Convert.ToBoolean(DR_In[FieldName_In]);

                ////    return pFldVal;
                ////}

                public Boolean CheckDBBoolean(Object Val_In)
                //==========================================  //AES 14AUG18 
                {
                    Boolean pFldVal = false;
                    if (Val_In == null)
                        pFldVal = false;
                    else
                        pFldVal = Convert.ToBoolean(Val_In);

                    return pFldVal;
                }

                ////public bool CheckDBBool(SqlDataReader DR_In, String FieldName_In)
                //////===============================================================
                ////{
                ////    bool pFldVal = false;

                ////    if (Convert.IsDBNull(DR_In[FieldName_In]))
                ////        pFldVal = false;
                ////    else
                ////        pFldVal = Convert.ToBoolean(DR_In[FieldName_In]);

                ////    return pFldVal;
                ////}

                public bool CheckDBBool(Object Val_In)
                //====================================  //AES 14AUG18
                {
                    bool pFldVal = false;

                    if (Val_In == null)
                        pFldVal = false;
                    else
                        pFldVal = Convert.ToBoolean(Val_In);

                    return pFldVal;
                }

                //public Boolean ProjectNo_Exists (string No_In,string No_Suffix_In)
                ////===================================================================
                //{
                //    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                //    Boolean pblnChkProjectNo;

                //    var pQryProjectDetails = (from pRec in pBearingDBEntities.tblProject_Details where 
                //                              pRec.fldNo == No_In && pRec.fldNo_Suffix == No_Suffix_In select pRec).ToList();

                //    if (pQryProjectDetails.Count() > 0)
                //    {
                //        pblnChkProjectNo = true;
                //    }
                //    else
                //    {
                //        pblnChkProjectNo = false;
                //    }
                    
                //    return pblnChkProjectNo;
                //}

            #endregion

        #endregion

    }
}


