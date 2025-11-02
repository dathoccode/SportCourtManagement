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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-C8E5HODE;Initial Catalog=QuanLySanTheThao;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
