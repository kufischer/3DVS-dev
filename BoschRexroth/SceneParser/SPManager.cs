using FIVES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneParser
{
    public class SPManager
    {
        public static SPManager Instance;

        public Dictionary<string, Entity> DynamicEntities { get; private set; }

        public void CreateEntities()
        {
            createScenery();
            createDynamicEntities();
        }

        private void createScenery()
        {
            Entity scenery = new Entity();
            var sceneryConfig = SPConfiguration.Configuration.Scenery;
            scenery["mesh"]["uri"].Suggest(sceneryConfig.Source);
            scenery["mesh"]["scale"].Suggest(new Vector(
                sceneryConfig.ScaleFactor,
                sceneryConfig.ScaleFactor,
                sceneryConfig.ScaleFactor));

            scenery["location"]["position"].Suggest(sceneryConfig.Offset);

            World.Instance.Add(scenery);
        }

        private void createDynamicEntities()
        {
            DynamicEntities = new Dictionary<string, Entity>();
            foreach (SPConfiguration.EntityConfiguration e in SPConfiguration.Configuration.Entities)
            {
                // Child Entities will be added by traversing the child lists of parents and thus must not be added yet
                if (e.Parent != null)
                    continue;

                addDynamicEntity(e);
            }
        }

        private void addDynamicEntity(SceneParser.SPConfiguration.EntityConfiguration entity, Entity parent = null)
        {
            Entity e = new Entity();
            e["mesh"]["uri"].Suggest(entity.Source);
            e["mesh"]["scale"].Suggest(new Vector(entity.ScaleFactor, entity.ScaleFactor, entity.ScaleFactor));

            if (parent != null)
            {
                var p = (Vector)parent["location"]["position"].Value;
                e["location"]["position"].Suggest(FIVES.Math.AddVectors(p, entity.Position));
                e["scene"]["parent"].Suggest(parent.Guid);
            }
            else
            {
                e["location"]["position"].Suggest(entity.Position);
            }

            foreach (SPConfiguration.EntityConfiguration c in entity.Children)
            {
                addDynamicEntity(c, e);
            }

            World.Instance.Add(e);
            string[] names = entity.Name.Split(',');
            foreach (string name in names)
            {
                DynamicEntities.Add(name, e);
            }
        }
    }
}
