using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    class LevelSpawner
    {
        private static int NumPlanets = 0;
        private static List<Planet> Planets;
        private static Random rand = new Random();
        private static Vector2[] Positions;
        private static float[] sizeMultipliers;
        public static Vector2 BorderMargin = new Vector2(GameRoot.ScreenSize.X / 35, GameRoot.ScreenSize.Y / 20); 
        private static int PositionsCreated = 0;
        private const int minDistance = 0;
        private const float maxSize = 0.9f; //determines the max size of planets
        private const float minSize = 0.075f; //determines the min size of plaents
        public static int imageSize;
        

        public LevelSpawner(int numplanets)
        {
            NumPlanets = numplanets;
            Planets = new List<Planet>();
            Positions = new Vector2[NumPlanets];
            sizeMultipliers = new float[NumPlanets];
            imageSize = Art.Planet.Width; // the image is 100 x 100 so i set the cosnstant to this. 
        }

        private void GeneratePositions()
        {
            int attempts = 0;
            
            int i = Team.teams.Count;
            while(i < NumPlanets) 
            {
                attempts++;
                if (attempts > NumPlanets + 1000000) //stops trying to generate planets if a certain threshold is reached
                    break;
                
                float randsize = rand.NextFloat(minSize, maxSize);// generate random sizes for each planet
                sizeMultipliers[i] = randsize;
                
                bool remove = false;
                float radius = 0.5f * sizeMultipliers[i] * imageSize;

                Vector2 randPosition = new Vector2(rand.NextFloat(BorderMargin.X, GameRoot.ScreenSize.X - BorderMargin.X - 2 * radius), rand.NextFloat(BorderMargin.Y, GameRoot.ScreenSize.Y - BorderMargin.Y - 2 * radius));

                int sizeIndex = 0;
                foreach (Vector2 position in Positions)
                {
                   
                    float distance = Vector2.DistanceSquared(randPosition, position);
                    if (distance < (0.5f * sizeMultipliers[sizeIndex] * imageSize + 0.5f * sizeMultipliers[i] * imageSize + minDistance) * (0.5f * sizeMultipliers[sizeIndex] * imageSize + 0.5 * sizeMultipliers[i] * imageSize + minDistance)) //compares the distance between the two positions and removes if they are overlapping.
                    {
                        remove = true;
                        break;
                    }
                    sizeIndex++;

                }

                if (!remove)// if remove is false set position
                {
                    Positions[i] = randPosition;
                    PositionsCreated++;
                    i++;
                }    
            }
        }

        public void Spawn()
        {
            GeneratePositions();      

            Console.WriteLine("Positions created: " + PositionsCreated);

            for (int i = Team.teams.Count; i < PositionsCreated; i++)
            {
                Planet planet = new Planet(Positions[i], Color.White, sizeMultipliers[i], 0.5f * sizeMultipliers[i] * imageSize);
                Planets.Add(planet);
                EntityManager.Add(planet);
            }
        }

        public void AddHomePlanets(Planet[] homePlanets)
        {
            int i = 0;
            foreach (Planet planet in homePlanets)
            {
                Planets.Add(planet);
                Positions[i] = planet.Position;
                sizeMultipliers[i] = 1.0f;
                EntityManager.Add(planet);
                i++;
            }
        }
    }
}
