using System.ComponentModel.DataAnnotations.Schema;

namespace SergipeVac.Model
{
    [Table("documentovacinacaoaracaju")]
    public class DocumentoImportadoCSV
    {
        [Column("document_id")]
        public string? DocumentId { get; set; }

        [Column("paciente_id")]
        public string PacienteId { get; set; }

        [Column("paciente_idade")]
        public int PacienteIdade { get; set; }

        [Column("paciente_datanascimento")]
        public DateTime PacienteDataNascimento { get; set; }

        [Column("paciente_enumsexobiologico")]
        public string PacienteEnumSexoBiologico { get; set; }

        [Column("paciente_racacor_codigo")]
        public int PacienteRacaCorCodigo { get; set; }

        [Column("paciente_racacor_valor")]
        public string PacienteRacaCorValor { get; set; }

        [Column("paciente_endereco_coibgemunicipio")]
        public int PacienteEnderecoCoibgeMunicipio { get; set; }

        [Column("paciente_endereco_copais")]
        public int PacienteEnderecoCoPais { get; set; }

        [Column("paciente_endereco_nmmunicipio")]
        public string PacienteEnderecoNmMunicipio { get; set; }

        [Column("paciente_endereco_nmpais")]
        public string PacienteEnderecoNmPais { get; set; }

        [Column("paciente_endereco_uf")]
        public string PacienteEnderecoUF { get; set; }

        [Column("paciente_endereco_cep")]
        public string PacienteEnderecoCEP { get; set; }

        [Column("paciente_nacionalidade_enumnacionalidade")]
        public string PacienteNacionalidadeEnumNacionalidade { get; set; }

        [Column("estabelecimento_valor")]
        public int EstabelecimentoValor { get; set; }

        [Column("estabelecimento_razaosocial")]
        public string EstabelecimentoRazaoSocial { get; set; }

        [Column("estalecimento_nofantasia")]
        public string EstabelecimentoNoFantasia { get; set; }

        [Column("estabelecimento_municipio_codigo")]
        public int EstabelecimentoMunicipioCodigo { get; set; }

        [Column("estabelecimento_municipio_nome")]
        public string EstabelecimentoMunicipioNome { get; set; }

        [Column("estabelecimento_uf")]
        public string EstabelecimentoUF { get; set; }

        [Column("vacina_grupoatendimento_codigo")]
        public int VacinaGrupoAtendimentoCodigo { get; set; }

        [Column("vacina_grupoatendimento_nome")]
        public string VacinaGrupoAtendimentoNome { get; set; }

        [Column("vacina_categoria_codigo")]
        public int VacinaCategoriaCodigo { get; set; }

        [Column("vacina_categoria_nome")]
        public string VacinaCategoriaNome { get; set; }

        [Column("vacina_lote")]
        public string VacinaLote { get; set; }

        [Column("vacina_fabricante_nome")]
        public string VacinaFabricanteNome { get; set; }

        [Column("vacina_fabricante_referencia")]
        public string VacinaFabricanteReferencia { get; set; }

        [Column("vacina_dataaplicacao")]
        public DateTime VacinaDataAplicacao { get; set; }

        [Column("vacina_descricao_dose")]
        public string VacinaDescricaoDose { get; set; }

        [Column("vacina_codigo")]
        public int VacinaCodigo { get; set; }

        [Column("vacina_nome")]
        public string VacinaNome { get; set; }

        [Column("sistema_origem")]
        public string SistemaOrigem { get; set; }
    }
}
