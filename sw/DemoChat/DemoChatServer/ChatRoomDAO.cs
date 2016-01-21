//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChatServer chat room DAO
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
using MySql.Data.MySqlClient;
using DemoChatLibrary;

namespace DemoChatServer
{
    /// <summary>
    /// http://dev.mysql.com/tech-resources/articles/mysql-installer-for-windows.html
    /// </summary>
    class ChatRoomDAO
    {
        String cs = "";

        public ChatRoomDAO(String cs)
        {
            this.cs = cs;
        }

        public UInt64 create(ChatRoom r)
        {
            MySqlConnection con = null;
            UInt64 id = 0;

            try
            {
                con = new MySqlConnection(cs);
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "INSERT INTO chat_rooms (name, user_A, user_B) VALUES(@name, @userA, @userB)";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@name", r.Name);
                cmd.Parameters.AddWithValue("@userA", r.UserA);
                cmd.Parameters.AddWithValue("@userB", r.UserB);

                cmd.ExecuteNonQuery();
                id = (UInt64)cmd.LastInsertedId;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            return id;
        }

        /// <summary>
        /// find chat rooms by user
        /// </summary>
        /// <param name="name"></param>
        public List<ChatRoom> find(String name)
        {
            MySqlConnection con = null;
            MySqlDataReader rdr = null;
            List<ChatRoom> rooms = new List<ChatRoom>();

            try
            {
                // connect to database
                con = new MySqlConnection(cs);
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT id, user_A, user_B, name, startTime FROM chat_rooms WHERE user_A = @userA OR user_B = @userB";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@userA", name);
                cmd.Parameters.AddWithValue("@userB", name);
                cmd.ExecuteNonQuery();

                rdr = cmd.ExecuteReader();

                // read result set
                while (rdr.Read())
                {
                    rooms.Add(new ChatRoom(rdr.GetUInt64(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetDateTime(4)));
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (con != null)
                {
                    con.Close();
                }
            }

            return rooms;
        }
    }
}
