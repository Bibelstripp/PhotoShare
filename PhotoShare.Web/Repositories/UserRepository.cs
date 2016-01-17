using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PhotoShare.Web.Models;

namespace PhotoShare.Web.Repositories
{
    public class UserRepository
    {
        private readonly PhotoContext photoContext;
        public UserRepository(PhotoContext context)
        {
            photoContext = context;
        }
        public User GetUser(string id)
        {
            return photoContext.Users.SingleOrDefault(u => u.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return photoContext.Users
                .SingleOrDefault(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            photoContext.Users.Add(user);
            photoContext.SaveChanges();
        }

        public void Save(User user)
        {
            photoContext.Entry(user).State = EntityState.Modified;
            photoContext.SaveChanges();
        }
    }
}