using Financeiro.Domain.Enums;
using Financeiro.Domain.Exceptions;
using financeiro.Domain.Events;
using financeiro.Domain.Common;

namespace Financeiro.Domain.Entities;

public class Movimentacao : Entity
{
    public long Id { get; private set; }
    public int UsuarioId { get; private set; }
    public int OrcamentoId {  get; private set; }
    public string? Tag { get; private set; }
    public string? Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public Categoria Categoria { get; private set; }
    public DateTime DataMovimentacao { get; private set; }
    public TipoMovimentacaoEnum Tipo { get; private set; }

    public Movimentacao(
        decimal valor,
        Categoria categoria,
        int orcamentoId,
        int usuarioId,
        string? Descricao,
        string? Tag
        )
    {
        DefinirCategoria(categoria);
        DefinirValor(valor);
        DefinirOrcamentoId(orcamentoId);
        DefinirUsuarioId(usuarioId);
        DefinirDataMovimentacao(null);
        AlterarDescricao(Descricao);
        AlterarTag(Tag);

        AddDomainEvent(new MovimentacaoCriadaEvent(this));
    }

    public static Movimentacao CriarMovimentacao(
        decimal valor,
        Categoria categoria,
        int orcamentoId,
        int usuarioId,
        TipoMovimentacaoEnum tipo,
        string? descricao,
        string? tag)
    {
        return new Movimentacao(valor, categoria, orcamentoId, usuarioId, descricao, tag);
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

    private void DefinirUsuarioId(int orcamentoId)
    {
        if (orcamentoId <= 0)
            throw new DomainException("Não foi encontrado usuário para essa movimentação.");

        UsuarioId = orcamentoId;
    }

    public void AlterarCategoria(Categoria novaCategoria)
    {
        if (novaCategoria == null)
            throw new DomainException("A categoria não pode ser nula.");

        if (this.Categoria == novaCategoria)
            return;

        if (!novaCategoria.EhCompativelCom(this.Tipo))
            throw new DomainException("Categoria incompatível com o tipo da movimentação.");

        this.Categoria = novaCategoria;

        AddDomainEvent(new MovimentacaoReclassificadaEvent(
            Id,
            UsuarioId,
            novaCategoria.Id
        ));
    }

    private void DefinirCategoria(Categoria categoria)
    {
        if (categoria == null)
            throw new DomainException("A categoria não pode ser nula.");

        if (categoria.Tipo != this.Tipo)
        {
            throw new DomainException($"A categoria '{categoria.Descricao}' é incompatível com o tipo da movimentação ({this.Tipo}).");
        }

        this.CategoriaId = categoria.Id;
    }
    private void DefinirOrcamentoId(int orcamentoId)
    {
        if (orcamentoId <= 0)
            throw new DomainException("Orçamento inválido para a movimentação.");

        OrcamentoId = orcamentoId;

    }

    public void AlterarTipo(TipoMovimentacaoEnum tipo)
    {
        if (!CategoriaPolicy.CategoriaEhCompativel(Categoria, tipo))
            throw new DomainException("Tipo incompatível com a categoria.");

        Tipo = tipo;
    }
}
