//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   Software: DemoChat
//   Module: DemoChatServer chat message DAO
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
using MySql.Data.MySqlClient;

namespace DemoChatServer
{
    class ChatMessagesDAO
    {
        String cs = "";
        public ChatMessagesDAO(String cs)
        {
            this.cs = cs;
        }

        public UInt64 create(ChatMessage m)
        {
            MySqlConnection con = null;
            UInt64 id = 0;

            try
            {
                con = new MySqlConnection(cs);
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "INSERT INTO chat_messages (chat_id, user_from, message) VALUES(@chatId, @user, @msg)";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@chatId", m.ChatId);
                cmd.Parameters.AddWithValue("@user", m.UserFrom);
                cmd.Parameters.AddWithValue("@msg", m.Message);

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
        /// find chat messages by chat room id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ChatMessage> find(UInt64 id)
        {
            MySqlConnection con = null;
            MySqlDataReader rdr = null;
            List<ChatMessage> messages = new List<ChatMessage>();

            try
            {
                // connect to database
                con = new MySqlConnection(cs);
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT id, user_from, message, time FROM chat_messages WHERE chat_id = @id";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                rdr = cmd.ExecuteReader();

                // read result set
                while (rdr.Read())
                {
                    messages.Add(new ChatMessage(rdr.GetUInt64(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDateTime(3)));
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

            return messages;
        }
    }
}
