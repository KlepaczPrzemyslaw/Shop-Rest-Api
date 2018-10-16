using Online_Shop.Helpers;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DAO
{
	public static class ProductDAO
	{
		/// <summary>
		/// 	Pobranie produktu po ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		/// 	ProductModel / null
		/// <returns>

		public static ProductModel GetProduct(Guid id)
		{
			SqlCommand sqlCommand = new SqlCommand
			{
				// Zapytanie
				CommandText = "SELECT * FROM Products WHERE ID = @ID"
			};
			// Dodanie parametru
			AddIDParam(sqlCommand, id);

			return GetProduct(sqlCommand);
		}

		/// <summary>
		/// 	Pobiera produkt po Name
		/// </summary>
		/// <param name="name"></param>
		/// <returns>
		/// 	ProductModel / null
		/// </returns>

		public static ProductModel GetProduct(string name)
		{
			SqlCommand sqlCommand = new SqlCommand
			{
				// Zapytanie
				CommandText = "SELECT * FROM Products WHERE name = @ExactlyName"
			};
			// Dodanie parametru
			AddExactlyNameParam(sqlCommand, name);

			return GetProduct(sqlCommand);
		}

		/// <summary>
		/// 	Pobiera listę produktów (z opcjonalnym parametrem)
		/// </summary>
		/// <param name="name"></param>
		/// <returns>
		/// 	List<ProductModel>
		/// </returns>

		public static List<ProductModel> GetProducts(string name = null)
		{
			List<ProductModel> list = new List<ProductModel>();

			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				SqlCommand sqlCommand = new SqlCommand
				{
					Connection = connection
				};

				// Zapytanie bez parametru
				if (string.IsNullOrWhiteSpace(name))
				{
					sqlCommand.CommandText = "SELECT * FROM Products";
				}
				// Zapytanie z Name
				else
				{
					sqlCommand.CommandText = "SELECT * FROM Products WHERE Name LIKE @Name";
					AddNameParam(sqlCommand, name);
				}

				// Zapytanie
				SqlDataReader data = sqlCommand.ExecuteReader();

				// Uzupełnienie listy
				while (data.HasRows && data.Read())
				{
					list.Add(new ProductModel(
						Guid.Parse(data["ID"].ToString()),
						data["Name"].ToString(),
						data["Description"].ToString(),
						(DateTime)data["CreatedAt"],
						(DateTime)data["UpdatedAt"]
						));
				}
			}

			// Uzupełnienie ogólnej liczby pojedyńczych sztuk (amount)
			// osobny foreach bo "gryzą" się SqlDataReader-y
			foreach (ProductModel product in list)
			{
				using (SqlConnection connection = SqlConnectionHelper.GetConnection())
				{
					SqlCommand sqlCommand = new SqlCommand
					{
						Connection = connection
					};

					// Zapytanie 
					sqlCommand.CommandText = "SELECT COUNT(*) FROM SingleProductCopy WHERE ProductID = @ID AND UserId IS NULL;";
					// Parametry
					AddIDParam(sqlCommand, product.Id);

					SqlDataReader data = sqlCommand.ExecuteReader();
					while (data.HasRows && data.Read())
					{
						product.UpdateAmount(data.GetInt32(0));
					}
				}
			}

			return list;
		}

		// INSERT - TRANSAKCJA

		/// <summary>
		/// 	Dodaje nowy wpis -> Transakcja
		/// </summary>
		/// <param name="productModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool Add(ProductModel productModel)
		{
			SqlCommand sqlCommand = new SqlCommand();

			// Zapytanie
			sqlCommand.CommandText = @"INSERT INTO Products VALUES (@ID, @Name, @Description, @CreatedAt, @UpdatedAt);";

			// Dodanie parametru ID
			AddIDParam(sqlCommand, productModel.Id);
			// Dodanie parametru nazwy
			AddNameParam(sqlCommand, productModel.Name);
			// // Dodanie parametru opisu 
			AddDescParams(sqlCommand, productModel);
			// Dodanie parametrów dla obu dat
			AddDateParams(sqlCommand, productModel);

			// Transakcja
			return SqlTransactionTool.Transaction(sqlCommand);
		}

		// UPDATE - TRANSAKCJA

		/// <summary>
		/// 	Edytuje wpis -> Transakcja
		/// </summary>
		/// <param name="productModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool Update(ProductModel productModel)
		{
			SqlCommand sqlCommand = new SqlCommand();

			// Zapytanie
			sqlCommand.CommandText = @"UPDATE Products SET Name = @Name, Description = @Description, UpdatedAt = GETDATE() WHERE ID = @ID";

			// Dodanie parametru ID
			AddIDParam(sqlCommand, productModel.Id);
			// Dodanie parametru nazwy
			AddNameParam(sqlCommand, productModel.Name);
			// Dodanie parametrów opisu i ceny
			AddDescParams(sqlCommand, productModel);

			// Transakcja
			return SqlTransactionTool.Transaction(sqlCommand);
		}

		// UPDATE AMOUNT - TRANSAKCJA

		/// <summary>
		/// 	Edytuje wpis -> Transakcja
		/// </summary>
		/// <param name="productModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool UpdateCopiesAmount(ProductModel productModel, int amount, decimal price)
		{
			// Obiekt transakcji - transakcja już rozpoczęta
			ProductCopiesTransaction pCT = new ProductCopiesTransaction();

			try
			{
				for (int i = 0; i < amount; i++)
				{
					// Dodawanie z warunkiem
					if (pCT.InsertSingleProductCopy(productModel, price) == false)
						throw new Exception("Wyjątek przy dodawaniu kopii produktów -> Przy transakcji coś poszło nie tak!!!");
				}

				pCT.TransactionCommit();
				return true;
			}
			catch
			{
				pCT.TransactionRollback();
				return false;
			}
		}

		// DELETE - TRANSAKCJA

		/// <summary>
		/// 	Usuń wpis -> Transakcja
		/// </summary>
		/// <param name="productModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool Delete(ProductModel productModel)
		{
			var sqlCommand = new SqlCommand();

			// Zapytanie
			sqlCommand.CommandText = @"DELETE FROM Products WHERE ID = @ID;";

			// Dodanie parametru ID
			AddIDParam(sqlCommand, productModel.Id);

			// Transakcja
			return SqlTransactionTool.Transaction(sqlCommand);
		}

		// UPDATE - PURCHASE

		/// <summary>
		/// 	Update na Bazie - kupuje produkty
		/// </summary>
		/// <param name="userModel"></param>
		/// <param name="productModel"></param>
		/// <param name="amount"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool PurchaseAsync(UserModel userModel, ProductModel productModel, int amount)
		{
			int availableAmount = 0;
			List<SingleProductCopyModel> Copies = new List<SingleProductCopyModel>();
			ProductCopiesTransaction pCT = new ProductCopiesTransaction();

			// Walidacja
			if (userModel == null)
				throw new Exception($"Nie podano żadnego konta użytkownika!!");

			if (productModel == null)
				throw new Exception($"Nie podano żadnego produktu!!");

			// Sprawdzenie ilości kopii
			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				SqlCommand sqlCommand = new SqlCommand
				{
					Connection = connection
				};

				sqlCommand.CommandText = "SELECT COUNT(*) FROM SingleProductCopy WHERE ProductID = @ID AND UserId IS NULL;";
				AddIDParam(sqlCommand, productModel.Id);

				SqlDataReader data = sqlCommand.ExecuteReader();
				while (data.HasRows && data.Read())
				{
					availableAmount = data.GetInt32(0);
				}
			}

			// Porównanie
			if (availableAmount < amount)
				throw new Exception("Brak wymaganej ilości!!!");

			// Pobranie konkretnej ilości do listy
			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				SqlCommand sqlCommand = new SqlCommand
				{
					Connection = connection
				};

				sqlCommand.CommandText = "SELECT * FROM SingleProductCopy WHERE ProductID = @ID AND UserId IS NULL ORDER BY ID FETCH NEXT @Amount ROWS ONLY;;";
				AddIDParam(sqlCommand, productModel.Id);

				SqlParameter sqlAmountParam = new SqlParameter { DbType = System.Data.DbType.Int32, Value = amount, ParameterName = "@Amount" };
				sqlCommand.Parameters.Add(sqlAmountParam);

				SqlDataReader data = sqlCommand.ExecuteReader();
				while (data.HasRows && data.Read())
				{
					Copies.Add(new SingleProductCopyModel
						(productModel,
						Guid.Parse(data["ID"].ToString()),
						(decimal)data["Price"],
						null, null, null));
				}
			}

			// Transakcja
			try
			{
				for (int i = 0; i < amount; i++)
				{
					if (pCT.UpdateSingleProductCopy(userModel, Copies[i]) == false)
						throw new Exception("Wyjątek przy dodawaniu kopii produktów -> Przy transakcji coś poszło nie tak!!!");
				}

				pCT.TransactionCommit();
				return true;
			}
			catch
			{
				pCT.TransactionRollback();
				return false;
			}
		}

		// UPDATE -> CANCEL

		/// <summary>
		/// 	Update na Bazie - anuluje kupione produkty
		/// </summary>
		/// <param name="userModel"></param>
		/// <param name="productModel"></param>
		/// <param name="amount"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool CancelAsync(UserModel userModel, ProductModel productModel, int amount)
		{
			int purchasedAmount = 0;
			List<SingleProductCopyModel> Copies = new List<SingleProductCopyModel>();
			ProductCopiesTransaction pCT = new ProductCopiesTransaction();

			// Walidacja
			if (userModel == null)
				throw new Exception($"Nie podano żadnego konta użytkownika!!");

			if (productModel == null)
				throw new Exception($"Nie podano żadnego produktu!!");

			// Sprawdzenie kupionych kopii
			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				SqlCommand sqlCommand = new SqlCommand
				{
					Connection = connection
				};

				sqlCommand.CommandText = "SELECT COUNT(*) FROM SingleProductCopy WHERE ProductID = @ID AND UserId = @UserId;";
				AddIDParam(sqlCommand, productModel.Id);

				SqlParameter sqlUserIdParam = new SqlParameter { DbType = System.Data.DbType.Guid, Value = userModel.Id, ParameterName = "@UserId" };
				sqlCommand.Parameters.Add(sqlUserIdParam);

				SqlDataReader data = sqlCommand.ExecuteReader();
				while (data.HasRows && data.Read())
				{
					purchasedAmount = data.GetInt32(0);
				}
			}

			// Porównanie
			if (purchasedAmount < amount)
				throw new Exception("Brak wymaganej ilości!!!");

			// Pobranie konkretnej ilości do listy
			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				SqlCommand sqlCommand = new SqlCommand
				{
					Connection = connection
				};

				sqlCommand.CommandText = "SELECT * FROM SingleProductCopy WHERE ProductID = @ID AND UserId = @UserId ORDER BY ID FETCH NEXT @Amount ROWS ONLY;;";
				AddIDParam(sqlCommand, productModel.Id);

				SqlParameter sqlAmountParam = new SqlParameter { DbType = System.Data.DbType.Int32, Value = amount, ParameterName = "@Amount" };
				sqlCommand.Parameters.Add(sqlAmountParam);

				SqlParameter sqlUserIdParam = new SqlParameter { DbType = System.Data.DbType.Guid, Value = userModel.Id, ParameterName = "@UserId" };
				sqlCommand.Parameters.Add(sqlUserIdParam);

				SqlDataReader data = sqlCommand.ExecuteReader();
				while (data.HasRows && data.Read())
				{
					Copies.Add(new SingleProductCopyModel
						(productModel,
						Guid.Parse(data["ID"].ToString()),
						(decimal)data["Price"],
						null, null, null));
				}
			}

			// Transakcja
			try
			{
				for (int i = 0; i < amount; i++)
				{
					if (pCT.CancelSingleProductCopy(Copies[i]) == false)
						throw new Exception("Wyjątek przy dodawaniu kopii produktów -> Przy transakcji coś poszło nie tak!!!");
				}

				pCT.TransactionCommit();
				return true;
			}
			catch
			{
				pCT.TransactionRollback();
				return false;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Metody dla pojedyńczyc kopii //
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// 	Pobierz pojedyńczą kopię po ID produktu i ID jego kopii
		/// </summary>
		/// <param name="singleProductCopy"></param>
		/// <param name="productModel"></param>
		/// <returns>
		/// 	SingleProductCopyModel / null
		/// </returns>

		public static SingleProductCopyModel GetSingleProductCopy(Guid productCopyID, ProductModel productModel)
		{
			SingleProductCopyModel model = null;

			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				SqlCommand sqlCommand = new SqlCommand
				{
					// Zapytanie
					CommandText = "SELECT * FROM SingleProductCopy WHERE ID = @ID"
				};
				// Parametr ID
				AddIDParam(sqlCommand, productCopyID);

				sqlCommand.Connection = connection;

				// Zapytanie
				SqlDataReader data = sqlCommand.ExecuteReader();

				// Uzupełnienie listy
				while (data.HasRows && data.Read())
				{
					model = new SingleProductCopyModel
						(productModel,
						Guid.Parse(data["ID"].ToString()),
						(decimal)data["Price"],
						data["UserId"] == DBNull.Value ? null : (Guid?)Guid.Parse(data["ID"].ToString()),
						data["UserName"] == DBNull.Value ? null : data["UserName"].ToString(),
						data["PurchasedAt"] == DBNull.Value ? null : (DateTime?)data["PurchasedAt"]
						);
				}
			}

			return model;
		}

		/// <summary>
		/// 	Pobranie listy kopii produktu
		/// </summary>
		/// <param name="userModel"></param>
		/// <param name="productModel"></param>
		/// <returns>
		/// 	List<SingleProductCopyModel>
		/// </returns>

		public static List<SingleProductCopyModel> GetSingleProductCopies(UserModel userModel, ProductModel productModel)
		{
			List<SingleProductCopyModel> models = new List<SingleProductCopyModel>();

			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				SqlCommand sqlCommand = new SqlCommand
				{
					// Zapytanie
					CommandText = "SELECT * FROM SingleProductCopy WHERE UserID = @ID"
				};
				// Parametr
				AddIDParam(sqlCommand, userModel.Id);

				sqlCommand.Connection = connection;

				// Zapytanie
				SqlDataReader data = sqlCommand.ExecuteReader();

				// Uzupełnienie listy
				while (data.HasRows && data.Read())
				{
					models.Add(new SingleProductCopyModel
						(productModel,
						Guid.Parse(data["ID"].ToString()),
						(decimal)data["Price"],
						data["UserId"] == DBNull.Value ? null : (Guid?)Guid.Parse(data["ID"].ToString()),
						data["UserName"] == DBNull.Value ? null : data["UserName"].ToString(),
						data["PurchasedAt"] == DBNull.Value ? null : (DateTime?)data["PurchasedAt"]
						));
				}
			}

			return models;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Prywatne metody //
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// 	Pobiera obiekty pojedyńczych produktów
		/// </summary>
		/// <returns>
		/// 	ISet<SingleProductCopyModel>
		/// </returns>

		private static ISet<SingleProductCopyModel> GetProductSingleCopies(ProductModel productModel)
		{
			ISet<SingleProductCopyModel> productCopies = new HashSet<SingleProductCopyModel>();

			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;

				// Zapytanie
				sqlCommand.CommandText = "SELECT * FROM SingleProductCopy WHERE ProductID = @ID";
				// Parametr
				AddIDParam(sqlCommand, productModel.Id);

				// Zapytanie
				SqlDataReader data = sqlCommand.ExecuteReader();

				// Uzupełnienie ISet
				while (data.HasRows && data.Read())
				{
					productCopies.Add(new SingleProductCopyModel
						(productModel,
						Guid.Parse(data["ID"].ToString()),
						(decimal)data["Price"],
						data["UserId"] == DBNull.Value ? null : (Guid?)Guid.Parse(data["ID"].ToString()),
						data["UserName"] == DBNull.Value ? null : data["UserName"].ToString(),
						data["PurchasedAt"] == DBNull.Value ? null : (DateTime?)data["PurchasedAt"]
						));
				}
			}

			return productCopies;
		}

		/// <summary>
		/// 	DRY / Helper do GetProduct (ID i Name)
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <returns></returns>

		private static ProductModel GetProduct(SqlCommand sqlCommand)
		{
			ProductModel productModel = null;
			ISet<SingleProductCopyModel> productCopies = new HashSet<SingleProductCopyModel>();

			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				sqlCommand.Connection = connection;

				// Zapytanie
				SqlDataReader data = sqlCommand.ExecuteReader();

				// Uzupełnienie listy
				while (data.HasRows && data.Read())
				{
					productModel = new ProductModel(
						Guid.Parse(data["ID"].ToString()),
						data["Name"].ToString(),
						data["Description"].ToString(),
						(DateTime)data["CreatedAt"],
						(DateTime)data["UpdatedAt"]
						);
				}
			}

			// Update na produkcie - Dodanie konkretnych kopii do IEnumerable
			productModel.LoadSingleCopies(GetProductSingleCopies(productModel));

			return productModel;
		}

		/// <summary>
		/// 	Dodanie ID
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="id"></param>

		private static void AddIDParam(SqlCommand sqlCommand, Guid? id)
		{
			SqlParameter sqlIDParam = new SqlParameter { DbType = System.Data.DbType.Guid, Value = id, ParameterName = "@ID" };
			sqlCommand.Parameters.Add(sqlIDParam);
		}

		/// <summary>
		/// 	Dodanie Name -> dla funkcji przeszukujących
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="name"></param>

		private static void AddNameParam(SqlCommand sqlCommand, string name)
		{
			SqlParameter sqlNameParam = new SqlParameter { DbType = System.Data.DbType.String, Value = $"%{name}%", ParameterName = "@Name" };
			sqlCommand.Parameters.Add(sqlNameParam);
		}

		/// <summary>
		/// 	Dodanie name -> Dla szukania dokładnie po nazwie
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="exactlyName"></param>

		private static void AddExactlyNameParam(SqlCommand sqlCommand, string exactlyName)
		{
			SqlParameter sqlExactlyNameParam = new SqlParameter { DbType = System.Data.DbType.String, Value = exactlyName, ParameterName = "@ExactlyName" };
			sqlCommand.Parameters.Add(sqlExactlyNameParam);
		}

		/// <summary>
		/// 	Dodanie parametrów dat - CreatedAt i UpdatedAt
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="productModel"></param>

		private static void AddDateParams(SqlCommand sqlCommand, ProductModel productModel)
		{
			SqlParameter sqlCreatedAtParam = new SqlParameter { DbType = System.Data.DbType.DateTime, Value = productModel.CreatedAt, ParameterName = "@CreatedAt" };
			sqlCommand.Parameters.Add(sqlCreatedAtParam);

			SqlParameter sqlUpdatedAtParam = new SqlParameter { DbType = System.Data.DbType.DateTime, Value = productModel.UpdatedAt, ParameterName = "@UpdatedAt" };
			sqlCommand.Parameters.Add(sqlUpdatedAtParam);
		}

		/// <summary>
		/// 	Dodanie parametrów - Description i Price
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="productModel"></param>

		private static void AddDescParams(SqlCommand sqlCommand, ProductModel productModel)
		{
			SqlParameter sqlDescriptionParam = new SqlParameter { DbType = System.Data.DbType.String, Value = productModel.Description, ParameterName = "@Description" };
			sqlCommand.Parameters.Add(sqlDescriptionParam);
		}
	}
}
