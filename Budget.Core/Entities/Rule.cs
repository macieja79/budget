using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Entities
{
    public class Rule : ICloneable
    {
        public enum RuleType
        {
            [Display(Name = @"Dane")]
            CounterpartData = 3,
            [Display(Name = @"Tytul")]
            Title = 1,
            [Display(Name = @"Szczegoly")]
            Details = 2,
            [Display(Name = @"Nr konta")]
            Account = 0,
        }
        
        public RuleType TypeOfRule { get ; set; }
        public string Value { get; set; }
        public string Color { get; set; }
        public bool IsEnabled { get; set; } = true;

        public bool CheckIfMatch(Transaction transaction)
        {
            if (transaction.IsNullObj()) return false;

            bool isMatch = false;
            var operatorAndValue = GetOperator(this.Value);
            string operejtor = operatorAndValue.Item1;
            string pureValue = operatorAndValue.Item2;
            if (this.TypeOfRule == Rule.RuleType.Account)
                isMatch = CompareValues(transaction.AccountNumber, pureValue, "=");
            if (this.TypeOfRule == Rule.RuleType.Title)
                isMatch = CompareValues(transaction.Title, pureValue, operejtor);
            if (this.TypeOfRule == Rule.RuleType.Details)
                isMatch = CompareValues(transaction.Details, pureValue, operejtor);
            if (this.TypeOfRule == Rule.RuleType.CounterpartData)
                isMatch = CompareValues(transaction.CounterPartData, pureValue, operejtor);

            return isMatch;
        }


      

       



         Tuple<string, string> GetOperator(string value)
        {

            if (value.IsNotNull())
            {
                string[] operators = {"*", "^", "$", "="};
                foreach (string op in operators)
                {
                    if (value.StartsWith(op))
                    {
                        string val = value.Substring(op.Length);
                        return new Tuple<string, string>(op, val);
                    }
                }
            }
            return new Tuple<string, string>("", value);

        }


        bool CompareValues(string first, string second, string operejtor)
        {

            if (ObjTools.IsAnyNull(first, second)) return false;

            string val1 = first.ToLower().Trim();
            string val2 = second.ToLower().Trim();

            if (val2.Contains(";"))
            {
                string[] splited = val2.Split(';');
                foreach (string iValue in splited)
                {
                    var iOperatorAndValue = GetOperator(iValue);
                    string iOperator = iOperatorAndValue.Item1;
                    if (iOperator.IsNull())
                        iOperator = operejtor;
                    bool isItemCompared = CompareValues(first, iOperatorAndValue.Item2, iOperator);
                    if (isItemCompared)
                        return true;
                }
            }

            if (operejtor == "=")
                return (val1 == val2);
            else if (operejtor == "*")
                return (val1.Contains(val2));
            else if (operejtor == "^")
                return (val1.StartsWith(val2));
            else if (operejtor == "$")
                return (val1.EndsWith(val2));

            return false;
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
