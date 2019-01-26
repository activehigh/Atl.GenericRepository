using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atl.Repository.Standard.Domains.Contracts;

namespace Atl.Repository.Standard.Repositories.Contracts
{
	/// <summary>
	/// The generic repository interface
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	public interface IGenericRepository<TKey> 
    {
        TDomain GetById<TDomain>(TKey id) where TDomain : class, IDomain<TKey>;
        IQueryable<TDomain> GetAll<TDomain>() where TDomain : class, IDomain<TKey>;
		TDomain Add<TDomain>(TDomain obj) where TDomain : class, IDomain<TKey>;
        TDomain Update<TDomain>(TDomain obj) where TDomain : class, IDomain<TKey>;
		TDomain Delete<TDomain>(TKey id) where TDomain : class, IDomain<TKey>;
        void DeleteRange<TDomain>(IEnumerable<TKey> ids) where TDomain : class, IDomain<TKey>;
        TDomain Delete<TDomain>(TDomain obj) where TDomain : class, IDomain<TKey>;
        void DeleteRange<TDomain>(IEnumerable<TDomain> objects) where TDomain : class, IDomain<TKey>;
    }
}
