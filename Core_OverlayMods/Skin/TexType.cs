using System;
using System.Collections.Generic;

namespace KoiSkinOverlayX
{
	// Names and values are important, don't change!
public enum TexType
{
	Unknown = 0,
	BodyOver = 1,
	FaceOver = 2,
	BodyUnder = 3,
	FaceUnder = 4,
	/// <summary>
	/// Same as using both EyeUnderL and EyeUnderR
	/// </summary>
	EyeUnder = 5,
	/// <summary>
	/// Same as using both EyeOverL and EyeOverR
	/// </summary>
	EyeOver = 6,
	EyeUnderL = 7,
	EyeOverL = 8,
	EyeUnderR = 9,
	EyeOverR = 10,

	/// <summary>
	/// There's no EyebrowOver because it's effectively the same as FaceOver
	/// </summary>
	EyebrowUnder = 20,

	/// <summary>
	/// There's no up/down separation because it's effectively the same texture in KK. Also, there is no up/down separation in HS2 at all.
	/// </summary>
	EyelineUnder = 30,
}

/// <summary>
/// Texture type for skin and eyes
/// </summary>
#if !KK
    [Flags]
#endif
    public enum TexType2 : ushort
    {
        Unknown = 0,

		Body = 1,
		Face = 1 << 1,
		Eye = 1 << 2,
		Eyebrow = 1 << 3,
		Eyeline = 1 << 4,

		Over = 1 << 5,
		Under = 1 << 6,
		Left = 1 << 7,
		Right = 1 << 8,

        SpecGloss = 1 << 9,

        SkinCategory = Body | Face,
        EyeCategory = Eye | Eyebrow | Eyeline,
        Categories = SkinCategory | EyeCategory,

        OverAndUnder = Over | Under,
		LeftAndRight = Left | Right,

		BodyOver = Body | Over,
		BodyUnder = Body | Under,
		FaceOver = Face | Over,
		FaceUnder = Face | Under,

		EyeUnder = Eye | Under,
		EyeOver = Eye | Over,
		EyeUnderL = Eye | Under | Left,
		EyeUnderR = Eye | Under | Right,
		EyeUnderBoth = EyeUnderL | EyeUnderR,
		EyeOverL = Eye | Over | Left,
		EyeOverR = Eye | Over | Right,
		EyeOverBoth = EyeOverL | EyeOverR,
		EyebrowUnder = Eyebrow | Under,
		EyelineUnder = Eyeline | Under
    }

	/// <summary>
	/// Extension methods for <see cref="TexType2"/>
	/// </summary>
    internal static class TexTypeExtensions
    {
		public static Dictionary<TexType, TexType2> map = GetMap();

		private static Dictionary<TexType, TexType2> GetMap()
		{
			var map = new Dictionary<TexType, TexType2>();

			foreach (TexType type in Enum.GetValues(typeof(TexType)))
			{
				if (type == TexType.EyeUnder)
				{
					map.Add(type, TexType2.EyeUnderBoth);
				}
				else if (type == TexType.EyeOver)
				{
					map.Add(type, TexType2.EyeOverBoth);
				}
				else
				{
					// Add key-value pair where the value has the same name as the specified type
					map.Add(type, (TexType2)Enum.Parse(typeof(TexType2), type.ToString()));
				}
			}
			return map;
		}

		public static TexType2 ToTexType2(this TexType texType) => map[texType];

		/// <summary>
		/// Checks if the texType has any of the specified flags; this is equivalent to a bitwise AND operation
		/// </summary>
		/// <param name="texType">The texture type being checked</param>
		/// <param name="flags">The flags to test against</param>
		/// <returns><see langword="true"/> if the texType has any of the specified flags, otherwise <see langword="false"/></returns>
		public static bool HasAnyFlag(this TexType2 texType, TexType2 flags) => (texType & flags) != 0;

		/// <summary>
		/// For a specified array of flag sets, check if the texType has any of the specified flags enabled;
		/// returns <see langword="true"/> if at least one flag from each set is enabled, otherwise returns <see langword="false"/>
		/// </summary>
		/// <param name="texType">The texture type being checked</param>
		/// <param name="flags">The flags to test against</param>
		/// <returns><see langword="true"/> if <paramref name="texType"/> has at least one enabled flag for each of the specified flag sets, otherwise <see langword="false"/></returns>
		/// <remarks>This method performs a bitwise AND operation agaisnt each set of flags, it only checks for flags that are enabled; this means that if any of the flag sets are empty, the result will be <see langword="false"/></remarks>
		public static bool HasAnyFlag(this TexType2 texType, params TexType2[] flags)
		{
			foreach (TexType2 flag in flags)
			{
				if ((texType & flag) == 0)
				{
					return false;
				}
			}
			return true;
		}

#if KK
		/// <summary>
		/// Implementation of Enum.HasFlag(Enum) method from: https://learn.microsoft.com/en-us/dotnet/api/system.enum.hasflag?view=netframework-4.6.2
		/// </summary>
		/// <param name="texType"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static bool HasFlag(this TexType2 texType, TexType2 flag) => (texType & flag) == flag;
#endif
	}
}