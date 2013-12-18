using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

using com.lexnetcg.saleslogix.sql;
using com.lexnetcg.saleslogix.utils;

namespace com.lexnetcg.saleslogix.data
{
    public class SLX_Data : SLX
    {
        //private OleDbConnection conn;

        //private void connect()
        //{
        //}        

        public static void executeSQL(string query, OleDbConnection conn)
        {
            /**
             * Used to run insert, update and delete type 
             * SQL statements against the database. Use other 
             * methods for retrieving datasets and datareaders, etc             
             **/

            OleDbCommand queryCommand;

            queryCommand = new OleDbCommand();

            queryCommand.Connection = conn;
            queryCommand.CommandType = CommandType.Text;
            queryCommand.CommandText = query;

            queryCommand.ExecuteNonQuery();
        }

        public static string addQuotes(string value)
        {
            string returnValue;

            if (value == null)
            {
                returnValue = null;
            }
            else
            {
                returnValue = "'" + value + "'";
            }

            return returnValue;
        }

        public static string addQuotes(DateTime value)
        {
            string returnValue;

            if (value.ToString().Trim() == "")
            {
                returnValue = null;
            }
            else
            {
                returnValue = "'" + value + "'";
            }

            return returnValue;
        }

        public static string fixSQLSingleQuote(string value)
        {
            string returnValue;

            if(value.Contains("'"))
            {
                returnValue = value.Replace("'", "''");
            }
            else
            {
                returnValue = value;
            }

            return returnValue;
        }

        public static string blankStringToNull(string value)
        {
            string returnValue;

            if (value == "''")
            {
                returnValue = "null";
            }
            else
            {
                returnValue = value;
            }

            return returnValue;
        }

        public static string getDataValue(string table, string returnField, string lookupField, string lookupValue, OleDbConnection conn)
        {
            string returnValue = "";
            
            SLX_SQL sql = new SLX_SQL();

            sql.ConnectionString = conn.ConnectionString;

            sql.Table = table;
            sql.ReturnField = returnField;
            sql.LookupField = lookupField;
            sql.LookupValue = lookupValue;

            returnValue = sql.getDataValue();

            return returnValue;
        }

        public static string getDataValueSQL(string query, OleDbConnection conn)
        {           
            string returnValue = "";
            SLX_SQL sql = new SLX_SQL();

            sql.ConnectionString = conn.ConnectionString;
            sql.Sql = query;

            returnValue = sql.getDataValueSQL();

            return returnValue;
        }

        public static int countRecords(string query, OleDbConnection conn)
        {
            int returnValue = 0;
            SLX_SQL sql = new SLX_SQL();

            sql.ConnectionString = conn.ConnectionString;
            sql.Sql = query;

            returnValue = sql.countRecords();
            
            return returnValue;
        }

        private void updateDataValue(string table, string field, string value, string pKeyField, string pKeyValue, string userID, bool debug)
        {
            string updateQuery;
            SLX_SQL sql = new SLX_SQL();

            updateQuery = "UPDATE " + table + " SET " +
                                "MODIFYUSER = " + addQuotes(userID) + ", " +
                                "MODIFYDATE = " + addQuotes(dateTimeToISODateString(DateTime.Now)) + ", " +
                                field + " = " + blankStringToNull(addQuotes(value)) + " " +
                            "WHERE " +
                                pKeyField + " = " + addQuotes(pKeyValue) + "";            

            sql.Sql = updateQuery;
            sql.executeSQL();         
        }

        public static string fixDate(DateTime date)
        {
            string returnValue;

            if (date.Year == 1899 && date.Month == 12 && date.Day == 31)
            {
                returnValue = "";
            }
            else
            {
                returnValue = date.ToString();
            }

            return returnValue;
        }

        //public static string newSLXID(string table, OleDbConnection conn)
        //{
        //    string newID = "";

        //    OleDbCommand command = new OleDbCommand("slx_dbids('" + table + "', 1)", conn);

        //    newID = (string)command.ExecuteScalar();

        //    return newID;
        //}

        public static string newSLXID(string table, OleDbConnection conn)
        {
            string[] ids = newSLXIDs(table, 1, conn);

            return ids[0];
        }

        public static string[] newSLXIDs(string table, int numberOfIds, OleDbConnection conn)
        {
            bool closeConnectionWhenFinished = false;

            if(conn.State == ConnectionState.Closed)
            {
                conn.Open();

                closeConnectionWhenFinished = true;
            }

            OleDbCommand command = new OleDbCommand("slx_dbids('" + table + "', " + numberOfIds + ")", conn);

            OleDbDataReader reader = command.ExecuteReader();

            string[] ids = new string[numberOfIds];
            int i = 0;

            while (reader.Read())
            {
                ids[i] = reader[0].ToString();

                i++;
            }

            return ids;

            //reader.GetValues(ids);

            if(closeConnectionWhenFinished == true)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return ids;
        }

        public static string getNewTimestamp()
        {
            DateTime dt = DateTime.Now;

            return dateTimeToISODateString(dt);
        }        

        public static string dateTimeToISODateString(DateTime value)
        {
            string returnValue;

            returnValue = value.Year.ToString();
            returnValue += "-";
            returnValue += value.Month.ToString().PadLeft(2, '0');
            returnValue += "-";
            returnValue += value.Day.ToString().PadLeft(2, '0');
            returnValue += " ";
            returnValue += value.Hour.ToString().PadLeft(2, '0');
            returnValue += ":";
            returnValue += value.Minute.ToString().PadLeft(2, '0');
            returnValue += ":";
            returnValue += value.Second.ToString().PadLeft(2, '0');

            return returnValue;
        }

        public static void saveDSToXMLFile(DataSet ds, string xmlFilePath)
        {            
            ds.WriteXml(xmlFilePath);
        }

        public static DataSet fillDataSet(string query, string table, OleDbConnection connection, OleDbDataAdapter da, DataSet ds, bool debug)
        {
            da.SelectCommand = new OleDbCommand(query, connection);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);

            if (debug == true)
            {
                SLX_Utils.sendToLog("Insert Statement: " + cb.GetInsertCommand().CommandText);
                SLX_Utils.sendToLog("Update Statement: " + cb.GetUpdateCommand().CommandText);
                SLX_Utils.sendToLog("Delete Statement: " + cb.GetDeleteCommand().CommandText);
            }

            da.InsertCommand = cb.GetInsertCommand();
            da.UpdateCommand = cb.GetUpdateCommand();
            da.DeleteCommand = cb.GetDeleteCommand();

            da.Fill(ds, table);

            return ds;
        }

        public static void updateDataSet(DataSet ds, OleDbDataAdapter da, string table, bool debug)
        {
            if (ds.HasChanges())
            {
                try
                {
                    if (ds.HasErrors)
                    {
                        SLX_Utils.sendToLog("DataSet has errors");
                    }

                    da.Update(ds.Tables[table]);
                    ds.AcceptChanges();

                    if (debug == true)
                    {
                        SLX_Utils.sendToLog("DataSet should have been updated");
                        saveDSToXMLFile(ds, @"C:\Debug.xml");
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

    }
}
