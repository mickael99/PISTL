using System.Data.Common;
using Project.Models;
using Project.Interface;

namespace Project.Repository
{
    public class DatabaseRepository: IDatabaseRepository{

        private readonly DatContext _context;

        public DatabaseRepository(DatContext context){
            _context = context;
        }

        public Database GetDatabase(int id){
            return _context.Databases.Where(db => db.DatabaseId == id).FirstOrDefault();
        }

        public ICollection<Database> GetDataBases(){
            return _context.Databases.OrderBy(db => db.DatabaseId).ToList();
        }

        public bool DatabaseExists(int Id)
        {
            return _context.Databases.Any(db => db.DatabaseId == Id);
        }

        public bool CreateDatabase(Database db){
            _context.Add(db);
            return Save();
        }

        public bool Save(){
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateDatabase(Database db){
            _context.Update(db);
            return Save();
        }

        public bool DeleteDatabase(Database db){
            _context.Remove(db);
            return Save();
        }

        public int GetDatabaseCount()
        {
            return _context.Databases.Count();
        }
    }
}