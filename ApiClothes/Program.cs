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


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });



builder.Services.AddAutoMapper(typeof(AutomovieMappingProfile));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.Configure<GoogleCloudStorageConfig>(
    builder.Configuration.GetSection("GoogleCloudStorage"));

var credentialPath = builder.Configuration.GetSection("GoogleCloudStorage")["CredentialPath"];
if (!string.IsNullOrEmpty(credentialPath))
{
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
}

builder.Services.AddDbContext<PlatformDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null)
    );

});

builder.Services.AddScoped<IProductCatalog, ProductCatalog>();
builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendOnly", policy =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()   // zezwala na ka¿dy nag³ówek Origin
                  .AllowAnyMethod()   // GET, POST, PUT, DELETE itd.
                  .AllowAnyHeader();  // ka¿dy nag³ówek
        });
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(key))
            throw new Exception("JWT key not configured");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PlatformDbContext>();
    db.Database.Migrate();
}

app.UseExceptionHandler("/error");  

app.UseCors("AllowFrontendOnly");

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllers();

app.Run();
