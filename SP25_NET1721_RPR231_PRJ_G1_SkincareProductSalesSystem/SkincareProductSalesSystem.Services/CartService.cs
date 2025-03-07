using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SkincareProductSalesSystem.Services.Base;



namespace SkincareProductSalesSystem.Services.ExtendServices
{
    public interface ICartService
    {
        Task<IServiceResult> AddToCartAsync(string productId);
        Task<IServiceResult> RemoveFromCartAsync(string productId);
        Task<IServiceResult> GetUserCartAsync();
    }


    public class CartService : ICartService
    {
        private readonly ICacheService _cacheService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int LIMIT_CART_COUNT = 100;
        public CartService(ICacheService cacheService, IHttpContextAccessor httpContextAccessor)
        {
            _cacheService = cacheService;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Người dùng chưa được cấp quyền");
            }
            return userId;
        }

        public async Task<IServiceResult> AddToCartAsync(string productId)
        {
            try
            {
                string userId = GetUserId();
                string key = $"cart:{userId}";

                var cart = await _cacheService.GetDataAsync<List<string>>(key) ?? new List<string>();
                if(cart.Count >= LIMIT_CART_COUNT)
                    return new ServiceResult(403, "Giỏ hàng đã đạt giới hạn 100");

                if (!cart.Contains(productId))
                {
                    cart.Add(productId);
                    await _cacheService.SetDataAsync(key, cart);
                }
                return new ServiceResult(200, "Thành công", cart);
            }
            catch (UnauthorizedAccessException e)
            {
                return new ServiceResult(401, e.Message);
            }

        }

        public async Task<IServiceResult> RemoveFromCartAsync(string productId)
        {
            try
            {
                string userId = GetUserId();
                string key = $"cart:{userId}";

                var cart = await _cacheService.GetDataAsync<List<string>>(key);
                if (cart == null || !cart.Contains(productId)) return new ServiceResult(400, "Không tìm thấy sản phẩm trong giỏ hàng");

                cart.Remove(productId);
                await _cacheService.SetDataAsync(key, cart);

                return new ServiceResult(200, "Thành công");
            }
            catch (UnauthorizedAccessException e)
            {
                return new ServiceResult(401, e.Message);
            }
        }

        public async Task<IServiceResult> GetUserCartAsync()
        {
            try
            {
                string userId = GetUserId();
                string key = $"cart:{userId}";
                var cartData = await _cacheService.GetDataAsync<List<string>>(key) ?? new List<string>();
                return new ServiceResult
                {
                    Status = 200,
                    Message = "Thành công",
                    Data = cartData
                };
            }
            catch (UnauthorizedAccessException e)
            {
                return new ServiceResult(401, e.Message);
            }
        }
    }
}

