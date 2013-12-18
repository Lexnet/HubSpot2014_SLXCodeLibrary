using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

using com.lexnetcg.saleslogix.data;

namespace com.lexnetcg.saleslogix.sql
{
    public class SLX_SQL : SLX
    {
        private string sql;
        private string table;
        private string pKeyField;
        private string pKeyValue;
        private string fKeyField;
        private string fKeyValue;
        private string returnField;
        private string lookupField;
        private string lookupValue;
        private int recordCount;
        private bool isConnected;
        private bool useUDLFile;        
        private string UdlFile;
        private string connectionString;
        private OleDbConnection conn;

        //Commented out becuase these will be tied to method return values
        //Need to use overridden methods in order to accomodate different data types
        //private string lookupValue;
        //private string returnField;

        public SLX_SQL()
        {
            isConnected = false;
            useUDLFile = false;            
            recordCount = 0;
        }

        public string Sql
        {
            set { sql = value; }
            get { return this.sql; }
        }

        public string Table
        {
            set { table = value; }
            get { return this.table; }
        }

        public string PKeyField
        {
            set { pKeyField = value; }
            get { return this.pKeyField; }
        }

        public string PKeyValue
        {
            set { pKeyValue = value; }
            get { return this.pKeyValue; }
        }

        public string FKeyField
        {
            set { fKeyField = value; }
            get { return this.fKeyField; }
        }

        public string FKeyValue
        {
            set { fKeyValue = value; }
            get { return this.fKeyValue; }
        }

        public string ReturnField
        {
            set { returnField = value; }
            get { return this.returnField; }
        }

        public string LookupField
        {
            set { lookupField = value; }
            get { return this.lookupField; }
        }

        public string LookupValue
        {
            set { lookupValue = value; }
            get { return this.lookupValue; }
        }

        public int RecordCount
        {         
            get { return this.recordCount; }
        }

        public bool UseUDLFile
        {
            set { useUDLFile = value; }
        }

        public string UDLFile
        {
            set { UdlFile = value; }
            get { return this.UdlFile; }
        }

        public string ConnectionString
        {
            set { connectionString = value; }
            get { return this.connectionString; }
        }

        private void connect()
        {
            try
            {
                if (useUDLFile == true)
                {
                    conn = new OleDbConnection("File Name = " + UDLFile);
                }
                else
                {
                    conn = new OleDbConnection(connectionString);
                }

                conn.Open();

                isConnected = true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void close()
        {
            conn.Close();
            conn.Dispose();
        }

        public int countRecords()
        {
            int returnValue = 0;            

            try
            {
                //Connect to database
                connect();

                if (isConnected == true)
                {
                    returnValue = Convert.ToInt32(runSQL());

                    //Close connection
                    close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return returnValue;
        }

        public string getDataValueSQL()
        {
            string returnValue = "";            

            try
            {
                //Connect to database
                //connect();

                //if (isConnected == true)
                //{
                    returnValue = runSQL();

                    //Close connection
                    //close();
                //}
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return returnValue;
        }

        public string getDataValue()
        {
            string returnValue = "";                        

            try
            {
                this.sql = "SELECT " +
                            this.returnField + " as field " +
                        "FROM " +
                            table + " " +
                        "WHERE " +
                            this.lookupField + " = " + SLX_Data.addQuotes(this.lookupValue);

                returnValue = runSQL();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Console.WriteLine("Value = " + returnValue + ":  " + sql);
            return returnValue;
        }

        /**
         *  Runs the SQL command
         */
        private string runSQL()
        {
            string returnValue = "";            
            OleDbCommand command;
            OleDbDataReader reader;

            try
            {
                connect();

                if (isConnected == true)
                {
                    command = new OleDbCommand(this.sql, this.conn);
                    command.CommandType = CommandType.Text;

                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            returnValue = reader.GetString(0);
                        }
                    }
                    else
                    {
                        returnValue = "";
                    }

                    close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Console.WriteLine("Value = " + returnValue + ":  " + sql);
            return returnValue;
        }

        public void executeSQL()
        {
            /**
             * Used to run insert, update and delete type 
             * SQL statements against the database. Use other 
             * methods for retrieving datasets and datareaders, etc             
             **/

            OleDbCommand queryCommand;

            try
            {
                connect();

                if (isConnected == true)
                {
                    queryCommand = new OleDbCommand();
                    queryCommand.Connection = this.conn;
                    queryCommand.CommandType = CommandType.Text;
                    queryCommand.CommandText = this.sql;
                    queryCommand.ExecuteNonQuery();

                    close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
