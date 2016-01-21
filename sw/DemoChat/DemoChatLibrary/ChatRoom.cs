//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChatLibrary chat room object
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

namespace DemoChatLibrary
{
    public class ChatRoom
    {
        private UInt64 id;
        private String userA;
        private String userB;
        private String name;
        private List<ChatMessage> messages;
        private DateTime startTime;

        public ChatRoom()
        {
            messages = new List<ChatMessage>();
        }

        public ChatRoom(String userA, String userB, String name)
        {
            this.userA = userA;
            this.userB = userB;
            this.name = name;
            messages = new List<ChatMessage>();
        }

        public ChatRoom(UInt64 id, String userA, String userB, String name)
        {
            this.id = id;
            this.userA = userA;
            this.userB = userB;
            this.name = name;
            messages = new List<ChatMessage>();
        }

        public ChatRoom(UInt64 id, String userA, String userB, String name, DateTime time)
        {
            this.id = id;
            this.userA = userA;
            this.userB = userB;
            this.name = name;
            messages = new List<ChatMessage>();
            this.startTime = time;
        }

        public void addMessage(String name, String message)
        {
            messages.Add(new ChatMessage(name, message));
        }

        public UInt64 Id
        {
            set { id = value; }
            get { return id; }
        }

        public String UserA
        {
            set { userA = value; }
            get { return userA; }
        }

        public String UserB
        {
            set { userB = value; }
            get { return userB; }
        }

        public String Name
        {
            set { name = value; }
            get { return name; }
        }

        public List<ChatMessage> Messages
        {
            set { messages = value; }
            get { return messages; }
        }

        public DateTime StartTime
        {
            set { startTime = value; }
            get { return startTime; }
        }
    }
}
