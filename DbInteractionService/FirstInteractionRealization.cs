using Common.Models;
using DataAccessLayer.Models;
using DataService;
using DbInteractionService;

namespace InteractionService
{
    public class FirstInteractionRealization : IDataModelInteraction
    {
        private IDbInteraction _db;

        public FirstInteractionRealization(IDbInteraction dbInteraction)
        {
            _db = dbInteraction;
        }

        public async Task ConvertAndSendToDB(List<InsertInputModel> model)
        {
            if (model == null)
                throw new ArgumentNullException($"Empty input data : {nameof(model)}");

            // Выпадет ошибка, если Parse не удастся
            List<ProductInfo> sorteredInfo = model.Select(
                it => new ProductInfo()
                {
                    Code = int.Parse(it.Code),
                    Name = it.Value
                }).OrderBy(it=>it.Code).ToList();

            await _db.ClearTable();
            await _db.InsertNewValue(sorteredInfo);
        }

        public async Task<List<ProductInfo>> GetFromDB(int? limit, long? codeFilter, string valueFilter)
        {
            var resultQuary = await _db.GetProductInfo();

            if (!string.IsNullOrEmpty(valueFilter))
                resultQuary = resultQuary.Where(it => it.Name.Contains(valueFilter));

            if (codeFilter.HasValue)
                resultQuary = resultQuary.Where(it=>it.Code == codeFilter.Value);

            if (limit.HasValue)
                resultQuary = resultQuary.Take(limit.Value);

            return resultQuary.ToList();
        }
    }
}
