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



public class httpreq : MonoBehaviour
{
 
    //string newURLH = "http://192.168.10.221/H";
    //string newURLL = "http://192.168.10.221/L";
    string meesURL = "https://www.htl-salzburg.ac.at/lehrerinnen-details/meerwald-stadler-susanne-prof-dipl-ing-g-009.html";
    string swefURL = "https://www.htl-salzburg.ac.at/lehrerinnen-details/schweiberer-franz-prof-dipl-ing-c-205.html";
 
 
    // See https://aka.ms/new-console-template for more information
    public string responseString = string.Empty;

    [SerializeField] private TMP_Text teacherName1;
    [SerializeField] private TMP_Text teacherRoom1;
    [SerializeField] private TMP_Text teacherName2;
    [SerializeField] private TMP_Text teacherRoom2;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doSomething();
    }

    private string ExtractTeacherName(string htmlContent)
    {
        // Der reguläre Ausdruck vom vorherigen Mal
        string pattern = @"<div\s+class=""field\s+Lehrername"">.*?<span\s+class=""text"">(.*?)<\/span>.*?<\/div>";
        
        // Verwenden Sie RegexOptions.Singleline, um Zeilenumbrüche zu ignorieren
        Regex regex = new Regex(pattern, RegexOptions.Singleline);
        
        Match match = regex.Match(htmlContent);

        if (match.Success && match.Groups.Count > 1)
        {
            // Gibt den Wert der Erfassungsgruppe 1 zurück und entfernt führende/nachfolgende Leerzeichen
            return match.Groups[1].Value.Trim();
        }
        
        return "Name nicht gefunden";
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
                    // Send a GET request to the specified URL
                    HttpResponseMessage response1 = await client.GetAsync(meesURL);
                    // Ensure the request was successful
                    response1.EnsureSuccessStatusCode();
                    // Read the response content as a string
                    responseString = await response1.Content.ReadAsStringAsync();
                    // Log the response string to the Unity console
                    Debug.Log("Response: " + responseString);
                    Console.WriteLine("Response: " + responseString);

                    teacherName1.text = ExtractTeacherName(responseString);
                }
                catch (HttpRequestException e)
                {
                    Debug.LogError("Request error: " + e.Message);
                    Console.WriteLine("Request error: " + e.Message);
                }
            }
        }).GetAwaiter().GetResult();
    }
}