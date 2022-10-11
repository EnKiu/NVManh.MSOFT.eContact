﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MS.Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkTest: IDisposable
    {
        Guid Id { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Begin();
        void Commit();
        void Rollback();
    }
}
