using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.OleDb;

using com.lexnetcg.saleslogix.data;
using com.lexnetcg.saleslogix.sql;

namespace com.lexnetcg.saleslogix.utils
{
    public class SLX_Utils : SLX
    {
        public static void Change_Account_Owner(string accountID, string newSeccodeID, OleDbConnection conn, bool debug)
        {
            /*
             *================================================================
             *=== 2006-01-20
             *=== Used to change account ownership
             *=== Elements 1 and 0 are there so you could have account_id and c_seccodeid
             *=== It is not necessary For the base tables but gives flexibility in the future
             *================================================================
             */

            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            string[,] arrTables = new string[4,2];
            int i;         
            string query;

            arrTables[0,0] = "ACCOUNT";   //Table
            arrTables[0,1] = "ACCOUNTID"; //AccountID Field
            arrTables[0,2] = "SECCODEID"; //SeccodeID Field

            arrTables[1,0] = "ACCOUNTSUMMARY";
            arrTables[1,1] = "ACCOUNTID";
            arrTables[1,2] = "SECCODEID";

            arrTables[2,0] = "CONTACT";
            arrTables[2,1] = "ACCOUNTID";
            arrTables[2,2] = "SECCODEID";

            arrTables[3,0] = "OPPORTUNITY";
            arrTables[3,1] = "ACCOUNTID";
            arrTables[3,2] = "SECCODEID";

            arrTables[4,0] = "TICKET";
            arrTables[4,1] = "ACCOUNTID";
            arrTables[4,2] = "SECCODEID";

            if(newSeccodeID.Trim() != "")
            {
                for(i = 0; i < arrTables.Length; i++)
                {
                    query = "SELECT " + arrTables[i,2] + " FROM " + arrTables[i,0] + " WHERE " + arrTables[i,1] + " = " + SLX_Data.addQuotes(accountID) + "";

                    if(debug == true)
                    {
                        sendToLog(query);
                    }

                    da.SelectCommand = new OleDbCommand(query, conn);

                    ds = fillDataSet(query, arrTables[i, 0], conn, da, ds);

                    if (ds.Tables[arrTables[i, 0]].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[arrTables[i, 0]].Rows)
                        {
                            dr[arrTables[i, 2]] = newSeccodeID;
                        }

                        SLX_Data.updateDataSet(ds, da, arrTables[i, 0], debug);
                    }


                }                   
            }
            else
            {

            }
        }

        public static void addEmptyRecord(string strTable, string strFKeyField, string strFKeyFieldValue)
        {
            /*
             *================================================================
             *=== Lexnet(JRM) on 2005-04-20
             *=== Insert a blank recOrd. This is to allow For adding one to one
             *=== and one to many recOrds at the same time with out first having to
             *=== save so that the realtionship to the one to many can be built.
             *=== Did this due to perFormance issues with MainViews and disconnected
             *=== recOrdSets
             *=== pass in an exisiting ID in strExisitingID to add recOrds based on existingID
             *=== Did this so I could add recOrds to 2 Or mOre tables using the same ID
             *================================================================
             */

            //Dim objCN
            //Dim objRS
            //Dim string query
            //Dim strNewRecOrdID
            //Dim strSecCodeID

            //strNewRecOrdID = Application.BasicFunctions.GetIDFor(strTable)

            //Select Case UCase(strTable)
            //       Case "OPPOrTUNITY"
            //            strSecCodeID = "SYST00000001"
            //       Case else

            //End Select

            //if strFKeyFieldValue = "" Or IsNull(strFKeyFieldValue))
            //   strFKeyFieldValue = "NULL"
            //}

            //string query = "INSERT INTO " & strTable & "(" & _
            //                            strTable & "ID,"

            //if Trim(strFKeyField) != "" And Trim(strFKeyFieldValue) != "")
            //   string query = string query & strFKeyField & ","
            //}

            //if strSecCodeID = "SYST00000001")
            //   string query = string query & "SECCODEID,"
            //}

            //string query = string query & Query_Time_Stamp("Add","Columns")
            //string query = string query & ") values("
            //string query = string query & Add_Quotes(strNewRecOrdID) & ","



            //if Trim(strFKeyField) != "" And Trim(strFKeyFieldValue) != "")
            //   string query = string query & Add_quotes(strFKeyFieldValue) & ","
            //}

            //if strSecCodeID = "SYST00000001")
            //   string query = string query & Add_Quotes(strSecCodeID) & ","
            //}

            //string query = string query & Query_Time_Stamp("Add","Values")
            //string query = string query & ")"

            //Set objCN = Application.GetNewConnection
            //Set objRS = objCN.Execute(string query)

            //if objRS.State = adStateOpen) objRS.Close
            //Set objRS = Nothing
            //objCN.Close
            //Set objCN = Nothing

            //Add_Empty_RecOrd = strNewRecOrdID
        }       

