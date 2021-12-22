using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureSmartDB
{
    class SmartSearchDBCreator: DesignTimeDBPattern<SmartSearchDataContext>
    {
        protected override SmartSearchDataContext CreateNewInstance(DbContextOptions<SmartSearchDataContext> options)
        {
            return new SmartSearchDataContext(options);
        }
    }
}
