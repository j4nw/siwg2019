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

        public World(int width, int height)
        {
            Width = width;
            Height = height;
            world = new WorldTile[width, height];
        }

        public void DrawRectangle(int x, int y, int width, int height, WorldLayer layer, byte value)
        {
            for (var dx = 0; dx < width; dx++)
            {
                for (var dy = 0; dy < height; dy++)
                {
                    switch (layer)
                    {
                        case WorldLayer.Land:
                            world[x + dx, y + dy].height = value;
                            break;
                        case WorldLayer.Water:
                            world[x + dx, y + dy].isWater = Convert.ToBoolean(value);
                            break;
                        case WorldLayer.Wall:
                            world[x + dx, y + dy].isWall = Convert.ToBoolean(value);
                            break;
                    }
                }
            }
        }

        public void GeneratePerlinTerrain(float waterLevel = 0.4f, float perlinScale = 50f)
        {
            var heightMap = new NoiseMap(Width, Height);
            var builder = new NoiseMapBuilderPlane();
            builder.NoiseMap = heightMap;
            builder.SourceModule = new ImprovedPerlin();
            builder.SetBounds(0, Width / perlinScale, 0, Height / perlinScale);
            builder.SetSize(Width, Height);
            builder.Build();
            heightMap.MinMax(out var minHeight, out var maxHeight);

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
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
