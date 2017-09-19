using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EventLoopPlugin;
using FIVES;
using Newtonsoft.Json.Linq;

namespace CustomizedAnimationPlugin
{
    class CustomizedAnimationManager
    {
        public static CustomizedAnimationManager Instance;

        internal CustomizedAnimationManager() { }

        internal void Initialize()
        {
            EventLoop.Instance.TickFired += new EventHandler<TickEventArgs>(HandleEventTick);
            LastTick = new TimeSpan(DateTime.Now.Ticks);
        }

        internal void Initialize(CustomizedAnimationPluginInitializer plugin)
        {
            this.animationPlugin = plugin;
            Initialize();
        }

        private void HandleEventTick(Object sender, TickEventArgs e)
        {
            double frameDuration = e.TimeStamp.Subtract(LastTick).TotalMilliseconds;
            LastTick = e.TimeStamp;

            //Console.WriteLine(frameDuration);

            //Triggered within fixed time
            lock (AnimationsForEntities)
            {
                foreach (KeyValuePair<string, CustomizedAnimation>
                    animatedEntity in AnimationsForEntities)
                {
                    if (animatedEntity.Value.States == 1) //meaning we allow it to play
                    {
                        animatedEntity.Value.animationTime += frameDuration;
                        //Console.WriteLine(animatedEntity.Value.animationTime);
                        //now we asked the current translation of object
                        if (animatedEntity.Value.animationTime < animatedEntity.Value.duration)
                        {
                            Vector position = animatedEntity.Value.getCurrentTranslationByTime(animatedEntity.Value.animationTime);
                            //Console.WriteLine(position.x + "  " + position.y + "  " + position.z);
                            animatedEntity.Value.entity["location"]["position"].Suggest(position);
                        }
                        else 
                        {
                            animatedEntity.Value.animationTime = 0.0; //Circle
                        }
                    }
                    else if (animatedEntity.Value.States == 0) //meaning it will stop
                    {
                        animatedEntity.Value.States = 0;
                        animatedEntity.Value.animationTime = 0.0;
                    }
                }
            }
        }

        public void PlaybackAnimation(string content)
        {
            //Now only support playback one entity once a time
            JObject o = JObject.Parse(content);
            string entityName = o["entityName"].ToString();

            lock (AnimationsForEntities)
            {     
                if (AnimationsForEntities.ContainsKey(entityName))
                {
                    AnimationsForEntities[entityName].States = 1; //change state to Playing 
                    Console.WriteLine("Playback animation for " + entityName);
                }
            }
        }

        public void StopAnimation(string content)
        {
            //Now only support playback one entity once a time
            JObject o = JObject.Parse(content);
            string entityName = o["entityName"].ToString();

            lock (AnimationsForEntities)
            {
                if (AnimationsForEntities.ContainsKey(entityName))
                {
                    AnimationsForEntities[entityName].States = 0; //change state to Stop                  

                    Vector position = AnimationsForEntities[entityName].getCurrentTranslationByTime(0.0f);
                    //Console.WriteLine(position.x + "  " + position.y + "  " + position.z);
                    AnimationsForEntities[entityName].entity["location"]["position"].Suggest(position);

                    Console.WriteLine("Stop animation for " + entityName);
                }
            }
        }

        public void SetAnimation(string content)
        {
            /* An JSON example:
              {
              "entityName":["gate_bosch"],
              "animationInfo":[
                {
                  "startPoint":[0, 0, 0],
                  "endPoint":[10, 10, 10],
                  "speed": 10
                }
              ]
            }*/

            JObject o = JObject.Parse(content);
            if (o["entityName"] != null & o["animationInfo"] != null)
            {
                IList<JToken> names = o["entityName"].Children().ToList();
                IList<JToken> animations = o["animationInfo"].Children().ToList();
                List<string> entityNameCollection = new List<string>();
                List<float> startPoints = new List<float>();
                List<float> endPoints = new List<float>();
                List<float> speeds = new List<float>();

                foreach (JToken name in names)
                {
                    entityNameCollection.Add(name.ToString());
                }

                foreach (JToken animation in animations)
                {
                    IList<JToken> startPt = animation["startPoint"].Children().ToList();
                    IList<JToken> endPt = animation["endPoint"].Children().ToList();
                    foreach (JToken pt in startPt)
                    {
                        startPoints.Add(float.Parse(pt.ToString()));
                    }
                    foreach (JToken pt in endPt)
                    {
                        endPoints.Add(float.Parse(pt.ToString()));
                    }
                    speeds.Add(float.Parse(animation["speed"].ToString()));
                    //Console.WriteLine(animation["speed"]);

                }

                for (int i=0; i<entityNameCollection.Count; i++)
                {
                    string name = entityNameCollection[i];
                    List<float> startPoint = new List<float>();
                    List<float> endPoint = new List<float>();
                    float speed = speeds[i];
                    for (int j = 0; j < 3; j++)
                    {
                        startPoint.Add(startPoints[i * 3 + j]);
                        endPoint.Add(endPoints[i * 3 + j]);
                    }
                    CustomizedAnimation ani = new CustomizedAnimation(name, startPoint, endPoint, speed);
                    AnimationsForEntities[name] = ani;
                }
            }

        }

        internal Dictionary<string, CustomizedAnimation> AnimationsForEntities = new Dictionary<string, CustomizedAnimation>();

        private TimeSpan LastTick;

        CustomizedAnimationPluginInitializer animationPlugin;
    }
}
