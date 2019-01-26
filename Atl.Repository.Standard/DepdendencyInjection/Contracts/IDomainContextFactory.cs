using System;
using System.Collections.Generic;
using System.Text;
using Atl.Repository.Standard.DatabaseContents.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Atl.Repository.Standard.DepdendencyInjection.Contracts
{
	public interface IDomainContextFactory<out T> : IDesignTimeDbContextFactory<T> where T: DbContext
	{
		new T CreateDbContext(params string[] args);
	}
}
