using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MS.Infrastructure.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        IDbConnection _connection = null;
        IDbTransaction _transaction = null;
        Guid _id = Guid.Empty;
        IConfiguration _configuration;
        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
            
            _id = Guid.NewGuid();
            _connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        IDbConnection IUnitOfWork.Connection
        {
            get { return _connection; }
        }
        IDbTransaction IUnitOfWork.Transaction
        {
            get { return _transaction; }
        }
        Guid IUnitOfWork.Id
        {
            get { return _id; }
        }

        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
            _transaction = null;
        }
    }
}
