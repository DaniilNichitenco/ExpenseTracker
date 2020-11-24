using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.EFMapping
{
    public class ExpenseConfig : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(p => p.Date)
                .IsRequired();

            builder.Property(p => p.Money)
                .IsRequired();

        }
    }
}
