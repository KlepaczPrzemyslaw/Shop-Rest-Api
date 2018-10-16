using Online_Shop.DAO.Repositories;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DAO
{
	public class UserRepository : IUserRepository
	{
		public async Task<UserModel> GetAsync(Guid id)
			=> await Task.FromResult(UserDAO.GetUser(id));

		public async Task<UserModel> GetAsync(string email)
			=> await Task.FromResult(UserDAO.GetUser(email));

		public async Task<bool> AddAsync(UserModel userModel)
			=> await Task.FromResult(UserDAO.Add(userModel));

		public async Task<bool> UpdateAsync(UserModel userModel)
			=> await Task.FromResult(UserDAO.Update(userModel));

		public async Task<bool> DeleteAsync(UserModel userModel)
			=> await Task.FromResult(UserDAO.Delete(userModel));
	}
}
