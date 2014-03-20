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
        private static float[] Sizes;
        private static int PositionsCreated = 0;
        

        public LevelSpawner(int numplanets)
        {
            NumPlanets = numplanets;
            Positions = new Vector2[NumPlanets];
            Sizes = new float[NumPlanets];
        }

        private void GeneratePositions()
        {
            //for (int i = 0; i < NumPlanets; i++) // generate random sizes for each planet
            //{
            //    float randsize = rand.NextFloat(0, 1);
            //    Sizes[i] = randsize;
            //}
            int attempts = 0;
            int BoarderConstant = 100; // the image is 100 x 100 so i set the cosnstant to this. 
            
            for (int i = 0; i < NumPlanets; i++) //
            {
                attempts++;
                if (attempts > NumPlanets + 1000) //stops trying to generate planets if a certain threshold is reached
                    break;
                
                float randsize = rand.NextFloat(.3f, 1.5f);// generate random sizes for each planet
                Sizes[i] = randsize;
                
               

                bool remove = false;
                bool testb = false;
                int Sizeindex = 0;

                Vector2 randposition = new Vector2(rand.NextFloat(GameRoot.ScreenSize.X / 35, GameRoot.ScreenSize.X - GameRoot.ScreenSize.X / 35), rand.NextFloat(GameRoot.ScreenSize.Y / 20, GameRoot.ScreenSize.Y - GameRoot.ScreenSize.Y / 20));
                
                foreach (var position in Positions)
                {
                    if ((randposition.X - Sizes[i] * BoarderConstant >= position.X + (Sizes[Sizeindex] * BoarderConstant)) || (randposition.X + Sizes[i] * BoarderConstant <= position.X - (Sizes[Sizeindex] * BoarderConstant))) // if x is not within range of this planet
                    {
                        //testb = (randposition.X >= position.X + (Sizes[Sizeindex] * 100));
                       // Console.WriteLine(testb);
                        //continue;
                        
                        
                    }
                    else if (!((randposition.Y - Sizes[i] * BoarderConstant >= position.Y + (Sizes[Sizeindex] * BoarderConstant)) || (randposition.Y + Sizes[i] * BoarderConstant <= position.Y + (Sizes[Sizeindex] * BoarderConstant)))) // otherwise if x is in range , check that y is not in range. if it is, remove is true, and planet will not be added.
                    {
                        remove = true;
                        var xtop = randposition.X + (Sizes[Sizeindex] * 100);
                        var xbot = randposition.X - (Sizes[Sizeindex] * 100);
                        var ytop = randposition.Y + (Sizes[Sizeindex] * 100);
                        var ybot = randposition.Y - (Sizes[Sizeindex] * 100);

                        Console.WriteLine("removed: " + randposition + "=" + position + ": (" + xtop + "-" + xbot + "), (" + ytop + "-" + ybot+")");
                        break;
                    }
                    else 
                    {
                        continue;
                    }
                    Sizeindex++;

                }

                if (!remove)// if remove is false set position
                {
                    Positions[i] = randposition;
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
            Console.WriteLine("Psotions created: " + PositionsCreated);
            
            for (int i = 0; i < PositionsCreated; i++)
            {
                Planets[i] = new Planet(Positions[i], Color.White, Sizes[i]);
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
