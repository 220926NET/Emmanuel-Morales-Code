using System.Security.Claims;
public class ReceiptService : IReceiptService
{

    private readonly DbContext _dbContext = new DbContext();
    private readonly BlobStorage _blobStorage = new BlobStorage();
    public async Task<ServiceResponse<string>> UploadReceipts(ICollection<IFormFile> receipts, ClaimsIdentity identity)
    {

        // verify that receit names are unique
        // 

        HashSet<string> uniqueFileNames = new HashSet<string>();
        ServiceResponse<string> uploadReceiptsRes = new ServiceResponse<string>();

        foreach (FormFile receipt in receipts)
        {
            uniqueFileNames.Add(receipt.FileName);
        }
        // only allow 3 files to be updated at a time 
        if (receipts.Count > 3)
        {
            uploadReceiptsRes.Success = false;
            uploadReceiptsRes.Message = "You can only upload 3 receipts at a time";
            return uploadReceiptsRes;
        }
        // check if file names are all unique 
        if (uniqueFileNames.Count != receipts.Count)
        {
            uploadReceiptsRes.Success = false;
            uploadReceiptsRes.Message = "Please ensure that all receipts have unique file names!";
            return uploadReceiptsRes;
        }

        IEnumerable<Claim> claims = identity!.Claims;
        int employeeId = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);
        string userName = identity.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;



        string fileExtension = string.Empty;

        if (receipts.Count == 0)
        {
            uploadReceiptsRes.Success = false;
            uploadReceiptsRes.Message = "Please add a pdf file.";
            return uploadReceiptsRes;
        }

        try
        {

            bool fileAlreadyExists = false;
            string duplicateFileName = string.Empty;
            foreach (FormFile receipt in receipts)
            {

                string[] fileName = receipt.FileName.Split(".");
                fileExtension = fileName[1];
                if (fileExtension != "pdf")
                {
                    uploadReceiptsRes.Success = false;
                    uploadReceiptsRes.Message = "Receipts must be in pdf format (.pdf) extension.";
                    return uploadReceiptsRes;
                }

                // checks db to see if file already exists 
                fileAlreadyExists = _dbContext.EmployeeReceiptExists(employeeId, receipt.FileName);
                if (fileAlreadyExists)
                {
                    duplicateFileName = receipt.FileName;
                }

            }

            if (fileAlreadyExists)
            {
                uploadReceiptsRes.Success = false;
                uploadReceiptsRes.Message = "Unable to add duplicate file of name " + duplicateFileName + ". Please rename file or delete duplicate!";
                return uploadReceiptsRes;
            }

            uploadReceiptsRes = await _blobStorage.UploadReceipts(receipts, userName, employeeId);


        }
        catch (IndexOutOfRangeException)
        {
            uploadReceiptsRes.Success = false;
            uploadReceiptsRes.Message = "Photo must have a .pdf extension";
            return uploadReceiptsRes;
        }


        return uploadReceiptsRes;
    }

    public ServiceResponse<List<Receipt>> GetAllReceipts(ClaimsIdentity identity)
    {

        IEnumerable<Claim> claims = identity!.Claims;
        int employeeId = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);

        ServiceResponse<List<Receipt>> getAllReceiptsRes = new ServiceResponse<List<Receipt>>();

        getAllReceiptsRes = _dbContext.GetAllEmployeeReceipts(employeeId);



        return getAllReceiptsRes;
    }

    public async Task<ServiceResponse<string>> DeleteAllReceipts(ClaimsIdentity identity)
    {


        ServiceResponse<string> deleteAllReceiptsRes = new ServiceResponse<string>();

        IEnumerable<Claim> claims = identity!.Claims;
        int employeeId = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);
        string userName = identity.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;

        ServiceResponse<List<string>> deleteAllReceiptsDbRes = _dbContext.DeleteAllReceipts(employeeId);

        if (!deleteAllReceiptsDbRes.Success)
        {
            deleteAllReceiptsRes.Data = null;
            deleteAllReceiptsRes.Message = deleteAllReceiptsDbRes.Message;
            deleteAllReceiptsRes.Success = false;
            return deleteAllReceiptsRes;

        }
        //TODO delete from storage 

        // deleteAllReceiptRes.message is set to deleteALlReceiptDbRes.message because if there are no tickets to delete , deleteAllReceiptsRes message wont be set
        deleteAllReceiptsRes.Message = deleteAllReceiptsDbRes.Message;

        if (deleteAllReceiptsDbRes.Data != null)
        {
            // TODO: Catch an exception blob storage may throw
            deleteAllReceiptsRes = await _blobStorage.DeleteAllReceipts(deleteAllReceiptsDbRes.Data, userName);

        }

        return deleteAllReceiptsRes;
    }

}