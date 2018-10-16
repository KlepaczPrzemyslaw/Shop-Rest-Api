using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DTO
{
	public class TokenDTO
	{
		public string Token { get; set; }
		public long Expires { get; set; }
		public Role AccountRole { get; set; }
	}
}
