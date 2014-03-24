using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invasion
{
    public static class InputParser
    {
        private static String Command = null;
        
        public static void Update()
        {
            Command = InputDisplay.Command.Replace("D", String.Empty);
            Console.WriteLine(Command);
            int destination = 0;        
            if(int.TryParse(Command, out destination))
            {
                int number = 40;
                List<Planet> planet =  EntityManager.planets.Where(x => x.ID == destination).ToList();

                if (TeamManager.teams[0].getHomePlanet().shipCount > 0)
                {
                    if (TeamManager.teams[0].getHomePlanet().shipCount < number)
                    {
                        ShipManager.sendShips(TeamManager.teams[0], (int)TeamManager.teams[0].getHomePlanet().shipCount, TeamManager.teams[0].getHomePlanet(), planet[0]);
                        TeamManager.teams[0].getHomePlanet().shipCount -= (int)TeamManager.teams[0].getHomePlanet().shipCount;
                    }
                    else
                    {
                        ShipManager.sendShips(TeamManager.teams[0], number, TeamManager.teams[0].getHomePlanet(), planet[0]);
                        TeamManager.teams[0].getHomePlanet().shipCount -= number;
                    } 
                }
            }
        }
    }
}
