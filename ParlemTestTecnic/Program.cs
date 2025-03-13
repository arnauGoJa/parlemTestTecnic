using Core;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using System.Net;
using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
string databaseName = builder.Configuration["CosmoDB:databaseName"];
string containerName = builder.Configuration["CosmoDB:containerName"];
string cosmosConnectionString = builder.Configuration["CosmoDB:cosmosConnectionString"];

CosmosClient cosmosClient = new CosmosClient(cosmosConnectionString, new CosmosClientOptions
{
    AllowBulkExecution = true,
    SerializerOptions = new CosmosSerializationOptions
    {
        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
    },
    LimitToEndpoint = false
});
//te create bd in cosmo db emulator
if (bool.Parse(builder.Configuration["CosmoDB:createDBInEmulator"]))
{
    Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(
        id: databaseName
    );
    Container container = await database.CreateContainerIfNotExistsAsync(
        id: containerName,
    partitionKeyPath: "/customerId",
        throughput: 400);

    try
    {

        ItemResponse<Customer> response = await container.ReadItemAsync<Customer>("555555", new PartitionKey("11111"));
    }
    catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    {
        Customer customer = new("555555", "nif", "11223344E", "it@parlem.com", "11111", "Enriqueta", "Parlem", "668668668", "customer");
        Customer createdCostumer = await container.CreateItemAsync(
            item: customer,
              partitionKey: new PartitionKey("11111")
        );
        Product product = new("1111111", "FIBRA 1000 ADAMO", "ftth", 933933933, DateTime.Parse("2019-01-09 14:26:17"), "11111", "product");
        Product createdProduct = await container.CreateItemAsync(
            item: product,
              partitionKey: new PartitionKey("11111")
        );
    }
}


builder.Services.AddSingleton(cosmosClient);
builder.Services.AddScoped<ICustomerRepository>(provider =>
    new CustomerRepository(cosmosClient, databaseName, containerName));
builder.Services.AddScoped<IRepository<Product>>(provider =>
    new ProductRepository(cosmosClient, databaseName, containerName));
builder.Services.AddSingleton<ICustomerValidator, CustomerValidator>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
