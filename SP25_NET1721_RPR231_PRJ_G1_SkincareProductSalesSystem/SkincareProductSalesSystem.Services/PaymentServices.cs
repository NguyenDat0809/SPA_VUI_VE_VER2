using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IPaymentServices
    {
        Task<IPaginate<Payment>> GetAllPaginate(int page, int size);
        Task<Payment?> GetPaymentById(string id);
        Task<IPaginate<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size);
    }
    public class PaymentServices : IPaymentServices
    {
        private readonly UnitOfWork _unitOfWork;

        public PaymentServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IPaginate<Payment>> GetAllPaginate(int page, int size)
        {
            return await _unitOfWork.PaymentRepository.GetPagingListAsync
                ( 
                    page: page,
                    size: size
                );
        }

        public async Task<Payment?> GetPaymentById(string id)
        {
            return await _unitOfWork.PaymentRepository.GetByIdAsync(id);
        }

        public async Task<IPaginate<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size)
        {
            return await _unitOfWork.PaymentRepository.GetPagingListAsync(
                predicate: x => x.OrderId.Equals(orderId),
                page: page,
                size: size
                );
        }
    }
}
