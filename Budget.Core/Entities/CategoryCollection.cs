using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Budget.Core.Actions;
using Metaproject.Collections;
using Metaproject.Patterns;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

namespace Budget.Core.Entities
{
    public class CategoryCollection : ITreeViewItem
    {

        public CategoryCollection()
        {
            Categories = new List<Category>();    
        }

        public Category this[int index] => Categories[index];


        public List<Category> Categories { get; set; }

        public SubCategory GetSubCategory(string categoryName, string subCategoryName)
        {

            Category category = Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category.IsNullObj())
            {
                category = new Category() {Name = categoryName};
                Categories.Add(category);
            }

            SubCategory sub = category.SubCategories.FirstOrDefault(s => s.Name == subCategoryName);
            if (sub.IsNullObj())
            {
                sub = new SubCategory() {Name = subCategoryName};
                category.SubCategories.Add(sub);
            }

            return sub;
        }

        public void AttachCategoryName(Transaction transaction, bool canOverride)
        {
            if (transaction.IsNullObj()) return;


            foreach (Category category in Categories)
            {
                foreach (SubCategory sub in category.SubCategories)
                {
                    foreach (Rule rule in sub.Rules)
                    {
                        bool isApplied = rule.CheckIfMatch(transaction);
                        bool isNotDescribed = transaction.Category.IsNull() && transaction.SubCategory.IsNull();
                        if (isApplied)
                        {
                            bool isToApply = isNotDescribed || (canOverride);
                            if (isToApply)
                            {
                                transaction.Category = category.Name;
                                transaction.SubCategory = sub.Name;
                                transaction.Color = category.Color;
                                break;
                            }

                        }

                    }

                }
            }

        }

        #region ITreeNode

        [JsonIgnore]
        public string TreeDisplayName
        {
            get { return "KATEGORIE"; }
            set { }
        }
        

        [JsonIgnore]
        public IList<ITreeViewItem> Children => Categories.Cast<ITreeViewItem>().ToList();

        
        public void RemoveTreeItem(ITreeViewItem item)
        {
            Categories.RemoveWhere(s => s == item);
        }

        public void AddItem(ITreeViewItem item)
        {
            Category category = item as Category;
            if (category.IsNullObj()) return;
            Categories.Add(category);
        }

        public void AddItem(int index, ITreeViewItem item)
        {
            Category category = item as Category;
            if (category.IsNullObj()) return;
            Categories.Insert(index, category);
        }

        #endregion

        public Category GetCategoryByName(string name)
        {
            return Categories.FirstOrDefault(c => c.Name == name);
        }

        public SubCategory GetSubcategoryByNames(string categoryName, string subCategoryName)
        {
            Category category = GetCategoryByName(categoryName);
            if (category.IsNullObj()) return null;

            return category.SubCategories.FirstOrDefault(s => s.Name == subCategoryName);
        }

        public List<string> GetSubCategoryNames(string categoryName)
        {
            Category category = GetCategoryByName(categoryName);
            if (category.IsNullObj()) return new List<string>();
            return category.SubCategories.Select(s => s.Name).ToList();
        }
    }
}
