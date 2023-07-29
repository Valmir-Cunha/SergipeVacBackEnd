using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SergipeVac.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoTabelaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Senha = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "documentovacinacao",
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
                    table.PrimaryKey("PK_documentovacinacao", x => x.document_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "documentovacinacao");
        }
    }
}
