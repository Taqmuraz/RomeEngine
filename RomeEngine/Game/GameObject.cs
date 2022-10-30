using RomeEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class GameObject : CompositeGameEntity, IInstantiatable<GameObject>, IGameObject
    {
        [SerializeField]
        public Transform Transform { get; private set; }
        ITransform IGameObject.Transform => Transform;
        [SerializeField]
        public string Name { get; set; }

        bool IsActive { get; set; }
        bool IsOnActivationProcess { get; set; }

        [Obsolete("Zero argument GameObject constructor only might be used in serialization", true)]
        GameObject()
        {

        }

        void IGameObject.Activate(IGameObjectActivityProvider activityProvider)
        {
            if (!IsActive)
            {
                IsActive = true;
                IsOnActivationProcess = true;
                activityProvider.Activate(this);
                IsOnActivationProcess = false;
            }
        }
        void IGameObject.Deactivate(IGameObjectActivityProvider activityProvider)
        {
            if (IsActive)
            {
                activityProvider.Deactivate(this);
                IsActive = false;
            }
        }

        public GameObject ActivateForActiveScene()
        {
            ((IGameObject)this).Activate(GameScene.ActiveScene);
            return this;
        }

        public GameObject(string name)
        {
            Name = name;
            Transform = new Transform(name);
            AppendEntity(Transform);
        }

        public override string ToString()
        {
            return $"(GameObject){Name}";
        }

        public Component AddComponent(Type type)
        {
            var constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);
            if (constructor == null) throw new System.InvalidOperationException($"Component of type {type.FullName} does not have zero-argument constructor");
            var component = (Component)constructor.Invoke(new object[0]);

            if (component.IsUnary && GetComponent(type) != null) throw new System.ArgumentException("GameObject cant have two unary Components of the same type");

            return AddComponent(component);
        }
        public TComponent AddComponent<TComponent>(TComponent component) where TComponent : Component, IInitializable<IGameObject>
        {
            component.Initialize(this);
            return component;
        }

        void IGameEntityActivityProvider.Activate(IGameEntity entity)
        {
            AppendEntity(entity);

            if (IsActive && !IsOnActivationProcess)
            {
                entity.CallEvent("Start");
            }
        }
        void IGameEntityActivityProvider.Deactivate(IGameEntity entity)
        {
            RemoveEntity(entity);
            entity.CallEvent("OnDestroy");
        }

        public TComponent AddComponent<TComponent>() where TComponent : Component, IInitializable<IGameObject>, new()
        {
            return (TComponent)AddComponent(typeof(TComponent));
        }

        public void RemoveComponent(Component component)
        {
            RemoveEntity(component);
            component.CallEvent("OnDestroy");
        }

        public TComponent GetComponent<TComponent>()
        {
            foreach (var component in InnerEntities)
            {
                if (component is TComponent t) return t;
            }
            return default;
        }
        public Component GetComponent(Type type)
        {
            foreach (var component in InnerEntities)
            {
                if (component.GetType() == type) return (Component)component;
            }
            return default;
        }
        public Component[] GetComponents()
        {
            return InnerEntities.OfType<Component>().ToArray();
        }

        public TComponent[] GetComponentsOfType<TComponent>()
        {
            return InnerEntities.Where(c => c is TComponent).Select(c => (TComponent)(object)c).ToArray();
        }

        GameObject IInstantiatable<GameObject>.CreateInstance()
        {
            Dictionary<ISerializable, ISerializable> objectsMap = new Dictionary<ISerializable, ISerializable>();
            var instance = (GameObject)Activator.CreateInstance(GetType(), true);
            objectsMap.Add(this, instance);
            var entitiesMap = InnerEntities.ToDictionary(c => c, c => (IGameEntity)c.CreateSerializableInstance(objectsMap));
            instance.AppendEntities(entitiesMap.Values);
            instance.Transform = (Transform)entitiesMap[Transform];
            instance.Name = Name;
            ActivateForActiveScene();
            return instance;
        }

        public static GameObject Instantiate(GameObject source, IGameObjectActivityProvider activityProvider)
        {
            var instance = ((IInstantiatable<GameObject>)source).CreateInstance();
            ((IGameObject)instance).Activate(activityProvider);
            return instance;
        }
    }
}
