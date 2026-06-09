using Financeiro.Domain.Enums;
using Financeiro.Domain.Exceptions;
using financeiro.Domain.Events;
using financeiro.Domain.Common;

namespace Financeiro.Domain.Entities;

public class Movimentacao : Entity
{
    public long Id { get; private set; }
    public long UsuarioId { get; private set; }
    public long OrcamentoId {  get; private set; }
    public string? Tag { get; private set; }
    public string? Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public Categoria Categoria { get; private set; }
    public DateTime DataMovimentacao { get; private set; }
    public TipoMovimentacaoEnum Tipo => Categoria.Tipo;

    public Movimentacao(
        decimal valor,
        Categoria categoria,
        long orcamentoId,
        long usuarioId,
        string? Descricao,
        DateTime dataMovimentacao,
        string? Tag
        )
    {
        DefinirCategoria(categoria);
        DefinirValor(valor);
        DefinirOrcamentoId(orcamentoId);
        DefinirUsuarioId(usuarioId);
        AlterarDescricao(Descricao);
        DefinirDataMovimentacao(dataMovimentacao);
        AlterarTag(Tag);

        AddDomainEvent(new MovimentacaoCriadaEvent(this));
    }

    public static Movimentacao CriarMovimentacao(
        decimal valor,
        DateTime dataMovimentacao,
        Categoria categoria,
        long orcamentoId,
        long usuarioId,
        string? descricao,
        string? tag)
    {
        return new Movimentacao(valor, categoria, orcamentoId, usuarioId, descricao, dataMovimentacao, tag);
    }
    public void DefinirId(long id)
    {
        if (id <= 0)
            throw new DomainException("Id inválido.");

        Id = id;
    }
    public void AlterarTag(string tag)
    {
        Tag = tag?.Trim();
    }

    public void AlterarDescricao(string descricao)
    {
        Descricao = descricao?.Trim();
    }

    private void DefinirValor(decimal valor)
    {
        if (valor <= 0)
            throw new DomainException("O valor deve ser maior que zero.");
        Valor = valor;
    }

    public void AlterarValor(decimal valor)
    {
        if (valor <= 0)
            throw new DomainException("O valor deve ser maior que zero.");
        Valor = valor;
    }

    public void AlterarData(DateTime? data)
    {
        DataMovimentacao = data ?? DateTime.UtcNow;
    }
    private void DefinirDataMovimentacao(DateTime? data)
    {
        DataMovimentacao = data ?? DateTime.UtcNow;
    }

    private void DefinirUsuarioId(long orcamentoId)
    {
        if (orcamentoId <= 0)
            throw new DomainException("Não foi encontrado usuário para essa movimentação.");

        UsuarioId = orcamentoId;
    }

    public void AlterarCategoria(Categoria novaCategoria)
    {
        if (novaCategoria == null)
            throw new DomainException("A categoria não pode ser nula.");

        if (Categoria == novaCategoria)
            return;

        Categoria = novaCategoria;

        AddDomainEvent(new MovimentacaoReclassificadaEvent(
            Id,
            UsuarioId,
            novaCategoria.Id
        ));
    }

    private void DefinirCategoria(Categoria categoria)
    {
        if (categoria is null)
            throw new DomainException("A categoria não pode ser nula.");

        if (Categoria != null && categoria.Tipo != Categoria.Tipo)
            throw new DomainException(
                $"Categoria '{categoria.Descricao}' é incompatível."
            );

        Categoria = categoria;
    }
    private void DefinirOrcamentoId(long orcamentoId)
    {
        if (orcamentoId <= 0)
            throw new DomainException("Orçamento inválido para a movimentação.");

        OrcamentoId = orcamentoId;
    }
}
