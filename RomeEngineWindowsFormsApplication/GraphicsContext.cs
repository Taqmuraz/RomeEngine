using System.Collections.Generic;
using System.Drawing;
using RomeEngine;

namespace RomeEngineWindowsFormsApplication
{
    class GraphicsContext : IGraphicsContext
    {
        Dictionary<IMeshIdentifier, IMesh> meshes = new Dictionary<IMeshIdentifier, IMesh>();
        Dictionary<IMesh, IMeshIdentifier> identifiers = new Dictionary<IMesh, IMeshIdentifier>();

        public IMesh GetMesh(IMeshIdentifier identifier) => meshes[identifier];

        public Texture LoadTexture(string fileName)
        {
            return new BitmapTexture(new Bitmap(fileName));
        }

        public IMeshIdentifier LoadMesh(IMesh mesh)
        {
            if (identifiers.TryGetValue(mesh, out IMeshIdentifier result)) return result;
            else
            {
                var identifier = new MeshIdentifier();
                meshes.Add(identifier, mesh);
                identifiers.Add(mesh, identifier);
                return identifier;
            }
        }
    }
}
