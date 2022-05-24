using System.IO;
using System.IO.Compression;

namespace PdUtils
{
	public class GZipUtils
	{
		public static long Compress(byte[] data, int count, byte[] compressed)
		{
			using (var sourceStream = new MemoryStream(data, 0, count, false))
			{
				using (var targetStream = new MemoryStream(compressed, true))
				{
					using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress, true))
					{
						sourceStream.CopyTo(compressionStream);
					}

					return targetStream.Position;
				}
			}
		}

		public static byte[] DecompressFile(string filePath)
		{
			using (var sourceStream = new FileStream(filePath, FileMode.Open))
			{
				using (var targetStream = new MemoryStream())
				{
					using (var decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
					{
						decompressionStream.CopyTo(targetStream);
					}

					return targetStream.ToArray();
				}
			}
		}
	}
}