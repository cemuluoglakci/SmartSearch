using Microsoft.EntityFrameworkCore;
using CoreSmart.Entities;
using System.Threading.Tasks;
using System.Threading;

namespace ApplicationSmart.Interfaces
{
    public interface ISmartSearchDataContext
    {
        DbSet<CoreSmart.Entities.Properties> Properties { get; set; }
        DbSet<Mgmt> Mgmt { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
