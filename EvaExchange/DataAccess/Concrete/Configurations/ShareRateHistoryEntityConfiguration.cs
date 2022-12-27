using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.Configurations
{
    public class ShareRateHistoryEntityConfiguration : IEntityTypeConfiguration<ShareRateHistory>
    {
        public void Configure(EntityTypeBuilder<ShareRateHistory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Rate).IsRequired().HasColumnType("decimal(18, 2)");
            builder.Property(x => x.UpdatedDate).IsRequired();

            builder.HasIndex(x => x.UpdatedDate);

            builder.HasOne(p => p.Share).WithMany(p => p.ShareRateHistories).HasForeignKey(p => p.ShareId);
        }
    }
}