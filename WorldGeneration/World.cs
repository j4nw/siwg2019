using Core;
using System;
using LibNoise.Builder;
using LibNoise.Primitive;

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

        public World(int width, int height, float waterLevel = 0.4f, float perlinScale = 50f)
        {
            Width = width;
            Height = height;
            world = new WorldTile[width, height];

            var heightMap = new NoiseMap(width, height);
            var builder = new NoiseMapBuilderPlane();
            builder.NoiseMap = heightMap;
            builder.SourceModule = new ImprovedPerlin();
            builder.SetBounds(0, width / perlinScale, 0, height / perlinScale);
            builder.SetSize(width, height);
            builder.Build();
            heightMap.MinMax(out var minHeight, out var maxHeight);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var landHeight = heightMap.GetValue(x, y);
                    var normalizedLandHeight = (landHeight - minHeight) / (maxHeight - minHeight);
                    var byteClampedHeight = 1 + (byte.MaxValue - 1) * normalizedLandHeight;
                    world[x, y].height = Convert.ToByte(byteClampedHeight);

                    if (normalizedLandHeight < waterLevel)
                    {
                        world[x, y].isWater = true;
                    }
                }
            }
        }
    }
}
