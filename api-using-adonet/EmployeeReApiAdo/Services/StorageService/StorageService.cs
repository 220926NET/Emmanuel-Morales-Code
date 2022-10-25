// using System.Security.Claims; 
// using Microsoft.Extensions.Configuration; 
// public class StorageService : IStorageService {

//     private readonly DbContext _dbContext ; 
    
//     private readonly BlobStorage _blobStorage; 
 

//     public StorageService(DbContext dbContext)
//     {
//         _dbContext = dbContext; 
//         // var key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AzureBlobKey")["Key"];
        
        
//         _blobStorage = new BlobStorage();
        
//     }
//         public async Task<ServiceResponse<string>> UploadPhoto(IFormFile image, ClaimsIdentity identity){

//             ServiceResponse<string> uploadPhotoRes = new ServiceResponse<string>(); 

//             IEnumerable<Claim> claims = identity!.Claims; 
//             int employeeId = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);
//             string userName = identity.FindFirst(c=> c.Type == ClaimTypes.Name)!.Value; 

//             string fileExtension = string.Empty;
//             try{
//                 string[] fileName = image.FileName.Split(".");
//                 fileExtension = fileName[1]; 
//                 if(fileExtension != "jpg" && fileExtension != "png"){
//                     uploadPhotoRes.Success =false; 
//                     uploadPhotoRes.Message = "Photo must be a .jpg or .png"; 
//                     return uploadPhotoRes; 
//                 }
//             } catch(IndexOutOfRangeException){
//                     uploadPhotoRes.Success =false; 
//                     uploadPhotoRes.Message = "Photo must have a .jpg or .png extension"; 
//                     return uploadPhotoRes; 
//             }
                
//             //Console.WriteLine("content type is " + employeePhoto.ContentType); 
//             // errors specified blob already exists 
//             // check that user has not files uploaded 

//             if(_dbContext.hasProfilePhoto(employeeId)){
//                 uploadPhotoRes.Success =false; 
//                 uploadPhotoRes.Message = "Employee already has a photo, delete it and try adding a new photo."; 
//                 return uploadPhotoRes; 
//             }

//             uploadPhotoRes =  await _blobStorage.uploadPhoto(userName, image, fileExtension); 

//             if(uploadPhotoRes.Success){
//                 // get photo url and upload it to db with employeeId
//                 uploadPhotoRes = _dbContext.UploadEmployeePhoto(employeeId, uploadPhotoRes.Data!); 
//                 return uploadPhotoRes; 
//          } 

//              return uploadPhotoRes; 
//         }


    

// }