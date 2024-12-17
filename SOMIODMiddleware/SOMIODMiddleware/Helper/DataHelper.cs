using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;
using SOMIODMiddleware.Models;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace SOMIODMiddleware.Helper
{
    public class DataHelper
    {


        /// Obtém dados genéricos da base de dados e os mapeia para uma lista de objetos do tipo T

        public static List<T> GetDataFromDatabase<T>(string query, T dataObject)
        {
            List<T> list = new List<T>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOMIODMiddleware.Properties.Settings.ConnStr"].ConnectionString))
                {
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.CommandType = CommandType.Text;

                        if (dataObject != null)
                        {
                            AddParametersToCommand(command, dataObject);
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            list = DataReaderMapToList<T>(reader);
                            reader.Close();
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Logar ou tratar o erro
                throw new Exception("Erro ao acessar o banco de dados.", ex);
            }

            return list;
        }



        /// Executa uma operação de transação na base de dados e retorna o ID gerado

        public static int TransactWithDatabase<T>(string query, T dataObject)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOMIODMiddleware.Properties.Settings.ConnStr"].ConnectionString))

            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.Text;

                    if (dataObject != null)
                    {
                        AddParametersToCommand(command, dataObject);
                    }

                    object result = command.ExecuteScalar(); // Obtém o ID gerado, se existir
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
                conn.Close();
            }
            return 0;
        }

        
        /// Mapeia os resultados do SqlDataReader para uma lista de objetos do tipo T
        
        private static List<T> DataReaderMapToList<T>(IDataReader dataReader)
        {
            List<T> objectList = new List<T>();

            while (dataReader.Read())
            {
                T newObject = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    try
                    {
                        if (!Equals(dataReader[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(newObject, dataReader[prop.Name]);
                        }
                    }
                    catch
                    {
                        
                        continue;
                    }
                }
                objectList.Add(newObject);
            }
            return objectList;
        }

        
        /// Adiciona os parâmetros de um objeto ao comando SQL
       
        private static void AddParametersToCommand<T>(SqlCommand command, T dataObject)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                object propValue = prop.GetValue(dataObject);

                // Define a data atual para propriedades "creation_dt"
                if (prop.Name == "creation_datetime")
                {
                    propValue = DateTime.Now;
                }

                if (propValue != null)
                {
                    command.Parameters.AddWithValue("@" + prop.Name, propValue);
                }
            }
        }
    }
}