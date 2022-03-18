namespace UIM.Core.Helpers;

public class ContextModifyResult<TEntity>
    where TEntity : class, IEntity
{
    public ContextModifyResult(TEntity? entity = null, bool succeeded = true, string? error = null)
    {
        Error = error;
        Entity = entity;
        Succeeded = succeeded;
    }

    public ContextModifyResult(string error)
    {
        Error = error;
        Succeeded = false;
    }

    public TEntity? Entity { get; }
    public string? Error { get; }
    public bool Succeeded { get; }
}