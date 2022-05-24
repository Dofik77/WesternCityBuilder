using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using PdUtils;
using PdUtils.Dao;
using UnityEngine;
using Utils.SeparateThreadExecutor;

namespace Utils.ThreadLocalStorage
{
	public class ThreadedLocalStorageDao<T> : IDao<T>
	{
		private const string OLD_FILE_PREFIX = ".old";
		private const string NEW_FILE_PREFIX = ".new";

		private readonly ISeparateThreadExecutor<string> _stringThreadExecutor;
		private readonly ISeparateThreadExecutor<DataPool> _bytesThreadExecutor;
		private readonly ISeparateThreadExecutor _separateThreadExecutor;
		private readonly Queue<DataPool> _freeQueue = new Queue<DataPool>();
		private readonly Queue<DataPool> _saveQueue = new Queue<DataPool>();
		private readonly string _filePath;
		private readonly string _newFilePath;
		private readonly string _oldFilePath;

		private bool _saveInProgress;

		public ThreadedLocalStorageDao(
			ISeparateThreadExecutor<string> stringThreadExecutor,
			ISeparateThreadExecutor<DataPool> bytesThreadExecutor,
			ISeparateThreadExecutor separateThreadExecutor,
			string filename
		)
		{
			_stringThreadExecutor = stringThreadExecutor;
			_bytesThreadExecutor = bytesThreadExecutor;
			_separateThreadExecutor = separateThreadExecutor;
			_filePath = GetPath(filename);
			_newFilePath = GetPath(filename + NEW_FILE_PREFIX);
			_oldFilePath = GetPath(filename + OLD_FILE_PREFIX);
			_freeQueue.Enqueue(new DataPool());
			_freeQueue.Enqueue(new DataPool());
		}

		public void Save(T vo)
		{
			_stringThreadExecutor.Execute(() =>
			{
				var json = JsonConvert.SerializeObject(vo, Utils.JsonSerializerSettings);
				GC.Collect();
				return json;
			}, OnSerializationComplete);
		}

		private void OnSerializationComplete(string json)
		{
			var decompressDataPool = GetFreeDataPool();
			_bytesThreadExecutor.Execute(() =>
			{
				decompressDataPool.Position = Encoding.UTF8.GetBytes(json,
					0, json.Length, decompressDataPool.Buffer, 0);
				return decompressDataPool;
			}, OnJsonCompressed);
		}

		private void OnJsonCompressed(DataPool dataPool)
		{
			if (_saveInProgress)
			{
				if (_saveQueue.Count > 0)
					_freeQueue.Enqueue(_saveQueue.Dequeue());
				_saveQueue.Enqueue(dataPool);
				return;
			}

			SaveDataToTempFile(dataPool);
		}

		private void SaveDataToTempFile(DataPool dataPool)
		{
			_saveInProgress = true;
			_separateThreadExecutor.Execute(
				() =>
				{
					FileHandling.CreateDirectoryIfDoesntExistAndWriteBytes(_newFilePath, dataPool);
					FileHandling.DeleteIfExists(_oldFilePath);
					FileHandling.RenameIfExists(_filePath, _oldFilePath);
					File.Move(_newFilePath, _filePath);
				}, () =>
				{
					_freeQueue.Enqueue(dataPool);
					_saveInProgress = false;
					OnSaveToTempFileComplete();
				});
		}

		private void OnSaveToTempFileComplete()
		{
			if (_saveQueue.Count == 0)
				return;
			SaveDataToTempFile(_saveQueue.Dequeue());
		}

		public T Load()
		{
			var filePath = GetExistingSavedFilePath();
			if (filePath.IsNullOrEmpty())
				return default;
			var decompressFile = File.ReadAllBytes(filePath);
			var json = Encoding.UTF8.GetString(decompressFile);
			try { return JsonConvert.DeserializeObject<T>(json, Utils.JsonSerializerSettings); }
			catch { return default; }
		}

		public void Remove()
		{
			FileHandling.DeleteIfExists(_filePath);
			FileHandling.DeleteIfExists(_oldFilePath);
		}

		private string GetExistingSavedFilePath()
		{
			if (File.Exists(_filePath))
				return _filePath;
			return File.Exists(_oldFilePath) ? _oldFilePath : null;
		}

		private static string GetPath(string fileName)
		{
			return Path.Combine(Application.persistentDataPath, fileName);
		}

		private DataPool GetFreeDataPool() => _freeQueue.Count > 0 ? _freeQueue.Dequeue() : new DataPool();
	}
}