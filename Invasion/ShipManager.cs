using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Invasion
{
    public static class ShipManager
    {
        private static Vector2 Velocity = new Vector2(10,10);
        
       
        
        public static void sendShips(Team team, int numShips, Planet origin, Planet destination)
        {
            
            for(int i = 0; i < numShips; i++)
            {
                Ship ship = new Ship(team, origin);
                ship.goTo(origin, destination);
                
            }
        }
    }
}
