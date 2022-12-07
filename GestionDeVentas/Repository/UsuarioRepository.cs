using GestionDeVentas.Models;
using System.Data;
using System.Data.SqlClient;

namespace GestionDeVentas.Repository
{
    public class UsuarioRepository
    {
        private SqlConnection conexion;
        private String cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=ajomuch92_coderhouse_csharp_40930;" +
            "User Id=ajomuch92_coderhouse_csharp_40930;" +
            "Password=ElQuequit0Sexy2022;";

        public UsuarioRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<Usuario> listarUsuario()
        {
            List<Usuario> lista = new List<Usuario>();
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM usuario", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Usuario usuario = obtenerUsuarioDesdeReader(reader);
                                lista.Add(usuario);
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

        private Usuario obtenerUsuarioDesdeReader(SqlDataReader reader)
        {
            Usuario usuario = new Usuario();
            usuario.Id = long.Parse(reader["Id"].ToString());
            usuario.Nombre = reader["Nombre"].ToString();
            usuario.Apellido = reader["Apellido"].ToString();
            usuario.NombreUsuario = reader["NombreUsuario"].ToString();
            usuario.Mail = reader["Mail"].ToString();
            return usuario;
        }

        public Usuario? obtenerUsuario(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM usuario WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Usuario usuario = obtenerUsuarioDesdeReader(reader);
                            return usuario;
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

        public Usuario crearUsuario(Usuario usuario)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO usuario(nombre, apellido, nombreusuario, contraseña, mail) VALUES(@nombre, @apellido, @nombreusuario, @contraseña, @mail); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Nombre", SqlDbType.VarChar) { Value = usuario.Nombre });
                    cmd.Parameters.Add(new SqlParameter("Apellido", SqlDbType.VarChar) { Value = usuario.Apellido });
                    cmd.Parameters.Add(new SqlParameter("NombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("Contraseña", SqlDbType.VarChar) { Value = usuario.Contraseña });
                    cmd.Parameters.Add(new SqlParameter("Mail", SqlDbType.VarChar) { Value = usuario.Mail });
                    usuario.Id = long.Parse(cmd.ExecuteScalar().ToString());
                    return usuario;
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

        public bool eliminarUsuario(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM usuario WHERE id = @id", conexion))
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

        public Usuario? ActualizarUsuario(long id, Usuario usuarioAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion establecida");
            }
            try
            {
                Usuario? usuario = obtenerUsuario(id);
                if (usuario == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (usuario.Nombre != usuarioAActualizar.Nombre && !string.IsNullOrEmpty(usuarioAActualizar.Nombre))
                {
                    camposAActualizar.Add("Nombre = @nombre");
                    usuario.Nombre = usuarioAActualizar.Nombre;
                }
                if (usuario.Apellido != usuarioAActualizar.Apellido && !string.IsNullOrEmpty(usuarioAActualizar.Apellido))
                {
                    camposAActualizar.Add("Apellido = @apellido");
                    usuario.Apellido = usuarioAActualizar.Apellido;
                }
                if (usuario.NombreUsuario != usuarioAActualizar.NombreUsuario && !string.IsNullOrEmpty(usuarioAActualizar.NombreUsuario))
                {
                    camposAActualizar.Add("NombreUsuario = @nombreusuario");
                    usuario.NombreUsuario = usuarioAActualizar.NombreUsuario;
                }
                if (usuario.Contraseña != usuarioAActualizar.Contraseña && !string.IsNullOrEmpty(usuarioAActualizar.Contraseña))
                {
                    camposAActualizar.Add("Contraseña = @contraseña");
                    usuario.Contraseña = usuarioAActualizar.Contraseña;
                }
                if (usuario.Mail != usuarioAActualizar.Mail && !string.IsNullOrEmpty(usuarioAActualizar.Mail))
                {
                    camposAActualizar.Add("Mail = @mail");
                    usuario.Mail = usuarioAActualizar.Mail;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("Ningun campo fue actualizado");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE usuario SET {String.Join(", ", camposAActualizar)} WHERE id=@id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuarioAActualizar.Nombre });
                    cmd.Parameters.Add(new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuarioAActualizar.Apellido });
                    cmd.Parameters.Add(new SqlParameter("nombreusuario", SqlDbType.VarChar) { Value = usuarioAActualizar.NombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("contraseña", SqlDbType.VarChar) { Value = usuarioAActualizar.Contraseña });
                    cmd.Parameters.Add(new SqlParameter("mail", SqlDbType.VarChar) { Value = usuarioAActualizar.Mail });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return usuario;
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
