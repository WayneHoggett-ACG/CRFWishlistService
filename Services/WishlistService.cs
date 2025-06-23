using Azure;
using Azure.Data.Tables;
using CRFWishlistService.Models;

namespace CRFWishlistService.Services;

public class WishlistService
{
    private readonly TableClient _tableClient;

    public WishlistService(TableClient tableClient)
    {
        _tableClient = tableClient;
        _tableClient.CreateIfNotExists();
    }

    public async Task AddItemAsync(string userId, int productId)
    {
        var entity = new WishlistItem
        {
            PartitionKey = userId,
            RowKey = productId.ToString()
        };
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task RemoveItemAsync(string userId, int productId)
    {
        await _tableClient.DeleteEntityAsync(userId, productId.ToString());
    }

    public IEnumerable<int> GetItems(string userId)
    {
        var items = _tableClient.Query<WishlistItem>(x => x.PartitionKey == userId);
        return items.Select(x => int.Parse(x.RowKey));
    }
    public Dictionary<string, List<int>> GetWishlists()
{
    var items = _tableClient.Query<WishlistItem>();

    return items
        .GroupBy(x => x.PartitionKey)
        .ToDictionary(
            g => g.Key,                          // UserId
            g => g.Select(x => int.Parse(x.RowKey)).ToList() // ItemIds
        );
}
}
