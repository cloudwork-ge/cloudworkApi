using System;
using System.Collections.Generic;
using System.Linq;
using cloudworkApi.Common;
using cloudworkApi.Models.dsModels;
using cloudworkApi.Models.tableModels;
using cloudworkApi.SqlDataBaseEntity;

namespace cloudworkApi.StoredProcedures
{
    public class PKG_MESSAGES
    {
        private readonly CloudWorkContext cwContext;
        public PKG_MESSAGES()
        {
            cwContext = new CloudWorkContext();
        }
        public List<tbMessages> GetMessages(int userID, int chatID)
        {
            using CloudWorkContext context = new CloudWorkContext();

            if (!ChatExists(userID,chatID))
            {
                ResponseBuilder.throwError("ჩატი არ მოიძებნა.");
                return null;
            }

            var messagesAll = new List<tbMessages>();
            try
            {
                messagesAll = context.Messages.Where(x => x.chatID == chatID).OrderBy(x=>x.ID).ToList();
            }
            catch(Exception ex)
            {
                messagesAll = new List<tbMessages>();
            }

            return messagesAll;
        }
        public bool CanContact(int userID, int chatUserID)
        {
            using CloudWorkContext context = new CloudWorkContext();
            var hasAcceptedProjects = context.Projects.Count(x => (x.userId == userID && x.workerUserId == chatUserID) || (x.userId == chatUserID && x.workerUserId == userID));
            if (hasAcceptedProjects > 0) return true;
            else return false;
        }
        public bool ChatExists(int userID, int chatID)
        {   
            using CloudWorkContext context = new CloudWorkContext();
            
            var chatExists = 0;
            chatExists = context.Chats.Count(x => x.ID == chatID && (x.firstUserID == userID || x.secondUserID == userID));
            return chatExists > 0;
        }
        public tbChats CreateOrGetChat(int firstUserID, int secondUserID, string name = null)
        {
            using CloudWorkContext context = new CloudWorkContext();
            int[] userIDs = { firstUserID, secondUserID };
            int chatID = 0;
            tbChats chat = new tbChats();
            chatID = context.Chats.Where(x => userIDs.Contains(x.firstUserID) && userIDs.Contains(x.secondUserID)).Select(x=>x.ID).FirstOrDefault();
            if (chatID == 0)
            {
                var firstUserName = context.Users.Where(x => x.ID == firstUserID).Select(x => x.fullname).FirstOrDefault().ToString();
                var secondUserName = context.Users.Where(x => x.ID == secondUserID).Select(x => x.fullname).FirstOrDefault().ToString();
                var newChatObj = new tbChats() { firstUserID = firstUserID, firstUserName = firstUserName, secondUserName = secondUserName, secondUserID = secondUserID, Name = name, createDate = DateTime.Now };
                
                context.Chats.Add(newChatObj);
                context.SaveChanges();

                chat = newChatObj;
                chat.Name = getChatName(firstUserID, chat);
            }
            else
            {
                chat = GetChat(firstUserID, chatID);
            }
            return chat;
        }
        public tbChats GetChat(int userID, int chatID)
        {
            using CloudWorkContext context = new CloudWorkContext();
            var chat = context.Chats.Where(x => x.ID == chatID).FirstOrDefault();
            chat.Name = getChatName(userID, chat);
            return chat;
        }
        public void SendMessage(string messageText, int userID, int chatID)
        {
            using CloudWorkContext context = new CloudWorkContext();
            //int[] userIDs = { firstUserID, secondUserID };
            if (string.IsNullOrEmpty(messageText)) ResponseBuilder.throwError("შეიყვანეთ მესიჯის ტექსტი");

            if (chatID > 0 && ChatExists(userID, chatID))
            {
                var fromUserName = context.Users.Where(x => x.ID == userID).Select(x => x.fullname).FirstOrDefault().ToString();
                //var secondUserName = context.Users.Where(x => x.ID == secondUserID).Select(x => x.fullname).FirstOrDefault().ToString();

                var newMessageObj = new tbMessages() { chatID = chatID, fromUserID = userID, fromUserName = fromUserName, message = messageText, read = 0, createDate = DateTime.Now};
                context.Messages.Add(newMessageObj);

                tbChats newChatObj = GetChat(userID,chatID);
                newChatObj.lastMessage = messageText;
                newChatObj.lastMessageDate = DateTime.Now;
                context.Chats.Update(newChatObj);

                context.SaveChanges();
            }
            else {
                ResponseBuilder.throwError("ჩატი არ არსებობს");
            }
        }
        public List<tbChats> GetRecentChats(int userID)
        {
            using CloudWorkContext context = new CloudWorkContext();
            
            List<tbChats> chats = context.Chats.Where(x => x.firstUserID == userID || x.secondUserID == userID).OrderByDescending(x=>x.lastMessageDate).ToList();
            chats.ForEach(x =>
            {
                x.Name = getChatName(userID, x);
            });

            return chats;
        }
        public string getChatName(int userID, tbChats chat)
        {
            if (string.IsNullOrEmpty(chat.Name))
                chat.Name = chat.firstUserID == userID ? chat.secondUserName : chat.firstUserName;

            return chat.Name;
        }
        public void MarkAsRead(int userID, int chatID, int messageID)
        {
            using CloudWorkContext context = new CloudWorkContext();
            var prevMessages = new List<tbMessages>();
            var chatCount = 0;
            chatCount = context.Chats.Count(x => x.ID == chatID && (x.firstUserID == userID || x.secondUserID == userID));
            if (chatCount > 0)
            {
                prevMessages = context.Messages.Where(x => x.ID <= messageID && x.chatID == chatID && x.fromUserID != userID && x.read == 0).ToList();
                if (prevMessages != null && prevMessages.Count > 0)
                {
                    var updated = prevMessages.Select(x => { x.read = 1; x.readDate = DateTime.Now; return x; });
                    context.Messages.UpdateRange(updated);
                    context.SaveChanges();
                }
            }
        }
    }
}
