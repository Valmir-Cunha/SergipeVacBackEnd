using System.Collections.Concurrent;
using System.ComponentModel.Design;
using SergipeVac.Infra;
using SergipeVac.Model.Interface;
using SergipeVac.Model.ModeloDados;

namespace SergipeVac.Conversores
{
    public class ConversorDados
    {
        private IRepositorio<DocumentoImportado> _documentoImportadoRepositorio;
        private IRepositorio<Categoria> _categoriaRepositorio;
        private IRepositorio<DocumentoVacinacao> _documentoVacinacaoRepositorio;
        private IRepositorio<Endereco> _enderecoRepositorio;
        private IRepositorio<Estabelecimento> _estabelecimentoRepositorio;
        private IRepositorio<Fabricante> _fabricanteRepositorio;
        private IRepositorio<GrupoAtendimento> _grupoAtendimentoRepositorio;
        private IRepositorioPaciente _pacienteRepositorio;
        private IRepositorio<Sistema> _sistemaRepositorio;
        private IServiceProvider _serviceProvider;

        int _sistemaId = 1;
        int _idPaciente = 1;
        int _idFabricante = 1;
        int _idEstabelecimento = 1;
        int _idEndereco = 1;

        ConcurrentBag<Paciente> pacientesCadastrados = new();
        ConcurrentBag<Sistema> sistemasCadastrados = new();
        ConcurrentBag<GrupoAtendimento> grupoAtendimentosCadastrados = new();
        ConcurrentBag<Fabricante> fabricantesCadastrados = new();
        ConcurrentBag<Categoria> categoriasAdicionadas = new();
        ConcurrentBag<Estabelecimento> estabelecimentosAdicionados = new();
        ConcurrentBag<Endereco> enderecosAdicionados = new();

        List<Paciente> pacientesCadastradosBanco = new();
        List<Sistema> sistemasCadastradosBanco = new();
        List<GrupoAtendimento> grupoAtendimentosCadastradosBanco = new();
        List<Fabricante> fabricantesCadastradosBanco = new();
        List<Categoria> categoriasAdicionadasBanco = new();
        List<Estabelecimento> estabelecimentosAdicionadosBanco = new();
        List<Endereco> enderecosAdicionadosBanco = new();
        //List<DocumentoVacinacao> documentosVacinacoes = new();

        public ConversorDados(IRepositorio<DocumentoImportado> documentoImportadoRepositorio,
                              IRepositorio<Categoria> categoriaRepositorio,
                              IRepositorio<DocumentoVacinacao> documentoVacinacaoRepositorio,
                              IRepositorio<Endereco> enderecoRepositorio,
                              IRepositorio<Estabelecimento> estabelecimentoRepositorio,
                              IRepositorio<Fabricante> fabricanteRepositorio,
                              IRepositorio<GrupoAtendimento> grupoAtendimentoRepositorio,
                              IRepositorioPaciente pacienteRepositorio,
                              IRepositorio<Sistema> sistemaRepositorio,
                              IServiceProvider serviceProvider)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _documentoVacinacaoRepositorio = documentoVacinacaoRepositorio;
            _enderecoRepositorio = enderecoRepositorio;
            _estabelecimentoRepositorio = estabelecimentoRepositorio;
            _fabricanteRepositorio = fabricanteRepositorio;
            _grupoAtendimentoRepositorio = grupoAtendimentoRepositorio;
            _pacienteRepositorio = pacienteRepositorio;
            _sistemaRepositorio = sistemaRepositorio;
            _serviceProvider = serviceProvider;
            _documentoImportadoRepositorio = documentoImportadoRepositorio;
            ObterProximosIds();
        }

        private void ObterProximosIds()
        {
            _sistemaId = _sistemaRepositorio.ObterProximoId("Id");
            _idPaciente = _pacienteRepositorio.ObterProximoId("Id");
            _idFabricante = _fabricanteRepositorio.ObterProximoId("Id");
            _idEstabelecimento = _estabelecimentoRepositorio.ObterProximoId("Id");
            _idEndereco = _enderecoRepositorio.ObterProximoId("Id");
        }

