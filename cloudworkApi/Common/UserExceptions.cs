using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudworkApi.Common
{
    public class UserExceptions : Exception
    {
        public UserExceptions() { }
        public UserExceptions(string message) : base(message) {
            ResponseBuilder.isUserException = true;
        }
        public UserExceptions(string message, Exception inner = null) : base(message, inner) { }
    }
}
