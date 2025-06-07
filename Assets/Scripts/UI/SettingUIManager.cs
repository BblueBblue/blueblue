using com.kleberswf.lib.core;
using PlayerControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class SettingUIManager : Singleton<SettingUIManager>
    {
        [Header("UI Canvas")] 
        [Tooltip("설정 UI 캔버스")][SerializeField]private GameObject settingUICanvas;
        public GameObject SettingUICanvas => settingUICanvas;
        [Tooltip("설정 키")] public KeyCode settingKeyCode = KeyCode.Escape;

        [Space(3)]
        [Header("BGM")] 
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Text bgmValueText;
        [Header("SFX")] 
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Text sfxValueText;
        
        [Space(3)]
        [Header("Sensitivity")]
        [SerializeField] private Slider mouseHorizontalSlider;
        [HideInInspector] public UnityEvent<float> onHorizontalSensitivityValueChanged;
        [SerializeField] private float curHorizontalSensitivity;
        [SerializeField] private float minHorizontalSensitivity = 20f;
        [SerializeField] private float maxHorizontalSensitivity = 200f;
        [SerializeField] private Text mouseHorizontalValueText;
        [Space(3)]
        [SerializeField] private Slider mouseVerticalSlider;
        [HideInInspector]public UnityEvent<float> onVerticalSensitivityValueChanged;
        [SerializeField] private float curVerticalSensitivity;
        [SerializeField] private float minVerticalSensitivity = 20f;
        [SerializeField] private float maxVerticalSensitivity = 200f;
        [SerializeField] private Text mouseVerticalValueText;

        [Space(3)] [Header("Language")] 
        [SerializeField] private Dropdown languageDropDown;
        
        [Space(3)]
        [Header("Button")] 
        [SerializeField] private Button continueBtn;
        [SerializeField] private Button exitBtn;

        // Start is called before the first frame update
        void Start()
        {
            Invoke(nameof(Initialize), 0.1f);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(settingKeyCode))
            {
                SwitchCanvas();
            }
        }

        private void Initialize()
        {
            settingUICanvas.SetActive(false);

            #region BGM

            var bgmVolume = SoundManager.Instance.BGMMasterVolume*8/10;
            SoundManager.Instance.ChangeBGMVolume(bgmVolume);
            bgmSlider.value = bgmVolume;
            bgmValueText.text = Mathf.RoundToInt(bgmVolume*100f).ToString(); 
            bgmSlider.onValueChanged.AddListener(OnBGMValueChanged);
            bgmSlider.onValueChanged.AddListener(value => OnSoundTextChanged(value,bgmValueText));
            #endregion  
            
            #region SFX

            var sfxVolume = SoundManager.Instance.SFXMasterVolume;
            sfxSlider.value = sfxVolume;
            sfxValueText.text = Mathf.RoundToInt(sfxVolume*100f).ToString(); 
            sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
            sfxSlider.onValueChanged.AddListener(value => OnSoundTextChanged(value,sfxValueText));
            #endregion

            #region Mouse
            mouseHorizontalSlider.maxValue = maxHorizontalSensitivity;
            mouseHorizontalSlider.minValue = minHorizontalSensitivity;
            curHorizontalSensitivity = PlayerPrefs.GetFloat("HorizontalSensitivity", 110);
            mouseHorizontalSlider.value = curHorizontalSensitivity;
            mouseHorizontalValueText.text = Mathf.RoundToInt(curHorizontalSensitivity).ToString();  
            mouseHorizontalSlider.onValueChanged.AddListener(OnHorizontalSensitivityValueChanged);
            mouseHorizontalSlider.onValueChanged.AddListener(value => OnSensitivityTextChanged(value,mouseHorizontalValueText));

            mouseVerticalSlider.maxValue = maxVerticalSensitivity;
            mouseVerticalSlider.minValue = minVerticalSensitivity;
            curVerticalSensitivity = PlayerPrefs.GetFloat("VerticalSensitivity", 110);
            mouseVerticalSlider.value = curVerticalSensitivity;
            mouseVerticalValueText.text =  Mathf.RoundToInt(curVerticalSensitivity).ToString();
            mouseVerticalSlider.onValueChanged.AddListener(OnVerticalSensitivityValueChanged);
            mouseVerticalSlider.onValueChanged.AddListener(value => OnSensitivityTextChanged(value,mouseVerticalValueText));
            
            #endregion

            #region Language

            languageDropDown.value = GetCurrentLocaleIndex();

            #endregion
            
            #region Button

            continueBtn.onClick.AddListener(OnClickContinueBtn);
            exitBtn.onClick.AddListener(OnClickExitBtn);

            #endregion
            
        }

        #region Slider
        
        private void OnBGMValueChanged(float volume)
        {
            SoundManager.Instance.ChangeBGMVolume(volume);
        }
        private void OnSFXValueChanged(float volume)
        {
            SoundManager.Instance.ChangeSFXVolume(volume);
        }

        private void OnVerticalSensitivityValueChanged(float value)
        {
            curVerticalSensitivity = value;
            PlayerPrefs.SetFloat("VerticalSensitivity", curVerticalSensitivity);
            onVerticalSensitivityValueChanged.Invoke(value);
        }
        
        private void OnHorizontalSensitivityValueChanged(float value)
        {
            curHorizontalSensitivity = value;
            PlayerPrefs.SetFloat("HorizontalSensitivity", curHorizontalSensitivity);
            onHorizontalSensitivityValueChanged.Invoke(value);
        }
        
        private void OnSoundTextChanged(float value, Text valueText) 
        {
            float displayValue = value * 100f;
            valueText.text = Mathf.RoundToInt(displayValue).ToString();
        }
        
        private void OnSensitivityTextChanged(float value, Text valueText) 
        {
            valueText.text = Mathf.RoundToInt(value).ToString();
        }
        
        #endregion
        
        #region Button

        private void OnClickContinueBtn()
        {
            SwitchCanvas();
        }

        private void OnClickExitBtn()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        #endregion

        #region Language

        private int GetCurrentLocaleIndex()
        {
            // 현재 선택된 로케일 가져오기
            Locale currentLocale = LocalizationSettings.SelectedLocale;
    
            if (currentLocale == null)
            {
                Debug.LogWarning("현재 선택된 로케일이 없습니다.");
                return 0;
            }
    
            // 사용 가능한 로케일 리스트 가져오기
            var availableLocales = LocalizationSettings.AvailableLocales.Locales;
    
            // 현재 로케일의 인덱스 찾기
            for (int i = 0; i < availableLocales.Count; i++)
            {
                if (availableLocales[i] == currentLocale)
                {
                    return i;
                }
            }
    
            Debug.LogWarning("현재 로케일이 사용 가능한 로케일 리스트에서 찾을 수 없습니다.");
            return 0;
        }

        public void SetLocaleIndex(int index)
        {
            var availableLocales = LocalizationSettings.AvailableLocales.Locales;
            LocalizationSettings.Instance.SetSelectedLocale(availableLocales[index]);
        }

        #endregion

        private void SwitchCanvas()
        {
            settingUICanvas.SetActive(!settingUICanvas.activeSelf);
            Cursor.visible = settingUICanvas.activeSelf;
            if (settingUICanvas.activeSelf) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
