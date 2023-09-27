using Common.Models;
using DataAccessLayer.Models;

namespace DbInteractionService
{
    public interface IDataModelInteraction
    {
        /// <summary>
        /// Преобразовать json в модели, преобразовать данные и отправить 
        /// </summary>
        public Task ConvertAndSendToDB(List<InsertInputModel> model);

        /// <summary>
        /// Получить данные из Базы используя фильтрацию
        /// </summary>
        /// <param name="limit"> Ограничение числа записей </param>
        /// <param name="codeFilter"> Фильтрация по коду "продукта" </param>
        /// <param name="valueFilter"> Фильтрация по содержанию фразы </param>
        /// <returns> Информацию о продукте согласно фильтрам </returns>
        public Task<List<ProductInfo>> GetFromDB(int? limit, long? codeFilter, string valueFilter);
    }
}
