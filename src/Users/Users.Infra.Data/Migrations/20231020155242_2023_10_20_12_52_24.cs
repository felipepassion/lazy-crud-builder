using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LazyCrud.Users.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class _2023_10_20_12_52_24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<long>(type: "bigint", nullable: false),
                    NeedResetPassword = table.Column<bool>(type: "bit", nullable: true),
                    NeedSendOnboardingMail = table.Column<bool>(type: "bit", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanUpdatePassword = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    RegisterDone = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitialPage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrivateProfile = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    RegisterDone = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContact_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    RegisterDone = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileList_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsersAggSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AutoSaveSettingsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAggSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersAggSettings_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCurrentAccessSelected",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserProfileId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCurrentAccessSelected", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCurrentAccessSelected_UserProfile_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserCurrentAccessSelected_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserProfileId = table.Column<int>(type: "int", nullable: false),
                    SystemPanelSubItemId = table.Column<int>(type: "int", nullable: true),
                    SystemPanelId = table.Column<int>(type: "int", nullable: true),
                    SystemPanelGroupId = table.Column<int>(type: "int", nullable: true),
                    IsSubItem = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsDirectLink = table.Column<bool>(type: "bit", nullable: false),
                    CanInsert = table.Column<bool>(type: "bit", nullable: false),
                    CanUpdate = table.Column<bool>(type: "bit", nullable: false),
                    CanList = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    RegisterDone = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileAccess_SystemPanelGroup_SystemPanelGroupId",
                        column: x => x.SystemPanelGroupId,
                        principalTable: "SystemPanelGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfileAccess_SystemPanel_SystemPanelId",
                        column: x => x.SystemPanelId,
                        principalTable: "SystemPanel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfileAccess_UserProfile_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactNumero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserContactId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactNumero", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNumero_UserContact_UserContactId",
                        column: x => x.UserContactId,
                        principalTable: "UserContact",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfileListUserProfile",
                columns: table => new
                {
                    AccessesListId = table.Column<int>(type: "int", nullable: false),
                    UserProfilesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileListUserProfile", x => new { x.AccessesListId, x.UserProfilesId });
                    table.ForeignKey(
                        name: "FK_UserProfileListUserProfile_UserProfileList_AccessesListId",
                        column: x => x.AccessesListId,
                        principalTable: "UserProfileList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileListUserProfile_UserProfile_UserProfilesId",
                        column: x => x.UserProfilesId,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumero_UserContactId",
                table: "ContactNumero",
                column: "UserContactId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrentAccessSelected_UserProfileId",
                table: "UserCurrentAccessSelected",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileAccess_SystemPanelGroupId",
                table: "UserProfileAccess",
                column: "SystemPanelGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileAccess_SystemPanelId",
                table: "UserProfileAccess",
                column: "SystemPanelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileAccess_UserProfileId",
                table: "UserProfileAccess",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileList_UserId",
                table: "UserProfileList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileListUserProfile_UserProfilesId",
                table: "UserProfileListUserProfile",
                column: "UserProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAggSettings_UserId",
                table: "UsersAggSettings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactNumero");

            migrationBuilder.DropTable(
                name: "UserCurrentAccessSelected");

            migrationBuilder.DropTable(
                name: "UserProfileAccess");

            migrationBuilder.DropTable(
                name: "UserProfileListUserProfile");

            migrationBuilder.DropTable(
                name: "UsersAggSettings");

            migrationBuilder.DropTable(
                name: "UserContact");

            migrationBuilder.DropTable(
                name: "UserProfileList");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
