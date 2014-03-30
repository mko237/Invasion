using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    public static class TeamManager
    {
        public static List<Team> teams = new List<Team>();
        public static int[] teamShipCount = new int[2];
        public static List<Color> Colors = new List<Color> { Color.Red, Color.CadetBlue, Color.GreenYellow, Color.Turquoise, Color.Goldenrod, Color.Pink, Color.ForestGreen, Color.Purple, Color.Brown };

        public static void GenerateTeams(LevelSpawner level, int n)
        {
            for (int i = 0; i < n; i++)
                teams.Add(new Team(i));

            Team.GenerateHomePositions(n);

            int j = 0;
            Planet[] homePlanets = new Planet[n];
            foreach (Team team in TeamManager.teams)
            {
                homePlanets[j] = team.getHomePlanet();
                j++;
            }

            level.AddHomePlanets(homePlanets);
        }

        public static void getShipCount()
        {
            int i = 0;
            int shipCount = new int();
           
            foreach (Team team in TeamManager.teams)
            {
                shipCount = 0;
                int shipsFlying = new int();
                foreach (Planet p in team.planetsColonized)
                    shipCount += (int)p.shipCount;
                foreach (Ship s in EntityManager.ships)
                {
                    if (s.Team.ID == team.ID)
                        shipsFlying += (int)s.Value;
                }
                teamShipCount[i] = shipCount+shipsFlying;
                i++;
            }
        }

        public static void addPlanet(Team t, Planet p) //we may want to implement a a team manager that can keep track of these things. but maybe here will be ok too.
        {
            t.planetsColonized.Add(p);
        }

        public static void removePlanet(Team t, Planet p)
        {
            List<Planet> planet = t.planetsColonized.Where(x => x.ID == p.ID).ToList();
            t.planetsColonized.Remove(planet[0]);
        }

        public static void Clear()
        {
            teams = new List<Team>();
        }
        public static void clearColonized()
        {
            foreach(var team in teams)
            {
                team.planetsColonized = new List<Planet>();
            }
        }
    }
}
