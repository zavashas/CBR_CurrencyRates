using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTracker.Models;

public partial class CurrencyDbContext : DbContext
{
    public CurrencyDbContext()
    {
    }

    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-7505LTQ\\SQLEXPRESS;Initial Catalog=CurrencyDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.IdCurrency).HasName("PK__Currency__87BF4C6208168B3C");

            entity.ToTable("Currency");

            entity.Property(e => e.IdCurrency).HasColumnName("ID_Currency");
            entity.Property(e => e.CharCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.NameCurrency)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NumCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CurrencyRate>(entity =>
        {
            entity.HasKey(e => e.IdCurrencyRate).HasName("PK__Currency__79495C196952F969");

            entity.ToTable("CurrencyRate");

            entity.Property(e => e.IdCurrencyRate).HasColumnName("ID_CurrencyRate");
            entity.Property(e => e.CurrencyId).HasColumnName("Currency_ID");
            entity.Property(e => e.DateCurrencyRate).HasColumnType("datetime");
            entity.Property(e => e.RateValue).HasColumnType("decimal(10, 4)");
            entity.Property(e => e.VunitRate).HasColumnType("decimal(10, 4)");

            entity.HasOne(d => d.Currency).WithMany(p => p.CurrencyRates)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyR__Curre__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
