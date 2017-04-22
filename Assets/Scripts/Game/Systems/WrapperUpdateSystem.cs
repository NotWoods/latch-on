using LatchOn.ECS.Events;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems {
	public class WrapperUpdateystem : EgoSystem<WrappingLine> {
		public override void Start() {
			EgoEvents<LineConnected>.AddHandler(Handle);
			EgoEvents<LineDisconnected>.AddHandler(Handle);
		}

		void Handle(LineConnected e) {
			WrappingLine wrapper = _bundles[e.egoComponent].component1;
			wrapper.Push(e.anchor, Side.None);
		}

		void Handle(LineDisconnected e) {
			WrappingLine wrapper = _bundles[e.egoComponent].component1;
			wrapper.Clear();
		}
	}
}
