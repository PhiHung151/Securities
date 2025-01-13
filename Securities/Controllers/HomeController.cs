using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Securities.Models;
using Securities.Data;

public class HomeController : Controller{
    public IActionResult Index(){
        return View();
    }
}