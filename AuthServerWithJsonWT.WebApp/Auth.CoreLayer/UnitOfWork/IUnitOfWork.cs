﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.CoreLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();

        void Commit();
    }
}
