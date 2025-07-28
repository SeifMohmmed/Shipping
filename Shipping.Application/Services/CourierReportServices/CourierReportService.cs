using AutoMapper;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Application.Abstraction.CourierReport.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.CourierReportServices;
public class CourierReportService(IUnitOfWork _unitOfWork,
    IMapper _mapper) : ICourierReportService
{
    public async Task<IEnumerable<GetAllCourierOrderCountDTO>> GetAllCourierReportAsync(PaginationParameters pramter)
    {
        var courierReports = await _unitOfWork.GetRepository<CourierReport, int>().GetAllAsync(pramter);

        if (courierReports is null)
            throw new KeyNotFoundException($"CourierReport not found.");

        var reportDTOs = _mapper.Map<List<CourierReportDTO>>(courierReports);

        List<GetAllCourierOrderCountDTO> getAllCourierOrderCounts = new();

        getAllCourierOrderCounts.Add(new GetAllCourierOrderCountDTO
        {
            CourierName = reportDTOs.Select(c => c.CourierName).FirstOrDefault(),
            OrdersCount = reportDTOs.Select(o => o.OrderId).Count()
        });

        return getAllCourierOrderCounts;
    }

    public async Task<CourierReportDTO> GetCourierReportAsync(int id)
    {
        var courierReport = await _unitOfWork.GetRepository<CourierReport, int>().GetByIdAsync(id);

        if (courierReport is null)
            throw new KeyNotFoundException($"CitySetting with ID {id} not found.");

        var reportDTO = _mapper.Map<CourierReportDTO>(courierReport);

        //TODO : Add ApplicationUser

        return reportDTO;
    }
}
