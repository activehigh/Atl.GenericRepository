using Atl.Repository.Standard.Domains.Contracts;

namespace Atl.Repository.Standard.ApplicationContext.Contracts
{
    public interface IApplicationContext<TKey>
    {
	    TDomain ApplyContext<TDomain>(TDomain obj) where TDomain : class, IDomain<TKey>;
    }
}
