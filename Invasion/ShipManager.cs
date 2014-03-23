using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Invasion
{
    public static class ShipManager
    {  
        public static void sendShips(Team team, int numShips, Planet origin, Planet destination)
        {      
            for(int i = 0; i < numShips; i++)
            {
                Ship ship = new Ship(team, origin, destination);
                EntityManager.Add(ship);
            }
        }
    }
}
