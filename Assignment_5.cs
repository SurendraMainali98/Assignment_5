using System;
using System.Collections.Generic;

namespace Post
{
    // Abstract class representing a generic mail
    public abstract class Mail
    {
        // Properties
        public double Weight { get; }
        public bool Express { get; }
        public string Destination { get; }

        // Constructor
        public Mail(double weight, bool express, string destination)
        {
            Weight = weight;
            Express = express;
            Destination = destination;
        }

        // Abstract method to calculate stamp amount
        public abstract double CalculateStampAmount();

        // Method to check if the mail is valid
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Destination);
        }

        // Method to check if the mail is express
        public bool GetIsExpress()
        {
            return Express;
        }

        // Method to get destination address
        public string GetDestinationAddress()
        {
            return Destination;
        }
    }

    // Class representing a letter
    public class Letter : Mail
    {
        // Additional properties for Letter
        public string Format { get; }

        // Constructor
        public Letter(double weight, bool express, string destination, string format)
            : base(weight, express, destination)
        {
            Format = format;
        }

        // Override method to calculate stamp amount for letter
        public override double CalculateStampAmount()
        {
            double baseFare = Format == "A4" ? 2.50 : 3.50;
            double amount = baseFare + 1.0 * Weight / 1000; // Convert grams to kilograms
            return Express ? amount * 2 : amount;
        }

        // Method to get letter format
        public string GetFormat()
        {
            return Format;
        }
    }

    // Class representing a parcel
    public class Parcel : Mail
    {
        // Additional properties for Parcel
        public double Volume { get; }

        // Constructor
        public Parcel(double weight, bool express, string destination, double volume)
            : base(weight, express, destination)
        {
            Volume = volume;
        }

        // Override method to calculate stamp amount for parcel
        public override double CalculateStampAmount()
        {
            double amount = 0.25 * Volume + Weight / 1000; // Convert grams to kilograms
            return Express ? amount * 2 : amount;
        }

        // Method to get parcel volume
        public double GetVolume()
        {
            return Volume;
        }
    }

    // Class representing an advertisement
    public class Advertisement : Mail
    {
        // Constructor
        public Advertisement(double weight, bool express, string destination)
            : base(weight, express, destination)
        {
        }

        // Override method to calculate stamp amount for advertisement
        public override double CalculateStampAmount()
        {
            double amount = 5.0 * Weight / 1000; // Convert grams to kilograms
            return Express ? amount * 2 : amount;
        }
    }

    // Class representing the mailbox
    public class Box
    {
        private List<Mail> mails;
        private int maxSize;

        // Constructor
        public Box(int maxSize)
        {
            this.maxSize = maxSize;
            this.mails = new List<Mail>();
        }

        // Method to add mail to the box
        public void AddMail(Mail mail)
        {
            if (mails.Count < maxSize && mail.IsValid())
            {
                mails.Add(mail);
            }
            else
            {
                Console.WriteLine("Mailbox is full or mail is invalid. Cannot add more mails.");
            }
        }

        // Method to calculate total stamp amount for all mails in the box
        public double Stamp()
        {
            double totalAmount = 0;
            foreach (var mail in mails)
            {
                if (mail.IsValid())
                {
                    totalAmount += mail.CalculateStampAmount();
                }
                else
                {
                    Console.WriteLine(mail.GetType().Name + " (Invalid courier)");
                }
            }
            return totalAmount;
        }

        // Method to display contents of the mailbox
        public void Display()
        {
            foreach (var mail in mails)
            {
                Console.WriteLine(typeof(Mail).Name);
                Console.WriteLine("Weight: " + mail.Weight + " grams");
                Console.WriteLine("Express: " + (mail.Express ? "yes" : "no"));
                Console.WriteLine("Destination: " + mail.Destination);
                if (mail.IsValid())
                {
                    Console.WriteLine("Price: " + mail.CalculateStampAmount() + " CHF");
                }
                else
                {
                    Console.WriteLine("Price: 0.0 CHF");
                }
                if (mail is Letter)
                {
                    Console.WriteLine("Format: " + ((Letter)mail).GetFormat());
                }
                else if (mail is Parcel)
                {
                    Console.WriteLine("Volume: " + ((Parcel)mail).GetVolume() / 1000 + " cubic meters"); // Convert liters to cubic meters
                }
                Console.WriteLine();
            }
        }

        // Method to count invalid mails in the box
        public int MailIsInvalid()
        {
            int count = 0;
            foreach (var mail in mails)
            {
                if (!mail.IsValid())
                {
                    count++;
                }
            }
            return count;
        }
    }

    class Post
    {
        public static void Main(string[] args)
        {
            // Creation of a mailbox 
            // The maximum size of a box is 30
            Box box = new Box(30);

            Letter letter1 = new Letter(200, true, "Chemin des Acacias 28, 1009 Pully", "A3");
            Letter letter2 = new Letter(800, false, "", "A4"); // invalid

            Advertisement adv1 = new Advertisement(1500, true, "Les Moilles  13A, 1913 Saillon");
            Advertisement adv2 = new Advertisement(3000, false, ""); // invalid

            Parcel parcel1 = new Parcel(5000, true, "Grand rue 18, 1950 Sion", 30);
            Parcel parcel2 = new Parcel(3000, true, "Chemin des fleurs 48, 2800 Delemont", 70); // invalid parcel

            box.AddMail(letter1);
            box.AddMail(letter2);
            box.AddMail(adv1);
            box.AddMail(adv2);
            box.AddMail(parcel1);
            box.AddMail(parcel2);

            Console.WriteLine("The total amount of postage is " + box.Stamp() + " CHF");
            box.Display();

            Console.WriteLine("The box contains " + box.MailIsInvalid() + " invalid mails");
        }
    }
}
