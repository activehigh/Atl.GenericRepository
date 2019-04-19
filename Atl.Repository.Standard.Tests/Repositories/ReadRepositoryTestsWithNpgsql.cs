using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Atl.Repository.Standard.Configuration.Contracts;
using Atl.Repository.Standard.Repositories.Contracts;
using Atl.Repository.Standard.Repositories.Implementations;
using Atl.Repository.Standard.Tests.Fixtures;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NUnit.Framework;

namespace Atl.Repository.Standard.Tests.Repositories
{
    [TestFixture]
    public class ReadRepositoryTestsWithNPgSQL
    {
        private IGenericRepository<int> _repo;
        private TestRepository _testRepo;
        private DockerClient _client;
        private CreateContainerResponse _containerResponse;

        public class PgSqlConfigurationProvider : IConfigurationProvider
        {
            private readonly string _port;

            public PgSqlConfigurationProvider(string port)
            {
                _port = port;
            }

            public string ConnectionString => throw new NotImplementedException();
            public DbContextOptionsBuilder ApplyDatabaseBuilderOptions(DbContextOptionsBuilder optionsBuilder)
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder() { ConnectionString = $"User ID=postgres;Password=password;Server=127.0.0.1;Port={_port};Database=repotest;Integrated Security=true;Pooling=false;CommandTimeout=3000" };
                return optionsBuilder.UseNpgsql(connectionStringBuilder.ToString());
            }
        }

        [OneTimeSetUp]
        public async Task SetUp()
        {
            //setup container
            _client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            var (containerResponse, port) = await GetContainer(_client, "postgres", "10.7-alpine");
            _containerResponse = containerResponse;

            _testRepo = new TestRepository(new PgSqlConfigurationProvider(port), 20, false);
            _repo = _testRepo.Repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="image"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private async Task<(CreateContainerResponse, string)> GetContainer(DockerClient client, string image, string tag)
        {
            var hostPort =  new Random((int) DateTime.UtcNow.Ticks).Next(10000, 12000);
            //look for image
            var images = await client.Images.ListImagesAsync(new ImagesListParameters()
            {
                MatchName = $"{image}:{tag}",
            }, CancellationToken.None);

            //check if container exists
            var pgImage = images.FirstOrDefault();
            if (pgImage == null)
                throw new Exception($"Docker image for {image}:{tag} not found.");

            //create container from image
            var container = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                User = "postgres",
                Env = new List<string>()
                {
                    "POSTGRES_PASSWORD=password",
                    "POSTGRES_DB=repotest",
                    "POSTGRES_USER=postgres"
                },
                ExposedPorts = new Dictionary<string, EmptyStruct>()
                {
                    ["5432"] = new EmptyStruct()
                },
                HostConfig = new HostConfig()
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>()
                    {
                        ["5432"] = new List<PortBinding>()
                            {new PortBinding() {HostIP = "0.0.0.0", HostPort = $"{hostPort}"}}
                    }
                },
                Image = $"{image}:{tag}",
            }, CancellationToken.None);

            if (!await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters()
            {
                DetachKeys = $"d={image}"
            }, CancellationToken.None))
            {
                throw new Exception($"Could not start container: {container.ID}");
            }

            var count = 10;
            Thread.Sleep(5000);
            var containerStat = await client.Containers.InspectContainerAsync(container.ID, CancellationToken.None);
            while (!containerStat.State.Running && count-- > 0)
            {
                Thread.Sleep(1000);
                containerStat = await client.Containers.InspectContainerAsync(container.ID, CancellationToken.None);
            }

            return (container, $"{hostPort}");
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
            Assert.AreEqual(userFetched.Id, user.Id);

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
                .First(x => x.Id == user.Id);
            Assert.AreEqual(userFetched.Id, user.Id);
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
            var userFetched =
                (await _repo.GetAllAsync<TestDatabaseContext.User>(CancellationToken.None)).First(x => x.Id == user.Id);
            Assert.AreEqual(userFetched.Id, user.Id);

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
                .First(x => x.Id == user.Id);
            Assert.AreEqual(userFetched.Id, user.Id);
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

        [OneTimeTearDown]
        public async Task TearDown()
        {
            //stop container
            if (await _client.Containers.StopContainerAsync(_containerResponse.ID, new ContainerStopParameters(), CancellationToken.None))
            {
                //delete container
                await _client.Containers.RemoveContainerAsync(_containerResponse.ID, new ContainerRemoveParameters(), CancellationToken.None);
            }

            _client?.Dispose();
            _testRepo?.Dispose();
        }
    }
}
