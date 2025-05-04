namespace DataAccess.Repositories.IRepositories
{
    /// <summary>
    /// Defines a common interface for all objects that can be stored in the database
    /// Contains all basic CRUD operations, both synchronously (Sync) and asynchronously (Async)
    /// </summary>
    /// <typeparam name="T">The type of object to be handled in the operations</typeparam>
    public interface IRepository<T>
    {


        #region Async CRUD Operations


        #region Create

        /// <summary>
        /// Create a new record of type T in the database asynchronously 
        /// </summary>
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create multiple records in the database asynchronously 
        /// </summary>
        Task CreateAllAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        #endregion



        #region Update

        /// <summary>
        /// Modify an existing record in the database asynchronously 
        /// </summary>
        Task<T> EditAsync(T entity, CancellationToken cancellationToken = default);

        #endregion



        #region Delete

        /// <summary>
        /// Delete a specific record from the database asynchronously
        /// </summary>
        Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);


        /// <summary>
        /// Delete a set of records from the database asynchronously
        /// </summary>
        Task DeleteAllAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        #endregion



        #region Save Changes


        /// <summary>
        /// Save all changes made to the context to the database asynchronously
        /// </summary>
        /// <returns>Number of records affected by the save</returns>
        Task<int> SaveInDataBaseAsync(CancellationToken cancellationToken = default);


        #endregion



        #region Read


        /// <summary>
        /// Retrieve a set of records based on an optional filter and the conditions for loading relationships (Includes)
        /// </summary>
        /// <param name="filter">An optional filter to specify the required records</param>
        /// <param name="includes">The relationships (Navigation Properties) that should be loaded with the object</param>
        /// <param name="tracked">Whether the object should be tracked from EF Core for changes</param>
        /// <param name="cancellationToken">A token to cancel the operation if necessary</param>
        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Retrieve a single record based on an optional filter and load conditions (Includes)
        /// </summary>
        Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? filter = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);


        #endregion


        #region Get Count
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        #endregion


        #endregion



        #region Sync Methods (Legacy Support)

        /// <summary>
        /// Create a new record synchronously
        /// </summary>
        void Create(T entity);

        /// <summary>
        /// Create a recordset in batch synchronously
        /// </summary>
        void CreateAll(IEnumerable<T> entities);

        /// <summary>
        /// Modify an existing record concurrently
        /// </summary>
        void Edit(T entity);

        /// <summary>
        /// Delete an existing record concurrently
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// Delete a set of records in batch synchronously
        /// </summary>
        void DeleteAll(IEnumerable<T> entities);

        /// <summary>
        /// Save changes to the database synchronously
        /// </summary>
        void SaveDB();


        /// <summary>
        /// Retrieve a set of records based on optional conditions synchronously
        /// </summary>
        IQueryable<T> Get(
            Expression<Func<T, bool>>? filter = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null,
            bool tracked = true);


        /// <summary>
        /// Retrieve a single record based on optional conditions simultaneously
        /// </summary>
        T? GetOne(
            Expression<Func<T, bool>>? filter = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null,
            bool tracked = true);

        #endregion



    }
}
