using RomeEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngineCubeWorld
{
    public sealed class CubeWorld : SerializableEventsHandler, IGameEntity, ICubeWorld
    {
        public static ICubeWorld Instance { get; private set; }

        Octotree<ICubeChunk> chunksTree;
        List<ICubeChunk> chunks;
        bool hasChanges;
        
        public CubeWorld(IEnumerable<ICubeChunk> chunks)
        {
            if (Instance != null) throw new System.InvalidOperationException("Cube world can have only one instance");
            Instance = this;

            this.chunks = new List<ICubeChunk>(chunks);
            Bounds bounds = Bounds.FromBoxes(chunks.Select(c => c.Bounds));
            chunksTree = new Octotree<ICubeChunk>(bounds, 4, 6);
            foreach (var chunk in chunks)
            {
                chunk.Initialize(this);
                chunksTree.AddLocatable(chunk);
            }
            hasChanges = true;
        }

        void RebuildWorld()
        {
            foreach (var chunk in chunks)
            {
                chunk.Rebuild();
            }
        }

        [BehaviourEvent]
        void OnPreRender()
        {
            if (hasChanges)
            {
                RebuildWorld();
                hasChanges = false;
            }
        }

        public void ModifyCube(ICubeModifier modifier, CubeCoords coords)
        {
            chunksTree.VisitTree(new CustomTreeAcceptor<ICubeChunk>(chunks =>
            {
                foreach (var chunk in chunks)
                {
                    chunk.ModifyCube(modifier, coords - chunk.Position);
                }
            }, box => box.ContainsPoint(coords)));

            hasChanges = true;
        }

        string IGameEntity.Name => "Cube world";

        void IGameEntity.Activate(IGameEntityActivityProvider activityProvider) => activityProvider.Activate(this);
        void IGameEntity.Deactivate(IGameEntityActivityProvider activityProvider) => activityProvider.Activate(this);

        const int BufferSize = 8192;
        static IBuffer<ICube> nonAllocRaycastBuffer = new Buffer<ICube>(BufferSize);

        public bool RaycastCube(Ray ray, out ICube cube)
        {
            nonAllocRaycastBuffer.Reset();

            RaycastCubesNonAlloc(ray, nonAllocRaycastBuffer);

            if (nonAllocRaycastBuffer.Position != 0)
            {
                cube = nonAllocRaycastBuffer.Enumerate().FindMin(c => (c.Position - ray.origin).length);
                return true;
            }
            else
            {
                cube = null;
                return false;
            }
        }
        public void RaycastCubesNonAlloc(Ray ray, IBuffer<ICube> buffer)
        {
            chunksTree.VisitTree(new CustomTreeAcceptor<ICubeChunk>(chunks =>
            {
                foreach (var chunk in chunks)
                {
                    chunk.RaycastCubesNonAlloc(ray, buffer);
                }
            }, box => box.IntersectsRay(ray)));
        }

        public IAsyncProcessHandle RaycastCubeAsync(Ray ray, Action<ICube> callback)
        {
            var process = new AsyncProcess<ICube>(() =>
            {
                if (RaycastCube(ray, out ICube cube))
                {
                    return cube;
                }
                return null;
            }, cube =>
            {
                if (cube != null) callback(cube);
            });

            return process.Start();
        }

        public IAsyncProcessHandle RaycastCubesAsunc(Ray ray, Action<IEnumerable<ICube>> callback)
        {
            var process = new AsyncProcess<IEnumerable<ICube>>(() =>
            {
                var buffer = new Buffer<ICube>(BufferSize);
                RaycastCubesNonAlloc(ray, buffer);
                return buffer.Enumerate();
            }, callback);

            return process.Start();
        }

        public bool TryGetCube(CubeCoords coords, out ICube cube)
        {
            ICube result = null;

            chunksTree.VisitTree(new CustomTreeAcceptor<ICubeChunk>(chunks =>
            {
                if (result != null) return;

                foreach (var chunk in chunks)
                {
                    if (chunk.TryGetCube(coords - chunk.Position, out ICube found))
                    {
                        result = found;
                        break;
                    }
                }
            }, box => box.ContainsPoint(coords)));

            cube = result;
            return result != null;
        }
    }
}
