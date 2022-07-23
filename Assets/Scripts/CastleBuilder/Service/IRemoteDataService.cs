using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using PG.CastleBuilder.Model.Response;
using RSG;
using UnityEngine;

namespace PG.CastleBuilder.Service
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