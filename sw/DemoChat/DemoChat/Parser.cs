//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChat client command line parser
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
using DemoChatLibrary;
using Newtonsoft.Json;
using System.IO;

namespace DemoChat
{
    /// <summary>
    /// Common interface for commands
    /// </summary>
    public interface ICommand
    {
        bool Execute();
    }

    /// <summary>
    /// 
    /// </summary>
    public class AddCommand : ICommand
    {
        private String nickName;
        private System.Net.Sockets.TcpClient tcpClient;

        public AddCommand(String name, System.Net.Sockets.TcpClient tcpClient)
        {
            this.nickName = name;
            this.tcpClient = tcpClient;
        }

        public bool Execute()
        {

            Console.Write("Give name for new discussion: ");
            String name = Console.ReadLine();

            Console.Write("Give name of receiver: ");
            String userTo = Console.ReadLine();

            Console.Write("Write first message: ");
            String msg = Console.ReadLine();

            // serialize new chat
            ChatRoom c = new ChatRoom(nickName, userTo, name);
            c.addMessage(nickName, msg);
            String json = JsonConvert.SerializeObject(c, Formatting.None);

            // send query to server
            ChatCommand cmd = new ChatCommand("add", nickName, json);
            StreamWriter writer = new StreamWriter(tcpClient.GetStream());
            writer.WriteLine(cmd.ToJson());
            writer.Flush();

            return false;
        }
    }

    /// <summary>
    /// Exit command class
    /// </summary>
    public class ExitCommand : ICommand
    {
        public bool Execute()
        {
            return true;
        }
    }

    /// <summary>
    /// Help command class
    /// </summary>
    public class HelpCommand : ICommand
    {
        public bool Execute()
        {
            Console.WriteLine("(number) - Open existing discussion");
            Console.WriteLine("add - Create new discussion");
            Console.WriteLine("help - program instructions");
            Console.WriteLine("list - retrieve discussions from server");
            Console.WriteLine("sort - Sort your discussions");
            Console.WriteLine("quit - Exit program");
            Console.WriteLine();
            Console.WriteLine("Press enter to return main view.");
            Console.ReadLine();
            return false;
        }
    }

    /// <summary>
    /// list chat rooms
    /// </summary>
    public class ListCommand : ICommand
    {
        private String nickName;
        private System.Net.Sockets.TcpClient tcpClient;

        public ListCommand(String name, System.Net.Sockets.TcpClient tcpClient)
        {
            this.nickName = name;
            this.tcpClient = tcpClient;
        }

        public bool Execute()
        {
            // send query to server
            ChatCommand cmd = new ChatCommand("list", nickName);
            StreamWriter writer = new StreamWriter(tcpClient.GetStream());
            writer.WriteLine(cmd.ToJson());
            writer.Flush();
            return false;
        }
    }

    /// <summary>
    /// Not a command class
    /// </summary>
    public class OpenCommand : ICommand
    {
        private String nickName;
        private System.Net.Sockets.TcpClient tcpClient;
        private int index;
        private ChatRoom r;

        public OpenCommand(int index, String name, System.Net.Sockets.TcpClient tcpClient)
        {
            this.index = index;
            this.nickName = name;
            this.tcpClient = tcpClient;
        }

        public bool Execute()
        {
            Console.Clear();

            if (r != null)
            {
                foreach (ChatMessage m in r.Messages)
                {
                    Console.WriteLine(m.Time + " " + m.UserFrom + ": " + m.Message);
                }

                Console.WriteLine();
                Console.Write("Enter new message: ");
                String msg = Console.ReadLine();

                if (msg.Length > 2)
                {

                    // simply clone chat room id
                    ChatRoom c = new ChatRoom();
                    c.Id = r.Id;
                    c.addMessage(nickName, msg);
                    String json = JsonConvert.SerializeObject(c, Formatting.None);

                    // send query to server
                    ChatCommand cmd = new ChatCommand("msg", nickName, json);
                    StreamWriter writer = new StreamWriter(tcpClient.GetStream());
                    writer.WriteLine(cmd.ToJson());
                    writer.Flush();
                }
                else
                {
                    Console.WriteLine("Too short message!");
                }
            }

            return false;
        }

        public int Index
        {
            set { index = value; }
            get { return index; }
        }

        public ChatRoom Room
        {
            set { r = value; }
            get { return r; }
        }
    }

    /// <summary>
    /// Sort chat rooms
    /// </summary>
    public class SortCommand : ICommand
    {
        private int sort;

        public int Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        public bool Execute()
        {
            Console.Clear();
            Console.WriteLine("1) Sort by chat room creation time");
            Console.WriteLine("2) Sort by chat room name");
            Console.WriteLine("3) Sort by chat room latest message");
            Console.WriteLine();
            Console.Write("Enter selection: ");
            String selection = Console.ReadLine();

            int.TryParse(selection, out sort);
            return false;
        }
    }

    /// <summary>
    /// Not a command class
    /// </summary>
    public class DefaultCommand : ICommand
    {
        public bool Execute()
        {
            //Console.WriteLine("Not a command!");
            return false;
        }
    }


    public static class Parser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommand Parse(String command, String nickNAme, System.Net.Sockets.TcpClient tcpClient)
        {
            int index;

            // Parse your string and create Command object
            var commandParts = command.Split(' ').ToList();
            var commandName = commandParts[0];

            // the arguments is after the command
            var args = commandParts.Skip(1).ToList();

            // check if user selected discussion
            if (int.TryParse(commandName, out index))
            {
                Console.WriteLine("discussion" + index);
                // tähän keskustelun listaus ja toimen piteet
                return new OpenCommand(index, nickNAme, tcpClient);
            }

            // handle other commands
            switch (commandName)
            {
                // Create command based on CommandName (and maybe arguments)
                case "add":
                    return new AddCommand(nickNAme, tcpClient);
                case "list":
                    return new ListCommand(nickNAme, tcpClient);
                case "quit":
                    return new ExitCommand();
                case "help":
                    return new HelpCommand();
                case "sort":
                    return new SortCommand();

            }

            // no acceptable command found
            return new DefaultCommand();
        }
    }
}
