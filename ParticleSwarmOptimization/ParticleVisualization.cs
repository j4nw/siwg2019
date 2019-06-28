using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace ParticleSwarmOptimization
{
    internal class ParticleVisualization : IBoard<VisualizationLayer>
    {
        public int Width { get; }
        public int Height { get; }

        private byte[,] world;
        private byte[,] particles;
        private byte[,] bestPosition;

        public ParticleVisualization(float[,] world)
        {
            Width = world.GetLength(0);
            Height = world.GetLength(1);

            this.world = new byte[Width, Height];
            this.particles = new byte[Width, Height];
            this.bestPosition = new byte[Width, Height];

            var flatWorld = world.Cast<float>().ToArray();
            var min = flatWorld.Min();
            var max = flatWorld.Max();

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var landHeight = world[x, y];
                    var normalizedLandHeight = (landHeight - min) / (max - min);
                    var byteClampedHeight = 1 + (byte.MaxValue - 1) * normalizedLandHeight;
                    this.world[x, y] = Convert.ToByte(byteClampedHeight);
                }
            }
        }

        public void SetDynamic(List<Particle> particles, (float x, float y) bestPosition)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    this.particles[x, y] = byte.MinValue;
                    this.bestPosition[x, y] = byte.MinValue;
                }
            }

            foreach (var particle in particles)
            {
                this.particles[(int)particle.position.x, (int)particle.position.y] = byte.MaxValue;
                this.particles[(int)particle.position.x + 1, (int)particle.position.y] = byte.MaxValue;
                this.particles[(int)particle.position.x - 1, (int)particle.position.y] = byte.MaxValue;
                this.particles[(int)particle.position.x, (int)particle.position.y + 1] = byte.MaxValue;
                this.particles[(int)particle.position.x, (int)particle.position.y - 1] = byte.MaxValue;
            }

            this.bestPosition[(int) bestPosition.x, (int) bestPosition.y] = byte.MaxValue;
            this.bestPosition[(int)bestPosition.x + 1, (int)bestPosition.y] = byte.MaxValue;
            this.bestPosition[(int)bestPosition.x - 1, (int)bestPosition.y] = byte.MaxValue;
            this.bestPosition[(int)bestPosition.x, (int)bestPosition.y + 1] = byte.MaxValue;
            this.bestPosition[(int)bestPosition.x, (int)bestPosition.y - 1] = byte.MaxValue;
            this.bestPosition[(int)bestPosition.x + 1, (int)bestPosition.y + 1] = byte.MaxValue;
            this.bestPosition[(int)bestPosition.x - 1, (int)bestPosition.y + 1] = byte.MaxValue;
            this.bestPosition[(int)bestPosition.x + 1, (int)bestPosition.y - 1] = byte.MaxValue;
            this.bestPosition[(int)bestPosition.x - 1, (int)bestPosition.y - 1] = byte.MaxValue;
        }

        public byte Layer(VisualizationLayer layer, int x, int y)
        {
            switch (layer)
            {
                case VisualizationLayer.BestPosition:
                    return bestPosition[x, y];
                case VisualizationLayer.Particles:
                    return particles[x, y];
                case VisualizationLayer.World:
                    return world[x, y];
                default:
                    return byte.MinValue;
            }
        }
    }
}