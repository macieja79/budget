using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;

namespace Budget.Core.Logic
{
    public class TransactionComparer_obsoletete : IComparer<Transaction>
    {

        List<Tuple<Category, SubCategory>> _list = new List<Tuple<Category, SubCategory>>();

        public TransactionComparer_obsoletete(CategoryCollection categoryCollection)
        {
            foreach (var category in categoryCollection.Categories)
                foreach (var subCategory in category.SubCategories)
                    _list.Add(new Tuple<Category, SubCategory>(category, subCategory));
        }

        public int Compare(Transaction x, Transaction y)
        {

            bool isXnull = x.Category.IsNullObj() || x.SubCategory.IsNullObj();
            bool isYnull = y.Category.IsNullObj() || y.SubCategory.IsNullObj();

            if (isXnull || isYnull)
                return isXnull.CompareTo(isYnull);

            var xItem = _list.First(i => i.Item1.Name == x.Category && i.Item2.Name == x.SubCategory);
            int xPosition = _list.IndexOf(xItem);

            var yItem = _list.First(i => i.Item1.Name == y.Category && i.Item2.Name == y.SubCategory);
            int yPosition = _list.IndexOf(yItem);

            int compareResult = xPosition.CompareTo(yPosition);
            if (compareResult != 0) return compareResult;

            int nameCompareResult = x.CounterPartData.CompareTo(y.CounterPartData);
            if (nameCompareResult != 0) return nameCompareResult;

            int valueCompareResult = x.Amount.CompareTo(y.Amount);
            return valueCompareResult;
            


        }
    }
}
