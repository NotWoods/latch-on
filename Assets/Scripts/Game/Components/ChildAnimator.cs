using UnityEngine;
using LatchOn.ECS.Events;

namespace LatchOn.ECS.Components {
	/// Contains a reference to the Animator component in a child of the entity
	[DisallowMultipleComponent]
	public class ChildAnimator : MonoBehaviour {
		[SerializeField]
		Animator _animator = null;

		public Animator Animator {
			get {
				if (!_animator) {
					foreach (Transform child in transform) {
						var childAnimator = child.GetComponent<Animator>();
						if (childAnimator) {
							_animator = childAnimator;
							break;
						}
					}
				}

				return _animator;
			}
		}
	}
}
