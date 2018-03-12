using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;

namespace Budget.Core.Entities
{
    public class TransactionComparer : IComparer<Transaction>
    {
        private readonly CategoryCollection _categories;

        public TransactionComparer(CategoryCollection categories)
        {
            _categories = categories;
        }


        public int Compare(Transaction x, Transaction y)
        {

            bool isXnull = x.Category.IsNullObj() || x.SubCategory.IsNullObj();
            bool isYnull = y.Category.IsNullObj() || y.SubCategory.IsNullObj();

            if (isXnull || isYnull)
                return isXnull.CompareTo(isYnull);

            int xIndex = GetCategoryIndex(x.Category);
            int yIndex = GetCategoryIndex(y.Category);

            if (xIndex == yIndex)
            {
                Category category = _categories[xIndex];
                int xSubCategoryIndex = GetSubCategoryIndex(category.SubCategories, x.SubCategory);
                int ySubCategoryIndex = GetSubCategoryIndex(category.SubCategories, y.SubCategory);

                if (xSubCategoryIndex == ySubCategoryIndex)
                {
                    DateTime xDate = x.TransactionDate;
                    DateTime yDate = y.TransactionDate;
                    return xDate.CompareTo(yDate);
                }
                else
                {
                    return xSubCategoryIndex.CompareTo(ySubCategoryIndex);
                }
            }
            else
            {
                return xIndex.CompareTo(yIndex);
            }

        }

        int GetCategoryIndex(string category)
        {

            foreach (var c in _categories.Categories)
            {
                if (c.Name == category)
                    return _categories.Categories.IndexOf(c);
            }

            return -1;
        }

        int GetSubCategoryIndex(List<SubCategory> subCategories, string subCategoryName)
        {
            foreach (var s in subCategories)
            {
                if (s.Name == subCategoryName)
                    return subCategories.IndexOf(s);
            }

            return -1;

        }
    }
}
