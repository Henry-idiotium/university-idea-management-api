namespace UIM.Core.Helpers;

public class ContextModifyResult<TEntity> where TEntity : class, IEntity
{
    public ContextModifyResult(TEntity? entity = null, bool succeeded = true)
    {
        Entity = entity;
        Succeeded = succeeded;
    }

    public ContextModifyResult(bool succeeded = true) => Succeeded = succeeded;

    public TEntity? Entity { get; private set; }
    public bool Succeeded { get; private set; }
}
