namespace YsmStore.API.Data.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Create(T entity);
        Task<T> Update(object id, T entity);
        Task<T> Delete(object id);
        Task<T> GetById(object id);
        Task<bool> IsExists(object id);
    }
}
