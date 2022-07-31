using OneEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneEngine
{
	public abstract class BehaviourEventsHandler : ISerializable
	{
		SafeDictionary<string, List<Action>> actions;

		public BehaviourEventsHandler()
		{
			actions = new SafeDictionary<string, List<Action>>();
			List<System.Reflection.MethodInfo> methods = new List<System.Reflection.MethodInfo>();

			var type = GetType();

			do
			{
				methods.AddRange(type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic));
				type = type.BaseType;
			} while (type != typeof(object));


			var zeroParam = new object[0];
			for (int i = 0; i < methods.Count; i++)
			{
				var method = methods[i];
				if (Attribute.IsDefined(method, typeof(BehaviourEventAttribute)) && method.GetParameters().Length == 0)
				{
					Action action = () => method.Invoke(this, zeroParam);
					if (actions.ContainsKey(method.Name))
					{
						actions[method.Name].Add(action);
					}
					else
					{
						actions.Add(method.Name, new List<Action> { action });
					}
				}
			}
		}

		public bool ContainsAction(string name)
		{
			return actions.ContainsKey(name);
		}

		public void CallEvent(string name)
		{
			OnEventCall(name);

			if (actions.ContainsKey(name))
			{
				var actionsList = actions[name];
				for (int i = actionsList.Count - 1; i >= 0; i--) actionsList[i].Invoke();
			}
		}

		protected virtual void OnEventCall(string name)
		{

		}

		public IEnumerable<SerializableField> EnumerateFields()
		{
			var type = GetType();
			var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;

			var properties = type.GetProperties(flags);
			foreach (var property in properties)
			{
                var attribute = Attribute.GetCustomAttribute(property, typeof(SerializeFieldAttribute)) as SerializeFieldAttribute;

                if (property.GetMethod != null && property.SetMethod != null && attribute != null)
				{
					yield return new SerializableField(property.Name, property.GetValue(this), value => property.SetValue(this, value), property.PropertyType, attribute.HideInInspector);
				}
			}

			do
			{
				var fields = type.GetFields(flags);
				foreach (var field in fields)
				{
                    var attribute = Attribute.GetCustomAttribute(field, typeof(SerializeFieldAttribute)) as SerializeFieldAttribute;

                    if (attribute != null)
					{
						yield return new SerializableField(field.Name, field.GetValue(this), value => field.SetValue(this, value), field.FieldType, attribute.HideInInspector);
					}
				}
				type = type.BaseType;
			} while (type != typeof(object));
		}
    }
}
