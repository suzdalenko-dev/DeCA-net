using Microsoft.EntityFrameworkCore;  // importamos Microsoft.EntityFrameworkCore, que contiene las clases y métodos de Entity Framework Core.

var builder = WebApplication.CreateBuilder(args); // Configuración de la aplicación, variables de entorno, configuracion de los servicios

builder.Services.AddControllers();  // registra lo necesario para utilizar los controladores
builder.Services.AddOpenApi();      // es un estandar que permite describir una API REST
builder.Services.AddDbContext<DeCaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DeCa")));
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

var app = builder.Build();
Console.WriteLine($"ENTORNO ACTUAL: {app.Environment.EnvironmentName}");

// 2. Publica el endpoint que devuelve el documento JSON
if (app.Environment.IsDevelopment()){
    app.MapOpenApi();
}

app.UseCors("AllowAll");    // Activar CORS
app.UseHttpsRedirection();  // Este middleware redirige las peticiones HTTP a HTTPS cuando ASP.NET Core conoce el puerto HTTPS correspondiente.
app.UseAuthorization();     // Este middleware se encarga de aplicar las políticas de autorización a los endpoints.
app.MapControllers();       // Esta instrucción publica los endpoints definidos en tus controladores.
app.Run();
