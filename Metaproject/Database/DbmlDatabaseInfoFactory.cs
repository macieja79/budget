using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Metaproject.Database
{
    public class DbmlDatabaseInfoFactory : IDatabaseInfoFactory
    {

        public enum DataType
        {
            Path,
            Content
        }

        private readonly DataType _dataType;
        private readonly string _dbmlFilePath;

        public DbmlDatabaseInfoFactory(DataType dataType,  string dbmlFilePath)
        {
            _dataType = dataType;
            _dbmlFilePath = dbmlFilePath;
        }

        public DatabaseInfo CreateDatabaseInfo()
        {
            XDocument xDoc = null;
            if (_dataType == DataType.Path)
            {
                xDoc = XDocument.Load(_dbmlFilePath);
            }
            else if (_dataType == DataType.Content)
            {
                xDoc = XDocument.Parse(_dbmlFilePath);
            }

            var namespaceManager = new XmlNamespaceManager(new NameTable()); // We now have a namespace manager that knows of the namespaces used in your document.
            namespaceManager.AddNamespace("x", "http://schemas.microsoft.com/linqtosql/dbml/2007"); // We add an explicit prefix mapping for our query.

            var connectionElement = xDoc.XPathSelectElement("//x:Database/x:Connection", namespaceManager);
            string connectionString = string.Empty;
            if (connectionString.IsNotNull())
                connectionString = connectionElement.Attribute("ConnectionString").Value;


            var tableElements = xDoc.XPathSelectElements("//x:Database/x:Table", namespaceManager);

            List<string> tableNames = tableElements.Select(e => e.Attribute("Name").Value).ToList();

            DatabaseInfo databaseInfo = new DatabaseInfo()
            {
                ConnectionString = connectionString,
                Tables = tableNames.Select(name => new TableInfo() { Name = name}).ToList()
            };

            return databaseInfo;
        }
    }
}
