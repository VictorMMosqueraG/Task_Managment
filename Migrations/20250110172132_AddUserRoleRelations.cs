using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_managment.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Email",
                table: "User",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Email",
                table: "User");
        }
    }
}
