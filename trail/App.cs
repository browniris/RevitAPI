#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Media;
#endregion

namespace ARCtools
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            String tabname = "ARCTools";
            String panelname = "Tools";

            //create the bimMAP
           BitmapImage btn1img = new BitmapImage(new Uri("pack://application:,,,/ARCtools;component/Resources/RS50X50.png"));
           BitmapImage btn2img = new BitmapImage(new Uri("pack://application:,,,/ARCtools;component/Resources/PF50X50.png"));

            a.CreateRibbonTab(tabname);


            var tester = a.CreateRibbonPanel(tabname, panelname);
            var button1 = new PushButtonData("FirstButton", "Search Room", Assembly.GetExecutingAssembly().Location, "ARCtools.SearchRoombyNumberMain");
            button1.Image = btn1img;
          //  var btn1 = tester.AddItem(button1) as PushButton;

            var button2 = new PushButtonData("SecondtButton", "Place Family", Assembly.GetExecutingAssembly().Location, "ARCtools.PlaceFamilyMain");
            button2.Image = btn2img;
           // var btn2 = tester.AddItem(button2) as PushButton;

            tester.AddStackedItems(button1, button2);


            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
