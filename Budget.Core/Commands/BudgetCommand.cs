using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.UI;
using Metaproject.Patterns.Command;

namespace Budget.Core.Commands
{
    public abstract class BudgetCommand : ICommand
    {
        protected IOutput _output;
        protected IExcel _excel;
        
        protected BudgetCommand(IOutput output, IExcel excel)
        {
            _output = output;
            _excel = excel;
        }
        
        public abstract string ID { get; }
        public abstract void Run();
        
    }
}
