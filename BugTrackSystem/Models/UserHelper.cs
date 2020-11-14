using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackSystem.Models
{
    public class UserHelper : IUserAction
    {

        public void CreateItem(Type T, string userId)
        {
            if(T == typeof(Ticket))
            {
                var ticket = (Ticket)Convert.ChangeType(T, typeof(Ticket));
                
            }
            if(T == typeof(Project))
            {
                var project = (Project)Convert.ChangeType(T, typeof(Project));
            }
        }

        public void EditItem()
        {
            throw new NotImplementedException();
        }

        public void ViewItem()
        {
            throw new NotImplementedException();
        }
    }
}