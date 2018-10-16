using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DTO
{
	public class JwtDTO
	{
		public string Token { get; set; }
		public long Expires { get; set; }
	}
}
