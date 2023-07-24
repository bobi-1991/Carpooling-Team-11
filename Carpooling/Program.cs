using CarPooling.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using CarPooling.Data.Data;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ////DB
        builder.Services.AddDbContext<CarPoolingDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<CarPoolingDbContext>();

        // Add services to the container.
        builder.Services.AddRazorPages();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Carpooling API", Version = "v1" });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Carpooling API V1");
            options.RoutePrefix = "api/swagger";
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapRazorPages();
        
        app.Run();
    }
}