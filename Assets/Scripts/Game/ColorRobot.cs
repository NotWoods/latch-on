using UnityEngine;
using System.Collections.Generic;
using LatchOn.ECS.Components.Rope;

[DisallowMultipleComponent]
public class ColorRobot : MonoBehaviour {
	public Material BaseMaterial;
	public Color Color;

	public MeshRenderer Head;
	public MeshRenderer LeftShoulder;
	public MeshRenderer LeftArm;
	public MeshRenderer RightShoulder;
	public MeshRenderer RightArm;

	static Color DarkColor(Color baseColor) {
		float hue;
		float saturation;
		float value;
		Color.RGBToHSV(baseColor, out hue, out saturation, out value);
		return Color.HSVToRGB(hue, saturation, value - 0.15f);
	}

	[ContextMenu("Apply Color")]
	void ApplyColor() {
		Material materialMain = new Material(BaseMaterial);
		materialMain.color = Color;

		Material materialDark = new Material(materialMain);
		materialDark.color = DarkColor(Color);

		var headMaterials = Head.materials;
		headMaterials[0] = materialMain;
		headMaterials[1] = materialDark;
		Head.materials = headMaterials;

		var armMaterials = LeftArm.materials;
		armMaterials[1] = materialMain;
		LeftArm.materials = armMaterials;
		RightArm.materials = armMaterials;

		LeftShoulder.material = materialMain;
		RightShoulder.material = materialMain;

		var canGrapple = GetComponent<CanGrapple>();
		var hookTransform = canGrapple.Hook.transform;
		foreach (Transform child in hookTransform) {
			var renderer = child.GetComponent<MeshRenderer>();
			if (renderer) renderer.materials = armMaterials;
		}
	}
}
