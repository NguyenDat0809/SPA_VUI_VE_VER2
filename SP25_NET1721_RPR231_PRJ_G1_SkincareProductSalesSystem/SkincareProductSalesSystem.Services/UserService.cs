using FirebaseAdmin.Auth;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{

    public interface IUserService
    {
        Task<User> CreateViaFirebase(UserRecord record);
        Task<User> GetUserAsync(string id);
        Task<IServiceResult> GetAllAsync(int page, int size);
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Delete(string id);
    }

    public class UserService : IUserService
    {
        private UnitOfWork _unitOfWork;

        public UserService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<User> CreateViaFirebase(UserRecord record)
        {
            var user = new User
            {
                UserId = record.Uid,
                Username = record.Email,
                PasswordHash = string.Empty,
                PasswordSalt = string.Empty,
                FullName = record.DisplayName,
                Email = record.Email,
                PhoneNumber = record.PhoneNumber,
                Avatar = record.PhotoUrl,
                RoleName = "Customer",
                IsEmailVerified = record.EmailVerified,
                IsPhoneVerified = !string.IsNullOrEmpty(record.PhoneNumber),
                IsActive = true,
                CreatedAt = DateTime.Now,
            };
            await _unitOfWork.UserRepository.CreateAsync(user);
            return user;
        }

        public async Task<IServiceResult> Delete(string id)
        {
            var userAccount = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (userAccount == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.UserRepository.RemoveAsync(userAccount);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = userAccount
            };
        }

        public async Task<IServiceResult> GetAllAsync(int page, int size)
        {
            var userAccounts = await _unitOfWork.UserRepository.GetPagingListAsync(page: page, size: size);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = userAccounts
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            var userAccount = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (userAccount == null) return new ServiceResult(404, "Không tìm thấy");
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = userAccount
            };
        }
        public async Task<User> GetUserAsync(string id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return user;
        }
    }
}