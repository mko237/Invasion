using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invasion
{
    class Planet : Entity
    {
        private static Planet instance;
        public static Planet Instance
        {
            get
            {
                if (instance == null)
                    instance = new Planet();
                return instance;
            }
        }

        private Planet()
        {
            //initialize planet here
            image = Art.Planet;
            Position = GameRoot.ScreenSize / 2;
        }

        public override void Update()
        {
            //planet logic here
        }

            
    }
}
