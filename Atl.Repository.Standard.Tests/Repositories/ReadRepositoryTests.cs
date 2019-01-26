using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atl.Repository.Standard.Repositories.Contracts;
using Atl.Repository.Standard.Tests.Fixtures;
using Xunit;

namespace Atl.Repository.Standard.Tests.Repositories
{
	public class ReadRepositoryTests : IClassFixture<TestRepository>
	{
		private readonly IGenericRepository<int> _repo;
		private readonly TestRepository _testRepo;

		public ReadRepositoryTests(TestRepository testRepo)
		{
			_repo = testRepo.Repository;
			_testRepo = testRepo;
		}

		[Fact]
		public void TestAll()
		{
			//save entity
			var tenant = new TestDatabaseContext.Tenant()
			{

			};
			tenant = _repo.Add(tenant);
			_testRepo.TContext.ContextId = tenant.Id;
			var org = new TestDatabaseContext.Organization()
			{
				
			};
			org = _repo.Add(org);
			_testRepo.OrgContext.ContextId = org.Id;
			var user = new TestDatabaseContext.User()
			{

			};
			_repo.Add(user);

			//get
			var userFethed = _repo.GetAll<TestDatabaseContext.User>().First();
			Assert.Equal(userFethed.Id, user.Id);
			Assert.Equal(10, userFethed.Id);

			//update
			userFethed.IsLocked = true;
			_repo.Update(userFethed);

			userFethed = _repo.GetAll<TestDatabaseContext.User>().First();
			Assert.True(userFethed.IsLocked);

			//delete
			_repo.Delete(user);

			userFethed = _repo.GetAll<TestDatabaseContext.User>().FirstOrDefault();
			Assert.Null(userFethed);
		}
	}
}
