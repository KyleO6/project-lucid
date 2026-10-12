using System;

namespace Hardlight
{
	// Original HLCrossPromo.Runtime 0200001a; bodies translated from the 1.10.1 x86-64 binary.
	public interface ICrossPromoDownloadManager
	{
		bool HasPendingDownloads
		{
			get;
		}

		void StartDownload(string uri, string filePath, bool deleteDuplicateBeforeMoving, Action<bool> metaDataDownloadCallback);
	}
}
