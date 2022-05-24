using Newtonsoft.Json;
using PdUtils.Json;
using Zenject;

namespace Utils.ThreadLocalStorage
{
    public static class Utils
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters =
            {
                new Vector2JsonConverter(),
                new Vector3JsonConverter(),
                new NullableVector3JsonConverter(),
                new QuaternionJsonConverter(),
                new NullableQuaternionJsonConverter()
            }
        };

        public static void BindStateAccess<T>(this DiContainer container) where T : IInitializable
        {
            container.BindInitializableExecutionOrder<T>(-100);
            container.BindInterfacesTo<T>().AsSingle();
        }
    }
}