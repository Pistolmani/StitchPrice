using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StitchPrice.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pricing_quotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PlacementType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FabricType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    StitchCount = table.Column<int>(type: "integer", nullable: false),
                    ColorCount = table.Column<int>(type: "integer", nullable: false),
                    GarmentCostPerItem = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RequiresDigitizing = table.Column<bool>(type: "boolean", nullable: false),
                    IsUrgent = table.Column<bool>(type: "boolean", nullable: false),
                    SetupFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DigitizingFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MarkupAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PricePerItem = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ProfitMarginPercentage = table.Column<decimal>(type: "numeric(7,1)", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing_quotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pricing_settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    PricePerThousandStitches = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SetupFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DigitizingFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UrgencyMultiplier = table.Column<decimal>(type: "numeric(7,4)", nullable: false),
                    DefaultMarkupPercentage = table.Column<decimal>(type: "numeric(7,2)", nullable: false),
                    MinimumOrderPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ColorComplexityFeePerColor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BulkDiscountEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product_pricing_profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ProductType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DefaultGarmentCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DefaultMarkupPercentage = table.Column<decimal>(type: "numeric(7,2)", nullable: false),
                    DifficultyMultiplier = table.Column<decimal>(type: "numeric(7,4)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_pricing_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pricing_breakdown_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PricingQuoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing_breakdown_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pricing_breakdown_items_pricing_quotes_PricingQuoteId",
                        column: x => x.PricingQuoteId,
                        principalTable: "pricing_quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pricing_breakdown_items_PricingQuoteId",
                table: "pricing_breakdown_items",
                column: "PricingQuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_product_pricing_profiles_ProductType",
                table: "product_pricing_profiles",
                column: "ProductType",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pricing_breakdown_items");

            migrationBuilder.DropTable(
                name: "pricing_settings");

            migrationBuilder.DropTable(
                name: "product_pricing_profiles");

            migrationBuilder.DropTable(
                name: "pricing_quotes");
        }
    }
}
