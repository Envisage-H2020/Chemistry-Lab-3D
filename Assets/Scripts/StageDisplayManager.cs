﻿using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageDisplayManager : MonoBehaviour
{


    //stage 1
    [SerializeField] private Canvas canvasStage1;
    [SerializeField] private GameObject stage1Button;

    //stage 2
    [SerializeField] private Canvas canvasStage2;
    [SerializeField] private Text moleculeNameText;
    [SerializeField] private GameObject inputField;

    private bool correctFormula = false;
    public bool formulaSubmited = false;

    //stage3
    [SerializeField] private Canvas canvasStage3;
    [SerializeField] private TextMeshProUGUI FormulaText;

    //Infromation canvas
    [SerializeField] private Canvas informationCanvas;

    //miscelaneous
    private CameraManager cam;
    public GameObject player;
    public GameGuide guide;
    [SerializeField] private MouseLook mouseLock;
    [SerializeField] private SoundManager soundManager;
    private bool menuEnabled = false;



    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Cam").GetComponent<CameraManager>();
        informationCanvas.enabled = false;
        player.GetComponent<FirstPersonController>().enabled = true;
        mouseLock.SetCursorLock(true);
        mouseLock.UpdateCursorLock();
        inputField.GetComponent<InputField>().text = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentStage == GameManager.Stage.stage0)
        {
            Stage0();
        }
        if (GameManager.instance.currentStage == GameManager.Stage.stage1)
        {
            Stage1();
        }
        if (GameManager.instance.currentStage == GameManager.Stage.stage2)
        {
            Stage2();
        }
        if (formulaSubmited == true && GameManager.instance.currentStage == GameManager.Stage.stage2)
        {
            canvasStage2.enabled = false;
            if (!menuEnabled)   cam.DisableCamera();
        }
        if (GameManager.instance.currentStage == GameManager.Stage.stage3)
        {
            canvasStage3.enabled = true;
            Stage3();
        }
        if(GameManager.instance.currentStage == GameManager.Stage.stage4)
        {
            canvasStage3.enabled = false;
            if(!menuEnabled)    cam.DisableCamera();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            soundManager.PlaySoundOnce(soundManager.audioClips[0]);
            EnableInformationCanvas();
        }
    }

    #region stage3
    public void Stage3()
    {
        inputField.GetComponent<InputField>().text = null;
        inputField.SetActive(false);
        FormulaText.text = GameManager.instance.chosenMolecule.getFormula();
    }
    #endregion

    #region stage2

    private void Stage2()
    {
        inputField.SetActive(true);
        canvasStage1.enabled = false;
        canvasStage2.enabled = true;
        canvasStage3.enabled = false;

        //display molecule name
        moleculeNameText.text = GameManager.instance.chosenMolecule.GetName().ToString();
        moleculeNameText.enabled = true;
    }

    public void CheckFormula(string formula)
    {
        if (GameManager.instance.chosenMolecule.checkFormula(formula) == true)
        {
            correctFormula = true;
        }
        else
        {
            correctFormula = false;
        }
    }

    public void SubmitFormula()
    {
        if(correctFormula == true)
        {
            formulaSubmited = true;
        }
        else
        {
            guide.DisplayWrongMsg("That was wrong, give it another try.");
        }
    }

    #endregion

    #region stage1

    private void Stage1()
    {
        canvasStage1.enabled = true;
        canvasStage2.enabled = false;
        canvasStage3.enabled = false;
    }

    public void Stage1Button()
    {
        stage1Button.SetActive(true);
    }

    #endregion

    #region stage0

    private void Stage0()
    {
        //initialize values
        canvasStage1.enabled = false;
        canvasStage2.enabled = false;
        canvasStage3.enabled = false;
        moleculeNameText.enabled = false;
        correctFormula = false;
        formulaSubmited = false;
        stage1Button.SetActive(false);
    }

    #endregion

    public void CloseCanvases()
    {
        cam.DisableCamera();
        canvasStage1.enabled = false;
        canvasStage2.enabled = false;
        GameManager.instance.ChangeToStage(0);
    }

    public void EnableInformationCanvas()
    {
        player.GetComponent<FirstPersonController>().enabled = false;
        informationCanvas.enabled = true;
        mouseLock.SetCursorLock(false);
        mouseLock.UpdateCursorLock();
        menuEnabled = true;
    }

    public void CloseInformationCanvas()
    {
        player.GetComponent<FirstPersonController>().enabled = true;
        informationCanvas.enabled = false;
        if(GameManager.instance.currentStage == GameManager.Stage.stage0)
        {
            mouseLock.SetCursorLock(true);
            mouseLock.UpdateCursorLock();
        }
        menuEnabled = false;
    }

}
