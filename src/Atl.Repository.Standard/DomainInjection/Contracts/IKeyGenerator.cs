using System;
using System.Linq.Expressions;
using Atl.Repository.Standard.Domains.Contracts;

namespace Atl.Repository.Standard.DomainInjection.Contracts
{
    public interface IKeyGenerator<TKey>
    {
        TKey Generate(IDomain<TKey> obj);
        bool DoesRequireNewKey(TKey currentKey);

        Expression<Func<TDomain, bool>> Equal<TDomain>(Expression<Func<TDomain, TKey>> left, TKey right)
            where TDomain : IDomain<TKey>;
    }
}
