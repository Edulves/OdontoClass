using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OdontoClass.Migrations
{
    /// <inheritdoc />
    public partial class Workspacetabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_schedule_users_schedules_schedule_id",
                table: "schedule_users");

            migrationBuilder.DropForeignKey(
                name: "fk_schedule_users_users_user_id",
                table: "schedule_users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_schedules",
                table: "schedules");

            migrationBuilder.DropPrimaryKey(
                name: "pk_schedule_users",
                table: "schedule_users");

            migrationBuilder.RenameTable(
                name: "schedules",
                newName: "schedule");

            migrationBuilder.RenameTable(
                name: "schedule_users",
                newName: "schedule_user");

            migrationBuilder.RenameIndex(
                name: "ix_schedule_users_schedule_id",
                table: "schedule_user",
                newName: "ix_schedule_user_schedule_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_of_schedule",
                table: "schedule",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddPrimaryKey(
                name: "pk_schedule",
                table: "schedule",
                column: "schedule_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_schedule_user",
                table: "schedule_user",
                columns: new[] { "user_id", "schedule_id" });

            migrationBuilder.CreateTable(
                name: "work_space",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_space", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_space_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "work_space_user",
                columns: table => new
                {
                    work_space_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_space_user", x => new { x.work_space_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_work_space_user_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_work_space_user_work_space_work_space_id",
                        column: x => x.work_space_id,
                        principalTable: "work_space",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_work_space_created_by_user_id",
                table: "work_space",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_space_user_user_id",
                table: "work_space_user",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_schedule_user_schedule_schedule_id",
                table: "schedule_user",
                column: "schedule_id",
                principalTable: "schedule",
                principalColumn: "schedule_id");

            migrationBuilder.AddForeignKey(
                name: "fk_schedule_user_users_user_id",
                table: "schedule_user",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_schedule_user_schedule_schedule_id",
                table: "schedule_user");

            migrationBuilder.DropForeignKey(
                name: "fk_schedule_user_users_user_id",
                table: "schedule_user");

            migrationBuilder.DropTable(
                name: "work_space_user");

            migrationBuilder.DropTable(
                name: "work_space");

            migrationBuilder.DropPrimaryKey(
                name: "pk_schedule_user",
                table: "schedule_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_schedule",
                table: "schedule");

            migrationBuilder.RenameTable(
                name: "schedule_user",
                newName: "schedule_users");

            migrationBuilder.RenameTable(
                name: "schedule",
                newName: "schedules");

            migrationBuilder.RenameIndex(
                name: "ix_schedule_user_schedule_id",
                table: "schedule_users",
                newName: "ix_schedule_users_schedule_id");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "date_of_schedule",
                table: "schedules",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "pk_schedule_users",
                table: "schedule_users",
                columns: new[] { "user_id", "schedule_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_schedules",
                table: "schedules",
                column: "schedule_id");

            migrationBuilder.AddForeignKey(
                name: "fk_schedule_users_schedules_schedule_id",
                table: "schedule_users",
                column: "schedule_id",
                principalTable: "schedules",
                principalColumn: "schedule_id");

            migrationBuilder.AddForeignKey(
                name: "fk_schedule_users_users_user_id",
                table: "schedule_users",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id");
        }
    }
}
