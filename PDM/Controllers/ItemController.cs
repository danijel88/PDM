using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDM.Data;
using PDM.Models;
using PDM.Models.Repository;
using PDM.Services;
using PDM.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDM.Controllers
{

    public class ItemController : Controller
    {
        private IRepository<Item> _repository;
        private IRepository<ItemType> _repositoryItemType;
        private IRepository<MachineType> _repositoryMachineType;
        private IRepository<Pdm> _repositoryPdm;
        private IRepository<ItemImage> _repositoryImage;
        private IRepository<ItemHist> _repositoryItemHist;
        private IRepository<Proposal> _repositoryPropospal;
        private ILogger<ItemController> _logger;
        private IHostingEnvironment _hostingEnviroment;
        private IEmailSender _emailSender;
        private ApplicationDbContext _dbContext;

        private string[] includes = { "ItemType", "MachineType" };
        private string[] pdmEntities = { "Item" };

        public ItemController(IRepository<Item> repository,
            IRepository<ItemType> repositoryItemType,
            IRepository<MachineType> repositoryMachineType,
            IRepository<Pdm> pdmRepository,
            IRepository<ItemImage> imageRepository,
            IRepository<ItemHist> repositoryItemHist,
            IRepository<Proposal> repositoryPropospal,
            ILogger<ItemController> logger,
            IHostingEnvironment hostingEnvironment,
            IEmailSender emailSender,
            ApplicationDbContext dbContext)
        {
            _repository = repository;
            _repositoryItemType = repositoryItemType;
            _repositoryMachineType = repositoryMachineType;
            _repositoryPdm = pdmRepository;
            _repositoryImage = imageRepository;
            _repositoryItemHist = repositoryItemHist;
            _repositoryPropospal = repositoryPropospal;
            _logger = logger;
            _hostingEnviroment = hostingEnvironment;
            _emailSender = emailSender;
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Administrator,Manager,Operator")]
        public IActionResult Index()
        {

            var items = _repository.GetAll(includes);
            var pdms = _repositoryPdm.GetAll(pdmEntities);
            var images = _repositoryImage.GetAll(w => w.Download == false, pdmEntities);
            foreach (var item in items)
            {
                foreach (var pdm in pdms)
                {
                    if (item.Id == pdm.Item.Id)
                    {
                        item.Pdms.Add(pdm);
                    }
                }
                foreach (var image in images)
                {
                    if (item.Id == image.ItemId)
                    {
                        item.ImagePath = image.Name;
                    }

                }
            }
            return View(Mapper.Map<IEnumerable<ItemViewModel>>(items));

        }

        [Authorize(Roles = "Administrator,Manager")]
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

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    newItem.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

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

                    if (id == 0)
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

                        ItemHistViewModel histViewModel = new ItemHistViewModel();
                        var itemHistViewModel = Mapper.Map<ItemHist>(histViewModel);
                        itemHistViewModel.InternalCode = newItem.InternalCode;
                        itemHistViewModel.Name = newItem.Name;
                        itemHistViewModel.Description = newItem.Description;
                        itemHistViewModel.Band = newItem.Band;
                        itemHistViewModel.Color = newItem.Color;
                        itemHistViewModel.CreateDate = DateTime.Now;
                        itemHistViewModel.Elastic = newItem.Elastic;
                        itemHistViewModel.Enter = newItem.Enter;
                        itemHistViewModel.Exit = newItem.Exit;
                        itemHistViewModel.MadeBy = newItem.MadeBy;
                        itemHistViewModel.Thickness = newItem.Thickness;
                        itemHistViewModel.UserId = newItem.UserId;
                        itemHistViewModel.ItemId = newItem.Id;
                        itemHistViewModel.Status = newItem.Status;
                        itemHistViewModel.MachineTypeId = newItem.MachineTypeId;
                        itemHistViewModel.ItemTypeId = newItem.ItemTypeId;
                        _repositoryItemHist.Insert(itemHistViewModel);
                        await _repositoryItemHist.SaveChangesAsync();


                    }
                    if (await _repository.SaveChangesAsync())
                    {
                        //if (id == 0)
                        //{
                        //    foreach (var user in _dbContext.Users.OrderBy(u => u.Email).ToList())
                        //    {
                        //        var message = ($"New Item with Internal Code: {newItem.InternalCode} are created by {User.Identity.Name}.");
                        //        await _emailSender.SendEmailAsync(user.Email, "New Item Creation", message);
                        //    }
                        //    TempData["item"] = "Item successfully created.";
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator,Manager,Operator")]
        public IActionResult Details(int id)
        {
            var itemDetails = _repository.GetAll(w => w.Id == id, includes);
            var itemPdms = _repositoryPdm.GetAll(pdmEntities);
            var itemImages = _repositoryImage.GetAll(w => w.ItemId == id, pdmEntities);
            var itemHists = _repositoryItemHist.GetAll(w => w.ItemId == id);

            foreach (var item in itemDetails)
            {
                foreach (var pdm in itemPdms)
                {
                    if (item.Id == pdm.ItemId)
                    {
                        item.Pdms.Add(pdm);
                    }
                }
                foreach (var image in itemImages)
                {
                    if (item.Id == image.Id)
                    {
                        item.Images.Add(image);
                    }
                }
                foreach (var hist in itemHists)
                {
                    item.History.Add(hist);
                }
            }
            return View(Mapper.Map<IEnumerable<ItemViewModel>>(itemDetails));
        }

        [Authorize(Roles = "Administrator,Manager,Operator")]
        public IActionResult History(int id)
        {

            var itemHists = _repositoryItemHist.GetAll(w => w.ItemId == id, includes);

            return PartialView("_History", Mapper.Map<IEnumerable<ItemHistViewModel>>(itemHists));
        }

        [Authorize(Roles = "Administrator,Manager,Operator")]
        public IActionResult Proposal(int id)
        {
            var suggestions = _repositoryPropospal.GetAll(w => w.ItemId == id);

            return View(Mapper.Map<IEnumerable<PropospalViewModel>>(suggestions));
        }


    }
}
