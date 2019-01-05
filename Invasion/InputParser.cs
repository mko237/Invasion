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

                    //TestInputDraw.Command = Command;


                    if (Command.Contains("<!>"))
                    {
                        int seperator = Command.IndexOf("<!>");
                        player = Command.Substring(0, seperator);
                        // Console.WriteLine("player = " + player);
                        Command = Command.Substring(seperator + 3);
                        Players.UpdatePlayerLastCmd(player);

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
                    //if(Command == "1111")
                    //{
                    //    Players.Team2.Add(GameRoot.Instance.TargetElapsedTime.Seconds.ToString());
                    //}
                    //if (Command == "0000")
                    //{
                    //    Players.Team1.Add(GameRoot.Instance.TargetElapsedTime.Seconds.ToString());
                    //}
                    
                    if(Command == "GIVEUP" || Command == "GIVE UP")
                    {
                        //Console.WriteLine("GIVE UP RECIEVED!!!!");
                        for (int i= 0; i < Players.Teams.Count; i++)
                        {
                            if (Players.Teams[i].Contains(player))
                            {
                                TeamManager.teams[i].giveUp += 100f * ((1 / (float)Players.Teams[i].Count) / .6f);
                                TestInputDraw.Color = TeamManager.teams[i].getColor();
                                TestInputDraw.Command = player + " : " + Command;
                                break;
                                //Console.WriteLine(TeamManager.teams[0].giveUp);
                            }
                          
                        }

                    }

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
                            List<int> percentList = new List<int>();





                            //Console.WriteLine(fromCommand);
                            //char fromTeam = string.IsNullOrEmpty(Command) ? ' ' : fromCommand[0];
                            // string fromPlanet = middleMarker == -1? " " : fromCommand.Substring(1,middleMarker -1);
                            //Console.WriteLine(fromPlanet);
                            //int intFromPlanet;
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
                                    string firstPercent = " ";
                                    int Planet = 999;
                                    int Percent = -999;
                                    if (fromCommand.Contains("+"))
                                    {
                                       
                                        firstPlanet = fromCommand.Substring(0, fromCommand.IndexOf("+"));
                                        
                                        fromCommand = fromCommand.Substring(fromCommand.IndexOf("+") + 1);
                                        if (firstPlanet.Contains("."))
                                        {
                                            int index = firstPlanet.IndexOf(".");                                            
                                            firstPercent = firstPlanet.Substring(index+1);
                                            firstPlanet = firstPlanet.Substring(0,index);
                                        }
                                            
                                        
                                        int.TryParse(firstPlanet, out Planet);
                                        
                                        if (int.TryParse(firstPercent[0].ToString(), out Percent))
                                        {
                                            
                                        }
                                        else
                                        {
                                            Percent = -999;
                                        }

                                        
                                        if (Planet != 999)
                                        {
                                            fromCommandList.Add(Planet);
                                            percentList.Add(Percent);
                                        }

                                    }
                                    else
                                    {
                                        
                                        firstPlanet = fromCommand;

                                        Console.WriteLine("fplanet: " + firstPlanet);
                                       
                                        if (firstPlanet.Contains("."))
                                        {
                                            int index = firstPlanet.IndexOf(".");                                            
                                            firstPercent = firstPlanet.Substring(index+1);
                                            firstPlanet = firstPlanet.Substring(0,index);
                                        }
                                        Console.WriteLine("planet:  " + Planet + "Precent: " + Percent);
                                        Console.WriteLine("ffplanet: " + firstPlanet); 
                                        Console.WriteLine("fpercent: " + firstPercent[0].ToString());
                                        
                                        int.TryParse(firstPlanet, out Planet); //breaks at these tryparses.
                                        if (int.TryParse(firstPercent[0].ToString(), out Percent))
                                        {
                                            
                                        }
                                        else
                                        {
                                            Percent = -999;
                                        }
                                       
                                        
                                        if (Planet != 999)
                                        {
                                            fromCommandList.Add(Planet);
                                            percentList.Add(Percent);
                                        }
                                            
                                        
                                        

                                        getFromPlanets = false;

                                       
                                    }
                                }

                                for (int i = 0; i < fromCommandList.Count; i++)
                                {
                                    Console.WriteLine("fcl: " + fromCommandList.Count);
                                    try
                                    {
                                        List<Planet> fPlanet = EntityManager.planets.Where(x => x.ID == fromCommandList[i]).ToList();
                                        List<Team> fromTeam = TeamManager.teams.Where(x => x.ID == fPlanet[0].team.ID).ToList();
                                        List<Planet> tPlanet = EntityManager.planets.Where(x => x.ID == toPlanet).ToList();
                                        int playerTeam = 999;
                                        Console.WriteLine("planet team : " + fPlanet[0].team.ID + "playerTeam : " + playerTeam);
                                        Console.WriteLine("pl: " + percentList[i]);
                                        bool checkTeams = true;
                                        bool playerOnTeam = false;
                                        while (checkTeams)
                                        {
                                            for (int j = 0 ; j < Players.Teams.Count; j++ )
                                            { 
                                                if (Players.Teams[j].Contains(player))
                                                {
                                                    
                                                    playerTeam = j;
                                                    if (percentList[i] == -999)
                                                        percentage = .25f;
                                                    else
                                                        percentage = (float)percentList[i] * .1f;//(float)((1 / (float)Players.Teams[j].Count) / fromCommandList.Count);
                                                    if (percentage == 0)
                                                        percentage = 1f;
                                                    playerOnTeam = true;                                                    
                                                    checkTeams = false;
                                                    break;

                                                }
                                            }
                                            Console.WriteLine("planTEAM: " + fPlanet[0].team.ID);
                                            if (!playerOnTeam && fPlanet[0].team.ID != TeamManager.biggestTeam)
                                            {
                                                for (int j = 0; j < Players.Teams.Count; j++)
                                                {

                                                    if (fPlanet[0].team.ID == j)
                                                    {
                                                        Players.Teams[j].Add(player);
                                                        playerTeam = j;
                                                        playerOnTeam = true;
                                                        if (percentList[i] == -999)
                                                            percentage = .1f;
                                                        else
                                                            percentage = (float)percentList[i] * .1f;//(float)((1 / (float)Players.Teams[j].Count) / fromCommandList.Count);
                                                        if (percentage == 0)
                                                            percentage = 1f;

                                                        checkTeams = false;
                                                        break;

                                                    }

                                                }
                                            }
                                            checkTeams = false;

                                        }
                                        //Console.WriteLine("planet team : " + fPlanet[0].team.ID + "playerTeam : " + playerTeam + " Percentage: "+percentage);
                                        if (fPlanet[0].team.ID == playerTeam && playerOnTeam)
                                        {
                                            ShipManager.sendShips(fromTeam[0], percentage, fPlanet[0], tPlanet[0]);
                                            TestInputDraw.Color = fromTeam[0].getColor();
                                            TestInputDraw.Command = player + " : " + Command;
                                        }
                                        
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (fromCommand.Contains("*") && !fromCommand.Contains("+"))
                            {
                                percentage = 999;
                                int playerTeam = 999;
                                                          
                                for (int j = 0 ; j < Players.Teams.Count; j++)
                                {
                                    if (Players.Teams[j].Contains(player))
                                    {
                                        playerTeam = j;
                                        percentage = .25f;//(float)((1 / (float)Players.Teams[j].Count) / fromCommandList.Count);
                                        //Console.WriteLine("percentage :" + percentage);
                                        
                                        break;

                                    }
                                }
                                   
                                   
                                     
                                    
                                
                                if (playerTeam != 999 )
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
                                            if (fromTeam[0].canSendAll)
                                            {                                                
                                                foreach (Planet planet in fromTeam[0].planetsColonized)
                                                {
                                                    ShipManager.sendShips(fromTeam[0], (float)(percentage), planet, tPlanet[0]);
                                                }
                                                TestInputDraw.Color = fromTeam[0].getColor();
                                                TestInputDraw.Command = player + " : " + Command;
                                                fromTeam[0].canSendAll = false;
                                                fromTeam[0].canSendAllDelay = 20; 
                                              
                                            }
                                           
                                        }
                                        catch { }
                                    }
                                }


                            }






                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e.Message);
                        }
                    }
                    
                    

                }
            }
            catch (Exception e)
            {
               // Console.WriteLine(e.Message);
            }
            

        }




    }
}

