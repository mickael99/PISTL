using System.Data.Common;
using Project.Models;
using Project.Interface;
using System.Security.Cryptography;
using System.Text;


namespace Project.Repository
{
    public class DatabaseRepository : IDatabaseRepository
    {

        private readonly DatContext _context;

        public DatabaseRepository(DatContext context)
        {
            _context = context;
        }

        public Database GetDatabase(int id)
        {
            return _context.Databases.Where(db => db.DatabaseId == id).FirstOrDefault();
        }

        public ICollection<Database> GetDataBases()
        {
            // return _context.Databases.OrderBy(db => db.DatabaseId).ToList();
            return _context.Databases.ToList();

        }

        public bool DatabaseExists(int Id)
        {
            return _context.Databases.Any(db => db.DatabaseId == Id);
        }

        public bool DatabaseExists(string Name)
        {
            return _context.Databases.Any(db => db.Name == Name);
        }


        public bool CreateDatabase(Database db)
        {
            _context.Add(db);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateDatabase(Database db)
        {
            _context.Update(db);
            return Save();
        }

        public bool DeleteDatabase(Database db)
        {
            _context.Remove(db);
            return Save();
        }

        public int GetDatabaseCount()
        {
            return _context.Databases.Count();
        }

        public int GetUnusedMinDatabaseId()
        {
            int minDatabaseId = 1;
            while (_context.Databases.Any(db => db.DatabaseId == minDatabaseId))
            {
                minDatabaseId++;
            }
            return minDatabaseId;
        }

        /***************************************************************************************/
        /// <summary>
        /// Generates a random salt.
        /// </summary>
        /// <param name="size">The size of the salt.</param>
        /// <returns>The generated salt.</returns>
        public string GetSalt(int size)
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(size));
        }

        /***************************************************************************************/
        /// <summary>
        /// Encrypts the password using SHA512 algorithm. // TODO change to PBKDF2
        /// </summary>
        /// <param name="password">The password to be encrypted.</param>
        /// <param name="salt">The salt to be used.</param>
        /// <returns>The encrypted password.</returns>
        public string EncryptPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var encryptedBytes = SHA512.HashData(bytes);
            return BitConverter.ToString(encryptedBytes).Replace("-", "");
        }
    }
}