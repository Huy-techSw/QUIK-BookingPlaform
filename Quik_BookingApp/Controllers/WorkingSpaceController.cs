using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.DAO;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Repos.Interface;
using Swashbuckle.AspNetCore.Annotations;
using Firebase.Storage;
using System;
using System.Diagnostics;
using Firebase.Auth;
using Quik_BookingApp.Service;

namespace Quik_BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingSpaceController : ControllerBase
    {
       

        private readonly IWebHostEnvironment environment;
        private readonly QuikDbContext context;
        private readonly IWorkingSpaceService workingSpaceService;
        private readonly ILogger logger;

        public WorkingSpaceController(IWebHostEnvironment environment, QuikDbContext context, IWorkingSpaceService workingSpaceService)
        {
            this.environment = environment;
            this.context = context;
            this.workingSpaceService = workingSpaceService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByLocation([FromQuery] string location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Location is required.");
                }

                var result = await workingSpaceService.SearchWorkingSpacesByLocationAsync(location);

                if (result == null || result.Count == 0)
                {
                    return NotFound("No working spaces found for the specified location.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error searching working spaces by location: {Location}", location);
                return StatusCode(500, "Internal server error");
            }
        }


        [SwaggerOperation(
            Summary = "Retrieve all working spaces",
            Description = "Returns a list of all working spaces. If no working spaces are found, it returns a 404 Not Found response. Optionally, a location can be specified in the path to filter the results."
        )]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string? location = null, string? type = null)
        {
            var data = await workingSpaceService.GetAll(location,type);

            if (data == null || !data.Any())
            {
                return Ok(data);
            }

            return Ok(data);
        }


       

        //[SwaggerOperation(
        //     Summary = "Retrieve all workign space",
        //     Description = "Returns a list of all working spaces. If no workign space are found, it returns a 404 Not Found response."
        // )]
        //[HttpGet("GetAll")]
        //public async Task<IActionResult> GetAll()
        //{
        //    var data = await workingSpaceService.GetAll();
        //    if (data == null || data.Count == 0)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(data);
        //}

        [SwaggerOperation(
             Summary = "Retrieve user by wsId",
             Description = "This API allows you to get WS details by providing a wsId. If the WS is not found, a 404 Not Found response will be returned."
        )]
        [HttpGet("GetById/{workingSpaceId}")]
        public async Task<IActionResult> GetBySpaceId(string workingSpaceId)
        {
            var data = await workingSpaceService.GetBySpaceId(workingSpaceId);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [SwaggerOperation(
             Summary = "New a working space",
             Description = "This API allows you to create a WS without input spaceId and ImageId"
        )]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] WorkingSpaceRequestModel workingSpace)
        {
            var response = await workingSpaceService.CreateWS(workingSpace);
            if (response.ResponseCode == 201)
            {
                return CreatedAtAction(nameof(GetBySpaceId), new { workingSpaceId = workingSpace.SpaceId }, response);

            }
            return StatusCode(response.ResponseCode, response);
        }

        [SwaggerOperation(
             Summary = "Get List theo room type: Không gian làm việc chung",
             Description = "Lấy list theo 4 loại:\r\n- Không gian làm việc chung \r\n- Phòng họp\r\n- Study hub\r\n- Không gian văn phòng"
        )]
        [HttpGet("WorkingArea")]
        public async Task<IActionResult> GetWorkingAreaSpaces()
        {
            var result = await workingSpaceService.GetWorkingSpacesForWorkingAreaAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound("No working area spaces found.");
            }
            return Ok(result);
        }

        [SwaggerOperation(
             Summary = "Get List theo room type: Phòng họp",
             Description = "Lấy list theo 4 loại:\r\n- Không gian làm việc chung \r\n- Phòng họp\r\n- Study hub\r\n- Không gian văn phòng"
        )]
        [HttpGet("CafeLamViec")]
        public async Task<IActionResult> GetMeetingRoomSpaces()
        {
            var result = await workingSpaceService.GetWorkingSpacesForWorkingCafeAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound("No meeting room spaces found.");
            }
            return Ok(result);
        }

        [SwaggerOperation(
             Summary = "Get List theo room type: Study hub",
             Description = "Lấy list theo 4 loại:\r\n- Không gian làm việc chung \r\n- Phòng họp\r\n- Study hub\r\n- Không gian văn phòng"
        )]
        [HttpGet("CommonSpace")]
        public async Task<IActionResult> GetCommonSpaceSpaces()
        {
            var result = await workingSpaceService.GetWorkingSpacesForCommonSpaceAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound("No common spaces found.");
            }
            return Ok(result);
        }

        [SwaggerOperation(
             Summary = "Get List theo room type: Không gian văn phòng",
             Description = "Lấy list theo 4 loại:\r\n- Không gian làm việc chung \r\n- Phòng họp\r\n- Study hub\r\n- Không gian văn phòng"
        )]
        [HttpGet("KhongGianSuKien")]
        public async Task<IActionResult> GetPrivateOfficeSpaces()
        {
            var result = await workingSpaceService.GetWorkingSpacesForEventSpaceAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound("No private office spaces found.");
            }
            return Ok(result);
        }




        [HttpPut("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile formFile, string code)
        {
            APIResponse response = new APIResponse();
            try
            {
                // Check if the formFile is null or empty
                if (formFile == null || formFile.Length == 0)
                {
                    response.ResponseCode = 400;
                    response.Message = "No file uploaded.";
                    return BadRequest(response);
                }

                // Prepare the Firebase storage path
                string imageFileName = $"{code}.png";
                string imagePathInStorage = $"Upload/workingspace/{code}/{imageFileName}";

                // Create a new FirebaseStorage instance using your Firebase project details
                var firebaseStorage = new FirebaseStorage("https://console.firebase.google.com/u/2/project/quik-a8158/storage/quik-a8158.appspot.com/files", // Replace with your Firebase storage URL
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult("https://oauth2.googleapis.com/token"), // Replace with your token
                        ThrowOnCancel = true
                    });

                // Upload the file to Firebase Storage
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);
                    stream.Position = 0; // Reset stream position

                    var uploadTask = await firebaseStorage
                        .Child(imagePathInStorage)
                        .PutAsync(stream);

                    // Prepare the image URL
                    string imageUrl = uploadTask; // This returns the URL of the uploaded image

                    // Create an instance of ImageWS and save to the database
                    var imageWS = new ImageWS
                    {
                        WorkingSpaceName = "Your Working Space Name", // Set as appropriate
                        WSCode = code,
                        WSImages = stream.ToArray() // Get the byte array directly
                    };

                    // Assuming _context is your database context
                    context.Images.Add(imageWS);
                    await context.SaveChangesAsync();

                    // Set the response
                    response.ResponseCode = 200;
                    response.Result = "pass";
                    response.Message = $"Image uploaded to Firebase and saved to database with URL: {imageUrl}";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Ok(response);
        }



        [HttpPut("MultiUploadImage")]
        public async Task<IActionResult> MultiUploadImage(IFormFileCollection filecollection, string code)
        {
            APIResponse response = new APIResponse();
            int passcount = 0; int errorcount = 0;
            try
            {
                string Filepath = GetFilepath(code);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                foreach (var file in filecollection)
                {
                    string imagepath = Filepath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await file.CopyToAsync(stream);
                        passcount++;

                    }
                }


            }
            catch (Exception ex)
            {
                errorcount++;
                response.Message = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + " Files uploaded &" + errorcount + " files failed";
            return Ok(response);
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string code)
        {
            string Imageurl = string.Empty;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(code);
                string imagepath = Filepath + "\\" + code + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    Imageurl = hosturl + "/Upload/" + code + "/" + code + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }

        [HttpGet("GetMultiImage")]
        public async Task<IActionResult> GetMultiImage(string code)
        {
            List<string> Imageurl = new List<string>();
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(code);

                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = Filepath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string _Imageurl = hosturl + "/Upload/" + code + "/" + filename;
                            Imageurl.Add(_Imageurl);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }

        [HttpGet("Download")]
        public async Task<IActionResult> download(string code)
        {
            // string Imageurl = string.Empty;
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(code);
                string imagepath = Filepath + "\\" + code + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    MemoryStream stream = new MemoryStream();
                    using (FileStream fileStream = new FileStream(imagepath, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(stream);
                    }
                    stream.Position = 0;
                    return File(stream, "image/png", code + ".png");
                    //Imageurl = hosturl + "/Upload/" + code + "/" + code + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }

        [HttpGet("Remove")]
        public async Task<IActionResult> remove(string code)
        {
            // string Imageurl = string.Empty;
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(code);
                string imagepath = Filepath + "\\" + code + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                    return Ok("pass");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }

        [HttpGet("MultiRemove")]
        public async Task<IActionResult> multiremove(string code)
        {
            // string Imageurl = string.Empty;
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(code);
                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        fileInfo.Delete();
                    }
                    return Ok("pass");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }

        [HttpPut("DBMultiUploadImage")]
        public async Task<IActionResult> DBMultiUploadImage(IFormFileCollection filecollection, string code)
        {
            APIResponse response = new APIResponse();
            int passcount = 0; int errorcount = 0;
            try
            {
                foreach (var file in filecollection)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        this.context.Images.Add(new DAO.Models.ImageWS()
                        {
                            WSCode = code,
                            WSImages = stream.ToArray()
                        });
                        await this.context.SaveChangesAsync();
                        passcount++;
                    }
                }


            }
            catch (Exception ex)
            {
                errorcount++;
                response.Message = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + " Files uploaded &" + errorcount + " files failed";
            return Ok(response);
        }


        [HttpGet("GetDBMultiImage")]
        public async Task<IActionResult> GetDBMultiImage(string code)
        {
            List<string> Imageurl = new List<string>();
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                var _image = this.context.Images.Where(item => item.WSCode == code).ToList();
                if (_image != null && _image.Count > 0)
                {
                    _image.ForEach(item =>
                    {
                        Imageurl.Add(Convert.ToBase64String(item.WSImages));
                    });
                }
                else
                {
                    return NotFound();
                }
                //string Filepath = GetFilepath(code);

                //if (System.IO.Directory.Exists(Filepath))
                //{
                //    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                //    FileInfo[] fileInfos = directoryInfo.GetFiles();
                //    foreach (FileInfo fileInfo in fileInfos)
                //    {
                //        string filename = fileInfo.Name;
                //        string imagepath = Filepath + "\\" + filename;
                //        if (System.IO.File.Exists(imagepath))
                //        {
                //            string _Imageurl = hosturl + "/Upload/" + code + "/" + filename;
                //            Imageurl.Add(_Imageurl);
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }


        [HttpGet("DbDownload")]
        public async Task<IActionResult> dbdownload(string code)
        {

            try
            {

                var _image = await this.context.Images.FirstOrDefaultAsync(item => item.WSCode == code);
                if (_image != null)
                {
                    return File(_image.WSImages, "image/png", code + ".png");
                }


                //string Filepath = GetFilepath(code);
                //string imagepath = Filepath + "\\" + code + ".png";
                //if (System.IO.File.Exists(imagepath))
                //{
                //    MemoryStream stream = new MemoryStream();
                //    using (FileStream fileStream = new FileStream(imagepath, FileMode.Open))
                //    {
                //        await fileStream.CopyToAsync(stream);
                //    }
                //    stream.Position = 0;
                //    return File(stream, "image/png", code + ".png");
                //    //Imageurl = hosturl + "/Upload/" + code + "/" + code + ".png";
                //}
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }

        [NonAction]
        private string GetFilepath(string code)
        {
            return this.environment.WebRootPath + "\\Upload\\" + code;
        }
    }
}
