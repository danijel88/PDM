using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDM.Models;
using PDM.Models.Repository;
using PDM.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PDM.Controllers
{
    public class ItemController : Controller
    {
        private IRepository<Item> _repository;
        private IRepository<ItemType> _repositoryItemType;
        private IRepository<MachineType> _repositoryMachineType;
        private ILogger<ItemController> _logger;
        private IHostingEnvironment _hostingEnviroment;
        private string[] includes = { "ItemType", "MachineType" };

        public ItemController(IRepository<Item> repository, IRepository<ItemType> repositoryItemType, IRepository<MachineType> repositoryMachineType, ILogger<ItemController> logger, IHostingEnvironment hostingEnvironment)
        {
            _repository = repository;
            _repositoryItemType = repositoryItemType;
            _repositoryMachineType = repositoryMachineType;
            _logger = logger;
            _hostingEnviroment = hostingEnvironment;
        }

        public IActionResult Index()
        {

            var items = _repository.GetAll(includes);

            return View(Mapper.Map<IEnumerable<ItemViewModel>>(items));

        }

        public IActionResult Create(int? id)
        {
            ViewBag.ItemTypes = _repositoryItemType.GetAll();
            ViewBag.MachineTypes = _repositoryMachineType.GetAll();
            if (id.HasValue)
            {
                var items = _repository.Get(w => w.Id == id, includes);
                return PartialView("_CreateItem", Mapper.Map<ItemViewModel>(items));
            }
            else
            {
                ItemViewModel itemViewModel = new ItemViewModel();
                return PartialView("_CreateItem", itemViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, [FromForm]ItemViewModel viewModel, List<IFormFile> files)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    bool isNew = !id.HasValue;
                    var newItem = Mapper.Map<Item>(viewModel);
                    newItem.MachineType = _repositoryMachineType.Get(w => w.Id == viewModel.MachineTypeId);
                    newItem.ItemType = _repositoryItemType.Get(w => w.Id == viewModel.ItemTypeId);

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var filePath = Path.Combine(_hostingEnviroment.WebRootPath + "\\images", file.FileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                newItem.ImagePath = "\\images\\" + file.FileName;
                            }
                        }
                    }

                    if (isNew)
                    {

                        newItem.CreateDate = DateTime.Now;
                        var exist = _repository.Get(w => string.Equals(w.InternalCode, viewModel.InternalCode, StringComparison.CurrentCultureIgnoreCase));
                        if (exist != null)
                        {
                            TempData["item"] = "Item already exist.";
                        }
                        else
                        {
                            _repository.Insert(newItem);
                        }
                    }
                    else
                    {
                        newItem.Id = (int)id;
                        newItem.UpdateDate = DateTime.Now;
                        _repository.Update(newItem);
                    }
                    if (await _repository.SaveChangesAsync())
                    {
                        TempData["item"] = "Item successfully created.";
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
