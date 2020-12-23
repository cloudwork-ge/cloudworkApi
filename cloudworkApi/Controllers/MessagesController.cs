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
        

        public void OpenChat()
        {

        }
        [HttpGet]
        [Authorization]
        public List<tbMessages> getMessages(Chat obj)
        {
            var list = new List<tbMessages>();
            return provider.getMessages(authUser.ID,obj.chatUserID);
            return list;
        }
    }
}
