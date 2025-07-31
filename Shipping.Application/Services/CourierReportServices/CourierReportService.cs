using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Application.Abstraction.CourierReport.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.CourierReportServices;
public class CourierReportService(IUnitOfWork _unitOfWork,
    IMapper _mapper) : ICourierReportService
{
    // Get All Courier Reports
    public async Task<IEnumerable<GetAllCourierOrderCountDTO>> GetAllCourierReportAsync(PaginationParameters pramter)
    {
        var courierReports = await _unitOfWork.GetRepository<CourierReport, int>().GetAllAsync(pramter,
            p => p.Include(c => c.Courier));

        if (courierReports is null)
            throw new KeyNotFoundException($"CourierReport not found.");

        var reportDTOs = _mapper.Map<List<CourierReportDTO>>(courierReports);

        var grouped = reportDTOs
            .GroupBy(r => r.CourierName)
            .Select(g => new GetAllCourierOrderCountDTO
            {
                CourierName = g.Key ?? "Unknown Courier",  // handle nulls if any
                OrdersCount = g.Count()
            }).ToList();

        return grouped;
    }

    // Get Courier Report By Id
    public async Task<CourierReportDTO> GetCourierReportAsync(int id, PaginationParameters parameter)
    {
        var courierReport = await _unitOfWork.GetRepository<CourierReport, int>().GetByIdAsync(id,
            p => p
            .Include(c => c.Courier)
            .Include(c => c.Order)
                .ThenInclude(c => c.CitySetting)
            .Include(c => c.Order)
                .ThenInclude(c => c.Products));

        if (courierReport is null)
            throw new KeyNotFoundException($"CitySetting with ID {id} not found.");

        var reportDTO = _mapper.Map<CourierReportDTO>(courierReport);
        var user = await _unitOfWork.GetRepository<ApplicationUser, int>().GetAllAsync(parameter);

        var merchant = user.FirstOrDefault(u => u.Id == reportDTO.MerchantId);

        reportDTO.ClientName = merchant?.StoreName ?? "Unknown StoreName";

        return reportDTO;
    }
}
