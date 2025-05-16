using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.data.Migrations
{
    /// <inheritdoc />
    public partial class addedUserToChatItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChatItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ChatItems_UserId",
                table: "ChatItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatItems_AspNetUsers_UserId",
                table: "ChatItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatItems_AspNetUsers_UserId",
                table: "ChatItems");

            migrationBuilder.DropIndex(
                name: "IX_ChatItems_UserId",
                table: "ChatItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatItems");
        }
    }
}
