using System;
using System.Drawing;
using Core;
using PerlinNoise;

namespace AlgorithmsLibrary
{
    //Particle Swarm Optimization 
    public class PSO : ProblemVisualization
    {
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

        //do losowania pozycji i prędkości
        static readonly Random random = new Random();

        //parametry z ustawień
        int width;
        int height;
        int swarmSize;
        double c1;
        double c2;
        int iters;

        //najlepsza znaleziona pozycja globalnie (globalBest[0] - pozycja w osi x, globalBest[1] - pozycja w osi y)
        int[] globalBest;
        //obecne pozycje cząsteczek (particles[i][0] - pozycja cząsteczki i w osi x, particles[i][1] - pozycja cząsteczki i w osi y)
        int[][] particles;
        //najlepsze znalezione pozycje cząsteczek (particlesBests[i][0] - pozycja cząsteczki i w osi x, particlesBests[i][1] - pozycja cząsteczki i w osi y)
        int[][] particlesBests;
        //obecne prędkości cząsteczek (velocities[i][0] - prędkość cząsteczki i w osi x, velocities[i][1] - prędkość cząsteczki i w osi y)
        double[][] velocities;

        //czy to pierwsza iteracja wizualizacji
        bool first = true;
        //liczba iteracji bez zmian
        int iteration;
        //czy najlepsza pozycja globalnie została zmieniona w obecnej iteracji
        bool changed;

        //mapa szumu Perlina, na której będzie szukana najlepsza pozycja
        PerlinNoise.PerlinNoise map;

        //wizualizacja
        public override Bitmap Visualization 
        {
            get
            {
                if (Settings.GetIntValue("Heighest/lowest (1/0)") == 1)
                    return FindHighest();
                else
                    return FindLowest();
            }
        }

