using NUnit.Framework;
using ZenKit;
using ZenKit.Vobs;

namespace ZenKitTest.Tests.Vobs;

public class TestCamera
{
	[OneTimeSetUp]
	public void SetUp()
	{
		Logger.Set(LogLevel.Trace,
			(level, name, message) =>
				Console.WriteLine(new DateTime() + " [ZenKit] (" + level + ") > " + name + ": " + message));
	}

	[Test]
	public void TestLoadG2()
	{
		var vob = new Camera("./Samples/G2/VOb/zCCSCamera.zen", GameVersion.Gothic2);

		Assert.That(vob.Type, Is.EqualTo(VirtualObjectType.zCCSCamera));
		Assert.That(vob.TrajectoryFOR, Is.EqualTo(CameraTrajectory.World));
		Assert.That(vob.TargetTrajectoryFOR, Is.EqualTo(CameraTrajectory.World));
		Assert.That(vob.LoopMode, Is.EqualTo(CameraLoopType.None));
		Assert.That(vob.LerpMode, Is.EqualTo(CameraLerpType.Path));
		Assert.That(vob.IgnoreFORVobRotation, Is.False);
		Assert.That(vob.IgnoreFORVobRotationTarget, Is.False);
		Assert.That(vob.Adapt, Is.False);
		Assert.That(vob.EaseFirst, Is.False);
		Assert.That(vob.EaseLast, Is.False);
		Assert.That(vob.TotalDuration, Is.EqualTo(20.0f));
		Assert.That(vob.AutoFocusVob, Is.EqualTo(""));
		Assert.That(vob.AutoPlayerMovable, Is.False);
		Assert.That(vob.AutoUntriggerLast, Is.False);
		Assert.That(vob.AutoUntriggerLastDelay, Is.EqualTo(0.0f));
		Assert.That(vob.PositionCount, Is.EqualTo(2));
		Assert.That(vob.TargetCount, Is.EqualTo(1));

		var frames = vob.Frames;
		Assert.That(frames, Has.Count.EqualTo(3));
		Assert.That(frames[0].Time, Is.EqualTo(0.0f));
		Assert.That(frames[0].RollAngle, Is.EqualTo(0.0f));
		Assert.That(frames[0].FovScale, Is.EqualTo(1.0f));
		Assert.That(frames[0].MotionType, Is.EqualTo(CameraMotion.Slow));
		Assert.That(frames[0].MotionTypeFov, Is.EqualTo(CameraMotion.Smooth));
		Assert.That(frames[0].MotionTypeRoll, Is.EqualTo(CameraMotion.Smooth));
		Assert.That(frames[0].MotionTypeTimeScale, Is.EqualTo(CameraMotion.Smooth));
		Assert.That(frames[0].Tension, Is.EqualTo(0.0f));
		Assert.That(frames[0].CamBias, Is.EqualTo(0.0f));
		Assert.That(frames[0].Continuity, Is.EqualTo(0.0f));
		Assert.That(frames[0].TimeScale, Is.EqualTo(1.0f));
		Assert.That(frames[0].TimeFixed, Is.False);

		var pose = frames[0].OriginalPose;
		Assert.That(pose[0, 0], Is.EqualTo(0.202226311f));
		Assert.That(pose[1, 0], Is.EqualTo(3.00647909e-11f));
		Assert.That(pose[2, 0], Is.EqualTo(-0.979338825f));
		Assert.That(pose[3, 0], Is.EqualTo(0));

		Assert.That(pose[0, 1], Is.EqualTo(0.00805913191f));
		Assert.That(pose[1, 1], Is.EqualTo(0.999966145f));
		Assert.That(pose[2, 1], Is.EqualTo(0.00166415179f));
		Assert.That(pose[3, 1], Is.EqualTo(0.0f));

		Assert.That(pose[0, 2], Is.EqualTo(0.979305685f));
		Assert.That(pose[1, 2], Is.EqualTo(-0.00822915602f));
		Assert.That(pose[2, 2], Is.EqualTo(0.202219456f));
		Assert.That(pose[3, 2], Is.EqualTo(0.0f));

		Assert.That(pose[0, 3], Is.EqualTo(81815.7656f));
		Assert.That(pose[1, 3], Is.EqualTo(3905.95044f));
		Assert.That(pose[2, 3], Is.EqualTo(29227.1875f));
		Assert.That(pose[3, 3], Is.EqualTo(1.0f));
	}
}