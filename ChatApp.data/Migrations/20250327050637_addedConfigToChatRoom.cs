using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.data.Migrations
{
    /// <inheritdoc />
    public partial class addedConfigToChatRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChatRooms_ChatRoomEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChatRooms_ChatRoomEntityId1",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatItems_ChatRooms_ChatRoomEntityId",
                table: "ChatItems");

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

            migrationBuilder.CreateTable(
                name: "ChatRoomActiveMembers",
                columns: table => new
                {
                    ActiveMembersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChatRoomEntity1Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomActiveMembers", x => new { x.ActiveMembersId, x.ChatRoomEntity1Id });
                    table.ForeignKey(
                        name: "FK_ChatRoomActiveMembers_AspNetUsers_ActiveMembersId",
                        column: x => x.ActiveMembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomActiveMembers_ChatRooms_ChatRoomEntity1Id",
                        column: x => x.ChatRoomEntity1Id,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatRoomMembers",
                columns: table => new
                {
                    ChatRoomEntityId = table.Column<int>(type: "int", nullable: false),
                    RegisteredMembersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomMembers", x => new { x.ChatRoomEntityId, x.RegisteredMembersId });
                    table.ForeignKey(
                        name: "FK_ChatRoomMembers_AspNetUsers_RegisteredMembersId",
                        column: x => x.RegisteredMembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomMembers_ChatRooms_ChatRoomEntityId",
                        column: x => x.ChatRoomEntityId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomActiveMembers_ChatRoomEntity1Id",
                table: "ChatRoomActiveMembers",
                column: "ChatRoomEntity1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomMembers_RegisteredMembersId",
                table: "ChatRoomMembers",
                column: "RegisteredMembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatItems_ChatRooms_ChatRoomEntityId",
                table: "ChatItems",
                column: "ChatRoomEntityId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatItems_ChatRooms_ChatRoomEntityId",
                table: "ChatItems");

            migrationBuilder.DropTable(
                name: "ChatRoomActiveMembers");

            migrationBuilder.DropTable(
                name: "ChatRoomMembers");

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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatRoomEntityId",
                table: "AspNetUsers",
                column: "ChatRoomEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatRoomEntityId1",
                table: "AspNetUsers",
                column: "ChatRoomEntityId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ChatItems_ChatRooms_ChatRoomEntityId",
                table: "ChatItems",
                column: "ChatRoomEntityId",
                principalTable: "ChatRooms",
                principalColumn: "Id");
        }
    }
}
