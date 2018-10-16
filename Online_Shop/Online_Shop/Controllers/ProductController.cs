using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.DTO;
using Online_Shop.Models;
using Online_Shop.Services.IServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Online_Shop.Controllers
{
	public class ProductController : BaseController
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			this._productService = productService;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Index(string id = null)
		{
			IEnumerable<ProductDTO> products = await _productService.BrowseAsync(id);

			return View(products);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetProduct(Guid id)
		{
			ProductDTO product = await _productService.GetAsync(id);

			if (product == null)
				return NotFound();

			return View(id);
		}

		[HttpPost]
		[Authorize(Policy = "HasAdminRole")]
		public async Task<IActionResult> Add( [FromBody] ProductModel productModel, [FromBody, Required] int amount, [FromBody, Required] decimal price)
		{
			if (await _productService.AddAsync(productModel) == true)
			{
				await _productService.AddProductCopiesAsync(productModel.Id, amount, price);

				return Created($"/Product/GetProduct/{productModel.Id}", null);
			}

			return View(model: productModel);
		}

		[HttpPut]
		[Authorize(Policy = "HasAdminRole")]
		public async Task<IActionResult> Update( [FromBody] ProductModel productModel)
		{
			if (await _productService.UpdateAsync(productModel) == true)
			{
				return NoContent();
			}

			return View(model: productModel);
		}

		[HttpDelete]
		[Authorize(Policy = "HasAdminRole")]
		public async Task<IActionResult> Delete(Guid id)
		{
			if (await _productService.DeleteAsync(id) == true)
			{
				return NoContent();
			}

			return Content("Nie udało się usunąć!!!");
		}
	}
}
