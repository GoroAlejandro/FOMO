using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fomo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateFomoAppDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmaAlert = table.Column<bool>(type: "bit", nullable: false),
                    BollingerAlert = table.Column<bool>(type: "bit", nullable: false),
                    StochasticAlert = table.Column<bool>(type: "bit", nullable: false),
                    RsiAlert = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TradeResults",
                columns: table => new
                {
                    TradeResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EntryPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Profit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfStocks = table.Column<int>(type: "int", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeResults", x => x.TradeResultId);
                    table.CheckConstraint("CK_Product_EntryPrice_NonNegative", "[EntryPrice] >= 0");
                    table.CheckConstraint("CK_Product_ExitPrice_NonNegative", "[ExitPrice] >= 0");
                    table.CheckConstraint("CK_Product_NumberOfStocks_NonNegative", "[NumberOfStocks] >= 0");
                    table.ForeignKey(
                        name: "FK_TradeResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeMethods",
                columns: table => new
                {
                    TradeMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sma = table.Column<bool>(type: "bit", nullable: false),
                    Bollinger = table.Column<bool>(type: "bit", nullable: false),
                    Stochastic = table.Column<bool>(type: "bit", nullable: false),
                    Rsi = table.Column<bool>(type: "bit", nullable: false),
                    Other = table.Column<bool>(type: "bit", nullable: false),
                    TradeResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeMethods", x => x.TradeMethodId);
                    table.ForeignKey(
                        name: "FK_TradeMethods_TradeResults_TradeResultId",
                        column: x => x.TradeResultId,
                        principalTable: "TradeResults",
                        principalColumn: "TradeResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeMethods_TradeResultId",
                table: "TradeMethods",
                column: "TradeResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeResults_UserId",
                table: "TradeResults",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeMethods");

            migrationBuilder.DropTable(
                name: "TradeResults");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
