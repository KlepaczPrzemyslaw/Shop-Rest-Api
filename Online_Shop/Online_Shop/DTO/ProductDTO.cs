using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DTO
{
	public class ProductDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public int AvailableProductCopiesCount { get; set; }
	}
}
