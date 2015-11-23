namespace EntityGenerics.Core.Abstractions
{
    public interface IEntity<TKey>
    {
         TKey Id { get; set; }
    }
}