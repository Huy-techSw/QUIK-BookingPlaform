using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quik_BookingApp.DAO;
using Quik_BookingApp.Extentions;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Modal;
using Quik_BookingApp.Repos.Interface;
using Quik_BookingApp.Service;
using QuikBookingApp.Modal;
using Serilog;
using System;
using System.Text;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Call firebase initialization
FirebaseInitializer.InitializeFirebase();

// Add services to the container
builder.Services.AddControllers();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quik_BookingApp API", Version = "v1" });

    // Configure Swagger to use JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add services for DI (Dependency Injection)
builder.Services.AddTransient<EmailService>();
builder.Services.Configure<FirebaseConfiguration>(builder.Configuration.GetSection("Firebase"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IFirebaseService, FirebaseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IBookingService, BookingService>();
builder.Services.AddScoped<IWorkingSpaceService, WorkingSpaceService>();
builder.Services.AddTransient<IBusinessService, BusinessService>();
builder.Services.AddTransient<IRefreshHandler, RefreshHandler>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IVnPayService, VNPayService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// DbContext and database connection
builder.Services.AddDbContext<QuikDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), options => options.CommandTimeout(60)));

// JWT Authentication setup
var _authkey = builder.Configuration.GetValue<string>("JwtSettings:securitykey");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// AutoMapper setup
var automapper = new MapperConfiguration(item => item.AddProfile(new AutoMapperHandler()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);

// Configure Serilog logging
string logpath = builder.Configuration.GetSection("Logging:Logpath").Value;
var _logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logpath)
    .CreateLogger();
builder.Logging.AddSerilog(_logger);

// Configure JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();

// CORS configuration
app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    //.AllowCredentials()
    .AllowAnyOrigin());
    //.WithOrigins("http://localhost:5173","https://note-now.website", "https://quik-jet.vercel.app/"));

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Apply database migrations in Development environment
    await using (var scope = app.Services.CreateAsyncScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<QuikDbContext>();
        // Uncomment to apply migrations if needed
        // await dbContext.Database.MigrateAsync();
    }
}

// Enable Swagger UI in development
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection(); // Use HTTPS if necessary

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
