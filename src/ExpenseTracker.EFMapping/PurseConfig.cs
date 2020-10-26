using ExpenseTracker.Domain.Purses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.EFMapping
{
    public class PurseConfig : IEntityTypeConfiguration<Purse>
    {
        public void Configure(EntityTypeBuilder<Purse> builder)
        {
            builder.Property(p => p.Bill)
                .IsRequired();

            builder.Property(p => p.CurrencyCode)
                .IsRequired()
                .HasColumnType("char(3)");
        }
    }
}
