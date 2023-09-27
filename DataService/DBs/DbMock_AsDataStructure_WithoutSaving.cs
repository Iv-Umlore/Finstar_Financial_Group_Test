using DataAccessLayer.Models;

namespace DataAccessLayer.DBs
{
    internal class DbMock_AsDataStructure_WithoutSaving
    {
        private List<ProductInfo> _products = new List<ProductInfo>();
        private int _delay = 200;

        public async Task ClearValues()
        {
            _products.Clear();
            await Task.Delay(_delay);
        }

        public async Task InsertRows(List<ProductInfo> products)
        {
            _products = products;
            await Task.Delay(_delay);
            _products.ForEach(it => Console.WriteLine(it.Code));
        }

        public async Task<IQueryable<ProductInfo>> SelectAll()
        {
            await Task.Delay(_delay);
            return _products.AsQueryable();
        }
    }
}
