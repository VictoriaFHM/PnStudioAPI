using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PnStudioAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calculations_Projects_ProjectId",
                table: "Calculations");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Calculations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Calculations_Projects_ProjectId",
                table: "Calculations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calculations_Projects_ProjectId",
                table: "Calculations");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Calculations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Calculations_Projects_ProjectId",
                table: "Calculations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
