using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Services.IServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Online_Shop.Controllers
{
	public class ProductCopyController : BaseController
	{
		private readonly ISingleProductCopyService _copyService;

		public ProductCopyController(ISingleProductCopyService copyService)
		{
			this._copyService = copyService;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetCopy(Guid productId, Guid singleCopyId)
		{
			var singleCopy = await _copyService.GetSingleCopyAsync(UserId, productId, singleCopyId);

			if (singleCopy == null)
				return NotFound();

			return View(singleCopy);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Purchase(Guid productId, int amount)
		{
			if (await _copyService.PurchaseAsync(UserId, productId, amount))
				NoContent();

			return BadRequest();
		}

		[HttpDelete]
		[Authorize]
		public async Task<IActionResult> Cancel(Guid productId, int amount)
		{
			if (await _copyService.CancelAsync(UserId, productId, amount))
				NoContent();

			return BadRequest();
		}
	}
}
