using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Text;

namespace com.lexnetcg.saleslogix.data
{
    public enum SlxOleDbExtendedProperties
    {
        PORT,
        RWPASS,
        LOG,
        CASEINSENSITIVEFIND
    }

    public class SLXConnectionString : DbConnectionStringBuilder
    {       
        private string _User;
        private string _Password;
        private string _ServerName;
        private string _Catalog;
        private bool _EnableLogging;
        private StringCollection _ExtendedProperties;

        public string User
        {
            get { return _User; }
            set { _User = value; }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }

        /// <summary>
        /// Catalog is found in the connection manager and is similar to the 
        /// database name. May or may not be the same as the database name, but is the same by default
        /// </summary>
        public string Catalog
        {
            get { return _Catalog; }
            set { _Catalog = value; }
        }


        public bool EnableLogging
        {
            get { return _EnableLogging; }
            set { _EnableLogging = value; }
        }

        public SLXConnectionString()
        {
            _EnableLogging = true;            
        }

        /// <summary>
        /// Override to include bool and int for the value parameter
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void AddExtendedProperty(SlxOleDbExtendedProperties property, string value)
        {
            
        }

        public string Get()
        {
            BuildConnectionString();

            return ConnectionString;
        }

        private void BuildConnectionString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Provider=SLXOLEDB.1;");
            sb.Append("User ID=" + _User + ";");
            sb.Append("Password=" + _Password + ";");
            sb.Append("Persist Security Info=True;");
            sb.Append("Initial Catalog=" + _Catalog + ";");
            sb.Append("Data Source=" + _ServerName + ";");

            if(_ExtendedProperties.Count > 0)
            {
                sb.Append("Extended Properties=\"");

                for(int i = 0; i < _ExtendedProperties.Count; i++)
                {

                }

                sb.Append("\";");
            }

            ConnectionString = sb.ToString();
        }

        public void Parse(String connectionString)
        {
            if (String.IsNullOrEmpty(connectionString) == true)
            {
                return;
            }

            string[] keyValuePairs = connectionString.Split(new String[] { ";" }, StringSplitOptions.None);            

            for (int i = 0; i < keyValuePairs.Length; i++)
            {
                if (keyValuePairs[i].Contains("=") == true)
                {
                    string[] keyValuePair = keyValuePairs[i].Split(new String[] { "=" }, StringSplitOptions.None);

                    string key = keyValuePair[0];
                    string value = keyValuePair[1];

                    if (key.Equals("Initial Catalog", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        this._Catalog = value;
                    }
                    else if (key.Equals("Password", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        this._Password = value;
                    }

                    else if (key.Equals("User ID", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        this._User = value;
                    }
                    else if (key.Equals("Data Source", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        this._ServerName = value;
                    }
                }
            }
        }

        //Provider=SLXOLEDB.1;Password=password;Persist Security Info=True;User ID=admin;Initial Catalog=SLX_DYNALINK;Data Source=DEV-SLX62;Extended Properties="PORT=1706;RWPASS=TEST;LOG=ON;CASEINSENSITIVEFIND=ON"
    }
}