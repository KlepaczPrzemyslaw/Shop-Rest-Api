using Microsoft.IdentityModel.Tokens;
using Online_Shop.DTO;
using Online_Shop.Models;
using Online_Shop.Services.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Online_Shop.Helpers;

namespace Online_Shop.Services.Services
{
	public class JwtHandler : IJwtHandler
	{
		public JwtDTO CreateToken(Guid userId, Role role)
		{
			DateTime now = DateTime.UtcNow;
			var claims = new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
				new Claim(ClaimTypes.Role, role.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString())
			};

			var expires = now.AddMinutes(JwtSettings.JwtSettings.ExpiryMinutes);
			var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.JwtSettings.Key)),
					SecurityAlgorithms.HmacSha256);

			var jwt = new JwtSecurityToken(
				issuer: JwtSettings.JwtSettings.Issuer,
				claims: claims,
				notBefore: now,
				expires: expires,
				signingCredentials: signingCredentials);

			var token = new JwtSecurityTokenHandler().WriteToken(jwt);

			return new JwtDTO
			{
				Token = token,
				Expires = expires.ToTimestamp()
			};

		}
	}
}
