using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Helpers
{
	public static class SqlConnectionHelper
	{
		private static string connectionString = "Integrated Security=SSPI;" +
								 "Data Source=.\\SQLEXPRESS;" +
								 "Initial Catalog=OnlineShop;";

		/// <summary>
		/// 	Zwraca obiekt połączony z bazą - połączenie jest otwarte!
		/// </summary>
		/// <returns>
		/// 	SqlConnection
		/// </returns>

		public static SqlConnection GetConnection()
		{
			// Obiekt
			SqlConnection conn = new SqlConnection(connectionString);
			// Otwarcie połączenia
			conn.Open();
			// Zwrotka
			return conn;
		}
	}
}
