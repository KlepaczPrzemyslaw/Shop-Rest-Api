using Online_Shop.Helpers;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DAO
{
	public class UserDAO
	{
		/// <summary>
		/// 	Pobiera user-a po ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		/// 	UserModel / null
		/// </returns>

		public static UserModel GetUser(Guid id)
		{
			SqlCommand sqlCommand = new SqlCommand
			{
				// Zapytanie
				CommandText = "SELECT * FROM Users WHERE ID = @ID"
			};
			// Parametr
			AddIDParam(sqlCommand, id);

			return GetUser(sqlCommand);
		}

		/// <summary>
		/// 	Pobiera user-a po email-u
		/// </summary>
		/// <param name="email"></param>
		/// <returns>
		/// 	UserModel / null
		/// </returns>

		public static UserModel GetUser(string email)
		{
			SqlCommand sqlCommand = new SqlCommand
			{
				// Zapytanie
				CommandText = "SELECT * FROM Users WHERE Email = @ExactlyEmail"
			};
			// Parametr
			AddExactlyEmailParam(sqlCommand, email);

			return GetUser(sqlCommand);
		}

		// INSERT - TRANSAKCJA

		/// <summary>
		/// 	Dodaje nowy wpis -> Transakcja
		/// </summary>
		/// <param name="userModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool Add(UserModel userModel)
		{
			SqlCommand sqlCommand = new SqlCommand();

			// Zapytanie
			sqlCommand.CommandText = @"INSERT INTO Users VALUES (@ID, @AccountRole, @Name, @Email, @Password, @CreatedAt);";

			// Dodanie parametru ID
			AddIDParam(sqlCommand, userModel.Id);
			// Dodanie parametrów
			AddAccRoleEmailCreParams(sqlCommand, userModel);
			// Dodanie parametrów
			AddNamePassParams(sqlCommand, userModel);

			// Transakcja
			return SqlTransactionTool.Transaction(sqlCommand);
		}

		// UPDATE - TRANSAKCJA

		/// <summary>
		/// 	Edytuje wpis -> Transakcja
		/// </summary>
		/// <param name="userModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool Update(UserModel userModel)
		{
			SqlCommand sqlCommand = new SqlCommand();

			// Zapytanie
			sqlCommand.CommandText = @"UPDATE Users SET Name = @Name, Password = @Password WHERE ID = @ID";

			// Dodanie parametru ID
			AddIDParam(sqlCommand, userModel.Id);
			// Dodanie parametrów
			AddNamePassParams(sqlCommand, userModel);

			// Transakcja
			return SqlTransactionTool.Transaction(sqlCommand);
		}

		// DELETE - TRANSAKCJA

		/// <summary>
		/// 	Usuń wpis -> Transakcja
		/// </summary>
		/// <param name="userModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public static bool Delete(UserModel userModel)
		{
			var sqlCommand = new SqlCommand();

			// Zapytanie
			sqlCommand.CommandText = @"DELETE FROM Users WHERE ID = @ID;";

			// Dodanie parametru ID
			AddIDParam(sqlCommand, userModel.Id);

			// Transakcja
			return SqlTransactionTool.Transaction(sqlCommand);
		}

		////////////////////////////////////////////////////////////
		// Prywatne metody //
		////////////////////////////////////////////////////////////

		/// <summary>
		/// 	Pobiera user-a
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <returns>
		/// 	UserModel / null
		/// </returns>

		private static UserModel GetUser(SqlCommand sqlCommand)
		{
			UserModel userModel = null;

			using (SqlConnection connection = SqlConnectionHelper.GetConnection())
			{
				sqlCommand.Connection = connection;

				// Zapytanie
				SqlDataReader data = sqlCommand.ExecuteReader();

				// Uzupełnienie listy
				while (data.HasRows && data.Read())
				{
					userModel = new UserModel(
						Guid.Parse(data["ID"].ToString()),
						(Role)data["AccountRole"],
						data["Name"].ToString(),
						data["Email"].ToString(),
						data["Password"].ToString(),
						(DateTime)data["CreatedAt"]
						);
				}
			}

			return userModel;
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
		/// 	Dodanie name -> Dla szukania dokładnie po nazwie
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="exactlyName"></param>

		private static void AddExactlyEmailParam(SqlCommand sqlCommand, string exactlyEmail)
		{
			SqlParameter sqlExactlyEmailParam = new SqlParameter { DbType = System.Data.DbType.String, Value = exactlyEmail, ParameterName = "@ExactlyEmail" };
			sqlCommand.Parameters.Add(sqlExactlyEmailParam);
		}

		/// <summary>
		/// 	Dodanie parametrów -> Role i Email i CreatedAt
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="userModel"></param>

		private static void AddAccRoleEmailCreParams(SqlCommand sqlCommand, UserModel userModel)
		{
			SqlParameter sqlAccountRoleParam = new SqlParameter { DbType = System.Data.DbType.Int16, Value = Role.User, ParameterName = "@AccountRole" };
			sqlCommand.Parameters.Add(sqlAccountRoleParam);

			SqlParameter sqlEmailParam = new SqlParameter { DbType = System.Data.DbType.String, Value = userModel.Email, ParameterName = "@Email" };
			sqlCommand.Parameters.Add(sqlEmailParam);

			SqlParameter sqlCreatedAtParam = new SqlParameter { DbType = System.Data.DbType.String, Value = userModel.CreatedAt, ParameterName = "@CreatedAt" };
			sqlCommand.Parameters.Add(sqlCreatedAtParam);
		}

		/// <summary>
		/// 	Dodanie parametrów -> Name i Password
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="userModel"></param>

		private static void AddNamePassParams(SqlCommand sqlCommand, UserModel userModel)
		{
			SqlParameter sqlNameParam = new SqlParameter { DbType = System.Data.DbType.String, Value = userModel.Name, ParameterName = "@Name" };
			sqlCommand.Parameters.Add(sqlNameParam);

			SqlParameter sqlPasswordParam = new SqlParameter { DbType = System.Data.DbType.String, Value = userModel.Password, ParameterName = "@Password" };
			sqlCommand.Parameters.Add(sqlPasswordParam);
		}
	}
}
