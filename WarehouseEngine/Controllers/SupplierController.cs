using EfCoreDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WarehouseEngine.Models;
using WarehouseEngineBusinessLayer.Interfaces;
using WarehouseEngineBusinessLayer.Services;
using WarehouseEngineBusinessLayer.ViewModels;

namespace WarehouseEngine.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly ICrudService<Supplier, SupplierViewModel> _supplierService;
        private readonly IExcelExportService _excelExportService;
        const string exportAll = "AllSuppliersExport";
        const string exportPart = "PartSuppliersExport";
        const string excelFormat = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public SupplierController(ICrudService<Supplier, SupplierViewModel> supplierService, IExcelExportService excelExportService)
        {
            _supplierService = supplierService;
            _excelExportService = excelExportService;
        }
        public async Task<IActionResult> Index(string searchString, int? pageNumberParam)
        {
            ViewData["CurrentFilter"] = searchString;
            var suppliers = await _supplierService.GetAllAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                suppliers = suppliers.Where(d => d.Name.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (pageNumberParam ?? 1);

            return View(await PaginatedList<SupplierViewModel>.CreateAsync(suppliers, pageNumber, pageSize));
        }
        public async Task<IActionResult> Details(int id)
        {
            var viewModelList = await _supplierService.GetByIdAsync(id);
            return View(viewModelList);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.CreateAsync(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _supplierService.GetByIdAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SupplierViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.UpdateAsync(id, viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _supplierService.GetByIdAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _supplierService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("ExcelExport")]
        public async Task<IActionResult> ExcelExport(string data)
        {
            var entitiesForExport = JsonSerializer.Deserialize<IEnumerable<SupplierViewModel>>(data);
            var fileName = $"{exportPart}_{DateTime.UtcNow}.xlsx";

            var bytes = _excelExportService.ExportToExcel(entitiesForExport);

            return File(bytes, excelFormat, fileName);
        }

        [HttpGet, ActionName("ExportAllSupplierData")]
        public async Task<IActionResult> ExcelExportAllSupplierData(string data)
        {
            var entitiesForExport = await _supplierService.GetAllAsync();

            var fileName = $"{exportAll}_{DateTime.UtcNow}.xlsx";

            var bytes = _excelExportService.ExportToExcel(entitiesForExport);

            return File(bytes, excelFormat, fileName);
        }
    }
}
