using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public abstract class Collider : Component, ILocatable
    {
        static IPhysicalBody DefaultBody { get; } = new StaticBody(1f);
        static List<Collider> colliders = new List<Collider>();

        static DateTime lastUpdate;
        public static float PhysicsDeltaTime { get; private set; }

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
            [ColliderShapeType.SphereVsMesh] = (p0, p1, s0, s1) => SphereVsMesh(p0, p1, (SphereShape)s0, (MeshShape)s1),
            [ColliderShapeType.MeshVsSphere] = (p0, p1, s0, s1) => SphereVsMesh(p1, p0, (SphereShape)s1, (MeshShape)s0),
        };

        protected abstract IColliderShape Shape { get; }
        public IPhysicalBody PhysicalBody { get; set; }

        protected abstract void UpdateShape();

        public static void UpdatePhysics()
        {
            var now = DateTime.Now;
            PhysicsDeltaTime = (float)(now - lastUpdate).TotalSeconds;
            lastUpdate = now;

            lock (colliders)
            {
                var shapes = colliders.Select(c => c.Shape);
                var bounds = Bounds.FromBoxes(shapes.Select(s => s.Bounds));
                var shapesTree = new Octotree<Collider>(bounds, 5, 6);
                foreach (var collider in colliders)
                {
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
                            if (first == second || (first.PhysicalBody == null && second.PhysicalBody == null) || first.Shape == null || second.Shape == null) continue;

                            var contactType = (ColliderShapeType)(((int)first.Shape.ShapeType << 16) | (int)second.Shape.ShapeType);
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

                foreach (var collider in colliders)
                {
                    if (collider.PhysicalBody != null) collider.PhysicalBody.FixedUpdate();
                }
            }
        }
        static void SphereVsShpere(IPhysicalBody firstBody, IPhysicalBody secondBody, SphereShape firstSphere, SphereShape secondShape)
        {
            Vector3 delta = secondShape.Center - firstSphere.Center;
            float distance = delta.Length;
            float doubleR = (firstSphere.Radius + secondShape.Radius);
            if (distance <= doubleR)
            {
                Vector3 contactPoint = (secondShape.Center + firstSphere.Center) * 0.5f;
                Vector3 contactNormal = delta.Normalized;

                float effect0 = Vector3.Dot(firstBody.GetVelocityAtPoint(contactPoint), contactNormal);
                float effect1 = Vector3.Dot(secondBody.GetVelocityAtPoint(contactPoint), -contactNormal);

                effect0 = effect0.Clamp(0f, effect0);
                effect1 = effect1.Clamp(effect1, 0f);

                firstBody.ApplyForceAtPoint(contactPoint, -contactNormal * effect0);
                secondBody.ApplyForceAtPoint(contactPoint, contactNormal * effect1);
            }
        }
        static void SphereVsMesh(IPhysicalBody sphereBody, IPhysicalBody meshBody, SphereShape sphereShape, MeshShape meshShape)
        {
            Bounds sphereBounds = sphereShape.Bounds;
            if (sphereBounds.IntersectsWith(meshShape.Bounds))
            {
                meshShape.CheckTriangles(sphereBounds, triangles =>
                {
                    foreach (var triangle in triangles)
                    {
                        Vector3 normal = triangle.Normal;
                        Vector3 sphereCenter = sphereShape.Center;
                        Vector3 triangleCenter = triangle.Center;
                        Vector3 pointOnTriangle = sphereShape.Center.ProjectOnPlane(triangleCenter, normal);
                        Vector3 delta = pointOnTriangle - sphereCenter;

                        if (delta.Length <= sphereShape.Radius)
                        {
                            Matrix3x3 triangleMatrixInv = new Matrix3x3();
                            Vector3 matrixRoot = triangleCenter + normal;
                            triangleMatrixInv.SetColumn(triangle.VertexA - matrixRoot, 0);
                            triangleMatrixInv.SetColumn(triangle.VertexB - matrixRoot, 1);
                            triangleMatrixInv.SetColumn(triangle.VertexC - matrixRoot, 2);
                            triangleMatrixInv = triangleMatrixInv.GetInversed();
                            Vector3 triangleSpace = triangleMatrixInv * (pointOnTriangle - matrixRoot);

                            if (triangleSpace.x >= 0f && triangleSpace.y >= 0f && triangleSpace.z >= 0f)
                            {
                                float dot = -Vector3.Dot(sphereBody.GetVelocityAtPoint(pointOnTriangle), normal);

                                if (Vector3.Dot(delta, normal).Sign() != dot.Sign())
                                {
                                    sphereBody.ApplyForceAtPoint(pointOnTriangle, normal * dot);
                                }
                            }
                        }
                    }
                });
            }
        }

        bool ILocatable.IsInsideBox(Bounds box)
        {
            return Shape.Bounds.IntersectsWith(box);
        }
    }
}