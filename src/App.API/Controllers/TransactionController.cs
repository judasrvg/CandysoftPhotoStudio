﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.Abstractions.Services;
using App.Application.DTOs;
using App.Application.Services.Command;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionQueryService _transactionQueryService;
    private readonly ITransactionCommandService _transactionCommandService;

    public TransactionController(ITransactionQueryService transactionQueryService, ITransactionCommandService transactionCommandService)
    {
        _transactionQueryService = transactionQueryService;
        _transactionCommandService = transactionCommandService;
    }

    // Endpoint para obtener transacciones aplicando filtros de rango de fecha y tipo de transacción
    [HttpPost("GetFilteredTransactions")]
    public async Task<IActionResult> GetFilteredTransactions([FromBody] TransactionFilterDto filter)
    {
        var transactions = await _transactionQueryService.GetTransactionsAsync();

        // Aplicar filtros de rango de fecha
        if (filter.StartDate.HasValue)
        {
            transactions = transactions.Where(t => t.TransactionDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            transactions = transactions.Where(t => t.TransactionDate <= filter.EndDate.Value);
        }

        // Aplicar filtro de tipo de transacción si se especifica
        if (filter.TransactionType.HasValue)
        {
            transactions = transactions.Where(t => t.TransactionType == filter.TransactionType.Value);
        }

        // Aplicar filtro de tipo de transacciónGrupo 
        if (filter.TransactionGroup.HasValue)
        {
            transactions = transactions.Where(t => t.TransactionGroup == filter.TransactionGroup.Value);
        }

        return Ok(transactions);
    }

    // DELETE: api/Product/{id}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteTransaction(long id)
    {
        await _transactionCommandService.DeleteTransactionAsync(id);
        return Ok(id);
    }
}
