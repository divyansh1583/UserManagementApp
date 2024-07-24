using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Infrastructure.Data;

public partial class UserManagementContext : DbContext
{
    public UserManagementContext()
    {
    }

    public UserManagementContext(DbContextOptions<UserManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DcAddressType> DcAddressTypes { get; set; }

    public virtual DbSet<DcUser> DcUsers { get; set; }

    public virtual DbSet<DcUserAddress> DcUserAddresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DcAddressType>(entity =>
        {
            entity.HasKey(e => e.AddressTypeId).HasName("PK__DC_Addre__8BF56CC1CA057CE2");

            entity.ToTable("DC_AddressTypes");

            entity.HasIndex(e => e.AddressTypeName, "UQ__DC_Addre__4D15DD88B463F126").IsUnique();

            entity.Property(e => e.AddressTypeId).HasColumnName("AddressTypeID");
            entity.Property(e => e.AddressTypeName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DcUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__DC_Users__1788CC4C620A0A4C");

            entity.ToTable("DC_Users");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DcUserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__DC_UserA__091C2AFB542AFB92");

            entity.ToTable("DC_UserAddresses");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.AddressType).WithMany(p => p.DcUserAddresses)
                .HasForeignKey(d => d.AddressTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_UserAd__Addre__220BD66F");

            entity.HasOne(d => d.User).WithMany(p => p.DcUserAddresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_UserAd__UserI__22FFFAA8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
