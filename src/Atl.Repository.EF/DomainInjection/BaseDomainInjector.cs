using Atl.Repository.EF.DomainInjection.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Atl.Repository.EF.DomainInjection
{
    public class BaseDomainInjector : IDomainInjector
    {
        /// <summary>
        /// Gets the domains.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public virtual void InjectDomain(ModelBuilder modelBuilder)
        {
        }
    }
}
