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
        public static ArrayList teams = new ArrayList();

        private Planet homePlanet;
        private float radius;
        private readonly int teamNumber;
        private int totalShipCount;
        private List<Planet> planetsColonized = new List<Planet>();
        private const float homeMultiplier = 1.0f;

        public Team(int t, Color color)
        {
            radius = 0.5f * LevelSpawner.imageSize * homeMultiplier;

            Vector2 randPosition;
            if (teams.Count == 0)
            {
                Random rand = new Random();
                randPosition = new Vector2(rand.NextFloat(LevelSpawner.BorderMargin.X, 0.2f * GameRoot.ScreenSize.X), rand.NextFloat(LevelSpawner.BorderMargin.Y, GameRoot.ScreenSize.Y - LevelSpawner.BorderMargin.Y - 2 * radius));
            }
            else
            {
                randPosition = GenerateHomePositions(teams.Count + 1);
            }
            homePlanet = new Planet(randPosition, color, homeMultiplier, radius);
            teamNumber = t;
        }

        private Vector2 GenerateHomePositions(int n) 
        {
            Random rand = new Random();
            bool passed = false;
            Vector2 randPos = new Vector2(0, 0);

            while (!passed)
            {
                randPos = new Vector2(rand.NextFloat(LevelSpawner.BorderMargin.X, GameRoot.ScreenSize.X - LevelSpawner.BorderMargin.X - 2 * radius), rand.NextFloat(LevelSpawner.BorderMargin.Y, GameRoot.ScreenSize.Y - LevelSpawner.BorderMargin.Y - 2 * radius));
                foreach (Team team in teams)
                {
                    float distance = Vector2.DistanceSquared(randPos, team.homePlanet.Position);
                    if (distance < (1.2f * GameRoot.ScreenSize.X / n) * (1.2f * GameRoot.ScreenSize.X / n))
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

            return randPos;

        }

        public int getTeam()
        {
            return teamNumber;
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
