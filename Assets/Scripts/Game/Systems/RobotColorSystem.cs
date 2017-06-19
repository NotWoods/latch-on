using UnityEngine;
using LatchOn.ECS.Components.Parts;
using System.Collections.Generic;

namespace LatchOn.ECS.Systems.Rendering {
	public class RobotColorSystem : EgoSystem<
		EgoConstraint<RobotColoredParts>
	> {
		static Color DarkColor(Color baseColor) {
			float hue;
			float saturation;
			float value;
			Color.RGBToHSV(baseColor, out hue, out saturation, out value);
			return Color.HSVToRGB(hue, saturation, value - 0.15f);
		}

		static void ApplyColor(RobotColoredParts colorParts) {
			Debug.Log("Apply color");
			Material materialMain = colorParts.BaseMaterial;
			materialMain.color = colorParts.Color;

			Material materialDark = new Material(materialMain);
			materialDark.color = DarkColor(colorParts.Color);

			colorParts.Head.materials[0] = materialMain;
			colorParts.Head.materials[1] = materialDark;
			colorParts.LeftShoulder.materials[0] = materialMain;
			colorParts.LeftArm.materials[1] = materialMain;
			colorParts.RightShoulder.materials[0] = materialMain;
			colorParts.RightArm.materials[1] = materialMain;
		}

		private Dictionary<EgoComponent, string> cache = new Dictionary<EgoComponent, string>();

		public override void Update() {
			constraint.ForEachGameObject((ego, colorParts) => {
				foreach (var entry in cache) {
					Debug.Log(entry.Value);
				}

				if (cache.ContainsKey(ego)) {
					if (cache[ego] != colorParts.Color.ToString()) {
						ApplyColor(colorParts);
						cache[ego] = colorParts.Color.ToString();
					}
				} else {
					cache[ego] = colorParts.Color.ToString();
				}
			});
		}
	}
}
