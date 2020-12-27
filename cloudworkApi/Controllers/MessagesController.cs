using System;
using System.Collections.Generic;
using System.Text.Json;
using cloudworkApi.Attributes;
using cloudworkApi.Models.dsModels;
using cloudworkApi.Models.tableModels;
using cloudworkApi.StoredProcedures;
using Microsoft.AspNetCore.Mvc;

namespace cloudworkApi.Controllers
{
    public class MessagesController : MainController
    {
        private readonly PKG_MESSAGES provider;

        public MessagesController()
        {
            provider = new PKG_MESSAGES();
        }

        [HttpPost]
        [Authorization]
        public List<tbMessages> OpenChat(Chat chat)
        {
            if (chat.chatID > 0)
                return getMessages(chat.chatID);
            else return new List<tbMessages>();
            //else
            //{
            //    int chatID = 0;
            //    chatID = CreateOrGetChat(chat.chatUserID);
            //    return getMessages(chatID);
            //}
        }

        private List<tbMessages> getMessages(int chatID)
        {
            //var list = new List<tbMessages>();
            return provider.GetMessages(authUser.ID,chatID);
        }

        [HttpPost]
        [Authorization]
        public tbChats CreateOrGetChat(Chat pChat)
        {
            tbChats chat = new tbChats();
            if (provider.CanContact(authUser.ID, pChat.chatUserID))
            {
                chat = provider.CreateOrGetChat(authUser.ID, pChat.chatUserID);
            }
            else throwError("ამ ოპერაციის განხორციელების უფლება არ გაქვთ.");
            return chat;
        }
        [HttpPost]
        [Authorization]
        public void SendMessage(Message message)
        {
            provider.SendMessage(message.messageText, authUser.ID, message.chatID);
        }
        [HttpPost]
        [Authorization]
        public List<tbChats> GetRecentChats()
        {
            var chats = provider.GetRecentChats(authUser.ID);
            return chats;
        }
    }
}
