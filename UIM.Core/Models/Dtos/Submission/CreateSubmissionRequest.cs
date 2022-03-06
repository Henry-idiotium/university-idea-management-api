using UIM.Core.Common;

namespace UIM.Core.Models.Dtos.Submission
{
    public class CreateSubmissionRequest : SubmissionDto, ICreateRequest
    {
        public string ModifiedBy { get; set; }
    }
}