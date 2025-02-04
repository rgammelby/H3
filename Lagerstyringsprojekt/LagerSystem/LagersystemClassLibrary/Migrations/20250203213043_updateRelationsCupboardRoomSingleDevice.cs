using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerstyringClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationsCupboardRoomSingleDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cupboards_Rooms_Roomid",
                table: "Cupboards");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleDevices_Rooms_Locationid",
                table: "SingleDevices");

            migrationBuilder.DropIndex(
                name: "IX_SingleDevices_Locationid",
                table: "SingleDevices");

            migrationBuilder.DropColumn(
                name: "Locationid",
                table: "SingleDevices");

            migrationBuilder.DropColumn(
                name: "location_cupboard_id",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "Roomid",
                table: "Cupboards",
                newName: "room_id");

            migrationBuilder.RenameIndex(
                name: "IX_Cupboards_Roomid",
                table: "Cupboards",
                newName: "IX_Cupboards_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_SingleDevices_location",
                table: "SingleDevices",
                column: "location");

            migrationBuilder.AddForeignKey(
                name: "FK_Cupboards_Rooms_room_id",
                table: "Cupboards",
                column: "room_id",
                principalTable: "Rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleDevices_Cupboards_location",
                table: "SingleDevices",
                column: "location",
                principalTable: "Cupboards",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cupboards_Rooms_room_id",
                table: "Cupboards");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleDevices_Cupboards_location",
                table: "SingleDevices");

            migrationBuilder.DropIndex(
                name: "IX_SingleDevices_location",
                table: "SingleDevices");

            migrationBuilder.RenameColumn(
                name: "room_id",
                table: "Cupboards",
                newName: "Roomid");

            migrationBuilder.RenameIndex(
                name: "IX_Cupboards_room_id",
                table: "Cupboards",
                newName: "IX_Cupboards_Roomid");

            migrationBuilder.AddColumn<int>(
                name: "Locationid",
                table: "SingleDevices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "location_cupboard_id",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SingleDevices_Locationid",
                table: "SingleDevices",
                column: "Locationid");

            migrationBuilder.AddForeignKey(
                name: "FK_Cupboards_Rooms_Roomid",
                table: "Cupboards",
                column: "Roomid",
                principalTable: "Rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleDevices_Rooms_Locationid",
                table: "SingleDevices",
                column: "Locationid",
                principalTable: "Rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
