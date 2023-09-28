using DataAccessLayer.Models;
using DataService;

namespace DataAccessLayer
{
    public class DbInteractionRealize_File : DelayAbstruct, IDbInteraction
    {
        FileStream _fileStream;
        private const string fileName = "db.txt";
        private const string separator = "&& &&";

        public DbInteractionRealize_File()
        {
            _fileStream = File.Open(fileName, FileMode.OpenOrCreate);
            _fileStream.Close();
        }

        public async Task ClearTable()
        {
            File.Delete(fileName);

            _fileStream = File.Open(fileName, FileMode.OpenOrCreate);
            _fileStream.Close();

            await Task.Delay(_delay);
        }

        public async Task<IQueryable<ProductInfo>> GetProductInfo()
        {
            List<ProductInfo> infos = new List<ProductInfo>();
            using (StreamReader read = new StreamReader(fileName)) {
                string? tmp = "";
                while (tmp != null)
                {
                    tmp = await read.ReadLineAsync();
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        infos.Add(ConvertStringToPI(tmp));
                    }
                }
            }

            await Task.Delay(_delay);

            return infos.AsQueryable();
        }

        public async Task InsertNewValue(List<ProductInfo> productInfos)
        {

            using (StreamWriter write = new StreamWriter(fileName))
            {
                ulong counter = 1;
                foreach (var pInfo in productInfos)
                {
                    pInfo.Number = counter++;
                    await write.WriteLineAsync(ConvertPItoString(pInfo));
                }
            }

            await Task.Delay(_delay);
        }

        private ProductInfo ConvertStringToPI(string line)
        {
            string[] splitRes = line.Split(separator);

            ProductInfo productInfo = new ProductInfo() {
                Number = ulong.Parse(splitRes[0]),
                Code = long.Parse(splitRes[1]),
                Name = splitRes[2]
            };

            return productInfo;
        }

        private string ConvertPItoString(ProductInfo pInfo)
        {
            return $"{pInfo.Number}{separator}{pInfo.Code}{separator}{pInfo.Name}";
        }
    }
}
