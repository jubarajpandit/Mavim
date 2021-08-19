namespace Mavim.Manager.Api.WopiHost.Models
{
    /// <summary>
    /// Enumeration for the different types of WOPI Requests
    /// For more information see: https://wopi.readthedocs.org/projects/wopirest/en/latest/index.html
    /// </summary>
    public enum WopiRequestType
    {
        None,
        CheckFileInfo,
        GetFile,
        Lock,
        GetLock,
        RefreshLock,
        Unlock,
        UnlockAndRelock,
        PutFile,
        PutRelativeFile,
        RenameFile,
        PutUserInfo
    }
}
