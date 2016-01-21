//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChatServer main module
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
using System.Net;


namespace DemoChatServer
{
    class DemoChatServer
    {
        private System.Net.Sockets.TcpListener chatServer;



        static void Main(string[] args)
        {
            DemoChatServer server = new DemoChatServer();
        }

        public DemoChatServer()
        {
            try
            {
                //create our TCPListener object
                chatServer = new System.Net.Sockets.TcpListener(IPAddress.Any, 4296);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("DemoChat server started.");
            //while (true) do the commands

            while (true)
            {
                //start the chat server
                chatServer.Start();

                //check if there are any pending connection requests
                if (chatServer.Pending())
                {
                    //if there are pending requests create a new connection
                    System.Net.Sockets.TcpClient chatConnection = chatServer.AcceptTcpClient();

                    //display a message letting the user know they're connected
                    Console.WriteLine("New connection from: " + GetRemoteIPAddress(chatConnection));

                    //create a new DoCommunicate Object
                    ChatConnection con = new ChatConnection(chatConnection);
                }
            }
        }

        /// <summary>
        /// Get TcpClient remote endpoint address
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Ip address of connecting end point</returns>
        private string GetRemoteIPAddress(System.Net.Sockets.TcpClient client)
        {
            // get remote end point
            IPEndPoint ep = client.Client.RemoteEndPoint as IPEndPoint;

            // check if valid
            if (ep == null)
            {
                return "unknown";
            }

            return ep.Address.ToString();
        }
    }
}
