using UnityEngine;
using UnityEngine.UI;
namespace Tactical
{
    public class GunDemo : MonoBehaviour
    {
        public Animator animator;
        public Sprite[] gunSprites;
        public SpriteRenderer weaponSpriteRenderer;
        public int gunIndex;
        public Text gunText;
        // Start is called before the first frame update
        void Start()
        {
            gunIndex = 0;
            weaponSpriteRenderer.sprite = gunSprites[gunIndex];
            gunText.text = weaponSpriteRenderer.sprite.name;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel")>0)
            {
                NextGun();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                PreviousGun();
            }
        }
        public void NextGun()
        {
            gunIndex++;
            gunIndex %= gunSprites.Length;
            weaponSpriteRenderer.sprite = gunSprites[gunIndex];
            gunText.text = weaponSpriteRenderer.sprite.name;
        }
        public void PreviousGun()
        {
            gunIndex--;
            if (gunIndex < 0) gunIndex = gunSprites.Length-1;
            weaponSpriteRenderer.sprite = gunSprites[gunIndex];
            gunText.text = weaponSpriteRenderer.sprite.name;
        }
        public void setGunAngle(float angle)
        {
            animator.SetFloat("aimDirection",(float) (angle - 0.5) * 180f);
        }
    }
}