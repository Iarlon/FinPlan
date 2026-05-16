using Financeiro.Domain.Enums;
using Financeiro.Domain.Exceptions;
using Financeiro.Domain.Policies;
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
    public CategoriaEnum Categoria { get; private set; }
    public DateTime DataMovimentacao { get; private set; }
    public TipoMovimentacaoEnum Tipo { get; private set; }

    public Movimentacao(
        decimal valor,
        CategoriaEnum categoria,
        int orcamentoId,
        int usuarioId,
        TipoMovimentacaoEnum tipo,
        string? Descricao,
        string? Tag
        )
    {
        AlterarTipo(tipo);
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
        CategoriaEnum categoria,
        int orcamentoId,
        int usuarioId,
        TipoMovimentacaoEnum tipo,
        string? descricao,
        string? tag)
    {
        return new Movimentacao(valor, categoria, orcamentoId, usuarioId, tipo, descricao, tag);
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

    private void AlterarCategoria(CategoriaEnum novaCategoria)
    {
        if (Categoria == novaCategoria)
            return;

        if (!CategoriaPolicy.CategoriaEhCompativel(novaCategoria, Tipo))
            throw new DomainException("Categoria incompatível com o tipo da movimentação.");

        var categoriaAntiga = Categoria;
        Categoria = novaCategoria;

        AddDomainEvent(new MovimentacaoReclassificadaEvent(
            Id,
            UsuarioId,
            categoriaAntiga,
            novaCategoria
        ));
    }

    private void DefinirCategoria(CategoriaEnum categoria)
    {
        if (!CategoriaPolicy.CategoriaEhCompativel(categoria, Tipo))
            throw new DomainException("Categoria incompatível com o tipo da movimentação.");

        Categoria = categoria;
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
