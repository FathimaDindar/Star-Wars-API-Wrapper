using Star_Wars_API_Wrapper.Service;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<filmService>();
builder.Services.AddScoped<filmService>();
builder.Services.AddHttpClient<filmService>(client =>
{
    client.BaseAddress = new Uri("https://swapi.dev/api/");
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Star Wars API Wrapper",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Star Wars API Wrapper");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
