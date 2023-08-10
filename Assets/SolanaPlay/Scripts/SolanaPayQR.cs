using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;

public class SolanaPayQR : MonoBehaviour {

	public CodeWriter codeWtr;// drag the codewriter into this
	public string input; // content input
	public RawImage previewImg; // code image preview
	public Text errorText;// tip:error tips
	public CodeWriter.CodeType codetype;

	string androidPaths = "";
	public Texture2D targetTex;
	// Use this for initialization
	void Start () {
		CodeWriter.onCodeEncodeFinished += GetCodeImage;
		CodeWriter.onCodeEncodeError += errorInfo;
		//create_Code();
	}

	public void SaveIamgeToGallery()
	{
		if ( targetTex != null) {
			MediaController.SaveImageToGallery (targetTex);
		}
	}

	/// <summary>
	/// Creates the code.
	/// </summary>
	public void create_Code()
	{
		if (codeWtr != null) {
			codeWtr.CreateCode (codetype,input);
		}
	}

	/// <summary>
	/// Sets the type of the code by dropdown list.
	/// </summary>
	/// <param name="typeId">Type identifier.</param>
	public void setCodeType(int typeId)
	{
		codetype = (CodeWriter.CodeType)(typeId);
		Debug.Log ("clicked typeid is " + typeId);
	}

	/// <summary>
	/// Gets the code image.
	/// </summary>
	/// <param name="tex">Tex.</param>
	public void GetCodeImage(Texture2D tex)
	{
        if(targetTex != null)
        {
			DestroyImmediate(targetTex,true);
        }
        targetTex = tex;
		RectTransform component = this.previewImg.GetComponent<RectTransform>();
		float y = component.sizeDelta.x * (float)tex.height / (float)tex.width;
		component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		previewImg.texture = targetTex;
		errorText.text = "";
	}

	/// <summary>
	/// Errors the info.
	/// </summary>
	/// <param name="str">String.</param>
	public void errorInfo(string str)
	{
		errorText.text = str;
	}
	public void OpenWebsiteLink()
	{
		Application.OpenURL(input);
	}
}
