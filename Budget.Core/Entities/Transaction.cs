using Metaproject.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Entities
{

    

    public class Transaction
    {

        public static string[] GetTransactionHeaders()
        {

            Transaction instance = new Transaction();
            string[] collection =
            {
                ReflectionTools.GetName(() => instance.TransactionDate),
                ReflectionTools.GetName(() => instance.Owner),
                ReflectionTools.GetName(() => instance.Category),
                ReflectionTools.GetName(() => instance.SubCategory),
                ReflectionTools.GetName(() => instance.Amount),
                ReflectionTools.GetName(() => instance.CounterPartData),
                ReflectionTools.GetName(() => instance.Title),
                ReflectionTools.GetName(() => instance.Details),
                ReflectionTools.GetName(() => instance.AccountNumber),
                ReflectionTools.GetName(() => instance.TransactionNumber),
                ReflectionTools.GetName(() => instance.Comment),
            };
            
            return collection;
        }

        public static int GetIndex(Expression<Func<object>> exp)
        {
            string propertyName = ReflectionTools.GetName(exp);
            var headers = GetTransactionHeaders().ToList();
            return headers.IndexOf(propertyName);
        }
        
        public string TransactionType
        {
            get
            {
                if (Amount > 0)
                    return BudgetConst.TransactionIncome;
                else if (Amount < 0)
                    return BudgetConst.TransactionCost;
                return "";
            }
        }

        [DisplayName(@"Data")]
        public DateTime TransactionDate { get; set; }
        [DisplayName(@"Kontrahent")]
        public string CounterPartData { get; set; }
        [DisplayName(@"Tytul")]
        public string Title { get; set; }
        [DisplayName(@"Szczegoly")]
        public string Details { get; set; }
        [DisplayName(@"Nr transakcji")]
        public string TransactionNumber { get; set; }
        [DisplayName(@"Kwota")]
        public decimal Amount { get; set; }
        [DisplayName(@"Waluta")]
        public string Currency { get; set; }
        [DisplayName(@"Nr konta")]
        public string AccountNumber { get; set; }
        [DisplayName(@"Kategoria")]
        public string Category { get; set; }
        [DisplayName(@"Podkategoria")]
        public string SubCategory { get; set; }
        [DisplayName(@"Konto")]
        public string Owner { get; set; }
        [DisplayName(@"Komentarz")]
        public string Comment { get; set; }

        public string Color { get; set; }

        public bool IsEdited { get; set; }

        public override string ToString()
        {
            string str = $"{TransactionDate};{Title};{Details};{CounterPartData};{Amount}";
            return str;
        }

    }
}
