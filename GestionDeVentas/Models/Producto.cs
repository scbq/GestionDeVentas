using Microsoft.Extensions.Hosting;

namespace GestionDeVentas.Models
{
    public class Producto
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public double Costo { get; set; }
        public double PrecioVenta { get; set; }
        public int Stock { get; set; }
        public string Descripciones { get; set; }

        public Producto()
        {
            Id = 0;
            IdUsuario = 0;
            Costo = 0;
            PrecioVenta = 0;
            Stock = 0;
            Descripciones = "";
        }
        public Producto(long id, long idUsuario, double costo, double precioVenta, int stock, string descripciones)
        {
            Id = id;
            IdUsuario = idUsuario;
            Costo = costo;
            PrecioVenta = precioVenta;
            Stock = stock;
            Descripciones = descripciones;
        }

    }
}
