// See https://aka.ms/new-console-template for more information


using DicePictureGenerator;
using System.Drawing;
using TestingApp;

string path = @"C:\Users\alvarezp6365\OneDrive - ARCADIS\Desktop\DDV.jpg";

Bitmap bitmap = new Bitmap(path);


var config = new DiceProcessorConfig()
{
    Bitmap = bitmap,
    OutputHeight = 119,
    OutputWidth = 100,
    MinValue = 1,
    MaxValue = 6
};




int[,] test = new int[,] { { 1, 2 }, { 3, 4 } };
var csv2 = test.ToCsv();

var results = DiceProcessor.ProcessImage(config);
var csv = results.ToCsv();



int stop = 5;