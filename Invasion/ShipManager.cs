using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Invasion
{
    public static class ShipManager
    {  
        public static void sendShips(Team team, float percentageOfShips, Planet origin, Planet destination)
        {
            
            if (origin.ID != destination.ID)
            {
                int TotalShips = TeamManager.teamShipCount[0] + TeamManager.teamShipCount[1];
                float value = (float)Math.Pow((double)(.0001f * TotalShips), (double)2) + 1; // this should be a log/exponential function so we can cap how many ships go out... need help implementing.

                int numShips = (int)(origin.shipCount * percentageOfShips*(1/value));

                if (numShips <= 0)
                {
                    numShips = 1;
                }

                if (numShips >= origin.shipCount)
                {

                    numShips = (int)Math.Floor((double)origin.shipCount);
                    for (int i = 0; i < numShips; i++)
                    {
                       
                        Ship ship = new Ship(team, origin, destination, 1);
                        EntityManager.Add(ship);                        

                    }

                }
                else
                {
                    if (value < origin.shipCount)
                    {
                        for (int i = 0; i < numShips; i++)
                        {
                            Ship ship = new Ship(team, origin, destination, value);
                            EntityManager.Add(ship);
                        }
                    }
                    else
                    {
                        for (int i = 0; i <origin.shipCount; i++)
                        {
                            Ship ship = new Ship(team, origin, destination, 1f);
                            EntityManager.Add(ship);
                        }
                    }
                    
                }
                
            }
        }
    }
}
