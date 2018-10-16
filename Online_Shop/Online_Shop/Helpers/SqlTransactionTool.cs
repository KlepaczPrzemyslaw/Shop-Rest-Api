﻿using Online_Shop.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DAO
{
	public static class SqlTransactionTool
	{
		/// <summary>
		/// 	Transakcja -> wymaga obiektu SqlCommand z wpisaną komendą i podpiętymi parametrami! (true - ok / false - rollback)
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool Transaction(SqlCommand sqlCommand)
		{
			int queryValue;
			SqlTransaction transaction = null;

			using (var sqlConnection = SqlConnectionHelper.GetConnection())
			{
				try
				{
					// Dodanie komendy do połączenia
					sqlCommand.Connection = sqlConnection;
					// Rozpoczęcie transakcji
					transaction = sqlConnection.BeginTransaction();
					// Dodanie transakcji do komendy
					sqlCommand.Transaction = transaction;
					// Wykonanie zapytania
					queryValue = sqlCommand.ExecuteNonQuery();
					// Exception jeżeli zapytanie zwróci 0 lub 1 dla zmienionych wierszy
					// 1 - dlatego że mamy bazę logów - a tam zapytanie wykona się zawsze
					// Więc - poprawna transakcja zwróci zawsze 2 wiersze - 1 dla usunięcia np. nieistniejącej wartości - 0 dla błędu
					if (queryValue == 0 || queryValue == 1)
						throw new Exception();

					// Commit
					transaction.Commit();
					return true;
				}
				catch (Exception)
				{
					transaction.Rollback();
					return false;
				}
			}
		}
	}
}
