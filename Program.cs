using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

static class Program
{
    // Initiate global constants
    const int BLACK = 0;
    const int WHITE = 255;

    // Method for Loading Image
    static byte[,] LoadImage(string path)
    {
        Image<Gray8> image = Image.Load<Gray8>(path);
        byte[,] result = new byte[image.Width, image.Height];
        for (int i = 0; i < image.Width; i++)
        {
            for (int j = 0; j < image.Height; j++)
            {
                result[i, j] = image[i, j].PackedValue;
            }
        }
        return result;
    }

    //Method for Saving Image

    static void SaveImage(string path, byte[,] image)
    {
        int width = image.GetLength(0);
        int height = image.GetLength(1);
        Image<Gray8> result = new Image<Gray8>(width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                result[i, j] = new Gray8(image[i, j]);
            }
        }
        result.SaveAsPng(new FileStream(path, FileMode.Create));
    }

    // Method for Masking Image
    public static void Masking(string input, string mask, string output)
    {

        // Load and parse Images input by the user
        byte[,] originalImage = LoadImage(input);
        byte[,] maskImage = LoadImage(mask);

        //Get Dimensions
        int height = maskImage.GetLength(0);
        int width = maskImage.GetLength(1);


        // Initiate variable to store new image
        byte[,] newImage = new byte[height, width];

        // iterate over dimensions of input image
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)

            {
                if (maskImage[row, col] == WHITE)
                {
                    // get values and store to the variable new image
                    newImage[row, col] = originalImage[row, col]; // get original image values
                }
            }
        }

        // Save New Image
        SaveImage(output, newImage);

        // Get average of new image (Call Average Calculator Method)
        AverageCalculator(newImage);

    }

    // Method for finding the average of the new image
    public static void AverageCalculator(byte[,] outputImage)
    {
        // Get Dimensions of image
        int height = outputImage.GetLength(0);
        int width = outputImage.GetLength(1);

        int sum = 0; // find sum of masked area
        int area = 0; // count area of the maskedImage

        // Iterate over dimension of new Image
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                int value = outputImage[row, col];

                // Check if Image is masked
                if (value > BLACK)
                {
                    sum += value;
                    area++;
                }

            }
        }

        //Initialize variable to store average value 
        int average = sum / area;

        //Print average value
        Console.WriteLine("Average:{0}", average);
      
    }

    public static void Main(string[] args)
    {
        // check if there are enough command-line arguments

        if (args.Length >= 3)
        {

            try
            {   // if sufficient arguments were given, try to execute. 

                byte[,] originalImage = LoadImage(args[0]);
                byte[,] maskImage = LoadImage(args[1]);

                //Check if original image is equal to mask image dimensions

                if (originalImage.GetLength(0) == maskImage.GetLength(0) && originalImage.GetLength(1) == maskImage.GetLength(1))
                {
                    Masking(args[0], args[1], args[2]);
                }
                else
                {
                    Console.WriteLine("Error: Input Image and Mask Image are incompatible Dimension");
                   
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: File does not exist");
            }

        }
        else
        {
            Console.WriteLine("Error: Command-line arguments are out of range!\n",
                "Usage: Masking.exe [Input-Image] [Mask-Image] [Output-Image])");
          
        }

    }


}