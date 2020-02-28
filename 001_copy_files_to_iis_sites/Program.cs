/*

    problem:   
        
        1-newrelic kurulumu için iis'te çalışan tüm uygulamaların fiziksel yollarına newrelic.config dosyası atılmalı.
        
        2-bu xml dosyalarının hepsinin içerisindeki name alan iis'teki web site ismiyle aynı olmalı.




    çözüm:  
    
        adım 1 :    IIS'teki tüm web sitelerinin adları ve fiziksel yolları json olarak alınır. bunun için aşağıdaki PS scripti çalıştırılır:

                        Import-Module Webadministration
                        Get-ChildItem -Path IIS:\Sites | select Name,PhysicalPath | CovertTo-Json >> "c:\\list_of_iis_apps.json"


        adım 2 :    aşağıdaki program çalıştırılır(dosyayı programla aynı klasöre atmayı ya da path'i değiştirmeyi unutma)

 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace nr
{
    public static class Options
    {
        public const string LicenseKey = "the_license_key";

        public const string ApplicationListFilePath = "list_of_iis_apps.json";

        public const string Template = @"<configuration xmlns=""urn:newrelic-config"" agentEnabled=""true"" maxStackTraceLines=""50"" timingPrecision=""low"">
    <service licenseKey=""[LICENSEKEYHERE]"" sendEnvironmentInfo=""true"" syncStartup=""false"" sendDataOnExit=""false"" sendDataOnExitThreshold=""60000"" autoStart=""true"" />
    <application>
        <name>[NAMEHERE]</name>
    </application>
</configuration>";

    }

    public class Site
    {
        public string Name { get; set; }

        public string PhysicalPath { get; set; }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var listOfIISApps = JsonConvert.DeserializeObject<List<Site>>(File.ReadAllText(Options.ApplicationListFilePath));

                foreach (var iisApp in listOfIISApps)
                {
                    var filepath = Path.Combine(iisApp.PhysicalPath, "newrelic.config");
                    File.WriteAllText(filepath, Options.Template.Replace("[NAMEHERE]", iisApp.Name).Replace("[LICENSEKEYHERE]", Options.LicenseKey));

                    Console.WriteLine($"Created file at {filepath}.");
                }
                Console.WriteLine("DONE!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
