using Atl.Repository.Standard.DatabaseContents.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Atl.Repository.Standard.Configuration.Contracts
{
    public interface IConfigurationProvider
    {
        string ConnectionString { get; }
	    DbContextOptionsBuilder ApplyDatabaseBuilderOptions(DbContextOptionsBuilder optionsBuilder);
    }
}
