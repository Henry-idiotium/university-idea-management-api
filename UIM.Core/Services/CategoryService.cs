using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using UIM.Core.Common;
using UIM.Core.Helpers;
using UIM.Core.Models.Dtos;
using UIM.Core.Models.Dtos.Category;
using UIM.Core.Models.Entities;
using UIM.Core.ResponseMessages;
using UIM.Core.Services.Interfaces;

namespace UIM.Core.Services
{
    public class CategoryService : Service, ICategoryService
    {
        public CategoryService(IMapper mapper,
            IOptions<SieveOptions> sieveOptions,
            SieveProcessor sieveProcessor,
            IUnitOfWork unitOfWork)
            : base(mapper,
                sieveOptions,
                sieveProcessor,
                unitOfWork)
        {
        }

        public async Task CreateAsync(CreateCategoryRequest request)
        {
            if (await _unitOfWork.Categories.GetByNameAsync(request.Name) != null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var added = await _unitOfWork.Categories.AddAsync(
                new Category { Name = request.Name });
            if (!added)
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        ErrorResponseMessages.UnexpectedError);
        }

        public async Task EditAsync(int categoryId, UpdateCategoryRequest request)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            category.Name = request.Name;
            var edited = await _unitOfWork.Categories.UpdateAsync(category);
            if (!edited)
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        ErrorResponseMessages.UnexpectedError);
        }

        public async Task<TableResponse> FindAsync(SieveModel model)
        {
            if (model?.Page < 0 || model?.PageSize < 1)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var categories = await _unitOfWork.Categories.GetAllAsync();
            var sortedCategories = _sieveProcessor.Apply(model, categories.AsQueryable());

            var pageSize = model.PageSize ?? _sieveOptions.DefaultPageSize;

            return new(sortedCategories, sortedCategories.Count(),
                currentPage: model.Page ?? 1,
                totalPages: (int)Math.Ceiling((float)categories.Count() / pageSize));
        }

        public async Task<CategoryDetailsResponse> FindByIdAsync(int categoryId)
        {
            var category = _mapper.Map<CategoryDetailsResponse>(
                await _unitOfWork.Categories.GetByIdAsync(categoryId));
            return category;
        }

        public async Task RemoveAsync(int categoryId)
        {
            var succeeded = await _unitOfWork.Categories.DeleteAsync(categoryId);
            if (!succeeded)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);
        }
    }
}