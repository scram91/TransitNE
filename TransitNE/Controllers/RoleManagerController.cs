﻿using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers
{
    public class RoleManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}