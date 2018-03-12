using Budget.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.UI
{
    public interface IRulesRepository
    {
        List<Rule> GetRules();
        void SaveRules(List<Rule> rules);
       

    }

}
