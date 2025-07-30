using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.OrderReport.DTO;
using Shipping.Application.Abstraction.OrderReport.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.OrderReportServices;
public class OrderReportService(IUnitOfWork unitOfWork,
    IMapper mapper,
    UserManager<ApplicationUser> userManager) : IOrderReportService
{
    // Get All Order Report By Pramter(Sort , Pagenation)
    public async Task<IEnumerable<OrderReportToShowDTO>> GetAllOrderReportAsync(OrderReportPramter pramter)
    {
        var orderReports = await unitOfWork.GetOrderReportRepository().GetOrderReportByPramter(pramter);
        var orderReportDTO = await GetMerchantNameAndAmountReceivedAndShippingCostPaid(orderReports);

        return orderReportDTO;
    }

    // Get Order Report By Id
    public async Task<OrderReportDTO> GetOrderReportAsync(int id)
    => mapper.Map<OrderReportDTO>(await unitOfWork.GetOrderReportRepository().GetByIdAsync(id));

    //Add Order Report
    public async Task AddAsync(OrderReportDTO DTO)
    {
        await unitOfWork.GetRepository<OrderReport, int>().AddAsync(mapper.Map<OrderReport>(DTO));
        await unitOfWork.CompleteAsync();
    }

    //Delete Order Report
    public async Task DeleteAsync(int id)
    {
        var orderReportRepo = unitOfWork.GetRepository<OrderReport, int>();
        var existingOrderReport = await orderReportRepo.GetByIdAsync(id);

        if (existingOrderReport is null)
            throw new KeyNotFoundException($"OrderReport with ID {id} not found.");

        await orderReportRepo.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }

    //Update Order Report
    public async Task UpdateAsync(int id, OrderReportDTO DTO)
    {
        var orderReportRepo = unitOfWork.GetRepository<OrderReport, int>();
        var existingOrderReport = await orderReportRepo.GetByIdAsync(id, include:
            p => p
            .Include(r => r.Order));

        if (existingOrderReport is null)
            throw new KeyNotFoundException($"OrderReport with ID {DTO.Id} not found.");

        mapper.Map(DTO, existingOrderReport);

        orderReportRepo.UpdateAsync(existingOrderReport);

        await unitOfWork.CompleteAsync();
    }

    //============================================================================

    // Method to get Merchant Name and Amount Received and Shipping Cost Paid
    private async Task<IEnumerable<OrderReportToShowDTO>> GetMerchantNameAndAmountReceivedAndShippingCostPaid(IEnumerable<OrderReport> orderreports)
    {
        var orderreportsDto = mapper.Map<IEnumerable<OrderReportToShowDTO>>(orderreports);
        foreach (var orderreport in orderreportsDto)
        {
            var MerchantName = await userManager.FindByIdAsync(orderreport.MerchantId!);
            var Courier = await userManager.FindByIdAsync(orderreport.CourierId!);
            orderreport.MerchantName = MerchantName?.FullName ?? "Unknown";
            orderreport.CompanyValue = Courier?.DeductionCompanyFromOrder ?? 0;

            switch (orderreport.PaymentType)
            {
                case "Collectible":
                    orderreport.AmountReceived = orderreport.ShippingCost + orderreport.OrderCost;
                    orderreport.ShippingCostPaid = 0;
                    break;
                case "Prepaid":
                    orderreport.AmountReceived = 00;
                    orderreport.ShippingCostPaid = orderreport.ShippingCost;
                    break;
                case "Expulsion":
                    orderreport.AmountReceived = orderreport.ShippingCost;
                    orderreport.ShippingCostPaid = 0;
                    break;
                default:
                    orderreport.AmountReceived = 0;
                    orderreport.ShippingCostPaid = 0;
                    break;
            }
        }
        return orderreportsDto;
    }
}
