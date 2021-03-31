using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KBDStocker.Models;

namespace KBDStocker.Repositories
{
    public class ProductRepository
    {
        public ProductRepository()
        {
            //create dapper instance
        }

        public IEnumerable<StockItem> ListAll()
        {
            //fetch all stock items. probably won't use
            throw new NotImplementedException();
        }

        public IEnumerable<StockItem> ListAllFromCategory(StockCategory category)
        {
            //get all stock from specific category
            throw new NotImplementedException();
        }

        public StockItem Get(Guid id)
        {
            //Get specific item
            throw new NotImplementedException();
        }

        public void Update(StockItem item)
        {
            //update item
            throw new NotImplementedException();
        }

        public void Insert(StockItem item)
        {
            //will probably never use unless created as a bot command
            throw new NotImplementedException();
        }
        
        
    }
}
