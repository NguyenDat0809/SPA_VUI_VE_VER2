using AutoMapper;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;
using SkincareProductSalesSystem.Services.Base;
using Solara.Main.Payload;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
	public interface IPromotionService
	{
		Task<IServiceResult> Create(CreatePromotionRequest request);
		Task<IServiceResult> Delete(DeletePromotionRequest request);
		Task<IServiceResult> GetAll();
		Task<IPaginate<Promotion>?> GetPaginate(int page, int size);
		Task<IServiceResult> GetById(string promotionId);
		Task<IServiceResult> Update(UpdatePromotionRequest request);
	}

	public class PromotionService : IPromotionService
	{
		private UnitOfWork _uOW;
		private IMapper _mapper;

		public PromotionService(UnitOfWork uOW, IMapper mapper)
		{
			_uOW ??= uOW;
			_mapper = mapper;
		}


		public async Task<IServiceResult> Create(CreatePromotionRequest request)
		{
			try
			{
				if (request.StartDate > request.EndDate)
					return new ServiceResult()
					{
						Status = 400,
						Message = "End Date must be after Start Date"
					};

				if (request.EndDate < DateTime.UtcNow.AddHours(7))
					return new ServiceResult()
					{
						Status = 400,
						Message = "End Date must be after current date"
					};

				if (request.Code.IsNullOrEmpty()) //nếu không nhập code thì random code
				{
					request.Code = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8)).Substring(0, 16);
				};

				Promotion entity = _mapper.Map<Promotion>(request);
				entity.Code = entity.Code.ToUpper();
				await _uOW.PromotionRepository.CreateAsync(entity);

				return new ServiceResult()
				{
					Status = 200,
					Message = "Thành công",
					Data = entity
				};
			}
			catch (Exception ex)
			{
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IServiceResult> GetAll()
		{
			try
			{
				var result = await _uOW.PromotionRepository.GetAllAsync();

				return new ServiceResult()
				{
					Status = 200,
					Message = "Thành công",
					Data = result
				};
			}
			catch (Exception ex)
			{
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IServiceResult> GetById(string promotionId)
		{
			try
			{
				var result = await _uOW.PromotionRepository.GetByIdAsync(promotionId);

				return new ServiceResult()
				{
					Status = 200,
					Message = "Thành công",
					Data = result
				};
			}
			catch (Exception ex)
			{
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IPaginate<Promotion>?> GetPaginate(int page, int size)
		{
			try
			{
				return await _uOW.PromotionRepository.GetPagingListAsync(page : page, size: size);	
			}
			catch (Exception ex)
			{
				return null;
			}
		}


		public async Task<IServiceResult> Delete(DeletePromotionRequest request)
		{
			try
			{
				if (!request.PromotionIds.Any())
					return new ServiceResult()
					{
						Status = 400,
					};

				foreach (var id in request.PromotionIds)
				{
					var result = await _uOW.PromotionRepository.GetByIdAsync(id);
					if (result != null) await _uOW.PromotionRepository.RemoveAsync(result);
				}
				return new ServiceResult()
				{
					Status = 200,
					Message = "Thành công",
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

		public async Task<IServiceResult> Update(UpdatePromotionRequest request)
		{
			try
			{
				var result = await _uOW.PromotionRepository.GetByIdAsync(request.PromotionId);
				if (result == null)
					return new ServiceResult()
					{
						Status = 404,
						Message = "Không tìm thấy"
					};

				result = _mapper.Map(request, result);
				await _uOW.PromotionRepository.UpdateAsync(result);
				return new ServiceResult()
				{
					Status = 200,
					Message = "Thành công",
					Data = result
				};
			}

			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new ServiceResult()
				{
					Status = 500,
				};
			}
		}
	}

	public class CreatePromotionRequest
	{
		public string? Code { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public decimal DiscountAmount { get; set; }
		[Required]
		public decimal? MinimumPurchase { get; set; }
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime EndDate { get; set; }
		public int? UsageLimit { get; set; }
	}

	public class DeletePromotionRequest 
	{
		[Required]
		public List<string> PromotionIds { get; set; }
	}

	public class UpdatePromotionRequest
	{
		[Required]
		public string PromotionId { get; set; }
		public string? Code { get; set; }
		public string? Name { get; set; }
		public decimal? DiscountAmount { get; set; }
		public decimal? MinimumPurchase { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? UsageLimit { get; set; }
	}
}
