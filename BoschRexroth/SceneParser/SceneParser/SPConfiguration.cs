using FIVES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SceneParser
{
    public class SPConfiguration
    {
        #region container objects

        public struct SceneryConfig
        {
            public string Source;
            public float ScaleFactor;
            public Vector Offset;
        }

        public struct CoordinateSystemConfig
        {
            public float UnitFactor;
        }

        public class EntityConfiguration
        {
            public string Name;
            public EntityConfiguration Parent;
            public HashSet<EntityConfiguration> Children = new HashSet<EntityConfiguration>();
            public long SemanticIdentifier = -1;
            public string Source;
            public float ScaleFactor;
            public Vector Position;
        }

        #endregion

        public static SPConfiguration Configuration { get { return configuration; } }
        private static SPConfiguration configuration = new SPConfiguration();

        private XmlDocument configDocument = new XmlDocument();

        public SceneryConfig Scenery = new SceneryConfig();
        public CoordinateSystemConfig CoordinateSystem = new CoordinateSystemConfig();
        public List<EntityConfiguration> Entities = new List<EntityConfiguration>();
        public bool APPLY_JOINT_VALUES { get; private set; }

        public void ReadConfig()
        {
            loadConfigXml();
            readScenery();
            readCoordinateSystem();
            readEntities();
            readJointFlag();
        }

        private void loadConfigXml()
        {
            string pluginConfigPath = this.GetType().Assembly.Location;
            configDocument.Load(pluginConfigPath + ".config");
        }

        private void readScenery()
        {
            var sceneryNode = configDocument.SelectSingleNode("configuration/scenery");
            if (sceneryNode != null)
            {
                Scenery.Source = sceneryNode.Attributes["src"].Value;
                Scenery.ScaleFactor = float.Parse(sceneryNode.Attributes["scalefactor"].Value);
                Scenery.Offset = parseVector(sceneryNode.Attributes["offset"].Value);
                createFurniture(sceneryNode.SelectNodes("item"));
            }
        }

        private void createFurniture(XmlNodeList furnitureItems)
        {
            foreach (XmlNode node in furnitureItems)
            {
                readSingleEntity(node);
            }
        }

        private void readCoordinateSystem()
        {
            var coordNode = configDocument.SelectSingleNode("configuration/coordinatesystem");
            if (coordNode != null)
            {
                CoordinateSystem.UnitFactor = float.Parse(coordNode.Attributes["unitfactor"].Value);
            }
        }

        private void readEntities()
        {
            var entityNode = configDocument.SelectSingleNode("configuration/entities");
            var entities = entityNode.SelectNodes("entity");
            foreach (XmlNode e in entities)
            {
                readSingleEntity(e);
            }
        }

        private EntityConfiguration readSingleEntity(XmlNode entityNode)
        {
            EntityConfiguration conf = new EntityConfiguration();

            if (entityNode.Attributes["name"] != null)
            {
                conf.Name = entityNode.Attributes["name"].Value;
            }

            if (entityNode.Attributes["position"] != null)
            {
                conf.Position = parseVector(entityNode.Attributes["position"].Value);
            }

            if (entityNode.Attributes["src"] != null)
            {
                conf.Source = entityNode.Attributes["src"].Value;
            }

            if (entityNode.Attributes["scalefactor"] != null)
            {
                conf.ScaleFactor = float.Parse(entityNode.Attributes["scalefactor"].Value);
            }

            if (entityNode.Attributes["semantic"] != null)
            {
                conf.SemanticIdentifier = long.Parse(entityNode.Attributes["semantic"].Value);
            }

            if (entityNode.SelectNodes("entity").Count > 0)
            {
                readChildEntities(conf, entityNode.SelectNodes("entity"));
            }

            Entities.Add(conf);
            return conf;
        }

        private void readChildEntities(EntityConfiguration parentConfig, XmlNodeList children)
        {
            foreach (XmlNode c in children)
            {
                var childConfig = readSingleEntity(c);
                childConfig.Parent = parentConfig;
                parentConfig.Children.Add(childConfig);
            }
        }

        private Vector parseVector(string vectorAsString)
        {
            string[] vectorComponents = vectorAsString.Split(' ');
            return new Vector(
                float.Parse(vectorComponents[0].Trim()),
                float.Parse(vectorComponents[1].Trim()),
                float.Parse(vectorComponents[2].Trim())
                );
        }

        private void readJointFlag()
        {
            var jointsNode = configDocument.SelectSingleNode("configuration/joints");
            if (jointsNode != null)
                APPLY_JOINT_VALUES = jointsNode.Attributes["read"].Value.ToUpper() == "TRUE";
        }
    }
}
