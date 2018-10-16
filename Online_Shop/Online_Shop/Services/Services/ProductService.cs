using Online_Shop.DAO;
using Online_Shop.DAO.Repositories;
using Online_Shop.DTO;
using Online_Shop.Models;
using Online_Shop.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Online_Shop.Helpers;

namespace Online_Shop.Services.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public ProductService(IProductRepository productRepository, IMapper mapper)
		{
			this._productRepository = productRepository;
			this._mapper = mapper;
		}

		/// <summary>
		/// 	Pobierz po ID -> Mapper
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		/// 	ProductDTO / null - w funkcji
		/// </returns>

		public async Task<ProductDetailsDTO> GetAsync(Guid id)
		{
			ProductModel productModel = await _productRepository.FindOrFailAsync(id);

			return _mapper.Map<ProductDetailsDTO>(productModel);
		}

		/// <summary>
		/// 	Pobierz po Name -> Mapper
		/// </summary>
		/// <param name="name"></param>
		/// <returns>
		/// 	ProductDTO / null - w funkcji
		/// </returns>

		public async Task<ProductDetailsDTO> GetAsync(string name)
		{
			ProductModel productModel = await _productRepository.FindOrFailAsync(name);

			return _mapper.Map<ProductDetailsDTO>(productModel);
		}

		/// <summary>
		/// 	Pobierz listę produktów -> Mapper
		/// </summary>
		/// <param name="name"></param>
		/// <returns>
		/// 	IEnumerable<ProductDTO> / null - w funkcji
		/// </returns>

		public async Task<IEnumerable<ProductDTO>> BrowseAsync(string name = null)
		{
			IEnumerable<ProductModel> productModels = await _productRepository.BrowseAsync(name);

			return _mapper.Map<IEnumerable<ProductDTO>>(productModels);
		}

		/// <summary>
		/// 	Dodaj azynchronicznie -> TRANSAKCJA
		/// </summary>
		/// <param name="productModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public async Task<bool> AddAsync(ProductModel productModel)
		{
			// Sprawdzenie ID
			ProductModel product = await _productRepository.GetOrFailAsync(productModel.Id);

			// Sprawdzenie nazwy
			product = await _productRepository.GetOrFailAsync(productModel.Name);

			// Dodanie
			return await _productRepository.AddAsync(productModel);
		}

		/// <summary>
		/// 	Dodaj kopie produktu pod ProductModel -> TRANSAKCJA
		/// </summary>
		/// <param name="id"></param>
		/// <param name="amount"></param>
		/// <param name="price"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public async Task<bool> AddProductCopiesAsync(Guid id, int amount, decimal price)
		{
			// Sprawdzenie ID
			ProductModel product = await _productRepository.FindOrFailAsync(id);

			return await _productRepository.UpdateCopiesAmountAsync(product, amount, price);
		}

		/// <summary>
		/// 	Edytuje produkt - jego dane -> TRANSAKCJA
		/// </summary>
		/// <param name="productModel"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public async Task<bool> UpdateAsync(ProductModel productModel)
		{
			// Sprawdzenie ID
			ProductModel product = await _productRepository.FindOrFailAsync(productModel.Id);

			// Sprawdzenie nowej nazwy czy nie istnieje
			product = await _productRepository.GetOrFailAsync(productModel.Name);

			return await _productRepository.UpdateAsync(productModel);
		}

		/// <summary>
		/// 	Usuwa produkt -> TRANSAKCJA
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		/// 	true - ok / false - rollback
		/// </returns>

		public async Task<bool> DeleteAsync(Guid id)
		{
			// Sprawdzenie ID
			ProductModel product = await _productRepository.FindOrFailAsync(id);

			return await _productRepository.DeleteAsync(product);
		}
	}
}
