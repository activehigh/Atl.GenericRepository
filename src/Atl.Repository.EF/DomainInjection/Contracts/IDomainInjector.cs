

using Microsoft.EntityFrameworkCore;

namespace Atl.Repository.EF.DomainInjection.Contracts
{
    public interface IDomainInjector
    {
        void InjectDomain(ModelBuilder modelBuilder);
    }
}
