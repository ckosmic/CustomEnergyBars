using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CustomEnergyBar
{
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("Custom Energy Bars/Skewed TextMeshPro")]
	public class SkewedTextMeshPro : TextMeshProUGUI
	{
		public float skewX = 0;
		public float skewY = 0;

		protected override void OnEnable() {
			base.OnEnable();
		}

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
