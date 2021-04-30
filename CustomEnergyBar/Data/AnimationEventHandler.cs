using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar.Data
{
	public class AnimationEventHandler : MonoBehaviour
	{
		public Animation animation;

		public void Play(string clipName) {
			animation.Play(clipName);
		}

		public void StopAndPlay(string clipName) {
			animation.Stop(clipName);
			animation.Play(clipName);
		}
	}
}
