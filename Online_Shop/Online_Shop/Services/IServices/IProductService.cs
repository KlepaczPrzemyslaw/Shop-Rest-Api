using Online_Shop.DAO;
using Online_Shop.DTO;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Services.IServices
{
	public interface IProductService
	{
		Task<ProductDetailsDTO> GetAsync(Guid id);
		Task<ProductDetailsDTO> GetAsync(string name);
		Task<IEnumerable<ProductDTO>> BrowseAsync(string name = null);

		Task<bool> AddProductCopiesAsync(Guid id, int amount, decimal price);

		Task<bool> AddAsync(ProductModel productModel);
		Task<bool> UpdateAsync(ProductModel productModel);
		Task<bool> DeleteAsync(Guid id);
	}
}
