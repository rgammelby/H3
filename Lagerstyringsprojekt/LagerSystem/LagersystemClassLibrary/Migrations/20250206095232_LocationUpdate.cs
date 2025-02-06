using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LagerstyringClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class LocationUpdate : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "telephone",
                table: "Users",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "Users",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "Users",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "status_type",
                table: "StatusTypes",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "type_name",
                table: "DeviceTypes",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "model",
                table: "DeviceOverview",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "activity_type",
                table: "ActivityTypes",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "id", "type_name" },
                values: new object[,]
                {
                    { 1, "Laptop" },
                    { 2, "Desktop" },
                    { 3, "Keyboard" },
                    { 4, "Monitor" },
                    { 5, "Mouse" },
                    { 6, "Server" },
                    { 7, "Router" },
                    { 8, "Switch" },
                    { 9, "Headset" },
                    { 10, "Microphone Set" },
                    { 11, "WebCam" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "id", "designation" },
                values: new object[,]
                {
                    { 1, "D.15" },
                    { 2, "D.16" },
                    { 3, "D.17" }
                });

            migrationBuilder.InsertData(
                table: "Cupboards",
                columns: new[] { "id", "designation", "room_id" },
                values: new object[,]
                {
                    { 1, "Cupboard 1", 1 },
                    { 2, "Cupboard 2", 1 },
                    { 3, "Cupboard 3", 1 },
                    { 4, "Cupboard 4", 1 },
                    { 5, "Cupboard 5", 1 },
                    { 6, "Cupboard 6", 1 },
                    { 7, "Cupboard 7", 2 },
                    { 8, "Cupboard 8", 2 },
                    { 9, "Cupboard 9", 2 },
                    { 10, "Cupboard 10", 2 },
                    { 11, "Cupboard 11", 2 },
                    { 12, "Cupboard 12", 2 },
                    { 13, "Cupboard 13", 3 },
                    { 14, "Cupboard 14", 3 },
                    { 15, "Cupboard 15", 3 },
                    { 16, "Cupboard 16", 3 },
                    { 17, "Cupboard 17", 3 },
                    { 18, "Cupboard 18", 3 }
                });

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

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cupboards",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "room_id",
                table: "Cupboards",
                newName: "Roomid");

            migrationBuilder.RenameIndex(
                name: "IX_Cupboards_room_id",
                table: "Cupboards",
                newName: "IX_Cupboards_Roomid");

            migrationBuilder.AlterColumn<string>(
                name: "telephone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "status_type",
                table: "StatusTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

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

            migrationBuilder.AlterColumn<string>(
                name: "type_name",
                table: "DeviceTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "model",
                table: "DeviceOverview",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "activity_type",
                table: "ActivityTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

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
