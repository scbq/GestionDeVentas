namespace GestionDeVentas.Models
{
    public class ProductoVendido
    {
        public long Id { get; set; }
        public long IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Stock { get; set; }

        public ProductoVendido() 
        {
            Id = 0;
            IdProducto = 0;
            IdVenta = 0;
            Stock = 0;
        }

        public ProductoVendido(long id, int idproducto, long idventa, int stock)
        {
            Id = id;
            IdProducto = idproducto;
            IdVenta = idventa;
            Stock = stock;
        }
    }
}
