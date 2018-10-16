using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.DTO
{
	public class ProductDetailsDTO : ProductDTO
	{
		private new int AvailableProductCopiesCount { get; set; }

		public IEnumerable<SingleProductCopyDTO> SingleCopiesContainer { get; set; }
	}
}
