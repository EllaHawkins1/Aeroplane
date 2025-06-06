using Aeroplane.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register the in-memory database
builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseInMemoryDatabase("FlightDb"));

var app = builder.Build();

// Seed database with data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FlightDbContext>();
    CsvSeeder.SeedFromCsv(db);
}

// Configure the HTTP request 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run(); //  starts app 
