using DataAccessLayer.DBs;
using DataAccessLayer.Models;
using DataService;

namespace DataAccessLayer
{
    public class DbInteractionRealize_Cache : DelayAbstruct, IDbInteraction
    {
        private DbMock_AsDataStructure_WithoutSaving _db;

        public DbInteractionRealize_Cache()
        {
            _db = new DbMock_AsDataStructure_WithoutSaving(_delay);
        }

        public async Task ClearTable()
        {
            await _db.ClearValues();
        }

        public async Task<IQueryable<ProductInfo>> GetProductInfo()
        {
            return await _db.SelectAll();
        }

        public async Task InsertNewValue(List<ProductInfo> productInfos)
        {
            ulong Id = 1;
            foreach (var productInfo in productInfos)
            {
                productInfo.Number = Id++;
            }

            await _db.InsertRows(productInfos);
        }
    }
}
