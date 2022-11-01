using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomeEngineCubeWorld
{
    public class Cube
    {
        int cubeId;
        const int AirCubeId = 0;

        public Cube()
        {
            cubeId = AirCubeId;
        }

        public Cube(int cubeId)
        {
            this.cubeId = cubeId;
        }
    }
}