        public static bool isUniqueRecord(string table, string lookupField, string lookupValue)
        {
            bool returnValue = false;            
            SLX_SQL sql = new SLX_SQL();

            try
            {
                sql.Table = table;
                sql.LookupField = lookupField;
                sql.LookupValue = lookupValue;

                sql.Sql = "SELECT COUNT(*) FROM " + table + " WHERE " + lookupField + " = " + SLX_Data.addQuotes(lookupValue) + "";

                if (sql.countRecords() == 0)
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }   

            return returnValue;
        }

        public static bool isUniqueRecordSQL(string query, OleDbConnection conn)
        {
            bool returnValue;

            if(query.Trim() != "")
            {
                if (SLX_Data.countRecords(query, conn) == 0)
               {
                    returnValue = true;
               }
               else
               {
                    returnValue = false;
               }
            }
            else
            {
                returnValue = false;
            }

            return returnValue;
        }

        public static string addDblQuotes(string value)
        {
            string returnValue;

            returnValue = "" + value + "";
            
            return returnValue;
        }        

        public static bool isDepartmentMember(string userID, string department)
        {
            bool returnValue = false;
            string departmentID;
            string query;            
            string userTeamID;
            SLX_SQL sql = new SLX_SQL();

            //if Trim(strUserID) <> "" Then
            
            //End if

            if(userID.Trim() == "ADMIN")
            {
               returnValue = true;
            }
            else
            {
                sql = new SLX_SQL();

                //Get users UserTeamID Value
                query = "SELECT " +
                            "CHILDSECCODEID " +
                        "FROM " +
                            "SECCODEJOINS " +
                        "WHERE " + 
                            "PARENTSECCODEID = " + SLX_Data.addQuotes(userID) + " AND " +
                            "Right(PARENTSECCODEID, 11) = Right(CHILDSECCODEID, 11) ";

                sql.Sql = query;
                userTeamID = sql.getDataValue();
                
                //Get DepartmentID
                query = "SELECT " +
                            "SECCODEID " +
                        "FROM " + 
                            "SECCODE " +
                        "WHERE " +
                            "SECCODEDESC = " + SLX_Data.addQuotes(department) + " AND " + 
                            "SECCODETYPE = 'D'";

                sql.Sql = query;
                departmentID = sql.getDataValue();            

                query = "SELECT " +
                            "CHILDSECCODEID " +
                        "FROM " +
                            "SECCODEJOINS " +
                        "WHERE " + 
                            "PARENTSECCODEID = " + SLX_Data.addQuotes(departmentID) + " AND " +
                            "CHILDSECCODEID = " + SLX_Data.addQuotes(userTeamID) + "";

                sql.Sql = query;
                sql.getDataValue();

                if(departmentID.Trim() != "")
                {
                    //if Trim(Right(strUserID, 11)) = Trim(Right(Get_Data_Value_SQL(strQuery, ""), 11))
                    //{
                    //    returnValue = true;
                    //}
                    //else
                    //{    
                    //    returnValue = false;
                    //}
                }
                else
                {
                    returnValue = false;
                }           
            }      

            //Uncomment for debugging
            //msgbox strGlobalPrefix & strVarPrefix & strVarName & " = " & Application.BasicFunctions.GlobalInfoFor(strGlobalPrefix & strVarPrefix & strVarName)

            return returnValue;
        }

        public static void addEditSLXCustomSetting(string settingDescription, string settingValue, string settingCategory, string settingDataType, string settingDataValidation, OleDbConnection conn,  bool debug)
        {
            DateTime currentDateTime = DateTime.Now;
            string customSettingsID = SLX_Data.getDataValue("CUSTOMSETTINGS", "CUSTOMSETTINGSID", "DESCRIPTION", settingDescription, conn);
            string customSettingsQuery;

            if (customSettingsID == "")
            {
                customSettingsID = SLX_Data.newSLXID("CUSTOMSETTINGS", conn);

                customSettingsQuery = "INSERT INTO CUSTOMSETTINGS(" +
                                        "CUSTOMSETTINGSID, " +
                                        "CREATEDATE, " +
                                        "CREATEUSER, " +
                                        "MODIFYDATE, " +
                                        "MODIFYUSER, " +
                                        "CATEGORY, " +
                                        "DESCRIPTION, " +
                                        "VERSIONNUMBER, " +
                                        "DATATYPE, " +
                                        "DATAVALIDATION, " +
                                        "DATAVALUE, " +
                                        "SECCODEID" +
                                    ") VALUES(" +
                                        SLX_Data.addQuotes(customSettingsID) + ", " +
                                        SLX_Data.addQuotes(SLX_Data.dateTimeToISODateString(currentDateTime)) + ", " +
                                        SLX_Data.addQuotes("ADMIN") + ", " +
                                        SLX_Data.addQuotes(SLX_Data.dateTimeToISODateString(currentDateTime)) + ", " +
                                        SLX_Data.addQuotes("ADMIN") + ", " +
                                        SLX_Data.addQuotes(settingCategory) + ", " +
                                        SLX_Data.addQuotes(settingDescription) + ", " +
                                        SLX_Data.addQuotes("") + ", " +
                                        SLX_Data.addQuotes(settingDataType) + ", " +
                                        SLX_Data.addQuotes(settingDataValidation) + ", " +
                                        SLX_Data.addQuotes(settingValue) + ", " +
                                        SLX_Data.addQuotes("SYST00000001") + "" +
                                    ")";
            }
            else
            {
                customSettingsQuery = "UPDATE CUSTOMSETTINGS SET " +
                                "DATAVALUE = " + SLX_Data.blankStringToNull(SLX_Data.addQuotes(settingValue)) + " " +
                            "WHERE " +
                                "CUSTOMSETTINGSID = " + SLX_Data.addQuotes(customSettingsID) + "";
            }

            if (debug == true)
            {
                sendToFile(customSettingsQuery + "\r\n", @"C:\SQL_Debug.txt");
            }

            SLX_Data.executeSQL(customSettingsQuery, conn);
        }

