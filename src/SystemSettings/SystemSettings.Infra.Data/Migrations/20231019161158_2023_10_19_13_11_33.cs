using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LazyCrud.SystemSettings.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class _2023_10_19_13_11_33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargaTabela",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInitialized = table.Column<bool>(type: "bit", nullable: false),
                    ArquivoCSV = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_CargaTabela", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPanel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_SystemPanel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPanelGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_SystemPanelGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettingsAggSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AutoSaveSettingsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettingsAggSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPanelSubItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemPanelSubItemId = table.Column<int>(type: "int", nullable: true),
                    SystemPanelId = table.Column<int>(type: "int", nullable: false),
                    IsSubItem = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    RegisterDone = table.Column<bool>(type: "bit", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkDireto = table.Column<bool>(type: "bit", nullable: false),
                    ActionButton = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPanelSubItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemPanelSubItem_SystemPanelSubItem_SystemPanelSubItemId",
                        column: x => x.SystemPanelSubItemId,
                        principalTable: "SystemPanelSubItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemPanelSubItem_SystemPanel_SystemPanelId",
                        column: x => x.SystemPanelId,
                        principalTable: "SystemPanel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemPanelGroupSystemPanel",
                columns: table => new
                {
                    GroupOfMenusId = table.Column<int>(type: "int", nullable: false),
                    SubItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPanelGroupSystemPanel", x => new { x.GroupOfMenusId, x.SubItemsId });
                    table.ForeignKey(
                        name: "FK_SystemPanelGroupSystemPanel_SystemPanelGroup_GroupOfMenusId",
                        column: x => x.GroupOfMenusId,
                        principalTable: "SystemPanelGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemPanelGroupSystemPanel_SystemPanel_SubItemsId",
                        column: x => x.SubItemsId,
                        principalTable: "SystemPanel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemPanelGroupSystemPanel_SubItemsId",
                table: "SystemPanelGroupSystemPanel",
                column: "SubItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPanelSubItem_SystemPanelId",
                table: "SystemPanelSubItem",
                column: "SystemPanelId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPanelSubItem_SystemPanelSubItemId",
                table: "SystemPanelSubItem",
                column: "SystemPanelSubItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargaTabela");

            migrationBuilder.DropTable(
                name: "SystemPanelGroupSystemPanel");

            migrationBuilder.DropTable(
                name: "SystemPanelSubItem");

            migrationBuilder.DropTable(
                name: "SystemSettingsAggSettings");

            migrationBuilder.DropTable(
                name: "SystemPanelGroup");

            migrationBuilder.DropTable(
                name: "SystemPanel");
        }
    }
}
