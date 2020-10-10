using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class AdaptQuestion : MonoBehaviour
{
    [Serializable]
    public class Question
    {
        public string question;
        public string reponse;
        public int gorgees;
        public int type;
    }


    int gorgées;
    string theme;
    string question;
    string reponse;
    int type;
    int round;

    public Image fond;
    List<String> listeJoueurs = PlayerList.listeJoueurs;
    int NumeroJoueur;
    int NumeroJoueur2;
    bool noDoublonOK = false;

    List<String> listeThemes = new List<String>();
    bool ansDisplayed = false;

    Dictionary<string, Question[]> RepertoireThemes;
    string dataFetched;
    string jsonString;

    Dictionary<string, Question[]> RepertoireThemesOnline;
    public Question[] repertoireQuestions;
    public Question currentQuestion;
    TextWriter textWriter;
    string tempPath;

    public Text qst;
    public Text rps;
    public Text joueurTxt;
    int quiCommence;

    public Image Verre1;
    public Image Verre2;
    public Image Verre3;
    public Image Verre4;
    public Image Verre5;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        if (listeJoueurs.Count < 2)
        {
            listeJoueurs.Add("Joueur num 1");
            listeJoueurs.Add("Joueur num 2");
        }

        getOnlineData();
        Directory.CreateDirectory(UnityEngine.Application.persistentDataPath + "/data");
        File.Create(UnityEngine.Application.persistentDataPath + "/data/OnlineData.json").Dispose();
        tempPath = Path.Combine(UnityEngine.Application.persistentDataPath + "/data/OnlineData.json");   //Creating online json data file
        fond = this.GetComponent<Image>();
        ReloadQuestion();
    }

    public void getOnlineData()
    {
        PlayerPrefs.SetInt("gameMode", 1);
        Debug.Log("Game mode " + PlayerPrefs.GetInt("gameMode"));
        switch (PlayerPrefs.GetInt("gameMode"))
        {
            case 1:
                StartCoroutine(GetRequest("http://www.sport-apero-quizz.fr/api/json.php"));
                break;
            case 2:
                StartCoroutine(GetRequest("http://www.sport-apero-quizz.fr/api/json.php?key=.%24%C3%93%C3%A4%2B%C3%A7N3%C3%8CE"));
                break;
            case 3:
                StartCoroutine(GetRequest("http://www.sport-apero-quizz.fr/api/json.php?hebdo=1&key=.%24%C3%93%C3%A4%2B%C3%A7N3%C3%8CE"));
                break;
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                dataFetched = webRequest.downloadHandler.text;
                RepertoireThemesOnline = JsonConvert.DeserializeObject<Dictionary<string, Question[]>>(dataFetched);
                //Debug.Log(dataFetched);
                File.WriteAllText(tempPath, dataFetched);
                RepertoireThemesOnline = JsonConvert.DeserializeObject<Dictionary<string, Question[]>>(dataFetched);

                foreach (KeyValuePair<string, Question[]> kvp in RepertoireThemesOnline)
                {
                    listeThemes.Add(kvp.Key);
                }

                SettingData();
                updateData();
                qst.text = question;
                rps.text = "";
            }
        }
    }

    public void updateData()
    {
        AffichageVerres();
        AffichageTheme();
        QuestionReponse();
        TirageJoueur();
    }

    public void AffichageVerres()
    {
        Verre1.enabled = false;
        Verre2.enabled = false;
        Verre3.enabled = false;
        Verre4.enabled = false;
        Verre5.enabled = false;
 
        if (gorgées >= 1)
        {
            Verre1.enabled = true;
        } 
        if (gorgées >= 2)
        {
            Verre2.enabled = true;
        }
        if (gorgées >= 3)
        {
            Verre3.enabled = true;
        }
        if (gorgées >= 4)
        {
            Verre4.enabled = true;
        }
        if (gorgées >= 5)
        {
            Verre5.enabled = true;
        }
    }

    public void AffichageTheme()
    {
        switch (theme)
        {
            case "But en or : Ligue 1":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or LIGUE 1 OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "But en or : Bundesliga":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or allemagne OK V2");
                joueurTxt.color = new Color(0f, 0f, 0f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "But en or : Liga":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or LIGA  OK V2");
                joueurTxt.color = new Color(0.9686275f, 0.8745099f, 0.2980392f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "But en or : PL":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or PREMIER LEAGUE OK V2");
                joueurTxt.color = new Color(0.172549f, 0.1607843f, 0.4313726f);
                qst.color = new Color(0.172549f, 0.1607843f, 0.4313726f);
                rps.color = new Color(0.172549f, 0.1607843f, 0.4313726f);
                break;
            case "But en or : Serie A":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or SERIE A OK V2");
                joueurTxt.color = new Color(0f, 0.7450981f, 0.1058824f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "But en or : CDM":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or cdm OK V2");
                joueurTxt.color = new Color(0f, 0f, 0f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "But en or : Euro":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or EURO OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "But en or : LDC":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or LDC OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "But en or: EDF":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or EDF OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "But en or: Les blases":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or les blases OK V2");
                joueurTxt.color = new Color(0f, 0f, 0f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "But en or : Goléador":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/but en or GOLEADOR OK V2");
                joueurTxt.color = new Color(0f, 0f, 0f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "But en or":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/BUT EN OR OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "Mises au vert":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/encheres - MISES AU VERT OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "Fergie Time":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/fergie time OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "Le groupe vit bien":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/le groupe vit bien OK V2");
                joueurTxt.color = new Color(0f, 0f, 0f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "11 de légende (1v1)":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/XI DE LEGENDE OK V2");
                joueurTxt.color = new Color(0f, 0f, 0f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "Passe et va ! (tous les joueurs)":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/passe et va OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "Star Académie (1v1)":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/Star académie OK V2");
                joueurTxt.color = new Color(0.8000001f, 0.1294118f, 0.1607843f);
                qst.color = new Color(0f, 0f, 0f);
                rps.color = new Color(0f, 0f, 0f);
                break;
            case "Gros clubbeur":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/Temp");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "Tiki ou Taka ?":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/Temp");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "En direct du PMU":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/en direct du pmu OK V2");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "PhilosoFoot":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/Temp");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "Plus ou moins ?":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/Temp");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case "Hebdo":
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/Temp");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
            case null:
                fond.overrideSprite = Resources.Load<Sprite>("Fonds/Temp");
                joueurTxt.color = new Color(1f, 1f, 1f);
                qst.color = new Color(1f, 1f, 1f);
                rps.color = new Color(1f, 1f, 1f);
                break;
        }
        //Debug.Log(theme); #DEBUG
    }

    public void QuestionReponse()
    {
        qst.text = question;
        rps.text = reponse;
    }

    public void TirageJoueur()
    {
        NumeroJoueur = UnityEngine.Random.Range(0, listeJoueurs.Count);
        NumeroJoueur2 = UnityEngine.Random.Range(0, listeJoueurs.Count);
        noDoublonOK = false;
        noDoublonJoueur();
        quiCommence = UnityEngine.Random.Range(1, 3);
        joueurTxt.fontSize = 43;

        if (type == 1)
        {
            joueurTxt.text = listeJoueurs[NumeroJoueur];
        } 
        else if (type == 2)
        {
            joueurTxt.text = (listeJoueurs[NumeroJoueur] + " commence !");
        }
        else if (type == 3)
        {
            joueurTxt.fontSize = 32;

            if(quiCommence == 1)
            {
                joueurTxt.text = (listeJoueurs[NumeroJoueur] + " contre " + listeJoueurs[NumeroJoueur2] + " : " + listeJoueurs[NumeroJoueur] + " commence !");
            } else
            {
                joueurTxt.text = (listeJoueurs[NumeroJoueur] + " contre " + listeJoueurs[NumeroJoueur2] + " : " + listeJoueurs[NumeroJoueur2] + " commence !");
            }
        }
        else
        {
            joueurTxt.text = "Tout le monde joue !";
        }
    }

    public void noDoublonJoueur()
    {
        if(noDoublonOK == false)
        {
        if (NumeroJoueur == NumeroJoueur2)
        {
            NumeroJoueur2 = UnityEngine.Random.Range(0, listeJoueurs.Count);
        }
        else
        {
            noDoublonOK = true;
        }
        }

        if (NumeroJoueur == NumeroJoueur2)
        {
            noDoublonJoueur();
        }
    }

    public void SettingData()
    {
        int randNum = UnityEngine.Random.Range(1, listeThemes.Count);
        theme = listeThemes[randNum]; //#DEBUG
        RepertoireThemesOnline.TryGetValue(theme, out repertoireQuestions);
        currentQuestion = repertoireQuestions[UnityEngine.Random.Range(1, repertoireQuestions.Length)];

        question = currentQuestion.question; //"Quels sont les deux joueurs de l'effectif Lyonnais s'étants fait les ligaments croisés lors du même match en 2019 ?";
        reponse = currentQuestion.reponse;
        gorgées = currentQuestion.gorgees;           //UnityEngine.Random.Range(1, 6); //#DEBUG
        type = currentQuestion.type;
        qst.text = question;
        rps.text = "";
    }

    // TESTING - DEBUGING - TESTING - DEBUGING - TESTING - DEBUGING - TESTING - DEBUGING - TESTING - DEBUGING - TESTING - DEBUGING - TESTING - DEBUGING

    public void ReloadQuestion()
    {
        round++;
        if(round == 11)
        {
            SceneManager.LoadScene("FinDePartie");
        }
        int randNum = UnityEngine.Random.Range(1, listeThemes.Count);
        theme = listeThemes[randNum]; //#DEBUG
        RepertoireThemesOnline.TryGetValue(theme, out repertoireQuestions);
        currentQuestion = repertoireQuestions[UnityEngine.Random.Range(1, repertoireQuestions.Length)];

        question = currentQuestion.question; //"Quels sont les deux joueurs de l'effectif Lyonnais s'étants fait les ligaments croisés lors du même match en 2019 ?";
        reponse = currentQuestion.reponse;
        gorgées = currentQuestion.gorgees;           //UnityEngine.Random.Range(1, 6); //#DEBUG
        type = currentQuestion.type;
        updateData();
        qst.text = question;
        rps.text = "";
    }

    public void AffichageQuestion()
    {
        if (ansDisplayed)
        {
            qst.text = question;
            rps.text = "";
            ansDisplayed = false;
        }
    }

    public void AffichageReponse()
    {
        if (ansDisplayed)
        {
            ReloadQuestion();
            qst.text = question;
            rps.text = "";
            ansDisplayed = false;
        } else
        {
            qst.text = "";
            rps.text = reponse;
            ansDisplayed = true;
        }
    }

    public void RetourMenu()
    {
        SceneManager.LoadScene("Home");
    }
}