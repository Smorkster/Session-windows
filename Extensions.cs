
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Session_windows
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public static class Extensions
	{
		[DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
		public static extern Int32 StrFormatByteSize(
			long fileSize,
			[MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer,
			int bufferSize);

		// Return a file size created by the StrFormatByteSize API function.
		public static string ToFileSizeApi(this long file_size)
		{
			StringBuilder sb = new StringBuilder(20);
			StrFormatByteSize(file_size, sb, 20);
			return sb.ToString();
		}

		// Return a string describing the value as a file size.
		// For example, 1.23 MB.
		public static string ToFileSize(this double value)
		{
			string[] suffixes = {
				"bytes",
				"KB",
				"MB",
				"GB",
				"TB",
				"PB",
				"EB",
				"ZB",
				"YB"
			};
			for (int i = 0; i < suffixes.Length; i++) {
				if (value <= (Math.Pow(1024, i + 1))) {
					return ThreeNonZeroDigits(value / Math.Pow(1024, i)) + " " + suffixes[i];
				}
			}

			return ThreeNonZeroDigits(value / Math.Pow(1024, suffixes.Length - 1)) +
			" " + suffixes[suffixes.Length - 1];
		}

		static string ThreeNonZeroDigits(double value)
		{
			if (value >= 100) {
				// No digits after the decimal.
				return value.ToString("0,0");
			} else if (value >= 10) {
				// One digit after the decimal.
				return value.ToString("0.0");
			} else {
				// Two digits after the decimal.
				return value.ToString("0.00");
			}
		}
	}
}