        //znajduje najwyższy punkt mapy
        private Bitmap FindHighest()
        {
            //jeśli to pierwsza iteracja
            if (first)
            {
                //pobierz ustawienia
                width = Settings.GetIntValue("Width");
                height = Settings.GetIntValue("Height");
                swarmSize = Settings.GetIntValue("Swarm Size");
                c1 = Settings.GetDoubleValue("c1");
                c2 = Settings.GetDoubleValue("c2");
                iters = Settings.GetIntValue("Iter. Without Change");

                //wygeneruj mapę szumem Perlina
                map = new PerlinNoise.PerlinNoise();
                map.DrawGradients(width, height, Settings.GetFloatValue("Grid Cell Size"));
                map.CreateNoise(width, height, Settings.GetFloatValue("Grid Cell Size"));

                //zainicjuj tablice cząsteczek
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

                //dla każdej cząsteczki
                for (int i = 0; i < swarmSize; i++)
                {
                    //wylosuj początkową pozycję cząsteczki
                    particles[i][0] = random.Next(0, width);
                    particles[i][1] = random.Next(0, height);

                    //ustaw obecną pozycję cząsteczki jako jej najlepszą
                    particlesBests[i][0] = particles[i][0];
                    particlesBests[i][1] = particles[i][1];

                    //wylosuj początkową prędkość cząsteczki
                    velocities[i][0] = RandomNumberBetween(-(double)width / 5, (double)width / 5);
                    velocities[i][1] = RandomNumberBetween(-(double)height / 5, (double)height / 5);

                    //jeśli to pierwsza ustawiana cząsteczka 
                    //lub najlepsza pozycja cząsteczki jest wyżej niż globalna
                    if (i == 0 || map.NoiseTable[particlesBests[i][0], particlesBests[i][1]] > map.NoiseTable[globalBest[0], globalBest[1]])
                    {
                        //uaktualnij globalną pozycję
                        globalBest[0] = particlesBests[i][0];
                        globalBest[1] = particlesBests[i][1];
                    }
                }

                iteration = 0;
                changed = false;
            }

            //jesli to niepierwsza iteracja i liczba iteracji bez zmian globalnej pozycji nie przekroczyła ustawionej granicy
            if (!first && iteration < iters)
            {
                //dla każdej cząsteczki
                for (int i = 0; i < swarmSize; i++)
                {
                    //wylosuj wartości parametrów
                    double r1 = RandomNumberBetween(0, 1);
                    double r2 = RandomNumberBetween(0, 1);

                    //uaktualnij prędkość cząsteczki
                    velocities[i][0] += c1 * r1 * (particlesBests[i][0] - particles[i][0]) + c2 * r2 * (globalBest[0] - particles[i][0]);
                    velocities[i][1] += c1 * r1 * (particlesBests[i][1] - particles[i][1]) + c2 * r2 * (globalBest[1] - particles[i][1]);

                    //uaktualnij pozycję cząsteczki
                    particles[i][0] += (int)velocities[i][0];
                    particles[i][1] += (int)velocities[i][1];

                    //jeśli pozycja wykracza poza granicę mapy, ustaw ją na granicy
                    if (particles[i][0] < 0)
                        particles[i][0] = 0;
                    else if (particles[i][0] >= width)
                        particles[i][0] = width - 1;

                    if (particles[i][1] < 0)
                        particles[i][1] = 0;
                    else if (particles[i][1] >= height)
                        particles[i][1] = height - 1;


                    //jeśli obecna pozycja cząsteczki jest wyżej niż jej najlepsza
                    if (map.NoiseTable[particles[i][0], particles[i][1]] > map.NoiseTable[particlesBests[i][0], particlesBests[i][1]])
                    {
                        //uaktualnij najlepszą pozycję cząsteczki
                        particlesBests[i][0] = particles[i][0];
                        particlesBests[i][1] = particles[i][1];

                        //jeśli obecna pozycja cząsteczki jest wyżej niż najlepsza globalnie pozycja
                        if (map.NoiseTable[particles[i][0], particles[i][1]] > map.NoiseTable[globalBest[0], globalBest[1]])
                        {
                            //uaktualnij globalną pozycję
                            globalBest[0] = particles[i][0];
                            globalBest[1] = particles[i][1];

                            //najlepsza pozycja globalnie została zmieniona w obecnej iteracji
                            changed = true;
                        }
                    }
                }

                //jeśli najlepsza pozycja globalnie została zmieniona w obecnej iteracji
                if (changed)
                    //ustaw liczbę iteracji bez zmian na zero
                    iteration = 0;
                //jeśli nie
                else
                    //zwiększ liczbę iteracji bez zmian na zero
                    iteration++;
            }

            first = false;
            changed = false;


            //wygeneruj obraz do wizualizacji
            Bitmap image = new Bitmap(width, height);

            //narysuj mapę szumu
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    image.SetPixel(i, j, Color.FromArgb(0, 0, (int)((map.NoiseTable[i, j] + 1) * 128)));

            //narysuj pozycje cząsteczek
            for (int i = 0; i < swarmSize; i++)
                particleDraw(image, particles[i][0], particles[i][1], 255, 0, 0);

            //jeśli warunek stopu algorytmu został spełniony
            if (iteration >= iters)
                //narysuj najlepszą globalnie pozycję
                particleDraw(image, globalBest[0], globalBest[1], 0, 255, 0);

            return image;
        }

