using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    public class Team
    {
        private Planet homePlanet {get; set;}
        public int ID { get; set; }
        private const float radius = 50;
        private Color Color;
        public string colorText;
        //private int totalShipCount;
        public List<Planet> planetsColonized = new List<Planet>();
        public float giveUp = 0; 

        public Team(int id)
        {
            ID = id;
            Random rand = new Random();
            bool loop = true;
            int i = 0;
            while(loop)
            {
                i = rand.Next(TeamManager.colorIndex.Count);
                if(TeamManager.usedColorIndexes.All(x => x != i))
                {
                    TeamManager.usedColorIndexes.Add(i);
                    loop = false;
                }
            }           
            int index = TeamManager.colorIndex[i];
            colorText = TeamManager.Colors[index].Item1;
            Color = TeamManager.Colors[index].Item2;
            TeamManager.colorIndex.RemoveAt(i);
            TeamManager.colorIndex.TrimExcess();
            
        }

        public Team()
        {
            // TODO: Complete member initialization
        }

     

        public static void GenerateHomePositions(int n) 
        {
            Random rand = new Random();
            bool passed = false;
            Vector2 randPosition = new Vector2(0, 0);

            int i = 0;
            foreach(Team team in TeamManager.teams) 
            {
                passed = false;
                while (!passed)
                {
                    randPosition = new Vector2(rand.NextFloat(LevelSpawner.BorderMargin.X + radius, GameRoot.ScreenSize.X - LevelSpawner.BorderMargin.X - 2 * radius), rand.NextFloat(LevelSpawner.BorderMargin.Y + radius, GameRoot.ScreenSize.Y - LevelSpawner.BorderMargin.Y - 2 * radius));
                    if (i == 0)
                    {
                        float distance = Vector2.DistanceSquared(randPosition, GameRoot.ScreenSize * 0.5f);
                        if(distance < (0.5f * GameRoot.ScreenSize.Y * 0.5f * GameRoot.ScreenSize.Y))
                            passed = false;
                        else
                            passed = true;
                    }
                        
                    else 
                    {
                        for (int j = 0; j < i; j++) 
                        {
                            float distance = Vector2.DistanceSquared(randPosition, TeamManager.teams[j].homePlanet.Position);
                            if (distance < (1.5f * GameRoot.ScreenSize.X / n) * (1.5f * GameRoot.ScreenSize.X / n))
                            {
                                passed = false;
                                break;
                            }
                            else
                                passed = true;
                        }
                    }                   
                }

                team.homePlanet = new Planet(randPosition, team.Color, 1.0f, i, team);
                team.planetsColonized.Add(team.homePlanet);
                i++;
            }
        }

        public Planet getHomePlanet()
        {
            
            return homePlanet;
        }
               
        public Color getColor()
        {
            return Color;
        }
    }
}
