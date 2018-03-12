using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Database
{
    public class ServerInfo : IDatabaseTreeElement
    {

        public ServerInfo()
        {
            Databases = new List<DatabaseInfo>();
        }

        public string Name { get; set; }
        public List<DatabaseInfo> Databases { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public List<IDatabaseTreeElement> Children
        {
            get
            {
                List<IDatabaseTreeElement> chilren = new List<IDatabaseTreeElement>();
                Databases.ForEach(d => chilren.Add(d as IDatabaseTreeElement));
                return chilren;
            }

        }
    }
}
