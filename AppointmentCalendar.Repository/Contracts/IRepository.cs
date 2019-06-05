using System.Linq;
using System.Threading.Tasks;
using AppointmentCalendar.Domain.Contracts;

namespace AppointmentCalendar.Repository.Contracts
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(TKey id);

        void Create(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TKey id);
    }
}
