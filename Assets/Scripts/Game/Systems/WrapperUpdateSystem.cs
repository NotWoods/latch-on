using LatchOn.ECS.Events;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems {
	public class WrapperUpdateystem : EgoSystem {
		public override void Start() {
			EgoEvents<LineConnected>.AddHandler(Handle);
			EgoEvents<LineDisconnected>.AddHandler(Handle);
		}

		void Handle(LineConnected e) {
			var wrapper = e.egoComponent.GetComponent<WrappingLine>();
			wrapper.Push(e.anchor, Side.None);
		}

		void Handle(LineDisconnected e) {
			var wrapper = e.egoComponent.GetComponent<WrappingLine>();
			wrapper.Clear();
		}
	}
}
