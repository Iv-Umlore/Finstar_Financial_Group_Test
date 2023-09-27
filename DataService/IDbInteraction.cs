using DataAccessLayer.Models;

namespace DataService
{
    public interface IDbInteraction
    {
        /// <summary>
        /// Очистить таблицу
        /// </summary>
        public Task ClearTable();

        /// <summary>
        /// Поместить в базу новые значения
        /// </summary>
        public Task InsertNewValue(List<ProductInfo> productInfos);

        /// <summary>
        /// Получить данные из БД
        /// </summary>
        public Task<IQueryable<ProductInfo>> GetProductInfo();
    }
}
