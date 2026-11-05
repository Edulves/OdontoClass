using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdontoClass.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationBetweenWorkSpaceAndSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "schedule_user");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "schedule",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "schedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "work_space_id",
                table: "schedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_schedule_created_by_user_id",
                table: "schedule",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_schedule_work_space_id",
                table: "schedule",
                column: "work_space_id");

            migrationBuilder.AddForeignKey(
                name: "fk_schedule_users_created_by_user_id",
                table: "schedule",
                column: "created_by_user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_schedule_work_space_work_space_id",
                table: "schedule",
                column: "work_space_id",
                principalTable: "work_space",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_schedule_users_created_by_user_id",
                table: "schedule");

            migrationBuilder.DropForeignKey(
                name: "fk_schedule_work_space_work_space_id",
                table: "schedule");

            migrationBuilder.DropIndex(
                name: "ix_schedule_created_by_user_id",
                table: "schedule");

            migrationBuilder.DropIndex(
                name: "ix_schedule_work_space_id",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "work_space_id",
                table: "schedule");

            migrationBuilder.CreateTable(
                name: "schedule_user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    schedule_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schedule_user", x => new { x.user_id, x.schedule_id });
                    table.ForeignKey(
                        name: "fk_schedule_user_schedule_schedule_id",
                        column: x => x.schedule_id,
                        principalTable: "schedule",
                        principalColumn: "schedule_id");
                    table.ForeignKey(
                        name: "fk_schedule_user_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_schedule_user_schedule_id",
                table: "schedule_user",
                column: "schedule_id");
        }
    }
}
