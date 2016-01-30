using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletBar : MonoBehaviour {

	public Image[] bulletIcons;
	public Sprite enabledIcon;
	public Sprite disabledIcon;

	public void SetBulletIconStatus(int index, bool value) {
		if (bulletIcons.Length > index) {
			if (value) {
				bulletIcons[index].sprite = enabledIcon;
			} else {
				bulletIcons[index].sprite = disabledIcon;
			}
		}
	}

	public void ResetBulletIcons() {
		foreach(Image Image in bulletIcons) {
			Image.sprite = enabledIcon;
		}
	}
}
