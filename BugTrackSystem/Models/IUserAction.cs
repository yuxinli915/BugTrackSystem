using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackSystem.Models
{
    interface IUserAction
    {
        void CreateItem(Type T, string userId);
        void EditItem();
        void ViewItem();
    }
}
