using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Invasion
{
    public class Ship : Entity
    {
        public Team Team;
        private const float AngSpeed = 0.2f;
        private const float speed = 1.5f;
        float spawnRadius;
        private Vector2 lastPosition;
        private Vector2 nextPosition;
        private Planet nextPlanet;
        private Vector2 Direction;
        private Planet Destination;
        private Planet Origin;
        public float Value;

        private static Random rand = new Random();
        private float randomAngle = rand.NextFloat((float)(Math.PI/2), (float)(-Math.PI/2));
        private List<Vector2> pathPoints = new List<Vector2>();
        private bool checkingAhead = true;
        private int i = 0;
        private int j = 1;

        List<Planet> planetsNotCrossed = EntityManager.planets;
        //Texture2D markerImage;

       

        public Ship(Team team, Planet sourcePlanet, Planet destinationPlanet,float value)
        {
            image = Art.Ship;
     
            Radius = 8;
            Team = team;
            Direction = destinationPlanet.Position - sourcePlanet.Position;
            color = team.getColor();
            ObjectSize = 0.65f;
            Value = value;
            spawnRadius = sourcePlanet.Radius + Radius;
            Destination = destinationPlanet;
            Origin = sourcePlanet;
            //Colliding = true;
            GeneratePosition(sourcePlanet.Position, destinationPlanet.Position);
            //getDirection(Position, Destination.Position);
            //getDirection();
            Origin.shipCount -= Value;
            planetsNotCrossed = planetsNotCrossed.Where(planet => planet.ID != sourcePlanet.ID).ToList();
            GeneratePathPoints(); 
            //markerImage = Art.Marker;
            //pathPoints.ReconstructPath(sourcePlanet, destinationPlanet, 250);
        }

        private void GeneratePosition(Vector2 source, Vector2 dest) 
        {
            Position.X = source.X + spawnRadius * (float)Math.Cos((dest - source).ToAngle() + randomAngle);
            Position.Y = source.Y + spawnRadius * (float)Math.Sin((dest - source).ToAngle() + randomAngle);

            LocalOrientation = randomAngle;
        }

        //public Team getTeam()
        //{
        //    return Team;
        //}

        public override void Update()
        {
            if (!Colliding)
                getDirection();// getDirection(Position, Destination.Position);

            if (Velocity.LengthSquared() > 0) 
            {
                if (LocalOrientation > 0)
                    LocalOrientation -= AngSpeed;
                else if (LocalOrientation < 0)
                    LocalOrientation += AngSpeed;

                if (Math.Abs(LocalOrientation) < 0.1f)
                    LocalOrientation = 0;
            }

            Position += Velocity;

            lastPosition = Position;
            Colliding = false;

            if (Position.WithinRadius(Destination.Position, Destination.Radius))
            {
                if (Destination.team == null || Destination.team.ID != Team.ID)
                {
                    if (Destination.shipCount > 0)
                    {
                        Destination.invadingTeam = Team;
                        Destination.enemyShipCount += Value;


                        if (Destination.shipCount - Value < 0)
                        {
                            Destination.changeTeams();
                            Destination.shipCount = Value - Destination.shipCount;
                        }
                        else
                            Destination.shipCount -= Value;
                        
                        
                    } 
                    //create sound
                    Cue cue = GameRoot.soundBank.GetCue("Woodblock-04");
                    cue.Play();
                    //particle creation below
                    for (int i = 0; i < 6; i++)
                    {
                        float speed = 1.8f * (1f - 1 / rand.NextFloat(1, 10));
                        var state = new ParticleState()
                        {
                            Velocity = rand.NextVector2(speed, speed),
                            Type = ParticleType.Ship,
                            LengthMultiplier = 1
                        };

                        //Color colorr = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                        GameRoot.ParticleManager.CreateParticle(Art.SquareParticle, Position, color, 190f, .5f, state);
                    }
                }
                else if (Destination.team.ID == Team.ID)
                {
                    Destination.shipCount += Value;
                    if(Destination.enemyShipCount > 0)
                        Destination.enemyShipCount -= Value;
                }
                   
                            
                
                IsExpired = true;
            }

            //delete ships that go off screen 
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
        public void HandleCollisionPlanet(Planet planet)
        {
            //if (planet.ID != Destination.ID && planet.ID != Origin.ID)
            //{
            //    var d = Position - planet.Position;
            //    Velocity += (3f / ObjectSize) * d / (d.LengthSquared() + 1);
            //    var r = Position - lastPosition;
            //    LocalOrientation = r.ToAngle();
            //    Colliding = true;
            //}
        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            //foreach (Vector2 point in pathPoints)
            //    spriteBatch.Draw(markerImage, point, null, color, 0, Size / 2f, 1.0f, 0, 0);
            spriteBatch.Draw(image, Position, null, color, LocalOrientation.ConvertToGlobal(Velocity), Size / 2f, ObjectSize, 0, 0);  
        }

        //private void getDirection(Vector2 position, Vector2 destination)
        //{
        //    //Direction = destination - position;
        //    //Direction.Normalize();
        //    //Velocity = speed * Direction;
        //}
        private void getDirection()
        {
             if ((pathPoints[j] - Position).LengthSquared() < 2 * 2)
                j++;

            Direction = pathPoints[j] - Position;
            Direction.Normalize();
            Velocity = speed * Direction;
        }
            
        private void GeneratePathPoints()
        {
            pathPoints.Add(Position);

            while (checkingAhead)
            {
                getNextPosition(pathPoints[i], Destination.Position, Origin, Destination);
                i++;
            }
            pathPoints.Add(Destination.Position);
        }

        private void getNextPosition(Vector2 position, Vector2 dest, Planet origin, Planet destination)
        {
            Vector2 finalPoint = dest;
            Planet finalPlanet = destination;
            foreach (Planet planet in planetsNotCrossed)
            {
                Rectangle bound = new Rectangle(position.X < dest.X ? (int)(position.X - 50) : (int)(dest.X - 50), position.Y < dest.Y ? (int)(position.Y - 50) : (int)(dest.Y - 50), (int)Math.Abs(position.X - dest.X) + 100, (int)Math.Abs(position.Y - dest.Y) + 100);
                if (planet.ID != destination.ID && planet.ID != origin.ID && planet.Position.X > bound.Left && planet.Position.X < bound.Right && planet.Position.Y > bound.Top && planet.Position.Y < bound.Bottom)
                {
                    float slope = (dest.Y - position.Y) / (dest.X - position.X);
                    float b2 = (-1 / slope) * (-planet.Position.X) + planet.Position.Y;
                    float b1 = slope * (-position.X) + position.Y;
                    float y = (slope * slope * b2 + b1) / (1 + slope * slope);
                    float x = -slope * (y - b2);

                    Vector2 possiblePoint = new Vector2(x, y);
                    if ((possiblePoint - planet.Position).LengthSquared() < (planet.Radius + 2 * Radius) * (planet.Radius + 2 * Radius))
                    {
                        int randomSide = rand.Next(0, 100);
                        Vector2 direction = new Vector2(0, 0);

                        if (randomSide % 2 == 0)
                            direction = possiblePoint - planet.Position;
                        else if (randomSide % 2 == 1)
                            direction = planet.Position - possiblePoint;

                        direction.Normalize();

                        Vector2 moreLikelyPoint = new Vector2(planet.Position.X + (direction * (planet.Radius + 2 * Radius)).X, planet.Position.Y + (direction * (planet.Radius + 2 * Radius)).Y);
                        if ((moreLikelyPoint - position).LengthSquared() < (finalPoint - position).LengthSquared())
                        {
                            finalPoint = moreLikelyPoint;
                            finalPlanet = planet;
                        }

                    }
                }
            }
                if (finalPoint != dest)
            {
                nextPosition = finalPoint;
                nextPlanet = finalPlanet;

                getNextPosition(position, nextPosition, origin, nextPlanet);
            }
            else
            {
                if (dest != Destination.Position)
                {
                    checkingAhead = true;

                    pathPoints.Add(nextPosition);
                    planetsNotCrossed = planetsNotCrossed.Where(planet => planet.ID != destination.ID).ToList();
                    Origin = destination;
                }
                else
                    checkingAhead = false;
            }
        }
    }
}
