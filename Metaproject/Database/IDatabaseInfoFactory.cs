using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Database
{
    public interface IDatabaseInfoFactory
    {
        DatabaseInfo CreateDatabaseInfo();
    }
}
