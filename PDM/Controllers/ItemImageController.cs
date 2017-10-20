using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDM.Controllers
{

    public class ItemImageController : Controller
    {
        private IRepository<ItemImage> _repository;
        private IRepository<Item> _itemRepository;
        private ILogger<ItemController> _logger;
        private IHostingEnvironment _hostingEnviroment;
        private string[] includes = { "Item" };

        public ItemImageController(IRepository<ItemImage> repository, IRepository<Item> itemRepository, ILogger<ItemController> logger, IHostingEnvironment hostingEnvironment)
        {
            _repository = repository;
            _itemRepository = itemRepository;
            _logger = logger;
            _hostingEnviroment = hostingEnvironment;
        }

        [Authorize(Roles = "Administrator,Manager,Operator")]
        public IActionResult Index(int id)
        {
            var images = _repository.GetAll(w => w.ItemId == id && w.Download == false, includes);
            return View(Mapper.Map<IEnumerable<ItemImageViewModel>>(images));
        }
        [Authorize(Roles = "Administrator,Manager")]
        public IActionResult Create()
        {
            ItemImageViewModel images = new ItemImageViewModel();
            ViewBag.Items = _itemRepository.GetAll();
            return PartialView("_Create", Mapper.Map<ItemImageViewModel>(images));
        }
        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ItemImageViewModel viewModel, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in files)
                {
                    var newItemImage = Mapper.Map<ItemImage>(viewModel);
                    newItemImage.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!Directory.Exists(_hostingEnviroment.WebRootPath + "\\images\\" + viewModel.ItemId + "\\"))
                    {
                        Directory.CreateDirectory(_hostingEnviroment.WebRootPath + "\\images\\" + viewModel.ItemId + "\\");
                    }
                    var filePath = Path.Combine(_hostingEnviroment.WebRootPath + "\\images\\" + viewModel.ItemId + "\\", file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {

                        await file.CopyToAsync(fileStream);
                        newItemImage.Name = "\\images\\" + viewModel.ItemId + "\\" + file.FileName;
                        newItemImage.CreateDate = DateTime.Now;
                    }
                    _repository.Insert(newItemImage);
                    await _repository.SaveChangesAsync();

                }


            }
            return RedirectToAction("Index", "Item");
        }
        [Authorize(Roles = "Administrator,Manager")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var image = _repository.Get(w => w.Id == id);

            System.IO.File.Delete(_hostingEnviroment.WebRootPath + image.Name);
            return PartialView("_Delete", Mapper.Map<ItemImageViewModel>(image));

        }
        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var image = _repository.Get(w => w.Id == id);
            var deleteImage = Mapper.Map<ItemImage>(image);
            _repository.Delete(deleteImage);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index", "Item");
        }


        [Authorize(Roles = "Administrator,Manager,Operator")]
        [HttpGet]
        public IActionResult Download(int id)
        {
            var images = _repository.GetAll(w => w.ItemId == id && w.Download == true, includes);
            ViewBag.Items = _itemRepository.GetAll();
            return View(Mapper.Map<IEnumerable<ItemImageViewModel>>(images));

        }


        [Authorize(Roles = "Administrator,Manager")]
        [HttpGet]
        public IActionResult Upload()
        {
            ItemImageViewModel images = new ItemImageViewModel();
            ViewBag.Items = _itemRepository.GetAll();
            return PartialView("_Upload", Mapper.Map<ItemImageViewModel>(images));

        }
        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] ItemImageViewModel viewModel, List<IFormFile> files)
        {

            if (ModelState.IsValid)
            {
                foreach (var file in files)
                {
                    var newItemImage = Mapper.Map<ItemImage>(viewModel);
                    newItemImage.Download = true;
                    newItemImage.Description = file.FileName;
                    newItemImage.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!Directory.Exists(_hostingEnviroment.WebRootPath + "\\images\\download\\" + viewModel.ItemId + "\\"))
                    {
                        Directory.CreateDirectory(_hostingEnviroment.WebRootPath + "\\images\\download\\" + viewModel.ItemId + "\\");
                    }
                    var filePath = Path.Combine(_hostingEnviroment.WebRootPath + "\\images\\download\\" + viewModel.ItemId + "\\", file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {

                        await file.CopyToAsync(fileStream);
                        newItemImage.Name = "\\images\\download\\" + viewModel.ItemId + "\\" + file.FileName;
                        newItemImage.CreateDate = DateTime.Now;
                    }
                    _repository.Insert(newItemImage);
                    await _repository.SaveChangesAsync();

                }


            }
            return RedirectToAction("Index", "Item");

        }

    }
}
