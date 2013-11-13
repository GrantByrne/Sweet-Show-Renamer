using System;
namespace SweetShowRenamer.Lib.Service
{
    interface IFileStorageService<T>
    {
        T Get();
        T Set(T data);
    }
}
