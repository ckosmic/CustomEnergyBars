using TMPro;
using UnityEngine;

namespace CustomEnergyBar
{
	/// <summary>
	/// Uh, so Unity is bugged or something and this class will not import into Unity since it inherits TMP_Text.  
	/// I have no idea why that happens, but a workaround is to inherit MonoBehaviour, comment out the GenerateTextMesh()
	/// function, build, let Unity import the plugin, add the component to a TextMeshProUGUI, change the inheritance back
	/// to TextMeshProUGUI, rebuild, let Unity import the plugin again, and it'll work.
	/// I wish I knew why TMP_Text stops Unity from loading the class...
	/// </summary>

	[DisallowMultipleComponent]
	[AddComponentMenu("Custom Energy Bars/Skewed TextMeshPro")]
	public class SkewedTextMeshPro : TextMeshProUGUI
	{
		public float skewX = 0;
		public float skewY = 0;

		protected override void GenerateTextMesh() {
			base.GenerateTextMesh();

			float width = m_mesh.bounds.size.x;
			float height = m_mesh.bounds.size.y;
			float xSkew = height * Mathf.Tan(Mathf.Deg2Rad * skewX);
			float ySkew = width * Mathf.Tan(Mathf.Deg2Rad * skewY);

			float xMin = m_mesh.bounds.min.x;
			float yMin = m_mesh.bounds.min.y;

			Vector3[] array = new Vector3[m_mesh.vertexCount];
			for (int i = 0; i < m_mesh.vertexCount; i++) {
				array[i] = m_mesh.vertices[i];
				array[i] += new Vector3(Mathf.Lerp(0, xSkew, (array[i].y - yMin) / height), Mathf.Lerp(0, ySkew, (array[i].x - xMin) / width), 0);
			}
			m_mesh.vertices = array;
			base.canvasRenderer.SetMesh(m_mesh);
		}
	}
}
