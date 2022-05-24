using System;
using System.IO;
using UnityEngine;

namespace PdUtils
{
	public static class FileHandling
	{
		public static void CreateDirectoryIfDoesntExistAndWriteAllText(string filename, string contents)
		{
			try
			{
				CreateDirectoryIfDoesntExist(filename);
				File.WriteAllText(filename, contents);
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
		}

		public static void CreateDirectoryIfDoesntExistAndWriteBytes(string filename, DataPool dataPool)
		{
			try
			{
				CreateDirectoryIfDoesntExist(filename);
				using (var stream = new FileStream(filename, FileMode.OpenOrCreate,
					FileAccess.Write, FileShare.None, dataPool.Position))
				{
					stream.Write(dataPool.Buffer, 0, dataPool.Position);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
		}

		private static void CreateDirectoryIfDoesntExist(string fileName)
		{
			var directory = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
		}

		public static void RenameIfExists(string filePath, string destinationPath)
		{
			if (!File.Exists(filePath))
				return;
			File.Move(filePath, destinationPath);
		}

		public static void DeleteIfExists(string path)
		{
			try
			{
				if (File.Exists(path))
					File.Delete(path);
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
		}
	}
}