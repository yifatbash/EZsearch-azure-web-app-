using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using EZsearch3.Models;
using System.Configuration;

namespace EZsearch3.DAL
{
    public class DAL_DB 
    {
        /// <summary>
        /// Saves the search details into tbl_history on ezsearchDB, connecting DB.
        /// </summary>
        /// <param name="search">A search entity. Contains the value and the date will be enter to the DB.</param>
        /// <returns>True- if added successfully, else- False.</returns>
        public static bool AddSearchEntity(SearchEnt search)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ezsearchDB"].ConnectionString;
            var query = "INSERT INTO tbl_history (Txt, SearchDate) VALUES (@value, @date)";
            //query = query.Replace("@value", '\''+ search.Value+ '\'').Replace("@date", '\''+search.Date.ToString("MM / dd / yyyy HH: mm:ss", CultureInfo.InvariantCulture)+'\'');
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@date", search.Date);
                command.Parameters.AddWithValue("@value", search.Value);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return true;

            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Upadates the date of value is already in DB, connecting DB.
        /// </summary>
        /// <param name="search">A search entity. Contains current date.</param>
        /// <returns>True- if updated successfully, else- False.</returns>
        public static bool UpdateSearchDate(SearchEnt search)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["ezsearchDB"].ConnectionString;
            var query = "Update tbl_history SET SearchDate=@date where txt=@value";
          //  query = query.Replace("@date", '\'' + search.Date.ToString("MM / dd / yyyy HH: mm:ss", CultureInfo.InvariantCulture) + '\'').Replace("@value", '\''+search.Value.ToString() + '\'');
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@date", search.Date);
                command.Parameters.AddWithValue("@value", search.Value.ToString());
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return true;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns list of all the previous searches, order by date, connecting DB.
        /// </summary>
        /// <returns>List of search entities</returns>
        public static IEnumerable<SearchEnt> GetHistoryList()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ezsearchDB"].ConnectionString;
            var query = "SELECT Txt FROM tbl_history ORDER BY SearchDate DESC";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                List<SearchEnt> historyList = new List<SearchEnt>();

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SearchEnt search = new SearchEnt();
                        search.Value = reader.GetString(0);
                        historyList.Add(search);
                    }
                    reader.NextResult();
                }
                command.Dispose();
                connection.Close();
                return historyList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
    

                