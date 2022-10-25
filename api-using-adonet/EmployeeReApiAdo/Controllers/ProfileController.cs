using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private IProfileService _ProfileService;

    private readonly DbContext _dbContext = new DbContext();


    public ProfileController(IProfileService ProfileService)
    {
        _ProfileService = ProfileService;

    }

    // TODO: check if user has already set profile details 
    [HttpPost]
    [Route("addProfileDetails")]
    public ActionResult<ServiceResponse<string>> addEmployeeDetails(EmployeeDetails employeeDetails)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        ServiceResponse<string> response = new ServiceResponse<string>();

        response = _ProfileService.addEmployeeDetails(employeeDetails, identity!);

        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);

    }

    [HttpPost]
    [Route("setProfilePhoto")]
    public async Task<ActionResult<ServiceResponse<string>>> addEmployeePhoto(IFormFile employeePhoto)
    {

        var identity = HttpContext.User.Identity as ClaimsIdentity;

        ServiceResponse<string> addEmployeePhotoRes = new ServiceResponse<string>();

        addEmployeePhotoRes = await _ProfileService.UploadEmployeePhoto(employeePhoto, identity!);
        if (!addEmployeePhotoRes.Success)
        {
            return BadRequest(addEmployeePhotoRes);
        }
        return Ok(addEmployeePhotoRes);

    }

    [HttpGet]
    [Route("getProfilePhoto")]
    public ActionResult<ServiceResponse<string>> getMyPhoto()
    {

        var identity = HttpContext.User.Identity as ClaimsIdentity;

        ServiceResponse<string> response = new ServiceResponse<string>();

        response = _ProfileService.getEmployeePhoto(identity!);
        if (response.Success)
        {
            return Ok(response);
        }

        return StatusCode(500);

    }

    [HttpDelete]

    [Route("deleteProfilePhoto")]

    public async Task<ActionResult<ServiceResponse<string>>> deleteProfilePhoto()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        ServiceResponse<string> deletePhotoRes = new ServiceResponse<string>();

        deletePhotoRes = await _ProfileService.deleteEmployeePhoto(identity!);

        //TODO: Add blob storage service to check for exceptions thrown from storage

        if (!deletePhotoRes.Success)
        {

            return StatusCode(500);
        }
        // Message can be set to 
        deletePhotoRes.Message = "Profile pic has been deleted";
        return Ok(deletePhotoRes);


    }


}