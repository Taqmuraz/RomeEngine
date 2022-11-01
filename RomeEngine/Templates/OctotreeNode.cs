﻿using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class OctotreeNode<TLocatable>
    where TLocatable : ILocatable
    {
        Bounds bounds;
        int maxLocatables;
        int maxDepth;
        int depth;
        bool collapsed;
        OctotreeNode<TLocatable>[] children;
        List<TLocatable> locatables = new List<TLocatable>();

        public OctotreeNode(Bounds bounds, int depth, int maxLocatables, int maxDepth)
        {
            this.bounds = bounds;
            this.maxLocatables = maxLocatables;
            this.maxDepth = maxDepth;
            this.depth = depth;
        }

        public void AddLocatable(TLocatable locatableToAdd)
        {
            if (collapsed)
            {
                foreach (var child in children)
                {
                    if (locatableToAdd.IsInsideBox(child.bounds))
                    {
                        child.AddLocatable(locatableToAdd);
                    }
                }
            }
            else
            {
                locatables.Add(locatableToAdd);
                if (locatables.Count >= maxLocatables && depth < maxDepth)
                {
                    collapsed = true;
                    children = new OctotreeNode<TLocatable>[8];

                    for (int i = 0; i < 8; i++)
                    {
                        var child = children[i] = CreateChild(i);
                        foreach (var locatable in locatables)
                        {
                            if (locatable.IsInsideBox(child.bounds))
                            {
                                child.AddLocatable(locatable);
                            }
                        }
                    }

                    locatables.Clear();
                }
            }
        }

        OctotreeNode<TLocatable> CreateChild(int index)
        {
            int x = index & 1;
            int y = (index / 2) & 1;
            int z = index / 4;
            float part = 0.5f;
            Vector3 partSize = bounds.Size * part;
            Vector3 pos = new Vector3(partSize.x * x, partSize.y * y, partSize.z * z) + bounds.Min;
            return new OctotreeNode<TLocatable>(new Bounds(pos + partSize * 0.5f, partSize), depth + 1, maxLocatables, maxDepth);
        }


        public void VisitNode(ITreeAcceptor<TLocatable> acceptor)
        {
            if (collapsed)
            {
                foreach (var child in children)
                {
                    if (acceptor.IsInsideBox(child.bounds))
                    {
                        child.VisitNode(acceptor);
                    }
                }
            }
            else if (locatables.Count != 0)
            {
                acceptor.AcceptLocatables(locatables);
            }
        }
    }
}