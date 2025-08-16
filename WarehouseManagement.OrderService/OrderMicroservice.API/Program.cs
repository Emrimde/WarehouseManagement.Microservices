using OrderMicroservice.Core;
using OrderMicroservice.Core.HttpClients;
using OrderMicroservice.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();
builder.Services.AddCore();
builder.Services.AddHttpClient<InventoryMicroserviceClient>(client => client.BaseAddress = new Uri("http://orderMicroservice.api:5055"));
builder.Services.AddHttpClient<ProductMicroserviceClient>(client => client.BaseAddress = new Uri("http://productservice.api:5049"));

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
