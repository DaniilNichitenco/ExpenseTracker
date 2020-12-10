using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.EFMapping
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.UserInfo)
                .WithOne(uf => uf.User)
                .HasForeignKey((UserInfo uf) => uf.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Expenses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Purses)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Topics)
                .WithOne(t => t.User)
                .HasForeignKey((Topic t) => t.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
