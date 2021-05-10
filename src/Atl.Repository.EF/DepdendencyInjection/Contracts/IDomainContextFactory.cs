using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Atl.Repository.EF.DepdendencyInjection.Contracts
{
	public interface IDomainContextFactory<out T> : IDesignTimeDbContextFactory<T> where T: DbContext
	{
		new T CreateDbContext(params string[] args);
	}
}
