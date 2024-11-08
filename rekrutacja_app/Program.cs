using System;
using System.Collections.Generic;
using System.Linq;

namespace rekrutacja_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var products = new List<Product>
        {
            new Product { Name = "Laptop", Price = 2500 },
            new Product { Name = "Klawiatura", Price = 120 },
            new Product { Name = "Mysz", Price = 90 },
            new Product { Name = "Monitor", Price = 1000 },
            new Product { Name = "Kaczka debuggująca", Price = 66 }
        };

            var order = new Order();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("1. Dodaj produkt");
                Console.WriteLine("2. Usuń produkt");
                Console.WriteLine("3. Wyświetl wartość zamówienia");
                Console.WriteLine("4. Wyjdz");
               
                int option;
                if (int.TryParse(Console.ReadLine(), out option))
                {
                    switch (option)
                    {
                        case 1:
                            Console.WriteLine("Wybierz produkt do dodania:");
                            for (int i = 0; i < products.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {products[i].Name} - {products[i].Price} PLN");
                            }
                            if (int.TryParse(Console.ReadLine(), out int productIndex) && productIndex >= 1 && productIndex <= products.Count)
                            {
                                Console.Write("Podaj ilość: "); 
                                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                                {
                                    order.AddProduct(products[productIndex - 1], quantity);
                                }
                                else
                                {
                                    Console.WriteLine("Niepoprawna ilość.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Niepoprawny wybór produktu.");
                            }
                            break;
                        case 2:
                            Console.Write("Podaj nazwę produktu do usunięcia: ");
                            var productName = Console.ReadLine();
                            order.RemoveProduct(productName);
                            break;
                        case 3:
                            order.DisplayOrder();
                            break;
                        case 4:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Niepoprawna opcja.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("ERROR-wybierz ponownie");
                }
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        private List<OrderItem> orderItems = new List<OrderItem>();

        public void AddProduct(Product product, int quantity)
        {
            var orderItem = orderItems.FirstOrDefault(oi => oi.Product.Name == product.Name);
            if (orderItem != null)
            {
                orderItem.Quantity += quantity;
            }
            else
            {
                orderItems.Add(new OrderItem { Product = product, Quantity = quantity });
            }
        }

        public void RemoveProduct(string productName)
        {
            var orderItem = orderItems.FirstOrDefault(oi => oi.Product.Name == productName);
            if (orderItem != null)
            {
                if (orderItem.Quantity > 1)
                {
                    orderItem.Quantity -= 1;
                }
                else
                {
                    orderItems.Remove(orderItem);
                }
            }
        }

        public decimal Total()
        {
            decimal totalValue = 0;
            foreach (var item in orderItems)
            {
                totalValue += item.Product.Price * item.Quantity;
            }

            // Apply discounts
            if (orderItems.Count >= 2)
            {
                var cheapestItem = orderItems.OrderBy(oi => oi.Product.Price).FirstOrDefault();
                if (cheapestItem != null)
                {
                    totalValue -= cheapestItem.Product.Price * 0.10m;
                }
            }

            if (orderItems.Count >= 3)
            {
                var thirdCheapestItem = orderItems.OrderBy(oi => oi.Product.Price).Skip(2).FirstOrDefault();
                if (thirdCheapestItem != null)
                {
                    totalValue -= thirdCheapestItem.Product.Price * 0.20m;
                }
            }

            if (totalValue > 5000)
            {
                totalValue *= 0.95m;
            }

            return totalValue;
        }

        public void DisplayOrder()
        {
            Console.WriteLine("Aktualne zamówienie:");
            foreach (var item in orderItems)
            {
                Console.WriteLine($"Produkt: {item.Product.Name}, Wartość: {item.Quantity}, Cena: {item.Product.Price} PLN");
            }
            Console.WriteLine($"Razem: {Total()} PLN");
        }
    }
}
