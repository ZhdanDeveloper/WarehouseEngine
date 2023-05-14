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
    public class ProductController : Controller
    {
        private readonly ICrudService<Product, ProductViewModel> _productService;
        private readonly ICrudService<Supplier, SupplierViewModel> _supplierService;
        private readonly IExcelExportService _excelExportService;
        const string exportAll = "AllProductExport";
        const string exportPart = "PartProductExport";
        const string excelFormat = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ProductController(ICrudService<Product, ProductViewModel> productService, ICrudService<Supplier, SupplierViewModel> supplierService, IExcelExportService excelExportService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _excelExportService = excelExportService;
        }

        public async Task<IActionResult> Index(string searchString, int? pageNumberParam)
        {
            ViewData["CurrentFilter"] = searchString;
            
            var product = await _productService.GetAllAsync();
            ViewBag.supService = _supplierService;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(d => d.Name.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (pageNumberParam ?? 1);

            return View(await PaginatedList<ProductViewModel>.CreateAsync(product, pageNumber, pageSize));
        }

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _productService.GetByIdAsync(id);
            ViewBag.Supplier = await _supplierService.GetByIdAsync(viewModel.SupplierId);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = await _supplierService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateAsync(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _productService.GetByIdAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            ViewBag.Suppliers = await _supplierService.GetAllAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateAsync(id, viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _productService.GetByIdAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("ExcelExport")]
        public async Task<IActionResult> ExcelExport(string data)
        {
            var entitiesForExport = JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(data);
            var fileName = $"{exportPart}_{DateTime.UtcNow}.xlsx";

            var bytes = _excelExportService.ExportToExcel(entitiesForExport);

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet, ActionName("ExportAllProductData")]
        public async Task<IActionResult> ExcelExportAllSupplierData(string data)
        {
            var entitiesForExport = await _productService.GetAllAsync();

            var fileName = $"{exportAll}_{DateTime.UtcNow}.xlsx";

            var bytes = _excelExportService.ExportToExcel(entitiesForExport);

            return File(bytes, excelFormat, fileName);
        }
    }
}
