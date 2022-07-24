using PG.ClassRoom.Model.Data;
using PG.Core.Installer;
using RSG;

namespace PG.ClassRoom.Context.Bootstrap
{
    public class LoadStaticDataSignal : Signal
    {
        public Promise LoadStaticData()
        {
            this.Fire();

            return OnComplete;
        }
    }

    public class LoadUserDataSignal : Signal
    {
        public Promise LoadUserData()
        {
            this.Fire();

            return OnComplete;
        }
    }

    public class CreateMetaDataSignal : Signal
    {
        public MetaData MetaData;

        public Promise CreateMetaData(MetaData metaData)
        {
            MetaData = metaData;
            this.Fire();

            return OnComplete;
        }
    }
    
    public class CreateUserDataSignal : Signal
    {
        public UserData UserData;

        public Promise CreateUserData(UserData userData)
        {
            UserData = userData;
            this.Fire();

            return OnComplete;
        }
    }
}