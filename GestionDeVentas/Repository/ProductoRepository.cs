using GestionDeVentas.Models;
using System.Data;
using System.Data.SqlClient;


namespace GestionDeVentas.Repository
{
    public class ProductoRepository
    {
        private SqlConnection conexion;
        private String cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=ajomuch92_coderhouse_csharp_40930;" +
            "User Id=ajomuch92_coderhouse_csharp_40930;" +
            "Password=ElQuequit0Sexy2022;";

        public ProductoRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<Producto> listarProducto()
        {
            List<Producto> lista = new List<Producto>();
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM producto", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Producto producto = obtenerProductoDesdeReader(reader);
                                lista.Add(producto);
                            }
                        }
                    }
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }

        private Producto obtenerProductoDesdeReader(SqlDataReader reader)
        {
            Producto producto = new Producto();
            producto.Id = long.Parse(reader["Id"].ToString());
            producto.Stock = int.Parse(reader["Stock"].ToString());
            producto.PrecioVenta = double.Parse(reader["PrecioVenta"].ToString());
            producto.Costo = double.Parse(reader["Costo"].ToString());
            producto.Descripciones = reader["Descripciones"].ToString();
            return producto;
        }

        public Producto? obtenerProducto(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM producto WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Producto producto = obtenerProductoDesdeReader(reader);
                            return producto;
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
        public Producto crearProducto(Producto producto)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Producto(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) VALUES(@descripciones, @costo, @precioVenta, @stock, @idUsuario); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Descripciones", SqlDbType.VarChar) { Value = producto.Descripciones });
                    cmd.Parameters.Add(new SqlParameter("Costo", SqlDbType.Float) { Value = producto.Costo });
                    cmd.Parameters.Add(new SqlParameter("PrecioVenta", SqlDbType.Float) { Value = producto.PrecioVenta });
                    cmd.Parameters.Add(new SqlParameter("Stock", SqlDbType.Int) { Value = producto.Stock });
                    cmd.Parameters.Add(new SqlParameter("IdUsuario", SqlDbType.BigInt) { Value = producto.IdUsuario });
                    producto.Id = long.Parse(cmd.ExecuteScalar().ToString());
                    return producto;
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

        public bool eliminarProducto(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM producto WHERE id = @id", conexion))
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

        public Producto? ActualizarProducto(long id , Producto productoAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion establecida");
            }
            try
            {
                Producto? producto = obtenerProducto(id);
                if (producto == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (producto.Descripciones != productoAActualizar.Descripciones && !string.IsNullOrEmpty(productoAActualizar.Descripciones))
                {
                    camposAActualizar.Add("Descripciones = @descripciones");
                    producto.Descripciones = productoAActualizar.Descripciones;
                }
                if (producto.Costo != productoAActualizar.Costo && productoAActualizar.Costo > 0)
                {
                    camposAActualizar.Add("Costo = @costo");
                    producto.Costo = productoAActualizar.Costo;
                }
                if (producto.PrecioVenta != productoAActualizar.PrecioVenta && productoAActualizar.PrecioVenta > 0)
                {
                    camposAActualizar.Add("PrecioVenta = @precioventa");
                    producto.PrecioVenta = productoAActualizar.PrecioVenta;
                }
                if (producto.Stock != productoAActualizar.Stock && productoAActualizar.Stock >= 0)
                {
                    camposAActualizar.Add("Stock = @stock");
                    producto.Stock = productoAActualizar.Stock;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("Ningun campo fue actualizado");
                }
                using(SqlCommand cmd = new SqlCommand($"UPDATE producto SET {String.Join(", ", camposAActualizar)} WHERE id=@id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("descripciones", SqlDbType.VarChar) { Value=productoAActualizar.Descripciones});
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = productoAActualizar.Costo});
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = productoAActualizar.PrecioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoAActualizar.Stock });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return producto;
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
