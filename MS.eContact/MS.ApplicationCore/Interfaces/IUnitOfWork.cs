﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MS.ApplicationCore.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        Guid Id { get; }
        //IDbConnection Connection { get; }
        //IDbTransaction Transaction { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        IAlbumRepository Albums { get; }
        IEventDetailRepository EventDetails { get; }
        IEventRepository Events { get; }
        IPictureRepository Pictures { get; }
        IUserRepository Users { get; }
        IUserRolesRepository UserRoles { get; }

    }
}
