using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject.Models.DataModels;

namespace CourseProject.Data.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<int> Create(TEntity entity, int masterId = 0);

    Task<TEntity> Get<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, TProperty value);

    Task<IEnumerable<TEntity>> GetMany<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression,
        TProperty value);

    Task<IEnumerable<TEntity>> GetAll(int id = 0);

    Task<TEntity> GetById(int id);

    Task Update<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression, TProperty value);

    Task Delete<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, TProperty value);

    Task ExecuteRawSql(string command);
}