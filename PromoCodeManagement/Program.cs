using Microsoft.Extensions.DependencyInjection;
using PromoCodeManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.Extensions.Configuration;
using System.Linq;
using QRCoder;
using System.Text;
using System.Drawing;
using System.IO;
using PromoCodeManagement.Entity;
using PromoCodeManagement.Models;

namespace PromoCodeManagement
{
    class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddScoped<IPurchaseHistoryRespository, PurchaseHistoryRespository>()
                .AddSingleton<IConfiguration>(configuration)
                .BuildServiceProvider();

            GeneratePromoAndQRCode(serviceProvider);
        }

        public static void GeneratePromoAndQRCode(IServiceProvider serviceProvider)
        {
            try
            {
                IPurchaseHistoryRespository purchaseHistoryRespository = serviceProvider.GetRequiredService<IPurchaseHistoryRespository>();

                List<Order> orders = purchaseHistoryRespository.GetOrders();

                foreach (var order in orders)
                {
                    List<PurchaseHistory> purchaseHistories = new List<PurchaseHistory>();

                    for (int i = 0; i < order.quantity; i++)
                    {
                        string promoCode = string.Empty;
                        PurchaseHistory purchaseHistory = new PurchaseHistory();

                    GenerateLabel:
                        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        var random = new Random();

                        promoCode = new string(
                            Enumerable.Repeat(chars, 11)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray());

                        if (purchaseHistoryRespository.CheckPromoCode(promoCode) > 0)
                        {
                            goto GenerateLabel;
                        }
                        else
                        {
                            purchaseHistory.promo_code = promoCode;

                            decimal amountPerQty = order.total_amount / order.quantity;
                            string qrCodeStr = "PhoneNo : " + order.phone_no + Environment.NewLine + "PromoCode : " + promoCode + Environment.NewLine + "ProductId : "
                                + order.productid + Environment.NewLine + "Amount : " + amountPerQty;
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeStr, QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);

                            using (Bitmap bitMap = qrCode.GetGraphic(20))
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                                    var img = new byte[ms.ToArray().Length];
                                    img = ms.ToArray();
                                    purchaseHistory.qr_code = img;
                                }
                            }

                            purchaseHistory.orderid = order.id;
                            purchaseHistory.id = Guid.NewGuid();
                            purchaseHistory.is_used = false;

                            purchaseHistories.Add(purchaseHistory);
                        }
                    }

                    int saveCount = purchaseHistoryRespository.AddPurchaseHistory(purchaseHistories);
                    if (saveCount > 0)
                    {
                        order.generated_status = true;
                        int updateCount = purchaseHistoryRespository.UpdateGenerateStatusToOrder(order);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception Message : " + ex.Message);
            }            
        }
    }
}
