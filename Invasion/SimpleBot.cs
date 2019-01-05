using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invasion
{
    public class SimpleBot
    {
        private Team currentTeam = null;
        private string botName = "SimpleBot";
        private Random rand = new Random();
        private int ID;
 
        public SimpleBot(int id)
        {
            ID = id;
            int seed = (int)System.Convert.ToInt32((7^(id*Guid.NewGuid().GetHashCode())).GetHashCode().ToString().ToCharArray()[0]);
           // int seed = Random // Need a more random seed to guarantee bots behave differently
            rand = new Random(seed);
            Console.WriteLine("{0}", seed);
           
           
        }
        private void SelectTeam()
        {
            currentTeam = TeamManager.getSmallestTeam();
            botName += ID.ToString();
            Players.Teams[currentTeam.ID].Add(botName);
        }

        private Planet getFromPlanet()
        {
            Planet from;
            int i;
               
            try
            {
                i = currentTeam.planetsColonized.Count();
                from = currentTeam.planetsColonized[rand.Next(i)];
            }
            catch (ArgumentOutOfRangeException)
            {
                //Selcect a random planet
                i = rand.Next(EntityManager.planets.Count);
                from = EntityManager.planets[i];
            }

            return from;
        }

        private Planet getToPlanet()
        {
           Planet to;
           int i;
           double agressiveness = .5;
           Team targetTeam = null;
           bool selectTarget = true;

           
           if (rand.NextDouble() <= agressiveness)
           {
               // Continue to select a random team until a team != to current is found
               while (selectTarget)
               {
                   i = rand.Next(TeamManager.teams.Count);
                   if (TeamManager.teams[i].ID == currentTeam.ID)
                   {
                       //pass, retry/select a different target team
                   }
                   else
                   {
                       targetTeam = TeamManager.teams[i];
                       selectTarget = false;
                   }
               }
               try
               {
                   i = rand.Next(targetTeam.planetsColonized.Count);
                   to = targetTeam.planetsColonized[i];
               }
               catch(ArgumentOutOfRangeException)
               {
                   //Selcect a random planet
                   i = rand.Next(EntityManager.planets.Count);
                   to = EntityManager.planets[i];
               }

           }
           else
           {
               //Selcect a random planet
               i = rand.Next(EntityManager.planets.Count);
               to = EntityManager.planets[i];
           }   

           return to;
        }

        public void Update()
        {
            double sendRate = .0008;
            float percentage = .25F;
            float playerLimit = 2; //TeamManager.numSbots + 1;
            Planet from;
            Planet to;
            double startDelay = (double)(rand.NextDouble() * 100)*2 + 15;
          
            

            int humanPlayers = Players.PlayerLastCmdDict.Where(kvp => !kvp.Key.Contains("SimpleBot")).Count();

            // Select Team
            if (currentTeam == null & humanPlayers < playerLimit & !TeamManager.gameOver & GameRoot.seconds > startDelay)
            {
                SelectTeam();
                Console.WriteLine("botstartdelay: " + startDelay.ToString());
            }

            // take an action with a probablity of sendRate
            if (currentTeam != null)
            {
                if (rand.NextDouble() <= sendRate & humanPlayers < playerLimit & !TeamManager.gameOver & !currentTeam.isDead)
                {
                    from = getFromPlanet();
                    to = getToPlanet();
                    ShipManager.sendShips(currentTeam, percentage, from, to);
                    Players.UpdatePlayerLastCmd(botName);
                    TestInputDraw.Color = currentTeam.getColor();
                    TestInputDraw.Command = botName + " : " + from.ID.ToString() + "-" + to.ID.ToString();
                }
            }
           
        }




    }
}
