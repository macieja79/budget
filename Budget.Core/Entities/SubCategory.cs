using System;
using System.Collections.Generic;
using Metaproject.Collections;
using Newtonsoft.Json;

namespace Budget.Core.Entities
{
    public class SubCategory : Entity , ITreeViewItem
    {
        public SubCategory()
        {
            Rules = new List<Rule>();
        }

        public string Name { get; set; }
        public List<Rule> Rules { get; set; }

        public override string ToString()
        {
            return Name;
        }

        #region ITreeViewItem
        [JsonIgnore]
        public string TreeDisplayName
        {
            get { return Name; }
            set { Name = value; }
        }
        [JsonIgnore]
        public IList<ITreeViewItem> Children
        {
            get
            {
                return null;
                
            }

        }

        public void RemoveTreeItem(ITreeViewItem item)
        {
            throw new System.NotImplementedException();
        }

        public void AddItem(ITreeViewItem item)
        {
            throw new System.NotImplementedException();
        }

        public void AddItem(int index, ITreeViewItem item)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}