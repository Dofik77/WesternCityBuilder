using System;
using System.Collections.Generic;
using Zenject;

namespace ZenjectUtil.Test.Extensions
{
	public static class ResourceSubstitute
	{
		private static readonly Dictionary<Type, Type> SubstituteType = new Dictionary<Type, Type>();
		private static readonly Dictionary<Type, object> SubstituteData = new Dictionary<Type, object>();

		public static void AddSubstitute<TContract>(TContract instance)
		{
			SubstituteData.Add(typeof(TContract), instance);
		}
		
		public static void AddSubstitute<TContract, TConcrete>()
			where TConcrete : TContract
		{
			SubstituteType.Add(typeof(TContract), typeof(TConcrete));
		}
		
		public static void ReplaceSubstitute<TContract>(TContract instance)
		{
			if (SubstituteData.ContainsKey(typeof(TContract)))
				SubstituteData[typeof(TContract)] = instance;
			else
				AddSubstitute(instance);
		}
		
		public static void ReplaceSubstitute<TContract, TConcrete>()
			where TConcrete : TContract
		{
			if (SubstituteType.ContainsKey(typeof(TContract)))
				SubstituteType[typeof(TContract)] = typeof(TConcrete);
			else
				AddSubstitute<TContract, TConcrete>();
		}

		public static bool RemoveSubstitute<TContract>()
		{ 
			var removeData = SubstituteData.Remove(typeof(TContract));
			var removeType = SubstituteType.Remove(typeof(TContract));
			return removeData || removeType;
		}

		public static void Clear()
		{
			SubstituteData.Clear();
			SubstituteType.Clear();
		}

		public static void BindFromSubstitutions(this DiContainer container, params object[] instances)
		{
			foreach (var instance in instances)
			{
				var type = instance.GetType();
				container.Bind(type)
					.FromInstance(SubstituteData.ContainsKey(type)
						? SubstituteData[type]
						: instance);
			}
		}

		public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindFromSubstitute<TContract, TType>(this DiContainer container) where TType : class, TContract
		{
			GetSubstituteData(out TContract substitute);
			return substitute != null  
				? container.Bind<TContract>().FromInstance(substitute) 
				: container.Bind<TContract>().To<TType>();
		}

		public static IdScopeConcreteIdArgConditionCopyNonLazyBinder BindFromSubstitute<TContract>(
			this DiContainer container, TContract instance)
		{
			return container
				.BindInstance(GetSubstituteData(out TContract substitute)
					? substitute
					: instance);
		}

		public static ScopeConcreteIdArgConditionCopyNonLazyBinder FromSubstitute<TContract>(
			this ConcreteIdBinderGeneric<TContract> binderGeneric, TContract instance)
		{
			return binderGeneric
				.FromInstance(GetSubstituteData(out TContract substitute)
					? substitute
					: instance);
		}
		
		public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindSubstituteInterfacesTo<TContract, TConcrete>(this DiContainer container)
			where TConcrete : TContract
		{
			return container
				.BindInterfacesTo(GetSubstituteType<TContract>(out var substitute)
					? substitute
					: typeof(TConcrete));
		}

		private static bool GetSubstituteData<TContract>(out TContract substitute)
		{
			substitute = default(TContract);
			if (!SubstituteData.ContainsKey(typeof(TContract)))
				return false;
			substitute = (TContract) SubstituteData[typeof(TContract)];
			return true;
		}
		
		private static bool GetSubstituteType<TContract>(out Type substitute)
		{
			substitute = null;
			if (!SubstituteType.ContainsKey(typeof(TContract)))
				return false;
			substitute = SubstituteType[typeof(TContract)];
			return true;
		}
	}
}