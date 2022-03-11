namespace UIM.Core.Common;

public interface IEntity
{
    DateTime CreatedDate { get; set; }
    DateTime ModifiedDate { get; set; }
    string? CreatedBy { get; set; }
    string? ModifiedBy { get; set; }
}

public interface IEntity<T> : IEntity
{
    T Id { get; set; }
}