namespace UIM.Core.Models.Dtos.Category;

public class CategoryDetailsResponse : CategoryDto, IResponse
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}