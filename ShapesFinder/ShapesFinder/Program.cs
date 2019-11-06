using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

using ShapesFinder.Recognizers;
using ShapesFinder.BorderBuilders;
using ShapesFinder.Recognizers.Templates;
using ShapesFinder.Recognizers.DifferStrategies;
using ShapesFinder.Recognizers.RawDataProviders;
using ShapesFinder.Recognizers.Shapes;

namespace ShapesFinder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Input source dir path: ");
            string sdname = Console.ReadLine();

            Console.WriteLine("\nInput destination dir path: ");
            string ddname = Console.ReadLine();

            Console.WriteLine("Input processing start: ");
            int start = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Input processing step: ");
            int step = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Input processing count: ");
            int count = Convert.ToInt32(Console.ReadLine());

            string[] files = Directory.GetFiles(sdname);

            for(int i = start, cnt = 0; i < files.Length && cnt < count; i += step, cnt++)
            {
                //string file = Environment.CurrentDirectory + "/Input/АДОШ_Лб_1_v_22.jpg";

                FileInfo file = new FileInfo(files[i]);
                Console.WriteLine($"Started to process {file.Name}");

                Bitmap sourceBmp = new Bitmap(file.FullName);
                IRawData rawDataSource = new BitmapRawDataProvider(sourceBmp).Map;

                IDifferStrategy strategy = new InnerTraversalDifferStrategy();
                IEnumerable<IRawData> rawShapes = strategy.DifferShape(rawDataSource);

                Console.WriteLine($"{rawShapes.Count()} shapes found");

                IEnumerable<IShapeRecognizer> recognizers = new List<IShapeRecognizer>() { new CircleRecognizer(), new RectRecognizer(), new TriangleRecognizer() };
                List<IShape> shapes = new List<IShape>();

                foreach(IRawData rawShape in rawShapes)
                {
                    foreach (IShapeRecognizer recognizer in recognizers)
                    {
                        IShape shape = recognizer.RecognizeShape(rawShape);
                        
                        if (shape == null)
                            continue;

                        Console.WriteLine($"{shape.GetType().Name} recognized");

                        shapes.Add(shape);
                        break;
                    }
                }

                Graphics canvas = Graphics.FromImage(sourceBmp);

                IDictionary<Type, Color> colors = new Dictionary<Type, Color>()
                {
                    { typeof(Circle), Color.Red },
                    { typeof(Rect), Color.Green },
                    { typeof(Triangle), Color.Blue }
                };

                IEnumerable<IGrouping<Type, IShape>> shapesByTypes = shapes.GroupBy(S => S.GetType());
                foreach(IGrouping<Type, IShape> shapesOfType in shapesByTypes)
                {
                    Console.WriteLine($"{shapesOfType.Count()} shapes of type {shapesOfType.Key.Name}");

                    IBorderBuilder borderBuilder = new RectBorderBuilder(10, colors[shapesOfType.Key]);

                    foreach (IShape shape in shapesOfType)
                        borderBuilder.Draw(canvas, shape.X, shape.Y, shape.Width, shape.Height, shapesOfType.Key.Name + "\nV = " + shape.Scope);
                }

                Console.WriteLine($"Finised processing {file.Name}");
                Console.WriteLine("\n--------------------------------\n");

                sourceBmp.Save(ddname + "/" + file.Name);
                sourceBmp.Dispose();
            }
        }
    }
}
