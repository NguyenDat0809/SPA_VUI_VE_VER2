using Azure;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IOrderService
    {
        Task<ServiceResult> GetPagination(int page, int size);
        Task<ServiceResult?> GetOrderById(string id);
        Task<ServiceResult?> CreateOrder();
        Task<ServiceResult?> UpdateOrder(Order order);
    }
    public class OrderService : IOrderService
    {
        private readonly UnitOfWork _unitOfWork;

        public OrderService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult?> CreateOrder()
        {
            var newOrder = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
                Status = "Processing",
                TotalAmount = 0,
                UpdatedAt = DateTime.Now,
                ShippingFee = 0,
            };
            var isSuccessful = (await _unitOfWork.OrderRepository.CreateAsync(newOrder));
            return new ServiceResult
            {
                Status = ((isSuccessful) > 0) ? 200 : 500,
                Data = ((isSuccessful) > 0) ? newOrder : null,
                Message = ((isSuccessful) > 0) ? "Success" : "Fail"
            };
        }

        public async Task<ServiceResult?> GetOrderById(string id)
        {
            var response = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            return new ServiceResult
            {
                Status = (response != null)? 200:500,
                Data = (response != null) ? response: null,
                Message = (response != null) ? "Success" : "Fail"
            };
        }

        public async Task<ServiceResult> GetPagination(int page, int size)
        {
            var responses = await _unitOfWork.OrderRepository.GetPagingListAsync(
                page: page,
                size: size,
                orderBy: x => x.OrderByDescending(x => x.OrderDate)
                );
            return new ServiceResult
            {
                Status = 200, 
                Data = responses,
                Message = "Success"
            };
        }
        public async Task<ServiceResult?> UpdateOrder(Order order)
        {
            var isSuccessful = (await _unitOfWork.OrderRepository.UpdateAsync(order));
            return new ServiceResult
            {
                Status = (isSuccessful > 0) ? 200 : 500,
                Data = (isSuccessful > 0) ? order : null,
                Message = (isSuccessful > 0) ? "Success" : "Fail"
            };
        }
    }
}
