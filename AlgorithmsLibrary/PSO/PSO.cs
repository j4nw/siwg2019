using System;
using System.Drawing;
using Core;
using PerlinNoise;

namespace AlgorithmsLibrary
{
    public class PSO : ProblemVisualization
    {
        private static readonly Random random = new Random();

        public PSO()
        {
            // name visible on list
            Name = "PSO A";

            // default settings as strings
            Settings.Add("Width", "500");
            Settings.Add("Height", "500");
            Settings.Add("Swarm Size", "40");
            Settings.Add("c1", "0,5");
            Settings.Add("c2", "0,5");
            Settings.Add("Iter. Without Change", "100");
            Settings.Add("Grid Cell Size", "100");
            Settings.Add("Heighest/lowest (1/0)", "1");
        }

        private bool first = true;

        private int width;
        private int height;
        private int swarmSize;
        private double c1;
        private double c2;
        private int iters;

        private int[] globalBest;
        private int[][] particles;
        private int[][] particlesBests;
        private double[][] velocities;

        private int iteration;
        private bool changed;

        private PerlinNoise.PerlinNoise map;

        //visualization
        public override Bitmap Visualization // get image for visualization
        {
            get
            {
                if (Settings.GetIntValue("Heighest/lowest (1/0)") == 1)
                    return FindHighest();
                else
                    return FindLowest();
            }
        }

        private Bitmap FindHighest()
        {
            if (first)
            {
                width = Settings.GetIntValue("Width");
                height = Settings.GetIntValue("Height");
                swarmSize = Settings.GetIntValue("Swarm Size");
                c1 = Settings.GetDoubleValue("c1");
                c2 = Settings.GetDoubleValue("c2");
                iters = Settings.GetIntValue("Iter. Without Change");

                map = new PerlinNoise.PerlinNoise();
                map.DrawGradients(width, height, Settings.GetFloatValue("Grid Cell Size"));
                map.CreateNoise(width, height, Settings.GetFloatValue("Grid Cell Size"));


                globalBest = new int[2];
                particles = new int[swarmSize][];
                particlesBests = new int[swarmSize][];
                velocities = new double[swarmSize][];

                for (int i = 0; i < swarmSize; i++)
                {
                    particles[i] = new int[2];
                    particlesBests[i] = new int[2];
                    velocities[i] = new double[2];
                }

                // for each particle i = 1, ..., S do
                for (int i = 0; i < swarmSize; i++)
                {
                    // Initialize the particle's position with a uniformly distributed random vector: xi ~ U(blo, bup)
                    // Initialize the particle's best known position to its initial position: pi ← xi
                    // Initialize the particle's velocity: vi ~ U(-|bup-blo|, |bup-blo|)
                    particles[i][0] = random.Next(0, width);
                    particlesBests[i][0] = particles[i][0];
                    velocities[i][0] = RandomNumberBetween(-(double)width / 5, (double)width / 5);

                    particles[i][1] = random.Next(0, height);
                    particlesBests[i][1] = particles[i][1];
                    velocities[i][1] = RandomNumberBetween(-(double)height / 5, (double)height / 5);


                    // if f(pi) < f(g) then
                    //    update the swarm's best known  position: g ← pi
                    if (i == 0 || map.NoiseTable[particlesBests[i][0], particlesBests[i][1]] > map.NoiseTable[globalBest[0], globalBest[1]])
                    {
                        globalBest[0] = particlesBests[i][0];
                        globalBest[1] = particlesBests[i][1];
                    }
                }

                iteration = 0;
                changed = false;
            }

            if (!first && iteration < iters)
            {
                // for each particle i = 1, ..., S do
                for (int i = 0; i < swarmSize; i++)
                {
                    // Pick random numbers: rp, rg ~U(0, 1)
                    double r1 = RandomNumberBetween(0, 1);
                    double r2 = RandomNumberBetween(0, 1);

                    // Update the particle's velocity: vi,d ← ω vi,d + φp rp (pi,d-xi,d) + φg rg (gd-xi,d)
                    velocities[i][0] += c1 * r1 * (particlesBests[i][0] - particles[i][0]) + c2 * r2 * (globalBest[0] - particles[i][0]);

                    // Update the particle's position: xi ← xi + vi
                    particles[i][0] += (int)velocities[i][0];
                    if (particles[i][0] < 0)
                        particles[i][0] = 0;
                    else if (particles[i][0] >= width)
                        particles[i][0] = width - 1;


                    // Update the particle's velocity: vi,d ← ω vi,d + φp rp (pi,d-xi,d) + φg rg (gd-xi,d)
                    velocities[i][1] += c1 * r1 * (particlesBests[i][1] - particles[i][1]) + c2 * r2 * (globalBest[1] - particles[i][1]);

                    // Update the particle's position: xi ← xi + vi
                    particles[i][1] += (int)velocities[i][1];
                    if (particles[i][1] < 0)
                        particles[i][1] = 0;
                    else if (particles[i][1] >= height)
                        particles[i][1] = height - 1;


                    // if f(xi) < f(pi) then
                    //    Update the particle's best known position: pi ← xi
                    if (map.NoiseTable[particles[i][0], particles[i][1]] > map.NoiseTable[particlesBests[i][0], particlesBests[i][1]])
                    {
                        particlesBests[i][0] = particles[i][0];
                        particlesBests[i][1] = particles[i][1];

                        // if f(pi) < f(g) then
                        //    Update the swarm's best known position: g ← pi
                        if (map.NoiseTable[particles[i][0], particles[i][1]] > map.NoiseTable[globalBest[0], globalBest[1]])
                        {
                            globalBest[0] = particles[i][0];
                            globalBest[1] = particles[i][1];

                            changed = true;
                        }
                    }
                }

                // stop condition
                if (changed)
                    iteration = 0;
                else
                    iteration++;
            }

            first = false;
            changed = false;


            //image
            Bitmap image = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    image.SetPixel(i, j, Color.FromArgb(0, 0, (int)((map.NoiseTable[i, j] + 1) * 128)));

            for (int i = 0; i < swarmSize; i++)
                particleDraw(image, particles[i][0], particles[i][1], 255, 0, 0);

            if (iteration >= iters)
                particleDraw(image, globalBest[0], globalBest[1], 0, 255, 0);


            return image;
        }

