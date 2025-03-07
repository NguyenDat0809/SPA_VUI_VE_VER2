using AutoMapper;
using Azure.Core;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{


	public interface IPromotionUsageService
	{
		Task<IServiceResult> Create(CreatePromotionUsageRequest request);
		Task<IServiceResult> Update(UpdatePromotionUsageRequest request);
		Task<IServiceResult> Delete(string promoUsageId);
		Task<IServiceResult> GetAll();
		Task<IPaginate<PromotionUsage>?> GetPaginate(int page, int size);
		Task<IServiceResult> GetById(string promoUsageId);
	}


	public class PromotionUsageService : IPromotionUsageService
	{
		private UnitOfWork _uOW;
		private IMapper _mapper;

		public PromotionUsageService(UnitOfWork uOW, IMapper mapper)
		{
			_uOW ??= uOW;
			_mapper = mapper;
		}

		public async Task<IServiceResult> Create(CreatePromotionUsageRequest request)
		{
			try
			{
				var promo = await _uOW.PromotionRepository.GetByCodeAsync(request.PromoCode.ToUpper());
				if (promo == null) return new ServiceResult(404, "Không tìm thấy code");

				var order = await _uOW.OrderRepository.GetByIdAsync(request.OrderId);
				if (order == null) return new ServiceResult(404, "Không tìm thấy Order");

				var promoUsage = _mapper.Map<PromotionUsage>(request);
				promoUsage.PromotionId = promo.PromotionId;
				promoUsage.DiscountAmount = promo.DiscountAmount;
				promoUsage.UsedAt = DateTime.Now;
				promoUsage.CreatedAt = DateTime.Now;
				promoUsage.IsValid = true;
				await _uOW.PromotionUsageRepository.CreateAsync(promoUsage);

				return new ServiceResult()
				{
					Status = 200,
					Message = "Thành công",
					Data = promoUsage
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IServiceResult> GetAll()
		{
			var result = await _uOW.PromotionUsageRepository.GetAllAsync();
			return new ServiceResult()
			{
				Status = (result.Count > 0) ? 200 : 404,
				Message = (result.Count > 0) ? "Thành công" : "Không tìm thấy",
				Data = result
			};
		}

		public async Task<IServiceResult> GetById(string promoUsageId)
		{
			var result = await _uOW.PromotionUsageRepository.GetByIdAsync(promoUsageId);
			return new ServiceResult()
			{
				Status = result != null ? 200 : 404,
				Message = result != null ? "Thành công" : "Không tìm thấy",
				Data = result
			};
		}


		public async Task<IServiceResult> Update(UpdatePromotionUsageRequest request)
		{
			try
			{
				var promoUsage = await _uOW.PromotionUsageRepository.GetByIdAsync(request.UsageId);
				if (promoUsage == null) return new ServiceResult(404, "PromotionUsage not found");
				if (request.OrderId != null)
				{
					var order = await _uOW.OrderRepository.GetByIdAsync(request.OrderId);
					if (order == null) return new ServiceResult(404, "Order not found)");
				}

				Promotion? promo = new Promotion();
				if (request.PromoCode != null)
				{
					promo = await _uOW.PromotionRepository.GetByCodeAsync(request.PromoCode);
					if (promo == null) return new ServiceResult(404, "Code not found");
				}

				promoUsage = _mapper.Map<PromotionUsage>(request);
				promoUsage.DiscountAmount = promo.DiscountAmount;
				promoUsage.UsedAt = DateTime.Now;
				promoUsage.CreatedAt = DateTime.Now;
				promoUsage.IsValid = true;
				await _uOW.PromotionUsageRepository.UpdateAsync(promoUsage);

				return new ServiceResult()
				{
					Status = 200,
					Message = "Thành công",
					Data = promoUsage
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IServiceResult> Delete(string promoUsageId)
		{
			try
			{
				var promoUsage = await _uOW.PromotionUsageRepository.GetByIdAsync(promoUsageId);
				if (promoUsage == null) return new ServiceResult(404, "PromotionUsage not found");
				await _uOW.PromotionUsageRepository.RemoveAsync(promoUsage);

				return new ServiceResult()
				{
					Status = 200,
					Message = "Thành công"
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IPaginate<PromotionUsage>?> GetPaginate(int page, int size)
		{
			try
			{
				return await _uOW.PromotionUsageRepository.GetPagingListAsync(page: page, size: size);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}
	}
	public class CreatePromotionUsageRequest
	{
		[Required]
		public string PromoCode { get; set; }
		[Required]
		public string OrderId { get; set; }
		[Required]
		public string Notes { get; set; }
	}

	public class UpdatePromotionUsageRequest
	{
		[Required]
		public string UsageId { get; set; }

		public string? PromoCode { get; set; }

		public string? OrderId { get; set; }

		public bool? IsValid { get; set; }

		public string? Notes { get; set; }
	}
}
