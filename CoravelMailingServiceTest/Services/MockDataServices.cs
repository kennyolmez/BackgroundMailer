using Bogus;
using CoravelMailingServiceTest.Models;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CoravelMailingServiceTest.Services
{
    public class MockDataServices 
    {
        public List<ProductModel> GenerateMockProducts()
        {
            var productFaker = new Faker<ProductModel>();


            productFaker.RuleFor(x => x.Name, x => x.Commerce.ProductName());
            productFaker.RuleFor(x => x.BrandName, x => x.Company.CompanyName());
            productFaker.RuleFor(x => x.UnitPrice, x => x.Finance.Amount(5, 1000));
            productFaker.RuleFor(x => x.Quantity, x => (int)x.Finance.Amount(5, 5));
            productFaker.RuleFor(x => x.ImgUrl, x => x.Image.PicsumUrl());

            var realProductList = new List<ProductModel>();

            foreach(var product in productFaker.Generate(10))
            {
                var text = JsonSerializer.Serialize(product);

                realProductList.Add(JsonConvert.DeserializeObject<ProductModel>(text)!);
            }

            return realProductList!;
        }
    }
}
