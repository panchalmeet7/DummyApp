using System;
using System.Collections.Generic;
using DummyApp.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DummyApp.Entities.Data;

public partial class DummyAppContext : DbContext
{
    public DummyAppContext()
    {
    }

    public DummyAppContext(DbContextOptions<DummyAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PCT38\\SQL2019;Database=DummyApp;User Id=sa;Password=Tatva@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("Employee_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.EmployeeDepartment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Employee_Department");
            entity.Property(e => e.EmployeeEmail)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Employee_Email");
            entity.Property(e => e.EmployeeFirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Employee_FirstName");
            entity.Property(e => e.EmployeeLastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Employee_LastName");
            entity.Property(e => e.EmployeeRole)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Employee_Role");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
