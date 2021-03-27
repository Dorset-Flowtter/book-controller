using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using book.Helpers;
using book.Models;
using book.Data;

namespace book.Services
{
    public interface ICustomerService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string currentPassword, string password, string confirmPassword);
        void Delete(int id);
    }

    public class CustomerService : ICustomerService
    {
        private Context _context;

        public CustomerService(Context context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.User.FirstOrDefault(x => x.username == username) ?? null;

            // check if username exists
            if (user == null)
            {
                return null;
            }

            // Granting access if the hashed password in the database matches with the password(hashed in computeHash method) entered by user.
            if(computeHash(password) != user.passwordhash)
            {
                return null;
            }
            return user;        
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User;
        }

        public User GetById(int id)
        {
            return _context.User.Find(id);
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.User.Any(x => x.username == user.username))
            {
                throw new AppException("username \"" + user.username + "\" is already taken");
            }

            //Saving hashed password into Database table
            user.passwordhash = computeHash(password);  

            _context.User.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User customerParam, string currentPassword = null, string password = null, string confirmPassword = null)
        {
            //Find the user by Id
            var user = _context.User.Find(customerParam.id);

            if (user == null) 
            {
                throw new AppException("User not found");
            }
            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(customerParam.username) && customerParam.username != user.username)
            {
                // throw error if the new username is already taken
                if (_context.User.Any(x => x.username == customerParam.username))
                {
                    throw new AppException("Username " + customerParam.username + " is already taken");
                }
                else
                {
                    user.username = customerParam.username;
                }
            }
            if (!string.IsNullOrWhiteSpace(customerParam.firstname))
            {
                user.firstname = customerParam.firstname;
            }
            if (!string.IsNullOrWhiteSpace(customerParam.lastname))
            {
                user.lastname = customerParam.lastname;
            }
            if (!string.IsNullOrWhiteSpace(currentPassword))
            {   
                if(computeHash(currentPassword) != user.passwordhash)
                {
                    throw new AppException("Invalid Current password!");
                }

                if(currentPassword == password)
                {
                    throw new AppException("Please choose another password!");
                }

                if(password != confirmPassword)
                {
                    throw new AppException("Password doesn't match!");
                }
    
                //Updating hashed password into Database table
                user.passwordhash = computeHash(password); 
            }
            
            _context.User.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.User.Find(id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

        private static string computeHash(string Password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var input = md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
            var hashstring = "";
            foreach(var hashbyte in input)
            {
                hashstring += hashbyte.ToString("x2"); 
            } 
            return hashstring;
        }
    }
}