using API.Context;
using API.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class RoleRepository : GeneralRepository<Role, MyContext>
    {
        public RoleRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
