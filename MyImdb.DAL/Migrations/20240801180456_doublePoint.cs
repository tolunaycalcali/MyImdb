using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyImdb.DAL.Migrations
{
    /// <inheritdoc />
    public partial class doublePoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Point",
                table: "Movie_Comment",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Point",
                table: "Movie_Comment",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
