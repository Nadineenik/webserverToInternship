using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UserManagementAPI.Models;



namespace UserManagementAPI.Services
{
    public class InMemoryUserService : IUserService
    {
        private readonly ConcurrentDictionary<string, User> _users = new();

        public InMemoryUserService()
        {
            // Seed Admin
            var admin = new User
            {
                Login = "admin",
                Password = "admin123",
                Name = "Administrator",
                Gender = 2,
                Admin = true,
                CreatedBy = "admin"
            };
            _users[admin.Login] = admin;
        }

        public User Create(User newUser, string createdBy)
        {
            if (_users.ContainsKey(newUser.Login)) throw new Exception("Login already exists");
            newUser.CreatedBy = createdBy;
            newUser.CreatedOn = DateTime.UtcNow;
            _users[newUser.Login] = newUser;
            return newUser;
        }

        public User UpdateProfile(string login, string name, int gender, DateTime? birthday, string performedBy)
        {
            if (!_users.TryGetValue(login, out var user)) throw new Exception("User not found");
            if (user.RevokedOn != null && !user.Admin) throw new Exception("User is revoked");
            user.Name = name;
            user.Gender = gender;
            user.Birthday = birthday;
            user.ModifiedBy = performedBy;
            user.ModifiedOn = DateTime.UtcNow;
            return user;
        }

        public User UpdatePassword(string login, string newPassword, string performedBy)
        {
            if (!_users.TryGetValue(login, out var user)) throw new Exception("User not found");
            if (user.RevokedOn != null && !user.Admin) throw new Exception("User is revoked");
            user.Password = newPassword;
            user.ModifiedBy = performedBy;
            user.ModifiedOn = DateTime.UtcNow;
            return user;
        }

        public User UpdateLogin(string oldLogin, string newLogin, string performedBy)
        {
            if (_users.ContainsKey(newLogin)) throw new Exception("New login already exists");
            if (!_users.TryRemove(oldLogin, out var user)) throw new Exception("User not found");
            user.Login = newLogin;
            user.ModifiedBy = performedBy;
            user.ModifiedOn = DateTime.UtcNow;
            _users[newLogin] = user;
            return user;
        }

        public IEnumerable<User> GetAllActive() => _users.Values
            .Where(u => u.RevokedOn == null)
            .OrderBy(u => u.CreatedOn);

        public User GetByLogin(string login)
        {
            if (!_users.TryGetValue(login, out var user)) throw new Exception("User not found");
            return user;
        }

        public User Authenticate(string login, string password)
        {
            var user = GetByLogin(login);
            if (user.Password != password) throw new Exception("Invalid credentials");
            if (user.RevokedOn != null) throw new Exception("User is revoked");
            return user;
        }

        public IEnumerable<User> GetOlderThan(int age)
        {
            var cutoff = DateTime.UtcNow.AddYears(-age);
            return _users.Values.Where(u => u.Birthday.HasValue && u.Birthday < cutoff);
        }

        public bool Revoke(string login, bool hardDelete, string performedBy)
        {
            if (!_users.TryGetValue(login, out var user)) throw new Exception("User not found");
            if (hardDelete) return _users.TryRemove(login, out _);
            user.RevokedOn = DateTime.UtcNow;
            user.RevokedBy = performedBy;
            return true;
        }

        public bool Restore(string login, string performedBy)
        {
            if (!_users.TryGetValue(login, out var user)) throw new Exception("User not found");
            user.RevokedOn = null;
            user.RevokedBy = null;
            user.ModifiedBy = performedBy;
            user.ModifiedOn = DateTime.UtcNow;
            return true;
        }
    }


}