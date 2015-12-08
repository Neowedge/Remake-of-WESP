using UnityEngine;

namespace com.MLR.Wesp
{
    public class TouchPlayerController : PlayerController
    {        
        protected override void UpdatePlayer() {

            //if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
            if (Input.GetMouseButton(0)) {
                RaycastHit hit;

                //Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    Tile tile = hit.collider.GetComponentInParent<Tile>();
                    if (tile != null && (this.x != tile.x || this.y != tile.y))
                    {
                        GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                        this.x = tile.x;
                        this.y = tile.y;
                        this.transform.localPosition = new Vector3(tile.transform.localPosition.x, tile.transform.localPosition.y, this.transform.localPosition.z);

                        GameManager.Instance.OnPlayerMove(this);
                    }
                }
                
            }


        }
    }
}