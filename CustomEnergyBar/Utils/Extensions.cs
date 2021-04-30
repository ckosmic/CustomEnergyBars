using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar.Utils
{
	public static class Extensions
	{
		public static IEnumerable<int> AllIndicesOf(this string str, string searchstring) {
			int minIndex = str.IndexOf(searchstring);
			while (minIndex != -1) {
				yield return minIndex;
				minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
			}
		}

        // Credit: https://answers.unity.com/questions/799429/transformfindstring-no-longer-finds-grandchild.html
        public static Transform FindDeepChild(this Transform aParent, string aName) {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(aParent);
            while (queue.Count > 0) {
                var c = queue.Dequeue();
                if (c.name == aName)
                    return c;
                foreach (Transform t in c)
                    queue.Enqueue(t);
            }
            return null;
        }

    }
}
