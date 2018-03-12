using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Files;

namespace Metaproject.Database
{
    public class BCP
    {

        public class ResultScripts
        {
            public string ExportScript { get; set; }
            public string ImportScript { get; set; }
            
        }


        public static ResultScripts GenerateExportScript(DatabaseInfo database, string sourceServerName,
            string destinationServerName, string destinationDatabaseName)
        {


            StringBuilder sbExport = new StringBuilder();
            foreach (TableInfo table in database.Tables)
            {
                string tablefullName = $"{database.Name}.{table.Name}";
                string destinationFilePath = $@"c:\Temp\{table.Name}.bcp";

                string bcpPattern = $"bcp {tablefullName} out {destinationFilePath} -n -T -S {sourceServerName}";
                sbExport.AppendLine(bcpPattern);
            }


            StringBuilder sbImport = new StringBuilder();
            foreach (TableInfo table in database.Tables)
            {
                string tablefullName = $"{destinationDatabaseName}.{table.Name}";
                string destinationFilePath = $@"c:\Temp\{table.Name}.bcp";

                string bcpPattern = $"bcp {tablefullName} in {destinationFilePath} -n -T -S {destinationServerName}";
                sbImport.AppendLine(bcpPattern);
            }

            ResultScripts result = new ResultScripts()
            {
                ExportScript = sbExport.ToString(),
                ImportScript = sbImport.ToString(),
            };

            return result;

        }


    }
}
