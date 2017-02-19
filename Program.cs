/**
 * Code Between The Lines @ http://www.codebetweenthelines.com
 * Capture an Entire Web Page in a C# Console Application
 * Feel free to use the code but please pass the credit.
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Add missing refrences
using System.Windows.Forms;
using System.Web;
using System.Drawing;
using System.Threading;

namespace CaptureWebPage
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Tuple<string, string>> tsks = new List<Tuple<string, string>>();
            tsks.Add(new Tuple<string, string>("1", "http://www.biggerschevy.com/"));
            
            //tsks.Add(new Tuple<string,string>("2","https://www.cars.com/dealers/84840/lithia-ford-lincoln-of-fresno/specialoffers/"));
            
            //tsks.Add(new Tuple<string,string>("3","https://www.cars.com/dealers/84840/lithia-ford-lincoln-of-fresno/specialoffers/"));
            
            //tsks.Add(new Tuple<string,string>("4","https://www.cars.com/dealers/84840/lithia-ford-lincoln-of-fresno/specialoffers/"));
            
            //tsks.Add(new Tuple<string,string>("5","https://www.cars.com/dealers/84840/lithia-ford-lincoln-of-fresno/specialoffers/"));
            
            //tsks.Add(new Tuple<string,string>("6","https://www.cars.com/dealers/84840/lithia-ford-lincoln-of-fresno/specialoffers/"));
            
            //tsks.Add(new Tuple<string,string>("7","https://www.cars.com/dealers/84840/lithia-ford-lincoln-of-fresno/specialoffers/"));
             
            foreach(var t in tsks)
            {
            CaptureWebPage capture = new CaptureWebPage(t.Item2, t.Item1);
            capture.Capture();
            Console.WriteLine("Done "+t.Item1);
            }
            //Console.ReadLine();
        }
    }

    public class CaptureWebPage
    {
        private WebBrowser browser;
        private String captureURL = "";
        private String id = "";
        public String CaptureURL
        {
            get { return captureURL; }
            set { captureURL = value; }
        }

        private System.Drawing.Imaging.ImageFormat captureFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
        public System.Drawing.Imaging.ImageFormat CaptureFormat
        {
            get { return captureFormat; }
            set { captureFormat = value; }
        }

        private String saveLocation = Environment.CurrentDirectory  ;
        public String SaveLocation
        {
            get { return saveLocation; }
            set { saveLocation = value; }
        }

        public CaptureWebPage(String pageURL,string _id)
        {
            CaptureURL = pageURL;
            id = _id;
        }

        delegate void updateView();
        public void Capture()
        {
            var th = new Thread(() =>
            {
                browser = new WebBrowser();
                browser.Width = 1400;
                browser.ScrollBarsEnabled = false;
                browser.ScriptErrorsSuppressed = true;
                //Register for the Document Completed Event
                browser.DocumentCompleted += webBrowserDocumentCompleted;
                browser.Navigate(CaptureURL);
                Application.Run();
            });

            //Set the threads appartment state to be singular
            th.SetApartmentState(ApartmentState.STA);
            Console.WriteLine("Browsing");
            //Start the thread
            th.Start();

            Thread.Sleep(30000);
            if (browser.InvokeRequired)
            {
                browser.Invoke(new updateView(() =>
                    {
                        Bitmap bmp = new Bitmap(1400, 1700);

                        //capture the image from browser
                        browser.DrawToBitmap(bmp, new Rectangle(browser.Location.X, browser.Location.Y, 1400, 1700));

                        //save image
                        bmp.Save(saveLocation + "\\" + id + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        Console.WriteLine("Saved");

                    }), null);
            }
        }

        void webBrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
             
            var s = browser.StatusText;
            if (browser.ReadyState == WebBrowserReadyState.Complete)
            {
                //Set the Width and Height of the browser to match the page size acording to the scroll bars
                int webPageHeight = browser.Document.Body.ScrollRectangle.Height;
                int webPageWidth = browser.Document.Body.ScrollRectangle.Width;
                Bitmap bmp = new Bitmap(webPageWidth, 1700);
                browser.Size = new Size(webPageWidth, webPageHeight);

                //Hide scroll bars so they wont show in the image capture
                browser.ScrollBarsEnabled = false;

              
            }
        }
    }
}
