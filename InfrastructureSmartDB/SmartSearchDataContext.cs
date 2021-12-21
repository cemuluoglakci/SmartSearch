using ApplicationSmart.Interfaces;
using CoreSmart.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureSmartDB
{
    class SmartSearchDataContext : DbContext, ISmartSearchDataContext
    {
        public DbSet<Properties> Properties { get; set; }
        public DbSet<Mgmt> Mgmt { get; set; }
    }
}
