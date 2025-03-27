using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.data.Migrations
{
    /// <inheritdoc />
    public partial class addedChatIdentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatRoomEntityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChatRoomEntityId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChatRoomEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatItems_ChatRooms_ChatRoomEntityId",
                        column: x => x.ChatRoomEntityId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatRoomEntityId",
                table: "AspNetUsers",
                column: "ChatRoomEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatRoomEntityId1",
                table: "AspNetUsers",
                column: "ChatRoomEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChatItems_ChatRoomEntityId",
                table: "ChatItems",
                column: "ChatRoomEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ChatRooms_ChatRoomEntityId",
                table: "AspNetUsers",
                column: "ChatRoomEntityId",
                principalTable: "ChatRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ChatRooms_ChatRoomEntityId1",
                table: "AspNetUsers",
                column: "ChatRoomEntityId1",
                principalTable: "ChatRooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChatRooms_ChatRoomEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChatRooms_ChatRoomEntityId1",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ChatItems");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChatRoomEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChatRoomEntityId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChatRoomEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChatRoomEntityId1",
                table: "AspNetUsers");
        }
    }
}
