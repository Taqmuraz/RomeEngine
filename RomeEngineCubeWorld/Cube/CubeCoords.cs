using RomeEngine;
using System;

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

        public override string ToString()
        {
            return $"{x}, {y}, {z}";
        }

        public static implicit operator Vector3 (CubeCoords coords) => new Vector3(coords.x, coords.y, coords.z);
        public static explicit operator CubeCoords (Vector3 vector) => new CubeCoords((int)vector.x, (int)vector.y, (int)vector.z);

        public static CubeCoords operator +(CubeCoords a, CubeCoords b) => new CubeCoords(a.x + b.x, a.y + b.y, a.z + b.z);
        public static CubeCoords operator -(CubeCoords a, CubeCoords b) => new CubeCoords(a.x - b.x, a.y - b.y, a.z - b.z);

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: return 0;
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                }
            }
        }

        public static CubeCoords CubeDirection(Vector3 normal)
        {
            CubeCoords max = new CubeCoords();
            float maxDot = -1f;

            for (int i = 0; i < 6; i++)
            {
                CubeCoords dir = new CubeCoords() { [i % 3] = (((i / 3) & 1) == 0 ? 1 : -1) };
                float dot = Vector3.Dot(dir, normal);
                if (dot > maxDot)
                {
                    max = dir;
                    maxDot = dot;
                }
            }

            return max;
        }
    }
}