        private Bitmap FindLowest()
        {
            if (first)
            {
                width = Settings.GetIntValue("Width");
                height = Settings.GetIntValue("Height");
                swarmSize = Settings.GetIntValue("Swarm Size");
                c1 = Settings.GetDoubleValue("c1");
                c2 = Settings.GetDoubleValue("c2");
                iters = Settings.GetIntValue("Iter. Without Change");

                map = new PerlinNoise.PerlinNoise();
                map.DrawGradients(width, height, Settings.GetFloatValue("Grid Cell Size"));
                map.CreateNoise(width, height, Settings.GetFloatValue("Grid Cell Size"));


                globalBest = new int[2];
                particles = new int[swarmSize][];
                particlesBests = new int[swarmSize][];
                velocities = new double[swarmSize][];

                for (int i = 0; i < swarmSize; i++)
                {
                    particles[i] = new int[2];
                    particlesBests[i] = new int[2];
                    velocities[i] = new double[2];
                }

                // for each particle i = 1, ..., S do
                for (int i = 0; i < swarmSize; i++)
                {
                    // Initialize the particle's position with a uniformly distributed random vector: xi ~ U(blo, bup)
                    // Initialize the particle's best known position to its initial position: pi ← xi
                    // Initialize the particle's velocity: vi ~ U(-|bup-blo|, |bup-blo|)
                    particles[i][0] = random.Next(0, width);
                    particlesBests[i][0] = particles[i][0];
                    velocities[i][0] = RandomNumberBetween(-(double)width / 5, (double)width / 5);

                    particles[i][1] = random.Next(0, height);
                    particlesBests[i][1] = particles[i][1];
                    velocities[i][1] = RandomNumberBetween(-(double)height / 5, (double)height / 5);


                    // if f(pi) < f(g) then
                    //    update the swarm's best known  position: g ← pi
                    if (i == 0 || map.NoiseTable[particlesBests[i][0], particlesBests[i][1]] < map.NoiseTable[globalBest[0], globalBest[1]])
                    {
                        globalBest[0] = particlesBests[i][0];
                        globalBest[1] = particlesBests[i][1];
                    }
                }

                iteration = 0;
                changed = false;
            }

            if (!first && iteration < iters)
            {
                // for each particle i = 1, ..., S do
                for (int i = 0; i < swarmSize; i++)
                {
                    // Pick random numbers: rp, rg ~U(0, 1)
                    double r1 = RandomNumberBetween(0, 1);
                    double r2 = RandomNumberBetween(0, 1);

                    // Update the particle's velocity: vi,d ← ω vi,d + φp rp (pi,d-xi,d) + φg rg (gd-xi,d)
                    velocities[i][0] += c1 * r1 * (particlesBests[i][0] - particles[i][0]) + c2 * r2 * (globalBest[0] - particles[i][0]);

                    // Update the particle's position: xi ← xi + vi
                    particles[i][0] += (int)velocities[i][0];
                    if (particles[i][0] < 0)
                        particles[i][0] = 0;
                    else if (particles[i][0] >= width)
                        particles[i][0] = width - 1;


                    // Update the particle's velocity: vi,d ← ω vi,d + φp rp (pi,d-xi,d) + φg rg (gd-xi,d)
                    velocities[i][1] += c1 * r1 * (particlesBests[i][1] - particles[i][1]) + c2 * r2 * (globalBest[1] - particles[i][1]);

                    // Update the particle's position: xi ← xi + vi
                    particles[i][1] += (int)velocities[i][1];
                    if (particles[i][1] < 0)
                        particles[i][1] = 0;
                    else if (particles[i][1] >= height)
                        particles[i][1] = height - 1;



                    // if f(xi) < f(pi) then
                    //    Update the particle's best known position: pi ← xi
                    if (map.NoiseTable[particles[i][0], particles[i][1]] < map.NoiseTable[particlesBests[i][0], particlesBests[i][1]])
                    {
                        particlesBests[i][0] = particles[i][0];
                        particlesBests[i][1] = particles[i][1];

                        // if f(pi) < f(g) then
                        //    Update the swarm's best known position: g ← pi
                        if (map.NoiseTable[particles[i][0], particles[i][1]] < map.NoiseTable[globalBest[0], globalBest[1]])
                        {
                            globalBest[0] = particles[i][0];
                            globalBest[1] = particles[i][1];

                            changed = true;
                        }
                    }
                }

                // stop condition
                if (changed)
                    iteration = 0;
                else
                    iteration++;
            }

            first = false;
            changed = false;


            //image
            Bitmap image = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    image.SetPixel(i, j, Color.FromArgb(0, 0, (int)((map.NoiseTable[i, j] + 1) * 128)));

            for (int i = 0; i < swarmSize; i++)
                particleDraw(image, particles[i][0], particles[i][1], 255, 0, 0);

            if (iteration >= iters)
                particleDraw(image, globalBest[0], globalBest[1], 0, 255, 0);


            return image;
        }

