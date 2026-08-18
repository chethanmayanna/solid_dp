using System;

namespace SOLID.DependencyInversion
{
    /// <summary>
    /// Dependency Inversion Principle (DIP):
    /// Depend on abstractions, not concrete implementations.
    /// High-level modules should not depend on low-level modules; both should depend on abstractions.
    /// </summary>

    // VIOLATES DIP - High-level OrderService depends on concrete payment implementations
    public class BadStripePayment
    {
        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing ${amount} via Stripe");
        }
    }

    public class BadPayPalPayment
    {
        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing ${amount} via PayPal");
        }
    }

    public class BadOrderService
    {
        private readonly BadStripePayment _stripePayment;
        private readonly BadPayPalPayment _paypalPayment;

        public BadOrderService()
        {
            _stripePayment = new BadStripePayment();
            _paypalPayment = new BadPayPalPayment();
        }

        public void CheckoutWithStripe(double amount)
        {
            _stripePayment.ProcessPayment(amount);
        }

        public void CheckoutWithPayPal(double amount)
        {
            _paypalPayment.ProcessPayment(amount);
        }
    }

    // FOLLOWS DIP - Depend on abstractions
    public interface IPaymentGateway
    {
        void ProcessPayment(double amount);
    }

    public class StripePayment : IPaymentGateway
    {
        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing ${amount} via Stripe");
        }
    }

    public class PayPalPayment : IPaymentGateway
    {
        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing ${amount} via PayPal");
        }
    }

    public class ApplePayment : IPaymentGateway
    {
        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing ${amount} via Apple Pay");
        }
    }

    public class OrderService
    {
        private readonly IPaymentGateway _paymentGateway;

        // Dependency is injected, not created internally
        public OrderService(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public void Checkout(double amount)
        {
            _paymentGateway.ProcessPayment(amount);
            Console.WriteLine("Order completed successfully");
        }
    }

    // Usage Example
    public class DIPExample
    {
        public static void Main()
        {
            Console.WriteLine("=== Dependency Inversion Principle ===\n");

            // OrderService depends on abstraction, not concrete implementations
            IPaymentGateway stripeGateway = new StripePayment();
            IPaymentGateway paypalGateway = new PayPalPayment();
            IPaymentGateway appleGateway = new ApplePayment();

            var orderService1 = new OrderService(stripeGateway);
            orderService1.Checkout(99.99);

            Console.WriteLine();

            var orderService2 = new OrderService(paypalGateway);
            orderService2.Checkout(49.99);

            Console.WriteLine();

            var orderService3 = new OrderService(appleGateway);
            orderService3.Checkout(199.99);

            Console.WriteLine("\nNew payment methods can be added without modifying OrderService.");
            Console.WriteLine("High-level modules depend on abstractions, not concrete implementations.");
        }
    }
}
