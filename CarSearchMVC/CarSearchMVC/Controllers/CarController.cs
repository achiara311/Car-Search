using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CarSearchMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarSearchMVC.Controllers
{
    public class CarController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394");

            var response = await client.GetAsync("api/car");
            var result = await response.Content.ReadAsAsync<List<Vehicles>>();
            return View(result);
        }

        public async Task<IActionResult> GetCarById(int Id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394");

            var response = await client.GetAsync($"api/car/{Id}");
            var result = await response.Content.ReadAsAsync<Vehicles>();
            return View(result);
        }


        public IActionResult AddCar()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddCar(Vehicles newCar)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394");
            //HTTP Post
            var postEmployee = await client.PostAsJsonAsync<Vehicles>("api/car", newCar);
            //refering to Add method
            if (postEmployee.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // will return bool
            }

            ModelState.AddModelError(string.Empty, "Server error. Did not add to Db");
            return View(newCar);
        }

        //before you can edit car, need to GET car
        //HttpGET basically
        public async Task<IActionResult> EditCar(int Id) //coming from a form
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394");

            var response = await client.GetAsync($"api/car/{Id}");
            var result = await response.Content.ReadAsAsync<Vehicles>();
            return View(result);
        }


        //PUT is used when creating/modifying api. Not when we're using it!!
        [HttpPost] //acting like HttpPut in a way
        public async Task<IActionResult> EditCar(Vehicles updatedCar)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394");
            var putTask = await client.PutAsJsonAsync<Vehicles>($"api/car/{updatedCar.Id}", updatedCar);
            //because we had id parameter on the other one, we need carId for updatedCar
            if (putTask.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("EditCar", updatedCar.Id);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteCar(int Id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394");

            var deleteTask = await client.DeleteAsync($"api/car/{Id}");

            return RedirectToAction("Index");
        }
    }
}
