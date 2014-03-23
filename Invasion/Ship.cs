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
        private const float AngSpeed = 0.03f;
        private const float speed = 1.5f;
        float spawnRadius;

        private static Random rand = new Random();
        private float randomAngle = rand.NextFloat((float)(-Math.PI), (float)Math.PI);

        public Ship(Team team, Planet sourcePlanet, Planet destinationPlanet)
        {
            image = Art.Ship;
            
            Radius = 3;
            Team = team; 
            color = team.getColor();
            ObjectSize = 0.65f;
            spawnRadius = sourcePlanet.Radius + 15;
            GeneratePosition(sourcePlanet.Position, destinationPlanet.Position);
            goTo(Position, destinationPlanet);
        }

        private void GeneratePosition(Vector2 source, Vector2 dest) 
        {
            if (source.X < dest.X && source.Y > dest.Y)
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

            Console.WriteLine(randomAngle * 180 / Math.PI);

            Orientation = (Position - source).ToAngle();
            LocalOrientation = randomAngle;
        }

        public Team getTeam()
        {
            return Team;
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
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

            //delete ships that go off screen 
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
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
    }
}
