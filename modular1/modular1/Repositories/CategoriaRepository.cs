using modular1.Conexion;
using modular1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace modular1.Repositories
{
    public class CategoriaRepository
    {

        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(ConexionDB.cn))
                {
                    string query = "SELECT IdCategoria, Descripcion, Activo, FechaRegistro FROM CATEGORIA";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                FechaRegistro = dr["FechaRegistro"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaRegistro"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Categoria>();
            }

            return lista;
        }


        // Método para registrar una nueva categoría
        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(ConexionDB.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCategoria", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;
                Mensaje = ex.Message;
            }

            return idAutogenerado;
        }

        // Método para editar una categoría existente
        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(ConexionDB.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarCategoria", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdCategoria", obj.IdCategoria);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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


        // Método para eliminar una categoría
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(ConexionDB.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdCategoria", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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
    }
}