using System;

namespace PdUtils
{
	public static class FlagUtils
	{
		public static TEnum GetRightmostFlag<TEnum>(TEnum e)
			where TEnum : Enum
		{
			var hash = e.GetHashCode();
			return (TEnum) Enum.ToObject(typeof(TEnum), hash & -hash);
		}

		public static TEnum RemoveFlag<TEnum>(TEnum flags, TEnum removed)
			where TEnum : Enum
		{
			var flagsHashCode = flags.GetHashCode();
			flagsHashCode &= ~removed.GetHashCode();
			return (TEnum) Enum.ToObject(typeof(TEnum), flagsHashCode);
		}

		public static int GetNextFlag(int flag) => flag << 1;

		public static int GetPreviousFlag(int flag) => flag >> 1;

		public static TEnum GetFirst<TEnum>()
			where TEnum : Enum
		{
			var values = (TEnum[]) Enum.GetValues(typeof(TEnum));
			return values[0];
		}

		public static TEnum GetLast<TEnum>()
			where TEnum : Enum
		{
			var values = (TEnum[]) Enum.GetValues(typeof(TEnum));
			return values[values.Length - 1];
		}
	}
}