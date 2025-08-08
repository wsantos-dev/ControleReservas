using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleReservas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmailConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EncryptedApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfigurations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailConfigurations");
        }
    }
}
