using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Collections;
using Metaproject.Patterns;
using Newtonsoft.Json;

namespace Budget.Core.Entities
{
    public class Category : Entity, ITreeViewItem 
    {
      

        public static Category Create(string name, params string[] subcategories)
        {
     
            Category category = new Category()
            {
                Name = name,
                Id = Guid.NewGuid(),
                SubCategories = new List<SubCategory>()
            };

            for (int i = 0; i < subcategories.Length; i++)
            {
                string iName = (string)subcategories[i];

                SubCategory subCategory = new SubCategory()
                {
                    Name = iName,
                    Id = Guid.NewGuid(),
                    
                };

                category.SubCategories.Add(subCategory);
            }

            return category;
        }

        public Category()
        {
            SubCategories = new List<SubCategory>();
        }

        public string Name { get; set; }
        public string Color { get; set; }
        public List<SubCategory> SubCategories { get; set; }

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
                if (SubCategories.IsNullObj()) return null;
                return SubCategories.Cast<ITreeViewItem>().ToList(); 
            }
        }

     
        public void RemoveTreeItem(ITreeViewItem item)
        {
            SubCategories.RemoveWhere(s => s == item);
        }

        public void AddItem(ITreeViewItem item)
        {
            SubCategory category = item as SubCategory;
            if (category.IsNullObj()) return;
            SubCategories.Add(category);
        }

        public void AddItem(int index, ITreeViewItem item)
        {
            SubCategory category = item as SubCategory;
            if (category.IsNullObj()) return;
            SubCategories.Insert(index, category);
        }

        #endregion
    }


}
