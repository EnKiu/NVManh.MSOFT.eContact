﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MS.ApplicationCore.Interfaces;
using MySqlConnector;

namespace MS.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Properties
        public IAlbumRepository Albums { get; }
        public IEventDetailRepository EventDetails { get; }
        public IEventRepository Events { get; }
        public IPictureRepository Pictures { get; }
        public IUserRepository Users { get; }
        #endregion

        private readonly MySqlDbContext _dbContext;
        Guid _id = Guid.Empty;
        public UnitOfWork(
            IAlbumRepository albums,
            IEventDetailRepository eventDetails,
            IEventRepository events,
            IPictureRepository pictures,
            IUserRepository users,
            MySqlDbContext dbContext)
        {
            _id = Guid.NewGuid();
            Albums = albums;
            EventDetails = eventDetails;
            Events = events;
            Pictures = pictures;
            Users = users;
            _dbContext = dbContext;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public void BeginTransaction()
        {
            _dbContext.Transaction = _dbContext.Connection.BeginTransaction();
        }

        public void Commit()
        {
            _dbContext.Transaction.Commit();
        }

        public void Rollback()
        {
            _dbContext.Transaction.Rollback();
        }

        public void Dispose()
        {
            _dbContext.Transaction?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
