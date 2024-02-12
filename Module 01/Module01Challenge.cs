#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace Module_01
{
    [Transaction(TransactionMode.Manual)]
    public class Module01Challenge : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // Global variables

            int num = 250;
            int elev = 0;
            int flrheight = 15;

            int levelStartNumber = 1;
            int sheetStartNumber = 101;

            int fizzCount = 0;
            int buzzCount = 0;
            int fizzbuzzCount = 0;

            Transaction t = new Transaction(doc);
            t.Start("Do the thing");
            
         // ~~~~ COLLECTORS ~~~

            // Get filtered element collector with all view family types
            FilteredElementCollector viewFamilyTypeCollector = new FilteredElementCollector(doc);
            viewFamilyTypeCollector.OfClass(typeof(ViewFamilyType));

            // Get filtered element collector with all title block types
            FilteredElementCollector titleBlockCollector = new FilteredElementCollector(doc);
            titleBlockCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);


         // ~~~ GET ELEMENT TYPES ~~~

            // Get the first floor familytype found in the loop
            ViewFamilyType floorPlanVFT = null;
            foreach (ViewFamilyType curVFT in viewFamilyTypeCollector)
            {
                if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    floorPlanVFT = curVFT;
                    break;
                }
            }

            // Get the first ceiling familytype found in the loop
            ViewFamilyType ceilingPlanVFT = null;
            foreach (ViewFamilyType curVFT in viewFamilyTypeCollector)
            {
                if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    ceilingPlanVFT = curVFT;
                    break;
                }
            }

            for (int i = 1; i <= num; i++)
            {
                // Create level
                Level newLevel = Level.Create(doc, elev);
                newLevel.Name = $"NEWLEVEL_{i}";

                elev += flrheight;

                // Create new sheets and add floor plans to sheet
                if (i % 3 == 0 && i % 5 == 0)
                {
                    ViewSheet newSheet = ViewSheet.Create(doc, titleBlockCollector.FirstElementId());
                    newSheet.Name = "FIZZBUZZ";
                    newSheet.SheetNumber = i.ToString();

                    ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    newFloorPlan.Name = $"FIZZBUZZ_{i}";

                    XYZ insPoint = new XYZ(1, 0.5, 0);
                    Viewport newViewPort = Viewport.Create(doc, newSheet.Id, newFloorPlan.Id, insPoint);
                    fizzbuzzCount++;
                }

                // Create ceiling plans
                else if (i % 5 == 0)
                {
                    ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, newLevel.Id);
                    newCeilingPlan.Name = $"BUZZ_{i}";
                    buzzCount++;
                }

                // Create floor plans
                else if (i % 3 == 0)
                {
                    ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    newFloorPlan.Name = $"FIZZ_{i}";
                    fizzCount++;
                }

                sheetStartNumber++;
                levelStartNumber++;

            }

            TaskDialog.Show("Counter", $"Created {fizzCount} floor plans, \r\n{buzzCount} ceilings, \r\nand {fizzbuzzCount} sheets.");

            t.Commit();
            t.Dispose();

            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
