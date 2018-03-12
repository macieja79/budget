using System.Collections.Generic;

namespace Metaproject.Database
{
    public class DatabaseInfo : IDatabaseTreeElement
    {
        public DatabaseInfo()
        {
            Tables = new List<TableInfo>();
        }

        public string Name { get; set; }
        public string ConnectionString { get; set; }

        public List<TableInfo> Tables { get; set; }


        public override string ToString()
        {
            return Name;
        }


        public List<IDatabaseTreeElement> Children
        {
            get
            {
                List<IDatabaseTreeElement> chilren = new List<IDatabaseTreeElement>();
                Tables.ForEach(t => chilren.Add(t as IDatabaseTreeElement));
                return chilren;
            }
        }
        
    }
}