using GestionDeVentas.Models;
using System.Data;
using System.Data.SqlClient;

namespace GestionDeVentas.Repository
{
    public class VentaRepository
    {
        private SqlConnection conexion;
        private String cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=ajomuch92_coderhouse_csharp_40930;" +
            "User Id=ajomuch92_coderhouse_csharp_40930;" +
            "Password=ElQuequit0Sexy2022;";

        public VentaRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<Venta> listarVenta()
        {
            List<Venta> lista = new List<Venta>();
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM venta", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Venta venta = obtenerVentaDesdeReader(reader);
                                lista.Add(venta);
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

        private Venta obtenerVentaDesdeReader(SqlDataReader reader)
        {
            Venta venta = new Venta();
            venta.Id = long.Parse(reader["Id"].ToString());
            venta.IdUsuario = long.Parse(reader["IdUsuario"].ToString());
            venta.Comentarios = reader["Comentarios"].ToString();
            return venta;
        }

        public Venta? obtenerVenta(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM venta WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Venta venta = obtenerVentaDesdeReader(reader);
                            return venta;
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

        public Venta crearVenta(Venta venta)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Venta(Comentarios, IdUsuario) VALUES(@comentarios, @idUsuario); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Comentarios", SqlDbType.VarChar) { Value = venta.Comentarios });
                    cmd.Parameters.Add(new SqlParameter("IdUsuario", SqlDbType.BigInt) { Value = venta.IdUsuario });
                    venta.Id = long.Parse(cmd.ExecuteScalar().ToString());
                    return venta;
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

        public bool eliminarVenta(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Venta WHERE id = @id", conexion))
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

        public Venta? ActualizarVenta(long id, Venta ventaAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion establecida");
            }
            try
            {
                Venta? venta = obtenerVenta(id);
                if (venta == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (venta.Comentarios != ventaAActualizar.Comentarios && !string.IsNullOrEmpty(ventaAActualizar.Comentarios))
                {
                    camposAActualizar.Add("Comentarios = @comentarios");
                    venta.Comentarios = ventaAActualizar.Comentarios;
                }
                if (venta.IdUsuario != ventaAActualizar.IdUsuario && ventaAActualizar.IdUsuario > 0)
                {
                    camposAActualizar.Add("Costo = @costo");
                    venta.IdUsuario = ventaAActualizar.IdUsuario;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("Ningun campo fue actualizado");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE venta SET {String.Join(", ", camposAActualizar)} WHERE id=@id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("comentarios", SqlDbType.VarChar) { Value = ventaAActualizar.Comentarios });
                    cmd.Parameters.Add(new SqlParameter("idusuario", SqlDbType.BigInt) { Value = ventaAActualizar.IdUsuario });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return venta;
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
