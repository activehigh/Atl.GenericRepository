using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Atl.Repository.Standard.Configuration.Contracts;
using Atl.Repository.Standard.Repositories.Contracts;
using Atl.Repository.Standard.Repositories.Implementations;
using Atl.Repository.Standard.Tests.Fixtures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Atl.Repository.Standard.Tests.Repositories
{
	public class ReadRepositoryTests
    {

        #region Configuration Provier
        public class SQLiteConfigurationProvider : IConfigurationProvider
        {
            public string ConnectionString => "";
            public DbContextOptionsBuilder ApplyDatabaseBuilderOptions(DbContextOptionsBuilder optionsBuilder)
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "TestDatabase.db" };
                return optionsBuilder.UseSqlite(connectionStringBuilder.ToString());
            }
        }
        #endregion

        private IGenericRepository<int> _repo;
		private TestRepository _testRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadRepositoryTests"/> class.
        /// </summary>
        [SetUp]
        public void SetUp()
		{
            _testRepo = new TestRepository(new SQLiteConfigurationProvider());
			_repo = _testRepo.Repository;
		}

        /// <summary>
        /// Gets all should return successfully.
        /// </summary>
        [TestCase]
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
			ClassicAssert.AreEqual(userFetched.Id, user.Id);
			ClassicAssert.AreEqual(10, userFetched.Id);

			//update
			userFetched.IsLocked = true;
			_repo.Update(userFetched);

			userFetched = _repo.GetAll<TestDatabaseContext.User>().First();
			ClassicAssert.True(userFetched.IsLocked);

			//delete
			_repo.Delete(user);

			userFetched = _repo.GetAll<TestDatabaseContext.User>().FirstOrDefault();
			ClassicAssert.Null(userFetched);
		}

        /// <summary>
        /// Gets all with include should return related object.
        /// </summary>
        [TestCase]
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
            ClassicAssert.AreEqual(userFetched.Id, user.Id);
            ClassicAssert.AreEqual(10, userFetched.Id);
            ClassicAssert.NotNull(userFetched.Tenant);
            ClassicAssert.NotNull(userFetched.Organization);

            //update
            userFetched.IsLocked = true;
            _repo.Update(userFetched);

            userFetched = _repo.GetAll<TestDatabaseContext.User>().First();
            ClassicAssert.True(userFetched.IsLocked);

            //delete
            _repo.Delete(user);

            userFetched = _repo.GetAll<TestDatabaseContext.User>().FirstOrDefault();
            ClassicAssert.Null(userFetched);
        }

        /// <summary>
        /// Gets all asynchronous should return successfully.
        /// </summary>
        /// <returns></returns>
        [TestCase]
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
            ClassicAssert.AreEqual(userFetched.Id, user.Id);
            ClassicAssert.AreEqual(10, userFetched.Id);

            //update
            userFetched.IsLocked = true;
            await _repo.UpdateAsync(userFetched, CancellationToken.None);

            userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).First();
            ClassicAssert.True(userFetched.IsLocked);

            //delete
            await _repo.DeleteAsync(user, CancellationToken.None);

            userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).FirstOrDefault();
            ClassicAssert.Null(userFetched);
        }

        /// <summary>
        /// Gets all asynchronous with include should return related object.
        /// </summary>
        /// <returns></returns>
        [TestCase]
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
            ClassicAssert.AreEqual(userFetched.Id, user.Id);
            ClassicAssert.AreEqual(10, userFetched.Id);
            ClassicAssert.NotNull(userFetched.Tenant);
            ClassicAssert.NotNull(userFetched.Organization);

            //update
            userFetched.IsLocked = true;
            await _repo.UpdateAsync(userFetched, CancellationToken.None);

            userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).First();
            ClassicAssert.True(userFetched.IsLocked);

            //delete
            await _repo.DeleteAsync(user, CancellationToken.None);

            userFetched = (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).FirstOrDefault();
            ClassicAssert.Null(userFetched);
        }
    }
}
