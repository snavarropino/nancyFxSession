using Nancy;
using Nancy.Testing;
using NancyApi.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NancyApi.Test
{
    public class CatalogueModuleTest
    {
        public CatalogueModuleTest()
        {
        }

        [Fact]
        public async void TestingOneModule_when_getProducts_then_receiveProducts()
        {
            // Given          
            IProductsCatalogue pc = new ProductsCatalogue();
            var browser = new Browser(with => with.Module(new CatalogueModule(pc)));
           
            // When
            var result = await browser.Get("/Products", with => {
                with.HttpRequest();
                with.Accept(new Nancy.Responses.Negotiation.MediaRange("application/xml"));
            });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void when_getProducts_then_receiveProducts()
        {
            // Given
            var browser = new Browser(new DefaultNancyBootstrapper());

            // When
            var result = await browser.Get("/Products", with => {
                with.HttpRequest();
                with.Accept(new Nancy.Responses.Negotiation.MediaRange("application/json"));
            });

            var ct = result.ContentType;
            var bodyStream = result.Body.AsStream();
            var strBody = result.Body.AsString();

            var sr = new StreamReader(bodyStream);
            var deserializedObj = JsonConvert.DeserializeObject<List<Product>>(sr.ReadToEnd());

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(deserializedObj.Any(a=> a.Name== "Mahou"));          
        }
    }
}
