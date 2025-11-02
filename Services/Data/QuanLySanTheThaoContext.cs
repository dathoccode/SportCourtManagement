using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Models;

namespace SportCourtManagement.Services.Data;

public partial class QuanLySanTheThaoContext : DbContext
{
    public QuanLySanTheThaoContext()
    {
    }

    public QuanLySanTheThaoContext(DbContextOptions<QuanLySanTheThaoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TAccount> TAccounts { get; set; }

    public virtual DbSet<TBooking> TBookings { get; set; }

    public virtual DbSet<TBookingDetail> TBookingDetails { get; set; }

    public virtual DbSet<TCourt> TCourts { get; set; }

    public virtual DbSet<TPrice> TPrices { get; set; }

    public virtual DbSet<TRole> TRoles { get; set; }

    public virtual DbSet<TSlot> TSlots { get; set; }

    public virtual DbSet<TSport> TSports { get; set; }

    public virtual DbSet<TStatus> TStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DATPHUNG;Initial Catalog=QuanLySanTheThao;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tAccount__349DA58679366BE5");

            entity.ToTable("tAccount");

            entity.HasIndex(e => e.Email, "UQ__tAccount__A9D1053435F3EDE6").IsUnique();

            entity.Property(e => e.AccountId)
                .HasMaxLength(10)
                .HasColumnName("AccountID");
            entity.Property(e => e.AccImg).HasColumnType("image");
            entity.Property(e => e.AccName).HasMaxLength(50);
            entity.Property(e => e.AccPassword).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.RoleId)
                .HasMaxLength(10)
                .HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.TAccounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tAccount__RoleID__5CD6CB2B");
        });

        modelBuilder.Entity<TBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__tBooking__73951ACDA798E28A");

            entity.ToTable("tBooking");

            entity.Property(e => e.BookingId)
                .HasMaxLength(10)
                .HasColumnName("BookingID");
            entity.Property(e => e.AccountId)
                .HasMaxLength(10)
                .HasColumnName("AccountID");
            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.StatusId)
                .HasMaxLength(10)
                .HasColumnName("StatusID");

            entity.HasOne(d => d.Account).WithMany(p => p.TBookings)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tBooking__Accoun__619B8048");

            entity.HasOne(d => d.Status).WithMany(p => p.TBookings)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tBooking__Status__628FA481");
        });

        modelBuilder.Entity<TBookingDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__tBooking__135C314D5E8CA309");

            entity.ToTable("tBookingDetail");

            entity.Property(e => e.DetailId)
                .HasMaxLength(10)
                .HasColumnName("DetailID");
            entity.Property(e => e.BookingId)
                .HasMaxLength(10)
                .HasColumnName("BookingID");
            entity.Property(e => e.CourtId)
                .HasMaxLength(10)
                .HasColumnName("CourtID");
            entity.Property(e => e.SlotId)
                .HasMaxLength(10)
                .HasColumnName("SlotID");

            entity.HasOne(d => d.Booking).WithMany(p => p.TBookingDetails)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tBookingD__Booki__656C112C");

            entity.HasOne(d => d.Court).WithMany(p => p.TBookingDetails)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tBookingD__Court__66603565");
        });

        modelBuilder.Entity<TCourt>(entity =>
        {
            entity.HasKey(e => e.CourtId).HasName("PK__tCourt__C3A67CFAB2EBA3AC");

            entity.ToTable("tCourt");

            entity.Property(e => e.CourtId)
                .HasMaxLength(10)
                .HasColumnName("CourtID");
            entity.Property(e => e.Contact).HasMaxLength(11);
            entity.Property(e => e.CourtAddress).HasMaxLength(100);
            entity.Property(e => e.CourtName).HasMaxLength(50);
            entity.Property(e => e.Img).HasMaxLength(200);
            entity.Property(e => e.SportId)
                .HasMaxLength(10)
                .HasColumnName("SportID");

            entity.HasOne(d => d.Sport).WithMany(p => p.TCourts)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tCourt__SportID__4BAC3F29");

            entity.HasMany(d => d.Accounts).WithMany(p => p.Courts)
                .UsingEntity<Dictionary<string, object>>(
                    "TFavoriteCourt",
                    r => r.HasOne<TAccount>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__tFavorite__Accou__6A30C649"),
                    l => l.HasOne<TCourt>().WithMany()
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__tFavorite__Court__693CA210"),
                    j =>
                    {
                        j.HasKey("CourtId", "AccountId").HasName("PK__tFavorit__B0EFA6A2A711ED86");
                        j.ToTable("tFavoriteCourt");
                        j.IndexerProperty<string>("CourtId")
                            .HasMaxLength(10)
                            .HasColumnName("CourtID");
                        j.IndexerProperty<string>("AccountId")
                            .HasMaxLength(10)
                            .HasColumnName("AccountID");
                    });
        });

        modelBuilder.Entity<TPrice>(entity =>
        {
            entity.HasKey(e => new { e.CourtId, e.SlotId }).HasName("PK__tPrice__2307585ED5CA6DE4");

            entity.ToTable("tPrice");

            entity.Property(e => e.CourtId)
                .HasMaxLength(10)
                .HasColumnName("CourtID");
            entity.Property(e => e.SlotId)
                .HasMaxLength(10)
                .HasColumnName("SlotID");
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.Court).WithMany(p => p.TPrices)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tPrice__CourtID__5165187F");
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__tRole__8AFACE3A3F7C5CBA");

            entity.ToTable("tRole");

            entity.Property(e => e.RoleId)
                .HasMaxLength(10)
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        modelBuilder.Entity<TSlot>(entity =>
        {
            entity.HasKey(e => new { e.SlotId, e.CourtId }).HasName("PK__tSlot__B6282D80FCA6568B");

            entity.ToTable("tSlot");

            entity.Property(e => e.SlotId)
                .HasMaxLength(10)
                .HasColumnName("SlotID");
            entity.Property(e => e.CourtId)
                .HasMaxLength(10)
                .HasColumnName("CourtID");
            entity.Property(e => e.SlotName).HasMaxLength(20);

            entity.HasOne(d => d.Court).WithMany(p => p.TSlots)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tSlot__CourtID__4E88ABD4");
        });

        modelBuilder.Entity<TSport>(entity =>
        {
            entity.HasKey(e => e.SportId).HasName("PK__tSport__7A41AF1C93612B10");

            entity.ToTable("tSport");

            entity.Property(e => e.SportId)
                .HasMaxLength(10)
                .HasColumnName("SportID");
            entity.Property(e => e.SportName).HasMaxLength(50);
        });

        modelBuilder.Entity<TStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__tStatus__C8EE2043BC5819AD");

            entity.ToTable("tStatus");

            entity.Property(e => e.StatusId)
                .HasMaxLength(10)
                .HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
