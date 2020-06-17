using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDulicious.Models;


namespace CRUDulicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context { get; set; }

        public HomeController(MyContext context)
        {
            _context = context;
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = _context.Dishes.ToList();
            return View(AllDishes);
            //return View();
        }

        [HttpGet("/create")]
        public IActionResult Dishes()
        {
            return View();
        }
        

        [HttpPost("create")]
        public IActionResult Create(Dish Dish)
        {
            if(ModelState.IsValid)
            {
                _context.Dishes.Add(Dish);
                _context.SaveChanges();
                return Redirect("/");
            }
            else
            {
                return View ("Dishes");
            }
        }

        [HttpGet("dish/{DId}")]
        public IActionResult DishProfile (int DId)
        {
            
            Dish dish = _context.Dishes.FirstOrDefault(d => d.DishId == DId);
            ViewBag.OneDish = dish;
            return View(dish);
        } 

        [HttpGet("dish/{DId}/edit")]
        public IActionResult EditDish (int DId)
        {
            Dish dish = _context.Dishes.FirstOrDefault(dsh => dsh.DishId == DId);
            return View("EditDish", dish);
        }

        [HttpPost("dish/{DId}/update")]
        public IActionResult UpdateDish(int DId, Dish d) //d is the form data
        {
            if(ModelState.IsValid)
            {
                Dish dish = _context.Dishes.FirstOrDefault(dsh => dsh.DishId == DId); //dsh.DishId refers to that dish, when it was just d.DishId it didn't know which dish it was referencing to
                Console.WriteLine("$$$$$$$$$$$$$$$$$$");
                Console.WriteLine(dish);
                Console.WriteLine(d);
                Console.WriteLine(DId);
                dish.Name = d.Name;
                dish.Chef = d.Chef;
                dish.Description = d.Description;
                dish.Calories = d.Calories;
                dish.Tastiness = d.Tastiness;
                dish.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                 //also why isn't the form carrying over the Name, Tastiness, etc
                Console.WriteLine(DId); //this is coming back 0
                return Redirect($"/dish/{DId}"); //why is this redirecting to /dish/0? rerouting to Index works fine.
            }
            else
            {
                d.DishId = DId;
                return View("EditDish", d);
            }
        }

        [HttpGet("dish/{DId}/remove")]
        public IActionResult Remove(Dish Dish, int DId)
        {
                Dish dish = _context.Dishes.FirstOrDefault(dsh => dsh.DishId == DId); //delete breaks on clicking Remove (because it's HttpGet not Post)
                _context.Dishes.Remove(dish);
                Console.WriteLine("$$$$$$$$$$$$$$");
                Console.WriteLine(dish);
                _context.SaveChanges();
                return Redirect("/");
        }
    }
}