        //draw particle as a square 3x3 pixels
        private void particleDraw(Bitmap image, int x, int y, int r = 255, int g = 255, int b = 255)
        {
            image.SetPixel(x, y, Color.FromArgb(r, g, b));

            if (y - 1 >= 0)
                image.SetPixel(x, y - 1, Color.FromArgb(r, g, b));

            if (y + 1 < height)
                image.SetPixel(x, y + 1, Color.FromArgb(r, g, b));

            if (x - 1 >= 0)
            {
                image.SetPixel(x - 1, y, Color.FromArgb(r, g, b));

                if (y - 1 >= 0)
                    image.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));

                if (y + 1 < height)
                    image.SetPixel(x - 1, y + 1, Color.FromArgb(r, g, b));
            }

            if (x + 1 < width)
            {
                image.SetPixel(x + 1, y, Color.FromArgb(r, g, b));

                if (y - 1 >= 0)
                    image.SetPixel(x + 1, y - 1, Color.FromArgb(r, g, b));

                if (y + 1 < height)
                    image.SetPixel(x + 1, y + 1, Color.FromArgb(r, g, b));
            }            
        }


        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            maxValue += Double.Epsilon;
            var next = random.NextDouble();
            return minValue + (next * (maxValue - minValue));
        }

        public static double[] FindSolution(Func<double[], double> Cost, double[,] boundaries, double c1 = 2, double c2 = 2, int dim = 2, int swarmSize = 40, int iterWithoutBetterSolution = 1000)
        {
            double[] globalBest = new double[dim];
            double[][] particles = new double[swarmSize][];
            double[][] particlesBests = new double[swarmSize][];
            double[][] velocities = new double[swarmSize][];

            for (int i = 0; i < swarmSize; i++)
            {
                particles[i] = new double[dim];
                particlesBests[i] = new double[dim];
                velocities[i] = new double[dim];
            }

            // for each particle i = 1, ..., S do
            for (int i = 0; i < swarmSize; i++)
            {
                // Initialize the particle's position with a uniformly distributed random vector: xi ~ U(blo, bup)
                // Initialize the particle's best known position to its initial position: pi ← xi
                // Initialize the particle's velocity: vi ~ U(-|bup-blo|, |bup-blo|)
                for (int j = 0; j < dim; j++)
                {
                    particles[i][j] = RandomNumberBetween(boundaries[j, 0], boundaries[j, 1]);
                    particlesBests[i][j] = particles[i][j];
                    velocities[i][j] = RandomNumberBetween(-(boundaries[j, 1] - boundaries[j, 0]), boundaries[j, 1] - boundaries[j, 0]);
                }

                // if f(pi) < f(g) then
                //    update the swarm's best known  position: g ← pi
                if (i == 0 || Cost(particlesBests[i]) < Cost(globalBest))
                    for (int j = 0; j < dim; j++)
                        globalBest[j] = particlesBests[i][j];

            }

            int iteration = 0;
            bool changed = false;
            // while a termination criterion is not met do:
            while (iteration != iterWithoutBetterSolution)
            {
                changed = false;
                // for each particle i = 1, ..., S do
                for (int i = 0; i < swarmSize; i++)
                {
                    // for each dimension d = 1, ..., n do
                    for (int j = 0; j < dim; j++)
                    {
                        // Pick random numbers: rp, rg ~U(0, 1)
                        double r1 = RandomNumberBetween(0, 1);
                        double r2 = RandomNumberBetween(0, 1);

                        // Update the particle's velocity: vi,d ← ω vi,d + φp rp (pi,d-xi,d) + φg rg (gd-xi,d)
                        velocities[i][j] += c1 * r1 * (globalBest[j] - particlesBests[i][j]) + c2 * r2 * (globalBest[j] - particlesBests[i][j]);

                        // Update the particle's position: xi ← xi + vi
                        if (particles[i][j] + velocities[i][j] < boundaries[j, 0])
                            particles[i][j] = boundaries[j, 0];
                        else if (particles[i][j] + velocities[i][j] > boundaries[j, 1])
                            particles[i][j] = boundaries[j, 1];
                        else
                            particles[i][j] += velocities[i][j];
                    }

                    // if f(xi) < f(pi) then
                    //    Update the particle's best known position: pi ← xi
                    if (Cost(particles[i]) < Cost(particlesBests[i]))
                    {
                        for (int j = 0; j < dim; j++)
                            particlesBests[i][j] = particles[i][j];

                        // if f(pi) < f(g) then
                        //    Update the swarm's best known position: g ← pi
                        if (Cost(particles[i]) < Cost(globalBest))
                        {
                            for (int j = 0; j < dim; j++)
                                globalBest[j] = particlesBests[i][j];
                            changed = true;
                        }
                    }
                }

                // stop condition
                if (changed)
                    iteration = 0;
                else
                    iteration++;
            }
            return globalBest;
        }
    }
}
