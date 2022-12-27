using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Business.Fakes.QT
{
    public sealed class QTInMemory : ProjectDbContext
    {
        public QTInMemory(DbContextOptions<QTInMemory> options, IConfiguration configuration)
            : base(options, configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(
                    optionsBuilder.UseInMemoryDatabase(Configuration.GetConnectionString("QTInMemory")));
            }
        }
    }
}