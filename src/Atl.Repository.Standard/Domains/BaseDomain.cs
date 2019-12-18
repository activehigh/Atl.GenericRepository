using System;
using System.ComponentModel.DataAnnotations;
using Atl.Repository.Standard.Domains.Contracts;

namespace Atl.Repository.Standard.Domains
{
    #region Domains
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Atl.Repository.Domains.Contracts.IDomain" />
    [Serializable]
    public class BaseDomain<TKey> : IDomain<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual bool IsLocked { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual bool IsSuspended { get; set; }

		public virtual DateTime? CreatedAt { get; set; }
		public virtual DateTime? UpdatedAt { get; set; }
    }

    public class AggregateDomain<TKey> : BaseDomain<TKey>, IAggregateDomain<TKey>
    {
        
    }

    public class NonAggregateDomain<TKey> : BaseDomain<TKey>, INonAggregateDomain<TKey>
    {
        [Key]
        public virtual TKey ParentId { get; set; }
    }
    #endregion


    #region Models
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Atl.Repository.Domains.Contracts.IModel" />
    public class BaseModel<TKey> : IModel<TKey>
    {
        public virtual TKey Id { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual bool IsLocked { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual bool IsSuspended { get; set; }
    }

    public class AggregateModel<TKey> : BaseModel<TKey>, IAggregateModel<TKey>
    {

    }

    public class NonAggregateModel<TKey> : BaseModel<TKey>, INonAggregateModel<TKey>
    {
        public virtual TKey ParentId { get; set; }
    }
    #endregion
}
