using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    class LevelSpawner
    {
        private static int NumPlanets = 0;
        private static Planet[] Planets;
        private static Random rand = new Random();
        private static Vector2[] Positions;
        private static int[] PlanetIDs;
        private static float[] sizes;
        private static int PositionsCreated = 0;
        private static int minDistance = 0;
        private static float maxSize = .8f; //determines the max size of planets
        private static float minSize = .075f; //determines the min size of plaents
        private static float homeMultiplier = 1.75f;
        
        

        public LevelSpawner(int numplanets)
        {
            NumPlanets = numplanets;
            Positions = new Vector2[NumPlanets];
            sizes = new float[NumPlanets];
            PlanetIDs = new int[NumPlanets];

        }

        private void GeneratePositions()
        {

            Vector2 borderMargin = new Vector2(GameRoot.ScreenSize.X / 35, GameRoot.ScreenSize.Y / 20);

            Vector2 randHomePositionA = new Vector2(rand.NextFloat(borderMargin.X, 0.25f * GameRoot.ScreenSize.X), rand.NextFloat(borderMargin.Y, GameRoot.ScreenSize.Y - borderMargin.Y));
            Positions[0] = randHomePositionA;
            sizes[0] = homeMultiplier;

            Vector2 randHomePositionB = new Vector2(rand.NextFloat(0.75f * GameRoot.ScreenSize.X, GameRoot.ScreenSize.X - borderMargin.X), rand.NextFloat(borderMargin.Y, GameRoot.ScreenSize.Y - borderMargin.Y));
            Positions[1] = randHomePositionB;
            sizes[1] = homeMultiplier;

            int attempts = 0;
            int imageSize = 100; // the image is 100 x 100 so i set the cosnstant to this. 
            
            for (int i = 0; i < NumPlanets; i++) //
            {
                attempts++;
                if (attempts > NumPlanets + 1000000) //stops trying to generate planets if a certain threshold is reached
                    break;
                
                float randsize = rand.NextFloat(minSize, maxSize);// generate random sizes for each planet
                sizes[i] = randsize;
                
                bool remove = false;
                float radius = 0.5f * sizes[i] * imageSize;
                int sizeIndex = 0;

                Vector2 randPosition = new Vector2(rand.NextFloat(borderMargin.X, GameRoot.ScreenSize.X - borderMargin.X), rand.NextFloat(borderMargin.Y, GameRoot.ScreenSize.Y - borderMargin.Y));
                
                foreach (Vector2 position in Positions)
                {
                   
                    float distance = Vector2.DistanceSquared(randPosition, position);
                    if (distance < (0.5f * sizes[sizeIndex] * imageSize + 0.5f * sizes[i] * imageSize + minDistance) * (0.5f * sizes[sizeIndex] * imageSize + 0.5 * sizes[i] * imageSize + minDistance)) //compares the distance between the two positions and removes if they are overlapping.
                    {
                        remove = true;
                        break;
                    }
                    else if (randPosition.X - radius < borderMargin.X || randPosition.Y - radius < borderMargin.Y || randPosition.X + radius > GameRoot.ScreenSize.X - borderMargin.X || randPosition.Y + radius > GameRoot.ScreenSize.Y - borderMargin.Y) // removes if position blus radius is a too close to the borderMargin.
                    {
                        remove = true;
                        break;
                    }
                    sizeIndex++;

                }

                if (!remove)// if remove is false set position
                {
                    Positions[i] = randPosition;
                    PlanetIDs[i] = i;
                    PositionsCreated++;
                }
                else //do nothing, try again
                {
                    i--;
                }
                
            }
        }
        public void Spawn()
        {
            GeneratePositions();
            Planets = new Planet[PositionsCreated];
            Console.WriteLine("Postions created: " + PositionsCreated);
            
            for (int i = 0; i < PositionsCreated; i++)
            {
                Planets[i] = new Planet(Positions[i], Color.White, sizes[i],PlanetIDs[i]);
                //Console.WriteLine("p: " + Positions[i] + " s: " + Sizes[i]);
                EntityManager.Add(Planets[i]);
            }
        }

        //public void Spawn() //spawns without collision logic
        //{
        //    for (int i = 0; i < NumPlanets; i++)
        //    {
        //        Vector2 randposition = new Vector2(rand.NextFloat(GameRoot.ScreenSize.X / 35, GameRoot.ScreenSize.X - GameRoot.ScreenSize.X / 35), rand.NextFloat(GameRoot.ScreenSize.Y / 20, GameRoot.ScreenSize.Y - GameRoot.ScreenSize.Y / 20));
        //        float randsize = rand.NextFloat(0, 1);
                
        //        Planets[i] = new Planet(randposition, Color.White, randsize);
        //        EntityManager.Add(Planets[i]);
        //    }
        //}

        

    }
}
