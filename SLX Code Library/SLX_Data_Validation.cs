using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;
using System.Text;

using com.lexnetcg.saleslogix;

namespace com.lexnetcg.saleslogix.data
{
    public class SLX_Data_Validation : SLX 
    {

        #region SLX Booleans

        public static string formatSLXBoolean(string value)
        {
            string returnValue;

            switch (value.Trim().ToUpper())
            {
                case "T":
                    returnValue = "T";
                    break;
                case "TRUE":
                    returnValue = "T";
                    break;
                case "Y":
                    returnValue = "T";
                    break;
                case "YES":
                    returnValue = "T";
                    break;
                case "F":
                    returnValue = "F";
                    break;
                case "FALSE":
                    returnValue = "F";
                    break;
                case "N":
                    returnValue = "F";
                    break;
                case "NO":
                    returnValue = "F";
                    break;
                default:
                    returnValue = "";
                    break;
            }

            return returnValue;
        }

        public static string formatSLXBoolean(int value)
        {
            string returnValue;

            switch (value)
            {
                case 1:
                    returnValue = "T";
                    break;              
                case 0:
                    returnValue = "F";
                    break;
                default:
                    returnValue = "";
                    break;
            }

            return returnValue;
        }

        public static string formatSLXBoolean(bool value)
        {
            string returnValue;

            switch (value)
            {
                case true:
                    returnValue = "T";
                    break;
                case false:
                    returnValue = "F";
                    break;
                default:
                    returnValue = "";
                    break;
            }

            return returnValue;
        }

        #endregion
    }    
}
