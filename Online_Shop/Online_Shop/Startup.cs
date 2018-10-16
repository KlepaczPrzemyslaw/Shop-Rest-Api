using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Online_Shop.DAO;
using Online_Shop.DAO.Repositories;
using Online_Shop.Mappers;
using Online_Shop.Services.IServices;
using Online_Shop.Services.Services;

namespace Online_Shop
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			// Dodanie autoryzacji
			services.AddAuthorization(x => x.AddPolicy("HasAdminRole", p => p.RequireRole("Admin")));

			// Przypisanie do IProductRepository -> ProductRepository
			services.AddScoped<IProductRepository, ProductRepository>();

			// Przypisanie do IUserRepository -> UserRepository
			services.AddScoped<IUserRepository, UserRepository>();

			// Przypisanie do IProductService -> ProductService
			services.AddScoped<IProductService, ProductService>();

			// Przypisanie do IUserService -> UserService
			services.AddScoped<IUserService, UserService>();

			// Przypisanie do ISingleProductCopyService -> SingleProductCopyService
			services.AddScoped<ISingleProductCopyService, SingleProductCopyService>();

			// Zainicjowanie automapera 1 raz
			services.AddSingleton(AutoMapperConfig.Initialize());

			// Zainicjowanie JwtHandler 1 raz
			services.AddSingleton<IJwtHandler, JwtHandler>();

			// Token
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = JwtSettings.JwtSettings.Issuer,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.JwtSettings.Key))
				};
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();
			app.UseStaticFiles();
			app.UseMvcWithDefaultRoute();
		}
	}
}
