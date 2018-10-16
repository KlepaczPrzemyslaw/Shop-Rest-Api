using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Models
{
	public class UserModel : Entity
	{
		[Required]
		public Role AccountRole { get; protected set; }

		[Required, MinLength(6), MaxLength(64), DisplayName("Imię i nazwisko")]
		public string Name { get; protected set; }

		[Required, MinLength(6), MaxLength(64), DisplayName("Email")]
		public string Email { get; protected set; }

		[Required, MinLength(6), MaxLength(64), DisplayName("Hasło")]
		public string Password { get; protected set; }

		// Z konstruktora
		public DateTime CreatedAt { get; protected set; }

		/// <summary>
		/// 	Konstruktor dla strony internetowej
		/// </summary>

		protected UserModel()
		{
			this.CreatedAt = DateTime.UtcNow;
		}

		/// <summary>
		/// 	Konstruktor dla bazy danych
		/// </summary>
		/// <param name="id"></param>
		/// <param name="role"></param>
		/// <param name="name"></param>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <param name="createdAt"></param>

		public UserModel(Guid id, Role role, string name, string email, string password, DateTime createdAt)
		{
			this.Id = id;
			this.AccountRole = role;
			this.Name = name;
			this.Email = email;
			this.Password = password;
			this.CreatedAt = createdAt;
		}
	}
}
