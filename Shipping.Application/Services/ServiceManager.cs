using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Branch.Service;
using Shipping.Application.Abstraction.CitySettings.Service;
using Shipping.Application.Abstraction.Courier;
using Shipping.Application.Abstraction.CourierReport.Service;
using Shipping.Application.Abstraction.Employee;
using Shipping.Application.Abstraction.Merchant;
using Shipping.Application.Abstraction.OrderReport.Service;
using Shipping.Application.Abstraction.Orders.Service;
using Shipping.Application.Abstraction.Product.Service;
using Shipping.Application.Abstraction.Region;
using Shipping.Application.Abstraction.ShippingType.Serivce;
using Shipping.Application.Abstraction.SpecialCityCost.Service;
using Shipping.Application.Abstraction.SpecialCourierRegion.Serivce;
using Shipping.Application.Abstraction.WeightSetting;
using Shipping.Application.Services.BranchServices;
using Shipping.Application.Services.CitySettingServices;
using Shipping.Application.Services.CourierReportServices;
using Shipping.Application.Services.CourierServices;
using Shipping.Application.Services.EmployeeServices;
using Shipping.Application.Services.MerchantServices;
using Shipping.Application.Services.OrderReportServices;
using Shipping.Application.Services.OrderSerivces;
using Shipping.Application.Services.ProductServices;
using Shipping.Application.Services.RegionServices;
using Shipping.Application.Services.ShippingTypeServices;
using Shipping.Application.Services.SpecialCityCostServices;
using Shipping.Application.Services.SpecialCourierRegionServices;
using Shipping.Application.Services.WeightSettingServices;
using Shipping.Domain.Entities;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services;
public class ServiceManager : IServiceManager
{
    // Lazy loading of services to improve performance
    // This allows the services to be created only when they are accessed for the first time.
    // This can help reduce the startup time of the application and improve overall performance.
    // It also helps to avoid unnecessary instantiation of services that may not be used.
    // This is particularly useful in scenarios where the services are expensive to create or have dependencies that may not be needed immediately.
    // By using Lazy<T>, the services are created only when they are accessed, which can help improve performance.
    // Lazy<T> is a thread-safe way to create objects only when they are needed.
    // This can help improve performance by avoiding unnecessary instantiation of services that may not be used.

    private readonly Lazy<IProductService> _productService;
    private readonly Lazy<IRegionService> _regionService;
    private readonly Lazy<IWeightSettingService> _weightSettingService;
    private readonly Lazy<ICourierReportService> _courierReportService;
    private readonly Lazy<IShippingTypeService> _shippingTypeService;
    private readonly Lazy<IOrderService> _orderService;
    private readonly Lazy<IBranchService> _branchService;
    private readonly Lazy<ISpecialCityCostService> _specialCityCostService;
    private readonly Lazy<ICitySettingService> _citySettingService;
    private readonly Lazy<ISpecialCourierRegionService> _specialCourierRegionService;
    private readonly Lazy<IOrderReportService> _orderReportService;
    private readonly Lazy<ICourierService> _courierService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IMerchantService> _merchantService;

    public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
        _regionService = new Lazy<IRegionService>(() => new RegionService(unitOfWork, mapper));
        _weightSettingService = new Lazy<IWeightSettingService>(() => new WeightSettingService(unitOfWork, mapper));
        _courierReportService = new Lazy<ICourierReportService>(() => new CourierReportService(unitOfWork, mapper));
        _shippingTypeService = new Lazy<IShippingTypeService>(() => new ShippingTypeService(unitOfWork, mapper));
        _orderService = new Lazy<IOrderService>(() => new OrderService(unitOfWork, mapper, userManager, httpContextAccessor));
        _branchService = new Lazy<IBranchService>(() => new BranchService(unitOfWork, mapper));
        _specialCityCostService = new Lazy<ISpecialCityCostService>(() => new SpecialCityCostService(unitOfWork, mapper));
        _citySettingService = new Lazy<ICitySettingService>(() => new CitySettingService(unitOfWork, mapper));
        _specialCourierRegionService = new Lazy<ISpecialCourierRegionService>(() => new SpecialCourierRegionService(unitOfWork, mapper));
        _orderReportService = new Lazy<IOrderReportService>(() => new OrderReportService(unitOfWork, mapper, userManager));
        _courierService = new Lazy<ICourierService>(() => new CourierService(unitOfWork, userManager, mapper));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(unitOfWork, mapper));
        _merchantService = new Lazy<IMerchantService>(() => new MerchantService(unitOfWork, mapper));
    }

    // Properties to access the services
    public IProductService productService => _productService.Value;
    public IRegionService regionService => _regionService.Value;
    public IWeightSettingService weightSettingService => _weightSettingService.Value;
    public ICourierReportService courierReportService => _courierReportService.Value;
    public IShippingTypeService shippingTypeService => _shippingTypeService.Value;
    public IOrderService orderService => _orderService.Value;
    public IOrderReportService orderReportService => _orderReportService.Value;
    public IBranchService branchService => _branchService.Value;
    public ISpecialCityCostService specialCityCostService => _specialCityCostService.Value;
    public ICitySettingService citySettingService => _citySettingService.Value;
    public ISpecialCourierRegionService specialCourierRegionService => _specialCourierRegionService.Value;
    public ICourierService courierService => _courierService.Value;
    public IEmployeeService employeeService => _employeeService.Value;
    public IMerchantService merchantService => _merchantService.Value;
}
