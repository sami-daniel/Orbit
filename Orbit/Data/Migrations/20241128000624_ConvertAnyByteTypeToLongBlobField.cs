using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orbit.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConvertAnyByteTypeToLongBlobField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "user_profile_image_byte_type",
                table: "user",
                type: "LONGBLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "user_profile_banner_image_byte_type",
                table: "user",
                type: "LONGBLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "post_video_byte_type",
                table: "post",
                type: "LONGBLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "post_image_byte_type",
                table: "post",
                type: "LONGBLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "user_profile_image_byte_type",
                table: "user",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "LONGBLOB",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "user_profile_banner_image_byte_type",
                table: "user",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "LONGBLOB",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "post_video_byte_type",
                table: "post",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "LONGBLOB",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "post_image_byte_type",
                table: "post",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "LONGBLOB",
                oldNullable: true);
        }
    }
}
