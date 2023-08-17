using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IMenuItemDao
    {

        public MenuItem GetMenuItemByItemId(int id);
        public IList<MenuItem> GetMenuItems();
        public MenuItem CreateMenuItem(MenuItem item);
        public MenuItem UpdateMenuItem(MenuItem item);
        public IList<MenuItem> GetMenuItemsForCookout(int cookoutId);


    }
}
