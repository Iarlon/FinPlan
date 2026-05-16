using Financeiro.Domain.Enums;
using Financeiro.Domain.Exceptions;

namespace Financeiro.Domain.Entities;

public class Meta
{
    public long Id { get; private set; }
    public DateTime DataFinal { get; private set; }
    public int OrcamentoId { get; private set; }
    public PeriodicidadeAporteEnum Periodicidade { get; private set; }
    public string Descricao { get; private set; }
    public decimal ValorObjetivo { get; private set; }
    public decimal ValorAporte { get; private set; }
    public int UsuarioId { get; private set; }
    public StatusAtualEnum Status { get; private set; }

    public Meta(
        decimal valorObjetivo,
        DateTime dataFinal,
        PeriodicidadeAporteEnum periodicidade,
        string descricao,
        int usuarioId,
        int orcamentoId,
        decimal valorAporte) 
    {
        DefinirValorFinal(valorObjetivo);
        DefinirDataFinal(dataFinal);
        DefinirPeriodicidade(periodicidade);
        DefinirDescricao(descricao);
        DefinirUsuarioId(usuarioId);
        DefinirOrcamento(orcamentoId);
        DefinirAporte(valorAporte);

        ValorAporte = decimal.Zero;
        Status = StatusAtualEnum.NaMeta;
    }

    private void DefinirValorFinal(decimal valorObjetivo) 
    {
        if (valorObjetivo <= 0)
            throw new DomainException("O valor objetivo da meta deve ser maior que zero.");
        
        ValorObjetivo = valorObjetivo;
    }

    private void DefinirDataFinal(DateTime dataFinal)
    {
        if (dataFinal <= DateTime.UtcNow)
            throw new DomainException("A data final da meta deve ser uma data futura.");
        
        DataFinal = dataFinal;
    }

    private void DefinirDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("A descrição da meta não pode ser vazia.");
        
        Descricao = descricao.Trim();
    }

    private void DefinirPeriodicidade(PeriodicidadeAporteEnum periodicidade)
    {
        if (!Enum.IsDefined(typeof(PeriodicidadeAporteEnum), periodicidade))
            throw new DomainException("Periodicidade inválida.");

        Periodicidade = periodicidade;
    }

    private void DefinirUsuarioId(int usuarioId)
    {
        if (usuarioId <= 0)
            throw new DomainException("Não foi encontrado usuário válido para essa meta.");
        
        UsuarioId = usuarioId;
    }

    private void DefinirOrcamento(int orcamentoId)
    {
        if (orcamentoId <= 0)
            throw new DomainException("Orçamento inválido.");
            
        OrcamentoId = orcamentoId;
    }

    private void DefinirAporte(decimal valorAporte)
    {
        if (valorAporte <= 0)
            throw new DomainException("O valor do aporte adicionado deve ser maior que zero.");
            
        ValorAporte = valorAporte;
    }
    
    public void AlterarValorAporte(decimal valorAporte)
    {
        if (valorAporte <= 0)
            throw new DomainException("O valor do aporte adicionado deve ser maior que zero.");
            
        ValorAporte += valorAporte;
    }

    public void AtualizarStatusAtual(StatusAtualEnum status)
    {
        if (!Enum.IsDefined(typeof(StatusAtualEnum), status))
            throw new DomainException("Status inválido.");
            
        Status = status;
    }

    public void AtualizarPeriodicidade(PeriodicidadeAporteEnum periodicidade)
    {
        if (!Enum.IsDefined(typeof(PeriodicidadeAporteEnum), periodicidade))
            throw new DomainException("Periodicidade inválida.");

        Periodicidade = periodicidade;
    }

    public void AtualizarDataFinal(DateTime dataFinal)
    {
        if (dataFinal <= DateTime.UtcNow)
            throw new DomainException("A data final da meta deve ser uma data futura.");

        DataFinal = dataFinal;
    }

    public void AtualizarValorObjetivo(decimal valorObjetivo)
    {
        if (valorObjetivo <= 0)
            throw new DomainException("O valor objetivo da meta deve ser maior que zero.");
        ValorObjetivo = valorObjetivo;
    }
     public void AtualizarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("A descrição da meta não pode ser vazia.");
        Descricao = descricao.Trim();
    }
}
