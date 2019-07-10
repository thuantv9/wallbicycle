using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DentistryManager.Models
{
    public interface IRepository<T> where T : class
    {
        T GetById(string id);
        IEnumerable<T> List();
        IEnumerable<T> List(Func<T, bool> predicate);
        int Add(T entity);
        int Delete(T entity);
        int Edit(T entity);
    }

    //public abstract class EntityBase
    //{
    //    public  string id { get; protected set; }
    //}
}
