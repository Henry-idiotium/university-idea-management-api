using System;
using UIM.Core.Common;

namespace UIM.Core.Models.Dtos.Idea
{
    public class IdeaDetailsResponse : IdeaDto, IResponse
    {
        public string Id { get; set; }
        public UserIdea User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public class UserIdea
        {
            public string Email { get; set; }
            public string FullName { get; set; }
        }
    }
}