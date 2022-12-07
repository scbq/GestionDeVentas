using GestionDeVentas.Models;
using System.Data;
using System.Data.SqlClient;

namespace GestionDeVentas.Repository
{
    public class ProductoVendidoRepository
    {
        private SqlConnection conexion;
        private String cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=ajomuch92_coderhouse_csharp_40930;" +
            "User Id=ajomuch92_coderhouse_csharp_40930;" +
            "Password=ElQuequit0Sexy2022;";

        public ProductoVendidoRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<ProductoVendido> listarProductoVendido()
        {
            List<ProductoVendido> lista = new List<ProductoVendido>();
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM productovendido", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductoVendido productoVendido = obtenerProductoVendidoDesdeReader(reader);
                                lista.Add(productoVendido);
                            }
                        }
                    }
                }
                conexion.Close();
            }
            catch (Exception ex)
            {

            }
            return lista;
        }

        private ProductoVendido obtenerProductoVendidoDesdeReader(SqlDataReader reader)
        {
            ProductoVendido productoVendido = new ProductoVendido();
            productoVendido.Id = long.Parse(reader["Id"].ToString());
            productoVendido.IdProducto = int.Parse(reader["IdProducto"].ToString());
            productoVendido.IdVenta = long.Parse(reader["IdVenta"].ToString());
            productoVendido.Stock =int.Parse(reader["Stock"].ToString());
            return productoVendido;
        }

        public ProductoVendido? obtenerProductoVendido(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM productovendido WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            ProductoVendido productoVendido = obtenerProductoVendidoDesdeReader(reader);
                            return productoVendido;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public ProductoVendido crearProductoVendido(ProductoVendido productoVendido)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO productovendido(IdProducto, IdVenta, Stock) VALUES(@IdProducto, @IdVenta, @Stock); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("IdProducto", SqlDbType.Int) { Value = productoVendido.IdProducto});
                    cmd.Parameters.Add(new SqlParameter("IdVenta", SqlDbType.BigInt) { Value = productoVendido.IdVenta});
                    cmd.Parameters.Add(new SqlParameter("Stock", SqlDbType.Int) { Value = productoVendido.Stock});
                    productoVendido.Id = long.Parse(cmd.ExecuteScalar().ToString());
                    return productoVendido;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public bool eliminarProductoVendido(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM productovendido WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
                conexion.Close();
                return filasAfectadas > 0;
            }
            catch
            {
                throw;
            }
        }

        public ProductoVendido? ActualizarProductoVendido(long id, ProductoVendido productoVendidoAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion establecida");
            }
            try
            {
                ProductoVendido? productoVendido = obtenerProductoVendido(id);
                if (productoVendido == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (productoVendido.IdProducto != productoVendidoAActualizar.IdProducto && productoVendidoAActualizar.IdProducto > 0)
                {
                    camposAActualizar.Add("IdProducto = @idproducto");
                    productoVendido.IdProducto = productoVendidoAActualizar.IdProducto;
                }
                if (productoVendido.IdVenta != productoVendidoAActualizar.IdVenta && productoVendidoAActualizar.IdVenta > 0)
                {
                    camposAActualizar.Add("IdVenta = @idventa");
                    productoVendido.IdVenta = productoVendidoAActualizar.IdVenta;
                }
                if (productoVendido.Stock != productoVendidoAActualizar.Stock && productoVendidoAActualizar.Stock >= 0)
                {
                    camposAActualizar.Add("Stock = @stock");
                    productoVendido.Stock = productoVendidoAActualizar.Stock;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("Ningun campo fue actualizado");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE productovendido SET {String.Join(", ", camposAActualizar)} WHERE id=@id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("idproducto", SqlDbType.BigInt) { Value = productoVendidoAActualizar.IdProducto });
                    cmd.Parameters.Add(new SqlParameter("idventa", SqlDbType.BigInt) { Value = productoVendidoAActualizar.IdVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoVendidoAActualizar.Stock });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return productoVendido;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }
    }
}
