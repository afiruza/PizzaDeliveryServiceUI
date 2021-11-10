using PizzaDeliveryServiceUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PizzaDeliveryServiceUI.Controllers
{
    public class PizzaController : Controller
    {

        private readonly string BASE_URL = "http://ec2-3-144-255-118.us-east-2.compute.amazonaws.com/";

        public async Task<ActionResult> Index()
        {

            List<Pizza> PizzaInfo = new List<Pizza>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Pizza");
                if (Res.IsSuccessStatusCode)
                {
                    var PizzaResponse = Res.Content.ReadAsStringAsync().Result;
                    PizzaInfo = JsonConvert.DeserializeObject<List<Pizza>>(PizzaResponse);
                }
                //returning the pizzas list to view 
                return View(PizzaInfo);
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BASE_URL);
                    HttpResponseMessage Res = await client.GetAsync("api/Pizza/" + id);
                    Pizza pizza = null;
                    if (Res.IsSuccessStatusCode)
                    {
                        var PizzaResponse = Res.Content.ReadAsStringAsync().Result;
                        pizza = JsonConvert.DeserializeObject<Pizza>(PizzaResponse);
                        return View(pizza);
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            Pizza pizza = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                HttpResponseMessage Res = await client.GetAsync("api/Pizza/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var PizzaResponse = Res.Content.ReadAsStringAsync().Result;
                    pizza = JsonConvert.DeserializeObject<Pizza>(PizzaResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(pizza);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Pizza p)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BASE_URL);
                    HttpResponseMessage Res = await client.GetAsync("api/Pizza/" + id);
                    Pizza pizza = null;
                    if (Res.IsSuccessStatusCode)
                    {
                        var PizzaResponse = Res.Content.ReadAsStringAsync().Result;
                        pizza = JsonConvert.DeserializeObject<Pizza>(PizzaResponse);
                    }
                    pizza.Name = p.Name;
                    pizza.Price = p.Price;
                    pizza.Description = p.Description;
                    var postTask = client.PutAsJsonAsync<Pizza>("api/Pizza/" + pizza.Id, pizza);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Pizza p)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BASE_URL);
                    var postTask = client.PostAsJsonAsync<Pizza>("api/Pizza/", p);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BASE_URL);
                    HttpResponseMessage Res = await client.GetAsync("api/Pizza/" + id);
                    Pizza pizza = null;
                    if (Res.IsSuccessStatusCode)
                    {
                        var PizzaResponse = Res.Content.ReadAsStringAsync().Result;
                        pizza = JsonConvert.DeserializeObject<Pizza>(PizzaResponse);
                        return View(pizza);
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id, Pizza p)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BASE_URL);
                    HttpResponseMessage Res = await client.GetAsync("api/Pizza/" + id);
                    Pizza pizza = null;
                    if (Res.IsSuccessStatusCode)
                    {
                        var PizzaResponse = Res.Content.ReadAsStringAsync().Result;
                        pizza = JsonConvert.DeserializeObject<Pizza>(PizzaResponse);
                    }

                    var deleteTask = client.DeleteAsync("api/Pizza/" + pizza.Id);
                    deleteTask.Wait();
                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}

