using Core_CallAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core_CallAPI.Controller1
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            List<SubEmpDTO> subEmpList = new List<SubEmpDTO>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44358/api/SubEmp/GetSubEmpList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    subEmpList = JsonConvert.DeserializeObject<List<SubEmpDTO>>(apiResponse);
                }
            }
            return View(subEmpList);
        }
        public ViewResult GetSubEmpDTO() => View();

        [HttpPost]
        public async Task<IActionResult> GetSubEmpDTO(int id)
        {
            SubEmpDTO subEmpDTO = new SubEmpDTO();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44324/api/Reservation/" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        subEmpDTO = JsonConvert.DeserializeObject<SubEmpDTO>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View(subEmpDTO);
        }

        public ViewResult AddSubEmp() => View();

        [HttpPost]
        public async Task<IActionResult> AddSubEmp(SubEmpDTO subEmpDTO)
        {
            SubEmpDTO emp = new SubEmpDTO();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(subEmpDTO), System.Text.Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44358/api/SubEmp/InsertSubEmp", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bool res = JsonConvert.DeserializeObject<bool>(apiResponse);
                    emp = subEmpDTO;
                }
            }
            return View(emp);
        }
    }
}
