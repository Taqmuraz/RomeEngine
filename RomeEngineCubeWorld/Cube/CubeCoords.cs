using RomeEngine;

namespace RomeEngineCubeWorld
{
    public struct CubeCoords
    {
        public int x;
        public int y;
        public int z;

        public CubeCoords(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3 (CubeCoords coords) => new Vector3(coords.x, coords.y, coords.z);
        public static CubeCoords operator +(CubeCoords a, CubeCoords b) => new CubeCoords(a.x + b.x, a.y + b.y, a.z + b.z);
        public static CubeCoords operator -(CubeCoords a, CubeCoords b) => new CubeCoords(a.x - b.x, a.y - b.y, a.z - b.z);
    }
}
