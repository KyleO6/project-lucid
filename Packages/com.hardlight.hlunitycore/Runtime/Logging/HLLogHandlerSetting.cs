using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight.Utils;
using UnityEngine;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000175; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class HLLogHandlerSetting
	{
		[Tooltip("The targetted platform those settings are for. (editor, ios, android, windows, osx, tvos, switch)")]
		[SerializeField]
		private string m_platform;

		[SerializeField]
		[Tooltip("The path to register the logs. i.e. \"Logs\".")]
		private string m_exportPath = "Logs";

		[SerializeField]
		[Tooltip("Format to export the log.")]
		private ExportFormats m_logExportFormat = ExportFormats.Txt;

		[SerializeField]
		[Tooltip("Log message type to be exported.")]
		[BitMask(typeof(SupportedLogTypes))]
		private SupportedLogTypes m_logTypeSupported;

		[SerializeField]
		[Tooltip("The stack trace will be exported for those log message types.")]
		[BitMask(typeof(SupportedLogTypes))]
		private SupportedLogTypes m_stackTraceExtractionPerLogType;

		[SerializeField]
		[Tooltip("Limit the memory usage by exporting and flushing the log when reaching this size.Set to 0 to disable. 1MB of data takes ~0.1ms to export in 3 formats (txt + json + markdown).")]
		private int m_maxLogSizeInMB;

		[SerializeField]
		[Tooltip("When to export the log.")]
		private ExportStages m_logExportStage = ExportStages.OnApplicationQuit;

		public string Platform
		{
			get
			{
				return m_platform;
			}
		}

		public string ExportPath
		{
			get
			{
				return m_exportPath;
			}
		}

		public SupportedLogTypes LogTypeSupport
		{
			get
			{
				return m_logTypeSupported;
			}
		}

		public SupportedLogTypes StackTraceExtractionPerLogType
		{
			get
			{
				return m_stackTraceExtractionPerLogType;
			}
		}

		public ExportFormats LogExportFormat
		{
			get
			{
				return m_logExportFormat;
			}
		}

		public int MaxLogSizeInMB
		{
			get
			{
				return m_maxLogSizeInMB;
			}
		}

		public ExportStages LogExportStage
		{
			get
			{
				return m_logExportStage;
			}
		}

		// 06000a64 (1.10.1 RVA 0x1B00E00)
		public HLLogHandlerSetting()
		{
		}
	}
}
