using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cloudant.Models;

namespace Cloudant.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDatabaseProvider _databaseProvider;

        public HomeController(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task<IActionResult> Index()
        {        
            return View(await _databaseProvider.ListAll());
        }


        [HttpPost]
        public async Task<IActionResult> Create(ContactViewModel model)
        {
            await _databaseProvider.CreateAsync(new Contact {FirstName = model.FirstName, LastName = model.LastName, Email = model.Email});
            

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(ContactViewModel model)
        {
            await _databaseProvider.DeleteAsync(model.Id, model.Rev);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(ContactViewModel model)
        {
        
            await _databaseProvider.UpdateAsync(

                new DocumentJSON()
                {
                    Id = model.Id,
                    Rev = model.Rev,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                }
            );

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
