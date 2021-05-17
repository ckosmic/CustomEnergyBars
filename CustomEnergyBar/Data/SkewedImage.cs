using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CustomEnergyBar
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Custom Energy Bars/Skewed Image")]
	public class SkewedImage : Image
	{
		public float skewX = 0;
		public float skewY = 0;

		protected override void OnPopulateMesh(VertexHelper toFill) {
			base.OnPopulateMesh(toFill);

			float width = rectTransform.rect.width;
			float height = rectTransform.rect.height;
			float xSkew = height * Mathf.Tan(Mathf.Deg2Rad * skewX);
			float ySkew = width * Mathf.Tan(Mathf.Deg2Rad * skewY);

			float xMin = rectTransform.rect.xMin;
			float yMin = rectTransform.rect.yMin;

			UIVertex v = new UIVertex();
			for (int i = 0; i < toFill.currentVertCount; i++) {
				toFill.PopulateUIVertex(ref v, i);
				v.position += new Vector3(Mathf.Lerp(0, xSkew, (v.position.y - yMin) / height), Mathf.Lerp(0, ySkew, (v.position.x - xMin) / width), 0);
				toFill.SetUIVertex(v, i);
			}
		}
	}
}
