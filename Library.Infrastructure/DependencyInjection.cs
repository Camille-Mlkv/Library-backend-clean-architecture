using Library.Infrastructure.Data;
using Library.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            return services;
        }
        public static IServiceCollection AddPersistence(this IServiceCollection services,DbContextOptions options)
        {
            services.AddPersistence()
                .AddSingleton<AppDbContext>(
                new AppDbContext((DbContextOptions<AppDbContext>)options));
            return services;
        }
    }
}
