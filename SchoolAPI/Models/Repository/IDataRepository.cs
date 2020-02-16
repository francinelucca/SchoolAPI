using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.Repository
{
    public interface IDataRepository<TEntity, TDto, RDto>
    {
        IQueryable<TEntity> GetAll();
        IEnumerable<TDto> GetAllDtos();
        TEntity Get(int id);
        TDto GetDto(int id);
        void Add(RDto entity);
        void Update(TEntity entityToUpdate, RDto entity);
        void Delete(TEntity entity);
        bool Exists(int id);
    }
}
