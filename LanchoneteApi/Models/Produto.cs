public class Produto
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorTotal => ValorUnitario * Quantidade;
}
