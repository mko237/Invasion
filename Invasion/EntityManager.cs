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

        static bool isUpdating;

        public static int Count { get { return addedEntities.Count; } }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
                entities.Add(entity);
            else
                addedEntities.Add(entity);
        }
        
        public static void Update()
        {
            isUpdating = true;

            foreach (var entity in entities)
                entity.Update();
            
            isUpdating = false; 

            foreach (var entity in addedEntities)
                entities.Add(entity);
            
            addedEntities.Clear();
            entities = entities.Where(x => !x.IsExpired).ToList();
            
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }
    }
}
