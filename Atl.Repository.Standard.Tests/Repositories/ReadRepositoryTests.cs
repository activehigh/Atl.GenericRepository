using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atl.Repository.Standard.Repositories.Contracts;
using Atl.Repository.Standard.Repositories.Implementations;
using Atl.Repository.Standard.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Atl.Repository.Standard.Tests.Repositories
{
	public class ReadRepositoryTests
	{
		private readonly IGenericRepository<int> _repo;
		private readonly TestRepository _testRepo;

		public ReadRepositoryTests()
		{
            _testRepo = new TestRepository();
			_repo = _testRepo.Repository;
		}

		[Fact]
		public void GetAll_Should_Return_Successfully()
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
			var userFetched = _repo.GetAll<TestDatabaseContext.User>()
                .First();
			Assert.Equal(userFetched.Id, user.Id);
			Assert.Equal(10, userFetched.Id);

			//update
			userFetched.IsLocked = true;
			_repo.Update(userFetched);

			userFetched = _repo.GetAll<TestDatabaseContext.User>().First();
			Assert.True(userFetched.IsLocked);

			//delete
			_repo.Delete(user);

			userFetched = _repo.GetAll<TestDatabaseContext.User>().FirstOrDefault();
			Assert.Null(userFetched);
		}

        [Fact]
        public void GetAll_WithInclude_Should_Return_RelatedObject()
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
            var userFetched = _repo.GetAll<TestDatabaseContext.User>()
                .Include(x => x.Organization)
                .Include(x => x.Tenant)
                .First();
            Assert.Equal(userFetched.Id, user.Id);
            Assert.Equal(10, userFetched.Id);
            Assert.NotNull(userFetched.Tenant);
            Assert.NotNull(userFetched.Organization);

            //update
            userFetched.IsLocked = true;
            _repo.Update(userFetched);

            userFetched = _repo.GetAll<TestDatabaseContext.User>().First();
            Assert.True(userFetched.IsLocked);

            //delete
            _repo.Delete(user);

            userFetched = _repo.GetAll<TestDatabaseContext.User>().FirstOrDefault();
            Assert.Null(userFetched);
        }
    }
}
