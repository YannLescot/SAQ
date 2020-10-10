using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerSet : MonoBehaviour
{

    public InputField input;
    string nomJoueur;
    List<String> listeJoueurs = PlayerList.listeJoueurs;
    public Dropdown listeDeroulante;
    public int NbJoueurs = 0;
    int NbListe = 0;
    int offSetX = 0;
    public Button btnPrefab;
    public Canvas alerteSuppr;

    string joueurSelect;
    string joueurSelectCleaned;
    int numJoueurSelect;


    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        listeJoueurs.Clear();
        input.onValueChanged.AddListener(EditAnswer); //"Surveille" la valeur du champ de texte 
    }

    private void EditAnswer(string arg0)
    {
        nomJoueur = arg0;
    }

    public void SubmitJoueur()
    {
        if (nomJoueur != null && nomJoueur.Length >= 1 && listeJoueurs.IndexOf(nomJoueur) < 0)
        {
        listeJoueurs.Add(nomJoueur);
        addJoueur();
        ResetInputField();
        }
    }

    public void ResetInputField()
    {
        input.Select();
        input.text = "";
    }

    public void addJoueur()
    {
        if(NbJoueurs == 15)
        {
            NbListe = 0;
            offSetX = 500;
        }

        Vector3 spawnPoint = new Vector3(312 + offSetX, ((1443) - 90 * NbListe), 0);
        NbJoueurs++;
        NbListe++;
        Button JoueurListe = Instantiate(btnPrefab, spawnPoint, Quaternion.identity, this.transform);

        Text textNom = JoueurListe.transform.Find("Nom").gameObject.GetComponent<Text>();
        textNom.text = nomJoueur;
        textNom.name = ("Nom " + NbJoueurs);

        Text textNum = JoueurListe.transform.Find("Numéro").gameObject.GetComponent<Text>();
        textNum.text = (NbJoueurs + ".");
        textNum.name = ("Numéro " + NbJoueurs);

        JoueurListe.name = ("Joueur " + NbJoueurs);
        JoueurListe.onClick.AddListener(DeleteJoueur);
    }

    public void UpdateListeEcrite()
    {

        for(int i = 0; i <= NbJoueurs; i++)
        {
            Destroy(GameObject.Find("Joueur " + (i)));
        }

        NbJoueurs = 0;
        NbListe = 0;
        foreach (string str in listeJoueurs)
        {
            Vector3 spawnPoint = new Vector3(((-188) + 500), ((423 + 1020) - 90 * NbJoueurs), 0);
            NbJoueurs++;
            NbListe++;
            Button JoueurListe = Instantiate(btnPrefab, spawnPoint, Quaternion.identity, this.transform);

            Text textNom = JoueurListe.transform.Find("Nom").gameObject.GetComponent<Text>();
            textNom.text = str;
            textNom.name = ("Numéro " + NbJoueurs);

            Text textNum = JoueurListe.transform.Find("Numéro").gameObject.GetComponent<Text>();
            textNum.text = (NbJoueurs + ".");
            textNum.name = ("Nom " + NbJoueurs);

            JoueurListe.name = ("Joueur " + NbJoueurs);
            JoueurListe.onClick.AddListener(DeleteJoueur);
        }
    }

    public void DeleteJoueur()
    {
        alerteSuppr.gameObject.SetActive(true);

        joueurSelect = EventSystem.current.currentSelectedGameObject.name;
        joueurSelectCleaned = string.Empty;

        for (int i = 0; i < joueurSelect.Length; i++)
        {
            if (Char.IsDigit(joueurSelect[i]))
                joueurSelectCleaned += joueurSelect[i];
        }
        numJoueurSelect = int.Parse(joueurSelectCleaned);

        alerteSuppr.transform.Find("JoueurValidation").gameObject.GetComponent<Text>().text = listeJoueurs[numJoueurSelect - 1];
    }

    public void supprPressOui()
    {
        if (joueurSelectCleaned.Length > 0)
        {
            if (GameObject.Find("Joueur " + numJoueurSelect) != null)
            {
                listeJoueurs.RemoveAt(numJoueurSelect - 1);
                Destroy(GameObject.Find("Joueur " + numJoueurSelect));
                Debug.Log("cbon");
            }
            else
            {
                Debug.Log("patrouvé");
            }
            UpdateListeEcrite();
        }

        alerteSuppr.gameObject.SetActive(false);
    }

    public void supprPressNon()
    {
        alerteSuppr.gameObject.SetActive(false);
    }

    public void CoupDenvoi()
    {
        if(listeJoueurs.Count >= 2)
        {
        SceneManager.LoadScene("Game");
        }
    }

}