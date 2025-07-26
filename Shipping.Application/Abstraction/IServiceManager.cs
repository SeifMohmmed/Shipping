using Shipping.Application.Abstraction.Product.Service;

namespace Shipping.Application.Abstraction;
public interface IServiceManager
{
    // Define all the services that the service manager will provide
    public IProductService productService { get; }

}
