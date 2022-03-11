using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Drawing;
using DicePictureGenerator;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace DicePictureGeneratorUI
{
    internal class DicePortraitGeneratorViewModel : INotifyPropertyChanged
    {
        private string _filePath = "";
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                FileLabel = "File Selected: " + Path.GetFileName(FilePath);
                RaiseEventChanged(nameof(FilePath));
                RaiseEventChanged(nameof(FileLabel));
            }
        }
        public string FileLabel { get; set; } = "No file selected";
        private Bitmap _inputImage;

        private BitmapImage _outputImage;
        public BitmapImage OutputImage
        {
            get { return _outputImage; }
            set
            {
                _outputImage = value;
                RaiseEventChanged(nameof(OutputImage));
            }
        }

        private double? _aspectRatio;
        private int _width = 50;
        public int Width { 
            get { return _width; }
            set 
            { 
                _width = value;
                RaiseEventChanged(nameof(Width));
                if (_constrainProportions && _aspectRatio.HasValue)
                {
                    int newHeight = (int)(Math.Round( _width/ _aspectRatio.Value,0));
                    if (newHeight != Height)
                    {
                        Height = newHeight;
                        RaiseEventChanged(nameof(Height));
                    }
                   
                }
 
            }
        }
        private int _height = 50;
        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaiseEventChanged(nameof(Height));
                if (_constrainProportions && _aspectRatio.HasValue)
                {
                    int newWidth = (int)(Math.Round(_height * _aspectRatio.Value,0));
                    if(newWidth != Width)
                    {
                        Width = newWidth;
                        RaiseEventChanged(nameof(Width));
                    }
                    
                }
            }
        }

        private bool _constrainProportions = true;
        public bool ConstrainProportions
        {
            get { return _constrainProportions; }
            set
            {
                _constrainProportions = value;
                RaiseEventChanged(nameof(ConstrainProportions));
            }
        }
        private DiceTypes _diceType = DiceTypes.BlackAndWhite;
        public DiceTypes DiceType {
            get { return _diceType; }
            set { 
                _diceType = value;
                RaiseEventChanged(nameof(DiceType));
            }
        }
        private Dice[,] _diceArray;
        public string BlackDiceLabel { get; set; } = "Black Dice: 0";
        public string WhiteDiceLabel { get; set; } = "White Dice: 0";
        private List<List<Bitmap>> _bitMapImages;
        public ICommand OpenFileCommand { get; set; }
        public ICommand ProcessCommand { get; set; }
        public ICommand ConstrainCommand { get; set; }
        public ICommand ExportCsvCommand { get; set; }
        public ICommand SaveAsImage { get; set; }

        public DicePortraitGeneratorViewModel()
        {
            OpenFileCommand = new RelayCommands.RelayCommand(OnFileClicked);
            ProcessCommand = new RelayCommands.RelayCommand(OnProcessClicked);
            ConstrainCommand = new RelayCommands.RelayCommand(OnConstrainChanged);
            ExportCsvCommand = new RelayCommands.RelayCommand(OnCsvExportClicked);
            SaveAsImage = new RelayCommands.RelayCommand(OnSaveImageClicked);
        }

        private void OnFileClicked()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg,*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            bool? fileSelected = openFileDialog.ShowDialog();
            if(fileSelected.HasValue && fileSelected.Value)
            {
                FilePath = openFileDialog.FileName;
                _inputImage = new Bitmap(FilePath);
                _aspectRatio = (double)_inputImage.Width / _inputImage.Height;
                //Trigger set method of width to deal with aspect ratio of new image
                Width = Width;
            }
        }
        private void OnProcessClicked()
        {
            Mouse.SetCursor(Cursors.Wait);
            DiceProcessorConfig config = new DiceProcessorConfig();
            config.Bitmap = _inputImage;
            config.OutputHeight = Height;
            config.OutputWidth = Width;
            config.DiceTypes = DiceType;
            _diceArray = DiceProcessor.ProcessImage(config);
            UpdateDiceCount();
            BitmapImage resultBitmapImage = CreateImage();
            OutputImage = resultBitmapImage;
        }

        private BitmapImage CreateImage()
        {
            _bitMapImages = new();
            for (int i = 0; i < _height; i++)
            {
                List<Bitmap> bitmapRow = new();
                for (int u = 0; u < _width; u++)
                {
                    bitmapRow.Add(GetBitmapFromDie(_diceArray[u, i]));
                }
                _bitMapImages.Add(bitmapRow);
            }

            Bitmap resultBitmap = DiceImageCreator.CreateCollage(_bitMapImages);
            BitmapImage resultBitmapImage = BitmapToImageSource(resultBitmap);
            resultBitmap.Dispose();
            foreach (var row in _bitMapImages)
            {
                foreach (var image in row)
                {
                    image.Dispose();
                }
            }

            return resultBitmapImage;
        }
        private void UpdateDiceCount()
        {
            int blackDice = 0;
            int whiteDice = 0;

            foreach(var dice in _diceArray)
            {
                if(dice.Color == DiceColor.Black)
                {
                    blackDice++;
                    continue;
                }

                whiteDice++;
            }

            BlackDiceLabel = $"Black dice: {blackDice}";
            WhiteDiceLabel = $"White dice: {whiteDice}";

            RaiseEventChanged(nameof(BlackDiceLabel));
            RaiseEventChanged(nameof(WhiteDiceLabel));
        }

        private async void OnCsvExportClicked()
        {
            if(_diceArray is null)
            {
                MessageBox.Show("No image ready to export");
                return;
            }
            
            StringBuilder csvBuilder = new StringBuilder();
            for (int i = 0; i < _diceArray.GetLength(1); i++)
            {
                for (int u = 0; u < _diceArray.GetLength(0); u++)
                {
                    csvBuilder.Append(_diceArray[u, i].ToString() + ",");
                }
                csvBuilder.Append("\n");
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
            var dialogResult = saveFileDialog.ShowDialog();
            if(dialogResult.HasValue && dialogResult.Value)
            {
                await File.WriteAllTextAsync(saveFileDialog.FileName, csvBuilder.ToString());
                MessageBox.Show("Image exported as csv", "Success");
            }
        }
        private void OnSaveImageClicked()
        {


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image file (*.png)|*.png|All files (*.*)|*.*";
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(OutputImage));
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    encoder.Save(stream);
                MessageBox.Show("Image saved", "Success");
            }


        }
        private void OnConstrainChanged()
        {
            if (ConstrainProportions)
            {
                //Trigger set method of width to deal with aspect ratio of new image
                Width = Width;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaiseEventChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Bitmap GetBitmapFromDie(Dice dice)
        {
            switch (dice.ToString())
            {
                case "w1":
                    return Properties.Resources.w1;
                case "w2":
                    return Properties.Resources.w2;
                case "w3":
                    return Properties.Resources.w3;
                case "w4":
                    return Properties.Resources.w4;
                case "w5":
                    return Properties.Resources.w5;
                case "w6":
                    return Properties.Resources.w6;
                case "b1":
                    return Properties.Resources.b1;
                case "b2":
                    return Properties.Resources.b2;
                case "b3":
                    return Properties.Resources.b3;
                case "b4":
                    return Properties.Resources.b4;
                case "b5":
                    return Properties.Resources.b5;
                case "b6":
                    return Properties.Resources.b6;
                default:
                    throw new FileNotFoundException();
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            BitmapImage bitmapimage = new BitmapImage();

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmapimage.Freeze();

            }
            return bitmapimage;
        }
    }
}
