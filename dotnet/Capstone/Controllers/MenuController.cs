using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private IMenuItemDao MenuDao;

        public MenuController(IMenuItemDao menuDao)
        {
            this.MenuDao = menuDao;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<MenuItem> GetMenuItemById(int id)
        {
            MenuItem item = null;
            item = MenuDao.GetMenuItemByItemId(id);
            if(item != null)
            {
                return Ok(item);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet()]
        [AllowAnonymous]
        public ActionResult<IList<MenuItem>> GetAllMenuItems()
        {
            return Ok(MenuDao.GetMenuItems());
        }

        //[HttpGet()]
        //public ActionResult<IList<MenuItem>>GetMenuForCookout(int cookoutId)
        //{
        //    return Ok(MenuDao.GetMenuItemsForCookout(cookoutId));
        //}

        [HttpPost()]
        [AllowAnonymous]
        public ActionResult<MenuItem> CreateMenuItem(MenuItem item)
        {
            MenuItem newItem = null;
            newItem = MenuDao.CreateMenuItem(item);

            if(newItem != null)
            {
                return Created($"/menu", newItem);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public ActionResult<MenuItem> UpdateMenuItem(int id, MenuItem item)
        {
            try
            {
                if (id != item.ItemID)
                {
                    return NotFound();
                }
                else
                {
                    item = MenuDao.UpdateMenuItem(item);
                    return Ok(item);
                }
            }
            catch(DaoException ex)
            {
                return NotFound();
            }
        }



    }
}
