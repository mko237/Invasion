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
           
            #region debug parse logic
            if (int.TryParse(Command, out destination)&&destination<=EntityManager.planets.Count)
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
            else if(Command == "")
            {
                ShipManager.sendShips(TeamManager.teams[0], 40, TeamManager.teams[0].getHomePlanet(), TeamManager.teams[1].getHomePlanet());
            }
            #endregion
           
            if (Command != "")
            {
                int middleMarker = Command.IndexOf("OemSemicolon");//change to ":" for server input
                string fromCommand = middleMarker == -1? " " : Command.Substring(0, middleMarker);
                //Console.WriteLine(fromCommand);
                //char fromTeam = string.IsNullOrEmpty(Command) ? ' ' : fromCommand[0];
                // string fromPlanet = middleMarker == -1? " " : fromCommand.Substring(1,middleMarker -1);
                //Console.WriteLine(fromPlanet);
                int intFromPlanet;
                string toCommand = middleMarker == -1? " " : Command.Substring(middleMarker+12);//remove plus twelve (its the count of OemSemicolon)
                //Console.WriteLine(toCommand);
                // char toTeam = toCommand[0] == 'A' || toCommand[0] == 'B'? toCommand[0] : 'N';

                int fromPlanet;
                int toPlanet;
                if(int.TryParse(fromCommand,out fromPlanet) && int.TryParse(toCommand,out toPlanet))
                {
                   
                    List<Planet> fPlanet = EntityManager.planets.Where(x => x.ID == fromPlanet).ToList();
                    List<Team> fromTeam = TeamManager.teams.Where(x => x.ID == fPlanet[0].team.ID).ToList();
                    List<Planet> tPlanet = EntityManager.planets.Where(x => x.ID == toPlanet).ToList();
                    float percentage = .5f;
                    int numship = (int)(percentage*fPlanet[0].shipCount);

                    ShipManager.sendShips(fromTeam[0], numship, fPlanet[0], tPlanet[0]);
                }
                //if(fromCommand.Substring(1) == "all")

                
               
                

            }
            
         }

       

    
     }
}

