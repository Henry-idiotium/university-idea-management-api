using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using UIM.Core.Common;
using UIM.Core.Helpers;
using UIM.Core.Models.Dtos;
using UIM.Core.Models.Dtos.Idea;
using UIM.Core.Models.Entities;
using UIM.Core.ResponseMessages;
using UIM.Core.Services.Interfaces;

namespace UIM.Core.Services
{
    public class IdeaService : Service, IIdeaService
    {
        private readonly UserManager<AppUser> _userManager;

        public IdeaService(IMapper mapper,
            IOptions<SieveOptions> sieveOptions,
            SieveProcessor sieveProcessor,
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager)
            : base(mapper,
                sieveOptions,
                sieveProcessor,
                unitOfWork) => _userManager = userManager;

        public async Task CreateAsync(CreateIdeaRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId) == null
                || await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            if (request.CategoryId == null)
                if (await _unitOfWork.Categories.GetByIdAsync((int)request.CategoryId) == null)
                    throw new HttpException(HttpStatusCode.BadRequest,
                                            ErrorResponseMessages.BadRequest);

            var newIdea = _mapper.Map<Idea>(request);
            var succeeded = await _unitOfWork.Ideas.AddAsync(newIdea);
            if (!succeeded)
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        ErrorResponseMessages.UnexpectedError);
        }

        public async Task EditAsync(string ideaId, UpdateIdeaRequest request)
        {
            var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
            if (idea == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            if (await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            if (request.CategoryId == null)
                if (await _unitOfWork.Categories.GetByIdAsync((int)request.CategoryId) == null)
                    throw new HttpException(HttpStatusCode.BadRequest,
                                            ErrorResponseMessages.BadRequest);

            idea = _mapper.Map<Idea>(request);
            var edited = await _unitOfWork.Ideas.UpdateAsync(idea);
            if (!edited)
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        ErrorResponseMessages.UnexpectedError);
        }

        public async Task<TableResponse> FindAsync(SieveModel model)
        {
            if (model == null)
                throw new ArgumentNullException(string.Empty);

            if (model?.Page < 0 || model?.PageSize < 1)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var ideas = await _unitOfWork.Ideas.GetAllAsync();
            var sortedIdeas = _sieveProcessor.Apply(model, ideas.AsQueryable());

            var mappedIdeas = new List<IdeaDetailsResponse>();
            foreach (var idea in sortedIdeas)
                mappedIdeas.Add(_mapper.Map<IdeaDetailsResponse>(idea, opt =>
                    opt.AfterMap((src, dest) =>
                    {
                        dest.User.Email = idea.User.Email;
                        dest.User.FullName = idea.User.FullName;
                    })));

            var pageSize = model.PageSize ?? _sieveOptions.DefaultPageSize;

            return new(mappedIdeas, ideas.Count(),
                currentPage: model.Page ?? 1,
                totalPages: (int)Math.Ceiling((float)ideas.Count() / pageSize));
        }

        public async Task<IdeaDetailsResponse> FindByIdAsync(string ideaId)
        {
            var idea = _mapper.Map<Idea, IdeaDetailsResponse>(
                await _unitOfWork.Ideas.GetByIdAsync(ideaId), opt => opt.AfterMap((src, dest) =>
                {
                    dest.User.Email = src.User.Email;
                    dest.User.FullName = src.User.FullName;
                }));
            return idea;
        }

        public async Task RemoveAsync(string ideaId)
        {
            var succeeded = await _unitOfWork.Ideas.DeleteAsync(ideaId);
            if (!succeeded)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);
        }
    }
}