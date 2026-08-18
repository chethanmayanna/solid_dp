using System;

namespace SOLID.InterfaceSegregation
{
    /// <summary>
    /// Interface Segregation Principle (ISP):
    /// Clients should not be forced to depend on interfaces they don't use.
    /// Split fat interfaces into smaller, more specific ones.
    /// </summary>

    // VIOLATES ISP - Classes forced to implement methods they don't need
    public interface IBadMultiFunctionDevice
    {
        void Print(string document);
        void Scan(string document);
        void Fax(string document);
    }

    public class BadSimplePrinter : IBadMultiFunctionDevice
    {
        public void Print(string document)
        {
            Console.WriteLine($"Printing: {document}");
        }

        public void Scan(string document)
        {
            throw new NotImplementedException("Simple printer cannot scan");
        }

        public void Fax(string document)
        {
            throw new NotImplementedException("Simple printer cannot fax");
        }
    }

    // FOLLOWS ISP - Segregate interfaces into smaller, focused ones
    public interface IPrintable
    {
        void Print(string document);
    }

    public interface IScannable
    {
        void Scan(string document);
    }

    public interface IFaxable
    {
        void Fax(string document);
    }

    public class SimplePrinter : IPrintable
    {
        public void Print(string document)
        {
            Console.WriteLine($"Printing: {document}");
        }
    }

    public class Scanner : IScannable
    {
        public void Scan(string document)
        {
            Console.WriteLine($"Scanning: {document}");
        }
    }

    public class FaxMachine : IFaxable
    {
        public void Fax(string document)
        {
            Console.WriteLine($"Faxing: {document}");
        }
    }

    public class MultiFunctionPrinter : IPrintable, IScannable, IFaxable
    {
        public void Print(string document)
        {
            Console.WriteLine($"Printing: {document}");
        }

        public void Scan(string document)
        {
            Console.WriteLine($"Scanning: {document}");
        }

        public void Fax(string document)
        {
            Console.WriteLine($"Faxing: {document}");
        }
    }

    // Usage Example
    public class ISPExample
    {
        public static void Main()
        {
            Console.WriteLine("=== Interface Segregation Principle ===\n");

            // Each device implements only the interfaces it needs
            IPrintable printer = new SimplePrinter();
            IScannable scanner = new Scanner();
            IFaxable fax = new FaxMachine();

            // Multi-function device implements all relevant interfaces
            var multiFunctionDevice = new MultiFunctionPrinter();

            printer.Print("Document1.txt");
            scanner.Scan("Document2.txt");
            fax.Fax("Document3.txt");

            Console.WriteLine("\nMulti-function device:");
            multiFunctionDevice.Print("Document4.txt");
            multiFunctionDevice.Scan("Document5.txt");
            multiFunctionDevice.Fax("Document6.txt");

            Console.WriteLine("\nClients depend only on the interfaces they actually use.");
        }
    }
}
