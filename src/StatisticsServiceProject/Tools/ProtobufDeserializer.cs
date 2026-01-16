using Confluent.Kafka;
using Google.Protobuf;

namespace StatisticsServiceProject.Tools;

public class ProtobufDeserializer<T> : IDeserializer<T> where T : IMessage<T>
{
    private readonly MessageParser<T> _parser;

    public ProtobufDeserializer(MessageParser<T> parser)
    {
        _parser = parser;
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return _parser.ParseFrom(data);
    }
}