using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.EFMapping
{
    public class UserInfoConfig : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(15);
        }
    }
}
