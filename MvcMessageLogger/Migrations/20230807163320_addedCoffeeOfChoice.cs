using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcMessageLogger.Migrations
{
    /// <inheritdoc />
    public partial class addedCoffeeOfChoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "coffee_of_choice",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coffee_of_choice",
                table: "users");
        }
    }
}
