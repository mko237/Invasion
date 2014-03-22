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
        // How we access the teams in the program
        public static List<Team> teams = new List<Team>();

        private Planet homePlanet {get; set;}
        private const float radius = 50;
        private Color Color { get; set; }
        private int totalShipCount;
        private List<Planet> planetsColonized = new List<Planet>();

        private static List<Color> Colors = new List<Color> { Color.Red, Color.Blue, Color.GreenYellow, Color.Turquoise, Color.Goldenrod, Color.Pink, Color.ForestGreen, Color.Purple, Color.Brown };

        public Team()
        {
            Random rand = new Random();
            int index = rand.Next(Colors.Count);
            Color = Colors[index];
            Colors.RemoveAt(index);
        }

        public static void GenerateTeams(LevelSpawner level, int n) 
        {
            for (int i = 0; i < n; i++)
            {
                teams.Add(new Team());
            }

            GenerateHomePositions(n);

            int j = 0;
            Planet[] homePlanets = new Planet[n];
            foreach (Team team in Team.teams)
            {
                homePlanets[j] = team.homePlanet;
                j++;
            }

            level.AddHomePlanets(homePlanets);
        }

        private static void GenerateHomePositions(int n) 
        {
            Random rand = new Random();
            bool passed = false;
            Vector2 randPosition = new Vector2(0, 0);

            int i = 0;
            foreach(Team team in teams) 
            {
                passed = false;
                while (!passed)
                {
                    randPosition = new Vector2(rand.NextFloat(LevelSpawner.BorderMargin.X + radius, GameRoot.ScreenSize.X - LevelSpawner.BorderMargin.X - 2 * radius), rand.NextFloat(LevelSpawner.BorderMargin.Y + radius, GameRoot.ScreenSize.Y - LevelSpawner.BorderMargin.Y - 2 * radius));
                    
                    if (i == 0)
                    {
                        float distance = Vector2.DistanceSquared(randPosition, GameRoot.ScreenSize * 0.5f);
                        if(distance < (0.5f * GameRoot.ScreenSize.Y * 0.5f * GameRoot.ScreenSize.Y))
                        {
                            passed = false;
                        }
                        else
                        {
                            passed = true;
                        }
                    }
                        
                    else 
                    {
                        for (int j = 0; j < i; j++) 
                        {
                            float distance = Vector2.DistanceSquared(randPosition, teams[j].homePlanet.Position);
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

        public void addPlanet(Planet p)
        {
            planetsColonized.Add(p);
        }

        public void removePlanet(Planet p)
        {
            //Planet planet = planetsColonized.Where(x => x.ID == p.ID);
            //planetsColonized.Remove(planet);
        }



        
    }
}
