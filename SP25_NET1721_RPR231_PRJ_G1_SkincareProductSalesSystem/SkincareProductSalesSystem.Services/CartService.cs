using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Services.Base;



namespace SkincareProductSalesSystem.Services.ExtendServices
{
    public class AddToCartRequest
    {
        [Required]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class UpdateToCartRequest : AddToCartRequest
    {
        
    }

    public interface ICartService
    {
        Task<IServiceResult> AddOrUpdateToCartAsync(AddToCartRequest request);
        Task<IServiceResult> RemoveFromCartAsync(string productId);
        Task<IServiceResult> GetUserCartAsync();
    }


    public class CartService : ICartService
    {
        private readonly ICacheService _cacheService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int LIMIT_CART_COUNT = 100;
        public CartService(ICacheService cacheService, IHttpContextAccessor httpContextAccessor, UnitOfWork unitOfWork)
        {
            _cacheService = cacheService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
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

        public async Task<IServiceResult> AddOrUpdateToCartAsync(AddToCartRequest request)
        {
            try
            {
                string userId = GetUserId();
                string key = $"cart:{userId}";

                var cart = await _cacheService.GetDataAsync<Dictionary<string, int>>(key) ?? new Dictionary<string, int>();
                if(cart.Count >= LIMIT_CART_COUNT)
                    return new ServiceResult(403, "Giỏ hàng đã đạt giới hạn 100");

                if (cart.TryGetValue(request.ProductId, out int quantity))
                {
                    cart[request.ProductId] = request.Quantity; 
                }
                else
                {
                    cart.Add(request.ProductId, request.Quantity); 
                }
                await _cacheService.SetDataAsync(key, cart);

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
                var cartData = await _cacheService.GetDataAsync<Dictionary<string, int>>(key) ??
                               new Dictionary<string, int>();

                var products = new List<Product>();

                foreach (var productId in cartData.Keys.Select(id => id).ToList())
                {
                    products.Add(await _unitOfWork.ProductRepository.GetByIdAsync(productId));
                }

                var result = products.Select(p => new
                {
                    Product = p,
                    Quantity = cartData[p.ProductId.ToString()]
                }).ToList();

                return new ServiceResult
                {
                    Status = 200,
                    Message = "Thành công",
                    Data = result
                };
            }
            catch (UnauthorizedAccessException e)
            {
                return new ServiceResult(401, e.Message);
            }
        }
    }
}

