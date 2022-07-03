using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using CourseProject.Attributes;
using CourseProject.Models;
using CourseProject.Models.DataModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CourseProject.Data
{
    public class TableInfo<TEntity> where TEntity : new()
    {
        public TableInfo()
        {
            EntityType = new TEntity().GetType();
            IsTransitional = EntityType.GetCustomAttribute<TransitionTableNameAttribute>() != null;

            TableName = IsTransitional
                ? EntityType.GetCustomAttribute<TransitionTableNameAttribute>()?.Name
                : EntityType.GetCustomAttributes<TableNameAttribute>()?.First().Name;
            EntityColumns = EntityType.GetProperties()
                .Where(prop => prop.GetCustomAttribute<ForeignKeyToMany>() == null).Select(x => x.Name).ToArray();
            TableColumns = EntityType.GetProperties().Where(x => x.GetCustomAttribute<ForeignKeyToMany>() == null)
                .Select(x =>
                    x.GetCustomAttributes<ForeignKeyAttribute>().Any()
                        ? x.GetCustomAttributes<ForeignKeyAttribute>().First().Name
                        : x.Name).ToArray();

            EntityColumnsDefinition = string.Join(',', EntityColumns.Select(x => $"\"{x}\""));
            EntityColumnsParameters = string.Join(',', EntityColumns.Select(x => $"@{x}"));
            TableColumnsDefinition =
                string.Join(',', TableColumns.Select(x => $"\"{x}\""));

            ReadOnlyColumns = EntityType.GetProperties()
                .Where(x => x.GetCustomAttribute(typeof(ReadOnlyPropertyAttribute)) != null)
                .Select(x => x.Name).ToArray();
            NonReadOnlyColumns = EntityColumns.Where(x => !ReadOnlyColumns.Contains(x)).ToArray();
            NonReadOnlyTableColumns = TableColumns.Where(x => !ReadOnlyColumns.Contains(x)).ToArray();

            NonReadOnlyTableColumnsDefinition = string.Join(',', NonReadOnlyTableColumns.Select(x => $"\"{x}\""));
            NonReadOnlyTableColumnsParameters = string.Join(',', NonReadOnlyColumns.Select(x => $"@{x}"));


            HasToManyRelation = EntityType.GetProperties()
                .Any(prop => prop.GetCustomAttribute<ForeignKeyToMany>() != null);
            if (HasToManyRelation)
                RelatedEntityType = EntityType.GetProperties()
                    .First(prop => prop.GetCustomAttribute<ForeignKeyToMany>() != null).PropertyType
                    .GetGenericArguments()[0];
            RelatedTableName = EntityType.GetCustomAttribute<RelatedTableNameAttribute>()?.Name;
            RelatedEntityName = EntityType.GetCustomAttribute<RelatedEntityTypeAttribute>()?.Name;
            MasterEntityName = EntityType.GetCustomAttribute<MasterEntityNameAttribute>()?.Name;

            TransitionalTableColumns = EntityType
                .GetProperties(System.Reflection.BindingFlags.Public 
                               | System.Reflection.BindingFlags.Instance 
                               | System.Reflection.BindingFlags.DeclaredOnly)
                .Where(x => x.GetCustomAttribute<ForeignKeyToMany>() == null).Select(x =>
                    x.GetCustomAttributes<ForeignKeyAttribute>().Any()
                        ? x.GetCustomAttributes<ForeignKeyAttribute>().First().Name
                        : x.Name).ToArray();
            TransitionalTableColumnsDefinition = 
                string.Join(',', TransitionalTableColumns.Select(x => $"{TableName}.\"{x}\""));

            
        }

        public string TransitionalTableColumnsDefinition { get; set; }

        public string[] NonReadOnlyTableColumns { get; set; }

        public string TableName { get; }

        public string[] EntityColumns { get; }
        public string[] TableColumns { get; }

        public string EntityColumnsDefinition { get; }
        public string TableColumnsDefinition { get; }

        public string NonReadOnlyTableColumnsDefinition { get; }

        public string EntityColumnsParameters { get; }

        public string NonReadOnlyTableColumnsParameters { get; }

        public Type EntityType { get; }

        public string[] ReadOnlyColumns { get; }

        public string[] NonReadOnlyColumns { get; }

        public bool IsTransitional { get; }
        public bool HasToManyRelation { get; }

        public Type RelatedEntityType { get; }

        public string RelatedTableName { get; }

        public string RelatedEntityName { get; }

        public string MasterEntityName { get; }

        public string[] TransitionalTableColumns { get; }

        
        public TEntity entity { get; set; }

        public string[] GetPropertiesForUpdate(TEntity entity)
        {
            var values = new string[NonReadOnlyTableColumns.Length];
            var entityProperties = EntityType.GetProperties();
            for (var i = 0; i < NonReadOnlyTableColumns.Length; i++)
            {
                string propertyValue;
                var prop = entityProperties.FirstOrDefault(x => x.Name == NonReadOnlyColumns[i]);
                if (!prop.GetCustomAttributes<ForeignKeyAttribute>().Any())
                {
                    propertyValue = prop?.GetValue(entity)?.ToString() ?? "";
                }
                else
                {
                    var source = prop.GetValue(entity, null);
                    var destination = Activator.CreateInstance(prop.PropertyType);
                    var innerProp = destination.GetType().GetProperty("Id");
                    var value = source.GetType().GetProperty(innerProp.Name).GetValue(source, null);

                    propertyValue = value.ToString();
                }

                values[i] = $"\"{NonReadOnlyTableColumns[i]}\"='{propertyValue}'";
            }

            return values;
        }

        public string[] GetPropertiesForCreate(TEntity entity)
        {
            return IsTransitional ? GetPropertiesForCreateTransitionEntity(entity) : GetPropertiesForCreateEntity(entity);
        }

        private string[] GetPropertiesForCreateEntity(TEntity entity)
        {
            var values = new string[NonReadOnlyColumns.Length];
            var entityProperties = EntityType.GetProperties();
            for (var i = 0; i < NonReadOnlyColumns.Length; i++)
            {
                string propertyValue;
                var prop = entityProperties.FirstOrDefault(x => x.Name == NonReadOnlyColumns[i]);
                if (!prop.GetCustomAttributes<ForeignKeyAttribute>().Any())
                {
                    propertyValue = prop?.GetValue(entity)?.ToString() ?? "";
                }
                else
                {
                    var source = prop.GetValue(entity, null);
                    var destination = Activator.CreateInstance(prop.PropertyType);
                    var innerProp = destination.GetType().GetProperty("Id");
                    var value = source.GetType().GetProperty(innerProp.Name).GetValue(source, null);

                    propertyValue = value.ToString();
                }

                values[i] = $"'{propertyValue}'";
            }

            return values;
        }
        private string[] GetPropertiesForCreateTransitionEntity(TEntity entity)
        {
            var values = new string[TransitionalTableColumns.Length];
            var entityProperties = EntityType.GetProperties();
            for (var i = 0; i < TransitionalTableColumns.Length; i++)
            {
                var prop = entityProperties.FirstOrDefault(x => x.Name == TransitionalTableColumns[i]);
                var propertyValue = prop?.GetValue(entity)?.ToString() ?? "";
                values[i] = $"'{propertyValue}'";
            }

            return values;
        }
    }
}