using System.Collections.Generic;
using Capstone.Models;
namespace Capstone.DAO

{
    public interface IInviteDao
    {
        Invite GetInviteByUserId(int id);
        Invite GetInviteByEmailAddress(int cookoutId, string email);
        IList<Invite> GetInvitesByEmail(string email);
        Invite GetInviteById(int id);
        Invite UpdateInvite(Invite invite);
        Invite CreateNewInvite(Invite newInvite);
        IList<Invite> GetAllInvites();
        IList<Invite> GetInvitesByCookoutId(int cookoutId);
    }
}
