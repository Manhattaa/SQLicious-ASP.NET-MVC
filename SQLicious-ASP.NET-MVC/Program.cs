using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;
using SQLicious_ASP.NET_MVC.Data.Repository;
using SQLicious_ASP.NET_MVC.Data;
using SQLicious_ASP.NET_MVC.Models;
using SQLicious_ASP.NET_MVC.Options.Email;
using SQLicious_ASP.NET_MVC.Options;
using SQLicious_ASP.NET_MVC.Services.IServices;
using SQLicious_ASP.NET_MVC.Services;
using SQLicious_ASP.NET_MVC.Options.Email.IEmail;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        var builder = WebApplication.CreateBuilder(args);

        var mailKitSettingsSection = new MailKitSettings
        {
            MailServer = Environment.GetEnvironmentVariable("MAILSERVER"),
            MailPort = int.Parse(Environment.GetEnvironmentVariable("MAILPORT")),
            SenderName = Environment.GetEnvironmentVariable("SENDERNAME"),
            Sender = Environment.GetEnvironmentVariable("SENDER"),
            Password = Environment.GetEnvironmentVariable("PASSWORD")
        };

        builder.Services.Configure<MailKitSettings>(options =>
        {
            options.MailServer = mailKitSettingsSection.MailServer;
            options.MailPort = mailKitSettingsSection.MailPort;
            options.SenderName = mailKitSettingsSection.SenderName;
            options.Sender = mailKitSettingsSection.Sender;
            options.Password = mailKitSettingsSection.Password;
        });

        // Identity Setup
        builder.Services.AddIdentity<Admin, IdentityRole>()
            .AddEntityFrameworkStores<RestaurantContext>()
            .AddDefaultTokenProviders();

        // Adding JWT Bearer Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")))
                };
            });

        // Register Services and Repositories
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<AuthenticationService>();
        builder.Services.AddScoped<JwtRepository>();
        builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        builder.Services.AddScoped<IMenuItemService, MenuItemService>();
        builder.Services.AddScoped<IEmailSender, EmailSender>();

        // Add Authorization
        builder.Services.AddAuthorization();

        // Swagger Configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SQLicious API", Version = "v1" });

            // Define Bearer Authentication scheme for Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            // Apply Bearer authentication globally in Swagger UI
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        // Database Configuration
        builder.Services.AddDbContext<RestaurantContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("SQLICIOUS_CONNECTION"));
        });

        // Add Controllers and Razor Pages
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SQLicious API v1");
            c.RoutePrefix = "swagger"; // Set Swagger at /swagger
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}