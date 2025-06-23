using CRFWishlistService.Services;
using Azure;

namespace CRFWishlistService.Endpoints;

public static class WishlistEndpoints
{
    public static void MapWishlistEndpoints(this WebApplication app)
    {
        app.MapPost("/wishlist/{userId}/{productId}", async (
            string userId, int productId, WishlistService service) =>
        {
            await service.AddItemAsync(userId, productId);
            return Results.Ok();
        });

        app.MapDelete("/wishlist/{userId}/{productId}", async (
            string userId, int productId, WishlistService service) =>
        {
            try
            {
                await service.RemoveItemAsync(userId, productId);
                return Results.Ok();
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return Results.NotFound("Wishlist item not found");
            }
        });

        app.MapGet("/wishlist/{userId}", (
            string userId, WishlistService service) =>
        {
            var productIds = service.GetItems(userId);
            return Results.Ok(productIds);
        });

        app.MapGet("/wishlists", (
            WishlistService service) =>
        {
            var wishlists = service.GetWishlists();
            return Results.Ok(wishlists);
        });
    }
}
