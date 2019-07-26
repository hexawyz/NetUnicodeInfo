using System.Buffers.Binary;
using System.IO;
using System.IO.Compression;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace System.Unicode.Build.Tasks
{
	[RunInMTA]
	public sealed class GetUnicodeDatabaseVersion : AsyncTask
	{
		public GetUnicodeDatabaseVersion()
		{
		}

		public GetUnicodeDatabaseVersion(ResourceManager taskResources) : base(taskResources)
		{
		}

		public GetUnicodeDatabaseVersion(ResourceManager taskResources, string helpKeywordPrefix) : base(taskResources, helpKeywordPrefix)
		{
		}

		[Required]
		public string DatabasePath { get; set; }

		[Output]
		public string UnicodeDatabaseVersion { get; private set; }

		protected override async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
		{
			var buffer = new byte[8];

			using (var file = new DeflateStream(File.OpenRead(DatabasePath), CompressionMode.Decompress))
			{
				await file.ReadAsync(buffer, 0, buffer.Length);
			}

			if (TryReadHeader(buffer, out var version))
			{
				UnicodeDatabaseVersion = version.ToString(3);
				return true;
			}

			Log.LogError("The database contained an invalid header.");

			return false;
		}

		private static bool TryReadHeader(ReadOnlySpan<byte> buffer, out Version version)
		{
			if (!buffer.StartsWith(new byte[] { (byte)'U', (byte)'C', (byte)'D', 2 }))
			{
				version = null;
				return false;
			}

			buffer = buffer.Slice(4);

			ushort major = BinaryPrimitives.ReadUInt16LittleEndian(buffer);

			buffer = buffer.Slice(sizeof(ushort));

			byte minor = buffer[0];
			byte build = buffer[1];

			version = new Version(major, minor, build);
			return true;
		}
	}
}
