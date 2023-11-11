using NUnit.Framework;
using ZenKit;
using ZenKit.Vobs;

namespace ZenKitTest.Tests.Vobs
{
	public class TestTriggerList
	{
		[Test]
		public void TestLoad()
		{
			var vob = new TriggerList("./Samples/G2/VOb/zCTriggerList.zen", GameVersion.Gothic2);
			Assert.That(vob.Mode, Is.EqualTo(TriggerBatchMode.All));
			Assert.That(vob.Targets, Has.Count.EqualTo(2));
			Assert.That(vob.Targets[0].Name, Is.EqualTo("EVT_ADDON_TROLLPORTAL_MASTERTRIGGERLIST_ALPHA_01"));
			Assert.That(vob.Targets[0].Delay.TotalSeconds, Is.EqualTo(0.0f));
			Assert.That(vob.Targets[1].Name, Is.EqualTo("EVT_ADDON_TROLLPORTAL_TRIGGERSCRIPT_01"));
			Assert.That(vob.Targets[1].Delay.TotalSeconds, Is.EqualTo(0.0f));
		}
	}
}