using CodeBolosJacquin.API.Context;
using CodeBolosJacquin.API.Interfaces;
using CodeBolosJacquin.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Pegando a connectionString
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrando o DbContext
builder.Services.AddDbContext<BolosJacquinContext>(options => 
    options.UseSqlServer(connectionString));

// Registrando as dependencias (injeção de dependencias)
builder.Services.AddScoped<IBoloRepository, BoloRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Configurar a política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        // policy.AllowAnyOrigin()
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


// Add Serialização para evitar erros de ciclo
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add autenticação com Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Coloque seu Bearer {token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});


// Habilitando AUTENTICAÇÃO via JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

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
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Ativar a exibição do Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}


// CONFIGURANDO UMA ROTA ESTÁTICA PARA ACESSAR OS ARQUIVOS DE IMAGENS;

var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "Uploads");

Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),

    // http://localhost:nroDaPorta/Uploads/nomeDoArquivo.png
    RequestPath = "/Uploads"
});

// Aplicar a politica de CORS
app.UseCors("AllowReact");

app.UseHttpsRedirection();

//disponibilizando o uso de autenticação
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
