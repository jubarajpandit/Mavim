namespace Mavim.Manager.Api.WopiFileLock.Repository.Interfaces
{
    public enum WopiFileLockStatus
    {
        FileNotLocked = 0,

        FileLockSuccessful = 1,

        FileAlreadyLocked = 2,

        FileUnLockSuccessful = 3,

        FileUnLockNReLockSuccessful = 4,

        FileLockMismatch = 5,

        FileLockExtendedSuccessful = 6
    }
}