using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Models;

public partial class TrenirovkaContext : DbContext
{
    public TrenirovkaContext()
    {
    }

    public TrenirovkaContext(DbContextOptions<TrenirovkaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingStatus> BookingStatuses { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Username=nastya;Password=123;Port=5438; Database=trenirovka");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("booking_pkey");

            entity.ToTable("booking", "coworking");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.BookingStatusId).HasColumnName("booking_status_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DateCreate).HasColumnName("date_create");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.BookingStatus).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BookingStatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("booking_booking_status_id_fkey");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("booking_room_id_fkey");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("booking_service_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("booking_user_id_fkey");
        });

        modelBuilder.Entity<BookingStatus>(entity =>
        {
            entity.HasKey(e => e.BookingStatusId).HasName("booking_status_pkey");

            entity.ToTable("booking_status", "coworking");

            entity.Property(e => e.BookingStatusId).HasColumnName("booking_status_id");
            entity.Property(e => e.BookingStatusName)
                .HasColumnType("character varying")
                .HasColumnName("booking_status_name");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("equipment_pkey");

            entity.ToTable("equipment", "coworking");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.EquipmentName)
                .HasColumnType("character varying")
                .HasColumnName("equipment_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role", "coworking");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasColumnType("character varying")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("room_pkey");

            entity.ToTable("room", "coworking");

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.MinTime).HasColumnName("min_time");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RoomName)
                .HasColumnType("character varying")
                .HasColumnName("room_name");
            entity.Property(e => e.RoomTypeId).HasColumnName("room_type_id");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("room_room_type_id_fkey");

            entity.HasMany(d => d.Equipment).WithMany(p => p.Rooms)
                .UsingEntity<Dictionary<string, object>>(
                    "RoomEquipment",
                    r => r.HasOne<Equipment>().WithMany()
                        .HasForeignKey("EquipmentId")
                        .HasConstraintName("fk_room_equipment_equipment"),
                    l => l.HasOne<Room>().WithMany()
                        .HasForeignKey("RoomId")
                        .HasConstraintName("fk_room_equipment_room"),
                    j =>
                    {
                        j.HasKey("RoomId", "EquipmentId").HasName("pk_room_equipment");
                        j.ToTable("room_equipment", "coworking");
                        j.IndexerProperty<int>("RoomId").HasColumnName("room_id");
                        j.IndexerProperty<int>("EquipmentId").HasColumnName("equipment_id");
                    });
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.RoomTypeId).HasName("room_type_pkey");

            entity.ToTable("room_type", "coworking");

            entity.Property(e => e.RoomTypeId).HasColumnName("room_type_id");
            entity.Property(e => e.RoomTypeName)
                .HasColumnType("character varying")
                .HasColumnName("room_type_name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("service_pkey");

            entity.ToTable("service", "coworking");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.ServiceName)
                .HasColumnType("character varying")
                .HasColumnName("service_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user", "coworking");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasColumnType("character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("user_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
