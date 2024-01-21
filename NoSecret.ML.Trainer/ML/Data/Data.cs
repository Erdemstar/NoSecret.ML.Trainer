using MongoDB.Driver;
using NoSecret.AI.Entity.Input;
using NoSecret.AI.Entity.Vulnerability;

namespace NoSecret.AI.ML.Data;

public class Data
{
    private readonly string _collectionName;
    private readonly string _connectionString;
    private readonly string _dbName;

    public Data(string connectionString, string dbName, string collectionName)
    {
        _connectionString = connectionString;
        _collectionName = collectionName;
        _dbName = dbName;
    }

    private IEnumerable<VulnerabilityEntity> GetVulnerabilityDataFromMongoDB(string RegexName)
    {
        //var connectionString = "mongodb://localhost:27017";
        //var dbName = "NoSecret";
        //var collectionName = "Vulnerability";

        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase(_dbName);
        var collection = database.GetCollection<VulnerabilityEntity>(_collectionName);

        var fieldName = "RegexName"; // Filtrelemek istediğiniz alanın adı
        var filter = Builders<VulnerabilityEntity>.Filter.Eq(fieldName, RegexName);
        var datas = collection.Find(filter).ToList();

        return datas;
    }

    public List<InputEntity> ReturnDataFromDBByCategory(string RegexName)
    {
        var data = GetVulnerabilityDataFromMongoDB(RegexName);
        var inputEntityData = data.Select(v => new InputEntity
        {
            Label = v.IsSuppresed,
            Secret = v.Secret
        }).ToList();

        return inputEntityData;
    }

    public List<InputEntity> ReturnTrainerDataFromCSV(string filePath)
    {
        try
        {
            var list = new List<InputEntity>();
            using var reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var result = line.Split("\t");
                list.Add(new InputEntity
                {
                    Label = bool.Parse(result[0]),
                    Secret = result[1]
                });
            }

            return list;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
            return null;
        }
    }

    public List<string> ReturnPredictDataFromCSV(string filePath)
    {
        try
        {
            var list = new List<string>();
            using var reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                list.Add(line);
            }

            return list;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
            return null;
        }
    }
}