using BikesIsland.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Interfaces
{
    public interface IDataRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T newEntity);
        Task<T> GetAsync(string entityId, string partionKey);
        Task<T> UpdateAsync(T entity, string partionKey);
        Task DeleteAsync(string entityId, string partionKey);
        //Task<IReadOnlyList<T>> GetAllAsync();
    }
}
