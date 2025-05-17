using System;
using System.Collections;
using System.Collections.Generic;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public interface IUserService
    {
        User Create(User newUser, string createdBy);
        User UpdateProfile(string login, string name, int gender, DateTime? birthday, string performedBy);
        User UpdatePassword(string login, string newPassword, string performedBy);
        User UpdateLogin(string oldLogin, string newLogin, string performedBy);
        IEnumerable<User> GetAllActive();
        IEnumerable<User> GetOlderThan(int age);
        User GetByLogin(string login);
        User Authenticate(string login, string password);
        bool Revoke(string login, bool hardDelete, string performedBy);
        bool Restore(string login, string performedBy);
    }
}