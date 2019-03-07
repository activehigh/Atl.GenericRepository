using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadRepositoryTests"/> class.
        /// </summary>
        public ReadRepositoryTests()
		{
            _testRepo = new TestRepository();
			_repo = _testRepo.Repository;
		}

        /// <summary>
        /// Gets all should return successfully.
        /// </summary>
        [Fact]
		public void GetAll_Should_Return_Successfully()
		{
			//save entity
            var tenant = new TestDatabaseContext.Tenant();
			tenant = _repo.Add(tenant);
			_testRepo.TContext.ContextId = tenant.Id;
            var org = new TestDatabaseContext.Organization();
			org = _repo.Add(org);
			_testRepo.OrgContext.ContextId = org.Id;
            var user = new TestDatabaseContext.User();
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

        /// <summary>
        /// Gets all with include should return related object.
        /// </summary>
        [Fact]
        public void GetAll_WithInclude_Should_Return_RelatedObject()
        {
            //save entity
            var tenant = new TestDatabaseContext.Tenant();
            tenant = _repo.Add(tenant);
            _testRepo.TContext.ContextId = tenant.Id;

            var org = new TestDatabaseContext.Organization();
            org = _repo.Add(org);
            _testRepo.OrgContext.ContextId = org.Id;

            var user = new TestDatabaseContext.User();
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

        /// <summary>
        /// Gets all asynchronous should return successfully.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllAsync_Should_Return_Successfully()
        {
            //save entity
            var tenant = new TestDatabaseContext.Tenant();
            tenant = await _repo.AddAsync(tenant, CancellationToken.None);
            _testRepo.TContext.ContextId = tenant.Id;

            var org = new TestDatabaseContext.Organization();
            org = await _repo.AddAsync(org, CancellationToken.None);
            _testRepo.OrgContext.ContextId = org.Id;

            var user = new TestDatabaseContext.User();
            await _repo.AddAsync(user, CancellationToken.None);

            //get
            var userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).First();
            Assert.Equal(userFetched.Id, user.Id);
            Assert.Equal(10, userFetched.Id);

            //update
            userFetched.IsLocked = true;
            await _repo.UpdateAsync(userFetched, CancellationToken.None);

            userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).First();
            Assert.True(userFetched.IsLocked);

            //delete
            await _repo.DeleteAsync(user, CancellationToken.None);

            userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).FirstOrDefault();
            Assert.Null(userFetched);
        }

        /// <summary>
        /// Gets all asynchronous with include should return related object.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllAsync_WithInclude_Should_Return_RelatedObject()
        {
            //save entity
            var tenant = new TestDatabaseContext.Tenant();
            tenant = await _repo.AddAsync(tenant, CancellationToken.None);
            _testRepo.TContext.ContextId = tenant.Id;

            var org = new TestDatabaseContext.Organization();
            org = await _repo.AddAsync(org, CancellationToken.None);
            _testRepo.OrgContext.ContextId = org.Id;

            var user = new TestDatabaseContext.User();
            await _repo.AddAsync(user, CancellationToken.None);

            //get
            var userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None))
                .Include(x => x.Organization)
                .Include(x => x.Tenant)
                .First();
            Assert.Equal(userFetched.Id, user.Id);
            Assert.Equal(10, userFetched.Id);
            Assert.NotNull(userFetched.Tenant);
            Assert.NotNull(userFetched.Organization);

            //update
            userFetched.IsLocked = true;
            await _repo.UpdateAsync(userFetched, CancellationToken.None);

            userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).First();
            Assert.True(userFetched.IsLocked);

            //delete
            await _repo.DeleteAsync(user, CancellationToken.None);

            userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).FirstOrDefault();
            Assert.Null(userFetched);
        }
    }
}
