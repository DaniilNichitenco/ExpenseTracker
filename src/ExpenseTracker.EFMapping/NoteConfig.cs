using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EFMapping
{
    public class NoteConfig : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(p => p.Message)
                .IsRequired();
        }
    }
}
