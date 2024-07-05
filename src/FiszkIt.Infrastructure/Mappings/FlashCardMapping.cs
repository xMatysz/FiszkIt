using FiszkIt.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiszkIt.Infrastructure.Mappings;

public class FlashCardMapping : IEntityTypeConfiguration<FlashCard>
{
    public void Configure(EntityTypeBuilder<FlashCard> builder)
    {
        builder.HasKey(x=>x.Id);
    }
}