using Atl.Repository.EF.Domains.Contracts;

namespace Atl.Repository.EF.ApplicationContext.Contracts
{
    public interface IApplicationContext<TKey>
    {
	    TDomain ApplyContext<TDomain>(TDomain obj) where TDomain : class, IDomain<TKey>;
    }
}
