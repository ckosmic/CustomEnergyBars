using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar
{
	[RequireComponent(typeof(EventManager))]
	public class EventFilterBehaviour : MonoBehaviour
	{
		protected EventManager EventManager {
			get {
				if (_eventManager == null) {
					_eventManager = GetComponent<EventManager>();
				}
				return _eventManager;
			}
		}

		private EventManager _eventManager;
	}
}
