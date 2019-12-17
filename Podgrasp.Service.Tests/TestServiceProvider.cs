using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;

namespace Podgrasp.Service.Model
{
    // A new instance of this should be created for any test as it has a singleton database.
    public class TestServiceProvider : IServiceProvider
    {
        DbContextOptions<PodgraspContext> _contextOptions = null;

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(PodgraspContext)) {
                return GetDbContext();
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