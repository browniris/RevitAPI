#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace ARCtools
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceFamilyMain : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,ref string message,ElementSet elements)
        {   
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;


            //Survey Point Value XYZ
            FilteredElementCollector ele = new FilteredElementCollector(doc);
            IList<Element> sbp = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_SharedBasePoint).ToElements();
            double SEW = sbp[0].get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM).AsDouble();
            double SNS = sbp[0].get_Parameter(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM).AsDouble();
            double SElev = sbp[0].get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM).AsDouble();


            //Project Base Point Value XYZ
            IList<Element> pbp = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_ProjectBasePoint).ToElements();
            double PEW = pbp[0].get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM).AsDouble();
            double PNS = pbp[0].get_Parameter(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM).AsDouble();
            double PElev = pbp[0].get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM).AsDouble();

            //Origin value XYZ
            double EW = 0, NS = 0, Elev = 0;
            //Place Point



            //from form
            PlaceFamilyForm form1 = new PlaceFamilyForm(commandData);
            form1.ShowDialog();

            try
            {
                double nsvalue = form1.NS;
                double ewvalue = form1.EW;
                string Radioctrl = form1.Radioctrlvalue.ToString();
                string famsys = form1.familysymbol.ToString();
                double elevalue = form1.elev;

                if (Radioctrl == "surveypoint")
                {
                    XYZ PBpoint = new XYZ(SEW - PEW, SNS - PNS, SElev - PElev);
                    Placefamily(PBpoint);
                }

                else if (Radioctrl == "projectpoint")
                {
                    XYZ PBpoint = new XYZ(EW, NS, Elev);
                    Placefamily(PBpoint);
                }

                else if (Radioctrl == "originpoint")
                {
                    XYZ PBpoint = new XYZ(EW - PEW, NS - PNS, Elev - PElev);
                    Placefamily(PBpoint);
                }

                else
                {
                    XYZ PBpoint = new XYZ(ewvalue - PEW, nsvalue - PNS, elevalue - PElev);
                    Placefamily(PBpoint);
                }

                void Placefamily(XYZ point)
                {
                    FamilySymbol famSym1 = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Where(q => q.Name == famsys).First() as FamilySymbol;
                    FamilyInstance familyInstance = null;


                    using (Transaction t1 = new Transaction(doc, "Place Instance"))
                    {
                        t1.Start();
                        //
                        famSym1.Activate();

                        if (famSym1.Family.FamilyPlacementType.Equals(FamilyPlacementType.ViewBased))
                        {
                            familyInstance = doc.Create.NewFamilyInstance(point, famSym1, doc.ActiveView);
                        }
                        else
                        {
                            familyInstance = doc.Create.NewFamilyInstance(point, famSym1, StructuralType.NonStructural);
                        }
                        t1.Commit();
                    }
                }

                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }
        }
    }
}
