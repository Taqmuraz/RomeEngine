using System;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ArithmeticTypeFieldSerializer : IFieldSerializer
    {
        (Type type, Action<ISerializationContext, object> writeAction, Func<ISerializationContext, object> readAction)[] actions = new(Type, Action<ISerializationContext, object>, Func<ISerializationContext, object>)[]
        {
            (typeof(int), (context, value) => context.Stream.WriteInt((int)value), context => context.Stream.ReadInt()),
            (typeof(float), (context, value) => context.Stream.WriteFloat((float)value), context => context.Stream.ReadFloat()),
            (typeof(short), (context, value) => context.Stream.WriteShort((short)value), context => context.Stream.ReadShort()),
            (typeof(byte), (context, value) => context.Stream.WriteByte((byte)value), context => context.Stream.ReadByte()),
            (typeof(sbyte), (context, value) => context.Stream.WriteByte((byte)(sbyte)value), context => (sbyte)context.Stream.ReadByte()),
        };

        public bool CanSerializeType(Type type)
        {
            return actions.Any(a => a.type == type);
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            var type = value.GetType();
            actions.First(a => a.type == type).writeAction(context, value);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            return actions.First(a => a.type == type).readAction(context);
        }
    }
}
