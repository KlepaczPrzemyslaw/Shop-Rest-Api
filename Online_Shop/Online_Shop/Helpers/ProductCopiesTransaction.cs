using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Helpers
{
	public class ProductCopiesTransaction
	{
		SqlConnection sqlConnection;
		SqlTransaction transaction;
		SqlCommand sqlCommand = new SqlCommand();

		/// <summary>
		/// 	Konstruktor -> otwiera transakcję
		/// </summary>

		public ProductCopiesTransaction()
		{
			sqlConnection = SqlConnectionHelper.GetConnection();
			this.transaction = sqlConnection.BeginTransaction();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.Transaction = transaction;
		}

		/// <summary>
		/// 	Potwierdzenie transakcji
		/// </summary>

		public void TransactionCommit()
		{
			transaction.Commit();
			sqlConnection.Close();
		}

		/// <summary>
		/// 	Anulowanie transakcji
		/// </summary>

		public void TransactionRollback()
		{
			transaction.Rollback();
			sqlConnection.Close();
		}

		/// <summary>
		/// 	Dodaje 1 Kopię produktu
		/// </summary>
		/// <param name="productModel"></param>
		/// <param name="price"></param>
		/// <returns>
		/// 	true OK / false Exception
		/// </returns>

		public bool InsertSingleProductCopy(ProductModel productModel, decimal price)
		{
			try
			{
				// Zapytanie 
				sqlCommand.CommandText = "INSERT INTO SingleProductCopy VALUES (@ID, @ProductID, @Price, null, null, null)";

				// Parametry
				SqlParameter sqlIDParam = new SqlParameter { DbType = System.Data.DbType.Guid, Value = new Guid(), ParameterName = "@ID" };
				sqlCommand.Parameters.Add(sqlIDParam);

				SqlParameter sqlProductIDParam = new SqlParameter { DbType = System.Data.DbType.Guid, Value = productModel.Id, ParameterName = "@ProductID" };
				sqlCommand.Parameters.Add(sqlProductIDParam);

				SqlParameter sqlPriceParam = new SqlParameter { DbType = System.Data.DbType.Decimal, Value = price, ParameterName = "@Price" };
				sqlCommand.Parameters.Add(sqlPriceParam);

				// Execute
				sqlCommand.ExecuteNonQuery();

				// Usunięcie parametrów
				sqlCommand.Parameters.Remove(sqlIDParam);
				sqlCommand.Parameters.Remove(sqlProductIDParam);
				sqlCommand.Parameters.Remove(sqlPriceParam);

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userModel"></param>
		/// <param name="singleProductCopyModel"></param>
		/// <returns>
		/// 	true OK / false Exception
		/// </returns>

		public bool UpdateSingleProductCopy(UserModel userModel, SingleProductCopyModel singleProductCopyModel)
		{
			try
			{
				// Zapytanie 
				sqlCommand.CommandText = "UPDATE SingleProductCopy SET UserId = @UserId, UserName = @UserName, PurchasedAt = GETDATE() WHERE ID = @ID";

				// Parametry
				SqlParameter sqlIDParam = new SqlParameter { DbType = System.Data.DbType.Guid, Value = singleProductCopyModel, ParameterName = "@ID" };
				sqlCommand.Parameters.Add(sqlIDParam);

				SqlParameter sqlUserIdParam = new SqlParameter { DbType = System.Data.DbType.Guid, Value = userModel.Id, ParameterName = "@UserId" };
				sqlCommand.Parameters.Add(sqlUserIdParam);

				SqlParameter sqlUserNameParam = new SqlParameter { DbType = System.Data.DbType.String, Value = userModel.Name, ParameterName = "@UserName" };
				sqlCommand.Parameters.Add(sqlUserNameParam);

				// Execute
				sqlCommand.ExecuteNonQuery();

				// Usunięcie parametrów
				sqlCommand.Parameters.Remove(sqlIDParam);
				sqlCommand.Parameters.Remove(sqlUserIdParam);
				sqlCommand.Parameters.Remove(sqlUserNameParam);

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="singleProductCopyModel"></param>
		/// <returns>
		/// 	true OK / false Exception
		/// </returns>

		public bool CancelSingleProductCopy(SingleProductCopyModel singleProductCopyModel)
		{
			try
			{
				// Zapytanie 
				sqlCommand.CommandText = "UPDATE SingleProductCopy SET UserId = null, UserName = null, PurchasedAt = null WHERE ID = @ID";

				// Parametry
				SqlParameter sqlIDParam = new SqlParameter { DbType = System.Data.DbType.Guid, Value = singleProductCopyModel, ParameterName = "@ID" };
				sqlCommand.Parameters.Add(sqlIDParam);

				// Execute
				sqlCommand.ExecuteNonQuery();

				// Usunięcie parametrów
				sqlCommand.Parameters.Remove(sqlIDParam);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
