using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProgressDisplayer2
{
    public class TextUI : MonoBehaviour
    {
	    public GameObject TextObject;
	    public Text text;
	    public Shadow shadowText;
	    public RectTransform rectTransform;
	    
	    public void SetSize(int size)
	    {
		    text.fontSize = size;
		    text.rectTransform.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);
	    }

	    public void SetText(string text)
	    {
		    this.text.text = text;
	    }

	    public void SetPosition(float x, float y)
	    {
		    Vector2 pos = new Vector2(x, y);
		    rectTransform.anchorMin = pos;
		    rectTransform.anchorMax = pos;
		    rectTransform.pivot = pos;
	    }
	    
	    public TextAnchor ToAlign(int align)
	    {
		    if (align == 0)
			    return TextAnchor.UpperLeft;
		    if (align == 1)
			    return TextAnchor.UpperCenter;
		    return TextAnchor.UpperRight;
	    }
	    
        private void Awake()
        {
            var mainCanvas = gameObject.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mainCanvas.sortingOrder = 10001;
            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.referenceResolution = new Vector2(1920, 1080);

            TextObject = new GameObject();
            TextObject.transform.SetParent(transform);
            TextObject.AddComponent<Canvas>();
            rectTransform = TextObject.GetComponent<RectTransform>();
            
            GameObject textObject = new GameObject();
            textObject.transform.SetParent(TextObject.transform);

            text = textObject.AddComponent<Text>();
            text.font = RDString.GetFontDataForLanguage(RDString.language).font;
            text.alignment = ToAlign(Main.setting.setAlign);
            text.fontSize = Main.setting.fontSize;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;

            shadowText = textObject.AddComponent<Shadow>();
            shadowText.effectColor =  new Color(0f, 0f, 0f, 0.45f);
            shadowText.effectDistance = new Vector2(2f, -2f);
			
            Vector2 pos = new Vector2(Main.setting.x, Main.setting.y);
            rectTransform.anchorMin = pos;
            rectTransform.anchorMax = pos;
            rectTransform.pivot = pos;
			
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}