# A generic repository for .net platform

A Lightweight EF Core Repository. More changes comming. 

# Supported Platform

|Platform   |Version      |
|----------|:-------------|
|.Net Standard |2.0+|

# Basic Usage

## 1. Domains

Please use any POCO classses but you have to derive it from `IDomain<>`. I would prefer to use `BaseDomain` to contain all the common properties.


```
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
}
```

## 2. Domain Injector

This class will inject domains when the repository is created. This provides options for using a DI to dynamically load domains from different assemblies

```
public class DomainInjector : IDomainInjector
{
  public void InjectDomain(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Tenant>();
  }
}
```

## 3. The Database configuration provider.

Same as the Domain Injector, this configuration provider supports DI and can be injected easily. Provides configuration and connectionstring to connect to database. For example, if we need to use a SQLite database we use -

```
public class TestConfigurationProvier : IConfigurationProvider
{
  public string ConnectionString => "";
  public DbContextOptionsBuilder ApplyDatabaseBuilderOptions(DbContextOptionsBuilder optionsBuilder)
  {
    var contectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "TestDatabase.db" };
    return optionsBuilder.UseSqlite(contectionStringBuilder.ToString());
  }
}
```

## 4. Put it all together and create a context factory

```
var configurationProvider = new TestConfigurationProvier();
var domainInjector = new DomainInjector();
ContextFactory = new DomainContextFactory(new List<IDomainInjector>() { domainInjector }, configurationProvider);
```

## 5. (Optional), If you want to generate Database using this library

Call ensure created to generate the database for you.
```
using (var context = ContextFactory.CreateDbContext())
{
  context.Database.EnsureCreated();
}
```

## 6. Create the repository and use it. 

```
Repository = new Repository<int>(idGenerator, ContextFactory, new DefaultSystemClock(), null);
```

Please note that, the `GetAll` and `GetById` method returns `IEnumerable` which are still to be excecuted. To load data call `.ToList()` and you can chain as many predicates as you like before calling it.

For example 

```
_repository.GetAll<>().Where(x => x.IsActive  && ... && ...).OrderByDescending(x => x...).ToList();
```

# Advanced Usage

Comming soon.


# License

Available as open source under the terms of the [MIT License](LICENSE).
