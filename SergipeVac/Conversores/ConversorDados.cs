﻿using SergipeVac.Model;
using SergipeVac.Model.Interface;

namespace SergipeVac.Conversores
{
    public class ConversorDados
    {
        private readonly IRepositorio<DocumentoImportadoCSV> _documentoImportadoRepositorio;
        private readonly IRepositorio<Categoria> _categoriaRepositorio;
        private readonly IRepositorio<DocumentoVacinacao> _documentoVacinacaoRepositorio;
        private readonly IRepositorio<Endereco> _enderecoRepositorio;
        private readonly IRepositorio<Estabelecimento> _estabelecimentoRepositorio;
        private readonly IRepositorio<Fabricante> _fabricanteRepositorio;
        private readonly IRepositorio<GrupoAtendimento> _grupoAtendimentoRepositorio;
        private readonly IRepositorio<Municipio> _municipioRepositorio;
        private readonly IRepositorio<Nacionalidade> _nacionalidadeRepositorio;
        private readonly IRepositorio<Paciente> _pacienteRepositorio;
        private readonly IRepositorio<Pais> _paisRepositorio;
        private readonly IRepositorio<Raca> _racaRepositorio;
        private readonly IRepositorio<SexoBiologico> _sexoBiologicoRepositorio;
        private readonly IRepositorio<Sistema> _sistemaRepositorio;
        private readonly object _lock = new object();

        int _sistemaId = 1;
        int _idPaciente = 1;
        int _idFabricante = 1;
        int _idEstabelecimento = 1;
        int _idEndereco = 1;
        int _idMunicipio = 1;

        List<Paciente> pacientesCadastrados = new List<Paciente>();
        List<Sistema> sistemasCadastrados = new List<Sistema>();
        List<GrupoAtendimento> grupoAtendimentosCadastrados = new List<GrupoAtendimento>();
        List<Fabricante> fabricantesCadastrados = new List<Fabricante>();
        List<Categoria> categoriasAdicionadas = new List<Categoria>();
        List<Estabelecimento> estabelecimentosAdicionadas = new List<Estabelecimento>();
        List<Endereco> enderecosAdicionadas = new List<Endereco>();
        List<Municipio> municipiosAdicionadas = new List<Municipio>();

