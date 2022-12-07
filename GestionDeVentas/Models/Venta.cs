namespace GestionDeVentas.Models
{
    public class Venta
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public string Comentarios { get; set; }

        public Venta() 
        {
            Id = 0;
            IdUsuario = 0;
            Comentarios = "";
        }

        public Venta(long id, long idusuario, string comentarios)
        {
            Id = id;
            IdUsuario = idusuario;
            Comentarios = comentarios;
        }
    }
}
