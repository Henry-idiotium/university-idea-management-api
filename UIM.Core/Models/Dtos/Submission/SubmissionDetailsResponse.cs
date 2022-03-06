using System;
using UIM.Core.Common;

namespace UIM.Core.Models.Dtos.Submission
{
    public class SubmissionDetailsResponse : SubmissionDto, IResponse
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}