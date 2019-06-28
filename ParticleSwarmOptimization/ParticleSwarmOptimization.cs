using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Core;

namespace ParticleSwarmOptimization
{
    internal class Particle
    {
        public (float x, float y) velocity;
        public (float x, float y) position;
        public (float x, float y) bestPosition;
    }

    public class ParticleSwarmOptimization : ProblemVisualization
    {
        private Random random = new Random();
        private float[,] world;
        private List<Particle> particles;
        private (float x, float y) globalBestPosition;
        private float c1;
        private float c2;
        private ParticleVisualization visualization;
        private int width;
        private int height;

        Dictionary<VisualizationLayer, (Color low, Color high)> colorsByLayer = new Dictionary<VisualizationLayer, (Color low, Color high)>
        {
            {VisualizationLayer.BestPosition, (Color.Red, Color.Red)},
            {VisualizationLayer.Particles, (Color.White, Color.White)},
            {VisualizationLayer.World, (Color.Black, Color.Gray)}
        };

        List<VisualizationLayer> orderedLayers = new List<VisualizationLayer>
        {
            VisualizationLayer.Particles,
            VisualizationLayer.BestPosition,
            VisualizationLayer.World
        };

        public ParticleSwarmOptimization()
        {
            Name = "Particle Swarm Optimization";
            Settings.Add("World Width", "500");
            Settings.Add("World Height", "500");
            Settings.Add("Perlin Cell Size", "100");
            Settings.Add("Particles", "20");
            Settings.Add("c1", "0.01");
            Settings.Add("c2", "0.01");
        }

        public void Initialize()
        {
            width = Settings.GetIntValue("World Width");
            height = Settings.GetIntValue("World Height");
            var perlinCellSize = Settings.GetFloatValue("Perlin Cell Size");
            var particleAmount = Settings.GetIntValue("Particles");
            c1 = Settings.GetFloatValue("c1");
            c2 = Settings.GetFloatValue("c2");

            var noise = new PerlinNoise.PerlinNoise();
            noise.DrawGradients(width, height, perlinCellSize);
            noise.CreateNoise(width, height, perlinCellSize);
            world = noise.NoiseTable;

            particles = new List<Particle>();
            for (var i = 0; i < particleAmount; i++)
            {
                var position = ((float) random.Next(1, width - 1), (float) random.Next(1, height - 1));
                var randomVelocity = RandomUnitVector();
                particles.Add(new Particle
                {
                    velocity = randomVelocity,
                    position = position,
                    bestPosition = position
                });

                globalBestPosition = particles.First().position;
                foreach (var particle in particles)
                {
                    if (Cost(particle.position) < Cost(globalBestPosition))
                    {
                        globalBestPosition = particle.position;
                    }
                }
            }

            visualization = new ParticleVisualization(world);
            visualization.SetDynamic(particles, globalBestPosition);
        }

        private float Cost((float x, float y) position)
        {
            return world[(int) position.x, (int) position.y];
        }

        public void Step()
        {
            foreach (var particle in particles)
            {
                var dx = c1 * RandomFloat() * (particle.bestPosition.x - particle.position.x) +
                         c2 * RandomFloat() * (globalBestPosition.x - particle.position.x);
                var dy = c1 * RandomFloat() * (particle.bestPosition.y - particle.position.y) +
                         c2 * RandomFloat() * (globalBestPosition.y - particle.position.y);
                particle.velocity = (particle.velocity.x + dx, particle.velocity.y + dy);
                particle.position = (
                    particle.position.x + particle.velocity.x,
                    particle.position.y + particle.velocity.y);

                // clamp
                var x = particle.position.x;
                var y = particle.position.y;
                if (x < 1)
                    x = 1;
                if (x > width - 2)
                    x = width - 2;
                if (y < 1)
                    y = 1;
                if (y > height - 2)
                    y = height - 2;
                particle.position = (x, y);

                if (Cost(particle.position) < Cost(particle.bestPosition))
                {
                    particle.bestPosition = particle.position;
                    if (Cost(particle.position) < Cost(globalBestPosition))
                    {
                        globalBestPosition = particle.position;
                    }
                }
            }
        }

        public override Bitmap Visualization
        {
            get
            {
                if (world == null)
                {
                    Initialize();
                }

                var image = Visualizer<VisualizationLayer>.BoardToImage(visualization, colorsByLayer, orderedLayers);

                Step();
                visualization.SetDynamic(particles, globalBestPosition);

                return new Bitmap(image);
            }
        }

        private (float x, float y) RandomUnitVector()
        {
            var angle = random.NextDouble() * 2 * Math.PI;
            var x = (float) Math.Cos(angle);
            var y = (float) Math.Sin(angle);
            return (x, y);
        }

        private float RandomFloat()
        {
            return (float) random.NextDouble();
        }
    }
}
