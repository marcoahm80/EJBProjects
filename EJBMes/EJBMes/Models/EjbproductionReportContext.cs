using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EJBMes.Models;

public partial class EjbproductionReportContext : DbContext
{
    public EjbproductionReportContext()
    {
    }

    public EjbproductionReportContext(DbContextOptions<EjbproductionReportContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ProdReport> ProdReports { get; set; }

    public virtual DbSet<ScrapReport> ScrapReports { get; set; }

    public virtual DbSet<UserMes> UserMes { get; set; }

    public virtual DbSet<DowntimeReport> DowntimeReports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProdReport>(entity =>
        {
            entity.ToTable("ProdReport");

            entity.Property(e => e.LaborQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");
        });

        modelBuilder.Entity<ScrapReport>(entity =>
        {
            entity.ToTable("ScrapReport");

            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");
            entity.Property(e => e.ScrapQty).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<UserMes>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Users");

            entity.ToTable("UserMES");

            entity.Property(e => e.UserId).HasMaxLength(20);
            entity.Property(e => e.Company).HasMaxLength(10);
            entity.Property(e => e.EmployeeId).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Site).HasMaxLength(8);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<DowntimeReport>(entity =>
        {
            entity.ToTable("DowntimeReport");

            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
