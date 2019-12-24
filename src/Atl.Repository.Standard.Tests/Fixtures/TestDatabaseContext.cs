using System;
using System.Collections.Generic;
using Atl.Repository.Standard.ApplicationContext.Contracts;
using Atl.Repository.Standard.Configuration.Contracts;
using Atl.Repository.Standard.DepdendencyInjection;
using Atl.Repository.Standard.DomainInjection.Contracts;
using Atl.Repository.Standard.Domains;
using Atl.Repository.Standard.Domains.Contracts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Atl.Repository.Standard.Tests.Fixtures
{
	public class TestDatabaseContext : IDisposable
	{
		#region Inner Classes
		#region Domains

		public interface ITenantDomain : IDomain<int>
		{
			Tenant Tenant { get; set; }
			int TenantId { get; set; }
		}

		public interface IOrganizationDomain : ITenantDomain
		{
			Organization Organization { get; set; }
			int OrganizationId { get; set; }
		}

		public abstract class BaseDomain
		{
			public virtual int Id { get; set; }
			public virtual bool IsActive { get; set; }
			public virtual bool IsDeleted { get; set; }
			public virtual bool IsLocked { get; set; }
			public virtual bool IsArchived { get; set; }
			public virtual bool IsSuspended { get; set; }
			public virtual DateTime? CreatedAt { get; set; }
			public virtual DateTime? UpdatedAt { get; set; }
		}

		public class Tenant : BaseDomain, IDomain<int>
		{
		}

		public class Organization : BaseDomain, ITenantDomain
		{
			public Tenant Tenant { get; set; }
			public int TenantId { get; set; }
		}

		public class User : BaseDomain, IOrganizationDomain
		{
			public Tenant Tenant { get; set; }
			public int TenantId { get; set; }

			public Organization Organization { get; set; }
			public int OrganizationId { get; set; }
		}
		#endregion

		#region Domain Injector
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="Atl.Repository.Standard.DomainInjection.Contracts.IDomainInjector" />
		public class DomainInjector : IDomainInjector
		{
			public void InjectDomain(ModelBuilder modelBuilder)
			{
				modelBuilder.Entity<Tenant>();
				modelBuilder.Entity<Organization>(cfg => { cfg.HasOne<Tenant>(x => x.Tenant).WithMany(); });
				modelBuilder.Entity<User>(cfg =>
                {
                    cfg.HasOne<Tenant>(x => x.Tenant).WithMany();
                    cfg.HasOne<Organization>(x => x.Organization).WithMany();
                });
			}
		}

		#endregion

        #endregion


        public DomainContextFactory ContextFactory { get; }


		/// <summary>
		/// Initializes a new instance of the <see cref="TestDatabaseContext"/> class.
		/// </summary>
		public TestDatabaseContext(IConfigurationProvider configurationProvider)
		{
			var domainInjector = new DomainInjector();
			ContextFactory = new DomainContextFactory(new List<IDomainInjector>() { domainInjector }, configurationProvider);

			using (var context = ContextFactory.CreateDbContext())
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
				context.Database.Migrate();
			}
		}
		
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			ContextFactory.CreateDbContext().Database.EnsureDeleted();
		}
	}
}
