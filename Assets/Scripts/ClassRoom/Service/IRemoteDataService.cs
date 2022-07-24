using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using PG.ClassRoom.Model.Response;
using RSG;
using UnityEngine;

namespace PG.ClassRoom.Service
{
    public interface IRemoteDataService
    {
        IPromise<MetaData> CreateMetaData(MetaData metaData);
        IPromise<MetaData> GetMetaData();
        IPromise<UserData> SaveUserData(UserData userData);
        IPromise<UserData> GetUserData();
        IPromise<AddNewModuleResponse> AddNewModule(uint moduleStaticId, Vector3 position, bool autoPlace);
        IPromise RemoveModule(long moduleRemoteId);
        IPromise UpdateModulePosition(ModuleRemoteDataModel moduleModel, Vector3 oldPos, Vector3 newPos);
    }
}