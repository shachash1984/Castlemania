using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class GUIScript : MonoBehaviour {

    static public GUIScript S;
    [SerializeField]
    Text goldText;
    [SerializeField]
    Text hpText;
    [SerializeField]
    Image hpBar;
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    GameObject gameoverPanel;
    [SerializeField]
    string mainMenuScene;
    [SerializeField]
    string gameScene;
    [SerializeField] private RealCastle _castle;
    [SerializeField] private Image _damagePanel;
    [SerializeField] private Text _enemiesKilledText;
    [SerializeField] private Text _timeLastedText;


    int startHp;
    int currentHp;

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
    }

    void Start()
    {
        this.startHp = 0;
        gameoverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.TogglePause();
        }
    }

    public void SetHp(int hp)
    {
        if(this.startHp == 0)
        {
            this.startHp = hp;
        }
        if (hp > 0)
        {
            this.currentHp = hp;
            
            float currentHPF = (float)(this.currentHp);
            float startHPF = (float)(this.startHp);
            hpBar.DOFillAmount((currentHPF / startHPF), 0.5f);//Mathf.Lerp(hpBar.fillAmount, (currentHPF / startHPF), Time.deltaTime);
            this.hpText.text = this.currentHp.ToString();
            
        }
        else
            this.currentHp = 0;
    }

    public void SetTimeText()
    {
        _timeLastedText.text = _castle.TimeLasted.ToString();
    }

    public void SetEnemiesKilledText()
    {
        _enemiesKilledText.text = _castle.enemiesKilled.ToString();
    }

    public void SetGold(int goldAmount)
    {
        this.goldText.text = goldAmount.ToString();
    }   

    public void TogglePause()
    {
        this.pausePanel.SetActive(!this.pausePanel.activeSelf);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void NewGame()
    {
        SceneManager.LoadSceneAsync(gameScene);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit ();
#endif
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void GameOver()
    {
        this.goldText.gameObject.SetActive(false);
        this.hpText.gameObject.SetActive(false);
        this.hpBar.gameObject.SetActive(false);
        this.gameoverPanel.SetActive(true);
        //Time.timeScale = 0;

    }

    public IEnumerator DamageFlash()
    {
        _damagePanel.DOFade(0.25f, 0.05f);
        yield return new WaitUntil(() => !DOTween.IsTweening(_damagePanel));
        _damagePanel.DOFade(0f, 0.05f);
        yield return null;
    }
}
