using System.Data.Common;
using Project.Data;
using Project.Models;

namespace Project.Repository
{
    public class DatabaseRepository{

        private readonly DatabaseContext _context;
        public DatabaseRepository(DatabaseContext context){
            _context = context;
        }

        public Database GetDatabase(){
            return _context.Databases.OrderBy(DbBatch => DbBatch.DatabaseId).FirstOrDefault();
        }
    }
}