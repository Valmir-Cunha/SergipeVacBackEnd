﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SergipeVac.Infra;

#nullable disable

namespace SergipeVac.Migrations
{
    [DbContext(typeof(Contexto))]
    partial class ContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SergipeVac.Model.Autenticacao.Usuario", b =>
                {
                    b.Property<int>("Codigo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Codigo"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "email");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nome");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "senha");

                    b.HasKey("Codigo");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("SergipeVac.Model.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("SergipeVac.Model.DocumentoImportadoCSV", b =>
                {
                    b.Property<string>("DocumentId")
                        .HasColumnType("text")
                        .HasColumnName("document_id");

                    b.Property<int>("EstabelecimentoMunicipioCodigo")
                        .HasColumnType("integer")
                        .HasColumnName("estabelecimento_municipio_codigo");

                    b.Property<string>("EstabelecimentoMunicipioNome")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("estabelecimento_municipio_nome");

                    b.Property<string>("EstabelecimentoNoFantasia")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("estalecimento_nofantasia");

                    b.Property<string>("EstabelecimentoRazaoSocial")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("estabelecimento_razaosocial");

                    b.Property<string>("EstabelecimentoUF")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("estabelecimento_uf");

                    b.Property<int>("EstabelecimentoValor")
                        .HasColumnType("integer")
                        .HasColumnName("estabelecimento_valor");

                    b.Property<DateTime>("PacienteDataNascimento")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("paciente_datanascimento");

                    b.Property<string>("PacienteEnderecoCEP")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paciente_endereco_cep");

                    b.Property<int>("PacienteEnderecoCoPais")
                        .HasColumnType("integer")
                        .HasColumnName("paciente_endereco_copais");

                    b.Property<int>("PacienteEnderecoCoibgeMunicipio")
                        .HasColumnType("integer")
                        .HasColumnName("paciente_endereco_coibgemunicipio");

                    b.Property<string>("PacienteEnderecoNmMunicipio")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paciente_endereco_nmmunicipio");

                    b.Property<string>("PacienteEnderecoNmPais")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paciente_endereco_nmpais");

                    b.Property<string>("PacienteEnderecoUF")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paciente_endereco_uf");

                    b.Property<string>("PacienteEnumSexoBiologico")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paciente_enumsexobiologico");

                    b.Property<string>("PacienteId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paciente_id");

                    b.Property<int>("PacienteIdade")
                        .HasColumnType("integer")
                        .HasColumnName("paciente_idade");

                    b.Property<string>("PacienteNacionalidadeEnumNacionalidade")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paciente_nacionalidade_enumnacionalidade");

                    b.Property<int>("PacienteRacaCorCodigo")
                        .HasColumnType("integer")
                        .HasColumnName("paciente_racacor_codigo");

                    b.Property<string>("PacienteRacaCorValor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paciente_racacor_valor");

                    b.Property<string>("SistemaOrigem")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sistema_origem");

                    b.Property<int>("VacinaCategoriaCodigo")
                        .HasColumnType("integer")
                        .HasColumnName("vacina_categoria_codigo");

                    b.Property<string>("VacinaCategoriaNome")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("vacina_categoria_nome");

                    b.Property<int>("VacinaCodigo")
                        .HasColumnType("integer")
                        .HasColumnName("vacina_codigo");

                    b.Property<DateTime>("VacinaDataAplicacao")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("vacina_dataaplicacao");

                    b.Property<string>("VacinaDescricaoDose")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("vacina_descricao_dose");

                    b.Property<string>("VacinaFabricanteNome")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("vacina_fabricante_nome");

                    b.Property<string>("VacinaFabricanteReferencia")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("vacina_fabricante_referencia");

                    b.Property<int>("VacinaGrupoAtendimentoCodigo")
                        .HasColumnType("integer")
                        .HasColumnName("vacina_grupoatendimento_codigo");

                    b.Property<string>("VacinaGrupoAtendimentoNome")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("vacina_grupoatendimento_nome");

                    b.Property<string>("VacinaLote")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("vacina_lote");

                    b.Property<string>("VacinaNome")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("vacina_nome");

                    b.HasKey("DocumentId");

                    b.ToTable("documentovacinacao");
                });

            modelBuilder.Entity("SergipeVac.Model.DocumentoVacinacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DataAplicacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DescricaoDose")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EstabelecimentoId")
                        .HasColumnType("integer");

                    b.Property<int>("FabricanteId")
                        .HasColumnType("integer");

                    b.Property<int>("GrupoAtendimentoId")
                        .HasColumnType("integer");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lote")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PacienteId")
                        .HasColumnType("integer");

                    b.Property<int>("SistemaOrigemId")
                        .HasColumnType("integer");

                    b.Property<int>("VacinaCodigo")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("EstabelecimentoId");

                    b.HasIndex("FabricanteId");

                    b.HasIndex("GrupoAtendimentoId");

