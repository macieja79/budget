using System;
using System.Drawing;
using Budget.Core.Entities;
using Metaproject.DataGrid;
using Metaproject.Enums;
using Metaproject.Graphic;

namespace Budget.UI.ViewModels
{
    public class RuleRowModel : GridModelRow<Rule>
    {
        private readonly Rule _previous;

        public RuleRowModel(Rule t, Rule previous) : base(t)
        {
            _previous = previous;
        }

        #region <bindables>
   
        public string Value
        {
            get { return _item.Value; }
            set { _item.Value = value; }
        }

        public bool IsEnabled
        {
            get { return _item.IsEnabled; }
            set { _item.IsEnabled = value; }
        }

        public string TypeOfRule
        {
            get
            {
                string displayValue = EnumTools<Rule.RuleType>.GetDisplayValue(_item.TypeOfRule);
                return displayValue;
            }
            set
            {
                Rule.RuleType rule = EnumTools<Rule.RuleType>.ParseDisplayValue(value);
                _item.TypeOfRule = rule;
            }
        }

        #endregion
       
    }
}
