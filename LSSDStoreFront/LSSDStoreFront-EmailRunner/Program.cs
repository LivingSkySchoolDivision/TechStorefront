using System;
using System.Collections.Generic;
using LSSD.StoreFront.DB;
using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.EmailRunner;
using LSSD.StoreFront.Lib;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using LSSD.StoreFront.Lib.Extensions;
using System.Linq;
using LSSD.StoreFront.EmailRunner.Helpers;
using LSSD.StoreFront.EmailRunner.Models;
using System.Threading.Tasks;

namespace LSSDStoreFront_EmailRunner
{
    class Program
    {
        private const int sleepTimeMinutes = 15;

        private static void ConsoleWrite(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm K") + ": " + message);
        }

        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables()                
                .Build();

            string keyvault_endpoint = configuration["KEYVAULT_ENDPOINT"];
            if (!string.IsNullOrEmpty(keyvault_endpoint))
            {
                ConsoleWrite("Loading configuration from Azure Key Vault: " + keyvault_endpoint);
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                                new KeyVaultClient.AuthenticationCallback(
                                    azureServiceTokenProvider.KeyVaultTokenCallback));

                configuration = new ConfigurationBuilder()
                    .AddConfiguration(configuration)
                    .AddAzureKeyVault(keyvault_endpoint, keyVaultClient, new DefaultKeyVaultSecretManager())
                    .Build();
            }           

            IConfigurationSection smtpConfig = configuration.GetSection("SMTP");
            DatabaseContext dbContext = new DatabaseContext(configuration.GetConnectionString(RunnerSettings.ConnectionStringName));


            // Program loop start
            while (true)
            {
                try
                {

                    OrderRepository orderRepo = new OrderRepository(dbContext);
                    OrderNotificationLogRepository orderNotificationRepo = new OrderNotificationLogRepository(dbContext);

                    // Find order ids requiring customer emails
                    List<string> OrdersNeedingCustomerNotifications = orderNotificationRepo.GetOrdersNeedingCustomerNotifications();

                    // Find order ids requiring order desk emails
                    List<string> OrdersNeedingOrderDeskNotifications = orderNotificationRepo.GetOrdersNeedingOrderDeskNotifications();

                    // Load the order objects so we can use them later
                    List<string> RequiredOrders = new List<string>();
                    RequiredOrders.AddRangeUnique(OrdersNeedingCustomerNotifications);
                    RequiredOrders.AddRangeUnique(OrdersNeedingOrderDeskNotifications);
                    Dictionary<string, Order> _orderCache = orderRepo.Get(RequiredOrders).ToDictionary(x => x.OrderThumbprint);

                    EmailHelper email = new EmailHelper(smtpConfig["hostname"], smtpConfig["port"].ToInt(), smtpConfig["username"], smtpConfig["password"], smtpConfig["fromaddress"], smtpConfig["replytoaddress"]);

                    string helpDeskEmailAddress = smtpConfig["HelpDeskEmail"];

                    ConsoleWrite("Found " + RequiredOrders.Count + " orders requiring email notifications");

                    // Enqueue order desk emails
                    foreach (string t in OrdersNeedingOrderDeskNotifications)
                    {
                        if (_orderCache.ContainsKey(t))
                        {
                            Order thisOrder = _orderCache[t];
                            // Only send emails to orders with email addresses
                            if (helpDeskEmailAddress.Length > 0)
                            {
                                email.NewMessage(helpDeskEmailAddress, CannedEmailMessage.OrderDeskNotification(thisOrder), thisOrder);
                                ConsoleWrite("Enqueueing Order Desk email for order: " + thisOrder.OrderThumbprint);
                            }
                        }
                    }

                    // Enqueue customer emails
                    foreach (string t in OrdersNeedingCustomerNotifications)
                    {
                        if (_orderCache.ContainsKey(t))
                        {
                            Order thisOrder = _orderCache[t];
                            // Only send emails to orders with email addresses
                            if (thisOrder.CustomerEmailAddress.Length > 0)
                            {
                                email.NewMessage(thisOrder.CustomerEmailAddress, CannedEmailMessage.CustomerOrderThanks(thisOrder), thisOrder);
                                ConsoleWrite("Enqueueing Customer email for order: " + thisOrder.OrderThumbprint);
                            }
                        }
                    }

                    // Send all emails
                    ConsoleWrite("Sending " + (OrdersNeedingCustomerNotifications.Count + OrdersNeedingOrderDeskNotifications.Count) + "  emails...");
                    email.FlushQueue(dbContext);
                    ConsoleWrite("Done!");

                    ConsoleWrite("Sleeping for " + sleepTimeMinutes + " minutes...");
                }
                catch { }

                // Sleep for 15 minutes
                Task.Delay(sleepTimeMinutes * 60 * 1000).Wait();
            }
        }
    }
}
