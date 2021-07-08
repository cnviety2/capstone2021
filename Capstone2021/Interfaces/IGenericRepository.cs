using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Capstone2021.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    T GetByID(object id);
    void Delete(object id);
    void Delete(T entityToDelete);
    void Update(T entityToUpdate);
    }
}
