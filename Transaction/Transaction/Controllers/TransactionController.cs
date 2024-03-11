using Microsoft.AspNetCore.Mvc;
using Transaction.BLL.Services.Interfaces;
using Transaction.Common.Configs;
using Microsoft.Extensions.Options;

namespace Transaction.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ExcelConfig _excelConfig;

    public TransactionController(
        ITransactionService transactionService,
        IOptions<ExcelConfig> excelConfig)
    {
        _transactionService = transactionService;
        _excelConfig = excelConfig.Value;
    }

    /// <summary>
    /// Retrieves a file containing transactions based on specified column names.
    /// </summary>
    /// Sample request:
    ///
    ///     GET /api/transaction/get-file?columnsName=Name,Email,Amount
    ///
    /// </remarks>
    /// <param name="columnsName">A list of column names to include in the file.</param>
    /// <returns>The file containing transactions.</returns>
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFile([FromQuery] List<string> columnsName)
    {
        try
        {
            var result = await _transactionService.GetTransactionsFile(columnsName);
            return File(result, _excelConfig.ContentType, _excelConfig.FileDownloadName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Receives transactions from 2023 in the user's specified time zone.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/transaction/get2023transactions-in-user-tz?userTimeZone=America/Chicago
    ///
    /// </remarks>
    /// <param name="userTimeZone">The time zone ID of the user.</param>
    /// <returns>The list of transactions from 2023 in the user's specified time zone.</returns>
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get2023TransactionsInUserTZ(string userTimeZone)
    {
        try
        {
            var result = await _transactionService.Get2023TransactionsInUserTZ(userTimeZone);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Receives transactions from 2023 in all clients time zones.
    /// </summary>
    /// Sample request:
    ///
    ///     GET /api/transaction/get2023transactions-by-client-tz
    ///
    /// </remarks>
    /// <returns>The list of transactions from 2023 in all clients time zones.</returns>
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get2023TransactionsByClientTZ()
    {
        try
        {
            var result = await _transactionService.Get2023TransactionsByClientTZ();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Receives transactions from 2024 in the user's specified time zone.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/transaction/get2024transactions-in-user-tz?userTimeZone=America/Chicago
    ///
    /// </remarks>
    /// <param name="userTimeZone">The time zone ID of the user.</param>
    /// <returns>The list of transactions from 2024 in the user's specified time zone.</returns>
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get2024TransactionsInUserTZ(string userTimeZone)
    {
        try
        {
            var result = await _transactionService.Get2024TransactionsInUserTZ(userTimeZone);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Receives transactions from 2024 in all clients time zones.
    /// </summary>
    /// Sample request:
    ///
    ///     GET /api/transaction/get2024transactions-by-client-tz
    ///
    /// </remarks>
    /// <returns>The list of transactions from 2024 in all clients time zones.</returns>
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get2024TransactionsByClientTZ()
    {
        try
        {
            var result = await _transactionService.Get2024TransactionsByClientTZ();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