        //znajduje najniższy punkt mapy
        private Bitmap FindLowest()
        {
            //jeśli to pierwsza iteracja
            if (first)
            {
                //pobierz ustawienia
                width = Settings.GetIntValue("Width");
                height = Settings.GetIntValue("Height");
                swarmSize = Settings.GetIntValue("Swarm Size");
                c1 = Settings.GetDoubleValue("c1");
                c2 = Settings.GetDoubleValue("c2");
                iters = Settings.GetIntValue("Iter. Without Change");

                //wygeneruj mapę szumem Perlina
                map = new PerlinNoise.PerlinNoise();
                map.DrawGradients(width, height, Settings.GetFloatValue("Grid Cell Size"));
                map.CreateNoise(width, height, Settings.GetFloatValue("Grid Cell Size"));

                //zainicjuj tablice cząsteczek
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

                //dla każdej cząsteczki
                for (int i = 0; i < swarmSize; i++)
                {
                    //wylosuj początkową pozycję cząsteczki
                    particles[i][0] = random.Next(0, width);
                    particles[i][1] = random.Next(0, height);

                    //ustaw obecną pozycję cząsteczki jako jej najlepszą
                    particlesBests[i][0] = particles[i][0];
                    particlesBests[i][1] = particles[i][1];

                    //wylosuj początkową prędkość cząsteczki
                    velocities[i][0] = RandomNumberBetween(-(double)width / 5, (double)width / 5);
                    velocities[i][1] = RandomNumberBetween(-(double)height / 5, (double)height / 5);

                    //jeśli to pierwsza ustawiana cząsteczka 
                    //lub najlepsza pozycja cząsteczki jest niżej niż globalna
                    if (i == 0 || map.NoiseTable[particlesBests[i][0], particlesBests[i][1]] < map.NoiseTable[globalBest[0], globalBest[1]])
                    {
                        globalBest[0] = particlesBests[i][0];
                        globalBest[1] = particlesBests[i][1];
                    }
                }

                iteration = 0;
                changed = false;
            }

            //jesli to niepierwsza iteracja i liczba iteracji bez zmian globalnej pozycji nie przekroczyła ustawionej granicy
            if (!first && iteration < iters)
            {
                //dla każdej cząsteczki
                for (int i = 0; i < swarmSize; i++)
                {
                    //wylosuj wartości parametrów
                    double r1 = RandomNumberBetween(0, 1);
                    double r2 = RandomNumberBetween(0, 1);

                    //uaktualnij prędkość cząsteczki
                    velocities[i][0] += c1 * r1 * (particlesBests[i][0] - particles[i][0]) + c2 * r2 * (globalBest[0] - particles[i][0]);
                    velocities[i][1] += c1 * r1 * (particlesBests[i][1] - particles[i][1]) + c2 * r2 * (globalBest[1] - particles[i][1]);

                    //uaktualnij pozycję cząsteczki
                    particles[i][0] += (int)velocities[i][0];
                    particles[i][1] += (int)velocities[i][1];

                    //jeśli pozycja wykracza poza granicę mapy, ustaw ją na granicy
                    if (particles[i][0] < 0)
                        particles[i][0] = 0;
                    else if (particles[i][0] >= width)
                        particles[i][0] = width - 1;

                    if (particles[i][1] < 0)
                        particles[i][1] = 0;
                    else if (particles[i][1] >= height)
                        particles[i][1] = height - 1;


                    //jeśli obecna pozycja cząsteczki jest niżej niż jej najlepsza
                    if (map.NoiseTable[particles[i][0], particles[i][1]] < map.NoiseTable[particlesBests[i][0], particlesBests[i][1]])
                    {
                        //uaktualnij najlepszą pozycję cząsteczki
                        particlesBests[i][0] = particles[i][0];
                        particlesBests[i][1] = particles[i][1];

                        //jeśli obecna pozycja cząsteczki jest niżej niż najlepsza globalnie pozycja
                        if (map.NoiseTable[particles[i][0], particles[i][1]] < map.NoiseTable[globalBest[0], globalBest[1]])
                        {
                            //uaktualnij globalną pozycję
                            globalBest[0] = particles[i][0];
                            globalBest[1] = particles[i][1];

                            //najlepsza pozycja globalnie została zmieniona w obecnej iteracji
                            changed = true;
                        }
                    }
                }

                //jeśli najlepsza pozycja globalnie została zmieniona w obecnej iteracji
                if (changed)
                    //ustaw liczbę iteracji bez zmian na zero
                    iteration = 0;
                //jeśli nie
                else
                    //zwiększ liczbę iteracji bez zmian na zero
                    iteration++;
            }

            first = false;
            changed = false;


            //wygeneruj obraz do wizualizacji
            Bitmap image = new Bitmap(width, height);

            //narysuj mapę szumu
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    image.SetPixel(i, j, Color.FromArgb(0, 0, (int)((map.NoiseTable[i, j] + 1) * 128)));

            //narysuj pozycje cząsteczek
            for (int i = 0; i < swarmSize; i++)
                particleDraw(image, particles[i][0], particles[i][1], 255, 0, 0);

            //jeśli warunek stopu algorytmu został spełniony
            if (iteration >= iters)
                //narysuj najlepszą globalnie pozycję
                particleDraw(image, globalBest[0], globalBest[1], 0, 255, 0);

            return image;
        }

        //rysuje pozycję cząsteczki jako kwadrat 3x3 piksele
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

        //losuje liczbę double z zakresu [minValue; maxValue]
        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            maxValue += Double.Epsilon;
            var next = random.NextDouble();
            return minValue + (next * (maxValue - minValue));
        }       
    }
}
