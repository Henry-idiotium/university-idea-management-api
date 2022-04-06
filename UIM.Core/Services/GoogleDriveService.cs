using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Gapi = UIM.Core.Helpers.EnvVars.Gapi;

namespace UIM.Core.Services;

public class GoogleDriveService : IGoogleDriveService
{
    private readonly DriveService _driveService;

    public GoogleDriveService()
    {
        _driveService = new DriveService(
            new BaseClientService.Initializer()
            {
                ApiKey = Gapi.Key,
                ApplicationName = "UIM",
                HttpClientInitializer = GoogleCredential
                    .FromJsonParameters(
                        new()
                        {
                            Type = Gapi.ServiceAccount.Type,
                            ClientEmail = Gapi.ServiceAccount.Email,
                            PrivateKey = Gapi.PrivateKey,
                        }
                    )
                    .CreateScoped(DriveService.ScopeConstants.Drive),
            }
        );
    }

    public void DeleteFile(string fileId) => _driveService.Files.Delete(fileId).Execute();

    public Attachment UploadFile(Stream file, string name, string description, string mime)
    {
        if (EnvVars.UseGoogleDrive)
            return new();

        var metadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = name,
            Description = description,
            MimeType = mime,
            Parents = new string[] { Gapi.FolderId }
        };

        var request = _driveService.Files.Create(metadata, file, mime);
        request.Fields = "*";

        var response = request.Upload();
        if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
            throw new HttpException(HttpStatusCode.InternalServerError, response.Exception.Message);

        return new()
        {
            Name = name,
            FileId = request.ResponseBody.Id,
            Url = request.ResponseBody.WebViewLink,
        };
    }
}
