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
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // Create a comment using a double forward slash

            // Creating variables
            // Syntax: DataType VariableName = Value; <- always end the line with semicolon
            string text1 = "this is my text";
            string text2 = "this is another wonderful text";

            string text3 = text1 + text2;
            string text4 = text1 + " " + text2 + "abcd";

            // Create number variables
            int number1 = 10;
            double number2 = 20.5;

            // Do some math
            double number3 = number1 + number2;
            double number4 = number3 - number2;
            double number5 = number4 / 100;
            double number6 = number5 + number4;
            double number7 = (number6 + number5) / number4;

            //Convert meters to feet
            double meters = 4;
            double metersToFeet = meters * 3.28084;

            //Convert mm to feet
            double mm = 3580;
            double mmToFeet = mm / 304.8;
            double mmToFeet2 = (mm / 1000) * 3.28084;

            // Find the remainder when dividing (i.e. the modulo or mod)

            double remainder1 = 100 % 10; // (100 / 10 = 10 with a reminader of 0)
            double remainder2 = 100 % 9; // (100 / 9 = 11 with a remainder of 1)

            //Increment or decrement a number by 1
            number6++;
            number6--;

            //Increase by 10
            number6 += 10;
            number6 -= 10;

            if (number7 > 10)
            {

            }

            if (number5 == 100)
            {

            }
            else
            {
                


            }

            if (number1 >= 25)
            {

            }
            else if (number2 < 3)
            {

            }
            else
            {

            }

            // Compound conditional statements use && to check if multiple conditions are true

            if (number3 == 100 && number2 > 500)
            {

            }

            // Logical or
            if (number3 == 100 || number2 > 500)
            {

            }

            // Instantiate a list
            List<string> list1 = new List<string>();

            //Add items to list
            list1.Add(text1);
            list1.Add(text2);
            list1.Add("frederick");

            // Create list and add items to it
            List<int> list2 = new List<int> { 1, 2, 3, 4, 5 };

            // For each loop
            int letterCounter = 0;
            foreach (string currentString in list1)
            {
                //letterCounter = letterCounter + currentString.Length;
                letterCounter += currentString.Length;
            }

            // For loop through a range of numbers
            int numberCount = 0;
            int counter = 100;
            for (int i = 0; i <= counter; i++)
            {
                numberCount += i;
            }

             TaskDialog.Show("Number counter", "The number count is " + numberCount.ToString());
   
   
             // Create a transaction to lock the model
             Transaction t = new Transaction(doc);
             t.Start("Doing something in Revit");
   
             Level newLevel = Level.Create(doc, 10);
             newLevel.Name = "fred";
   
             // Create a floor plan view
             // But first need to get a floor plan View Family Type
             // ...by creating a filtered element collector
             // and then by using First Element to get the first item in the list
   
             FilteredElementCollector collector1 = new FilteredElementCollector(doc);
             collector1.OfClass(typeof(ViewFamilyType));
   
             ViewFamilyType floorPlanVFT = null;
             foreach (ViewFamilyType curVFT in collector1)
             {
                 if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                 {
                     floorPlanVFT = curVFT;
                     break;
                 }
             }
   
             // Create a view by specifying the document, view family type, and level
             ViewPlan newPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
             newPlan.Name = "My new floor plan";
   
             // Get ceiling plan view family type
             ViewFamilyType ceilingPlanVFT = null;
             foreach (ViewFamilyType curVFT in collector1)
             {
                 if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                 {
                     ceilingPlanVFT = curVFT;
                     break;
                 }
             }
   
   
             ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, newLevel.Id);
             newCeilingPlan.Name = "My new ceiling plan";
   
             // Create a sheet, but first need to get title block by creating a filtered element collector
             FilteredElementCollector collector2 = new FilteredElementCollector(doc);
             collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);
   
             ViewSheet newSheet = ViewSheet.Create(doc, collector2.FirstElementId());
             newSheet.Name = "Frederick deSheet";
             newSheet.SheetNumber = "fred101";
   
            // Add a view to a sheet using a viewport - show in API
            // First create a point

             XYZ insPoint = new XYZ(1, 0.5, 0);
             Viewport newViewport = Viewport.Create(doc, newSheet.Id, newPlan.Id, insPoint);
   
             t.Commit();
             t.Dispose();
 //
 //
             return Result.Succeeded;
        }



        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
