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
        private const float AngSpeed = .002f;
        private const float speed = 1.5f;
        float spawnRadius;
        private Vector2 lastPosition;
        private Vector2 Direction;
        private Planet Destination;
        private Planet Origin;

        private static Random rand = new Random();
        private float randomAngle = rand.NextFloat((float)(Math.PI/2), (float)(-Math.PI/2));

        private bool wasColliding;
        private int collisions; // used to add a buffer when it collides, to change the behaviour of rotation if this is greater than 0.

       

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
            Position.X = source.X + spawnRadius * (float)Math.Cos((dest - source).ToAngle() + randomAngle);
            Position.Y = source.Y + spawnRadius * (float)Math.Sin((dest - source).ToAngle() + randomAngle);

            LocalOrientation = randomAngle;
        }

        public Team getTeam()
        {
            return Team;
        }

        public override void Update()
        {
            if (!Colliding)
<<<<<<< HEAD
                getDirection(Position, Destination.Position); //color = Color.Red;
            

            if (Velocity.LengthSquared() > 0 && collisions <= 0) //what is this check for?
=======
                getDirection(Position, Destination.Position);

            if (Velocity.LengthSquared() > 0) 
>>>>>>> origin/master
            {
                if (LocalOrientation > 0)
                    LocalOrientation -= AngSpeed;
                else if (LocalOrientation < 0)
<<<<<<< HEAD
                {
                    if (Math.Abs(LocalOrientation) < Math.PI)
                        LocalOrientation -= AngSpeed;
                    else
                        LocalOrientation += AngSpeed;
                }
                
            }

            if (collisions>0)
            {
                var r = Position - lastPosition;
                adjustLocOrientation(r.ToAngle());
                collisions--;

=======
                    LocalOrientation += AngSpeed;

                if (Math.Abs(LocalOrientation - Direction.ToAngle()) < 0.1f)
                    LocalOrientation = 0;
>>>>>>> origin/master
            }

            Position += Velocity;

            lastPosition = Position;
            wasColliding = Colliding;
            Colliding = false;

            if (Position.WithinRadius(Destination.Position, Destination.Radius))
            {
                if (Destination.team == null || Destination.team.ID != Team.ID)
                {
                    Destination.enemyShipCount++;
                    Destination.shipCount--;
                    Destination.invadingTeam = Team;
                }
                else if (Destination.team.ID == Team.ID)
                    Destination.shipCount++;

                IsExpired = true;
            }

            //delete ships that go off screen 
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
        private bool justCollided()
        {
            return (wasColliding && !Colliding);
        }

        private void adjustLocOrientation(float angle)
        {
            
            if (LocalOrientation > angle)
            {
                LocalOrientation -= AngSpeed;
            }
            else if (LocalOrientation < angle)
            {
               
                LocalOrientation += AngSpeed;
            }

           
        }
        public void HandleCollisionPlanet(Planet planet)
        {
            if (planet.ID != Destination.ID && planet.ID != Origin.ID)
            {
                var d = Position - planet.Position;
                Velocity += (3f / ObjectSize) * d / (d.LengthSquared() + 1);
                var r = Position - lastPosition;
                adjustLocOrientation(r.ToAngle().ConvertToGlobal(Velocity));
                Colliding = true;
                collisions +=1;
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
            var r = Position - lastPosition;
            LocalOrientation = r.ToAngle();
             
        }
    }
}
