using Core;
using System;

namespace WorldGeneration
{
    public class World : IBoard<WorldLayer>
    {
        public int Width { get; set; }

        public int Height { get; set; }

        private WorldTile[,] world;

        public byte Layer(WorldLayer layer, int x, int y)
        {
            switch (layer)
            {
                case WorldLayer.Land:
                    return world[x, y].height;
                case WorldLayer.Water:
                    return Convert.ToByte(world[x, y].isWater);
                case WorldLayer.Wall:
                    return Convert.ToByte(world[x, y].isWall);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
