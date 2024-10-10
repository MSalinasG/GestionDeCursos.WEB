using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GestionDeCursos.Data.MongoModels
{
    public class ExcelFile
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string? FileName {  get; set; }
        public byte[]? ExcelFileData { get; set; }
    }
}
