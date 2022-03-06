using UIM.Core.Common;

namespace UIM.Core.Models.Dtos.Submission
{
    public class UpdateSubmissionRequest : SubmissionDto, IUpdateRequest
    {
        public string ModifiedBy { get; set; }
    }
}