using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Data.Models;

namespace SportCourtManagement.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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
        => optionsBuilder.UseSqlServer("Server=ADMIN\\SQLEXPRESS01;Database=QuanLySanTheThao;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tAccount__349DA586B182A2DE");

            entity.HasOne(d => d.Role).WithMany(p => p.TAccounts).HasConstraintName("FK__tAccount__RoleID__534D60F1");
        });

        modelBuilder.Entity<TBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__tBooking__73951ACD3ECB3D67");

            entity.HasOne(d => d.Account).WithMany(p => p.TBookings).HasConstraintName("FK__tBooking__Accoun__5812160E");

            entity.HasOne(d => d.Status).WithMany(p => p.TBookings).HasConstraintName("FK__tBooking__Status__59063A47");
        });

        modelBuilder.Entity<TBookingDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__tBooking__135C314DED5D126B");

            entity.HasOne(d => d.Booking).WithMany(p => p.TBookingDetails).HasConstraintName("FK__tBookingD__Booki__5BE2A6F2");

            entity.HasOne(d => d.Court).WithMany(p => p.TBookingDetails).HasConstraintName("FK__tBookingD__Court__5CD6CB2B");
        });

        modelBuilder.Entity<TCourt>(entity =>
        {
            entity.HasKey(e => e.CourtId).HasName("PK__tCourt__C3A67CFAA3E7F6D8");

            entity.HasMany(d => d.Accounts).WithMany(p => p.Courts)
                .UsingEntity<Dictionary<string, object>>(
                    "TFavoriteCourt",
                    r => r.HasOne<TAccount>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__tFavorite__Accou__60A75C0F"),
                    l => l.HasOne<TCourt>().WithMany()
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__tFavorite__Court__5FB337D6"),
                    j =>
                    {
                        j.HasKey("CourtId", "AccountId").HasName("PK__tFavorit__B0EFA6A2B49AC7C8");
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
            entity.HasKey(e => new { e.CourtId, e.SlotId }).HasName("PK__tPrice__2307585E75F33268");

            entity.HasOne(d => d.Court).WithMany(p => p.TPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tPrice__CourtID__4E88ABD4");
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__tRole__8AFACE3A3F515A3F");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
        });

        modelBuilder.Entity<TSlot>(entity =>
        {
            entity.HasKey(e => new { e.SlotId, e.CourtId }).HasName("PK__tSlot__B6282D80690E04AF");

            entity.HasOne(d => d.Court).WithMany(p => p.TSlots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tSlot__CourtID__4BAC3F29");
        });

        modelBuilder.Entity<TStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__tStatus__C8EE204309CA3AD4");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
