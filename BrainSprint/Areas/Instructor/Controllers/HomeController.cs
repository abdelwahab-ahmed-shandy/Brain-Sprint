﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrainSprint.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]

    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
