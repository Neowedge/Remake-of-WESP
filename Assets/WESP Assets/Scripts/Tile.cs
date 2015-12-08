using UnityEngine;
using System.Collections;

namespace com.MLR.Wesp
{
    public class Tile : MonoBehaviour {

        GameObject normalTile;
        GameObject hightlightedTile;

        bool hightlighted;

        public int x;
        public int y;

        public bool IsHighligthed {
            get {
                return this.hightlighted;
            }
        }

	    // Use this for initialization
	    void Awake () {
            this.normalTile = this.transform.FindChild("normalTile").gameObject;
            this.hightlightedTile = this.transform.FindChild("hightlightedTile").gameObject;
	    }

        public void HightlightTile(bool hightlight) {
            this.normalTile.SetActive(!hightlight);
            this.hightlightedTile.SetActive(hightlight);

            this.hightlighted = hightlight;
        }
    }
}