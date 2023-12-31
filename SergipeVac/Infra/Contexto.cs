﻿using Microsoft.EntityFrameworkCore;
using Npgsql;
using SergipeVac.Model.Autenticacao;
using SergipeVac.Model.Localizacao;
using SergipeVac.Model.ModeloDados;
using SergipeVac.Model.Sincronizacao;

namespace SergipeVac.Infra
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        #region Registros de vacinação
        public DbSet<DocumentoImportado> DocumentoImportadoCSV { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<DocumentoVacinacao> DocumentosVacinacao { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Estabelecimento> Estabelecimento { get; set; }
        public DbSet<Fabricante> Fabricante { get; set; }
        public DbSet<GrupoAtendimento> GruposAtendimento { get; set; }
        public DbSet<Municipio> Municipio { get; set; }
        public DbSet<Nacionalidade> Nacionalidade { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Raca> Raca { get; set; }
        public DbSet<SexoBiologico> SexoBiologico { get; set; }
        public DbSet<Sistema> Sistema { get; set; }

        #endregion

        public DbSet<Usuario> Usuario { get; set; }
        
        public DbSet<DadosSincronizacao> DadosSincronizacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Registros de vacinação

            modelBuilder.Entity<DocumentoVacinacao>(entity =>
            {
                entity.HasOne(c => c.Paciente)
                      .WithMany()
                      .HasForeignKey(c => c.PacienteId);

                entity.HasOne(c => c.Estabelecimento)
                      .WithMany()
                      .HasForeignKey(c => c.EstabelecimentoId);

                entity.HasOne(c => c.SistemaOrigem)
                      .WithMany()
                      .HasForeignKey(c => c.SistemaOrigemId);
                entity.HasOne(c => c.GrupoAtendimento)
                    .WithMany()
                    .HasForeignKey(c => c.GrupoAtendimentoId);

                entity.HasOne(c => c.Categoria)
                    .WithMany()
                    .HasForeignKey(c => c.CategoriaId);

                entity.HasOne(c => c.Fabricante)
                    .WithMany()
                    .HasForeignKey(c => c.FabricanteId);
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.HasOne(c => c.Municipio)
                      .WithMany()
                      .HasForeignKey(c => c.MunicipioId);
            });

            modelBuilder.Entity<Estabelecimento>(entity =>
            {
                entity.HasOne(c => c.Municipio)
                      .WithMany()
                      .HasForeignKey(c => c.MunicipioId);
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.HasOne(c => c.Pais)
                      .WithMany()
                      .HasForeignKey(c => c.PaisId);
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.SexoBiologico)
                      .WithMany()
                      .HasForeignKey(c => c.SexoBiologicoId);

                entity.HasOne(c => c.Raca)
                      .WithMany()
                      .HasForeignKey(c => c.RacaId);

                entity.HasOne(c => c.Endereco)
                      .WithMany()
                      .HasForeignKey(c => c.EnderecoId);

                entity.HasOne(c => c.Nacionalidade)
                      .WithMany()
                      .HasForeignKey(c => c.NacionalidadeId);
            });

            modelBuilder.Entity<DocumentoImportado>(entity =>
            {
                entity.HasKey(c => c.DocumentId);
            });

            #endregion

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(p => p.Codigo);
                entity.HasIndex(p => p.Email).IsUnique();
            });

            modelBuilder.Entity<Localizacao>(entity =>
            {

                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.Cidade);
                entity.HasIndex(p => p.Estado);

            });

            modelBuilder.Entity<DadosSincronizacao>(entity =>
            {
                entity.HasKey(p => p.Id);
            });
        }
    }
}
