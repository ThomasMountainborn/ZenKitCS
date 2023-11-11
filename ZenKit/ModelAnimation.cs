using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using ZenKit.Util;

namespace ZenKit
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct AnimationSample
	{
		public Vector3 Position;
		public Quaternion Rotation;
	}

	namespace Materialized
	{
		[Serializable]
		public struct ModelAnimation
		{
			public string Name;
			public string Next;
			public uint Layer;
			public uint FrameCount;
			public uint NodeCount;
			public float Fps;
			public float FpsSource;
			public AxisAlignedBoundingBox BoundingBox;
			public uint Checksum;
			public string SourcePath;
			public DateTime SourceDate;
			public string SourceScript;
			public List<AnimationSample> Samples;
			public uint[] NodeIndices;
		}
	}

	public class ModelAnimation : IMaterializing<Materialized.ModelAnimation>
	{
		private readonly UIntPtr _handle;

		public ModelAnimation(string path)
		{
			_handle = Native.ZkModelAnimation_loadPath(path);
			if (_handle == UIntPtr.Zero) throw new Exception("Failed to load model animation");
		}

		public ModelAnimation(Read r)
		{
			_handle = Native.ZkModelAnimation_load(r.Handle);
			if (_handle == UIntPtr.Zero) throw new Exception("Failed to load model animation");
		}

		public ModelAnimation(Vfs vfs, string name)
		{
			_handle = Native.ZkModelAnimation_loadVfs(vfs.Handle, name);
			if (_handle == UIntPtr.Zero) throw new Exception("Failed to load model animation");
		}

		public string Name => Native.ZkModelAnimation_getName(_handle).MarshalAsString() ??
		                      throw new Exception("Failed to load model animation name");

		public string Next => Native.ZkModelAnimation_getNext(_handle).MarshalAsString() ??
		                      throw new Exception("Failed to load model animation next");

		public uint Layer => Native.ZkModelAnimation_getLayer(_handle);
		public uint FrameCount => Native.ZkModelAnimation_getFrameCount(_handle);
		public uint NodeCount => Native.ZkModelAnimation_getNodeCount(_handle);
		public float Fps => Native.ZkModelAnimation_getFps(_handle);
		public float FpsSource => Native.ZkModelAnimation_getFpsSource(_handle);
		public AxisAlignedBoundingBox BoundingBox => Native.ZkModelAnimation_getBbox(_handle);
		public uint Checksum => Native.ZkModelAnimation_getChecksum(_handle);

		public string SourcePath => Native.ZkModelAnimation_getSourcePath(_handle).MarshalAsString() ??
		                            throw new Exception("Failed to load model animation source path");

		public DateTime SourceDate => Native.ZkModelAnimation_getSourceDate(_handle).AsDateTime();

		public string SourceScript => Native.ZkModelAnimation_getSourceScript(_handle).MarshalAsString() ??
		                              throw new Exception("Failed to load model animation source script");

		public ulong SampleCount => Native.ZkModelAnimation_getSampleCount(_handle);

		public List<AnimationSample> Samples
		{
			get
			{
				var samples = new List<AnimationSample>();

				Native.ZkModelAnimation_enumerateSamples(_handle, (_, sample) =>
				{
					samples.Add(Marshal.PtrToStructure<AnimationSample>(sample));
					return false;
				}, UIntPtr.Zero);

				return samples;
			}
		}

		public uint[] NodeIndices =>
			Native.ZkModelAnimation_getNodeIndices(_handle, out var size).MarshalAsArray<uint>(size);

		public Materialized.ModelAnimation Materialize()
		{
			return new Materialized.ModelAnimation
			{
				Name = Name,
				Next = Next,
				Layer = Layer,
				FrameCount = FrameCount,
				NodeCount = NodeCount,
				Fps = Fps,
				FpsSource = FpsSource,
				BoundingBox = BoundingBox,
				Checksum = Checksum,
				SourcePath = SourcePath,
				SourceDate = SourceDate,
				SourceScript = SourceScript,
				Samples = Samples,
				NodeIndices = NodeIndices
			};
		}

		~ModelAnimation()
		{
			Native.ZkModelAnimation_del(_handle);
		}

		public AnimationSample GetSample(ulong i)
		{
			return Native.ZkModelAnimation_getSample(_handle, i);
		}
	}
}