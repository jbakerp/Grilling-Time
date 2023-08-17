using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.Logic;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class InviteController : ControllerBase
    {
        private readonly IInviteDao inviteDao;


        public InviteController(IInviteDao inviteDao)
        {
            this.inviteDao = inviteDao;
        }

        [HttpGet()]
        public ActionResult<List<Invite>> GetInvites()
        {
            IList<Invite> invites = new List<Invite>();
            invites = inviteDao.GetAllInvites();
            if(invites != null)
            {
                return Ok(invites);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("email/{email}")]
        [AllowAnonymous]
        public ActionResult<List<Invite>> GetInvitesByEmail(string email)
        {
            IList<Invite> invites = new List<Invite>();
            invites = inviteDao.GetInvitesByEmail(email);
            if(invites != null)
            {
                return Ok(invites);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{cookoutId}/email/{email}")]
        [AllowAnonymous]
        public ActionResult<Invite> GetInviteByEmailAddress(int cookoutId, string email)
        {
            Invite invite = new Invite();
            invite = inviteDao.GetInviteByEmailAddress(cookoutId, email);

            if(invite != null)
            {
                return invite;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Invite> GetInviteById(int id)
        {
            Invite invite = new Invite();
            invite = inviteDao.GetInviteById(id);

            if(invite != null)
            {
                return invite;
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("cookout/{cookoutId}")]
        [AllowAnonymous]
        public ActionResult<List<Invite>> GetInviteByCookoutId(int cookoutId)
        {
            IList<Invite> invites = new List<Invite>();
            invites = inviteDao.GetInvitesByCookoutId(cookoutId);

            if(invites != null)
            {
                return Ok(invites);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("/user/{id}")]
        [AllowAnonymous]
        public ActionResult<Invite> GetInviteByUserId(int id)
        {
            Invite invite = inviteDao.GetInviteByUserId(id);

            if (invite != null)
            {
                return invite;
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("{id}")]
        [AllowAnonymous]
        public ActionResult<Invite> UpdateInvite(Invite inviteToUpdate)
        {
            Invite updatedInvite = inviteDao.UpdateInvite(inviteToUpdate);

            if(updatedInvite != null)
            {
                return Ok(updatedInvite);
            }
            else
            {
                return Conflict();
            }
        }
        [HttpPost()]
        public ActionResult<Invite> CreateNewInvite(Invite newInvite)
        {
            Invite createdInvite = inviteDao.CreateNewInvite(newInvite);

            if(createdInvite != null)
            {
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmail(newInvite.InviteEmail, createdInvite.CookoutId, createdInvite.InviteId);
                return Ok(createdInvite);
            }
            else
            {
                return Conflict();
            }
        }
    }
}
