namespace LatchOn.ECS.Events {
	public class EntityDestroyed: EgoEvent {
		public readonly EgoComponent egoComponent;

		public EntityDestroyed(EgoComponent ego) {
			egoComponent = ego;
		}
	}
}
