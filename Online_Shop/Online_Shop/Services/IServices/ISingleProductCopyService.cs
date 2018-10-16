using Online_Shop.DTO;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Services.IServices
{
	public interface ISingleProductCopyService
	{
		Task<SingleProductCopyDTO> GetSingleCopyAsync(Guid userId, Guid productId, Guid singleCopyId);
		Task<IEnumerable<SingleProductCopyDTO>> GetSingleCopiesAsync(Guid userId, Guid productId);

		Task<bool> PurchaseAsync(Guid userId, Guid productId, int amount);
		Task<bool> CancelAsync(Guid userId, Guid productId, int amount);
	}
}