                    b.HasIndex("PacienteId");

                    b.HasIndex("SistemaOrigemId");

                    b.ToTable("DocumentosVacinacao");
                });

            modelBuilder.Entity("SergipeVac.Model.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MunicipioId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MunicipioId");

                    b.ToTable("Endereco");
                });

            modelBuilder.Entity("SergipeVac.Model.Estabelecimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MunicipioId")
                        .HasColumnType("integer");

                    b.Property<string>("NomeFantasia")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Valor")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MunicipioId");

                    b.ToTable("Estabelecimento");
                });

            modelBuilder.Entity("SergipeVac.Model.Fabricante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Referencia")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Fabricante");
                });

            modelBuilder.Entity("SergipeVac.Model.GrupoAtendimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GruposAtendimento");
                });

            modelBuilder.Entity("SergipeVac.Model.Localizacao.Localizacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "cidade");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "estado");

                    b.Property<int>("Quantidade")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "quantidade");

                    b.HasKey("Id");

                    b.HasIndex("Cidade");

                    b.HasIndex("Estado");

                    b.ToTable("Localizacao");
                });

            modelBuilder.Entity("SergipeVac.Model.Municipio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CodigoIBGE")
                        .HasColumnType("integer");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PaisId")
                        .HasColumnType("integer");

                    b.Property<string>("UF")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PaisId");

                    b.ToTable("Municipio");
                });

            modelBuilder.Entity("SergipeVac.Model.Nacionalidade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Nacionalidade");
                });

            modelBuilder.Entity("SergipeVac.Model.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EnderecoId")
                        .HasColumnType("integer");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Idade")
                        .HasColumnType("integer");

                    b.Property<int>("NacionalidadeId")
                        .HasColumnType("integer");

                    b.Property<int>("RacaId")
                        .HasColumnType("integer");

                    b.Property<int>("SexoBiologicoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoId");

                    b.HasIndex("NacionalidadeId");

                    b.HasIndex("RacaId");

                    b.HasIndex("SexoBiologicoId");

                    b.ToTable("Paciente");
                });

            modelBuilder.Entity("SergipeVac.Model.Pais", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Pais");
                });

            modelBuilder.Entity("SergipeVac.Model.Raca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Raca");
                });

            modelBuilder.Entity("SergipeVac.Model.SexoBiologico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SexoBiologico");
                });

            modelBuilder.Entity("SergipeVac.Model.Sistema", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Sistema");
                });

            modelBuilder.Entity("SergipeVac.Model.DocumentoVacinacao", b =>
                {
                    b.HasOne("SergipeVac.Model.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SergipeVac.Model.Estabelecimento", "Estabelecimento")
                        .WithMany()
                        .HasForeignKey("EstabelecimentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SergipeVac.Model.Fabricante", "Fabricante")
                        .WithMany()
                        .HasForeignKey("FabricanteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SergipeVac.Model.GrupoAtendimento", "GrupoAtendimento")
                        .WithMany()
                        .HasForeignKey("GrupoAtendimentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SergipeVac.Model.Paciente", "Paciente")
                        .WithMany()
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SergipeVac.Model.Sistema", "SistemaOrigem")
                        .WithMany()
                        .HasForeignKey("SistemaOrigemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Estabelecimento");

                    b.Navigation("Fabricante");

                    b.Navigation("GrupoAtendimento");

                    b.Navigation("Paciente");

                    b.Navigation("SistemaOrigem");
                });

            modelBuilder.Entity("SergipeVac.Model.Endereco", b =>
                {
                    b.HasOne("SergipeVac.Model.Municipio", "Municipio")
                        .WithMany()
                        .HasForeignKey("MunicipioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Municipio");
                });

            modelBuilder.Entity("SergipeVac.Model.Estabelecimento", b =>
                {
                    b.HasOne("SergipeVac.Model.Municipio", "Municipio")
                        .WithMany()
                        .HasForeignKey("MunicipioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Municipio");
                });

            modelBuilder.Entity("SergipeVac.Model.Municipio", b =>
                {
                    b.HasOne("SergipeVac.Model.Pais", "Pais")
                        .WithMany()
                        .HasForeignKey("PaisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pais");
                });

            modelBuilder.Entity("SergipeVac.Model.Paciente", b =>
                {
                    b.HasOne("SergipeVac.Model.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SergipeVac.Model.Nacionalidade", "Nacionalidade")
                        .WithMany()
                        .HasForeignKey("NacionalidadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SergipeVac.Model.Raca", "Raca")
                        .WithMany()
                        .HasForeignKey("RacaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SergipeVac.Model.SexoBiologico", "SexoBiologico")
                        .WithMany()
                        .HasForeignKey("SexoBiologicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Endereco");

                    b.Navigation("Nacionalidade");

                    b.Navigation("Raca");

                    b.Navigation("SexoBiologico");
                });
#pragma warning restore 612, 618
        }
    }
}
