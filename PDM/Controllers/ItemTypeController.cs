using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDM.Models;
using PDM.Models.Repository;
using PDM.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDM.Controllers
{
    public class ItemTypeController : Controller
    {
        private IRepository<ItemType> _repository;
        private ILogger<ItemTypeController> _logger;

        public ItemTypeController(IRepository<ItemType> repository, ILogger<ItemTypeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var itemTypes = _repository.GetAll();
            //_logger.LogInformation("Fetching all data from ItemType table");
            return View(Mapper.Map<IEnumerable<ItemTypeViewModel>>(itemTypes));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_AddEditType");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ItemTypeViewModel viewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var newType = Mapper.Map<ItemType>(viewModel);
                    newType.CreateDate = DateTime.Now;
                    var exist = _repository.Get(w => string.Equals(w.Name, viewModel.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (exist != null)
                    {
                        TempData["itemType"] = "Item Type already exist.";
                    }
                    else
                    {
                        _repository.Insert(newType);
                        if (await _repository.SaveChangesAsync())
                        {
                            TempData["itemType"] = "Item Type is successfully created.";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
