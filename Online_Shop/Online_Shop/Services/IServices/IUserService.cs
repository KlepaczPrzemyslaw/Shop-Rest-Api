using Online_Shop.DTO;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Services.IServices
{
	public interface IUserService
	{
		Task<AccountDTO> GetAccountAsync(Guid userId);

		Task<bool> RegisterAsync(UserModel userModel);
		Task<TokenDTO> LoginAsync(string email, string password);
	}
}