        public static void addEditSLXOption(string optionName, string optionValue, string userID, OleDbConnection conn, bool debug)
        {            
            string optionID = SLX_Data.getDataValue("USEROPTIONS", "OPTIONID", "NAME", optionName, conn);
            string optionQuery;
            
            if (optionID == "")
            {
                optionID = System.Guid.NewGuid().ToString();
                optionID = optionID.Replace("-", "");
                optionID = optionID.ToUpper();

                optionQuery = "INSERT INTO USEROPTIONS(" +
                                        "OPTIONID, " +
                                        "NAME, " +
                                        "CATEGORY, " +
                                        "USERID, " +
                                        "OPTIONVALUE, " +
                                        "LOCKED" +
                                    ") VALUES(" +
                                        SLX_Data.addQuotes(optionID) + ", " +
                                        SLX_Data.addQuotes(optionName) + ", " +
                                        SLX_Data.addQuotes("CustomOption") + ", " +
                                        SLX_Data.addQuotes(userID) + ", " +
                                        SLX_Data.addQuotes(optionValue) + ", " +
                                        SLX_Data.addQuotes("F") + "" +
                                    ")";                
            }
            else
            {
                optionQuery = "UPDATE USEROPTIONS SET " +
                                "OPTIONVALUE = " + SLX_Data.blankStringToNull(SLX_Data.addQuotes(optionValue)) + " " +
                            "WHERE " +
                                "OPTIONID = " + SLX_Data.addQuotes(optionID) + "";
            }

            if (debug == true)
            {
                sendToFile(optionQuery + "\r\n", @"C:\SQL_Debug.txt");
            }

            SLX_Data.executeSQL(optionQuery, conn);
        }

        public static void sendToLog(string value)
        {            
            if (value.Trim() != "")            
            {
                sendToFile(value + "\r\n", @"C:\SQL_Debug.txt");
            }         
        }

        public static void saveDSToXMLFile(DataSet ds, string xmlFilePath)
        {            
            ds.WriteXml(xmlFilePath);            
        }

        public static void sendToFile(string value, string path)
        {            
            StreamWriter sw;

            if (File.Exists(path))
            {
                sw = File.AppendText(path);
                sw.WriteLine(value);
                sw.Close();
            }
            else
            {                
                sw = File.CreateText(path);
                sw.WriteLine(value);
                sw.Close();
            }
        }

        public static OleDbDataReader fillReader(string query, OleDbConnection connection)
        {
            OleDbCommand selectCommand;
            OleDbDataReader dr = null;

            selectCommand = new OleDbCommand(query, connection);

            dr = selectCommand.ExecuteReader();           

            return dr;
        }

        public static DataSet fillDataSet(string query, string table, OleDbConnection connection, OleDbDataAdapter da, DataSet ds)
        {
            da.SelectCommand = new OleDbCommand(query, connection);            

            da.Fill(ds, table);

            return ds;
        }

        public static OleDbCommandBuilder buildSQL(OleDbDataAdapter da)
        {
            /**
             *  CommandBuilder does not create columns for statements if
             *  using "ISNULL(SOMEFIELD, '') AS SOMEFIELD"
             */
            OleDbCommandBuilder cb = null;

            try
            {
                
                //Make sure we have a data adapter passed in
                if (da != null)
                {
                    //Make sure the data adapter has already had a select statement added to it
                    if (da.SelectCommand != null)
                    {
                        cb = new OleDbCommandBuilder(da);

                        da.InsertCommand = cb.GetInsertCommand();
                        da.UpdateCommand = cb.GetUpdateCommand();
                        da.DeleteCommand = cb.GetDeleteCommand();
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (cb != null)
            {
                return cb;
            }
            else
            {
                return null;
            }
        }
    }
}
