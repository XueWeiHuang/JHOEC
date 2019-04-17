using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JHOEC.Migrations
{
    public partial class onlinetest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    countryCode = table.Column<string>(maxLength: 2, nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    postalPattern = table.Column<string>(maxLength: 150, nullable: true),
                    phonePattern = table.Column<string>(maxLength: 50, nullable: true),
                    retailTaxName = table.Column<string>(maxLength: 50, nullable: true),
                    retailTaxRate = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.countryCode);
                });

            migrationBuilder.CreateTable(
                name: "crop",
                columns: table => new
                {
                    cropId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 255, nullable: true),
                    image = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crop", x => x.cropId)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "fertilizer",
                columns: table => new
                {
                    fertilizerName = table.Column<string>(maxLength: 255, nullable: false),
                    OECProduct = table.Column<bool>(nullable: false),
                    liquid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fertilizer", x => x.fertilizerName)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "province",
                columns: table => new
                {
                    provinceCode = table.Column<string>(maxLength: 2, nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    countryCode = table.Column<string>(maxLength: 2, nullable: true),
                    retailTaxName = table.Column<string>(maxLength: 50, nullable: true),
                    retailTaxRate = table.Column<double>(nullable: true),
                    federalTaxIncluded = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_province", x => x.provinceCode);
                });

            migrationBuilder.CreateTable(
                name: "variety",
                columns: table => new
                {
                    varietyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cropId = table.Column<int>(nullable: true),
                    name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variety", x => x.varietyId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "cropVariety_FK00",
                        column: x => x.cropId,
                        principalTable: "crop",
                        principalColumn: "cropId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "farm",
                columns: table => new
                {
                    farmId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    address = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    town = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    county = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    provinceCode = table.Column<string>(maxLength: 2, nullable: false),
                    postalCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    homePhone = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    cellPhone = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    directions = table.Column<string>(unicode: false, nullable: true),
                    dateJoined = table.Column<DateTime>(type: "datetime", nullable: true),
                    lastContactDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_farm", x => x.farmId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_farm_province",
                        column: x => x.provinceCode,
                        principalTable: "province",
                        principalColumn: "provinceCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "plot",
                columns: table => new
                {
                    plotId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    farmId = table.Column<int>(nullable: true),
                    varietyId = table.Column<int>(nullable: true),
                    datePlanted = table.Column<DateTime>(type: "datetime", nullable: true),
                    dateHarvested = table.Column<DateTime>(type: "datetime", nullable: true),
                    plantingRate = table.Column<int>(nullable: true),
                    plantingRateByPounds = table.Column<bool>(nullable: false),
                    rowWidth = table.Column<int>(nullable: true),
                    patternRepeats = table.Column<int>(nullable: true),
                    organicMatter = table.Column<double>(nullable: true),
                    bicarbP = table.Column<double>(nullable: true),
                    potassium = table.Column<double>(nullable: true),
                    magnesium = table.Column<double>(nullable: true),
                    calcium = table.Column<double>(nullable: true),
                    pHSoil = table.Column<double>(nullable: true),
                    pHBuffer = table.Column<double>(nullable: true),
                    CEC = table.Column<double>(nullable: true),
                    percentBaseSaturationK = table.Column<double>(nullable: true),
                    percentBaseSaturationMg = table.Column<double>(nullable: true),
                    percentBaseSaturationCa = table.Column<double>(nullable: true),
                    percentBaseSaturationH = table.Column<double>(nullable: true),
                    comments = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plot", x => x.plotId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "plots_FK00",
                        column: x => x.farmId,
                        principalTable: "farm",
                        principalColumn: "farmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "plots_FK01",
                        column: x => x.varietyId,
                        principalTable: "variety",
                        principalColumn: "varietyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "treatment",
                columns: table => new
                {
                    treatmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 256, nullable: true),
                    plotId = table.Column<int>(nullable: false),
                    moisture = table.Column<float>(nullable: true),
                    yield = table.Column<double>(nullable: true),
                    weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treatment", x => x.treatmentId);
                    table.ForeignKey(
                        name: "FK_treatment_plot",
                        column: x => x.plotId,
                        principalTable: "plot",
                        principalColumn: "plotId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "treatmentFertilizer",
                columns: table => new
                {
                    treatmentFertilizerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    treatmentId = table.Column<int>(nullable: true),
                    fertilizerName = table.Column<string>(maxLength: 255, nullable: true),
                    ratePerAcre = table.Column<double>(nullable: true),
                    rateMetric = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treatmentFertilizer", x => x.treatmentFertilizerId);
                    table.ForeignKey(
                        name: "FK_treatmentFertilizer_fertilizer",
                        column: x => x.fertilizerName,
                        principalTable: "fertilizer",
                        principalColumn: "fertilizerName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_treatmentFertilizer_treatment",
                        column: x => x.treatmentId,
                        principalTable: "treatment",
                        principalColumn: "treatmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "province code",
                table: "farm",
                column: "provinceCode");

            migrationBuilder.CreateIndex(
                name: "locationID",
                table: "plot",
                column: "farmId");

            migrationBuilder.CreateIndex(
                name: "cropID",
                table: "plot",
                column: "varietyId");

            migrationBuilder.CreateIndex(
                name: "IX_treatment_plotId",
                table: "treatment",
                column: "plotId");

            migrationBuilder.CreateIndex(
                name: "IX_treatmentFertilizer_fertilizerName",
                table: "treatmentFertilizer",
                column: "fertilizerName");

            migrationBuilder.CreateIndex(
                name: "IX_treatmentFertilizer_treatmentId",
                table: "treatmentFertilizer",
                column: "treatmentId");

            migrationBuilder.CreateIndex(
                name: "cropID",
                table: "variety",
                column: "cropId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "treatmentFertilizer");

            migrationBuilder.DropTable(
                name: "fertilizer");

            migrationBuilder.DropTable(
                name: "treatment");

            migrationBuilder.DropTable(
                name: "plot");

            migrationBuilder.DropTable(
                name: "farm");

            migrationBuilder.DropTable(
                name: "variety");

            migrationBuilder.DropTable(
                name: "province");

            migrationBuilder.DropTable(
                name: "crop");
        }
    }
}
