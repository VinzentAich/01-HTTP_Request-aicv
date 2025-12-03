using UnityEngine;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Linq;
using System.Net;




public class httprequest : MonoBehaviour
{

    //string newURLH = "http://192.168.10.221/H";
    //string newURLL = "http://192.168.10.221/L";
    string lehrerURL = "https://www.htl-salzburg.ac.at/lehrerinnen.html";
    // See https://aka.ms/new-console-template for more information
    public string responseString = string.Empty;

    [SerializeField] private TMP_Text teachers;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doSomething();
    }

    private List<string> ExtractTeachers(string htmlContent)
    {
        List<string> result = new List<string>();
        Regex teacherRegex = new Regex("<span class=\"text\">(.*?)</span>");

        MatchCollection matches = teacherRegex.Matches(htmlContent);

        foreach (Match match in matches)
        {
            string rawName = match.Groups[1].Value.Trim();

            // HTML Entities automatisch decodieren
            string decodedName = WebUtility.HtmlDecode(rawName);

            result.Add(decodedName);
        }

        return result;
    }




    // Update is called once per frame
    void Update()
    {
        Console.WriteLine("Hello, World Framework!");
    }


    void doSomething()
    {
        Task.Run(async () =>
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string html = await client.GetStringAsync(lehrerURL);

                    List<string> teachersFound = ExtractTeachers(html);

                    // Erste fünf auswählen
                    var firstFive = teachersFound.Take(5).ToList();

                    foreach (var t in firstFive)
                    {
                        Debug.Log("Lehrer: " + t);
                    }

                    // Optional: Ins UI schreiben
                    teachers.text = string.Join("\n", firstFive);
                }
                catch (HttpRequestException e)
                {
                    Debug.LogError("Request error: " + e.Message);
                }
            }
        }).GetAwaiter().GetResult();
    }
}