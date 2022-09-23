using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Keles : MonoBehaviour
{
    Animator animatorum;

    [Header("DEG›SKENLER")]
    public bool atesEdebilirmi;
    float iceridenAtesEtmeSikligi;
    public float disaridanAtesEtmeSikligi;
    public float Menzil;

    [Header("SESLER")]
    public AudioSource silahSesi;
    public AudioSource sarjorsesi;
    public AudioSource MermiBittiSesi;

    [Header("EFEKTLER")]
    public ParticleSystem silahEfekt;
    public ParticleSystem Mermi›zi;
    public ParticleSystem kan›zi;

    [Header("D›GERLER›")]
    public Camera Kameram;

    [Header("S›LAHAYARLAR›")]
    public int ToplamMermiSayisi;
    public int SarjorKapasitesi;
    public int KalanMermiSayisi;
    public TextMeshProUGUI ToplamMermiSayisi_Text;
    public TextMeshProUGUI KalanMermiSayisi_Text;

    int AradakiFark;
    AudioSource[] sesler;

    void Start()
    {
        sesler = GetComponents<AudioSource>();
        sesler=GetComponentsInChildren<AudioSource>();
        sesler=GetComponentsInParent<AudioSource>();
        foreach (var item in sesler)
        {
            item.volume = .1f;
        }
        
        KalanMermiSayisi = SarjorKapasitesi;
        SarjorDoldurmaFonk("MermiYaz");
        animatorum = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (atesEdebilirmi && Time.time > iceridenAtesEtmeSikligi && KalanMermiSayisi != 0)
            {
                iceridenAtesEtmeSikligi = Time.time + disaridanAtesEtmeSikligi;
                AtesEt();
            }
            if (KalanMermiSayisi == 0)
            {
                MermiBittiSesi.Play();

            }


        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(SarjorDegistirme());
        }
    }
    IEnumerator SarjorDegistirme()
    {
        if (KalanMermiSayisi != SarjorKapasitesi && ToplamMermiSayisi != 0)
        {

            animatorum.Play("sarjordegistir");
        }
            yield return new WaitForSeconds(.8f);
        if (KalanMermiSayisi != SarjorKapasitesi && ToplamMermiSayisi != 0)
        {
            if (KalanMermiSayisi != 0)
            {
                SarjorDoldurmaFonk("MermiVar");
            }
            else
            {
                SarjorDoldurmaFonk("MermiYok");
            }

            
        }
    }
    void SarjorDegistir()
    {
        sarjorsesi.Play();
    }
    void AtesEt()
    {
        silahSesi.Play();
        silahEfekt.Play();
        animatorum.Play("ateset");
        KalanMermiSayisi--;
        KalanMermiSayisi_Text.text = KalanMermiSayisi.ToString();

        RaycastHit hit;
        if (Physics.Raycast(Kameram.transform.position, Kameram.transform.forward, out hit, Menzil))
        {
            if (hit.transform.gameObject.CompareTag("Dusman"))
            {
                Instantiate(kan›zi, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Instantiate(Mermi›zi, hit.point, Quaternion.LookRotation(hit.normal));
            }

        }

    }

    void SarjorDoldurmaFonk(string tur)
    {
        switch (tur)
        {
            case "MermiVar":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    int OlusanToplamDeger = KalanMermiSayisi + ToplamMermiSayisi;
                    if (OlusanToplamDeger > SarjorKapasitesi)
                    {
                        KalanMermiSayisi = SarjorKapasitesi;
                        ToplamMermiSayisi = OlusanToplamDeger - SarjorKapasitesi;
                    }
                    else
                    {
                        KalanMermiSayisi += ToplamMermiSayisi;
                        ToplamMermiSayisi = 0;
                    }


                }
                else
                {
                    ToplamMermiSayisi -= SarjorKapasitesi - KalanMermiSayisi;
                    KalanMermiSayisi = SarjorKapasitesi;
                }
                ToplamMermiSayisi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermiSayisi_Text.text = KalanMermiSayisi.ToString();
                break;
            case "MermiYok":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    KalanMermiSayisi = ToplamMermiSayisi;
                    ToplamMermiSayisi = 0;
                }
                else
                {
                    ToplamMermiSayisi -= SarjorKapasitesi;
                    KalanMermiSayisi = SarjorKapasitesi;
                }
                ToplamMermiSayisi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermiSayisi_Text.text = KalanMermiSayisi.ToString();
                break;
            case "MermiYaz":
                ToplamMermiSayisi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermiSayisi_Text.text = KalanMermiSayisi.ToString();
                break;
        }
    }
}
