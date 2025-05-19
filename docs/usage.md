# Atl.Repository.Standard

A flexible and powerful generic repository pattern implementation for .NET applications that supports dependency injection, multi-tenancy, and domain-driven design principles.

## Features

- Generic CRUD operations
- Built-in support for Dependency Injection
- Multi-tenancy support
- Parallelism and concurrency support
- Customizable unit of work patterns
- Domain-driven design friendly
- Support for multiple assemblies

## Basic Usage

### 1. Setup Domain Classes

First, create your domain classes inheriting from `BaseDomain`:

```csharp
public abstract class BaseDomain
{
    public virtual int Id { get; set; }
    public virtual bool IsActive { get; set; }
    public virtual bool IsDeleted { get; set; }
    public virtual bool IsLocked { get; set; }
    public virtual bool IsArchived { get; set; }
    public virtual bool IsSuspended { get; set; }
    public virtual DateTime? CreatedAt { get; set; }
    public virtual DateTime? UpdatedAt { get; set; }
}

public class Tenant : BaseDomain, IDomain<int>
{
    // Your tenant properties
}
```

### 2. Required Components

#### Id Generator
```csharp
// Use built-in implementations
var idGenerator = new IdentityKeyGenerator(); // For int keys
// or
var idGenerator = new GuidKeyGenerator(); // For Guid keys
```

#### Domain Injector
```csharp
public class DomainInjector : IDomainInjector
{
    public void InjectDomain(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>();
    }
}
```

#### System Clock
```csharp
var clock = new DefaultSystemClock();
```

#### Configuration Provider
```csharp
public class TestConfigurationProvider : IConfigurationProvider
{
    public string ConnectionString => "";
    public DbContextOptionsBuilder ApplyDatabaseBuilderOptions(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "TestDatabase.db" };
        return optionsBuilder.UseSqlite(connectionStringBuilder.ToString());
    }
}
```

#### Database Context Factory
```csharp
var configurationProvider = new TestConfigurationProvider();
var domainInjector = new DomainInjector();
var contextFactory = new DomainContextFactory(
    new List<IDomainInjector>() { domainInjector }, 
    configurationProvider
);
```

### 3. Create Repository
```csharp
var repository = new Repository<int>(idGenerator, contextFactory, clock);
```

## Advanced Usage with Dependency Injection

### 1. Install Required Package
```bash
dotnet add package Microsoft.Extensions.DependencyInjection
```

### 2. Configure Services
```csharp
var serviceCollection = new ServiceCollection();

// Register repository components
serviceCollection.AddTransient<IDomainContextFactory<DatabaseContext>, DomainContextFactory>();
serviceCollection.AddTransient<IDomainInjector, DomainInjector>();
serviceCollection.AddTransient<IKeyGenerator<long>, IdentityKeyGenerator>();
serviceCollection.AddTransient<IConfigurationProvider, ConfigProvider>();
serviceCollection.AddTransient<ISystemClock, DefaultSystemClock>();
serviceCollection.AddTransient(typeof(IGenericRepository<>), typeof(Repository<>));

// Build service provider
var serviceProvider = serviceCollection.BuildServiceProvider();
```

### 3. Use in Services
```csharp
public class OrderService : IOrderService
{
    private readonly IGenericRepository<long> _repository;
    
    public OrderService(IGenericRepository<long> repository)
    {
        _repository = repository;
    }

    public List<Order> GetActiveOrders()
    {
        return _repository.GetAll<Order>()
            .Where(x => x.IsActive)
            .OrderBy(x => x.CreatedAt)
            .ToList();
    }
}
```

## Working with Related Entities

You can easily work with related entities using Include:

```csharp
// Example with Organization and Tenant
public class Organization : BaseDomain
{
    public Tenant Tenant { get; set; }
    public int TenantId { get; set; }
}

// Query with includes
var organizations = _repository.GetAll<Organization>()
    .Include(x => x.Tenant)
    .Where(x => x.Tenant.Name == "Some Tenant")
    .ToList();
```
