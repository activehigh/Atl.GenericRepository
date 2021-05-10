using System;
using System.Collections.Generic;
using Atl.Repository.EF.ApplicationContext.Contracts;
using Atl.Repository.EF.Configuration.Contracts;
using Atl.Repository.EF.DomainInjection.Contracts;
using Atl.Repository.EF.Repositories.Implementations;
using Atl.Repository.Standard.Domains.Contracts;
using Atl.Repository.Standard.Repositories.Contracts;
using Microsoft.Extensions.Internal;
using NSubstitute;

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


		public TestRepository(IConfigurationProvider configurationProvider, int id = 10, bool requiresId = true)
		{
			var dbContext = new TestDatabaseContext(configurationProvider);
			var idGenerator = Substitute.For<IKeyGenerator<int>>();
			idGenerator.DoesRequireNewKey(Arg.Any<int>()).Returns(requiresId);
			idGenerator.Generate(Arg.Any<IDomain<int>>()).Returns(id);

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
