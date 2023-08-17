using Capstone.Models;
using System.Collections;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICookoutDao
    {
        public IList<Cookout> GetListOfCookouts();
        public IList<Cookout> GetListOfCookoutsByHostId(int hostId);
        public Cookout GetCookoutByCookoutId(int cookoutId);
        public Cookout CreateCookout(Cookout cookout);
        public Cookout UpdateCookout(Cookout cookout);
        public IList<Order> GetOrdersByCookoutId(int cookoutId);
        public IList<Cookout> GetInvitedCookouts(string email);
        public IList<Cookout> GetListOfCookoutsByChefId(int chefId);
        public IList<MenuItem> GetMenuItemsForCookout(int cookoutId);
        public IList<Order> GetOrdersByEmailForCookout(int cookoutId, string email);
    }
}
