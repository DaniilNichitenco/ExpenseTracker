using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.EFMapping
{
    public class OccasionConfig : IEntityTypeConfiguration<Occasion>
    {
        public void Configure(EntityTypeBuilder<Occasion> builder)
        {
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(p => p.Context)
                .IsRequired();
        }
    }
}
