﻿using Commercio.Data;
using Commercio.DTOs;
using Commercio.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace Commercio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            
        }
        public IActionResult Index()
        {

           return View();

        }
    }
}
