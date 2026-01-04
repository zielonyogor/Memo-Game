using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;
using NR155910155992.WebUI.Services;

namespace NR155910155992.WebUI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();

			builder.Services.AddJsEngineSwitcher(options =>
			{
				options.AllowCurrentProperty = false;
				options.DefaultEngineName = V8JsEngine.EngineName;
			})
			.AddV8();
			builder.Services.AddWebOptimizer(pipeline =>
			{
				pipeline.CompileScssFiles();
			});

			builder.Services.AddSingleton<LibraryLoader>();
            builder.Services.AddScoped<IGameStateStore, SessionGameStateStore>();
			builder.Services.AddScoped<IGameManager>(provider =>
			{
				var config = provider.GetRequiredService<IConfiguration>();
				var loader = provider.GetRequiredService<LibraryLoader>();
                var gameStateStore = provider.GetRequiredService<IGameStateStore>();

				return loader.LoadObjectFromLibrary<IGameManager>(
					LibraryKey.Bl,
					new object[] { config, gameStateStore }
				);
			});


			var app = builder.Build();
			app.UseWebOptimizer();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

            app.UseSession();
			app.UseAuthorization();

			app.MapStaticAssets();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}")
				.WithStaticAssets();

			app.Run();
		}
	}
}
