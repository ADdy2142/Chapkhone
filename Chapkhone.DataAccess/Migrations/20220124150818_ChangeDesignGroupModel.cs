using Microsoft.EntityFrameworkCore.Migrations;

namespace Chapkhone.DataAccess.Migrations
{
    public partial class ChangeDesignGroupModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesignPrice",
                table: "DesignGroups",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<byte>(
                name: "UnitPriceType",
                table: "DesignGroups",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPriceType",
                table: "DesignGroups");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "DesignGroups",
                newName: "DesignPrice");
        }
    }
}
