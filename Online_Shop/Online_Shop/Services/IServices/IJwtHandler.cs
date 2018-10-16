using Online_Shop.DTO;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Services.IServices
{
	public interface IJwtHandler
	{
		JwtDTO CreateToken(Guid userId, Role role);
	}
}
