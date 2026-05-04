using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Models
{
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
        public virtual DbSet<RoomEquipment> RoomEquipments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Username=nastya;Password=123;Port=5438;Database=trenirovka");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка таблицы booking
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

                entity.HasOne(d => d.BookingStatus)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookingStatusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("booking_booking_status_id_fkey");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("booking_room_id_fkey");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("booking_service_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("booking_user_id_fkey");
            });

            // Таблица booking_status
            modelBuilder.Entity<BookingStatus>(entity =>
            {
                entity.HasKey(e => e.BookingStatusId).HasName("booking_status_pkey");
                entity.ToTable("booking_status", "coworking");
                entity.Property(e => e.BookingStatusId).HasColumnName("booking_status_id");
                entity.Property(e => e.BookingStatusName)
                    .HasColumnType("character varying")
                    .HasColumnName("booking_status_name");
            });

            // Таблица equipment
            modelBuilder.Entity<Equipment>(entity =>
            {
                // Указываем EquipmentId как PK
                entity.HasKey(e => e.EquipmentId).HasName("equipment_pkey");
                entity.ToTable("equipment", "coworking");

                // Конфигурация свойств
                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
                entity.Property(e => e.EquipmentName)
                    .HasColumnType("character varying")
                    .HasColumnName("equipment_name");

                // Связь один-ко-многим с RoomEquipment
                entity.HasMany(e => e.RoomEquipments)     // Связь с RoomEquipment
                      .WithOne(re => re.Equipment)       // Связь с Equipment в RoomEquipment
                      .HasForeignKey(re => re.EquipmentId) // Указывает внешний ключ
                      .OnDelete(DeleteBehavior.Cascade) // Условие каскадного удаления
                      .HasConstraintName("fk_room_equipment_equipment"); // Обычно это имя FK, если нужно
            });

            // Таблица role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("role_pkey");
                entity.ToTable("role", "coworking");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.RoleName)
                    .HasColumnType("character varying")
                    .HasColumnName("role_name");
            });

            // Таблица room
            modelBuilder.Entity<Room>(entity =>
            {
                // Указываем RoomId как PK
                entity.HasKey(e => e.RoomId).HasName("room_pkey");
                entity.ToTable("room", "coworking");

                // Конфигурация свойств
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

                // Связь один-ко-многим с RoomType
                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.Rooms) // Ожидается: public List<Room> Rooms { get; set; } в RoomType
                    .HasForeignKey(d => d.RoomTypeId)
                    .OnDelete(DeleteBehavior.Cascade) // Условие каскадного удаления
                    .HasConstraintName("room_room_type_id_fkey"); // Имя FK

                // Связь один-ко-многим с RoomEquipment
                entity.HasMany(r => r.RoomEquipments)     // Связь с RoomEquipment
                      .WithOne(re => re.Room)           // Связь с Room в RoomEquipment
                      .HasForeignKey(re => re.RoomId)   // Указывает внешний ключ
                      .OnDelete(DeleteBehavior.Cascade) // Условие каскадного удаления
                      .HasConstraintName("fk_room_equipment_room"); // Имя FK

            });


            // Таблица room_type
            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.HasKey(e => e.RoomTypeId).HasName("room_type_pkey");
                entity.ToTable("room_type", "coworking");
                entity.Property(e => e.RoomTypeId).HasColumnName("room_type_id");
                entity.Property(e => e.RoomTypeName)
                    .HasColumnType("character varying")
                    .HasColumnName("room_type_name");
            });

            // Таблица service
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

            // Таблица user
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

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("user_role_id_fkey");
            });

            modelBuilder.Entity<RoomEquipment>(entity =>
            {
                entity.ToTable("room_equipment", "coworking");

                entity.HasKey(re => new { re.RoomId, re.EquipmentId })
                      .HasName("pk_room_equipment"); // Устанавливаем имя первичного ключа

                // Настройка свойств
                entity.Property(e => e.RoomId)
                      .HasColumnName("room_id");

                entity.Property(e => e.EquipmentId)
                      .HasColumnName("equipment_id");

                // Настройка внешних ключей

                // Связь с Room
                entity.HasOne(re => re.Room)
                      .WithMany(r => r.RoomEquipments) // Ожидается: public List<RoomEquipment> RoomEquipment { get; set; } в Room
                      .HasForeignKey(re => re.RoomId) // Связывается со столбцом "room_id"
                      .OnDelete(DeleteBehavior.Cascade) // Соответствует "on delete cascade"
                      .HasConstraintName("fk_room_equipment_room"); // Имя ограничения в БД

                // Связь с Equipment
                entity.HasOne(re => re.Equipment)
                      .WithMany(e => e.RoomEquipments) // Ожидается: public List<RoomEquipment> RoomEquipments { get; set; } в Equipment
                      .HasForeignKey(re => re.EquipmentId) // Связывается со столбцом "equipment_id"
                      .OnDelete(DeleteBehavior.Cascade) // Соответствует "on delete cascade"
                      .HasConstraintName("fk_room_equipment_equipment"); // Имя ограничения в БД
            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
