using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.Security.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
   
    public class UserController : ControllerBase
    {
        private readonly IUserDao userDao;


        public UserController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [HttpGet()]
        public ActionResult<User> GetAllUsers()
        {
            IList<User> users = userDao.GetUsers();
            if(users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("username/{username}")]
        public ActionResult<User> GetUserByUsername(string username)
        {
            User user = userDao.GetUserByUsername(username);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }


        }
    }
}
