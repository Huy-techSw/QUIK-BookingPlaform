using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quik_BookingApp.DAO;
using Quik_BookingApp.Repos.AutoMapper;
using Quik_BookingApp.Repos.Interface;
using Quik_BookingApp.Service;
using System.Text;
using AutoMapper;



namespace Quik_BookingApp.Extentions
{
    //public static class ServiceCollectionExtensions
    //{

    //    /// <summary>
    //    /// Add needed instances for database
    //    /// </summary>
    //    /// <param name="services"></param>
    //    /// <param name="configuration"></param>
    //    /// <returns></returns>
    //    public static IServiceCollection AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
    //    {
    //        // Configure DbContext with Scoped lifetime  
    //        services.AddDbContext<QuikDbContext>(options =>
    //        {
    //            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    //                sqlOptions => sqlOptions.CommandTimeout(120));
    //            // options.UseLazyLoadingProxies();
    //        }
    //        );

    //        services.AddScoped<Func<QuikDbContext>>((provider) => () => provider.GetService<QuikDbContext>());
        
           
    //        //        services.AddIdentity<User, IdentityRole>()
    //        //.AddEntityFrameworkStores<UberSystemDbContext>()
    //        //.AddDefaultTokenProviders();
    //        // Configure JWT authentication
    //        var jwtSettings = configuration.GetSection("JwtSettings");
    //        var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

    //        services.AddAuthentication(options =>
    //        {
    //            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //        })
    //        .AddJwtBearer(options =>
    //        {
    //            options.RequireHttpsMetadata = false;
    //            options.SaveToken = true;
    //            options.TokenValidationParameters = new TokenValidationParameters
    //            {
    //                ValidateIssuerSigningKey = true,
    //                IssuerSigningKey = new SymmetricSecurityKey(key),
    //                ValidateIssuer = false,
    //                ValidateAudience = false
    //            };
    //        });


    //        // AutoMapper
    //        services.AddAutoMapper(typeof(MappingProfileExtension));
    //        return services;
    //    }

    //    /// <summary>
    //    /// Add instances of in-use services
    //    /// </summary>
    //    /// <param name="services"></param>
    //    /// <returns></returns>
    //    public static IServiceCollection AddServices(this IServiceCollection services)
    //    {
    //        services.AddScoped<IUserService, UserService>();
    //        services.AddScoped<IEmailService, EmailService>();
    //        services.AddTransient<SmtpClient>(provider =>
    //        {
    //            var smtpClient = new SmtpClient();
    //            smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
    //            smtpClient.Authenticate("huylqse173543@fpt.edu.vn", "hexk qxkd kxvh ohfc");
    //            return smtpClient;
    //        });
    //        services.AddScoped(typeof(TokenService));

    //        return services;
    //    }
    //}
}
