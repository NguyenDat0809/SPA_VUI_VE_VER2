using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.Services
{
    public interface IOrderDetailServices
    {
        Task<IPaginate<OrderDetail>> GetOrderDetailsByOrderId(int page, int size, string orderId);
        Task<OrderDetail?> CreateOrderDetail(OrderDetail newOrderDetail);
        Task<OrderDetail?> UpdateOrderDetail(OrderDetail updateOrderDetail);
    }
    public class OrderDetailServices : IOrderDetailServices
    {
       private readonly UnitOfWork _unitOfWork;

        public OrderDetailServices()
        {
           _unitOfWork ??= new UnitOfWork();
        }

        public async Task<OrderDetail?> CreateOrderDetail(OrderDetail newOrderDetail)
        {
            return ((await _unitOfWork.OrderDetailRepository.CreateAsync(newOrderDetail)) > 0)? newOrderDetail : null;
        }

        public async Task<IPaginate<OrderDetail>> GetOrderDetailsByOrderId(int page, int size, string orderId)
        {
            return await _unitOfWork.OrderDetailRepository.GetPagingListAsync(
                    predicate: x => x.OrderId == orderId,
                    size: size,
                    page: page,
                    include: x => x.Include(x => x.Product)
                );
        }

        public async Task<OrderDetail?> UpdateOrderDetail(OrderDetail updateOrderDetail)
        {
            return ((await _unitOfWork.OrderDetailRepository.UpdateAsync(updateOrderDetail)) > 0)? updateOrderDetail : null;
        }
    }
}
