using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invasion
{
    static class InputParser
    {
        private static String Command = null;
        
        
        
        
        public static void Update()
        {
            Command = InputDisplay.Command;
            if(Command != null)
            {
                //ShipManager.sendShips(GameRoot.Instance.team1, 4, GameRoot.Instance.team1.getHomePlanet(), GameRoot.Instance.team2.getHomePlanet());
            }
        }
    }
}
