using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    static class EntityManager
    {
        static List<Entity> addedEntities = new List<Entity>();
        static List<Entity> entities = new List<Entity>();
        public static List<Planet> planets = new List<Planet>();
        public static List<Ship> ships = new List<Ship>();
        public static Tuple<bool,Team> giveUp= new Tuple<bool,Team>(false,null);

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
            //HandleCollisions();

            foreach (var entity in entities)
                entity.Update();
            
            isUpdating = false; 

            foreach (var entity in addedEntities)
               AddEntities(entity);
            
            addedEntities.Clear();
            try
            {
                entities = entities.Where(x => !x.IsExpired).ToList();
            }
            catch(Exception) 
            { 
               
            }

            ships = ships.Where(x => !x.IsExpired).ToList();            
            
            
        }

     
        public static void newLevel()
        {
            planets = new List<Planet>();
            ships = new List<Ship>();
            entities = entities.Where(x => x is Background).ToList();
            TeamManager.Clear();
            Players.Team1 = new List<string>();
            Players.Team2 = new List<string>();
            TeamManager.colorIndex = new List<int> { 0, 1, 2, 3, 4, 5};
            TeamManager.usedColorIndexes = new List<int>();
            LevelSpawner Level = new LevelSpawner(45);
            TeamManager.GenerateTeams(Level, 2);
            Level.Spawn();
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
