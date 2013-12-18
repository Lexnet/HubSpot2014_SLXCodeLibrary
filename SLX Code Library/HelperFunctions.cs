using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;


namespace com.lexnetcg.saleslogix
{
    public class HelperFunctions : SLX
    {
        public static void SendToFile(string dataString, string path)
        {
            if (File.Exists(path) == false)
            {
                File.Create(path);
            }

            StreamWriter sw = File.AppendText(path);
            sw.WriteLine(dataString);
            sw.Close();
            sw.Dispose();
        }

        public static string AddQuotes(string value)
        {
            return "'" + value + "'";
        }

        public static void FillSQLDataSet(string query, SqlConnection conn, DataSet ds)
        {
            if (ds != null)
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                da.Fill(ds);
            }
        }

        public static string GetID()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public static void CreateDirectory(string @path, string directory)
        {
            string directoryPath = path;

            if (IsValidPath(path) == false)
            {
                directoryPath += "\\";
            }

            directoryPath += directory;

            if (Directory.Exists(path))
            {
                if (Directory.Exists(path + "\\" + directory))
                {

                }
                else
                {
                    Directory.CreateDirectory(path + "\\" + directory);
                }
            }
            else
            {

            }
        }

        public static string ReplaceInvalidFolderCharacters(string folderName)
        {
            string returnValue;
            string[] invalidCharacters = new string[10];

            invalidCharacters[0] = "\\";
            invalidCharacters[1] = "/";
            invalidCharacters[2] = ":";
            invalidCharacters[3] = "*";
            invalidCharacters[4] = "?";
            invalidCharacters[5] = "<";
            invalidCharacters[6] = ">";
            invalidCharacters[7] = "|";
            invalidCharacters[8] = "\"";
            invalidCharacters[9] = ",";

            returnValue = folderName;

            for (int i = 0; i < invalidCharacters.Length; i++)
            {
                if (returnValue.Contains(invalidCharacters[i]))
                {
                    returnValue = returnValue.Replace(invalidCharacters[i], "");
                }
            }

            return returnValue;
        }

        public static string MakeUSAlphaNumeric(string value)
        {
            char letter;

            for (int i = 0; i < value.Length; i++)
            {
                //letter = value.ToCharArray(i + 1, 1);


            }

            return null;
        }

        public static bool IsValidPath(string @path)
        {
            if (path.EndsWith("\\"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ValidateDirectoryPath(string path)
        {
            if (HelperFunctions.IsValidPath(path) == true)
            {
                return path;
            }
            else
            {
                return path + "\\";
            }
        }

        public static string CalculatefileSize(long sizeInBytes)
        {
            string returnValue;

            if (sizeInBytes < 1024568)
            {
                returnValue = Convert.ToString(sizeInBytes / 1024) + " Kb";
            }
            else
            {
                if (sizeInBytes < 1024568000)
                {
                    returnValue = Convert.ToString((sizeInBytes / 1024568) + "." + (sizeInBytes % 1024568)) + " Mb";
                }
                else
                {
                    returnValue = Convert.ToString(sizeInBytes / 1024568000) + " Gb";
                }
            }

            return returnValue;
        }

        public static bool QSKeyExists(System.Web.HttpRequest request, string keyValue)
        {
            bool returnValue = false;

            for (int i = 0; i < request.QueryString.Keys.Count; i++)
            {
                if (request.QueryString.GetKey(i).ToString().Trim().ToUpper() == keyValue.Trim().ToUpper())
                {
                    returnValue = true;
                    break;
                }
            }

            return returnValue;
        }

        #region Boolean Conversions

        public static string ConvertBoolean(bool value)
        {
            string returnValue;

            if (value == true)
            {
                returnValue = "T";
            }
            else
            {
                returnValue = "F";
            }

            return returnValue;
        }

        public static string ConvertBoolean(string value)
        {
            string returnValue;

            if (value.Trim().ToUpper() == "TRUE" || value.Trim().ToUpper() == "T" || value.Trim().ToUpper() == "YES" || value.Trim().ToUpper() == "Y")
            {
                returnValue = "T";
            }
            else
            {
                returnValue = "F";
            }

            return returnValue;
        }

        public static string ConvertBoolean(int value)
        {
            string returnValue;

            if (value == 1)
            {
                returnValue = "T";
            }
            else
            {
                returnValue = "F";
            }

            return returnValue;
        }

        #endregion
    }
}
