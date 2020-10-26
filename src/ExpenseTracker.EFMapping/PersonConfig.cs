using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.EFMapping
{
    public class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasMany(p => p.Purses)
                .WithOne(p => p.Person)
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Notes)
                .WithOne(p => p.Person)
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Occasions)
                .WithOne(p => p.Person)
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
