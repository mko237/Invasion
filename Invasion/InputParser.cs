using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invasion
{
    public static class InputParser
    {
        private static String Command = null;
        private static string lastCommand = "";

        public static void Update()
        {
            Command = InputDisplay.Command.Replace("D", String.Empty);
           //Command = TestInputDraw.Command;
           // Console.WriteLine("command: " + Command);
            int destination = 0;
            float percentage = .5f;
            if(Command != "R")
                lastCommand = Command;
            if (Command == "R")
                Command = lastCommand;

            #region debug parse logic
            if (int.TryParse(Command, out destination) && destination <= EntityManager.planets.Count)
            {
               
                List<Planet> planet = EntityManager.planets.Where(x => x.ID == destination).ToList();              
                //if (TeamManager.teams[0].getHomePlanet().shipCount < number)
                //{
                //    ShipManager.sendShips(TeamManager.teams[0], (int)TeamManager.teams[0].getHomePlanet().shipCount, TeamManager.teams[0].getHomePlanet(), planet[0]);
                //    TeamManager.teams[0].getHomePlanet().shipCount = 0;
                //}
                //else
                //{
                    
                //    TeamManager.teams[0].getHomePlanet().shipCount -= number;
                //}  
                ShipManager.sendShips(TeamManager.teams[0], percentage, TeamManager.teams[0].getHomePlanet(), planet[0]);
            }
            else if (Command == "")
            {
                ShipManager.sendShips(TeamManager.teams[0], percentage, TeamManager.teams[0].getHomePlanet(), TeamManager.teams[1].getHomePlanet());
            }
            else if (Command == "L")
            {
                EntityManager.newLevel();
            }
            #endregion

            if (Command != "")
            {
                int middleMarker = Command.IndexOf("OemSemicolon");
                //int middleMarker = Command.IndexOf(";");//change to ":" for server input
                string fromCommand = middleMarker == -1 ? " " : Command.Substring(0, middleMarker);
                //Console.WriteLine(fromCommand);
                //char fromTeam = string.IsNullOrEmpty(Command) ? ' ' : fromCommand[0];
                // string fromPlanet = middleMarker == -1? " " : fromCommand.Substring(1,middleMarker -1);
                //Console.WriteLine(fromPlanet);
                int intFromPlanet;
                string toCommand = middleMarker == -1 ? " " : Command.Substring(middleMarker + 12);
                //string toCommand = middleMarker == -1 ? " " : Command.Substring(middleMarker + 1);//remove plus twelve (its the count of OemSemicolon)
               // Console.WriteLine("to: " + toCommand);
               // Console.WriteLine("from: " + fromCommand);
                //Console.WriteLine(toCommand);
                // char toTeam = toCommand[0] == 'A' || toCommand[0] == 'B'? toCommand[0] : 'N';

                int fromPlanet;
                int toPlanet;
                if (int.TryParse(fromCommand, out fromPlanet) && int.TryParse(toCommand, out toPlanet))
                {

                    try
                    {
                         List<Planet> fPlanet = EntityManager.planets.Where(x => x.ID == fromPlanet).ToList();
                         List<Team> fromTeam = TeamManager.teams.Where(x => x.ID == fPlanet[0].team.ID).ToList();
                         List<Planet> tPlanet = EntityManager.planets.Where(x => x.ID == toPlanet).ToList();
                         ShipManager.sendShips(fromTeam[0], percentage, fPlanet[0], tPlanet[0]);
                    }
                    catch (NullReferenceException)
                    {
                        
                    }
                       
                    
                    
                    

                    
                }
                int allIndex = fromCommand.IndexOf("ALL");
                if(allIndex != -1)
                {
                    int tId = 0;
                    int pId = 0;
                    bool fteam = int.TryParse(fromCommand.Substring(0,allIndex), out tId)? true: false;
                    bool tteam = int.TryParse(toCommand, out pId) ? true : false;

                    if(fteam && tteam)
                    {
                        try
                        {
                            List<Planet> planetsWithTeams = EntityManager.planets.Where(x => x.team != null).ToList();
                            List<Planet> selectedPlanet = planetsWithTeams.Where(x => x.ID == tId).ToList();
                            List<Team> fromTeam = TeamManager.teams.Where(x => x.ID == selectedPlanet[0].team.ID).ToList();
                            List<Planet> tPlanet = EntityManager.planets.Where(x => x.ID == pId).ToList();

                            foreach (Planet planet in fromTeam[0].planetsColonized)
                            {
                                ShipManager.sendShips(fromTeam[0], percentage, planet, tPlanet[0]);
                            }
                        }
                        catch { }
                    }

                    
                }
                    





            }

        }




    }
}

