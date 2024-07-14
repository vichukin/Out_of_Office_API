using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Out_of_Office_API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Out_of_Office_API.Services;
using Out_of_Office_API.AutoMapper;
using Microsoft.Extensions.Azure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        build =>
        {
            build
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CompanyDBContext>(options =>
{
    options.UseSqlServer("workstation id=CompanyDB.mssql.somee.com;packet size=4096;user id=TimeAccountForDB_SQLLogin_1;pwd=h8p5wznbbf;data source=CompanyDB.mssql.somee.com;persist security info=False;initial catalog=CompanyDB;TrustServerCertificate=True");
});
builder.Services.AddIdentityApiEndpoints<Employee>().AddRoles<IdentityRole>().AddEntityFrameworkStores<CompanyDBContext>();

builder.Services.AddAuthorization(); //authorization

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["blob:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["blob:queue"]!, preferMsi: true);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
