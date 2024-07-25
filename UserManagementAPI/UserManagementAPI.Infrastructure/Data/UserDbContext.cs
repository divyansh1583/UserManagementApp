using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Domain;

public partial class UserDbContext : DbContext
{
    public UserDbContext()
    {
    }

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DcCity> DcCities { get; set; }

    public virtual DbSet<DcCountry> DcCountries { get; set; }

    public virtual DbSet<DcState> DcStates { get; set; }

    public virtual DbSet<DcUser> DcUsers { get; set; }

    public virtual DbSet<DcUserAddress> DcUserAddresses { get; set; }

    public virtual DbSet<DcUserAddressType> DcUserAddressTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DcCity>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__DC_City__F2D21B764F774A4F");

            entity.ToTable("DC_City");

            entity.Property(e => e.CityName).HasMaxLength(50);

            entity.HasOne(d => d.State).WithMany(p => p.DcCities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_City__StateId__78359834");
        });

        modelBuilder.Entity<DcCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__DC_Count__10D1609F6F41461F");

            entity.ToTable("DC_Country");

            entity.Property(e => e.CountryName).HasMaxLength(50);
        });

        modelBuilder.Entity<DcState>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__DC_State__C3BA3B3AA854509C");

            entity.ToTable("DC_State");

            entity.Property(e => e.StateName).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.DcStates)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_State__Countr__1C481021");
        });

        modelBuilder.Entity<DcUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__DC_User__1788CC4CA968AAA4");

            entity.ToTable("DC_User");

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
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordResetToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordResetTokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DcUserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__DC_UserA__091C2AFB163CF40E");

            entity.ToTable("DC_UserAddress");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.AddressType).WithMany(p => p.DcUserAddresses)
                .HasForeignKey(d => d.AddressTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_UserAd__Addre__0B486CA8");

            entity.HasOne(d => d.City).WithMany(p => p.DcUserAddresses)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_UserAd__CityI__086BFFFD");

            entity.HasOne(d => d.Country).WithMany(p => p.DcUserAddresses)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_UserAd__Count__0A54486F");

            entity.HasOne(d => d.State).WithMany(p => p.DcUserAddresses)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_UserAd__State__09602436");

            entity.HasOne(d => d.User).WithMany(p => p.DcUserAddresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DC_UserAd__UserI__0C3C90E1");
        });

        modelBuilder.Entity<DcUserAddressType>(entity =>
        {
            entity.HasKey(e => e.AddressTypeId).HasName("PK__DC_UserA__8BF56C21410F0519");

            entity.ToTable("DC_UserAddressType");

            entity.HasIndex(e => e.AddressTypeName, "UQ__DC_UserA__4D15DD8828E908D0").IsUnique();

            entity.Property(e => e.AddressTypeName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
