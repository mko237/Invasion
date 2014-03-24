using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Invasion
{
    static class EntityManager
    {
        static List<Entity> addedEntities = new List<Entity>();
        static List<Entity> entities = new List<Entity>();
        public static List<Planet> planets = new List<Planet>();
        static List<Ship> ships = new List<Ship>();

        static bool isUpdating;

        public static int Count { get { return addedEntities.Count; } }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntities(entity);
            else
                addedEntities.Add(entity);
        }
        static void AddEntities(Entity entity)
        {
            entities.Add(entity);
            if(entity is Planet)
                planets.Add(entity as Planet);
            else if(entity is Ship)
                ships.Add(entity as Ship);
        }

        static void HandleCollisions()
        {
            //collisions between ships and planets
            for (int i = 0; i < ships.Count; i++)
            {
                for(int j = 0; j < planets.Count; j++)
                {
                    Entity ship = ships[i];
                    Entity planet = planets[j];
                    if (IsColliding(ref ship, ref planet))
                        ships[i].HandleCollisionPlanet(planets[j]);
                }
            }

            //collisions between ships and ships
        }
        public static void Update()
        {
            isUpdating = true;
            HandleCollisions();

            foreach (var entity in entities)
                entity.Update();
            
            isUpdating = false; 

            foreach (var entity in addedEntities)
               AddEntities(entity);
            
            addedEntities.Clear();
            entities = entities.Where(x => !x.IsExpired).ToList();
            ships = ships.Where(x => !x.IsExpired).ToList();
            
        }
        private static bool IsColliding(ref Entity a, ref Entity b)
        {
            float radius = a.Radius + b.Radius;
            bool collisionDetected = !a.IsExpired && !b.IsExpired && a.Position.WithinRadius(b.Position, radius);
            //a.Colliding = collisionDetected;
            //b.Colliding = collisionDetected;

            return collisionDetected;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }
    }
}
