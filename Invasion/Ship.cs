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
        private const float AngSpeed = 0.13f;
        private const float speed = 1.5f;
        float spawnRadius;
        private Planet Destination;
        private float destinationRadius;
        private Planet Origin;

        private static Random rand = new Random();
        private float randomAngle = rand.NextFloat((float)(Math.PI/2), (float)(3*Math.PI/2));//Math.PI);

       

        public Ship(Team team, Planet sourcePlanet, Planet destinationPlanet)
        {
            image = Art.Ship;
            
            Radius = 8;
            Team = team; 
            color = team.getColor();
            ObjectSize = 0.65f;
            spawnRadius = sourcePlanet.Radius + 5;
            Destination = destinationPlanet;
            Origin = sourcePlanet;
            Colliding = true;
            GeneratePosition(sourcePlanet.Position, destinationPlanet.Position);
            goTo(Position, destinationPlanet);

        }

        private void GeneratePosition(Vector2 source, Vector2 dest) 
        {
            if (source.X < dest.X && source.Y > dest.Y) //what do these do?
            {
                Position.X = source.X - spawnRadius * (float)Math.Cos((dest - source).ToAngle() + randomAngle);
                Position.Y = source.Y - spawnRadius * (float)Math.Sin((dest - source).ToAngle() + randomAngle);
            }
            else if (source.X > dest.X && source.Y > dest.Y)
            {
                Position.X = source.X - spawnRadius * (float)Math.Cos((dest - source).ToAngle() + randomAngle);
                Position.Y = source.Y - spawnRadius * (float)Math.Sin((dest - source).ToAngle() + randomAngle);
            }
            else if (source.X > dest.X && source.Y < dest.Y)
            {
                Position.X = source.X - spawnRadius * (float)Math.Cos((dest - source).ToAngle() + randomAngle);
                Position.Y = source.Y - spawnRadius * (float)Math.Sin((dest - source).ToAngle() + randomAngle);
            }
            else if (source.X < dest.X && source.Y < dest.Y)
            {
                Position.X = source.X - spawnRadius * (float)Math.Cos((dest - source).ToAngle() + randomAngle);
                Position.Y = source.Y - spawnRadius * (float)Math.Sin((dest - source).ToAngle() + randomAngle);
            }

            //Console.WriteLine(randomAngle * 180 / Math.PI);

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
            //else
              //  color = Team.getColor(); //getDirection(Position, Destination);

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

            Colliding = false;

            if (Position.WithinRadius(Destination.Position,Destination.Radius+3f))
                IsExpired = true;

            //delete ships that go off screen 
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
        public void HandleCollisionPlanet(Planet planet)
        {
            if(planet.Position != Destination.Position || planet.Position != Origin.Position)
            {
                var d = Position - planet.Position;
                Velocity += 4 * d / (d.LengthSquared() + 1);
                Colliding = true; 
           
            }
               
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, LocalOrientation.ConvertToGlobal(Velocity), Size / 2f, ObjectSize, 0, 0);  
        }

        private void goTo(Vector2 position, Planet destination )
        {
            Vector2 Direction = destination.Position - position;
            Direction.Normalize();
            Velocity = speed * Direction;
        }

        private void getDirection(Vector2 position, Vector2 destination)
        {
            Vector2 Direction = destination - position;
            Direction.Normalize();
            Velocity = speed * Direction;
             
        }
    }
}
