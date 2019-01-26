using System.Configuration;
using Atl.Repository.Standard.Configuration.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Atl.Repository.Standard.Configuration
{
    public class DefaultConfigurationProvider : IConfigurationProvider
    {
        public string ConnectionString => ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

		/// <summary>
		/// Applies the database builder options.
		/// </summary>
		/// <param name="optionsBuilder">The options builder.</param>
		/// <returns></returns>
		public DbContextOptionsBuilder ApplyDatabaseBuilderOptions(DbContextOptionsBuilder optionsBuilder)
	    {
		    return optionsBuilder;
	    }
    }
}
