﻿using RomeEngine;
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
            BuildChunksTree();
            hasChanges = true;
        }

        void BuildChunksTree()
        {
            Bounds bounds = Bounds.FromBoxes(chunks.Select(c => c.Bounds));
            chunksTree = new Octotree<ICubeChunk>(bounds, 2, 4);
            foreach (var chunk in chunks) chunksTree.AddLocatable(chunk);
        }
        void RebuildWorld()
        {
            foreach (var chunk in chunks)
            {
                chunk.RebuildMesh();
            }
            BuildChunksTree();
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
    }
}
