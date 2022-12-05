using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoravelMailingServiceTest.Migrations
{
    public partial class changestodb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "PendingMails");

            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "PendingMails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PendingMails");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PendingMails");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "PendingMails");

            migrationBuilder.CreateTable(
                name: "MailProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailProducts_PendingMails_MailId",
                        column: x => x.MailId,
                        principalTable: "PendingMails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailProducts_MailId",
                table: "MailProducts",
                column: "MailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailProducts");

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "PendingMails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "PendingMails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PendingMails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PendingMails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "PendingMails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
