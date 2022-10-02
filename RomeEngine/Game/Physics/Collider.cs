using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public abstract class Collider : Component, ILocatable
    {
        static IPhysicalBody DefaultBody { get; } = new StaticBody(1f, 1f);
        static List<Collider> colliders = new List<Collider>();

        [BehaviourEvent]
        void Start()
        {
            lock (colliders)
            {
                colliders.Add(this);
            }
        }
        [BehaviourEvent]
        void OnDestroy()
        {
            lock (colliders)
            {

                colliders.Remove(this);
            }
        }

        static Dictionary<ColliderShapeType, Action<IPhysicalBody, IPhysicalBody, IColliderShape, IColliderShape>> functions = new Dictionary<ColliderShapeType, Action<IPhysicalBody, IPhysicalBody, IColliderShape, IColliderShape>>()
        {
            [ColliderShapeType.SphereVsSphere] = (p0, p1, s0, s1) => SphereVsShpere(p0, p1, (SphereShape)s0, (SphereShape)s1),
        };

        protected abstract IColliderShape Shape { get; }
        public IPhysicalBody PhysicalBody { get; set; }

        protected abstract void UpdateShape();

        public static void UpdatePhysics()
        {
            lock (colliders)
            {
                var shapes = colliders.Select(c => c.Shape);
                var bounds = Bounds.FromBoxes(shapes.Select(s => s.Bounds));
                var shapesTree = new Octotree<Collider>(bounds, 5, 6);
                foreach (var collider in colliders)
                {
                    if (collider.PhysicalBody != null) collider.PhysicalBody.Update();
                    collider.UpdateShape();
                    shapesTree.AddLocatable(collider);
                }
                shapesTree.VisitTree(new CustomTreeAcceptor<Collider>(locatables =>
                {
                    var checkList = locatables.ToList();
                    foreach (var first in locatables)
                    {
                        foreach (var second in checkList)
                        {
                            if (first == second || (first.PhysicalBody == null && second.PhysicalBody == null)) continue;

                            var contactType = (ColliderShapeType)(((int)first.Shape.ShapeType << 16) & (int)second.Shape.ShapeType);
                            if (functions.TryGetValue(contactType, out var func))
                            {
                                func(first.PhysicalBody ?? DefaultBody, second.PhysicalBody ?? DefaultBody, first.Shape, second.Shape);
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        checkList.RemoveAt(0);
                    }
                }, b => true));
            }
        }
        static void SphereVsShpere(IPhysicalBody firstBody, IPhysicalBody secondBody, SphereShape firstSphere, SphereShape secondShape)
        {
            Vector3 delta = secondShape.Center - firstSphere.Center;
            float distance = delta.length;
            if (distance <= (firstSphere.Radius + secondShape.Radius))
            {
                Vector3 contactPoint = (secondShape.Center + firstSphere.Center) * 0.5f;
                Vector3 contactLine = delta.normalized;
                float proj0 = Vector3.Dot(contactLine, firstBody.GetVelocityAtPoint(contactPoint));
                float proj1 = Vector3.Dot(contactLine, secondBody.GetVelocityAtPoint(contactPoint));
                float effect0 = (firstBody.Mass - secondBody.Mass * secondBody.RestitutionCoefficient) * proj0 + (1f + secondBody.RestitutionCoefficient) * secondBody.Mass * proj1;
                float effect1 = (secondBody.Mass - firstBody.Mass * firstBody.RestitutionCoefficient) * proj1 + (1f + firstBody.RestitutionCoefficient) * firstBody.Mass * proj0;
                firstBody.ApplyForceAtPoint(contactPoint, contactLine * effect0);
                secondBody.ApplyForceAtPoint(contactPoint, contactLine * effect1);
            }
        }
        static void SphereVsMesh(IPhysicalBody sphereBody, IPhysicalBody meshBody, SphereShape sphereShape, MeshShape meshShape)
        {

        }

        bool ILocatable.IsInsideBox(Bounds box)
        {
            return Shape.Bounds.IntersectsWith(box);
        }
    }
}