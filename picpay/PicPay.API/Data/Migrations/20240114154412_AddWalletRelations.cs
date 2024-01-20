using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicPay.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Transactions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Entries",
                newName: "WalletId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Entries",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TransactionWallet",
                columns: table => new
                {
                    TransactionsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WalletsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionWallet", x => new { x.TransactionsId, x.WalletsId });
                    table.ForeignKey(
                        name: "FK_TransactionWallet_Transactions_TransactionsId",
                        column: x => x.TransactionsId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionWallet_Wallets_WalletsId",
                        column: x => x.WalletsId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_WalletId",
                table: "Entries",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWallet_WalletsId",
                table: "TransactionWallet",
                column: "WalletsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Wallets_WalletId",
                table: "Entries",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Wallets_WalletId",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "TransactionWallet");

            migrationBuilder.DropIndex(
                name: "IX_Entries_WalletId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Entries");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Transactions",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Entries",
                newName: "Value");
        }
    }
}
