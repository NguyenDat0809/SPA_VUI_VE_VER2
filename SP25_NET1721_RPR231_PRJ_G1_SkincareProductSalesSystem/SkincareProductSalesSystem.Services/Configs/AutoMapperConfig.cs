using AutoMapper;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Models.SkinTestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Configs
{
	public class AutoMapperConfig : Profile
	{
        public AutoMapperConfig()
        {
			CreateMap<RegisterRequest, User>()
				.ForMember(b => b.Email, opt => opt.MapFrom(a => a.Email))
				.ForMember(b => b.Username, opt => opt.MapFrom(a => a.Username))
				.ForMember(b => b.PhoneNumber, opt => opt.MapFrom(a => a.PhoneNumber))
				.ForMember(b => b.FullName, opt => opt.MapFrom(a => a.FullName));

			
			CreateMap<CreatePromotionRequest, Promotion>();

			CreateMap<UpdatePromotionRequest, Promotion>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			CreateMap<CreatePromotionUsageRequest, PromotionUsage>();

			CreateMap<UpdatePromotionUsageRequest, PromotionUsage>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			CreateMap<CreatePromotionRequest, Promotion>();

			CreateMap<SkinTestQuestion, SkinTestModel>()
				.ForMember(dest => dest.SkinTestOptions, opt => opt.MapFrom(src => src.SkinTestOptions));

			CreateMap<SkinTestOption, SkinTestOptionModel>()
				.ForMember(dest => dest.SkinTypeId, opt => opt.MapFrom(src => src.SkinTypeId));
		}
    }
}

