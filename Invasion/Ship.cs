using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Invasion
{
    class Ship :Entity
    {
        private Team Team;
        private Vector2 Direction;
        private const int speed = 8;
        private Random rand;
        public Ship(Team team, Planet sourcePlanet)
        {
            image = Art.Ship;
            
            Radius = 3;
            float spawnMargin = sourcePlanet.Radius + 3;
            Team = team;
            var randomAngle = rand.NextDouble()*2*Math.PI;
            Position.X = sourcePlanet.Position.X + spawnMargin * (float)Math.Cos(randomAngle);
            Position.Y = sourcePlanet.Position.X - spawnMargin * (float)Math.Cos(randomAngle);
            Direction = Position - sourcePlanet.Position;
        }

        public override void Update()
        {
            
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Direction;

            //delete ships that go off screen 
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }

        public void goTo(Planet origin, Planet destination )
        {
            Direction = Position - destination.Position;
            Velocity = speed * Direction;
        }
    }
}