        public ConversorDados(IRepositorio<DocumentoImportadoCSV> documentoImportadoRepositorio, IRepositorio<Categoria> categoriaRepositorio, IRepositorio<DocumentoVacinacao> documentoVacinacaoRepositorio, IRepositorio<Endereco> enderecoRepositorio, IRepositorio<Estabelecimento> estabelecimentoRepositorio, IRepositorio<Fabricante> fabricanteRepositorio, IRepositorio<GrupoAtendimento> grupoAtendimentoRepositorio, IRepositorio<Municipio> municipioRepositorio, IRepositorio<Nacionalidade> nacionalidadeRepositorio, IRepositorio<Paciente> pacienteRepositorio, IRepositorio<Pais> paisRepositorio, IRepositorio<Raca> racaRepositorio, IRepositorio<SexoBiologico> sexoBiologicoRepositorio, IRepositorio<Sistema> sistemaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _documentoVacinacaoRepositorio = documentoVacinacaoRepositorio;
            _enderecoRepositorio = enderecoRepositorio;
            _estabelecimentoRepositorio = estabelecimentoRepositorio;
            _fabricanteRepositorio = fabricanteRepositorio;
            _grupoAtendimentoRepositorio = grupoAtendimentoRepositorio;
            _municipioRepositorio = municipioRepositorio;
            _nacionalidadeRepositorio = nacionalidadeRepositorio;
            _pacienteRepositorio = pacienteRepositorio;
            _paisRepositorio = paisRepositorio;
            _racaRepositorio = racaRepositorio;
            _sexoBiologicoRepositorio = sexoBiologicoRepositorio;
            _sistemaRepositorio = sistemaRepositorio;
            _documentoImportadoRepositorio = documentoImportadoRepositorio;
        }
        public void ConverterCSVDocumentosVacinacao()
        {
            bool consultaConcluida = false;
            List<DocumentoImportadoCSV> documentosImportador = null;
            List<DocumentoVacinacao> documenetVacinacaos = new List<DocumentoVacinacao>();
            List<DocumentoImportadoCSV> categoriasDistintas = new List<DocumentoImportadoCSV>();

            Thread consultaThread = new Thread(() =>
            {
                documentosImportador = _documentoImportadoRepositorio.Obter(p => p.EstabelecimentoMunicipioCodigo == 280030).ToList();
                //categoriasDistintas = documentosImportador.DistinctBy(p => p.EstabelecimentoNoFantasia).ToList();
                consultaConcluida = true;
            });

            consultaThread.Start();

            while (!consultaConcluida)
            {
                Thread.Sleep(50); // Aguarda 100 milissegundos antes de verificar novamente
            }

            int contador = 0;

            pacientesCadastrados = _pacienteRepositorio.ObterTodos().ToList();
            estabelecimentosAdicionadas = _estabelecimentoRepositorio.ObterTodos().ToList();
            sistemasCadastrados = _sistemaRepositorio.ObterTodos().ToList();
            grupoAtendimentosCadastrados = _grupoAtendimentoRepositorio.ObterTodos().ToList();
            categoriasAdicionadas = _categoriaRepositorio.ObterTodos().ToList();
            fabricantesCadastrados = _fabricanteRepositorio.ObterTodos().ToList();

            foreach (var documento in documentosImportador)
            {
                //var sexoId = 0;
                //if (documento.PacienteEnumSexoBiologico == "M")
                //{
                //    sexoId = 1;
                //}
                //else if (documento.PacienteEnumSexoBiologico == "F")
                //{
                //    sexoId = 2;
                //}
                //else
                //{
                //    sexoId = 3;
                //}

                //var nacionalidadeId = 0;
                //if (documento.PacienteNacionalidadeEnumNacionalidade == "B")
                //{
                //    nacionalidadeId = 1;
                //}
                //else
                //{
                //    nacionalidadeId = 2;
                //}

                var paciente = pacientesCadastrados.Single(p => p.Guid == documento.PacienteId);
                var estabelecimento = estabelecimentosAdicionadas.Single(p => p.Valor == documento.EstabelecimentoValor);
                var sistema = sistemasCadastrados.Single(p => p.Nome == documento.SistemaOrigem);
                var grupoAtendimento = grupoAtendimentosCadastrados.Single(p => p.Id == documento.VacinaGrupoAtendimentoCodigo);
                var categoria = categoriasAdicionadas.Single(p => p.Id == documento.VacinaCategoriaCodigo);
                var fabricante = fabricantesCadastrados.Single(p => p.Nome == documento.VacinaFabricanteNome);

                DocumentoVacinacao documentoVacinacao = new DocumentoVacinacao()
                {
                    Guid = documento.DocumentId,
                    PacienteId = paciente.Id,
                    EstabelecimentoId = estabelecimento.Id,
                    SistemaOrigemId = sistema.Id,
                    Nome = documento.VacinaNome,
                    GrupoAtendimentoId = grupoAtendimento.Id,
                    CategoriaId = categoria.Id,
                    DataAplicacao = DateTime.SpecifyKind(documento.VacinaDataAplicacao, DateTimeKind.Utc),
                    DescricaoDose = documento.VacinaDescricaoDose,
                    FabricanteId = fabricante.Id,
                    Lote = documento.VacinaLote,
                    VacinaCodigo = documento.VacinaCodigo
                };

                documenetVacinacaos.Add(documentoVacinacao);

                contador++;
                if (contador >= 5000)
                {
                    _documentoVacinacaoRepositorio.AdicionarConjunto(documenetVacinacaos);
                    documenetVacinacaos.Clear();
                    contador = 0;
                }
            }
            //_pacienteRepositorio.AdicionarConjunto(pacientesCadastrados);
            _documentoVacinacaoRepositorio.AdicionarConjunto(documenetVacinacaos);
        }

        private Sistema CadastrarERetornarSistema(DocumentoImportadoCSV documento)
        {
            var sistema = new Sistema()
            {
                Id = _sistemaId,
                Nome = documento.SistemaOrigem
            };


            //_sistemaRepositorio.Adicionar(sistema);

            //var sistemaAdicionado = _sistemaRepositorio.Obter(s => s.Nome == documento.SistemaOrigem).SingleOrDefault();

            //if (sistemaAdicionado != null)
            //{
            //    sistema.Id = sistemaAdicionado.Id;
            //}

            _sistemaId++;
            sistemasCadastrados.Add(sistema);

            return sistema;
        }

