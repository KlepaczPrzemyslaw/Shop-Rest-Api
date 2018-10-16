using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DTO
{
	public class AccountDTO
	{
		public Guid Id { get; set; }
		public Role AccountRole { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}
}
