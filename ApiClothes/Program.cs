using ApiClothes.Entities;
using Microsoft.EntityFrameworkCore;
using ApiClothes.Services.Interfaces;
using ApiClothes.Services.Services;
using ApiClothes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Serilog;
using Microsoft.Extensions.Logging;




var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logowanie tak¿e do konsoli
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)  // Logowanie do pliku
    .CreateLogger();
// Add services to the container.
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutomovieMappingProfile));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration["Azure:BlobStorage:ConnectionString"]));
builder.Services.AddDbContext<PlatformDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }).EnableSensitiveDataLogging());
builder.Services.AddScoped<IProductCatalog, ProductCatalog>();
builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Umo¿liwia dostêp z dowolnej domeny
        policy.AllowAnyOrigin()
              .AllowAnyMethod()   // Umo¿liwia wszystkie metody (GET, POST, PUT, DELETE, itd.)
              .AllowAnyHeader();  // Umo¿liwia wszystkie nag³ówki
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.Configure<GoogleCloudStorageConfig>(
    builder.Configuration.GetSection("GoogleCloudStorage"));

// Ustawienie zmiennej œrodowiskowej dla Google Cloud SDK
var credentialPath = builder.Configuration.GetSection("GoogleCloudStorage")["CredentialPath"];
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");  // U¿ywamy wczeœniej zdefiniowanej polityki CORS

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
