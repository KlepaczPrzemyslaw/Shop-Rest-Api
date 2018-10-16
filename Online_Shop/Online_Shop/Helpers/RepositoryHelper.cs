using Online_Shop.DAO.Repositories;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Helpers
{
	public static class RepositoryHelper
	{
		///////////////////////////////////////////////////////////////////////////////
		// Single Copies Of Products //
		///////////////////////////////////////////////////////////////////////////////

		public static async Task<SingleProductCopyModel> FindCopyOrFailAsync(this IProductRepository repository, Guid id, ProductModel productModel)
		{
			SingleProductCopyModel product = await repository.GetSingleProductCopyAsync(id, productModel);
			if (product == null)
			{
				throw new Exception($"Kopia produktu o ID: {id} - nie istnieje!!!");
			}

			return product;
		}

		///////////////////////////////////////////////////////////////////////////////
		// Product //
		///////////////////////////////////////////////////////////////////////////////

		public static async Task<ProductModel> FindOrFailAsync(this IProductRepository repository, Guid id)
		{
			ProductModel product = await repository.GetAsync(id);
			if (product == null)
			{
				throw new Exception($"Produkt o ID: {id} - nie istnieje!!!");
			}

			return product;
		}

		public static async Task<ProductModel> FindOrFailAsync(this IProductRepository repository, string name)
		{
			ProductModel product = await repository.GetAsync(name);
			if (product == null)
			{
				throw new Exception($"Produkt o nazwie: {name} - nie istnieje!!!");
			}

			return product;
		}

		public static async Task<ProductModel> GetOrFailAsync(this IProductRepository repository, Guid id)
		{
			ProductModel product = await repository.GetAsync(id);
			if (product != null)
			{
				throw new Exception($"Produkt o ID: {id} - już istnieje!!!");
			}

			return product;
		}

		public static async Task<ProductModel> GetOrFailAsync(this IProductRepository repository, string name)
		{
			ProductModel product = await repository.GetAsync(name);
			if (product != null)
			{
				throw new Exception($"Produkt o nazwie: {name} - już istnieje!!!");
			}

			return product;
		}

		///////////////////////////////////////////////////////////////////////////////
		// User //
		///////////////////////////////////////////////////////////////////////////////

		public static async Task<UserModel> GetOrFailAsync(this IUserRepository repository, string email)
		{
			UserModel user = await repository.GetAsync(email);
			if (user != null)
			{
				throw new Exception($"User o adresie email: {email} - już istnieje!!!");
			}

			return user;
		}

		public static async Task<UserModel> LoginOrFailAsync(this IUserRepository repository, string email, string password)
		{
			UserModel user = await repository.GetAsync(email);
			if (user == null)
			{
				throw new Exception($"Niepoprawne dane logowania!!!");
			}

			// Proste sprawdzenie poprawnośći hasła -> bez zabezpieczeń!
			if (user.Password != password)
			{
				throw new Exception($"Niepoprawne dane logowania!!!");
			}

			return user;
		}

		public static async Task<UserModel> FindOrFailAsync(this IUserRepository repository, Guid id)
		{
			UserModel user = await repository.GetAsync(id);
			if (user == null)
			{
				throw new Exception($"User o id: {id} - nie istnieje!!!");
			}

			return user;
		}
	}
}
