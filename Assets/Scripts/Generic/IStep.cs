public interface IStep {
	/// In Step, the system should interact with entity and apply
	/// modifications to it as desired.
	void Step(EgoComponent entity);

	bool CanUpdate(EgoComponent entity);
}
