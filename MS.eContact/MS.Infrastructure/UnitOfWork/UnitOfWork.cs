using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MS.ApplicationCore.Interfaces;
using MySqlConnector;

namespace MS.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties
        public IAlbumRepository Albums { get; }
        public IEventDetailRepository EventDetails { get; }
        public IEventRepository Events { get; }
        public IPictureRepository Pictures { get; }
        public IUserRepository Users { get; }
        #endregion

        IDbConnection _connection = null;
        IDbTransaction _transaction = null;
        Guid _id = Guid.Empty;
        IConfiguration _configuration;
        public UnitOfWork(IConfiguration configuration, 
            IAlbumRepository albums, 
            IEventDetailRepository eventDetails, 
            IEventRepository events, 
            IPictureRepository pictures, 
            IUserRepository users)
        {
            _configuration = configuration;

            _id = Guid.NewGuid();
            _connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            Albums = albums;
            EventDetails = eventDetails;
            Events = events;
            Pictures = pictures;
            Users = users;
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }
        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }
        public Guid Id
        {
            get { return _id; }
        }

        public void BeginTransaction()
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
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
