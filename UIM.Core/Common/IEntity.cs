namespace UIM.Core.Common;

public interface IBaseEntity
{
    string? CreatedBy { get; set; }
    DateTime CreatedDate { get; set; }
    string? ModifiedBy { get; set; }
    DateTime ModifiedDate { get; set; }
}

public interface IEntity : IBaseEntity
{
    string Id { get; set; }
}