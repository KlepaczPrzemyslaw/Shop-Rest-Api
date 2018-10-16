using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Models
{
	public abstract class Entity
	{
		[Required]
		public Guid Id { get; protected set; }

		/// <summary>
		/// 	Konstruktor
		/// </summary>

		protected Entity()
		{
			Id = Guid.NewGuid();
		}
	}
}
