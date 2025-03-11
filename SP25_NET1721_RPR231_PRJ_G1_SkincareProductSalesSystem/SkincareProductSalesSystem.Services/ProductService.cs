using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace SkincareProductSalesSystem.Services
{
    public class CreateProductRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }

        public string BrandId { get; set; }

        public int? StockQuantity { get; set; }

        public IFormFile ImageFile { get; set; }

        public string Ingredients { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UpdateProductRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }

        public string BrandId { get; set; }

        public int? StockQuantity { get; set; }

        public IFormFile ImageFile { get; set; }

        public string Ingredients { get; set; }

        public bool? IsActive { get; set; }
    }

    public class GetAllProductQuery
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public string? FilterBy { get; set; }
        public string? FilterQuery { get; set; }
        public string? SortBy { get; set; }
        public bool IsAsc { get; set; }
    }

    public interface IProductService
    {
        Task<IServiceResult> GetAllAsync(GetAllProductQuery query);
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Create(CreateProductRequest request);
        Task<IServiceResult> Update(string id, UpdateProductRequest request);
        Task<IServiceResult> Delete(string id);
    }

    public class ProductService : IProductService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> Create(CreateProductRequest request)
        {
            var product = new Product
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = request.Name,
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                Price = request.Price,
                ImageUrl = "",
                Description = request.Description,
                IsActive = request.IsActive,
                StockQuantity = request.StockQuantity,
                Ingredients = request.Ingredients,
            };

            var result = await _unitOfWork.ProductRepository.CreateAsync(product);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = product
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.ProductRepository.RemoveAsync(product);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = product
            };
        }

        public async Task<IServiceResult> GetAllAsync(GetAllProductQuery query)
        {
            Expression<Func<Product, bool>> predicate = x => true;


            if (!query.Category.IsNullOrEmpty())
            {
                Expression<Func<Product, bool>> categoryFilter = x => x.Category.Name == query.Category;
                predicate = CombineExpressions(predicate, categoryFilter);
            }

            if (!query.Brand.IsNullOrEmpty())
            {
                Expression<Func<Product, bool>> brandFilter = x => x.Brand.Name == query.Brand;
                predicate = CombineExpressions(predicate, brandFilter);
            }

            if (!string.IsNullOrEmpty(query.FilterBy) && !string.IsNullOrEmpty(query.FilterQuery))
            {
                Expression<Func<Product, bool>>? filterExpression = query.FilterBy.ToLower() switch
                {
                    "name" => x => x.Name.Contains(query.FilterQuery),
                    _ => null
                };

                if (filterExpression != null)
                {
                    predicate = CombineExpressions(predicate, filterExpression);
                }
            }

            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null;
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                orderBy = query.IsAsc
                    ? q => q.OrderBy(x => EF.Property<Product>(x, query.SortBy))
                    : q => q.OrderByDescending(x => EF.Property<Product>(x, query.SortBy));
            }


            var products = await _unitOfWork.ProductRepository.GetPagingListAsync(
                predicate: predicate,
                orderBy: orderBy,
                page: query.Page,
                size: query.Size,
                include:
                x => x.Include(x => x.Category).Include(x => x.Brand)
            );
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = products
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(x => x.ProductId == id,
                include: x => x.Include(x => x.Category).Include(x => x.Brand));
            if (product == null) return new ServiceResult(404, "Không tìm thấy");
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = product
            };
        }

        public async Task<IServiceResult> Update(string id, UpdateProductRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return new ServiceResult(404, "Không tìm thấy");

            //update skin type
            product.Name = request.Name;
            product.Description = request.Description;
            product.CategoryId = request.CategoryId;
            product.BrandId = request.BrandId;
            product.Price = request.Price;
            product.ImageUrl = "";
            product.Ingredients = request.Ingredients;
            product.StockQuantity = request.StockQuantity;


            await _unitOfWork.ProductRepository.UpdateAsync(product);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = product
            };
        }

        private Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (first == null) return second;
            if (second == null) return first;

            var parameter = Expression.Parameter(typeof(T));

            var combined = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }
}