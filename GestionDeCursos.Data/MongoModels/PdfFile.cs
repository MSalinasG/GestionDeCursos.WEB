using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GestionDeCursos.Data.MongoModels
{
    public class PdfFile
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string? FileName { get; set; }
        public byte[]? PdfFileData { get; set; }
    }
}
