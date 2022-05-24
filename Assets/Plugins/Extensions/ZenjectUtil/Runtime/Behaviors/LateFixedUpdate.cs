using System.Collections;
using UnityEngine;

namespace Zenject {
	public class LateFixedUpdate : MonoBehaviour {
		private LateFixedManager _lateFixedManager;

		[Inject]
		private void Construct(LateFixedManager lateFixedManager) { _lateFixedManager = lateFixedManager; }

		private void Start() { StartCoroutine(WaitFixedUpdate()); }

		private IEnumerator WaitFixedUpdate() {
			while (true) {
				yield return new WaitForFixedUpdate();
				_lateFixedManager.OnLateFixed();
			}
		}
	}
}
