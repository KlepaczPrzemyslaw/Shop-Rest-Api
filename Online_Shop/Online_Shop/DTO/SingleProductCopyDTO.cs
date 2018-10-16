using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DTO
{
	public class SingleProductCopyDTO
	{
		public Guid Id { get; set; }
		public decimal Price { get; set; }

		public Guid? UserId { get; set; }
		public string UserName { get; set; }
		public DateTime? PurchasedAt { get; set; }
		public bool Purchased { get; set; }
	}
}
