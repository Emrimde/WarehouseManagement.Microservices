using OrderMicroservice.Core;
using OrderMicroservice.Core.HttpClients;
using OrderMicroservice.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();
builder.Services.AddCore();
builder.Services.AddHttpClient<InventoryMicroserviceClient>(client => client.BaseAddress = new Uri("http://inventorymicroservice.api:8080"));
builder.Services.AddHttpClient<ProductMicroserviceClient>(client => client.BaseAddress = new Uri("http://productmicroservice.api:8080"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
