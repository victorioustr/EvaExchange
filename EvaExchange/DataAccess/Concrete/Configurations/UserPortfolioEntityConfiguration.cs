using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.Configurations
{
    public class UserPortfolioEntityConfiguration : IEntityTypeConfiguration<UserPortfolio>
    {
        public void Configure(EntityTypeBuilder<UserPortfolio> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.ShareId).IsRequired();
            builder.Property(x => x.Lot).IsRequired();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.ShareId);

            builder.HasOne(p => p.Share).WithMany(p => p.UserPortfolios).HasForeignKey(p => p.ShareId);
            builder.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId);
        }
    }
}