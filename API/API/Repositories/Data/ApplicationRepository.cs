using API.Context;
using API.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class ApplicationRepository : GeneralRepository<Application, MyContext>
    {
        public ApplicationRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
