using AutoMapper;
using Online_Shop.DTO;
using Online_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Mappers
{
	public static class AutoMapperConfig
	{
		public static IMapper Initialize()
			=> new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ProductModel, ProductDTO>()
					.ForMember(x => x.AvailableProductCopiesCount, m => m.MapFrom(p => p.Amount));
				cfg.CreateMap<ProductModel, ProductDetailsDTO>();
				cfg.CreateMap<SingleProductCopyModel, SingleProductCopyDTO>();
				cfg.CreateMap<UserModel, AccountDTO>();
			})
			.CreateMapper();
	}
}
