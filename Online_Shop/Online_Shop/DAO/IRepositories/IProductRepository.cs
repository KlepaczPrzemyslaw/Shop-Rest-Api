using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DAO.Repositories
{
	public interface IProductRepository
	{
		Task<ProductModel> GetAsync(Guid id);
		Task<ProductModel> GetAsync(string name);
		Task<IEnumerable<ProductModel>> BrowseAsync(string name = null);

		Task<SingleProductCopyModel> GetSingleProductCopyAsync(Guid singleCopyID, ProductModel productModel);
		Task<IEnumerable<SingleProductCopyModel>> GetSingleProductCopiesAsync(UserModel userModel, ProductModel productModel);

		Task<bool> AddAsync(ProductModel productModel);
		Task<bool> UpdateAsync(ProductModel productModel);
		Task<bool> UpdateCopiesAmountAsync(ProductModel productModel, int amount, decimal price);
		Task<bool> DeleteAsync(ProductModel productModel);

		Task<bool> PurchaseAsync(UserModel userModel, ProductModel productModel, int amount);
		Task<bool> Cancel(UserModel userModel, ProductModel productModel, int amount);
	}
}
