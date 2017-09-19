﻿// This file is part of FiVES.
//
// FiVES is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation (LGPL v3)
//
// FiVES is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with FiVES.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIVES;
using NLog;

namespace SceneParser
{
    public class StaticSceneryPluginInitializer : IPluginInitializer
    {
        #region IPluginInitializer implementation

        public string Name
        {
            get
            {
                return "SceneParser";
            }
        }

        public List<string> PluginDependencies
        {
            get
            {
                return new List<string> { "ClientManager" };
            }
        }

        public List<string> ComponentDependencies
        {
            get
            {
                return new List<string>();
            }
        }

        public void Initialize()
        {
            ComponentDefinition sp = new ComponentDefinition("scene");
            sp.AddAttribute<Guid>("parent");
            ComponentRegistry.Instance.Register(sp);
            SPConfiguration.Configuration.ReadConfig();
            new SPManager().CreateEntities();
        }

        public void Shutdown()
        {
        }

        #endregion

        private void RegisterService()
        {
        }
    }
}
