using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PR20.Models;

public partial class SpravochnikV5Context : DbContext
{
    public SpravochnikV5Context()
    {
    }

    public SpravochnikV5Context(DbContextOptions<SpravochnikV5Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DirectoryCompletionWork> DirectoryCompletionWorks { get; set; }

    public virtual DbSet<DirectoryObject> DirectoryObjects { get; set; }

    public virtual DbSet<DirectoryPrice> DirectoryPrices { get; set; }

    public virtual DbSet<DirectoryTypeWork> DirectoryTypeWorks { get; set; }

    public virtual DbSet<VolumeWorkObject> VolumeWorkObjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=LAB44-11\\SQLEXPRESS; Database=SpravochnikV5; user=исп-34; Password=1234567890; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DirectoryCompletionWork>(entity =>
        {
            entity.HasKey(e => e.IdObject);

            entity.ToTable("DirectoryCompletionWork");

            entity.Property(e => e.IdObject).HasColumnName("idObject");
            entity.Property(e => e.DateCompletionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DirectoryObject>(entity =>
        {
            entity.HasKey(e => e.IdObject);

            entity.ToTable("DirectoryObject");

            entity.Property(e => e.IdObject).HasColumnName("idObject");
            entity.Property(e => e.DateBeginingWork).HasColumnType("datetime");
        });

        modelBuilder.Entity<DirectoryPrice>(entity =>
        {
            entity.HasKey(e => e.IdWork);

            entity.ToTable("DirectoryPrice");

            entity.Property(e => e.IdWork).HasColumnName("idWork");
            entity.Property(e => e.IdTypeWork).HasColumnName("idTypeWork");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdTypeWorkNavigation).WithMany(p => p.DirectoryPrices)
                .HasForeignKey(d => d.IdTypeWork)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectoryPrice_DirectoryTypeWork");
        });

        modelBuilder.Entity<DirectoryTypeWork>(entity =>
        {
            entity.HasKey(e => e.IdTypeWork);

            entity.ToTable("DirectoryTypeWork");

            entity.Property(e => e.IdTypeWork).HasColumnName("idTypeWork");
        });

        modelBuilder.Entity<VolumeWorkObject>(entity =>
        {
            entity.HasKey(e => e.IdObject);

            entity.ToTable("VolumeWorkObject");

            entity.Property(e => e.IdObject)
                .ValueGeneratedNever()
                .HasColumnName("idObject");
            entity.Property(e => e.IdWork).HasColumnName("idWork");
            entity.Property(e => e.VolumeWork).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdObjectNavigation).WithOne(p => p.VolumeWorkObject)
                .HasForeignKey<VolumeWorkObject>(d => d.IdObject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VolumeWorkObject_DirectoryCompletionWork");

            entity.HasOne(d => d.IdObject1).WithOne(p => p.VolumeWorkObject)
                .HasForeignKey<VolumeWorkObject>(d => d.IdObject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VolumeWorkObject_DirectoryObject");

            entity.HasOne(d => d.IdWorkNavigation).WithMany(p => p.VolumeWorkObjects)
                .HasForeignKey(d => d.IdWork)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VolumeWorkObject_DirectoryPrice");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
