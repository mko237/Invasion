using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invasion
{
    public static class InputParser
    {
        public static String Command = null;
        private static string lastCommand = "";
        private static string player;

        public static void Update()
        {
            try
            {
                Command = ServerInput.serverInput;
                ServerInput.serverInput = null;

                if (Command != null)
                {

                    Command = Command.Replace("\r", string.Empty);
                    Command = Command.Replace("\n", string.Empty);
                    Command = Command.Replace("\0", string.Empty);

                    TestInputDraw.Command = Command;


                    if (Command.Contains("<!>"))
                    {
                        int seperator = Command.IndexOf("<!>");
                        player = Command.Substring(0, seperator);
                        // Console.WriteLine("player = " + player);
                        Command = Command.Substring(seperator + 3);

                    }

                    //Command = InputDisplay.Command.Replace("D", String.Empty);

                    // Console.WriteLine("command: " + Command);
                    int destination = 0;
                    float percentage = .5f;
                    if (Command != "R")
                        lastCommand = Command;
                    if (Command == "R")
                        Command = lastCommand;

                    #region debug parse logic
                    //if (int.TryParse(Command, out destination) && destination <= EntityManager.planets.Count)//kill this for live version
                    //{

                    //    List<Planet> planet = EntityManager.planets.Where(x => x.ID == destination).ToList();
                    //    //if (TeamManager.teams[0].getHomePlanet().shipCount < number)
                    //    //{
                    //    //    ShipManager.sendShips(TeamManager.teams[0], (int)TeamManager.teams[0].getHomePlanet().shipCount, TeamManager.teams[0].getHomePlanet(), planet[0]);
                    //    //    TeamManager.teams[0].getHomePlanet().shipCount = 0;
                    //    //}
                    //    //else
                    //    //{

                    //    //    TeamManager.teams[0].getHomePlanet().shipCount -= number;
                    //    //}  
                    //    ShipManager.sendShips(TeamManager.teams[0], percentage, TeamManager.teams[0].getHomePlanet(), planet[0]);
                    //}
                    //else if (Command == "")//kill this for live version
                    //{
                    //    ShipManager.sendShips(TeamManager.teams[0], percentage, TeamManager.teams[0].getHomePlanet(), TeamManager.teams[1].getHomePlanet());
                    //}
                    //else if (Command == "L")//kill this for live version
                    //{
                    //    EntityManager.newLevel();
                    //}
                    #endregion

                    if (Command != "")
                    {
                        try
                        {
                            //int middleMarker = Command.IndexOf("OemSemicolon");

                            int middleMarker = Command.IndexOf("-");//change to ":" for server input
                            string fromCommand = middleMarker == -1 ? " " : Command.Substring(0, middleMarker);
                            if (Command.Contains("*"))
                            {
                                fromCommand = Command;
                            }
                            List<int> fromCommandList = new List<int>();




                            //Console.WriteLine(fromCommand);
                            //char fromTeam = string.IsNullOrEmpty(Command) ? ' ' : fromCommand[0];
                            // string fromPlanet = middleMarker == -1? " " : fromCommand.Substring(1,middleMarker -1);
                            //Console.WriteLine(fromPlanet);
                            int intFromPlanet;
                            //string toCommand = middleMarker == -1 ? " " : Command.Substring(middleMarker + 12);
                            string toCommand = middleMarker == -1 ? " " : Command.Substring(middleMarker + 1);//remove plus twelve (its the count of OemSemicolon)
                            int toPlanet;
                            // Console.WriteLine("to: " + toCommand);
                            // Console.WriteLine("from: " + fromCommand);
                            //Console.WriteLine(toCommand);
                            // char toTeam = toCommand[0] == 'A' || toCommand[0] == 'B'? toCommand[0] : 'N';
                            if (int.TryParse(toCommand, out toPlanet) && !fromCommand.Contains("*"))
                            {
                                //handles multiple from planets
                                bool getFromPlanets = true;
                                while (getFromPlanets)
                                {
                                    string firstPlanet = null;
                                    if (fromCommand.Contains("+"))
                                    {
                                        int Planet = 999;
                                        firstPlanet = fromCommand.Substring(0, fromCommand.IndexOf("+"));
                                        fromCommand = fromCommand.Substring(fromCommand.IndexOf("+") + 1);
                                        int.TryParse(firstPlanet, out Planet);
                                        if (Planet != 999)
                                            fromCommandList.Add(Planet);

                                    }
                                    else
                                    {
                                        int Planet = 999;
                                        firstPlanet = fromCommand;
                                        int.TryParse(firstPlanet, out Planet);
                                        if (Planet != 999)
                                            fromCommandList.Add(Planet);
                                        getFromPlanets = false;


                                    }
                                }

                                for (int i = 0; i < fromCommandList.Count; i++)
                                {

                                    try
                                    {
                                        List<Planet> fPlanet = EntityManager.planets.Where(x => x.ID == fromCommandList[i]).ToList();
                                        List<Team> fromTeam = TeamManager.teams.Where(x => x.ID == fPlanet[0].team.ID).ToList();
                                        List<Planet> tPlanet = EntityManager.planets.Where(x => x.ID == toPlanet).ToList();
                                        int playerTeam = 999;
                                        bool checkTeams = true;
                                        while (checkTeams)
                                        {
                                            if (Players.Team1.Contains(player))
                                            {
                                                playerTeam = 0;
                                                percentage = (float)((1 / Players.Team1.Count) / fromCommandList.Count);
                                                checkTeams = false;

                                            }
                                            else if (Players.Team2.Contains(player))
                                            {
                                                playerTeam = 1;
                                                percentage = (float)((1 / Players.Team2.Count) / fromCommandList.Count);
                                                checkTeams = false;
                                            }
                                            else
                                            {
                                                if (fPlanet[0].team.ID == 0)
                                                {
                                                    Players.Team1.Add(player);
                                                    playerTeam = 0;
                                                    percentage = (float)((1 / Players.Team1.Count) / fromCommandList.Count);
                                                    checkTeams = false;

                                                }
                                                else
                                                {
                                                    Players.Team2.Add(player);
                                                    playerTeam = 1;
                                                    percentage = (float)((1 / Players.Team2.Count) / fromCommandList.Count);
                                                    checkTeams = false;

                                                }

                                            }

                                        }
                                        if (fPlanet[0].team.ID == playerTeam)
                                            ShipManager.sendShips(fromTeam[0], percentage, fPlanet[0], tPlanet[0]);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (fromCommand.Contains("*") && !fromCommand.Contains("+"))
                            {
                                int playerTeam = 999;
                                bool checkTeams = true;
                                while (checkTeams)
                                {
                                    if (Players.Team1.Contains(player))
                                    {
                                        playerTeam = 0;
                                        percentage = (float)((1 / Players.Team1.Count));
                                        checkTeams = false;

                                    }
                                    else if (Players.Team2.Contains(player))
                                    {
                                        playerTeam = 1;
                                        percentage = (float)((1 / Players.Team2.Count));
                                        checkTeams = false;
                                    }
                                    else
                                    {
                                        checkTeams = false;
                                    }
                                }
                                if (playerTeam != 999)
                                {
                                    int allIndex = fromCommand.IndexOf("*");
                                    //int tId = 0;
                                    int pId = 0;
                                    //bool fteam = int.TryParse(fromCommand.Substring(0, allIndex), out tId) ? true : false;
                                    bool tteam = int.TryParse(fromCommand.Substring(allIndex + 1), out pId) ? true : false;

                                    if (tteam)
                                    {
                                        try
                                        {
                                            //List<Planet> planetsWithTeams = EntityManager.planets.Where(x => x.team != null).ToList();
                                            //List<Planet> selectedPlanet = planetsWithTeams.Where(x => x.ID == tId).ToList();
                                            List<Team> fromTeam = TeamManager.teams.Where(x => x.ID == playerTeam).ToList();
                                            List<Planet> tPlanet = EntityManager.planets.Where(x => x.ID == pId).ToList();

                                            foreach (Planet planet in fromTeam[0].planetsColonized)
                                            {
                                                ShipManager.sendShips(fromTeam[0], (float)(percentage / fromTeam[0].planetsColonized.Count), planet, tPlanet[0]);
                                            }
                                        }
                                        catch { }
                                    }
                                }


                            }






                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

        }




    }
}

