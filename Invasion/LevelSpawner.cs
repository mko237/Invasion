﻿using System;
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
        private static float[] sizes;
        private static int PositionsCreated = 0;
        private static int minDistance = 0;
        private static float maxSize = .05f; //determines the max size of planets
        private static float minSize = .005f; //determines the min size of plaents
        

        public LevelSpawner(int numplanets)
        {
            NumPlanets = numplanets;
            Positions = new Vector2[NumPlanets];
            sizes = new float[NumPlanets];
        }

        private void GeneratePositions()
        {
            //for (int i = 0; i < NumPlanets; i++) // generate random sizes for each planet
            //{
            //    float randsize = rand.NextFloat(0, 1);
            //    Sizes[i] = randsize;
            //}
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
                //bool testb = false;
                int sizeIndex = 0;
                Vector2 borderMargin = new Vector2(GameRoot.ScreenSize.X / 35, GameRoot.ScreenSize.Y / 20);

                Vector2 randPosition = new Vector2(rand.NextFloat(borderMargin.X, GameRoot.ScreenSize.X - borderMargin.X), rand.NextFloat(borderMargin.Y, GameRoot.ScreenSize.Y - borderMargin.Y));
                
                foreach (Vector2 position in Positions)
                {
                    /*
                    if ((randposition.X - sizes[i] * BoarderConstant >= position.X + (sizes[sizeIndex] * BoarderConstant)) || (randposition.X + sizes[i] * BoarderConstant <= position.X - (sizes[sizeIndex] * BoarderConstant))) // if x is not within range of this planet
                    {
                        //testb = (randposition.X >= position.X + (Sizes[Sizeindex] * 100));
                        //Console.WriteLine(testb);
                        //continue;
                        
                        
                    }
                    else if (!((randposition.Y - sizes[i] * BoarderConstant >= position.Y + (sizes[sizeIndex] * BoarderConstant)) || (randposition.Y + sizes[i] * BoarderConstant <= position.Y + (sizes[sizeIndex] * BoarderConstant)))) // otherwise if x is in range , check that y is not in range. if it is, remove is true, and planet will not be added.
                    {
                        remove = true;
                        var xtop = randposition.X + (sizes[sizeIndex] * 100);
                        var xbot = randposition.X - (sizes[sizeIndex] * 100);
                        var ytop = randposition.Y + (sizes[sizeIndex] * 100);
                        var ybot = randposition.Y - (sizes[sizeIndex] * 100);

                        Console.WriteLine("removed: " + randposition + "=" + position + ": (" + xtop + "-" + xbot + "), (" + ytop + "-" + ybot + ")");
                        break;
                    }
                     */
                    float distance = Vector2.DistanceSquared(randPosition, position);
                    if (distance < (0.5f * sizes[sizeIndex] * imageSize + 0.5f * sizes[i] * imageSize + minDistance) * (0.5f * sizes[sizeIndex] * imageSize + 0.5 * sizes[i] * imageSize + minDistance))
                    {
                        remove = true;
                        break;
                    }
                    else if (randPosition.X - radius < borderMargin.X || randPosition.Y - radius < borderMargin.Y || randPosition.X + radius > GameRoot.ScreenSize.X - borderMargin.X || randPosition.Y + radius > GameRoot.ScreenSize.Y - borderMargin.Y)
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
                Planets[i] = new Planet(Positions[i], Color.White, sizes[i]);
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

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    foreach(Planet planet in Planets)
        //    {
        //        planet.Draw(spriteBatch);
        //    }
        //}

    }
}