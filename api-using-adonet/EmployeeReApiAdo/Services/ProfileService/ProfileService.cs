
using System.Security.Claims;
public class ProfileService : IProfileService
{

    private readonly DbContext _dbContext = new DbContext();
    private readonly BlobStorage _blobStorage = new BlobStorage();
    public ServiceResponse<string> addEmployeeDetails(EmployeeDetails employeeDetails, ClaimsIdentity identity)
    {
        IEnumerable<Claim> claims = identity!.Claims;
        int employeeId = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);

        ServiceResponse<string> addEmployeeDetailsRes = _dbContext.AddEmployeeDetails(employeeDetails, employeeId);


        return addEmployeeDetailsRes;
    }

    public ServiceResponse<string> getEmployeePhoto(ClaimsIdentity identity)
    {
        IEnumerable<Claim> claims = identity!.Claims;
        int employeeId = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);

        //TODO: Return if user has not profile pic

        ServiceResponse<string> getEmployeePhotoRes = new ServiceResponse<string>();
        // return photo url form database 
        getEmployeePhotoRes = _dbContext.getPhotoUrl(employeeId);


        return getEmployeePhotoRes;
    }

    //uploads user file to storage
    //uploads url and file extension to database 
    public async Task<ServiceResponse<string>> UploadEmployeePhoto(IFormFile image, ClaimsIdentity identity)
    {

        ServiceResponse<string> uploadPhotoRes = new ServiceResponse<string>();

        IEnumerable<Claim> claims = identity!.Claims;
        int employeeId = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);
        string userName = identity.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;

        string fileExtension = string.Empty;

        try
        {
            string[] fileName = image.FileName.Split(".");
            fileExtension = fileName[1];
            if (fileExtension != "jpg" && fileExtension != "png")
            {
                uploadPhotoRes.Success = false;
                uploadPhotoRes.Message = "Photo must be a .jpg or .png";
                return uploadPhotoRes;
            }
        }
        catch (IndexOutOfRangeException)
        {
            uploadPhotoRes.Success = false;
            uploadPhotoRes.Message = "Photo must have a .jpg or .png extension";
            return uploadPhotoRes;
        }



        if (_dbContext.hasProfilePhoto(employeeId))
        {
            uploadPhotoRes.Success = false;
            uploadPhotoRes.Message = "Employee already has a photo, delete it and try adding a new photo.";
            return uploadPhotoRes;
        }

        uploadPhotoRes = await _blobStorage.uploadPhoto(userName, image, fileExtension);

        if (uploadPhotoRes.Success)
        {
            // get photo url and upload it to db with employeeId
            uploadPhotoRes = _dbContext.UploadEmployeePhoto(employeeId, uploadPhotoRes.Data!, fileExtension);
            return uploadPhotoRes;
        }
        else
        {
            uploadPhotoRes.Success = false;
            uploadPhotoRes.Message = "Unable to upload image.";
            return uploadPhotoRes;

        }


    }


    public async Task<ServiceResponse<string>> deleteEmployeePhoto(ClaimsIdentity identity)
    {

        ServiceResponse<string> deletePhotoRes = new ServiceResponse<string>();

        IEnumerable<Claim> claims = identity!.Claims;
        int employeeId = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);
        string employeeUserName = identity.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;


        //deletePhotoDbRes = _dbContext.deleteUserPhoto(employeeId); 
        deletePhotoRes = _dbContext.deleteUserPhoto(employeeId);
        // need file name "username" + "file extension" 
        if (!deletePhotoRes.Success)
        {
            deletePhotoRes.Data = null;
            deletePhotoRes.Success = false;
            deletePhotoRes.Message = "Database error";
            return deletePhotoRes;
        }
        if (deletePhotoRes.Success)
        {
            ServiceResponse<string> deletePhotoFromStorageRes = await _blobStorage.DeletePhoto(employeeUserName, deletePhotoRes.Data!);
            if (!deletePhotoFromStorageRes.Success)
            {
                deletePhotoRes.Data = null;
                deletePhotoRes.Success = true;
                deletePhotoRes.Message = "No picture to delete!";
            }
            // todo check if delete form storage was successful 
        }
        else
        {
            return deletePhotoRes;
        }

        return deletePhotoRes;



    }





}