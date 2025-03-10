using Microsoft.AspNetCore.Http;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Repositories;
using SkincareProductSalesSystem.Services.Base;


namespace SkincareProductSalesSystem.Services
{
    public class CreateBrandRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
    }
    public class UpdateBrandRequest : CreateBrandRequest
    {
        public string BrandId { get; set; }
    }
    public interface IBrandService
    {
        Task<IServiceResult> GetPaginate(int page, int size);
        Task<IServiceResult> GetBrandById(string id);
        Task<IServiceResult> GetBrandByName(int page, int size, string name);
        Task<IServiceResult> CreateBrand(CreateBrandRequest request);
        Task<IServiceResult> UpdateBrand(UpdateBrandRequest request);
        Task<IServiceResult> DeleteBrand(string id);
    }
    public class BrandServices : IBrandService
    {
        private readonly UnitOfWork _unitOfWork;

        public BrandServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IServiceResult> GetPaginate(int page, int size)
        {
            var response = await _unitOfWork.BrandRepository.GetPagingListAsync(
                    predicate: b => b.Status == "Active",
                    page: page,
                    size: size
                );
            return new ServiceResult
            {
                Status = 200,
                Message = "",
                Data = response
            };
        }

        public async Task<IServiceResult> GetBrandById(string id)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
            if (brand == null) return new ServiceResult(404, "Không tìm thấy nhãn hàng");
            return new ServiceResult
            {
                Status = 200,
                Message = "",
                Data = brand
            };
        }

        public async Task<IServiceResult> GetBrandByName(int page, int size, string name)
        {
            var response = await _unitOfWork.BrandRepository.GetBrandsByName(page, size, name);
            return new ServiceResult
            {
                Status = 200,
                Message = "",
                Data = response
            };
        }

        public async Task<IServiceResult> CreateBrand(CreateBrandRequest request)
        {
            var newBrand = new Brand
            {
                BrandId = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Status = request.Status,
                CreatedAt = DateTime.Now,
            };
            var response = await _unitOfWork.BrandRepository.CreateAsync(newBrand);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = response
            };
        }

        public async Task<IServiceResult> UpdateBrand(UpdateBrandRequest request)
        {
            var updateBrand = await _unitOfWork.BrandRepository.GetByIdAsync(request.BrandId);
            if (updateBrand == null) return new ServiceResult(404, "Không tìm thấy nhãn hàng");

            await _unitOfWork.BrandRepository.UpdateAsync(updateBrand);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = updateBrand
            };

        }


        public async Task<IServiceResult> DeleteBrand(string id)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
            await _unitOfWork.BrandRepository.RemoveAsync(brand);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = brand
            };
            
        }
    }
}
