using Microsoft.EntityFrameworkCore.Migrations;

namespace Chapkhone.DataAccess.Migrations
{
    public partial class ChangeSpecificationOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "SpecificationOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "SpecificationOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "SpecificationOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "SpecificationOrders");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "SpecificationOrders");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "SpecificationOrders");
        }
    }
}
