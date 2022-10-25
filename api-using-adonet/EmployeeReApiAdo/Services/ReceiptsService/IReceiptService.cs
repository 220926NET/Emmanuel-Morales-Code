
using System.Security.Claims;
public interface IReceiptService
{

    public Task<ServiceResponse<string>> UploadReceipts(ICollection<IFormFile> receipts, ClaimsIdentity identity);
    public ServiceResponse<List<Receipt>> GetAllReceipts(ClaimsIdentity identity);

    public Task<ServiceResponse<string>> DeleteAllReceipts(ClaimsIdentity identity);

}