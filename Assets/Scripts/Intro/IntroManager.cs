using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IntroManager : MonoBehaviour
{
    [SerializeField] AudioSource IntroBGM; // ��Ʈ�� ��� ����
    [SerializeField] AudioSource ButtonSFX; // ��ư Ŭ�� ȿ����
    [SerializeField] TextMeshProUGUI Logo;
    [SerializeField] TextMeshProUGUI GameStartText;
    [SerializeField] Image LogoBackground;
    [SerializeField] Image GameIntroImage;
    [SerializeField] Image SignInWindow;
    [SerializeField] Image SignUpWindow;
    bool TouchAble = false;

    private void Awake()
    {
        GameIntroImage.enabled = false;
        GameStartText.gameObject.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(2f, LogoBackground, Logo));
    }

    private void Update()
    {
        if ((Application.platform == RuntimePlatform.Android))
        {
            if (Input.touchCount > 0 && (TouchAble == true))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Vector3 pos = Input.GetTouch(0).position;

                    if (pos.y <= Screen.height / 2)
                        SignInWindow.gameObject.SetActive(true);
                }
            }
        }
        else if (Input.GetKey(KeyCode.Mouse0) && (TouchAble == true))
            SignInWindow.gameObject.SetActive(true);
    }

    public IEnumerator FadeTextToFullAlpha(float time, Image image, TextMeshProUGUI text)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        while (image.color.a < 1.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (Time.deltaTime / time));
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / time));
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

        while (image.color.a > 0.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime / time));
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / time));
            yield return null;
        }

        GameIntroImage.enabled = true;
        GameStartText.gameObject.SetActive(true);
        TouchAble = true;
        IntroBGM.Play();
    }
    public void ButtonSFXPlay()
    {
        ButtonSFX.Play();
    }

    public void UIReset() // inputFieldImage color reset
    {
        Image inputFieldImage = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        inputFieldImage.color = Color.white;
    }

    public void SignUpCancel()
    {
        SignUpWindow.gameObject.SetActive(false);
    }

    public void SignUpButtonClick()
    {
        SignUpWindow.gameObject.SetActive(true);
    }

    public void WindowExit()
    {
        SignInWindow.gameObject.SetActive(false);
    }
}
