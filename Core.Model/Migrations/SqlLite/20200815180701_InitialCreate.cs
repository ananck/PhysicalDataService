using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Model.Migrations.SqlLite
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationRecords",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastKey = table.Column<string>(nullable: true),
                    LastIndex = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationRecords", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RepeatingTables",
                columns: table => new
                {
                    DetailID = table.Column<string>(maxLength: 100, nullable: false),
                    ItemInfo = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepeatingTables", x => x.DetailID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationRecords");

            migrationBuilder.DropTable(
                name: "RepeatingTables");
        }
    }
}
