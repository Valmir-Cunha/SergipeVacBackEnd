using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SergipeVac.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoTabelaParaSalvarRegistrosDeSincronizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documentovacinacao");

            migrationBuilder.CreateTable(
                name: "DadosSincronizacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuantidadeRegistrosAdicionados = table.Column<int>(type: "integer", nullable: false),
                    BemSucedida = table.Column<bool>(type: "boolean", nullable: false),
                    UltimaSincronizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosSincronizacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoImportadoCSV",
                columns: table => new
                {
                    document_id = table.Column<string>(type: "text", nullable: false),
                    paciente_id = table.Column<string>(type: "text", nullable: false),
                    paciente_idade = table.Column<int>(type: "integer", nullable: false),
                    paciente_datanascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    paciente_enumsexobiologico = table.Column<string>(type: "text", nullable: false),
                    paciente_racacor_codigo = table.Column<int>(type: "integer", nullable: false),
                    paciente_racacor_valor = table.Column<string>(type: "text", nullable: false),
                    paciente_endereco_coibgemunicipio = table.Column<int>(type: "integer", nullable: false),
                    paciente_endereco_copais = table.Column<int>(type: "integer", nullable: false),
                    paciente_endereco_nmmunicipio = table.Column<string>(type: "text", nullable: false),
                    paciente_endereco_nmpais = table.Column<string>(type: "text", nullable: false),
                    paciente_endereco_uf = table.Column<string>(type: "text", nullable: false),
                    paciente_endereco_cep = table.Column<string>(type: "text", nullable: false),
                    paciente_nacionalidade_enumnacionalidade = table.Column<string>(type: "text", nullable: false),
                    estabelecimento_valor = table.Column<int>(type: "integer", nullable: false),
                    estabelecimento_razaosocial = table.Column<string>(type: "text", nullable: false),
                    estalecimento_nofantasia = table.Column<string>(type: "text", nullable: false),
                    estabelecimento_municipio_codigo = table.Column<int>(type: "integer", nullable: false),
                    estabelecimento_municipio_nome = table.Column<string>(type: "text", nullable: false),
                    estabelecimento_uf = table.Column<string>(type: "text", nullable: false),
                    vacina_grupoatendimento_codigo = table.Column<int>(type: "integer", nullable: false),
                    vacina_grupoatendimento_nome = table.Column<string>(type: "text", nullable: false),
                    vacina_categoria_codigo = table.Column<int>(type: "integer", nullable: false),
                    vacina_categoria_nome = table.Column<string>(type: "text", nullable: false),
                    vacina_lote = table.Column<string>(type: "text", nullable: false),
                    vacina_fabricante_nome = table.Column<string>(type: "text", nullable: false),
                    vacina_fabricante_referencia = table.Column<string>(type: "text", nullable: false),
                    vacina_dataaplicacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vacina_descricao_dose = table.Column<string>(type: "text", nullable: false),
                    vacina_codigo = table.Column<int>(type: "integer", nullable: false),
                    vacina_nome = table.Column<string>(type: "text", nullable: false),
                    sistema_origem = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoImportadoCSV", x => x.document_id);
                });

            migrationBuilder.Sql(@"INSERT INTO ""DadosSincronizacao"" (""Id"", ""QuantidadeRegistrosAdicionados"", ""BemSucedida"", ""UltimaSincronizacao"") VALUES (1, 1295272, true, '2023-06-04 21:00:00-03');");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosSincronizacao");

            migrationBuilder.DropTable(
                name: "DocumentoImportadoCSV");

            migrationBuilder.CreateTable(
                name: "documentovacinacao",
                columns: table => new
                {
                    document_id = table.Column<string>(type: "text", nullable: false),
                    estabelecimento_municipio_codigo = table.Column<int>(type: "integer", nullable: false),
                    estabelecimento_municipio_nome = table.Column<string>(type: "text", nullable: false),
                    estalecimento_nofantasia = table.Column<string>(type: "text", nullable: false),
                    estabelecimento_razaosocial = table.Column<string>(type: "text", nullable: false),
                    estabelecimento_uf = table.Column<string>(type: "text", nullable: false),
                    estabelecimento_valor = table.Column<int>(type: "integer", nullable: false),
                    paciente_datanascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    paciente_endereco_cep = table.Column<string>(type: "text", nullable: false),
                    paciente_endereco_copais = table.Column<int>(type: "integer", nullable: false),
                    paciente_endereco_coibgemunicipio = table.Column<int>(type: "integer", nullable: false),
                    paciente_endereco_nmmunicipio = table.Column<string>(type: "text", nullable: false),
                    paciente_endereco_nmpais = table.Column<string>(type: "text", nullable: false),
                    paciente_endereco_uf = table.Column<string>(type: "text", nullable: false),
                    paciente_enumsexobiologico = table.Column<string>(type: "text", nullable: false),
                    paciente_id = table.Column<string>(type: "text", nullable: false),
                    paciente_idade = table.Column<int>(type: "integer", nullable: false),
                    paciente_nacionalidade_enumnacionalidade = table.Column<string>(type: "text", nullable: false),
                    paciente_racacor_codigo = table.Column<int>(type: "integer", nullable: false),
                    paciente_racacor_valor = table.Column<string>(type: "text", nullable: false),
                    sistema_origem = table.Column<string>(type: "text", nullable: false),
                    vacina_categoria_codigo = table.Column<int>(type: "integer", nullable: false),
                    vacina_categoria_nome = table.Column<string>(type: "text", nullable: false),
                    vacina_codigo = table.Column<int>(type: "integer", nullable: false),
                    vacina_dataaplicacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vacina_descricao_dose = table.Column<string>(type: "text", nullable: false),
                    vacina_fabricante_nome = table.Column<string>(type: "text", nullable: false),
                    vacina_fabricante_referencia = table.Column<string>(type: "text", nullable: false),
                    vacina_grupoatendimento_codigo = table.Column<int>(type: "integer", nullable: false),
                    vacina_grupoatendimento_nome = table.Column<string>(type: "text", nullable: false),
                    vacina_lote = table.Column<string>(type: "text", nullable: false),
                    vacina_nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentovacinacao", x => x.document_id);
                });
        }
    }
}
