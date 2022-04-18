namespace UIM.Core.Services.Interfaces;

public interface IGoogleDriveService
{
    void DeleteFile(string fileId);
    Attachment UploadFile(Stream file, string name, string description, string mime);
}
