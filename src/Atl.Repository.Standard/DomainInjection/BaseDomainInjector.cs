using Atl.Repository.Standard.DomainInjection.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Atl.Repository.Standard.DomainInjection
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
