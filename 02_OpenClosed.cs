using System;
using System.Collections.Generic;

namespace SOLID.OpenClosed
{
    /// <summary>
    /// Open/Closed Principle (OCP):
    /// Software entities should be open for extension but closed for modification.
    /// New functionality should be added through extension, not by modifying existing code.
    /// </summary>

    // VIOLATES OCP - Must modify this class to add new shapes
    public class BadShapeCalculator
    {
        public double CalculateArea(string shapeType, double dimension)
        {
            if (shapeType == "Circle")
            {
                return Math.PI * dimension * dimension;
            }
            else if (shapeType == "Rectangle")
            {
                return dimension * dimension;
            }
            else if (shapeType == "Triangle")
            {
                return 0.5 * dimension * dimension;
            }
            // Must modify this method to add new shapes
            return 0;
        }
    }

    // FOLLOWS OCP - Use abstraction to extend without modifying
    public interface IShape
    {
        double CalculateArea();
    }

    public class Circle : IShape
    {
        private readonly double _radius;

        public Circle(double radius)
        {
            _radius = radius;
        }

        public double CalculateArea()
        {
            return Math.PI * _radius * _radius;
        }
    }

    public class Rectangle : IShape
    {
        private readonly double _width;
        private readonly double _height;

        public Rectangle(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public double CalculateArea()
        {
            return _width * _height;
        }
    }

    public class Triangle : IShape
    {
        private readonly double _baseLength;
        private readonly double _height;

        public Triangle(double baseLength, double height)
        {
            _baseLength = baseLength;
            _height = height;
        }

        public double CalculateArea()
        {
            return 0.5 * _baseLength * _height;
        }
    }

    // New shape can be added without modifying existing code
    public class Pentagon : IShape
    {
        private readonly double _side;

        public Pentagon(double side)
        {
            _side = side;
        }

        public double CalculateArea()
        {
            // Pentagon area formula
            return (_side * _side * Math.Sqrt(25 + 10 * Math.Sqrt(5))) / 4;
        }
    }

    public class ShapeCalculator
    {
        public double CalculateTotalArea(IEnumerable<IShape> shapes)
        {
            double totalArea = 0;
            foreach (var shape in shapes)
            {
                totalArea += shape.CalculateArea();
            }
            return totalArea;
        }
    }

    // Usage Example
    public class OCPExample
    {
        public static void Main()
        {
            Console.WriteLine("=== Open/Closed Principle ===\n");

            var calculator = new ShapeCalculator();

            var shapes = new IShape[]
            {
                new Circle(5),
                new Rectangle(4, 6),
                new Triangle(3, 4),
                new Pentagon(5)
            };

            double totalArea = calculator.CalculateTotalArea(shapes);
            Console.WriteLine($"Total area of all shapes: {totalArea:F2}");
            Console.WriteLine("\nNew shapes can be added by implementing IShape without modifying existing code.");
        }
    }
}
