using EfCoreDataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WarehouseEngine.Models;
using WarehouseEngineBusinessLayer.Interfaces;
using WarehouseEngineBusinessLayer.Services;
using WarehouseEngineBusinessLayer.ViewModels;

namespace WarehouseEngine.Controllers
{
    public class DetailsController : Controller
    {
        private readonly ICrudService<Detail, DetailViewModel> _detailService;
        private readonly ICrudService<Supplier, SupplierViewModel> _supplierService;
        private readonly IExcelExportService _excelExportService;
        const string exportAll = "AllDetailsExport";
        const string exportPart = "PartDetailsExport";
        const string excelFormat = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public DetailsController(ICrudService<Detail, DetailViewModel> detailService, ICrudService<Supplier, SupplierViewModel> supplierService, IExcelExportService excelExportService)
        {
            _detailService = detailService;
            _supplierService = supplierService;
            _excelExportService = excelExportService;
        }

        public async Task<IActionResult> Index(string searchString, int? pageNumberParam)
        {
            ViewData["CurrentFilter"] = searchString;
            
            var details = await _detailService.GetAllAsync();
            ViewBag.supService = _supplierService;

            if (!String.IsNullOrEmpty(searchString))
            {
                details = details.Where(d => d.Name.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (pageNumberParam ?? 1);

            return View(await PaginatedList<DetailViewModel>.CreateAsync(details, pageNumber, pageSize));
        }

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _detailService.GetByIdAsync(id);
            ViewBag.Supplier = await _supplierService.GetByIdAsync(viewModel.SupplierId);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = await _supplierService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DetailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _detailService.CreateAsync(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _detailService.GetByIdAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            ViewBag.Suppliers = await _supplierService.GetAllAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DetailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _detailService.UpdateAsync(id, viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _detailService.GetByIdAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _detailService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("ExcelExport")]
        public async Task<IActionResult> ExcelExport(string data)
        {
            var entitiesForExport = JsonSerializer.Deserialize<IEnumerable<DetailViewModel>>(data);
            var fileName = $"{exportPart}_{DateTime.UtcNow}.xlsx";

            var bytes = _excelExportService.ExportToExcel(entitiesForExport);

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet, ActionName("ExportAllSupplierData")]
        public async Task<IActionResult> ExcelExportAllSupplierData(string data)
        {
            var entitiesForExport = await _detailService.GetAllAsync();

            var fileName = $"{exportAll}_{DateTime.UtcNow}.xlsx";

            var bytes = _excelExportService.ExportToExcel(entitiesForExport);

            return File(bytes, excelFormat, fileName);
        }
    }
}
