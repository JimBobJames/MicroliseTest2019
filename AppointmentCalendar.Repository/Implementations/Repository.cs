using System;
using System.Linq;
using System.Threading.Tasks;
using AppointmentCalendar.Domain.Contracts;
using AppointmentCalendar.Repository.Contexts;
using AppointmentCalendar.Repository.Contracts;
using AppointmentCalendar.Repository.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AppointmentCalendar.Repository.Implementations
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        protected readonly AppointmentContext _dbContext;
        private readonly IValidator<TEntity> _validator;

        public Repository(AppointmentContext dbContext,
                            IValidator<TEntity> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetById(TKey id)
        {
            return await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public void Create(TEntity entity)
        {
            ValidationResult validationResult = _validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ToString());
            }

            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            ValidationResult validationResult = _validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ToString());
            }

            _dbContext.Set<TEntity>().Update(entity);
            _dbContext.SaveChanges();
            return GetById(entity.Id).Result;
        }

        public void Delete(TKey id)
        {
            var entity = GetAll().FirstOrDefault(x => x.Id.Equals(id));
            if (entity == null)
            {
                throw new EntityNotFoundException("Unable to delete entity.");
            }

            EntityEntry<TEntity> entry = _dbContext.Entry(entity);
            entry.State = EntityState.Unchanged;
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
