using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyFinder.Migrations
{
    public static class DatabaseTypes
    {
        public const string Number10 = "NUMBER(10)";
    }

    public static class OracleAnnotations
    {
        public const string Identity = "Oracle:Identity";
    }

    public static class OracleIdentityConfig
    {
        public const string StartWith1IncrementBy1 = "START WITH 1 INCREMENT BY 1";
    }

    public static class OracleColumnTypes
    {
        public const string NVarChar2000 = "NVARCHAR2(2000)";
    }

    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ANDAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                        .Annotation(OracleAnnotations.Identity, OracleIdentityConfig.StartWith1IncrementBy1),
                    NUMERO_ANDAR = table.Column<int>(type: DatabaseTypes.Number10, nullable: false),
                    GALPAO_ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANDAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BLOCO",
                columns: table => new
                {
                    ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                        .Annotation(OracleAnnotations.Identity, OracleIdentityConfig.StartWith1IncrementBy1),
                    LETRA_BLOCO = table.Column<string>(type: OracleColumnTypes.NVarChar2000, nullable: false),
                    PATIO_ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLOCO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GALPAO",
                columns: table => new
                {
                    ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                        .Annotation(OracleAnnotations.Identity, OracleIdentityConfig.StartWith1IncrementBy1),
                    NOME_GALPAO = table.Column<string>(type: OracleColumnTypes.NVarChar2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GALPAO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MOTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                        .Annotation(OracleAnnotations.Identity, OracleIdentityConfig.StartWith1IncrementBy1),
                    STATUS = table.Column<string>(type: OracleColumnTypes.NVarChar2000, nullable: false),
                    MODELO = table.Column<string>(type: OracleColumnTypes.NVarChar2000, nullable: false),
                    MARCA = table.Column<string>(type: OracleColumnTypes.NVarChar2000, nullable: false),
                    PLACA = table.Column<string>(type: OracleColumnTypes.NVarChar2000, nullable: false),
                    CHASSI = table.Column<string>(type: OracleColumnTypes.NVarChar2000, nullable: false),
                    VAGA_ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PATIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                        .Annotation(OracleAnnotations.Identity, OracleIdentityConfig.StartWith1IncrementBy1),
                    NUMERO_PATIO = table.Column<int>(type: DatabaseTypes.Number10, nullable: false),
                    ANDAR_ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VAGA",
                columns: table => new
                {
                    ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                        .Annotation(OracleAnnotations.Identity, OracleIdentityConfig.StartWith1IncrementBy1),
                    NUMERO_VAGA = table.Column<int>(type: DatabaseTypes.Number10, nullable: false),
                    BLOCO_ID = table.Column<int>(type: DatabaseTypes.Number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VAGA", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANDAR");

            migrationBuilder.DropTable(
                name: "BLOCO");

            migrationBuilder.DropTable(
                name: "GALPAO");

            migrationBuilder.DropTable(
                name: "MOTO");

            migrationBuilder.DropTable(
                name: "PATIO");

            migrationBuilder.DropTable(
                name: "VAGA");
        }
    }
}
