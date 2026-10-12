using System;

// Original Game.Runtime 02000003; bodies translated from the 1.10.1 x86-64 binary.
public static class PrimitiveExtensions
{
	// 06000007 (1.10.1 RVA 0x505F10)
	public static bool IsValidAsType(this string input, Type type)
	{
			if (type == typeof(string)) return true;
			if (type == typeof(float)) return float.TryParse(input, out _);
			if (type == typeof(int)) return int.TryParse(input, out _);
			if (type == typeof(bool)) return bool.TryParse(input, out _);
			return false;
		}

	// 06000008 (1.10.1 RVA 0x98C780)
	public static T ConvertToType<T>(this string input)
	{
			if (typeof(T) == typeof(string)) return (T)(object)input;
			if (typeof(T) == typeof(float) && float.TryParse(input, out float f)) return (T)(object)f;
			if (typeof(T) == typeof(int) && int.TryParse(input, out int i)) return (T)(object)i;
			if (typeof(T) == typeof(bool) && bool.TryParse(input, out bool b)) return (T)(object)b;
			return default(T);
		}
}
