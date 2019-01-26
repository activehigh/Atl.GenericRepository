using System;
using System.Collections.Generic;
using System.Text;
using Atl.Repository.Standard.ApplicationContext.Contracts;
using Atl.Repository.Standard.DomainInjection.Contracts;
using Atl.Repository.Standard.DomainInjection.KeyGenerators;
using Atl.Repository.Standard.Domains.Contracts;
using Atl.Repository.Standard.Repositories.Contracts;
using Atl.Repository.Standard.Repositories.Implementations;
using Microsoft.Extensions.Internal;
using NSubstitute;
using Xunit;

namespace Atl.Repository.Standard.Tests.Fixtures
{
	public class TestRepository : IDisposable
	{
		public IGenericRepository<int> Repository { get; }
		public OrganizationContext OrgContext { get; }
		public TenantContext TContext { get; }

		#region Context

		public class TenantContext : IApplicationContext<int>
		{
			public int ContextId { get; set; }
			public TDomain ApplyContext<TDomain>(TDomain obj) where TDomain : class, IDomain<int>
			{
				if (obj is TestDatabaseContext.ITenantDomain nobj)
				{
					nobj.TenantId = ContextId;
				}
				return obj;
			}
		}


		public class OrganizationContext : IApplicationContext<int>
		{
			public int ContextId { get; set; }
			public TDomain ApplyContext<TDomain>(TDomain obj) where TDomain : class, IDomain<int>
			{
				if (obj is TestDatabaseContext.IOrganizationDomain nobj)
				{
					nobj.OrganizationId = ContextId;
				}
				return obj;
			}
		}
		#endregion


		public TestRepository()
		{
			var dbContext = new TestDatabaseContext();
			var idGenerator = Substitute.For<IKeyGenerator<int>>();
			idGenerator.DoesRequireNewKey(Arg.Any<int>()).Returns(true);
			idGenerator.Generate(Arg.Any<IDomain<int>>()).Returns(10);

			OrgContext = new OrganizationContext();
			TContext = new TenantContext();

			var clock = Substitute.For<ISystemClock>();
			var applicationContexts =
				new List<IApplicationContext<int>>() { TContext, OrgContext };
			Repository = new Repository<int>(idGenerator, dbContext.ContextFactory, clock, applicationContexts);
		}

		public void Dispose()
		{
		}
	}
}
