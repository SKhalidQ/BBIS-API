﻿using BBIS_API.DbAccess;
using BBIS_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BBIS_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ActionName("VerifyUser")]
        public async Task<ActionResult<string>> CheckUser([FromBody]GetUser user)
        {
            try
            {
                var userExists = await DbAccessClass.VerifyUser(user.Username, user.Password, _context);

                return userExists ? Ok(new JsonResult($"Welcome {user.Username}")) : throw new Exception("Wrong username or password");

                //await Task.WhenAll();

                //return (Username == "Administrator" && Password == "@dm1n")
                //    ? Ok(new JsonResult("User verified"))
                //    : throw new Exception("User not verified");
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "User not verified" => Unauthorized(new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public async Task<ActionResult<string>> ChangePassword([FromBody]GetUser _user, [FromHeader]string newPassword)
        {
            try
            {
                var userExists = await DbAccessClass.VerifyUser(_user.Username, _user.Password, _context);

                if (!userExists)
                    throw new Exception("Wrong username or password");

                var user = await DbAccessClass.GetUser(_user.Username, _context);

                var changePassword = await DbAccessClass.ChangePassword(user, newPassword, _context);

                if (changePassword)
                    return Ok(new JsonResult("Success"));
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "Wrong username or password": return Unauthorized(new JsonResult(ex.Message));
                    default: return BadRequest(new JsonResult(ex.Message));
                }
            }
        }
    }
}
