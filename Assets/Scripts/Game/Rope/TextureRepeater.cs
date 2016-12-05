using UnityEngine;

namespace Rope {
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	public class TextureRepeater : MonoBehaviour {
		public Texture texture;
		public Vector2 textureSize = Vector2.one;

		Material ropeMaterial;

		void Start() {
			ropeMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
			ropeMaterial.mainTexture = texture;

			MeshRenderer renderer = GetComponent<MeshRenderer>();
			renderer.material = ropeMaterial;
		}

		void LateUpdate() {
			float tileCountX = transform.localScale.x / textureSize.x;
			float tileCountY = transform.localScale.y / textureSize.y;
			ropeMaterial.mainTextureScale = new Vector2(tileCountX, tileCountY);
		}
	}
}