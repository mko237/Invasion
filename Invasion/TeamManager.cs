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
        public static int[] teamShipCount { get; private set; }
        public static float[] teamProductionRate { get; private set; }

        private static bool[] justDied;
        private static bool[] lastIsDead;
        public static List<Tuple<String, Color>> Colors = new List<Tuple<String, Color>> 
        { 
            new Tuple<String, Color>("RED", Color.DarkRed), 
            new Tuple<String, Color>("BLUE",Color.DeepSkyBlue),
            new Tuple<String, Color>("ORANGE",Color.Orange),          
            new Tuple<String, Color>("PINK",Color.HotPink), 
            new Tuple<String, Color>("GREEN",Color.LawnGreen), 
            new Tuple<String, Color>("PURPLE", new Color(100, 100, 255)),
             
        };
        public static List<int> colorIndex = new List<int> { 0, 1, 2 , 3, 4, 5};
        public static List<int> usedColorIndexes = new List<int>();
        public static int winWait = 0;
        private static Team winningTeam;
        public static bool gameOver = false;
        private static int second;
        private static int lastsecond;
        public static int biggestTeam = -1;

        public static List<SimpleBot> bots = new List<SimpleBot>();
        public static int numbots = 1; 

        public static void GenerateTeams(LevelSpawner level, int n)
        {
            for (int i = 0; i < n; i++)
                teams.Add(new Team(i));

            teamShipCount = new int[n];
            teamProductionRate = new float[n];
            justDied = new bool[n];
            lastIsDead = new bool[n];

            Team.GenerateHomePositions(n);

            int j = 0;
            Planet[] homePlanets = new Planet[n];
            foreach (Team team in TeamManager.teams)
            {
                homePlanets[j] = team.getHomePlanet();
                j++;
            }

            level.AddHomePlanets(homePlanets);
            Players.Initialize(n);

            // Add bots
            for (int i = 0; i < numbots; i++)
            {
                bots.Add(new SimpleBot(i));
            }
        }

        public static void getShipCount()
        {
            
            int i = 0;
            int shipCount = new int();
           
            foreach (Team team in TeamManager.teams)
            {
                if (team.isDead)
                {
                   
                }
                else
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
                    teamShipCount[i] = shipCount + shipsFlying;                   
                }
                i++;
            }
        }
        public static void getProductionRate()
        {
            int i = 0;
            float productionRate = new float();

            foreach (Team team in TeamManager.teams)
            {
                if (team.isDead)
                {
                    
                }
                else
                {
                    productionRate = 0;
                    foreach (Planet p in team.planetsColonized)
                        productionRate += p.productionRate;
                    teamProductionRate[i] = productionRate;
                }
                i++;
            }
        }

        public static float getTotalShipCount()
        {

            float totalShipCount = 0;
            for (int i = 0; i < teams.Count; i++)
                totalShipCount += teamShipCount[i];
            return totalShipCount;
        }

        public static void sendAllDelay()
        {
            second = (int)GameRoot.seconds;
            

            foreach (Team team in teams)
            {
                if (!team.canSendAll)
                {
                    if(team.canSendAllDelay>0)
                    {
                        if (second != lastsecond)
                        {
                            team.canSendAllDelay -= 1;
                        }
                    }
                    else
                    {
                        team.canSendAll = true;
                        team.canSendAllDelay = 0;
                    }
                }
            }
            lastsecond = second;
        }

        private static void getBiggestTeam()
        {
           bool tie = false;
           int maxCount = -1;
           int bTeam = -1;
           for(int i=0; i<Players.Teams.Count; i++)
           {
               if (Players.Teams[i].Where(plyr=> !plyr.Contains("SimpleBot")).Count() > maxCount)
               {
                   maxCount = Players.Teams[i].Count;
                   bTeam = i;  
               }
               else if (Players.Teams[i].Where(plyr => !plyr.Contains("SimpleBot")).Count() == maxCount)
               {
                   tie = true;
               }
                   
           }
           if(!tie)
           {
               biggestTeam = bTeam;
           }
           else
           {
               biggestTeam = -1;
           }
        }

        public static Team getSmallestTeam()
        {
           // bool tie = false;
            int minCount = 999;
            int sTeam = 0;
            for (int i = 0; i < Players.Teams.Count; i++)
            {
                if (Players.Teams[i].Count < minCount)
                {
                    minCount = Players.Teams[i].Count;
                    sTeam = i;
                }
                else if (Players.Teams[i].Count == minCount)
                {
                 //   tie = true;
                }

            }

            return teams[sTeam];
        }

        public static void removeIdlePlayers()
        {
            
            int idleMinutesPeriod = 3;
            int currentMinute = (int)GameRoot.minutes;
            List<string> idlePlayers = new List<string>();
            //Console.WriteLine(currentSecond);
            foreach (string player in Players.PlayerLastCmdDict.Keys)
            {
                if (currentMinute - Players.PlayerLastCmdDict[player] >= idleMinutesPeriod)
                {
                    //Remove player from Players.Teams list
                    foreach (List<String> team in Players.Teams)
                    {
                        team.RemoveAll(plyr => plyr == player);
                    }

                    //Mark for removal
                    idlePlayers.Add(player);
                }

            }
            // Also Remove from PlayerLastCmdDict
            foreach (string idleplayer in idlePlayers)
            {
                Players.PlayerLastCmdDict.Remove(idleplayer);
                Console.Write("Removing: ");
                Console.WriteLine(idleplayer);
            }


        }

        public static void Update()
        {

            getShipCount();
            getProductionRate();
            sendAllDelay();
            getBiggestTeam();
            removeIdlePlayers();
            
            //update bots
            for (int i = 0; i < numbots; i++ )
            {
                bots[i].Update();
            }
                //determines win condition
                //gameOver = true;

                //int teamsWithPlanets = 0;


            //winningTeam = teams[0];
            for (int i = 0; i < teams.Count; i++)//kill game is give up reaches 100
            {
                if (teams[i].giveUp >= 100f)
                {
                    //Console.WriteLine("GIve up is over 100f should end now");
                    teams[i].isDead = true;
                    teams[i].gaveUp = true;

                }

            }

            if (!gameOver)
            {
                int teamsAlive = 0;
                for (int i = 0; i < teamShipCount.Count(); i++)
                {
                    if (teamShipCount[i] <= 0)
                    {
                        teams[i].isDead = true;
                        
                    }

                }
                for (int i = 0; i < teams.Count; i++ )
                {
                    if(!teams[i].isDead)
                    {
                        teamsAlive++;
                        winningTeam = teams[i];
                    }
                }
                //foreach (Team team in teams)
                //{
                //    if (team.planetsColonized.Count > 0)
                //    {
                //        teamsWithPlanets++;
                //        winningTeam = team;
                //    }

                //}

                for (int i = 0; i < teams.Count; i++ )
                {
                    if (!lastIsDead[i] && teams[i].isDead)
                    {
                        justDied[i]= true;
                    }
                    if (justDied[i])
                    {
                        int count = teams[i].planetsColonized.Count;
                        int planetsremoved = 0;
                        for (int j = 0; j<count; j++)
                        {
                            teams[i].planetsColonized[j - planetsremoved].removeTeam();
                            removePlanet(teams[i], teams[i].planetsColonized[j - planetsremoved]);                     
                            planetsremoved++;
                        }
                        teamShipCount[i] = 0;
                        teamProductionRate[i] = 0;
                        justDied[i] = false;
                    }

                    lastIsDead[i] = teams[i].isDead;
                }

                    gameOver = (teamsAlive == 1);

            }

            

                     
            if(gameOver)
            {


                //Update LeaderBoard once, on game completion.
                if (!LeaderBoard.isUpdated)
                {

                    for (int i = 0; i < Players.Teams.Count; i++)
                    {
                        //Update total_game count
                        foreach (var player in Players.Teams[i])
                        {
                            LeaderBoard.updateTotalGames(player);

                        }
                        //Update total_win count
                        if (i == winningTeam.ID)
                        {
                            foreach (var player in Players.Teams[i])
                            {
                                LeaderBoard.updateTotalWins(player);
                            }
                        }
                    }

                    LeaderBoard.isUpdated = true;
                    Console.WriteLine("leader board true");
                }

                WinScreen.winTeam = winningTeam;
                if(winWait<400)
                {
                    winWait++;
                    WinScreen.drawScreen = true;
                }
                else
                {
                    winWait = 0;
                    gameOver = false;
                    WinScreen.drawScreen = false;                    
                    EntityManager.newLevel();
                }
                if (winWait == 399)
                {
                    WinScreen.teamPosition = new Vector2(-50, GameRoot.ScreenSize.Y / 2 - 50);
                    WinScreen.victoryPosition = new Vector2(GameRoot.ScreenSize.X + 350, GameRoot.ScreenSize.Y / 2 + 150);
                }
                    WinScreen.Update();
            }

            //refreshes total team production rate and ship count 
            
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
            bots = new List<SimpleBot>();
            LeaderBoard.isUpdated = false;
            Console.WriteLine("leader board false");
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
