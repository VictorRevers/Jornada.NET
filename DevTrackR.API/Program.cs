using DevTrackR.API.Persistence;
using DevTrackR.API.Persistence.Repository;
using DevTrackR.API.Persitence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
var sendGridApiKey = builder.Configuration.GetSection("SendGridApiKey").Value;

builder.Services.AddSendGrid(o => o.ApiKey = sendGridApiKey);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => {
    o.SwaggerDoc("v1", new OpenApiInfo{
        Title = "DevTrackR.API",
        Version = "v1",
        Contact = new OpenApiContact{
            Name = "Victor",
            Email = "victorrkasnowski@gmail.com",
            Url = new Uri("https://victorrk.com.br")
        }
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, "DevTrackR.API.xml");
    o.IncludeXmlComments(xmlPath);
});

var connectionString = builder.Configuration.GetConnectionString("DevTrackRCs");
builder.Services
    .AddDbContext<DevTrackRContext>(o=> o.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
