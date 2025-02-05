using Azure.Core;
using DataLayer.Context;
using DataLayer.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Provider;

public class MetricService : IMetricService
{
    private readonly ILogger<BillService> _logger;
    private readonly IMSDbContext _context;

    public MetricService(ILogger<BillService> logger, IMSDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<MetricDto>> GetDashboardMetricsAsync()
    {
        try
        {
            var customers = await _context.Customers.ToListAsync();
            var installments = await _context.Installments.ToListAsync();
            var payments = await _context.Payments.ToListAsync();
            var bills = await _context.Bills.ToListAsync();
            var metrics = new List<MetricDto>();

            var totalCustomers = customers.Count;
            var totalInstallments = installments.Count;
            var totalPayments = payments.Sum(p => p.Amount);
            var totalBills = bills.Sum(p => p.Amount);

            metrics.Add(new MetricDto("Total Customers", totalCustomers.ToString()));
            metrics.Add(new MetricDto("Total Installments", totalInstallments.ToString()));
            metrics.Add(new MetricDto("Total Payments", string.Format("{0:0.00}", totalPayments)));
            metrics.Add(new MetricDto("Total Bills", string.Format("{0:0.00}", totalBills)));

            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while fetching metrics");
            throw;
        }
    }
}
