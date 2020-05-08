using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistrationApplication
{
    class HashSalt
    {

        private readonly int num_of_iterations;

        // --- constructor to initialize num_of_iterations
        public HashSalt(int numOfIterations)
        {
            num_of_iterations = numOfIterations;
        }

        // --- Generate Salt ---
        public string  generateSalt()
        {
            var salt = new byte[32];

            var randomProvider = new RNGCryptoServiceProvider();
            randomProvider.GetBytes(salt);

            return Convert.ToBase64String(salt);        // returns salt as a string
        }

        // --- converts salt string into byte[]
        public byte[] saltToByte(string salt)
        {
            var byteSalt = Convert.FromBase64String(salt);
            return byteSalt;
        }

        // --- Generate hash of(pass+salt) ---
        public string generateHash(string password, byte[] salt)
        {
            
            var rfc2898 = new Rfc2898DeriveBytes(password, salt, num_of_iterations);
                       
            var Password = rfc2898.GetBytes(32);    // gives 32 byte encoded password

            return Convert.ToBase64String(Password);    // returns hash
        }

        // --- Authenticate User ---
        public bool authenticateUser(string enteredPassword, string storedHash, string storedSalt)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltToByte(storedSalt), num_of_iterations);
            if (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(32)) == storedHash)  // check whether the passwords are same
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        
        
        
        
        

    }
}
