# Atl.Repository.Standard

A flexible and powerful generic repository pattern implementation for .NET applications that supports dependency injection, multi-tenancy, and domain-driven design principles.

## Overview

Atl.Repository.Standard is a robust implementation of the repository pattern that helps developers maintain a clean and maintainable data access layer. It provides a standardized way to handle database operations while supporting advanced features like:

- Multi-tenancy support for data isolation
- Built-in dependency injection
- Support for multiple assemblies
- Customizable unit of work patterns
- Domain-driven design principles
- Parallelism and concurrency handling

For detailed usage instructions, please refer to:
- [Basic Usage Guide](https://activehigh.wordpress.com/2019/03/18/atl-repository-standard-basic-usage/)
- [Advanced Usage with Dependency Injection](https://activehigh.wordpress.com/2019/03/21/atl-repository-standard-using-ioc-or-a-di-container/)

## Installation

### NuGet Package

```bash
# Using .NET CLI
dotnet add package Atl.Repository.Standard

# Using Package Manager Console
Install-Package Atl.Repository.Standard
```

### Required Dependencies

The package requires the following dependencies:
- Microsoft.EntityFrameworkCore (>= 6.0.0)
- Microsoft.Extensions.DependencyInjection (>= 6.0.0)

## Features

- Generic CRUD operations
- Built-in support for Dependency Injection
- Multi-tenancy support
- Parallelism and concurrency support
- Customizable unit of work patterns
- Domain-driven design friendly
- Support for multiple assemblies

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Author

Mahmudul Islam - [@activehigh](https://activehigh.wordpress.com/)
