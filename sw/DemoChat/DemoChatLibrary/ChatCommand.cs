//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChatLibrary command object
//   Version 0.0.1
//   Copyright (C) 2015  
//   Kim Kristo
//   
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DemoChatLibrary
{
    public class ChatCommand
    {
        private String cmd = "";
        private String name = "";
        private String parameters = "";

        public ChatCommand()
        {
        }

        public ChatCommand(String cmd, String name)
        {
            this.cmd = cmd;
            this.name = name;
        }

        public ChatCommand(String cmd, String name, String parameters)
        {
            this.cmd = cmd;
            this.name = name;
            this.parameters = parameters;
        }

        public String Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public String ToJson()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.None);

            return json;
        }
    }
}
