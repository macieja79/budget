using System.Collections.Generic;

namespace Metaproject.Database
{
    public class TableInfo : IDatabaseTreeElement
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
        
        public List<IDatabaseTreeElement> Children
        {
            get { return new List<IDatabaseTreeElement>(); }
        }


    }
}