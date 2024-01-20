using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YsmStore.API.Data;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Data.Implementations;
using YsmStore.API.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();

// ������������ ����������� �� Bearer ������ 
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));

string secretKey = builder.Configuration.GetSection("JWTSettings:SecretKey").Value;
string issuer = builder.Configuration.GetSection("JWTSettings:Issuer").Value;
string audience = builder.Configuration.GetSection("JWTSettings:Audience").Value;
SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        IssuerSigningKey = signingKey,
        ValidateIssuerSigningKey = true
    };
});

// ����������� ���� ������
builder.Services.AddDbContext<PostgresContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaulConnection")));

// ����������
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllersWithViews();

// ��������� �������� email-��
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
