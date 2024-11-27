using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orbit.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintFromPreferenceNameInPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "preference_name_UNIQUE",
                table: "post_preference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "preference_name_UNIQUE",
                table: "post_preference",
                column: "preference_name",
                unique: true);
        }
    }
}
