using System;
using System.Collections.Generic;

namespace SOLD.LiskovSubstitution
{
    /// <summary>
    /// SOLD - Liskov Substitution Principle (L)
    /// Extended implementation showing real-world scenarios
    /// Subtypes must be substitutable for their base types without breaking contracts
    /// </summary>

    // ========== VIOLATION: Breaking Contracts with Inheritance ==========
    public abstract class BadPaymentProcessor
    {
        public abstract void ProcessPayment(double amount);
        public abstract void Refund(double amount);
    }

    public class BadCreditCardProcessor : BadPaymentProcessor
    {
        public override void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing credit card payment: ${amount}");
        }

        public override void Refund(double amount)
        {
            Console.WriteLine($"Credit card refund: ${amount}");
        }
    }

    public class BadCryptoCurrencyProcessor : BadPaymentProcessor
    {
        public override void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing crypto payment: ${amount}");
        }

        public override void Refund(double amount)
        {
            // Cryptocurrencies are irreversible!
            throw new InvalidOperationException("Cryptocurrency transactions cannot be refunded!");
        }
    }

    // This violates LSP - Crypto breaks the Refund contract
    public class BadPaymentService
    {
        public void ProcessAndMaybeRefund(BadPaymentProcessor processor, double amount)
        {
            processor.ProcessPayment(amount);
            // This could crash if processor is CryptoCurrency!
            processor.Refund(amount / 2);
        }
    }

    // ========== SOLUTION: Proper Contract-Based Hierarchy ==========

    // Base interface for all payments
    public interface IPaymentProcessor
    {
        void ProcessPayment(double amount);
        bool IsRefundable { get; }
    }

    // Extended interface for refundable payments
    public interface IRefundablePayment : IPaymentProcessor
    {
        void Refund(double amount);
    }

    // ========== REFUNDABLE PAYMENT PROCESSORS ==========

    public class CreditCardProcessor : IRefundablePayment
    {
        public bool IsRefundable => true;

        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing credit card payment: ${amount}");
        }

        public void Refund(double amount)
        {
            Console.WriteLine($"Refunding to credit card: ${amount}");
        }
    }

    public class DebitCardProcessor : IRefundablePayment
    {
        public bool IsRefundable => true;

        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing debit card payment: ${amount}");
        }

        public void Refund(double amount)
        {
            Console.WriteLine($"Refunding to debit card: ${amount}");
        }
    }

    public class BankTransferProcessor : IRefundablePayment
    {
        public bool IsRefundable => true;

        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing bank transfer: ${amount}");
        }

        public void Refund(double amount)
        {
            Console.WriteLine($"Initiating bank transfer refund: ${amount}");
        }
    }

    // ========== NON-REFUNDABLE PAYMENT PROCESSORS ==========

    public class CryptoCurrencyProcessor : IPaymentProcessor
    {
        public bool IsRefundable => false;

        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing cryptocurrency payment: ${amount}");
            Console.WriteLine("⚠️  Warning: Cryptocurrency transactions are irreversible");
        }
    }

    public class GiftCardProcessor : IPaymentProcessor
    {
        public bool IsRefundable => false;

        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing gift card payment: ${amount}");
        }
    }

    public class CashProcessor : IPaymentProcessor
    {
        public bool IsRefundable => false;

        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing cash payment: ${amount}");
        }
    }

    // ========== PAYMENT SERVICE RESPECTING LSP ==========

    public class PaymentService
    {
        // Works with any payment processor
        public void ProcessPayment(IPaymentProcessor processor, double amount)
        {
            processor.ProcessPayment(amount);
        }

        // Only works with refundable payments
        public void ProcessPaymentWithRefund(IRefundablePayment processor, double amount)
        {
            processor.ProcessPayment(amount);
            processor.Refund(amount * 0.5);
        }

        // Safely handle both refundable and non-refundable
        public void ProcessPaymentSafely(IPaymentProcessor processor, double amount)
        {
            processor.ProcessPayment(amount);

            if (processor.IsRefundable)
            {
                var refundable = processor as IRefundablePayment;
                if (refundable != null)
                {
                    refundable.Refund(amount * 0.25);
                }
            }
            else
            {
                Console.WriteLine("Note: This payment method doesn't support refunds");
            }
        }
    }

    // ========== ANIMAL HIERARCHY EXAMPLE (LSP) ==========

    public abstract class Animal
    {
        public abstract void Move();
        public abstract void Eat();
    }

    public interface ISwimmer
    {
        void Swim();
    }

    public interface IFlyer
    {
        void Fly();
    }

    public class Dog : Animal
    {
        public override void Move()
        {
            Console.WriteLine("Dog is running");
        }

        public override void Eat()
        {
            Console.WriteLine("Dog is eating");
        }
    }

    public class Eagle : Animal, IFlyer
    {
        public override void Move()
        {
            Console.WriteLine("Eagle is moving");
        }

        public override void Eat()
        {
            Console.WriteLine("Eagle is eating");
        }

        public void Fly()
        {
            Console.WriteLine("Eagle is flying");
        }
    }

    public class Duck : Animal, IFlyer, ISwimmer
    {
        public override void Move()
        {
            Console.WriteLine("Duck is moving");
        }

        public override void Eat()
        {
            Console.WriteLine("Duck is eating");
        }

        public void Fly()
        {
            Console.WriteLine("Duck is flying");
        }

        public void Swim()
        {
            Console.WriteLine("Duck is swimming");
        }
    }

    public class Penguin : Animal, ISwimmer
    {
        public override void Move()
        {
            Console.WriteLine("Penguin is waddling");
        }

        public override void Eat()
        {
            Console.WriteLine("Penguin is eating fish");
        }

        public void Swim()
        {
            Console.WriteLine("Penguin is swimming");
        }
    }

    // ========== ANIMAL HANDLER ==========

    public class AnimalHandler
    {
        public void MakeAnimalMove(Animal animal)
        {
            animal.Move();
            animal.Eat();
        }

        public void MakeAnimalFly(IFlyer flyer)
        {
            flyer.Fly();
        }

        public void MakeAnimalSwim(ISwimmer swimmer)
        {
            swimmer.Swim();
        }
    }

    // ========== USAGE EXAMPLE ==========
    public class LSPDemoSOLD
    {
        public static void Main()
        {
            Console.WriteLine("===== SOLD: Liskov Substitution Principle (L) =====\n");

            // ========== PAYMENT EXAMPLE ==========
            Console.WriteLine("--- Payment Processing Example ---\n");
            var paymentService = new PaymentService();

            // Refundable payments
            IRefundablePayment creditCard = new CreditCardProcessor();
            IRefundablePayment bankTransfer = new BankTransferProcessor();

            // Non-refundable payments
            IPaymentProcessor crypto = new CryptoCurrencyProcessor();
            IPaymentProcessor giftCard = new GiftCardProcessor();

            Console.WriteLine("Processing Refundable Payments:");
            paymentService.ProcessPaymentWithRefund(creditCard, 100);
            Console.WriteLine();
            paymentService.ProcessPaymentWithRefund(bankTransfer, 200);

            Console.WriteLine("\nProcessing All Payments (Safe Handling):");
            paymentService.ProcessPaymentSafely(creditCard, 150);
            Console.WriteLine();
            paymentService.ProcessPaymentSafely(crypto, 300);
            Console.WriteLine();
            paymentService.ProcessPaymentSafely(giftCard, 50);

            // ========== ANIMAL EXAMPLE ==========
            Console.WriteLine("\n--- Animal Substitution Example ---\n");
            var handler = new AnimalHandler();

            List<Animal> animals = new List<Animal>
            {
                new Dog(),
                new Eagle(),
                new Duck(),
                new Penguin()
            };

            Console.WriteLine("All animals can move and eat:");
            foreach (var animal in animals)
            {
                handler.MakeAnimalMove(animal);
                Console.WriteLine();
            }

            Console.WriteLine("Only birds can fly:");
            var flyers = new List<IFlyer> { new Eagle(), new Duck() };
            foreach (var flyer in flyers)
            {
                handler.MakeAnimalFly(flyer);
            }

            Console.WriteLine("\nOnly swimmers can swim:");
            var swimmers = new List<ISwimmer> { new Duck(), new Penguin() };
            foreach (var swimmer in swimmers)
            {
                handler.MakeAnimalSwim(swimmer);
            }

            Console.WriteLine("\n✓ Subtypes can be substituted without breaking contracts");
            Console.WriteLine("✓ Crypto can't be used where refunds are required");
            Console.WriteLine("✓ Each animal type implements only the behaviors it supports");
            Console.WriteLine("✓ No forced NotImplementedException errors");
        }
    }
}
