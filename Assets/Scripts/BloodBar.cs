using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodBar : MonoBehaviour
{
    GameController gameController;
    KillScript killScript;
    Slider bloodBar;

    public Sprite endMsgNormal;
    public Sprite endMsgGarlic;

    Image bloodFlash;
    Animator bloodWave;
    float flashTimer = 0;
    float flashSpeed = 2;
    bool flashIntensifying = true;
    public Sprite flashSprite;
    public Sprite flashSpriteGarlic;
    bool nearGarlic = false;

    private float currentBlood;
    private float maxBlood = 100;
    public float bloodDecreaseSpeed = 10;
    public float bloodFromKill = 65;

    void Start()
    {
        currentBlood = maxBlood;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        killScript = GameObject.FindGameObjectWithTag("Player").GetComponent<KillScript>();
        bloodBar = GameObject.Find("BloodSlider").GetComponent<Slider>();
        bloodFlash = bloodBar.transform.Find("Front_Shining").GetComponent<Image>();
        bloodWave = bloodBar.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Animator>();
    }


    void Update()
    {
        bloodBar.value = currentBlood;

        BloodDecrease(bloodDecreaseSpeed);
        if (currentBlood > maxBlood)
        {
            currentBlood = maxBlood;
        }
        if (currentBlood <= 0)
        {
            currentBlood = 0;
            if (!gameController.gameOver)
            {
                if (nearGarlic)
                {
                    gameController.GameOver(false, endMsgGarlic);
                }
                else
                {
                    gameController.GameOver(false, endMsgNormal);
                }
            }
        }
        FlashBloodBar();
    }
    public void BloodDecrease(float amount)
    {
        if (!killScript.isSuckingBlood && currentBlood > 0)
        {
            currentBlood -= (amount * Time.deltaTime);
        }
    }
    public void GetBloodFromKill(float amount)
    {
        currentBlood += amount;
    }
    void FlashBloodBar()
    {
        if (nearGarlic || currentBlood < (maxBlood * 0.25f))
        {
            //Change Sprite
            if (nearGarlic)
            {
                if (currentBlood > (maxBlood * 0.25f) && bloodFlash.sprite != flashSpriteGarlic)
                {
                    bloodFlash.sprite = flashSpriteGarlic;
                }
                else if (currentBlood < (maxBlood * 0.25f) && bloodFlash.sprite != flashSprite)
                {
                    bloodFlash.sprite = flashSprite;
                }
            }
            else if (!nearGarlic && bloodFlash.sprite != flashSprite)
            {
                bloodFlash.sprite = flashSprite;
            }

            //Flash sprite
            if (flashIntensifying)
            {
                flashTimer += Time.deltaTime * flashSpeed;

                if (flashTimer > 1)
                {
                    flashTimer = 1;
                    flashIntensifying = false;
                }
            }
            else if (!flashIntensifying)
            {
                flashTimer -= Time.deltaTime * flashSpeed;

                if (flashTimer < 0)
                {
                    flashTimer = 0;
                    flashIntensifying = true;
                }
            }
            bloodFlash.color = new Color(1f, 1f, 1f, flashTimer);
        }
        else if (bloodFlash.color.a != 0f)
        {
            bloodFlash.color = new Color(1f, 1f, 1f, 0f);
            flashIntensifying = true;
        }
    }
    public void SetNearGarlic(bool boolean)
    {
        nearGarlic = boolean;
    }
    public void SetWaveAnim(RuntimeAnimatorController AnimationController)
    {
        bloodWave.runtimeAnimatorController = AnimationController;
    }
}
