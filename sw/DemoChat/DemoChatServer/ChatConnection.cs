//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChatServer client connection handler
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
using System.Threading;
using DemoChatLibrary;
using Newtonsoft.Json;

namespace DemoChatServer
{
    class ChatConnection
    {
        System.Net.Sockets.TcpClient client;
        System.IO.StreamReader reader;
        System.IO.StreamWriter writer;
        String nickName;
        string cs = String.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};",
            "localhost", "3306", "chat_demo", "root", "kissa");

        public ChatConnection(System.Net.Sockets.TcpClient client)
        {
            //create our TcpClient
            this.client = client;

            //create a new thread
            Thread chatThread = new Thread(new ThreadStart(startChat));

            //start the new thread
            chatThread.Start();
        }

        /// <summary>
        /// Initialize variables to socket communication
        /// </summary>
        private void startChat()
        {
            // add time consuming thing here before start socket worker thread

            //create our StreamReader object to read the current stream
            reader = new System.IO.StreamReader(client.GetStream());

            //create our StreamWriter objec to write to the current stream
            writer = new System.IO.StreamWriter(client.GetStream());

            //create a new thread for this user
            Thread chatThread = new Thread(new ThreadStart(runChat));

            //start the thread
            chatThread.Start();
        }

        /// <summary>
        /// Socket communication thread
        /// </summary>
        private void runChat()
        {
            ChatRoomDAO roomDao = new ChatRoomDAO(cs);
            ChatMessagesDAO msgDao = new ChatMessagesDAO(cs);

            try
            {
                //set out line variable to an empty string
                string line = "";
                while (true)
                {
                    //read the current line
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        Console.WriteLine(line);

                        try
                        {
                            // parse JSON object
                            ChatCommand cmd = JsonConvert.DeserializeObject<ChatCommand>(line);

                            // do actions
                            switch (cmd.Cmd)
                            {
                                case "list":
                                    {
                                        // get all chat room names for one user
                                        List<ChatRoom> rooms = roomDao.find(cmd.Name);
                                        foreach (ChatRoom r in rooms)
                                        {
                                            r.Messages = msgDao.find(r.Id);
                                        }

                                        String json = JsonConvert.SerializeObject(rooms, Formatting.None);
                                        writer.WriteLine(json);
                                        writer.Flush();
                                    }
                                    break;
                                case "open":
                                    // open single chat room and return latest messages

                                    break;
                                case "add":
                                    {
                                        // add new chat room and first message                                    
                                        ChatRoom room = JsonConvert.DeserializeObject<ChatRoom>(cmd.Parameters);
                                        ChatMessage m = room.Messages.ElementAt(0);
                                        m.ChatId = roomDao.create(room);
                                        msgDao.create(m);
                                    }
                                    break;
                                case "msg":
                                    {
                                        // add new message to existing chat room
                                        ChatRoom room = JsonConvert.DeserializeObject<ChatRoom>(cmd.Parameters);
                                        ChatMessage m = room.Messages.ElementAt(0);
                                        m.ChatId = room.Id;
                                        msgDao.create(m);
                                    }
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception e44)
            {
                Console.WriteLine(e44);
            }
        }
    }
}
