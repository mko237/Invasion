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
            Command = InputDisplay.Command;
            if(Command != null)
            {
                ShipManager.sendShips(Team.teams[0], 1, Team.teams[0].getHomePlanet(), Team.teams[1].getHomePlanet());
            }
        }
    }
}
