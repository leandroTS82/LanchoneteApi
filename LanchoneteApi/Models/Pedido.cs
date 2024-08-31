public class Pedido
{
    public int Id { get; set; }
    public int DepartamentoId { get; set; }
    public Departamento Departamento { get; set; }
    public DateTime DataCriacao { get; set; }
    public string? Periodo { get; set; }
    public string? NomeCliente { get; set; }
    public string? StatusPedido { get; set; } // Liberado, Pendente, Cancelado
    public bool Pago { get; set; }
    public decimal ValorTotal { get; set; }
    public string? TipoPagamento { get; set; } // Dinheiro, Pix, Cartão, Pagar Depois
    public List<Produto> Produtos { get; set; }
}
