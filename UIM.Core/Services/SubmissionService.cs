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
using UIM.Core.Models.Dtos.Submission;
using UIM.Core.Models.Entities;
using UIM.Core.ResponseMessages;
using UIM.Core.Services.Interfaces;

namespace UIM.Core.Services
{
    public class SubmissionService : Service, ISubmissionService
    {
        public SubmissionService(IMapper mapper,
            IOptions<SieveOptions> sieveOptions,
            SieveProcessor sieveProcessor,
            IUnitOfWork unitOfWork)
            : base(mapper,
                sieveOptions,
                sieveProcessor,
                unitOfWork)
        {
        }

        public async Task AddIdeaToSubmissionAsync(AddIdeaRequest request)
        {
            var submission = await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId);
            var idea = await _unitOfWork.Ideas.GetByIdAsync(request.SubmissionId);
            if (submission == null || idea == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            idea.SubmissionId = submission.Id;
            var edited = await _unitOfWork.Ideas.UpdateAsync(idea);
            if (!edited)
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        ErrorResponseMessages.UnexpectedError);
        }

        public async Task CreateAsync(CreateSubmissionRequest request)
        {
            var submission = _mapper.Map<Submission>(request);
            var isCreated = await _unitOfWork.Submissions.AddAsync(submission);
            if (!isCreated)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);
        }

        public async Task EditAsync(string submissionId, UpdateSubmissionRequest request)
        {
            var oldSubmission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
            _mapper.Map(request, oldSubmission);

            var isEdited = await _unitOfWork.Submissions.UpdateAsync(oldSubmission);
            if (!isEdited)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);
        }

        public async Task<TableResponse> FindAsync(SieveModel model)
        {
            if (model?.Page < 0 || model?.PageSize < 1)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var subs = await _unitOfWork.Submissions.GetAllAsync();
            var sortedSubs = _sieveProcessor.Apply(model, subs.AsQueryable());

            var pageSize = model.PageSize ?? _sieveOptions.DefaultPageSize;

            return new(sortedSubs, sortedSubs.Count(),
                currentPage: model.Page ?? 1,
                totalPages: (int)Math.Ceiling((float)subs.Count() / pageSize));
        }

        public async Task<SubmissionDetailsResponse> FindByIdAsync(string submissionId)
        {
            var submission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
            return _mapper.Map<SubmissionDetailsResponse>(submission);
        }

        public async Task RemoveAsync(string submissionId)
        {
            var subExists = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
            if (subExists == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var isDeleted = await _unitOfWork.Submissions.DeleteAsync(submissionId);
            if (!isDeleted)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);
        }
    }
}