using Shipping.Application.Abstraction.Branch.Service;
using Shipping.Application.Abstraction.CitySettings.Service;
using Shipping.Application.Abstraction.CourierReport.Service;
using Shipping.Application.Abstraction.OrderReport.Service;
using Shipping.Application.Abstraction.Orders.Service;
using Shipping.Application.Abstraction.Product.Service;
using Shipping.Application.Abstraction.ShippingType.Serivce;
using Shipping.Application.Abstraction.SpecialCityCost.Service;
using Shipping.Application.Abstraction.SpecialCourierRegion.Serivce;

namespace Shipping.Application.Abstraction;
public interface IServiceManager
{
    // Define all the services that the service manager will provide
    public IProductService productService { get; }
    public ICourierReportService courierReportService { get; }
    public IShippingTypeService shippingTypeService { get; }
    public IOrderService orderService { get; }
    public IOrderReportService orderReportService { get; }
    public IBranchService branchService { get; }
    public ISpecialCityCostService specialCityCostService { get; }
    public ICitySettingService citySettingService { get; }
    public ISpecialCourierRegionService specialCourierRegionService { get; }
}
