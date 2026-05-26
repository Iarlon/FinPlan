using financeiro.Infraestructure.Database;
using Financeiro.Application;
using Financeiro.Infraestructure.Configuration;
using Financeiro.Infraestructure.Database;
using Financeiro.Infrastructure;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração de CORS com suporte a desenvolvimento e produção
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Permite qualquer origem (IP do celular, emulador, etc)
              .AllowAnyMethod()    // Permite GET, POST, PUT, DELETE e o OPTIONS
              .AllowAnyHeader();   // Permite qualquer cabeçalho (como Content-Type e Authorization)
    });

    // Política mais restritiva para produção (recomendado)
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:3000", "http://localhost:8080" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// configura o swagger para usar autorização
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Financeiro API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


// Busca connection
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new Exception("Connection string não encontrada.");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("AllowSpecificOrigins");
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

MigrationRunner.Run(connectionString);
app.Run("http://0.0.0.0:8080");
