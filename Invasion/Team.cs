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
        private const float radius = 50;
        private Color Color;
        private int totalShipCount;
        private List<Planet> planetsColonized = new List<Planet>();

        public Team()
        {
            Random rand = new Random();
            int index = rand.Next(TeamManager.Colors.Count);
            Color = TeamManager.Colors[index];
            TeamManager.Colors.RemoveAt(index);
            TeamManager.Colors.TrimExcess();
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
                            {
                                passed = true;
                            }
                        }
                    }                   
                }

                team.homePlanet = new Planet(randPosition, team.Color, 1.0f, i, team);
                i++;
            }
        }

        public Planet getHomePlanet()
        {
            return homePlanet;
        }

        public void addPlanet(Planet p) //we may want to implement a a team manager that can keep track of these things. but maybe here will be ok too.
        {
            planetsColonized.Add(p);
        }

        public void removePlanet(Planet p)
        {
            //Planet planet = planetsColonized.Where(x => x.ID == p.ID);
            //planetsColonized.Remove(planet);
        }

        public Color getColor()
        {
            return Color;
        }
    }
}
