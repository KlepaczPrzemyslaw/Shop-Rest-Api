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
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IJwtHandler _jwtHandler;
		private readonly IMapper _mapper;

		public UserService(IUserRepository userRepository, IJwtHandler jwtHandler, IMapper mapper)
		{
			this._userRepository = userRepository;
			this._jwtHandler = jwtHandler;
			this._mapper = mapper;

		}

		/// <summary>
		/// 	Rejestracja
		/// </summary>
		/// <param name="userModel"></param>
		/// <returns>
		/// 	True - ok / False - rollback
		/// </returns>

		public async Task<bool> RegisterAsync(UserModel userModel)
		{
			var user = await _userRepository.GetOrFailAsync(userModel.Email);

			return await _userRepository.AddAsync(userModel);
		}

		/// <summary>
		/// 	Logowanie
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns>
		/// 	TokenDTO / null - w funkcji
		/// </returns>

		public async Task<TokenDTO> LoginAsync(string email, string password)
		{
			var user = await _userRepository.LoginOrFailAsync(email, password);

			var jwt = _jwtHandler.CreateToken(user.Id, user.AccountRole);

			return new TokenDTO
			{
				Token = jwt.Token,
				Expires = jwt.Expires,
				AccountRole = user.AccountRole
			};
		}

		/// <summary>
		/// 	Pobiera konto user-a
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>
		/// 	AccountDTO / null - w funkcji
		/// </returns>

		public async Task<AccountDTO> GetAccountAsync(Guid userId)
		{
			var user = await _userRepository.FindOrFailAsync(userId);

			return _mapper.Map<AccountDTO>(user);
		}
	}
}
