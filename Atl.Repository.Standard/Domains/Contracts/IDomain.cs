using System;

namespace Atl.Repository.Standard.Domains.Contracts
{
    #region IDomain Definition
    /// <summary>
    /// TO be used as the interface for all the domain
    /// </summary>
    public interface IDomain<T>
    {
        T Id { get; set; }

        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        bool IsLocked { get; set; }
        bool IsArchived { get; set; }
        bool IsSuspended { get; set; }

	    DateTime? CreatedAt { get; set; }
	    DateTime? UpdatedAt { get; set; }
	}

    public interface IAggregateDomain<T> : IDomain<T>
    {
    }

    public interface INonAggregateDomain<T> : IAggregateDomain<T>
    {
        T ParentId { get; set; }
	}

	#endregion

	#region Model Definition
	/// <summary>
	/// 
	/// </summary>
	public interface IModel<T>
    {
        T Id { get; set; }

        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        bool IsLocked { get; set; }
        bool IsArchived { get; set; }
        bool IsSuspended { get; set; }
    }

    public interface IAggregateModel<T> : IModel<T>
    {
    }

    public interface INonAggregateModel<T> : IAggregateModel<T>
    {
        T ParentId { get; set; }
    }

    #endregion
}
