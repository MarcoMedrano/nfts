namespace NftSample.Domain.Repositories.Base;

/// <summary>
/// Generic repository interface for basic CRUD operations on entities.
/// </summary>
/// <typeparam name="T">Type of the entity.</typeparam>
/// <typeparam name="K">Type of the entity's primary key.</typeparam>
public interface IRepository<T, K>
{
    /// <summary>
    /// Gets an entity by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity.</param>
    /// <returns>A task that represents the asynchronous operation and holds the entity with the specified primary key.</returns>
    Task<T> GetById(K id);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation and holds a collection of all entities.</returns>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<K> Add(T entity);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Update(T entity);

    /// <summary>
    /// Deletes an entity by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Delete(K id);
}