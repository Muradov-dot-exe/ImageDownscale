using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageResizeApp
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;

        private PictureBox OriginalPictureBox;
        private PictureBox SequentialPictureBox;
        private PictureBox ParallelPictureBox;
        private TextBox DownscaleFactorTextBox;
        private Button OpenButton;
        private Button DownscaleButton;
        private Label OriginalLabel;
        private Label SequentialLabel;
        private Label ParallelLabel;
        private PictureBox BetterQualityPictureBox;
        private Label BetterQualityLabel;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OriginalLabel = new Label();
            SequentialLabel = new Label();
            ParallelLabel = new Label();
            BetterQualityLabel = new Label();

            OriginalLabel.AutoSize = true;
            OriginalLabel.Location = new Point(12, 215);
            OriginalLabel.Name = "OriginalLabel";
            OriginalLabel.TabIndex = 6;

            SequentialLabel.AutoSize = true;
            SequentialLabel.Location = new Point(250, 215);
            SequentialLabel.Name = "SequentialLabel";
            SequentialLabel.TabIndex = 7;

            ParallelLabel.AutoSize = true;
            ParallelLabel.Location = new Point(488, 215);
            ParallelLabel.Name = "ParallelLabel";
            ParallelLabel.TabIndex = 8;

            BetterQualityLabel.AutoSize = true;
            BetterQualityLabel.Location = new Point(999, 215);
            BetterQualityLabel.Name = "BetterQualityLabel";
            BetterQualityLabel.TabIndex = 9;

            Controls.Add(OriginalLabel);
            Controls.Add(SequentialLabel);
            Controls.Add(ParallelLabel);
            Controls.Add(BetterQualityLabel);

            Controls.Add(BetterQualityPictureBox);
            Controls.Add(BetterQualityLabel);
        }

        private void UpdateImageLabels(Bitmap image, Label label)
        {
            if (image != null)
            {
                label.Text = $"Size: {image.Width} x {image.Height}";
            }
            else
            {
                label.Text = "Size: -";
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        originalImage = new Bitmap(openFileDialog.FileName);
                        OriginalPictureBox.Image = originalImage;
                        UpdateImageLabels(originalImage, OriginalLabel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void DownscaleButton_Click(object sender, EventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Please select an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(DownscaleFactorTextBox.Text, out double downscaleFactor) || downscaleFactor <= 0 || downscaleFactor > 100)
            {
                MessageBox.Show("Invalid downscaling factor. Enter a value between 1 and 100.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            long sequentialElapsedMilliseconds;
            long parallelElapsedMilliseconds;
            long betterQualityElapsedMilliseconds;

            Bitmap sequentialResult = DownscaleSequential(originalImage, downscaleFactor, out sequentialElapsedMilliseconds);
            SequentialPictureBox.Image = sequentialResult;
            UpdateImageLabels(sequentialResult, SequentialLabel);

            Tuple<Bitmap, long> parallelResultTuple = await DownscaleParallel(originalImage, downscaleFactor);
            Bitmap parallelResult = parallelResultTuple.Item1;
            parallelElapsedMilliseconds = parallelResultTuple.Item2;

            Bitmap betterQualityResult = DownscaleBetterQuality(originalImage, downscaleFactor);
            BetterQualityPictureBox.Image = betterQualityResult;
            BetterQualityPictureBox.Refresh();
            UpdateImageLabels(betterQualityResult, BetterQualityLabel);
            betterQualityElapsedMilliseconds = CalculateElapsedMilliseconds(() => DownscaleBetterQuality(originalImage, downscaleFactor));

            if (sequentialResult != null && parallelResult != null && betterQualityResult != null)
            {

                ParallelPictureBox.Image = parallelResult;
                UpdateImageLabels(parallelResult, ParallelLabel);

                MessageBox.Show($"Sequential Time: {sequentialElapsedMilliseconds} ms\nParallel Time: {parallelElapsedMilliseconds} ms\nBetter Quality Time: {betterQualityElapsedMilliseconds} ms", "Performance Metrics", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private long CalculateElapsedMilliseconds(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        private Bitmap DownscaleBetterQuality(Bitmap sourceImage, double downscaleFactor)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int newWidth = Math.Max(1, (int)(sourceImage.Width * downscaleFactor / 100));
            int newHeight = Math.Max(1, (int)(sourceImage.Height * downscaleFactor / 100));

            Bitmap resultImage = new Bitmap(newWidth, newHeight);

            BitmapData sourceData = sourceImage.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, resultImage.Width, resultImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                for (int x = 0; x < newWidth; x++)
                {
                    for (int y = 0; y < newHeight; y++)
                    {
                        Color averagedColor = AveragePixels(sourceData, x, y, downscaleFactor);
                        SetPixel(resultData, x, y, averagedColor);
                    }
                }
            }

            sourceImage.UnlockBits(sourceData);
            resultImage.UnlockBits(resultData);

            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            return resultImage;
        }

        private void SetPixel(BitmapData imageData, int x, int y, Color color)
        {
            int bytesPerPixel = 4;
            int index = y * imageData.Stride + x * bytesPerPixel;

            unsafe
            {
                byte* ptr = (byte*)imageData.Scan0 + index;
                ptr[0] = color.B;
                ptr[1] = color.G;
                ptr[2] = color.R;
                ptr[3] = color.A;
            }
        }

        private Color GetPixel(BitmapData imageData, int x, int y)
        {
            int bytesPerPixel = 4;
            int index = y * imageData.Stride + x * bytesPerPixel;

            unsafe
            {
                byte* ptr = (byte*)imageData.Scan0 + index;
                byte b = ptr[0];
                byte g = ptr[1];
                byte r = ptr[2];
                byte a = ptr[3];

                return Color.FromArgb(a, r, g, b);
            }
        }

        private Color AveragePixels(BitmapData originalData, int x, int y, double factor)
        {
            int newWidth = (int)(originalData.Width * factor / 100);
            int newHeight = (int)(originalData.Height * factor / 100);

            int startX = (int)(x * originalData.Width / newWidth);
            int startY = (int)(y * originalData.Height / newHeight);

            int endX = Math.Min((int)((x + 1) * originalData.Width / newWidth), originalData.Width - 1);
            int endY = Math.Min((int)((y + 1) * originalData.Height / newHeight), originalData.Height - 1);

            int totalRed = 0, totalGreen = 0, totalBlue = 0;

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    Color pixelColor = GetPixel(originalData, i, j);
                    totalRed += pixelColor.R;
                    totalGreen += pixelColor.G;
                    totalBlue += pixelColor.B;
                }
            }

            int pixelCount = (endX - startX + 1) * (endY - startY + 1);

            int averagedRed = totalRed / pixelCount;
            int averagedGreen = totalGreen / pixelCount;
            int averagedBlue = totalBlue / pixelCount;

            return Color.FromArgb(averagedRed, averagedGreen, averagedBlue);
        }

        private Bitmap DownscaleSequential(Bitmap sourceImage, double downscaleFactor, out long elapsedMilliseconds)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int newWidth = Math.Max(1, (int)(sourceImage.Width * downscaleFactor / 100));
            int newHeight = Math.Max(1, (int)(sourceImage.Height * downscaleFactor / 100));

            Bitmap resultImage = new Bitmap(newWidth, newHeight);

            BitmapData sourceData = sourceImage.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, resultImage.Width, resultImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    Color weightedColor = ComputeWeightedAverageLanczos(sourceData, x, y, downscaleFactor);
                    SetPixel(resultData, x, y, weightedColor);
                }
            }

            sourceImage.UnlockBits(sourceData);
            resultImage.UnlockBits(resultData);

            stopwatch.Stop();
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            return resultImage;
        }
        private async Task<Tuple<Bitmap, long>> DownscaleParallel(Bitmap sourceImage, double downscaleFactor)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int newWidth = Math.Max(1, (int)(sourceImage.Width * downscaleFactor / 100));
            int newHeight = Math.Max(1, (int)(sourceImage.Height * downscaleFactor / 100));

            if (newWidth <= 0 || newHeight <= 0)
            {
                MessageBox.Show("Invalid downscaling factor. Resulting image dimensions are non-positive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                stopwatch.Stop();
                return null;
            }

            Bitmap resultImage = new Bitmap(newWidth, newHeight);

            BitmapData sourceData = sourceImage.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, resultImage.Width, resultImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            await Task.Run(() =>
            {
                Parallel.For(0, newWidth, x =>
                {
                    for (int y = 0; y < newHeight; y++)
                    {
                        Color weightedColor = ComputeWeightedAverageLanczos(sourceData, x, y, downscaleFactor);
                        SetPixel(resultData, x, y, weightedColor);
                    }
                });
            });

            sourceImage.UnlockBits(sourceData);
            resultImage.UnlockBits(resultData);

            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            return new Tuple<Bitmap, long>(resultImage, elapsedMilliseconds);
        }
        private Color ComputeWeightedAverageLanczos(BitmapData originalData, int x, int y, double factor)
        {
            int newWidth = (int)(originalData.Width * factor / 100);
            int newHeight = (int)(originalData.Height * factor / 100);

            double u = x * (originalData.Width - 1.0) / (newWidth - 1.0);
            double v = y * (originalData.Height - 1.0) / (newHeight - 1.0);

            int startX = (int)Math.Floor(u - 2);
            int endX = (int)Math.Ceiling(u + 2);

            int startY = (int)Math.Floor(v - 2);
            int endY = (int)Math.Ceiling(v + 2);

            double totalWeight = 0;
            double totalRed = 0, totalGreen = 0, totalBlue = 0;

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    double weight = LanczosKernel(u - i) * LanczosKernel(v - j);

                    int px = Math.Min(Math.Max(0, i), originalData.Width - 1);
                    int py = Math.Min(Math.Max(0, j), originalData.Height - 1);

                    Color pixelColor = GetPixel(originalData, px, py);

                    totalRed += pixelColor.R * weight;
                    totalGreen += pixelColor.G * weight;
                    totalBlue += pixelColor.B * weight;
                    totalWeight += weight;
                }
            }

            int averagedRed = (int)(totalRed / totalWeight);
            int averagedGreen = (int)(totalGreen / totalWeight);
            int averagedBlue = (int)(totalBlue / totalWeight);

            averagedRed = (int)Math.Max(0, Math.Min(averagedRed, 255));
            averagedGreen = (int)Math.Max(0, Math.Min(averagedGreen, 255));
            averagedBlue = (int)Math.Max(0, Math.Min(averagedBlue, 255));

            return Color.FromArgb(averagedRed, averagedGreen, averagedBlue);
        }

        private double LanczosKernel(double x)
        {
            if (Math.Abs(x) < double.Epsilon)
            {
                return 1.0;
            }
            else if (Math.Abs(x) < 2.0)
            {
                return Math.Sin(Math.PI * x) * Math.Sin(Math.PI * x / 2.0) / (Math.PI * x * Math.PI * x / 2.0);
            }
            else
            {
                return 0.0;
            }
        }
    }
}
