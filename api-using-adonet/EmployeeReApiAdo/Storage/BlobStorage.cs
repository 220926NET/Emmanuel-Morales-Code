using Azure.Storage.Blobs;
using Azure.Storage;

public class BlobStorage
{



    private readonly BlobServiceClient _blobServiceClient = new BlobServiceClient(new Uri("https://storagetestrev.blob.core.windows.net"), new StorageSharedKeyCredential("storagetestrev", "Your key here"));

    private readonly DbContext _dbContext = new DbContext();

    public async Task<ServiceResponse<string>> uploadPhoto(string userName, IFormFile employeeImage, string extension)
    {

        ServiceResponse<string> uploadPhotoRes = new ServiceResponse<string>();
        try
        {


            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("userphotos");

            BlobClient blobClient = container.GetBlobClient(userName + "." + extension);


            MemoryStream memoryStream = new MemoryStream();
            await employeeImage.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            var url = blobClient.Uri.AbsoluteUri;

            uploadPhotoRes.Data = url;
            uploadPhotoRes.Success = true;
            uploadPhotoRes.Message = "Successfully created user photo!";


        }
        catch (Azure.RequestFailedException)
        {

            uploadPhotoRes.Success = false;
            uploadPhotoRes.Message = "Sorry user photo already exists. Please delete and re-upload.";
        }

        return uploadPhotoRes;
    }

    public async Task<ServiceResponse<string>> DeletePhoto(string userName, string extension)
    {
        ServiceResponse<string> deletePhotoRes = new ServiceResponse<string>();
        try
        {

            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("userphotos");

            // get 



            Azure.Response response = await container.DeleteBlobAsync(userName + "." + extension);


            deletePhotoRes.Message = "Photo Successfully Deleted";
            deletePhotoRes.Success = true;



        }
        catch (Azure.RequestFailedException)
        {
            deletePhotoRes.Message = "Unable to delete Photo unexpected error occured. ";
            deletePhotoRes.Success = false;
        }

        return deletePhotoRes;
    }


    public async Task<ServiceResponse<string>> UploadReceipts(ICollection<IFormFile> employeeReceipts, string userName, int employeeId)
    {

        ServiceResponse<string> uploadPhotoRes = new ServiceResponse<String>();
        ServiceResponse<string> uploadReceiptsRes = new ServiceResponse<string>();
        List<string> fileNames = new List<string>();

        try
        {

            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("employeereceipts");
            List<string> receiptUrlList = new List<string>();

            foreach (IFormFile receipt in employeeReceipts)
            {
                // what if blobclient already exists?
                // file name is the same?
                // throw excpetion
                // TODO: file is written over if name is same
                // save file as file name 
                // check db if file already exists?
                fileNames.Add(receipt.FileName);
                string[] nameArr = receipt.FileName.Split(".");


                BlobClient blobClient = container.GetBlobClient(userName + nameArr[0] + ".pdf");

                MemoryStream memoryStream = new MemoryStream();

                await receipt.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // keep track of current file being uploaded to return to user if an exception is thrown
                uploadPhotoRes.Data = receipt.FileName;

                await blobClient.UploadAsync(memoryStream);
                var url = blobClient.Uri.AbsoluteUri;
                receiptUrlList.Add(url);


            }

            //TODO: remove UploadEmployeeReceipts receipts to receipt esrvice 
            uploadPhotoRes = _dbContext.UploadEmployeeReceipts(employeeId, receiptUrlList, fileNames);

            return uploadPhotoRes;

        }
        catch (Azure.RequestFailedException)
        {

            uploadPhotoRes.Success = false;
            uploadPhotoRes.Message = "Sorry one of your receipts was saved under the same name please change the FileName of " + uploadPhotoRes.Data;
        }

        return uploadPhotoRes;
    }

    public async Task<ServiceResponse<string>> DeleteAllReceipts(List<string> fileNames, string userName)
    {
        ServiceResponse<string> deletePhotoRes = new ServiceResponse<string>();
        try
        {

            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("employeereceipts");

            foreach (string filename in fileNames)
            {
                string completeFileName = userName + filename;
                Azure.Response response = await container.DeleteBlobAsync(completeFileName);

            }
            deletePhotoRes.Message = "Receipts have been deleted from storage.";
            deletePhotoRes.Success = true;

        }
        catch (Azure.RequestFailedException)
        {

            deletePhotoRes.Message = "Unable to delete Photo unexpected error occured. ";
            deletePhotoRes.Success = false;
        }

        return deletePhotoRes;
    }



}