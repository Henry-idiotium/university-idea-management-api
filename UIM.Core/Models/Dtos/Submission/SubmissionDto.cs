using System;
using System.ComponentModel.DataAnnotations;

namespace UIM.Core.Models.Dtos.Submission
{
    public abstract class SubmissionDto
    {
        [Required] public string Title { get; set; }
        public string Description { get; set; }
        [Required] public DateTime InitialDate { get; set; }
        [Required] public DateTime FinalDate { get; set; }
        public bool IsActive { get; set; }
    }
}