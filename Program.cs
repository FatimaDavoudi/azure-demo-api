

using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// ✅ Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=tcp:fd-server-test.database.windows.net,1433;Initial Catalog=test-db;Persist Security Info=False;User ID=azureuser;Password=jyRtOamuyQb9vjonnqZJ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    ,sqlOptions => sqlOptions.EnableRetryOnFailure()));
var app = builder.Build();

// ✅ Enable Swagger
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

// Sample endpoint
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseDeveloperExceptionPage();

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapGet("/api/test", () =>
{
   // return "Hello from Fatemeh API 🚀";
   return "CI/CD is working 🚀";
});


app.MapPost("/products", async (AppDbContext db) =>
{
    var product = new Product { Name = "Test Product" };
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return product;
});

app.MapGet("/products", async (AppDbContext db) =>
{
    return await db.Products.ToListAsync();
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}