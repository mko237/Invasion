using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invasion
{
    class Players
    {
        public static List<List<String>> Teams = new List<List<String>>(); //{ new List<String>(), new List<String>(), new List<String>(), new List<String>() };   
        public static IDictionary<string, int> PlayerLastCmdDict = new Dictionary<string, int>();
        
        public static void Initialize(int n )
        {
            Teams.Clear();
            PlayerLastCmdDict.Clear();

            for(int i = 0; i < n; i++)
            {
                Teams.Add(new List<String>());
            }
        }

        public static void UpdatePlayerLastCmd(string player)
        {
          int currentMinute;          
          currentMinute= (int)GameRoot.minutes;

          if (PlayerLastCmdDict.Keys.Contains(player))
          {
              PlayerLastCmdDict[player] = currentMinute;
          }
          else
          {
              PlayerLastCmdDict.Add(player, currentMinute);
          }
        }
    }

}