        public void ConverterDocumentoImportadoParaDocumentosVacinacao(List<DocumentoImportado> documentosImportados)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Contexto>();

            using var transaction = dbContext.Database.BeginTransaction();

            _categoriaRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorio<Categoria>>();
            _documentoVacinacaoRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorio<DocumentoVacinacao>>();
            _enderecoRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorio<Endereco>>();
            _estabelecimentoRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorio<Estabelecimento>>();
            _fabricanteRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorio<Fabricante>>();
            _grupoAtendimentoRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorio<GrupoAtendimento>>();
            _pacienteRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorioPaciente>();
            _sistemaRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorio<Sistema>>();
            _documentoImportadoRepositorio = scope.ServiceProvider.GetRequiredService<IRepositorio<DocumentoImportado>>();

            pacientesCadastradosBanco = _pacienteRepositorio.ObterTodos().ToList();
            sistemasCadastradosBanco = _sistemaRepositorio.ObterTodos().ToList();
            grupoAtendimentosCadastradosBanco = _grupoAtendimentoRepositorio.ObterTodos().ToList();
            fabricantesCadastradosBanco = _fabricanteRepositorio.ObterTodos().ToList();
            categoriasAdicionadasBanco = _categoriaRepositorio.ObterTodos().ToList();
            estabelecimentosAdicionadosBanco = _estabelecimentoRepositorio.ObterTodos().ToList();
            enderecosAdicionadosBanco = _enderecoRepositorio.ObterTodos().ToList();

