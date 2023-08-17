using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using static System.Collections.Specialized.BitVector32;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CookoutController : ControllerBase
    {
        private ICookoutDao CookoutDao;

        public CookoutController(ICookoutDao cookoutDao)
        {
            this.CookoutDao = cookoutDao;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Cookout> GetCookoutByCookoutId(int id)
        {
            Cookout cookout = null;
            cookout = CookoutDao.GetCookoutByCookoutId(id);
            if (cookout != null)
            {
                return Ok(cookout);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult<IList<Cookout>> GetAllCookouts()
        {
            IList<Cookout> cookouts = new List<Cookout>();
            cookouts = CookoutDao.GetListOfCookouts();

            return Ok(cookouts);
        }

        [HttpGet("guest/{email}")]
        [AllowAnonymous]
        public ActionResult<IList<Cookout>> GetInvitedCookouts(string email)
        {
            IList<Cookout> cookouts = new List<Cookout>();
            cookouts = CookoutDao.GetInvitedCookouts(email);
            return Ok(cookouts);
        }

        [HttpGet("chef/{id}")]
        public ActionResult<IList<Cookout>> GetCookoutForChef(int id)
        {
            return Ok(CookoutDao.GetListOfCookoutsByChefId(id));
        }


        [HttpGet("host/{id}")]
        public ActionResult<IList<Cookout>> GetCookoutForHost(int id)
        {
            return Ok(CookoutDao.GetListOfCookoutsByHostId(id));
        }

        [HttpGet("{cookoutId}/orders")]
        public ActionResult<IList<Order>> GetCookoutOrders(int cookoutId)
        {
            return Ok(CookoutDao.GetOrdersByCookoutId(cookoutId));
        }

        [HttpPost]
        public ActionResult<Cookout> CreateACookout(Cookout cookout)
        {
            Cookout newCookout = null;
            newCookout = CookoutDao.CreateCookout(cookout);

            return Created($"/cookout/{newCookout.CookoutID}", newCookout);

        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public ActionResult<Cookout> UpdateCookout(Cookout cookout)
        {

            try
            {

                cookout = CookoutDao.UpdateCookout(cookout);
                return Ok(cookout);

            }
            catch (DaoException ex)
            {
                return NotFound();
            }

        }

        [HttpGet("{cookoutId}/menu")]
        [AllowAnonymous]
        public ActionResult<IList<MenuItem>> GetMenuForCookout(int cookoutId)
        {
            return Ok(CookoutDao.GetMenuItemsForCookout(cookoutId));
        }

        [HttpGet("{cookoutId}/orders/{email}")]
        public ActionResult<IList<Order>> GetOrdersByEmailForCookout(int cookoutId, string email)
        {
            return Ok(CookoutDao.GetOrdersByEmailForCookout(cookoutId, email));
        }






    }
}
