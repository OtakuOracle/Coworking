using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Coworking.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoomEquipmentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "coworking");

            migrationBuilder.CreateTable(
                name: "booking_status",
                schema: "coworking",
                columns: table => new
                {
                    booking_status_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_status_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("booking_status_pkey", x => x.booking_status_id);
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                schema: "coworking",
                columns: table => new
                {
                    equipment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equipment_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("equipment_pkey", x => x.equipment_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "coworking",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "room_type",
                schema: "coworking",
                columns: table => new
                {
                    room_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_type_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("room_type_pkey", x => x.room_type_id);
                });

            migrationBuilder.CreateTable(
                name: "service",
                schema: "coworking",
                columns: table => new
                {
                    service_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_name = table.Column<string>(type: "character varying", nullable: true),
                    cost = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("service_pkey", x => x.service_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "coworking",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    full_name = table.Column<string>(type: "character varying", nullable: true),
                    email = table.Column<string>(type: "character varying", nullable: true),
                    phone = table.Column<string>(type: "character varying", nullable: true),
                    password = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "user_role_id_fkey",
                        column: x => x.role_id,
                        principalSchema: "coworking",
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room",
                schema: "coworking",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_name = table.Column<string>(type: "character varying", nullable: true),
                    room_type_id = table.Column<int>(type: "integer", nullable: true),
                    capacity = table.Column<int>(type: "integer", nullable: true),
                    cost = table.Column<int>(type: "integer", nullable: true),
                    min_time = table.Column<int>(type: "integer", nullable: true),
                    discount = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    rating = table.Column<decimal>(type: "numeric", nullable: true),
                    photo = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("room_pkey", x => x.room_id);
                    table.ForeignKey(
                        name: "room_room_type_id_fkey",
                        column: x => x.room_type_id,
                        principalSchema: "coworking",
                        principalTable: "room_type",
                        principalColumn: "room_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                schema: "coworking",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    room_id = table.Column<int>(type: "integer", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: true),
                    time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    booking_status_id = table.Column<int>(type: "integer", nullable: true),
                    service_id = table.Column<int>(type: "integer", nullable: true),
                    date_create = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("booking_pkey", x => x.booking_id);
                    table.ForeignKey(
                        name: "booking_booking_status_id_fkey",
                        column: x => x.booking_status_id,
                        principalSchema: "coworking",
                        principalTable: "booking_status",
                        principalColumn: "booking_status_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "booking_room_id_fkey",
                        column: x => x.room_id,
                        principalSchema: "coworking",
                        principalTable: "room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "booking_service_id_fkey",
                        column: x => x.service_id,
                        principalSchema: "coworking",
                        principalTable: "service",
                        principalColumn: "service_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "booking_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "coworking",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentRoom",
                schema: "coworking",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "integer", nullable: false),
                    RoomsRoomId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentRoom", x => new { x.EquipmentId, x.RoomsRoomId });
                    table.ForeignKey(
                        name: "FK_EquipmentRoom_equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalSchema: "coworking",
                        principalTable: "equipment",
                        principalColumn: "equipment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentRoom_room_RoomsRoomId",
                        column: x => x.RoomsRoomId,
                        principalSchema: "coworking",
                        principalTable: "room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room_equipment",
                schema: "coworking",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    equipment_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_equipment", x => new { x.room_id, x.equipment_id });
                    table.ForeignKey(
                        name: "fk_room_equipment_equipment",
                        column: x => x.equipment_id,
                        principalSchema: "coworking",
                        principalTable: "equipment",
                        principalColumn: "equipment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_room_equipment_room",
                        column: x => x.room_id,
                        principalSchema: "coworking",
                        principalTable: "room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_booking_booking_status_id",
                schema: "coworking",
                table: "booking",
                column: "booking_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_room_id",
                schema: "coworking",
                table: "booking",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_service_id",
                schema: "coworking",
                table: "booking",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_user_id",
                schema: "coworking",
                table: "booking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentRoom_RoomsRoomId",
                schema: "coworking",
                table: "EquipmentRoom",
                column: "RoomsRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_room_room_type_id",
                schema: "coworking",
                table: "room",
                column: "room_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_equipment_equipment_id",
                schema: "coworking",
                table: "room_equipment",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_id",
                schema: "coworking",
                table: "user",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booking",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "EquipmentRoom",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "room_equipment",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "booking_status",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "service",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "user",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "equipment",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "room",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "role",
                schema: "coworking");

            migrationBuilder.DropTable(
                name: "room_type",
                schema: "coworking");
        }
    }
}
