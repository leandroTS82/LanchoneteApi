public class Produto
{
    public int ProdutoId { get; set; }
    public string Descricao { get; set; }
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorTotal => ValorUnitario * Quantidade;
}
