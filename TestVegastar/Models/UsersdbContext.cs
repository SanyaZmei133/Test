using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestVegastar.Models;

public partial class UsersdbContext : DbContext
{
    public UsersdbContext()
    {
    }

    public UsersdbContext(DbContextOptions<UsersdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usergroup> Usergroups { get; set; }

    public virtual DbSet<Userstate> Userstates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Createddate).HasColumnName("createddate");
            entity.Property(e => e.Login)
                .HasMaxLength(30)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .HasColumnName("password");
            entity.Property(e => e.Usergroupid).HasColumnName("usergroupid");
            entity.Property(e => e.Userstateid).HasColumnName("userstateid");

            entity.HasOne(d => d.Usergroup).WithMany(p => p.Users)
                .HasForeignKey(d => d.Usergroupid)
                .HasConstraintName("users_usergroupid_fkey");

            entity.HasOne(d => d.Userstate).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userstateid)
                .HasConstraintName("users_userstateid_fkey");
        });

        modelBuilder.Entity<Usergroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usergroup_pkey");

            entity.ToTable("usergroup");

            entity.Property(e => e.Id).HasColumnName("usergroupid");
            entity.Property(e => e.Code)
                .HasMaxLength(8)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(64)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Userstate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userstate_pkey");

            entity.ToTable("userstate");

            entity.Property(e => e.Id).HasColumnName("userstateid");
            entity.Property(e => e.Code)
                .HasMaxLength(8)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(64)
                .HasColumnName("description");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
