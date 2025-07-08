using PekiYaBen.API.Models;
using PekiYaBen.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Helpers
{
    public static class AppUserHelper
    {
        public static IEnumerable<AppUser> WithoutPasswords(this IEnumerable<AppUser> users)
        {
            return users.Select(x => x.WithoutPassword());
        }
        public static AppUser WithoutPassword(this AppUser user)
        {
            user.Password = null;
            return user;
        }
    }
}
