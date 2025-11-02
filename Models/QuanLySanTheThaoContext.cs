using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Models;

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

    public virtual DbSet<TStatus> TStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ADMIN\\SQLEXPRESS01;Initial Catalog=QuanLySanTheThao;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tAccount__349DA5863459BF5F");

            entity.ToTable("tAccount");

            entity.HasIndex(e => e.Email, "UQ__tAccount__A9D105345163A25A").IsUnique();

            entity.Property(e => e.AccountId)
                .HasMaxLength(10)
                .HasColumnName("AccountID");
            entity.Property(e => e.AccImg).HasColumnType("image");
            entity.Property(e => e.AccName).HasMaxLength(50);
            entity.Property(e => e.AccPassword).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.TAccounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__tAccount__RoleID__5441852A");
        });

        modelBuilder.Entity<TBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__tBooking__73951ACDF528DCB8");

            entity.ToTable("tBooking");

            entity.Property(e => e.BookingId)
                .HasMaxLength(10)
                .HasColumnName("BookingID");
            entity.Property(e => e.AccountId)
                .HasMaxLength(10)
                .HasColumnName("AccountID");
            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");

            entity.HasOne(d => d.Account).WithMany(p => p.TBookings)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__tBooking__Accoun__59063A47");

            entity.HasOne(d => d.Status).WithMany(p => p.TBookings)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__tBooking__Status__59FA5E80");
        });

        modelBuilder.Entity<TBookingDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__tBooking__135C314DBF3CAE06");

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
                .HasConstraintName("FK__tBookingD__Booki__5CD6CB2B");

            entity.HasOne(d => d.Court).WithMany(p => p.TBookingDetails)
                .HasForeignKey(d => d.CourtId)
                .HasConstraintName("FK__tBookingD__Court__5DCAEF64");
        });

        modelBuilder.Entity<TCourt>(entity =>
        {
            entity.HasKey(e => e.CourtId).HasName("PK__tCourt__C3A67CFA46C44B5B");

            entity.ToTable("tCourt");

            entity.Property(e => e.CourtId)
                .HasMaxLength(10)
                .HasColumnName("CourtID");
            entity.Property(e => e.Contact).HasMaxLength(11);
            entity.Property(e => e.CourtAddress).HasMaxLength(50);
            entity.Property(e => e.CourtName).HasMaxLength(50);
            entity.Property(e => e.Img).HasMaxLength(50);

            entity.HasMany(d => d.Accounts).WithMany(p => p.Courts)
                .UsingEntity<Dictionary<string, object>>(
                    "TFavoriteCourt",
                    r => r.HasOne<TAccount>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__tFavorite__Accou__619B8048"),
                    l => l.HasOne<TCourt>().WithMany()
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__tFavorite__Court__60A75C0F"),
                    j =>
                    {
                        j.HasKey("CourtId", "AccountId").HasName("PK__tFavorit__B0EFA6A2BB89F1CE");
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
            entity.HasKey(e => new { e.CourtId, e.SlotId }).HasName("PK__tPrice__2307585E22BACA8E");

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
                .HasConstraintName("FK__tPrice__CourtID__4E88ABD4");
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__tRole__8AFACE3A00E31B02");

            entity.ToTable("tRole");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        modelBuilder.Entity<TSlot>(entity =>
        {
            entity.HasKey(e => new { e.SlotId, e.CourtId }).HasName("PK__tSlot__B6282D80769C3AA3");

            entity.ToTable("tSlot");

            entity.Property(e => e.SlotId)
                .HasMaxLength(10)
                .HasColumnName("SlotID");
            entity.Property(e => e.CourtId)
                .HasMaxLength(10)
                .HasColumnName("CourtID");
            entity.Property(e => e.SlotType).HasMaxLength(100);

            entity.HasOne(d => d.Court).WithMany(p => p.TSlots)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tSlot__CourtID__4BAC3F29");
        });

        modelBuilder.Entity<TStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__tStatus__C8EE204322E72405");

            entity.ToTable("tStatus");

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
