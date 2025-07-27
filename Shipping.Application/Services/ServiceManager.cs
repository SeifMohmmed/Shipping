using AutoMapper;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.CourierReport.Service;
using Shipping.Application.Abstraction.Product.Service;
using Shipping.Application.Abstraction.ShippingType.Serivce;
using Shipping.Application.Services.CourierReportServices;
using Shipping.Application.Services.ProductServices;
using Shipping.Application.Services.ShippingTypeServices;
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
    private readonly Lazy<ICourierReportService> _courierReportService;
    private readonly Lazy<IShippingTypeService> _shippingTypeService;

    public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
        _courierReportService = new Lazy<ICourierReportService>(() => new CourierReportService(unitOfWork, mapper));
        _shippingTypeService = new Lazy<IShippingTypeService>(() => new ShippingTypeService(unitOfWork, mapper));

    }

    // Properties to access the services
    public IProductService productService => _productService.Value;
    public ICourierReportService courierReportService => _courierReportService.Value;
    public IShippingTypeService shippingTypeService => _shippingTypeService.Value;
}
