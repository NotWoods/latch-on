namespace LatchOn.ECS.Events {
	public class LineDisconnected: EgoEvent {
		public readonly EgoComponent egoComponent;

		public LineDisconnected(EgoComponent ego) {
			egoComponent = ego;
		}
	}
}
