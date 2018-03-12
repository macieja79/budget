using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Patterns;
using Metaproject.Windows;

namespace Metaproject.Database
{
    public class CopyDatabaseAction : IAction<CopyDatabaseAction.Data, CopyDatabaseAction.Result>
    {

        public class Data : ActionData
        {
         
            public DatabaseInfo SourceDatabase { get; set; }
            public string SourceServer { get; set; }
            public string DestinationServer { get; set; }
            public string DestinationDatabase { get; set; }
            public List<string> SelectedSourceTables { get; set; }

            public IDatabaseHelper DatabaseHelper { get; set; }
            public ICmd Cmd { get; set; }

        }

        public class Result : ActionResult
        {
            

        }

        public Result Execute(Data data)
        {

            BCP.ResultScripts scripts = BCP.GenerateExportScript(data.SourceDatabase, data.SourceServer,
                data.DestinationServer, data.DestinationDatabase);
            
            SqlScipt createTablesScript = data.DatabaseHelper.GetCreateTableScripts(data.SourceDatabase);
            
            data.Cmd.ExecuteScript(scripts.ExportScript);

            data.DatabaseHelper.CreateDatabase(data.DestinationServer, data.DestinationDatabase);

            data.DatabaseHelper.ExecuteScript(data.DestinationServer, data.DestinationDatabase, createTablesScript);

            data.Cmd.ExecuteScript(scripts.ImportScript);
            
            return new Result();


        }
    }
}
