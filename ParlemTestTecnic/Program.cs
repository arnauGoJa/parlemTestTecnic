using Core;
using Domain.Entities;
using Infrastructure;
using Microsoft.Azure.Cosmos;
using System.Net;
using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string databaseName = "parlemtesttecnic";
string containerName = "custumersproducts";
string cosmosConnectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";


CosmosClient cosmosClient = new CosmosClient(cosmosConnectionString, new CosmosClientOptions
{
    AllowBulkExecution = true,
    SerializerOptions = new CosmosSerializationOptions
    {
        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
    },
    LimitToEndpoint = false
});
Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(
    id: databaseName
);
Container container = await database.CreateContainerIfNotExistsAsync(
    id: containerName,
partitionKeyPath: "/partitionKey",
    throughput: 400);

try
{

    ItemResponse<Customer> response = await container.ReadItemAsync<Customer>("555555", new PartitionKey("customer_11111"));
}
catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
{
    Customer customer = new("555555", "customer_11111", "nif", "11223344E", "it@parlem.com", "11111", "Enriqueta", "Parlem", "668668668", "customer");
    Customer createdCostumer = await container.CreateItemAsync(
        item: customer,
          partitionKey: new PartitionKey("customer_11111")
    );
    Product product = new("1111111", "customer_11111", "FIBRA 1000 ADAMO", "ftth", 933933933, DateTime.Parse("2019-01-09 14:26:17"), "11111", "product");
    Product createdProduct = await container.CreateItemAsync(
        item: product,
          partitionKey: new PartitionKey("customer_11111")
    );
}


builder.Services.AddSingleton(cosmosClient);
builder.Services.AddScoped<ICustomerRepository>(provider =>
    new CustomerRepository(cosmosClient, databaseName, containerName));
builder.Services.AddScoped<IRepository<Product>>(provider =>
    new ProductRepository(cosmosClient, databaseName, containerName));
builder.Services.AddScoped<CustomerService, CustomerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
