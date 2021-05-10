using Microsoft.EntityFrameworkCore;

namespace Atl.Repository.EF.Configuration.Contracts
{
    public interface IConfigurationProvider
    {
        string ConnectionString { get; }
	    DbContextOptionsBuilder ApplyDatabaseBuilderOptions(DbContextOptionsBuilder optionsBuilder);
    }
}
