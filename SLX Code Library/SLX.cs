using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;

namespace com.lexnetcg.saleslogix
{
    public class SLX
    {
        public string getNativeConnectionString(OleDbConnection slxConn, string sysdbaPW)
        {
            /**
             *  Assumes masterkey for sysdba
             *  If it has been changed then pass in the value
             *  for the sysdba password
             */
            string nativeConnectionString = "";
            int start;
            int end;

            if (sysdbaPW == null || sysdbaPW.Trim() == "")
            {
                sysdbaPW = "masterkey";
            }

            OleDbCommand command = new OleDbCommand("slx_getNativeConnInfo", slxConn);

            nativeConnectionString = (string)command.ExecuteScalar();

            start = nativeConnectionString.IndexOf("SQLOLEDB.1;") + 11; //11 is the length of "SQLOLEDB.1;" without the quotes
            end = nativeConnectionString.IndexOf(";", start);

            nativeConnectionString = nativeConnectionString.Replace(nativeConnectionString.Substring(start, end - start), "Password=" + sysdbaPW);

            return nativeConnectionString;
        }           
    }


}
