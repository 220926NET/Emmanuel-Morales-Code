using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projectOneApi_v1.Migrations
{
    public partial class DeleteEmployeeRelationToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employees_Employeeid",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Employeeid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Employeeid",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Employeeid",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Employeeid",
                table: "Users",
                column: "Employeeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employees_Employeeid",
                table: "Users",
                column: "Employeeid",
                principalTable: "Employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
