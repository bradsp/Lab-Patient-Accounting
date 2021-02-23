using System;
using System.IO;

namespace Opulos.Core.Utils {

public static class StreamEx {

	public static void Write(this Stream s, byte[] arr) {
		s.Write(arr, 0, arr.Length);
	}
}

public static class IDisposableEx {
	public static T Dispose<T>(this IDisposable item) where T : class {
		if (item != null)
			item.Dispose();
		return (T) null;
	}
}
}