namespace OdontoClass.Repositories.Interfaces;

public interface IGenericOperationsRepositosy
{
    Task<T> CreateAsync<T>(T entity) where T : class;
    Task<T> UpdateAsync<T>(T entity) where T : class;
    Task<List<T>> CreateAsync<T>(List<T> entity) where T : class;
    Task<List<T>> UpdateAsync<T>(List<T> entity) where T : class;
}
