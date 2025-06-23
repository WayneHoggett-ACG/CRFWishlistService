using Azure.Data.Tables;
using CRFWishlistService.Services;
using CRFWishlistService.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:80");

builder.Services.AddSingleton(x =>
{
    var connectionString = builder.Configuration["AzureStorage:ConnectionString"];
    var tableClient = new TableServiceClient(connectionString).GetTableClient("Wishlists");
    return tableClient;
});
builder.Services.AddScoped<WishlistService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapWishlistEndpoints();

app.Run();
