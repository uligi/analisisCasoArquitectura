using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using modular1.Models;
using modular1.Conexion;

namespace modular1.Repositories
{
    public class ClienteRepository
    {
        // Obtener lista de clientes
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(ConexionDB.cn))
                {
                    string query = "SELECT IdCliente, Nombres, Apellidos, Correo, Clave, Reestablecer FROM CLIENTE";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Cliente>();
            }

            return lista;
        }

        // Registrar un nuevo cliente
        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(ConexionDB.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", oconexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Reestablecer", obj.Reestablecer);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }

            return idautogenerado;
        }

        // Editar un cliente existente
        public bool Editar(Cliente obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(ConexionDB.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarCliente", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", obj.IdCliente);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Reestablecer", obj.Reestablecer);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        // Eliminar un cliente
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(ConexionDB.cn))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM CLIENTE WHERE IdCliente = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }
    }
}
