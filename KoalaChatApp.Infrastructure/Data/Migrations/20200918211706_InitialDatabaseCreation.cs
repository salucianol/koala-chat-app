using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KoalaChatApp.Infrastructure.Data.Migrations
{
    public partial class InitialDatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MaxUsersAllowed = table.Column<short>(nullable: false),
                    MaxCharactersCount = table.Column<short>(nullable: false),
                    MaxMessagesCount = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessageText",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChatRoomId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    MessageType = table.Column<int>(nullable: false),
                    SentDate = table.Column<DateTimeOffset>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessageText", x => new { x.ChatRoomId, x.Id });
                    table.ForeignKey(
                        name: "FK_ChatMessageText_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessageText");

            migrationBuilder.DropTable(
                name: "ChatRooms");
        }
    }
}
