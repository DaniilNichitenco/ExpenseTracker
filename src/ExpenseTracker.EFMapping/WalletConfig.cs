using ExpenseTracker.Domain.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.EFMapping
{
    public class WalletConfig : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.Property(p => p.Bill)
                .IsRequired();

            builder.Property(p => p.CurrencyCode)
                .IsRequired()
                .HasColumnType("char(3)");

            builder.Property(p => p.RowVersion)
                .IsRowVersion();

            builder.HasMany(p => p.Expenses)
                .WithOne(e => e.Wallet)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
