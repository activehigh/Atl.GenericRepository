

using Microsoft.EntityFrameworkCore;

namespace Atl.Repository.Standard.DomainInjection.Contracts
{
    public interface IDomainInjector
    {
        void InjectDomain(ModelBuilder modelBuilder);
    }
}
