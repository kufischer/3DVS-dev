using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FIVES;
using ClientManagerPlugin;
using System.IO;
using SINFONI;

namespace CustomizedAnimationPlugin
{
    public class CustomizedAnimationPluginInitializer : IPluginInitializer
    {
        public string Name
        {
            get { return "CustomizedAnimation"; }
        }

        public List<string> PluginDependencies
        {
            get { return new List<string> { "EventLoop", "ClientManager" }; }
        }

        public List<string> ComponentDependencies
        {
            get { return new List<string>(); }
        }

        public void Initialize()
        {
            RegisterComponents();
            RegisterClientServices();
            RegisterToEvents();

            RESTServicePlugin.RequestDispatcher.Instance.RegisterHandler(new CustomizedAnimationRequestHandler("/CA", "application/json"));

            CustomizedAnimationManager.Instance = new CustomizedAnimationManager();
            CustomizedAnimationManager.Instance.Initialize(this);
        }

        public void Shutdown()
        {

        }

        internal void RegisterComponents()
        {
           // ComponentDefinition animationComponent = new ComponentDefinition("animation");
           // animationComponent.AddAttribute<string>("animationKeyframes");
           // ComponentRegistry.Instance.Register(animationComponent);
        }

        private void RegisterToEvents()
        {

        }

        private void RegisterClientServices()
        {

        }

    }
}
