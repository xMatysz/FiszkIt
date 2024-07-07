using FiszkIt.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiszkIt.Application.Mappings;

public class FlashSetMapping : IEntityTypeConfiguration<FlashSet>
{
    public void Configure(EntityTypeBuilder<FlashSet> builder)
    {
        builder.ToTable("FlashSets");

        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.FlashCards)
            .WithOne()
            .HasForeignKey("FlashSetId");
    }
}