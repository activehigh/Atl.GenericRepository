using System;
using System.Linq.Expressions;
using Atl.Repository.Standard.DomainInjection.Contracts;
using Atl.Repository.Standard.Domains.Contracts;

namespace Atl.Repository.Standard.DomainInjection.KeyGenerators
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Guid" />
    public class GuidKeyGenerator : IKeyGenerator<Guid>
    {
        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <returns></returns>
        public Guid Generate(IDomain<Guid> obj)
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Doeses this require new key.
        /// </summary>
        /// <param name="currentKey">The current key.</param>
        /// <returns></returns>
        public bool DoesRequireNewKey(Guid currentKey)
        {
            return currentKey == Guid.Empty;
        }

        /// <summary>
        /// Equals the specified left.
        /// </summary>
        /// <typeparam name="TDomain">The type of the domain.</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public Expression<Func<TDomain, bool>> Equal<TDomain>(Expression<Func<TDomain, Guid>> left, Guid right) where TDomain : IDomain<Guid>
        {
            var member = left.Body as MemberExpression;
            var boolFuncType = typeof(Func<,>).MakeGenericType(typeof(TDomain), typeof(bool));
            var invokedExp = Expression.Invoke((Expression<Func<Guid, bool>>)(a => a == right), member);
            var searchExp = Expression.Lambda(boolFuncType, invokedExp, left.Parameters);
            return Expression.Lambda<Func<TDomain, bool>>(searchExp.Body, left.Parameters);
        }
    }
}
