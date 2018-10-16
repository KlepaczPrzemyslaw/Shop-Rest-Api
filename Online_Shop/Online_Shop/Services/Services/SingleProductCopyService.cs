using AutoMapper;
using Online_Shop.DAO.Repositories;
using Online_Shop.DTO;
using Online_Shop.Helpers;
using Online_Shop.Models;
using Online_Shop.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Services.Services
{
	public class SingleProductCopyService : ISingleProductCopyService
	{
		private readonly IUserRepository _userRepository;
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public SingleProductCopyService(IUserRepository userRepository, IProductRepository productRepository, IMapper mapper)
		{
			this._userRepository = userRepository;
			this._productRepository = productRepository;
			this._mapper = mapper;
		}

		/// <summary>
		/// 	Pobiera 1 kopię z danymi
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="productId"></param>
		/// <param name="singleCopyId"></param>
		/// <returns>
		/// 	SingleProductCopyDTO / null - w funkcji
		/// </returns>

		public async Task<SingleProductCopyDTO> GetSingleCopyAsync(Guid userId, Guid productId, Guid singleCopyId)
		{
			var user = await _userRepository.FindOrFailAsync(userId);
			var product = await _productRepository.FindOrFailAsync(productId);
			var SingleProductCopyModel = await _productRepository.FindCopyOrFailAsync(userId, product);

			return _mapper.Map<SingleProductCopyDTO>(SingleProductCopyModel);
		}

		/// <summary>
		/// 	Pobiera wszystkie kopie dla konta
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="productId"></param>
		/// <returns>
		/// 	IEnumerable<SingleProductCopyDTO>
		/// </returns>

		public async Task<IEnumerable<SingleProductCopyDTO>> GetSingleCopiesAsync(Guid userId, Guid productId)
		{
			var user = await _userRepository.FindOrFailAsync(userId);
			var product = await _productRepository.FindOrFailAsync(productId);
			var singleCopies = await _productRepository.GetSingleProductCopiesAsync(user, product);

			return _mapper.Map<IEnumerable<SingleProductCopyDTO>>(singleCopies);
		}

		/// <summary>
		/// 	Kup kopie produktu
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="productId"></param>
		/// <param name="amount"></param>
		/// <returns>
		/// 	True - commit / False - rollback
		/// </returns>

		public async Task<bool> PurchaseAsync(Guid userId, Guid productId, int amount)
		{
			var user = await _userRepository.FindOrFailAsync(userId);
			var product = await _productRepository.FindOrFailAsync(productId);

			return await _productRepository.PurchaseAsync(user, product, amount);
		}

		/// <summary>
		/// 	Anuluj kupno produktu
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="productId"></param>
		/// <param name="amount"></param>
		/// <returns>
		/// 	True - commit / False - rollback
		/// </returns>

		public async Task<bool> CancelAsync(Guid userId, Guid productId, int amount)
		{
			var user = await _userRepository.FindOrFailAsync(userId);
			var product = await _productRepository.FindOrFailAsync(productId);

			return await _productRepository.Cancel(user, product, amount);
		}
	}
}