        private GrupoAtendimento CadastrarERetornarGrupoAtendimento(DocumentoImportadoCSV documento)
        {
            var grupoAtendimento = new GrupoAtendimento()
            {
                Id = documento.VacinaGrupoAtendimentoCodigo,
                Nome = documento.VacinaGrupoAtendimentoNome
            };

            grupoAtendimentosCadastrados.Add(grupoAtendimento);
            //_grupoAtendimentoRepositorio.Adicionar(grupoAtendimento);

            return grupoAtendimento;
        }

        private Fabricante CadastrarERetornarFabricante(DocumentoImportadoCSV documento)
        {
            var fabricante = new Fabricante()
            {
                Id = _idFabricante,
                Nome = documento.VacinaFabricanteNome,
                Referencia = documento.VacinaFabricanteReferencia
            };

            _idFabricante++;

            fabricantesCadastrados.Add(fabricante);

            //_fabricanteRepositorio.Adicionar(fabricante);
            //var fabricanteAdicionado = _fabricanteRepositorio.Obter(s => s.Nome == documento.VacinaFabricanteNome).SingleOrDefault();

            //if (fabricanteAdicionado != null)
            //{
            //    fabricante.Id = fabricanteAdicionado.Id;
            //}

            return fabricante;
        }

        private Categoria CadastrarERetornarCategoria(DocumentoImportadoCSV documento)
        {

            var categoria = new Categoria()
            {
                Id = documento.VacinaCategoriaCodigo,
                Nome = documento.VacinaCategoriaNome
            };

            categoriasAdicionadas.Add(categoria);

            //_categoriaRepositorio.Adicionar(categoria);

            return categoria;

        }

        private Estabelecimento CadastrarERetornarEstabelecimento(DocumentoImportadoCSV documento, int municipioId)
        {

            var estabelecimento = new Estabelecimento()
            {
                Id = _idEstabelecimento,
                MunicipioId = municipioId,
                NomeFantasia = documento.EstabelecimentoNoFantasia,
                RazaoSocial = documento.EstabelecimentoRazaoSocial,
                Valor = documento.EstabelecimentoValor
            };

            _idEstabelecimento++;
            //_estabelecimentoRepositorio.Adicionar(estabelecimento);

            estabelecimentosAdicionadas.Add(estabelecimento);

            //var estabelecimentoCadastrado = _estabelecimentoRepositorio.Obter(p => p.NomeFantasia == estabelecimento.NomeFantasia).SingleOrDefault();

            return estabelecimento;

        }

        private Paciente CadastrarERetornarPaciente(DocumentoImportadoCSV documento, int sexoId, Endereco endereco, int nacionalidadeId)
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
            //_pacienteRepositorio.Adicionar(paciente);

            //var pacienteCadastrado = _pacienteRepositorio.Obter(p => p.Guid == documento.PacienteId).SingleOrDefault();

            return paciente;
        }

        private Endereco CadastrarERetornarEndereco(DocumentoImportadoCSV documento, int municipio)
        {

            var endereco = new Endereco()
            {
                Id = _idEndereco,
                MunicipioId = municipio,
                CEP = documento.PacienteEnderecoCEP
            };

            _idEndereco++;
            enderecosAdicionadas.Add(endereco);
            //_enderecoRepositorio.Adicionar(endereco);

            //return _enderecoRepositorio.Obter(p => p.CEP == documento.PacienteEnderecoCEP).SingleOrDefault();

            return endereco;
        }

        private Municipio CadastrarERetornarMunicipio(DocumentoImportadoCSV documento, int pais)
        {

            var municipio = new Municipio()
            {
                Id = _idMunicipio,
                CodigoIBGE = documento.PacienteEnderecoCoibgeMunicipio,
                Nome = documento.PacienteEnderecoNmMunicipio,
                UF = documento.PacienteEnderecoUF,
                PaisId = pais
            };

            _idMunicipio++;
            municipiosAdicionadas.Add(municipio);
            //_municipioRepositorio.Adicionar(municipio);

            //return _municipioRepositorio.Obter(p => p.CodigoIBGE == municipio.CodigoIBGE).SingleOrDefault();
            return municipio;
        }
    }
}
