using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDM.Models;
using PDM.Models.Repository;
using PDM.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDM.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    public class PdmController : Controller
    {
        private IRepository<Pdm> _repository;
        private IRepository<Item> _itemRepository;
        private ILogger<PdmController> _logger;

        public PdmController(IRepository<Pdm> repository, IRepository<Item> itemRepository, ILogger<PdmController> logger)
        {
            _repository = repository;
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            string[] entities = { "Item" };
            var pdms = _repository.GetAll(entities);
            return View(Mapper.Map<IEnumerable<PdmViewModel>>(pdms));
        }

        public IActionResult Create()
        {
            ViewBag.Items = _itemRepository.GetAll();
            PdmViewModel viewModel = new PdmViewModel();
            return PartialView("_Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PdmViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newPdm = Mapper.Map<Pdm>(viewModel);
                newPdm.CreateDate = DateTime.Now;
                newPdm.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                _repository.Insert(newPdm);
                if (await _repository.SaveChangesAsync())
                {
                    TempData["pdm"] = "PDM successfully created.";
                }
            }

            return RedirectToAction("Index");
        }


    }
}
