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
    public class MachineTypeController : Controller
    {
        private IRepository<MachineType> _repository;
        private ILogger<MachineTypeController> _logger;

        public MachineTypeController(IRepository<MachineType> repository, ILogger<MachineTypeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var machines = _repository.GetAll();

            return View(Mapper.Map<IEnumerable<MachineTypeViewModel>>(machines));
        }

        public IActionResult Create()
        {
            return PartialView("_AddEditType");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MachineTypeViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newType = Mapper.Map<MachineType>(viewModel);
                    newType.CreateDate = DateTime.Now;
                    newType.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var exist = _repository.Get(w => string.Equals(w.Name, viewModel.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (exist != null)
                    {
                        TempData["machineType"] = "Machine Type already exist.";
                    }
                    else
                    {
                        _repository.Insert(newType);
                        if (await _repository.SaveChangesAsync())
                        {
                            TempData["machineType"] = "Machine Type is successfully created.";
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
