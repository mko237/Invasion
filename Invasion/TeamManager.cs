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
        public static float[] teamProductionRate = new float[2];
        public static List<Tuple<String, Color>> Colors = new List<Tuple<String, Color>> 
        { 
            new Tuple<String, Color>("RED", Color.Red), 
            new Tuple<String, Color>("BLUE",Color.DeepSkyBlue), 
            new Tuple<String, Color>("YELLOW",Color.Yellow),           
            new Tuple<String, Color>("PINK",Color.HotPink), 
            new Tuple<String, Color>("GREEN",Color.LawnGreen), 
            new Tuple<String, Color>("PURPLE",Color.MediumVioletRed), 
             
        };
        public static List<int> colorIndex = new List<int> { 0, 1, 2, 3, 4, 5};
        public static List<int> usedColorIndexes = new List<int>();

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

        public static void Update()
        {
            //determines win condition
            bool gameOver = false;
            int teamsWithPlanets = 0;
            foreach(Team team in teams)
            {
                if (team.planetsColonized.Count > 0)
                    teamsWithPlanets++;                                   
            }
            
            gameOver = teamsWithPlanets == 1;         
            if(gameOver)
            {
                EntityManager.newLevel();
            }

            //refreshes total team production rate and ship count 
            getShipCount();
            getProductionRate();
        }

        public static void getProductionRate()
        {
            int i = 0;
            float productionRate = new float();

            foreach(Team team in TeamManager.teams)
            {
                productionRate = 0;
                foreach (Planet p in team.planetsColonized)
                    productionRate += p.productionRate;
                teamProductionRate[i] = productionRate;
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
