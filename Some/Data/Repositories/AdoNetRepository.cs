using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using CourseProject.Attributes;
using CourseProject.Extensions;
using CourseProject.Models;
using CourseProject.Models.DataModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CourseProject.Data.Repositories;

public class AdoNetRepository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly string _connectionString;
    private readonly IOptions<Settings> _option;

    private readonly TableInfo<TEntity> _tableInfo;

    public AdoNetRepository(IOptions<Settings> option)
    {
        _option = option;
        _connectionString = option.Value.DbConnection;
        _tableInfo = new TableInfo<TEntity>();
    }


    public async Task<int> Create(TEntity entity, int masterId = 0)
    {
        _tableInfo.entity = entity;
        var values = _tableInfo.GetPropertiesForCreate(entity);

        var createCommand = !_tableInfo.IsTransitional
            ? $"INSERT INTO \"{_tableInfo.TableName}\"({_tableInfo.NonReadOnlyTableColumnsDefinition}) " +
              $"OUTPUT INSERTED.ID " +
              $" VALUES ({string.Join(',', values)});"
            : $"INSERT INTO \"{_tableInfo.TableName}\"({_tableInfo.MasterEntityName}_Id,{_tableInfo.RelatedEntityName}_Id,{_tableInfo.TransitionalTableColumnsDefinition}) " +
              $" VALUES ({masterId}, {entity.Id}, {string.Join(',', values)});";
        var createdEntityId = await ExecuteNonQuery(createCommand);

        return createdEntityId;
    }


    public async Task<TEntity> GetById(int value)
    {
        var selectCommand =
            $"SELECT TOP 1 {_tableInfo.TableColumnsDefinition} FROM \"{_tableInfo.TableName}\" WHERE Id='{value}'";

        return (await ExecuteSelect(selectCommand)).FirstOrDefault();
    }

    public async Task<TEntity> Get<TProperty>(
        Expression<Func<TEntity, TProperty>> propertyExpression,
        TProperty value)
    {
        var propertyName = propertyExpression.GetTablePropertyName(_tableInfo);
        var selectCommand =
            $"SELECT TOP 1 {_tableInfo.TableColumnsDefinition} FROM \"{_tableInfo.TableName}\" WHERE {propertyName}='{value}'";

        return (await ExecuteSelect(selectCommand)).FirstOrDefault();
    }

    public async Task<IEnumerable<TEntity>> GetMany<TProperty>(
        Expression<Func<TEntity, TProperty>> propertyExpression,
        TProperty value)
    {
        var propertyName = propertyExpression.GetTablePropertyName(_tableInfo);
        var selectCommand =
            $"SELECT  {_tableInfo.TableColumnsDefinition} FROM \"{_tableInfo.TableName}\" WHERE {propertyName}='{value}'";

        return await ExecuteSelect(selectCommand);
    }

    public async Task<IEnumerable<TEntity>> GetAll(int masterId = 0)
    {
        var selectCommand = !_tableInfo.IsTransitional
            ? $"SELECT {_tableInfo.TableColumnsDefinition} FROM \"{_tableInfo.TableName}\""
            : $"SELECT {_tableInfo.TableColumnsDefinition} FROM \"{_tableInfo.TableName}\" " +
              $"JOIN {_tableInfo.RelatedTableName} " +
              $"ON {_tableInfo.RelatedTableName}.Id = {_tableInfo.TableName}.{_tableInfo.RelatedEntityName}_Id " +
              $"WHERE {_tableInfo.MasterEntityName}_Id='{masterId}'";

        return await ExecuteSelect(selectCommand);
    }

    public async Task Update<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression,
        TProperty value)
    {
        var propertyName = propertyExpression.GetTablePropertyName(_tableInfo);
        var values = _tableInfo.GetPropertiesForUpdate(entity);
        var updateCommand =
            $"UPDATE \"{_tableInfo.TableName}\" SET {string.Join(',', values)} WHERE {propertyName}='{value}'";

        await ExecuteNonQuery(updateCommand);
    }

    public async Task Delete<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, TProperty value)
    {
        var propertyName = propertyExpression.GetTablePropertyName(_tableInfo);
        var deleteCommand = $"DELETE FROM \"{_tableInfo.TableName}\" WHERE {propertyName}='{value}'";

        await ExecuteNonQuery(deleteCommand);
    }

    public async Task ExecuteRawSql(string commandText)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand();
        await connection.OpenAsync();
        command.Connection = connection;
        command.CommandText = commandText;

        var transaction = connection.BeginTransaction();
        command.Transaction = transaction;
    }

    public async Task<SqlTransaction> CreateTransactionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        return connection.BeginTransaction();
    }

    public async Task ExecuteTransactionAsync(SqlTransaction transaction, string commandText)
    {
        if (!CheckConnection(transaction))
        {
            return;
            ;
        }

        await using (var command = new SqlCommand())
        {
            command.Connection = transaction.Connection;

            command.CommandText = commandText;
            command.Transaction = transaction;
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task CommitAsync(SqlTransaction transaction)
    {
        if (!CheckConnection(transaction))
        {
            return;
            ;
        }

        await using (var connection = transaction.Connection)
        {
            await transaction.CommitAsync();
            await connection.CloseAsync();
        }
    }

    public async Task RollbackAsync(SqlTransaction transaction)
    {
        if (!CheckConnection(transaction))
        {
            return;
            ;
        }

        await using (var connection = transaction.Connection)
        {
            await transaction.RollbackAsync();
            await connection.CloseAsync();
        }
    }

    protected async Task<int> ExecuteNonQuery(string commandText)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand();
        await connection.OpenAsync();
        command.Connection = connection;
        command.CommandText = commandText;

        var transaction = connection.BeginTransaction();
        command.Transaction = transaction;

        var result = 0;
        try
        {
            if (new CultureInfo("ru").CompareInfo.IndexOf(commandText, "output", CompareOptions.IgnoreCase) >= 0)
                result = (int)await command.ExecuteScalarAsync();
            else await command.ExecuteNonQueryAsync();
        }
        catch (SqlException e)
        {
            throw new Exception(commandText + "\n" + e.Message);
        }

        transaction.Commit();
        await connection.CloseAsync();
        if (_tableInfo.HasToManyRelation &&
            !(new CultureInfo("ru").CompareInfo.IndexOf(commandText, "update", CompareOptions.IgnoreCase) >= 0))
            foreach (var relatedEntity in (IEnumerable<Entity>)_tableInfo.EntityType
                         .GetProperty(_tableInfo.EntityType
                             .GetProperties()
                             .First(p => p.GetCustomAttribute<ForeignKeyToMany>() != null).Name)
                         .GetValue(_tableInfo.entity, null))
            {
                var genericType = _tableInfo.EntityType.GetProperties()
                    .First(prop => prop.GetCustomAttribute<ForeignKeyToMany>() != null)
                    .PropertyType.GetGenericArguments()[0];
                var repositoryType = typeof(AdoNetRepository<>).MakeGenericType(genericType);
                var repository = Activator.CreateInstance(repositoryType, _option);
                var method = repositoryType.GetMethod("Create");

                var resultTask =
                    await method.InvokeAsync(repository, relatedEntity, result);
            }

        return result;
    }

    private bool CheckConnection(SqlTransaction transaction)
    {
        return transaction.Connection?.State == ConnectionState.Open;
    }

    protected async Task<List<TEntity>> ExecuteSelect(string selectCommand)
    {
        var resultEntities = new List<TEntity>();
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand();
        await connection.OpenAsync();
        command.Connection = connection;
        command.CommandText = selectCommand;

        var transaction = connection.BeginTransaction();
        command.Transaction = transaction;
        SqlDataReader reader;
        try
        {
            reader = await command.ExecuteReaderAsync();
        }
        catch (Exception e)
        {
            throw new Exception(selectCommand + "\n" + e.Message);
        }


        while (await reader.ReadAsync())
        {
            var resultEntity = new TEntity();


            for (var i = 0;
                 i < _tableInfo.TableColumns.Length;
                 i++)
            {
                var propertyValue = reader.GetValue(i);
                if (propertyValue.GetType() == typeof(DBNull)) continue;

                if (_tableInfo.EntityType.GetProperty(_tableInfo.EntityColumns[i])
                    .GetCustomAttributes<ForeignKeyAttribute>().Any())
                {
                    var genericType = _tableInfo.EntityType
                        .GetProperty(_tableInfo.EntityColumns[i])?.PropertyType;
                    var repositoryType = typeof(AdoNetRepository<>).MakeGenericType(genericType);
                    var repository = Activator.CreateInstance(repositoryType, _option);
                    var method = repositoryType.GetMethod("GetById");
                    var resultTask = method.InvokeAsync(repository, propertyValue);
                    var result = await resultTask;
                    _tableInfo.EntityType.GetProperty(_tableInfo.EntityColumns[i])
                        ?.SetValue(resultEntity, result);
                }
                else
                {
                    _tableInfo.EntityType.GetProperty(_tableInfo.EntityColumns[i])?
                        .SetValue(resultEntity, propertyValue);
                }
            }

            if (_tableInfo.HasToManyRelation)
            {
                var genericType = _tableInfo.EntityType.GetProperties()
                    .First(prop => prop.GetCustomAttribute<ForeignKeyToMany>() != null)
                    .PropertyType.GetGenericArguments()[0];
                var repositoryType = typeof(AdoNetRepository<>).MakeGenericType(genericType);
                var repository = Activator.CreateInstance(repositoryType, _option);
                var method = repositoryType.GetMethod("GetAll");
                var resultTask = method.InvokeAsync(repository, resultEntity.Id);
                var result = await resultTask;
                var propName = _tableInfo.EntityType.GetProperties()
                    .First(prop =>
                        prop.PropertyType.GetGenericArguments().FirstOrDefault() ==
                        result.GetType().GetGenericArguments()[0]).Name;
                _tableInfo.EntityType.GetProperty(propName)
                    ?.SetValue(resultEntity, result);
            }

            resultEntities.Add(resultEntity);
        }

        await reader.CloseAsync();
        transaction.Commit();
        await connection.CloseAsync();

        return resultEntities;
    }
}