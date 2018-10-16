using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DAO.Repositories
{
	public interface IUserRepository
	{
		Task<UserModel> GetAsync(Guid id);
		Task<UserModel> GetAsync(string email);

		Task<bool> AddAsync(UserModel userModel);
		Task<bool> UpdateAsync(UserModel userModel);
		Task<bool> DeleteAsync(UserModel userModel);
	}
}
