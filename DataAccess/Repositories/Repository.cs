﻿
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{

    /// <summary>
    /// The Repository class implements the IRepository interface and provides basic CRUD operations.
    /// For working with the database using Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of entity to be worked with.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {

        // Reference to the database context (ApplicationDbContext)
        // DbSet object representing the data table for entity T
        private readonly ApplicationDbContext _dbContext;
        public DbSet<T> DbSet { get; }

        /// <summary>
        /// Constructor of the Repository class. Initializes the context and DbSet
        /// </summary>
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbSet = _dbContext.Set<T>();
        }


        #region Async CRUD Operations


        #region Create

        /// <summary>
        /// Add a new record to the DbSet asynchronously
        /// </summary>
        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await DbSet.AddAsync(entity, cancellationToken);

            return entity;
        }

        /// <summary>
        /// Add a set of records at once asynchronously
        /// </summary>
        public async Task CreateAllAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            await DbSet.AddRangeAsync(entities, cancellationToken);
        }

        #endregion


        #region Update

        /// <summary>
        /// Update an existing record in a DbSet
        /// Note: Updating does not require a direct save until SaveDBAsync() is called
        /// </summary>
        public Task<T> EditAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            DbSet.Update(entity);

            //await _dbContext.SaveChangesAsync(cancellationToken);

            return Task.FromResult(entity);
        }

        #endregion


        #region Delete


        /// <summary>
        /// Delete a specific record from the DbSet
        /// </summary>
        public Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            DbSet.Remove(entity);
            return Task.FromResult(entity);
        }


        /// <summary>
        /// Delete a recordset in bulk from a DbSet
        /// </summary>
        public Task DeleteAllAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            DbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        #endregion


        #region Save Changes


        /// <summary>
        /// Save all changes made to the context to the database asynchronously
        /// </summary>
        public async Task<int> SaveInDataBaseAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }


        #endregion


        #region Read

        /// <summary>
        /// Retrieve a recordset based on an optional filter, with the ability to load relationships, and with or without tracing
        /// </summary>
        public async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        IEnumerable<Expression<Func<T, object>>>? includes = null,
        IEnumerable<Func<IQueryable<T>, IQueryable<T>>>? thenIncludes = null,
        bool tracked = true,
        CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = tracked ? DbSet : DbSet.AsNoTracking();

            // Basic Includes implementation 
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            // Advanced ThenIncludes application 
            if (thenIncludes != null)
            {
                query = thenIncludes.Aggregate(query, (current, thenInclude) => thenInclude(current));
            }

            // Apply the filter 
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieve a single record based on an optional filter, with the required relationships loaded and the tracing option
        /// </summary>
        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? filter = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null,
            IEnumerable<Func<IQueryable<T>, IQueryable<T>>>? thenIncludes = null,
            bool tracked = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = DbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (thenIncludes != null)
                query = thenIncludes.Aggregate(query, (current, include) => include(current));

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        #endregion


        #region Get By Id

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object[] { id }, cancellationToken);
        }


        public async Task<T?> GetByIdAsync(int id, IEnumerable<Expression<Func<T, object>>> includes, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = DbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);
        }

        #endregion


        #region Get Count

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        #endregion


        #endregion



        #region Sync Methods (Legacy Support)

        /// <summary>
        /// Add a record synchronously
        /// </summary>
        public void Create(T entity) => DbSet.Add(entity);


        /// <summary>
        /// Add a recordset synchronously
        /// </summary>
        public void CreateAll(IEnumerable<T> entities) => DbSet.AddRange(entities);


        /// <summary>
        /// Update a record synchronously
        /// </summary>
        public void Edit(T entity) => DbSet.Update(entity);


        /// <summary>
        /// Delete a record synchronously
        /// </summary>
        public void Delete(T entity) => DbSet.Remove(entity);


        /// <summary>
        /// Delete a recordset synchronously
        /// </summary>
        public void DeleteAll(IEnumerable<T> entities) => DbSet.RemoveRange(entities);


        /// <summary>
        /// Save changes synchronously
        /// </summary>
        public void SaveDB() => _dbContext.SaveChanges();


        /// <summary>
        /// Retrieve a recordset synchronously with support for filtering and relationships
        /// </summary>
        public IQueryable<T> Get(
        Expression<Func<T, bool>>? filter = null,
        IEnumerable<Expression<Func<T, object>>>? includes = null,
        bool tracked = true)
        {
            IQueryable<T> query = DbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }


        /// <summary>
        /// Retrieve one record synchronously based on specific conditions
        /// </summary>
        public T? GetOne(
        Expression<Func<T, bool>>? filter = null,
        IEnumerable<Expression<Func<T, object>>>? includes = null,
        bool tracked = true)
        {
            return Get(filter, includes, tracked).FirstOrDefault();
        }


        #endregion


    }

}
