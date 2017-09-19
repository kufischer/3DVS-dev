using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using FIVES;

namespace CustomizedAnimationPlugin
{
    class CustomizedAnimation
    {
        private string entityName;
        private List<float> startPoint = new List<float>();
        private List<float> endPoint = new List<float>();
        public float duration { get; private set; }
        public Entity entity { get; private set; }
        public int States; // { Stop, Playing, Pause };
        public double animationTime; //Played time duration for current animation

        public CustomizedAnimation(string name, List<float> s, List<float> e, float speed)
        {
            this.States = 0;
            this.animationTime = 0.0;

            this.entityName = name;
            this.startPoint = s;
            this.endPoint = e;
            this.duration = speed * 1000.0f; //transform to milliseconds

            //Console.WriteLine(SceneParser.SPManager.Instance.DynamicEntities.Count);
            if (SceneParser.SPManager.Instance.DynamicEntities.ContainsKey(name))
            {
                entity = SceneParser.SPManager.Instance.DynamicEntities[name];
                //Console.WriteLine(entity.Guid.ToString());
            }
            else
            {
                Console.WriteLine("CustomizedAnimation: Cannot find this entity, is the name correct?");
            }
        }

        public Vector getCurrentTranslationByTime(double t)
        {

            float x = (endPoint[0] - startPoint[0]) * (float)t / duration + startPoint[0];
            float y = (endPoint[1] - startPoint[1]) * (float)t / duration + startPoint[1];
            float z = (endPoint[2] - startPoint[2]) * (float)t / duration + startPoint[2];

            return new Vector(x, y, z);
        }

    }
}
