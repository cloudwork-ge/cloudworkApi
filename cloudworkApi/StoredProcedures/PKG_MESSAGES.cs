using System;
using System.Collections.Generic;
using System.Linq;
using cloudworkApi.Models.dsModels;
using cloudworkApi.Models.tableModels;
using cloudworkApi.SqlDataBaseEntity;

namespace cloudworkApi.StoredProcedures
{
    public class PKG_MESSAGES
    {
        public PKG_MESSAGES()
        {
        }
        public List<tbMessages> getMessages(int userID,int chatUserID)
        {
            using CloudWorkContext context = new CloudWorkContext();
            var messagesAll = new List<tbMessages>();
            try
            {
                messagesAll = context.Messages.Where(x => (x.fromUserID == userID && x.toUserID == chatUserID) || (x.fromUserID == chatUserID && x.toUserID == userID)).OrderByDescending(x => x.ID).ToList();
            }
            catch(Exception ex)
            {
                messagesAll = new List<tbMessages>();
            }

            return messagesAll;
        }
    }
}
