using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DataPubSource.RunModel;
namespace FxtSpider.DataPubSource.Run
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] runType = args;
            if (args == null || args.Length < 1)
            {
                string paraStr = Console.ReadLine();
                runType = paraStr.Split(' ');                
            }
            foreach (string str in runType)
            {
                string webName = str.Split('.')[0];
                string cityName = str.Split('.')[1];
                int count=Convert.ToInt32(str.Split('.')[2]);
                int ride = 1000;
                if (str.Split('.').Length > 3)
                {
                    ride = Convert.ToInt32(str.Split('.')[3]);
                }
                if (webName.Equals("所有"))
                {
                    if (cityName.Equals("所有"))
                    {
                        new CaseDataUploadAll(count, ride).start();
                    }
                    else
                    {
                        new CaseDataUpload(cityName, webName, count, ride).start();
                    }
                }
                else if (cityName.Equals("所有"))
                {
                    if (cityName.Equals("所有"))
                    {
                        new CaseDataUploadAll(count, ride).start();
                    }
                    else
                    {
                        new CaseDataUploadWeb(webName, count,ride).start(); 
                    }
                }
                else
                {
                    new CaseDataUpload(cityName,webName, count,ride).start(); 
                }
            }
        }
    }
}
