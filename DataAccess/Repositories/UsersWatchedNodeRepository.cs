﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UsersWatchedNodeRepository : Repository<UsersWatchedNode>, IUsersWatchedNodeRepository
    {
        public UsersWatchedNodeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
