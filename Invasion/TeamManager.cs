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
        public static List<Color> Colors = new List<Color> { Color.Red, Color.CadetBlue, Color.GreenYellow, Color.Turquoise, Color.Goldenrod, Color.Pink, Color.ForestGreen, Color.Purple, Color.Brown };

        public static void GenerateTeams(LevelSpawner level, int n)
        {
            for (int i = 0; i < n; i++)
                teams.Add(new Team());

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
    
    }
}
