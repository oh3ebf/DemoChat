//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChat client program
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
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using DemoChatLibrary;
using Newtonsoft.Json;
using System.Linq;

namespace DemoChat
{
    class DemoChat
    {
        [DllImport("kernel32.dll")]
        private static extern void ExitProcess(int a);
        private List<ChatRoom> chatRooms;
        private System.Net.Sockets.TcpClient tcpClient;

        static void Main(string[] args)
        {
            DemoChat d = new DemoChat();
        }

        public DemoChat()
        {
            bool exit = false;
            String nickName;


            // system running on localhost
            tcpClient = new System.Net.Sockets.TcpClient();

            try
            {
                tcpClient.Connect("localhost", 4296);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect: ");
            }

            Thread chatThread = new Thread(new ThreadStart(run));
            chatThread.Start();

            Console.Clear();
            Console.WriteLine("Welcome to simple chat!");

            // get user nick name for discussions
            nickName = getNicName();

            // send query to server
            ChatCommand cmd = new ChatCommand("list", nickName);
            StreamWriter writer = new StreamWriter(tcpClient.GetStream());
            //Console.WriteLine(cmd.ToJson());
            writer.WriteLine(cmd.ToJson());
            writer.Flush();

            // program main loop
            while (exit == false)
            {
                Console.Clear();
                Console.WriteLine("Discussions for (" + nickName + ")");
                // discussionsAA
                if (chatRooms != null)
                {
                    int i = 0;
                    foreach (ChatRoom r in chatRooms)
                    {
                        Console.WriteLine(i + ") " + r.UserA + " ( " + r.StartTime + ": " + r.Name + " )");
                        i++;
                    }
                }

                Console.WriteLine();
                Console.Write("Enter command (help to display help): ");
                String line = Console.ReadLine();
                var command = Parser.Parse(line, nickName, tcpClient);

                // chat room has different parameters
                if (command.GetType() == typeof(OpenCommand))
                {
                    // open chat room
                    ChatRoom r = chatRooms.ElementAt(((OpenCommand)command).Index);
                    ((OpenCommand)command).Room = r;
                    exit = command.Execute();
                }
                else
                {
                    if (command.GetType() == typeof(SortCommand))
                    {
                        exit = command.Execute();

                        switch (((SortCommand)command).Sort)
                        {
                            case 1:
                                chatRooms = chatRooms.OrderByDescending(o => o.StartTime).ToList();
                                break;
                            case 2:
                                chatRooms = chatRooms.OrderBy(o => o.Name).ToList();
                                break;
                            case 3:
                                chatRooms = chatRooms.OrderBy(o => o.Messages.ElementAt(o.Messages.Count - 1).Time).ToList();
                                break;
                        }
                    }
                    else
                    {
                        exit = command.Execute();
                    }
                }
            }

            tcpClient.Close();
            // terminate program
            ExitProcess(0);
        }

        /// <summary>
        /// Read user nick name for discussions
        /// </summary>
        /// <returns>name as String</returns>
        static String getNicName()
        {
            String name = "";

            // minimum length for nick name
            while (name.Length < 4)
            {
                try
                {
                    Console.Write("Give your nick name: ");
                    name = Console.ReadLine();
                    if (name.Length < 4)
                    {
                        Console.WriteLine("You gave empty or too short nick name!");
                        Console.WriteLine("Minimum length is four characters!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to read console input: ");
                }
            }

            return name;
        }

        private void run()
        {
            //create our StreamReader Object, based on the current NetworkStream
            StreamReader reader = new StreamReader(tcpClient.GetStream());

            while (true)
            {
                //
                String json = reader.ReadLine();
                //Console.WriteLine(json);
                chatRooms = JsonConvert.DeserializeObject<List<ChatRoom>>(json);
            }
        }
    }
}
