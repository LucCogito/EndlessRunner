using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AudioSlider: MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private Image _image;
	[SerializeField]
	private GraphicRaycaster _graphicRaycaster;

    public void OnPointerDown(PointerEventData eventData) => StartCoroutine(TrackPointer());

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        var angle = GameManager.instance.GetVolume();
		_image.fillAmount = angle;
		_image.color = Color.Lerp(Color.green, Color.red, angle);
	}

    public void Mute()
    {
		if (_image.fillAmount == 0f)
        {
			_image.fillAmount = 1f;
			_image.color = Color.red;
			GameManager.instance.ChangeVolume(1f);
		}
        else
        {
			_image.fillAmount = 0f;
			_image.color = Color.green;
			GameManager.instance.ChangeVolume(0f);
        }
	}

    private IEnumerator TrackPointer()
	{
		while (true)
		{                    
			Vector2 localPos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle( transform as RectTransform, Input.mousePosition, _graphicRaycaster.eventCamera, out localPos );
			float angle = (Mathf.Atan2(-localPos.y, localPos.x)*180f/Mathf.PI+180f)/360f;
			_image.fillAmount = angle;
			_image.color = Color.Lerp(Color.green, Color.red, angle);
			GameManager.instance.ChangeVolume(angle);
			yield return null;
		}
	}
}
