using System;
using System.Collections.Generic;
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
	public class AccountController : BaseController
	{
		private readonly IUserService _userService;
		private readonly ISingleProductCopyService _copiesService;

		public AccountController(IUserService userService, ISingleProductCopyService copiesService)
		{
			this._userService = userService;
			this._copiesService = copiesService;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAccount(Guid id)
		{
			AccountDTO account = await _userService.GetAccountAsync(id);

			if (account == null)
				return NotFound();

			return View(model: account);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetCopies(Guid userId, Guid productId)
		{
			IEnumerable<SingleProductCopyDTO> singleCopies = await _copiesService.GetSingleCopiesAsync(userId, productId);

			return View(model: singleCopies);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register( [FromBody] UserModel userModel)
		{
			if (await _userService.RegisterAsync(userModel) == true)
			{
				return Created($"/Account/Login", null);
			}

			return View(model: userModel);
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] string email, [FromBody] string password)
		{
			TokenDTO tokenDTO = await _userService.LoginAsync(email, password);

			return Json(tokenDTO);
		}
	}
}
