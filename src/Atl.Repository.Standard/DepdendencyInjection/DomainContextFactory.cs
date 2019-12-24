using System;
using System.Collections.Generic;
using System.Linq;
using Atl.Repository.Standard.Configuration.Contracts;
using Atl.Repository.Standard.DatabaseContents.Implementations;
using Atl.Repository.Standard.DepdendencyInjection.Contracts;
using Atl.Repository.Standard.DomainInjection.Contracts;
using Microsoft.EntityFrameworkCore.Design;

namespace Atl.Repository.Standard.DepdendencyInjection
{
    public class DomainContextFactory : IDomainContextFactory<DatabaseContext>
    {
        private readonly IEnumerable<IDomainInjector> _domainInjectors;
        private readonly IConfigurationProvider _configProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainContextFactory"/> class.
        /// </summary>
        /// <param name="domainInjectors">The domain injectors.</param>
        /// <param name="configProvider">The configuration provider.</param>
        public DomainContextFactory(IEnumerable<IDomainInjector> domainInjectors, IConfigurationProvider configProvider)
        {
            _domainInjectors = domainInjectors;
            _configProvider = configProvider;
        }

		/// <summary>Creates a new instance of a derived context.</summary>
		/// <param name="args">Arguments provided by the design-time service.</param>
		/// <returns>An instance of <span class="typeparameter">TContext</span>.</returns>
		public DatabaseContext CreateDbContext(params string[] args)
		{
			return new DatabaseContext(_domainInjectors, _configProvider);
		}
    }
}
