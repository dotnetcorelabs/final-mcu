using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MarvelCharacters.Api.Services.Db
{
    public class IdBsonSerializer : IBsonSerializer
    {
        public static IdBsonSerializer Instance { get; } = new IdBsonSerializer();

        public Type ValueType => typeof(int);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var type = context.Reader.GetCurrentBsonType();
            switch (type)
            {
                case BsonType.ObjectId:
                    return context.Reader.ReadObjectId().ToString();
                case BsonType.String:
                    return context.Reader.ReadString();
                case BsonType.Int32:
                    return context.Reader.ReadString();
                default:
                    throw new NotSupportedException($"Cannot convert a {type} to an Int32.");
            }
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            context.Writer.WriteObjectId(ObjectId.Parse(value.ToString()));
        }
    }
}
