using Dapper;
using Microsoft.Extensions.Configuration;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Helpers;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.UnitOfWork
{
    public class MySqlDbContext:IDisposable
    {
        protected readonly string ConnectionString;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }
        public MySqlDbContext(IConfiguration configuration) 
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
        }
        public void Dispose() => Connection?.Dispose();

        #region Methods
        public virtual async Task<IReadOnlyList<T>> GetAllAsync<T>()
        {
            var tableName = GetTableName<T>();
            var data = await Connection.QueryAsync<T>($"select * from {tableName}", transaction: Transaction);
            return data.ToList();
        }

        public virtual async Task<T> GetByIdAsync<T>(string id)
        {
            var tableName = GetTableName<T>();
            var propKeyName = GetPrimaryKeyName<T>();

            var sqlCommand = $"SELECT * FROM {tableName} WHERE {propKeyName} = @Id";
            var parameters = new DynamicParameters();
            parameters.Add($"@Id", id);
            var entity = await Connection.QueryFirstOrDefaultAsync<T>(sqlCommand, param: parameters,transaction: Transaction);
            return entity;
        }

        public virtual async Task<int> AddAsync<T>(T entity) where T : BaseEntity
        {
            var sqlCommand = BuildAddQuery(entity);
            var rowAdded = await Connection.ExecuteAsync(sqlCommand, param: entity, transaction: Transaction);
            return rowAdded;
        }

        public virtual async Task<int> UpdateAsync<T>(T entity) where T : BaseEntity
        {
            var sqlCommand = BuildUpdateQuery(entity);
            var rowUpdated = await Connection.ExecuteAsync(sqlCommand, param: entity, transaction: Transaction);
            return rowUpdated;
        }

        public virtual async Task<int> DeleteAsync<T>(Guid id)
        {
            var sqlCommand = BuildDeleteQueryById<T>();
            var propKeyName = GetPrimaryKeyName<T>();
            var parameters = new DynamicParameters();
            parameters.Add($"@{propKeyName}Id", id);
            var rowsDeleted = await Connection.ExecuteAsync(sqlCommand, param: parameters, transaction: Transaction);
            return rowsDeleted;
        }

        public virtual async Task<int> DeleteAsync<T>(T entity)
        {
            var tableName = GetTableName<T>();
            var sqlCommand = BuildDeleteQueryById<T>();
            var rowsDeleted = await Connection.ExecuteAsync(sqlCommand, param: entity, transaction: Transaction);
            return rowsDeleted;
        }
        #endregion

        #region Utilities
        private string GetTableName<T>()
        {
            var tableAttr = typeof(T).GetCustomAttributes(typeof(DataTableName), true).FirstOrDefault();
            if (tableAttr != null)
            {
                return (tableAttr as DataTableName).Name;
            }
            else
            {
                return typeof(T).Name;
            }
        }
        public string GetPrimaryKeyName<T>()
        {
            var tableName = GetTableName<T>();
            var properties = typeof(T).GetProperties();
            var propsKey = properties.Where(e => { return e.IsDefined(typeof(PrimaryKey), false) == true; }).FirstOrDefault();
            if (propsKey != null)
                return propsKey.Name;
            else
                return $"{tableName}Id";
        }

        /// <summary>
        /// Build chuỗi câu truy vấn thêm mới;
        /// </summary>
        /// <param name="entity">đối tượng thêm mới</param>
        /// <returns>chuỗi câu truy vấn INSERT</returns>
        /// Author: NVMANH (11/08/2022)
        public string? BuildAddQuery<T>(T entity)
        {
            if (entity == null)
                return null;
            var tableName = GetTableName<T>();
            var properties = entity.GetType().GetProperties();
            var propsMapQuery = properties.Where(e => { return !e.IsDefined(typeof(NotMapQuery), false); });
            var colCommandList = string.Empty;
            var colCommandParamList = string.Empty;
            var propPrimaryKeyName = GetPrimaryKeyName<T>();
            foreach (var prop in propsMapQuery)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);
                var propType = prop.PropertyType;
                if ((propName.ToLower() == propPrimaryKeyName.ToLower()) && propType == typeof(Guid) && (propValue == null || propValue.ToString() == Guid.Empty.ToString()))
                {
                    prop.SetValue(entity, Guid.NewGuid());
                    propName = propPrimaryKeyName;
                }

                colCommandList = String.Join(",", colCommandList, propName);
                colCommandParamList = String.Join(",", colCommandParamList, $"@{propName}");
            }
            colCommandList = colCommandList.Substring(1, colCommandList.Length - 1);
            colCommandParamList = colCommandParamList.Substring(1, colCommandParamList.Length - 1);
            var sql = $"INSERT INTO {tableName}({colCommandList}) VALUES ({colCommandParamList})";
            return sql;
        }

        /// <summary>
        /// Build chuỗi câu truy vấn sửa;
        /// </summary>
        /// <param name="entity">đối tượng thêm mới</param>
        /// <returns>chuỗi câu truy vấn UPDATE</returns>
        /// Author: NVMANH (11/08/2022)
        public string? BuildUpdateQuery<T>(T entity)
        {
            if (entity != null)
            {
                var tableName = GetTableName<T>();
                var properties = entity.GetType().GetProperties();
                var propsMapQuery = properties.Where(e => { return !e.IsDefined(typeof(NotMapQuery), false); });
                var colCommandList = string.Empty;
                var colCommandParamList = string.Empty;
                var propKeyName = GetPrimaryKeyName<T>();
                foreach (var prop in propsMapQuery)
                {
                    var propName = prop.Name;
                    if (propName == propKeyName)
                        continue;
                    var propValue = prop.GetValue(entity);
                    var propType = prop.PropertyType;
                    colCommandList = String.Join(",", colCommandList, $"{propName}=@{propName}");
                }


                colCommandList = colCommandList.Substring(1, colCommandList.Length - 1);
                var sql = $"UPDATE {tableName} SET {colCommandList} WHERE {propKeyName} = @{propKeyName}";
                return sql;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Build chuỗi câu sql xóa dữ liệu theo khóa chính;
        /// </summary>
        /// <returns>chuỗi câu truy vấn DELETE</returns>
        /// Author: NVMANH (11/08/2022)
        public string BuildDeleteQueryById<T>()
        {
            var tableName = GetTableName<T>();
            var propKeyName = GetPrimaryKeyName<T>();
            return $"DELETE FROM {tableName} WHERE {propKeyName}Id = @{propKeyName}Id";
        }
        #endregion
    }
}
