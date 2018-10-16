using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Online_Shop.Controllers
{
	public class BaseController : Controller
	{
		protected Guid UserId => User?.Identity?.IsAuthenticated == true ?
			Guid.Parse(User.Identity.Name) :
			Guid.Empty;
	}
}
