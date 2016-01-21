//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChatLibrary chat message object
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

namespace DemoChatLibrary
{
    public class ChatMessage
    {
        private UInt64 id;
        private String userFrom;
        private String message;
        private DateTime time;
        private UInt64 chatId;

        public ChatMessage()
        {
        }

        public ChatMessage(String user, String message)
        {
            this.userFrom = user;
            this.message = message;
        }

        public ChatMessage(UInt64 id, String user, String message)
        {
            this.id = id;
            this.userFrom = user;
            this.message = message;
        }

        public ChatMessage(UInt64 id, String user, String message, DateTime time)
        {
            this.id = id;
            this.userFrom = user;
            this.message = message;
            this.time = time;
        }

        public UInt64 Id
        {
            set { id = value; }
            get { return id; }
        }

        public String UserFrom
        {
            set { userFrom = value; }
            get { return userFrom; }
        }

        public String Message
        {
            set { message = value; }
            get { return message; }
        }

        public DateTime Time
        {
            set { time = value; }
            get { return time; }
        }

        public UInt64 ChatId
        {
            set { chatId = value; }
            get { return chatId; }
        }
    }
}
