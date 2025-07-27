using Shipping.Application.Abstraction.CourierReport.Service;
using Shipping.Application.Abstraction.Product.Service;
using Shipping.Application.Abstraction.ShippingType.Serivce;

namespace Shipping.Application.Abstraction;
public interface IServiceManager
{
    // Define all the services that the service manager will provide
    public IProductService productService { get; }
    public ICourierReportService courierReportService { get; }
    public IShippingTypeService shippingTypeService { get; }

}
