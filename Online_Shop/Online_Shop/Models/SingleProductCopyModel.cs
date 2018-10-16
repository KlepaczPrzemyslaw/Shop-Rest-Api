using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Models
{
	public class SingleProductCopyModel : Entity
	{
		[Required]
		public Guid ProductId { get; protected set; }

		[Required, DisplayName("Cena")]
		public decimal Price { get; protected set; }

		// Przy kupieniu
		public Guid? UserId { get; protected set; }

		// Przy kupieniu
		public string UserName { get; protected set; }

		// Przy kupieniu
		public DateTime? PurchasedAt { get; protected set; }

		// Lambda
		public bool Purchased => UserId.HasValue;

		/// <summary>
		/// 	Konstruktor dla Strony
		/// </summary>

		protected SingleProductCopyModel()
		{

		}

		/// <summary>
		/// 	Konstruktor dla bazy danych
		/// </summary>
		/// <param name="productModel"></param>
		/// <param name="id"></param>
		/// <param name="price"></param>
		/// <param name="userId"></param>
		/// <param name="userName"></param>
		/// <param name="purchasedAt"></param>

		public SingleProductCopyModel(ProductModel productModel, Guid id, decimal price, Guid? userId, string userName, DateTime? purchasedAt)
		{
			this.ProductId = productModel.Id;

			this.Id = id;
			this.Price = price;

			this.UserId = userId;
			this.UserName = userName;
			this.PurchasedAt = purchasedAt;
		}
	}
}
