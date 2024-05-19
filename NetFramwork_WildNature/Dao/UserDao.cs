using NetFramwork_WildNature.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetFramwork_WildNature.Dao
{
    public class UserDao
    {   
        WildNature db=null;
        public UserDao() {
            db= new WildNature();
        }

        public Account GetById(string Name)
        {
            return db.Accounts.SingleOrDefault(x => x.Name == Name);
        }
        public int Login(string Name, string password)
        {
            var result  =db.Accounts.Count(x=>x.Name == Name );
            if (result == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}