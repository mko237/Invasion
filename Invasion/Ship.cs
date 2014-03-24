using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    public class Ship : Entity
    {
        private Team Team;
        private const float AngSpeed = 0.2f;
        private const float speed = 1.5f;
        float spawnRadius;
        private Vector2 lastPosition;
        private Vector2 Direction;
        private Planet Destination;
        private Planet Origin;

        private static Random rand = new Random();
        private float randomAngle = rand.NextFloat((float)(Math.PI/2), (float)(3*Math.PI/2));//Math.PI);

       

        public Ship(Team team, Planet sourcePlanet, Planet destinationPlanet)
        {
            image = Art.Ship;
     
            Radius = 8;
            Team = team;
            Direction = destinationPlanet.Position - sourcePlanet.Position;
            color = team.getColor();
            ObjectSize = 0.65f;
            spawnRadius = sourcePlanet.Radius + 5;
            Destination = destinationPlanet;
            Origin = sourcePlanet;
            Colliding = true;
            GeneratePosition(sourcePlanet.Position, destinationPlanet.Position);
            getDirection(Position, Destination.Position);
        }

        private void GeneratePosition(Vector2 source, Vector2 dest) 
        {
            Position.X = source.X - spawnRadius * (float)Math.Cos((dest - source).ToAngle() + randomAngle);
            Position.Y = source.Y - spawnRadius * (float)Math.Sin((dest - source).ToAngle() + randomAngle);

            Orientation = (Position - source).ToAngle();
            LocalOrientation = randomAngle;
        }

        public Team getTeam()
        {
            return Team;
        }

        public override void Update()
        {
            if (!Colliding)
                getDirection(Position, Destination.Position); //color = Color.Red;

            if (Velocity.LengthSquared() > 0) //what is this check for?
            {
                if (LocalOrientation > 0)
                {
                    if (LocalOrientation < Math.PI) 
                        LocalOrientation += AngSpeed;
                    else 
                        LocalOrientation -= AngSpeed;
                }
                else if (LocalOrientation < 0)
                {
                    if (Math.Abs(LocalOrientation) < Math.PI)
                        LocalOrientation -= AngSpeed;
                    else
                        LocalOrientation += AngSpeed;
                }
            }

            Position += Velocity;

            lastPosition = Position;
            Colliding = false;

            if (Position.WithinRadius(Destination.Position, Destination.Radius))
            {
                if (Destination.team == null)
                {
                    Destination.shipCount--;
                    if (Destination.shipCount <= 0)
                    {
                        Destination.changeTeams(Team);
                    }
                }
                else if (Destination.team.ID == Team.ID)
                    Destination.shipCount++;
                else if(Destination.team.ID != Team.ID)
                {
                    Destination.shipCount--;
                    if (Destination.shipCount <= 0)
                    {
                        Destination.changeTeams(Team);
                    }
                }
                IsExpired = true;
            }

            //delete ships that go off screen 
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
        public void HandleCollisionPlanet(Planet planet)
        {
            if (planet.ID != Destination.ID && planet.ID != Origin.ID)
            {
                var d = Position - planet.Position;
                Velocity += (3f / ObjectSize) * d / (d.LengthSquared() + 1);
                var r = Position - lastPosition;
                LocalOrientation = r.ToAngle();
                Colliding = true;
            }
        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, LocalOrientation.ConvertToGlobal(Velocity), Size / 2f, ObjectSize, 0, 0);  
        }

        private void getDirection(Vector2 position, Vector2 destination)
        {
            Direction = destination - position;
            Direction.Normalize();
            Velocity = speed * Direction;
             
        }
    }
}
