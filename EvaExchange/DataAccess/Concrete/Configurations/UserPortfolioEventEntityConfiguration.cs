using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.Configurations
{
    public class UserPortfolioEventEntityConfiguration : IEntityTypeConfiguration<UserPortfolioEvent>
    {
        public void Configure(EntityTypeBuilder<UserPortfolioEvent> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserPortfolioEventType).IsRequired();
            builder.Property(x => x.Lot).IsRequired();
            builder.Property(x => x.Rate).IsRequired().HasColumnType("decimal(18, 2)");

            builder.HasIndex(x => x.UserPortfolioEventType);

            builder.HasOne(p => p.UserPortfolio).WithMany(p => p.UserPortfolioEvents).HasForeignKey(p => p.UserPortfolioId);
        }
    }
}