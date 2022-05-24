using Zenject;

namespace ZenjectUtil.Extensions
{
    public static class ZenjectBindDefaultValueWithFactoryExtension
    {
        public static void RebindValue<TContract>(this DiContainer container, IFactory<TContract> factory) where  TContract : class
        {
            var defaultValue = container.Resolve<TContract>();
            container.Rebind<TContract>().FromMethod(_ =>  GetTContract(factory, defaultValue)).AsSingle();
        }

        private static TContract GetTContract<TContract>(IFactory<TContract> factory, TContract defaultValue) where  TContract : class
        {
            var contract = factory.Create();
            return contract ?? defaultValue;
        }
    }
}