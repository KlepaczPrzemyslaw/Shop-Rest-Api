using Online_Shop.DAO.Repositories;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DAO
{
	public class ProductRepository : IProductRepository
	{
		public async Task<ProductModel> GetAsync(Guid id)
			=> await Task.FromResult(ProductDAO.GetProduct(id));

		public async Task<ProductModel> GetAsync(string name)
			=> await Task.FromResult(ProductDAO.GetProduct(name));

		public async Task<IEnumerable<ProductModel>> BrowseAsync(string name = null)
			=> await Task.FromResult(ProductDAO.GetProducts(name).AsEnumerable());



		public async Task<SingleProductCopyModel> GetSingleProductCopyAsync(Guid singleCopyID, ProductModel productModel)
			=> await Task.FromResult(ProductDAO.GetSingleProductCopy(singleCopyID, productModel));

		public async Task<IEnumerable<SingleProductCopyModel>> GetSingleProductCopiesAsync(UserModel userModel, ProductModel productModel)
			=> await Task.FromResult(ProductDAO.GetSingleProductCopies(userModel, productModel).AsEnumerable());



		public async Task<bool> AddAsync(ProductModel productModel)
			=> await Task.FromResult(ProductDAO.Add(productModel));
		
		public async Task<bool> UpdateAsync(ProductModel productModel)
			=> await Task.FromResult(ProductDAO.Update(productModel));

		public async Task<bool> UpdateCopiesAmountAsync(ProductModel productModel, int amount, decimal price)
			=> await Task.FromResult(ProductDAO.UpdateCopiesAmount(productModel, amount, price));

		public async Task<bool> DeleteAsync(ProductModel productModel)
			=> await Task.FromResult(ProductDAO.Delete(productModel));



		public async Task<bool> PurchaseAsync(UserModel userModel, ProductModel productModel, int amount)
			=> await Task.FromResult(ProductDAO.PurchaseAsync(userModel, productModel, amount));
		
		public async Task<bool> Cancel(UserModel userModel, ProductModel productModel, int amount)
			=> await Task.FromResult(ProductDAO.CancelAsync(userModel, productModel, amount));
	}
}
