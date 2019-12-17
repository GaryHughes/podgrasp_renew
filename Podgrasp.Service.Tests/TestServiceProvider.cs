using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Podgrasp.Service.Model
{
    // A new instance of TestServiceProvider should be created for any test as it has a singleton database.
    // We provide the scope classes here but they don't do anything other than provide the interfaces the
    // calling code requires.

    public class TestServiceScope : IServiceScope
    {
        readonly IServiceProvider _serviceProvider;

        public TestServiceScope(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider => _serviceProvider;

        public void Dispose()
        {
        }
    }

    public class TestServiceScopeFactory : IServiceScopeFactory
    {
        readonly IServiceProvider _serviceProvider;

        public TestServiceScopeFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IServiceScope CreateScope() => new TestServiceScope(_serviceProvider);    
        
    }

    public class TestServiceProvider : IServiceProvider
    {
        DbContextOptions<PodgraspContext> _contextOptions = null;

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(PodgraspContext)) {
                return GetDbContext();
            }

            if (serviceType == typeof(IServiceScopeFactory)) {
                return new TestServiceScopeFactory(this);            
            }

            throw new ArgumentException($"Unknown type {serviceType}");
        }

        object GetDbContext()
        {
            if (_contextOptions is null) {
                _contextOptions = new DbContextOptionsBuilder<PodgraspContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            }
            
            var operationalStoreOptions = Options.Create(new OperationalStoreOptions());
            var context = new PodgraspContext(_contextOptions, operationalStoreOptions);
            
            return context;
        }
    }
}