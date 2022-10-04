using RomeEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public static class InstantiatableExtensions
	{
		static IEnumerable<IInstancer> Instancers { get; } = new IInstancer[]
		{
			new SourceObjectInstancer(),
			new ArrayListInstancer(),
			new SerializableInstancer(),
			new ArrayInstancer(),

			new EmptyInstancer()
		};

		public static IInstancer GetInstancerForSource(object source)
		{
			return Instancers.First(i => i.CanInstantiate(source));
		}

		public static ISerializable CreateSerializableInstance(this ISerializable source, Dictionary<ISerializable, ISerializable> objectsMap)
		{
			var instance = (ISerializable)source.GetType().GetConstructor(new Type[0]).Invoke(new object[0]);
			objectsMap.Add(source, instance);
			var fieldsMap = instance.EnumerateFields().ToDictionary(f => f.Name);
			foreach (var field in source.EnumerateFields())
			{
				var value = field.Value;
				fieldsMap[field.Name].Setter(GetInstancerForSource(value).Instantiate(value, objectsMap));
			}
			return instance;
		}
	}
}
