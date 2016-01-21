insert into chat_rooms (name, user_A, user_B) values ("Juttua pukkaa","make","erkki");

insert into chat_messages (chat_id, user_from, message) values (4,"make","Miten menee?");

select * from chat_rooms where user_A = 'kake' OR user_B = 'kake';
select * from chat_rooms;
select * from chat_messages;

delete from chat_rooms limit 100;
delete from chat_messages limit 100;