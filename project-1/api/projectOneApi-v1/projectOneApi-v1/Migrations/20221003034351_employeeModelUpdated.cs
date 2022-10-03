using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projectOneApi_v1.Migrations
{
    public partial class employeeModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_userid",
                table: "Employees",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_userid",
                table: "Employees",
                column: "userid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_userid",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_userid",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "Employees");
        }
    }
}