            try
            {
                ConcurrentBag<DocumentoVacinacao> documentosVacinacoes = new();

                documentosImportados.AsParallel().ForAll(documentoImportado =>
                {
                    var sexoId = ObterSexoBiologico(documentoImportado.PacienteEnumSexoBiologico);
                    var endereco = ObterEndereco(documentoImportado.PacienteEnderecoCEP);
                    var nacionalidadeId = documentoImportado.PacienteNacionalidadeEnumNacionalidade == "B" ? 1 : 2;
                    var paciente = ObterPaciente(documentoImportado, sexoId, endereco, nacionalidadeId);
                    var estabelecimento = ObterEstabelecimento(documentoImportado);
                    var categoria = ObterCategoria(documentoImportado);
                    var fabricante = ObterFabricante(documentoImportado);
                    var grupoAtendimento = ObterGrupoAtendimento(documentoImportado);
                    var sistema = ObterSistema(documentoImportado);
                    var documentoVacinacao = new DocumentoVacinacao()
                    {
                        Guid = documentoImportado.DocumentId,
                        PacienteId = paciente.Id,
                        EstabelecimentoId = estabelecimento.Id,
                        SistemaOrigemId = sistema.Id,
                        Nome = documentoImportado.VacinaNome,
                        GrupoAtendimentoId = grupoAtendimento.Id,
                        CategoriaId = categoria.Id,
                        DataAplicacao = DateTime.SpecifyKind(documentoImportado.VacinaDataAplicacao, DateTimeKind.Utc),
                        DescricaoDose = documentoImportado.VacinaDescricaoDose,
                        FabricanteId = fabricante.Id,
                        Lote = documentoImportado.VacinaLote,
                        VacinaCodigo = documentoImportado.VacinaCodigo
                    };
                    lock (documentosVacinacoes)
                    {
                        documentosVacinacoes.Add(documentoVacinacao);
                    }
                });

                _pacienteRepositorio.AdicionarConjunto(pacientesCadastrados.ToList());
                _documentoVacinacaoRepositorio.AdicionarConjunto(documentosVacinacoes.ToList());
                _estabelecimentoRepositorio.AdicionarConjunto(estabelecimentosAdicionados.ToList());
                _categoriaRepositorio.AdicionarConjunto(categoriasAdicionadas.ToList());
                _fabricanteRepositorio.AdicionarConjunto(fabricantesCadastrados.ToList());
                _grupoAtendimentoRepositorio.AdicionarConjunto(grupoAtendimentosCadastrados.ToList());
                _sistemaRepositorio.AdicionarConjunto(sistemasCadastrados.ToList());
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Erro ao converter dados: " + ex.Message);
                throw;
            }
        }

        private int ObterSexoBiologico(string EnumSexo)
        {
            var sexoId = EnumSexo switch
            {
                "M" => 1,
                "F" => 2,
                _ => 3
            };

            return sexoId;
        }

        private Endereco ObterEndereco(string cep)
        {
            Endereco endereco;
            var enderecoLista = enderecosAdicionados.FirstOrDefault(e => e.CEP == cep);
            var enderecoBanco = enderecosAdicionadosBanco.FirstOrDefault(e => e.CEP == cep);
            if (enderecoLista == null && enderecoBanco == null)
            {
                endereco = CadastrarERetornarEndereco(cep);
            }
            else
            {
                endereco = enderecoLista ?? enderecoBanco;
            }
            return endereco;
        }

        private Endereco CadastrarERetornarEndereco(string cep)
        {
            var endereco = new Endereco()
            {
                Id = _idEndereco,
                CEP = cep
            };
            _idEndereco++;
            enderecosAdicionados.Add(endereco);
            return endereco;
        }

        private Paciente ObterPaciente(DocumentoImportado documentoImportado, int sexoId, Endereco endereco, int nacionalidadeId)
        {
            Paciente paciente;
            var pacienteLista = pacientesCadastrados.FirstOrDefault(p => p.Guid == documentoImportado.PacienteId);
            var pacienteBanco = pacientesCadastradosBanco.FirstOrDefault(p => p.Guid == documentoImportado.PacienteId);
            if (pacienteLista == null && pacienteBanco == null)
            {
                paciente = CadastrarERetornarPaciente(documentoImportado, sexoId, endereco, nacionalidadeId);
            }
            else
            {
                paciente = pacienteBanco ?? pacienteLista;
            }
            return paciente;
        }

        private Paciente CadastrarERetornarPaciente(DocumentoImportado documento, int sexoId, Endereco endereco, int nacionalidadeId)
        {
            var paciente = new Paciente()
            {
                Id = _idPaciente,
                Guid = documento.PacienteId,
                Idade = documento.PacienteIdade,
                DataNascimento = DateTime.SpecifyKind(documento.PacienteDataNascimento, DateTimeKind.Utc),
                SexoBiologicoId = sexoId,
                RacaId = documento.PacienteRacaCorCodigo,
                EnderecoId = endereco.Id,
                NacionalidadeId = nacionalidadeId
            };
            _idPaciente++;
            pacientesCadastrados.Add(paciente);
            return paciente;
        }

        private Estabelecimento ObterEstabelecimento(DocumentoImportado documentoImportado)
        {
            Estabelecimento estabelecimento;
            var estabelecimentoLista = estabelecimentosAdicionados.FirstOrDefault(e => e.NomeFantasia == 
                                                                                            documentoImportado.EstabelecimentoNoFantasia);
            var estabelecimentoBanco = estabelecimentosAdicionadosBanco.FirstOrDefault(e => e.NomeFantasia ==
                                                                                            documentoImportado.EstabelecimentoNoFantasia);

            if (estabelecimentoBanco == null && estabelecimentoLista == null)
            {
                estabelecimento = CadastrarERetornarEstabelecimento(documentoImportado);
            }
            else
            {
                estabelecimento = estabelecimentoLista ?? estabelecimentoBanco;
            }

            return estabelecimento;
        }

        private Estabelecimento CadastrarERetornarEstabelecimento(DocumentoImportado documento)
        {
            var idMunicipioAracaju = 1;
            var estabelecimento = new Estabelecimento()
            {
                Id = _idEstabelecimento,
                MunicipioId = idMunicipioAracaju,
                NomeFantasia = documento.EstabelecimentoNoFantasia,
                RazaoSocial = documento.EstabelecimentoRazaoSocial,
                Valor = documento.EstabelecimentoValor
            };
            _idEstabelecimento++;
            estabelecimentosAdicionados.Add(estabelecimento);
            return estabelecimento;
        }

        private Categoria ObterCategoria(DocumentoImportado documentoImportado)
        {
            Categoria categoria;
            var categoriaLista = categoriasAdicionadas.FirstOrDefault(c => c.Id == documentoImportado.VacinaCategoriaCodigo);
            var categoriaBanco = categoriasAdicionadasBanco.FirstOrDefault(c => c.Id == documentoImportado.VacinaCategoriaCodigo);
            if (categoriaLista == null && categoriaBanco == null)
            {
                categoria = CadastrarERetornarCategoria(documentoImportado);
            }
            else
            {
                categoria = categoriaBanco ?? categoriaLista;
            }
            return categoria;
        }

        private Categoria CadastrarERetornarCategoria(DocumentoImportado documento)
        {
            var categoria = new Categoria()
            {
                Id = documento.VacinaCategoriaCodigo,
                Nome = documento.VacinaCategoriaNome
            };
            categoriasAdicionadas.Add(categoria);
            return categoria;
        }
        private Fabricante ObterFabricante(DocumentoImportado documentoImportado)
        {
            Fabricante fabricante;
            var fabricanteLista = fabricantesCadastrados.FirstOrDefault(f => f.Nome == documentoImportado.VacinaFabricanteNome);
            var fabricanteBanco = fabricantesCadastradosBanco.FirstOrDefault(f => f.Nome == documentoImportado.VacinaFabricanteNome);
            if (fabricanteLista == null && fabricanteBanco == null)
            {
                fabricante = CadastrarERetornarFabricante(documentoImportado);
            }
            else
            {
                fabricante = fabricanteLista ?? fabricanteBanco;
            }
            return fabricante;
        }

        private Fabricante CadastrarERetornarFabricante(DocumentoImportado documento)
        {
            var fabricante = new Fabricante()
            {
                Id = _idFabricante,
                Nome = documento.VacinaFabricanteNome,
                Referencia = documento.VacinaFabricanteReferencia
            };
            _idFabricante++;
            fabricantesCadastrados.Add(fabricante);
            return fabricante;
        }

        private Sistema ObterSistema(DocumentoImportado documentoImportado)
        {
            Sistema sistema;
            var sistemaLista = sistemasCadastrados.FirstOrDefault(s => s.Nome == documentoImportado.SistemaOrigem);
            var sistemaBanco = sistemasCadastradosBanco.FirstOrDefault(s => s.Nome == documentoImportado.SistemaOrigem);

            if (sistemaLista == null && sistemaBanco == null)
            {
                sistema = CadastrarERetornarSistema(documentoImportado);
            }
            else
            {
                sistema = sistemaLista ?? sistemaBanco;
            }
            return sistema;
        }

        private Sistema CadastrarERetornarSistema(DocumentoImportado documento)
        {
            var sistema = new Sistema()
            {
                Id = _sistemaId,
                Nome = documento.SistemaOrigem
            };
            _sistemaId++;
            sistemasCadastrados.Add(sistema);
            return sistema;
        }

        private GrupoAtendimento ObterGrupoAtendimento(DocumentoImportado documentoImportado)
        {
            GrupoAtendimento grupoAtendimento;
            var grupoAtendimentoLista = grupoAtendimentosCadastrados.FirstOrDefault(g => g.Id == documentoImportado.VacinaGrupoAtendimentoCodigo);            
            var grupoAtendimentoBanco = grupoAtendimentosCadastradosBanco.FirstOrDefault(g => g.Id == documentoImportado.VacinaGrupoAtendimentoCodigo);
            if (grupoAtendimentoLista == null && grupoAtendimentoBanco == null)
            {
                grupoAtendimento = CadastrarERetornarGrupoAtendimento(documentoImportado);
            }
            else
            {
                grupoAtendimento = grupoAtendimentoLista ?? grupoAtendimentoBanco;
            }
            return grupoAtendimento;
        }

        private GrupoAtendimento CadastrarERetornarGrupoAtendimento(DocumentoImportado documento)
        {
            var grupoAtendimento = new GrupoAtendimento()
            {
                Id = documento.VacinaGrupoAtendimentoCodigo,
                Nome = documento.VacinaGrupoAtendimentoNome
            };
            grupoAtendimentosCadastrados.Add(grupoAtendimento);

            return grupoAtendimento;
        }
    }
}