using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly IReceiptService _receiptService;
    public ReceiptsController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }

    [HttpPost]
    [Route("addReceipts")]
    public async Task<ServiceResponse<string>> AddEmployeePhoto(ICollection<IFormFile> receipts)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        ServiceResponse<string> addEmployeeReceipts = new ServiceResponse<string>();

        addEmployeeReceipts = await _receiptService.UploadReceipts(receipts, identity!);

        if (addEmployeeReceipts.Success)
        {
            addEmployeeReceipts.Message = "Successfully added receipt.";
        }

        return addEmployeeReceipts;


    }

    [HttpGet]
    [Route("getAllReceipts")]
    public ServiceResponse<List<Receipt>> GetAllEmployeeReceipts()
    {

        var identity = HttpContext.User.Identity as ClaimsIdentity;

        ServiceResponse<List<Receipt>> getAllReceiptsRes = new ServiceResponse<List<Receipt>>();



        getAllReceiptsRes = _receiptService.GetAllReceipts(identity!);


        return getAllReceiptsRes;


    }

    [HttpDelete]
    [Route("deleteAllReceipts")]
    public async Task<ActionResult<ServiceResponse<string>>> DeleteAllReceipts()
    {

        var identity = HttpContext.User.Identity as ClaimsIdentity;

        ServiceResponse<string> deleteAllReceiptsRes = new ServiceResponse<string>();

        deleteAllReceiptsRes = await _receiptService.DeleteAllReceipts(identity!);

        if (!deleteAllReceiptsRes.Success)
        {
            return StatusCode(500);
        }

        return deleteAllReceiptsRes;


    }


}