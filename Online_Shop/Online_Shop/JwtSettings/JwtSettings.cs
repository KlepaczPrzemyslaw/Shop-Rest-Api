using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.JwtSettings
{
	public static class JwtSettings
	{
		public static string Key { get; } = "secretPassword_111.";
		
		// Dodaj swój poprawny localhost
		public static string Issuer { get; } = "http://localhost:63436";

		public static int ExpiryMinutes { get; } = 60;
	}
}
