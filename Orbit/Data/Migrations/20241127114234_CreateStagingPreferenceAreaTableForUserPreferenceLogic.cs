using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orbit.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateStagingPreferenceAreaTableForUserPreferenceLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            CREATE TABLE preference_staging_area
            (
                user_id INT UNSIGNED,
                preference_id INT UNSIGNED,
                created_at DATETIME DEFAULT (NOW()),
                PRIMARY KEY (user_id, preference_id),
                FOREIGN KEY (user_id) REFERENCES user(user_id),
                FOREIGN KEY (preference_id) REFERENCES post_preference(preference_id)
            );
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE preference_staging_area");
        }
    }
}
