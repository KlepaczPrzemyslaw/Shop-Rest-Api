using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Models
{
	public class ProductModel : Entity
	{
		[Required, MinLength(6), MaxLength(64), DisplayName("Nazwa produktu")]
		public string Name { get; protected set; }

		[Required, MinLength(3), MaxLength(256), DisplayName("Opis")]
		public string Description { get; protected set; }

		// Z konstruktora
		public DateTime CreatedAt { get; protected set; }

		// Z konstruktora
		public DateTime UpdatedAt { get; protected set; }

		// Przy pobraniu dokładnie 1 produktu
		public IEnumerable<SingleProductCopyModel> SingleCopiesContainer { get; protected set; }

		// Ogólne info przy pobraniu listy produktów
		public int Amount { get; protected set; }

		/// <summary>
		/// 	Konstruktor dla Strony
		/// </summary>

		protected ProductModel()
		{
			CreatedAt = DateTime.UtcNow;
			UpdatedAt = DateTime.UtcNow;
		}

		/// <summary>
		/// 	Konstruktor dla bazy danych
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="createdAt"></param>
		/// <param name="updatedAt"></param>
		
		public ProductModel(Guid id, string name, string description, DateTime createdAt, DateTime updatedAt)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
			this.CreatedAt = createdAt;
			this.UpdatedAt = updatedAt;
		}

		/// <summary>
		/// 	Set dla listy pojedyńczych kopii produktu
		/// </summary>
		/// <param name="singleCopies"></param>

		public void LoadSingleCopies(ISet<SingleProductCopyModel> singleCopies)
		{
			this.SingleCopiesContainer = singleCopies.AsEnumerable();
		}

		/// <summary>
		/// 	Set dla ilości pojedyńczych kopii
		/// </summary>
		/// <param name="amount"></param>

		public void UpdateAmount(int amount)
		{
			this.Amount = amount;
		}
	}
}
