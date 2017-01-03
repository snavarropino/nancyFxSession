using Moq;
using Nancy;
using Nancy.Testing;
using NancyApi.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace NancyApi.Test
{
    public class CatalogueModuleTest
    {
        [Fact]
        public async void TestingOneModule_when_getProducts_then_receiveProducts_Case1()
        {
            // Given          
            IProductsCatalogue pc = new ProductsCatalogue();
            var browser = new Browser(with => with.Module(new CatalogueModule(pc)));
           
            // When
            var result = await browser.Get("/Products", with => {
                //with.HttpRequest();
                with.Accept(new Nancy.Responses.Negotiation.MediaRange("application/xml"));
            });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void TestingOneModule_when_getProducts_then_receiveProducts_Case2()
        {
            // Given
            var browser = new Browser(new DefaultNancyBootstrapper());

            // When
            var result = await browser.Get("/Products", with => {
                with.HttpRequest();
                with.Accept(new Nancy.Responses.Negotiation.MediaRange("application/json"));
            });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void TestingOneModule_when_getProducts_then_receiveProducts_Case3()
        {
            // Given
            var m = new Mock <IProductsCatalogue>();
            m.Setup<IList<Product>>(a => a.GetProducts(null)).Returns(new List<Product>()
            {
                new Product() { Id=1, Name="Estrella" ,Stock=1000 }
            });        

            var bootstrapper = new ConfigurableBootstrapper(with =>
                {
                    with.Module<CatalogueModule>();
                    with.Dependencies(m.Object);
                });

            var browser = new Browser(bootstrapper);

            // When
            var result = await browser.Get("/Products", with => {
                with.HttpRequest();
                with.Accept(new Nancy.Responses.Negotiation.MediaRange("application/json"));
            });

            var ct = result.ContentType;
            var bodyStream = result.Body.AsStream();
            var strBody = result.Body.AsString();
            var productList = result.Body.DeserializeJson <List<Product>>();

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(productList.Any(a=> a.Name== "Estrella"));          
        }

        [Fact]
        public async void TestingOneModule_when_postProducts_then_productAdded()
        {
            // Given           
            var browser = new Browser(new DefaultNancyBootstrapper());

            // When
            var result = await browser.Post("/Products", with => {
                with.HostName ( "localhost:1234");                
                with.FormValue("Id","1");
                with.FormValue("Name", "Mahou Clasica");
                with.FormValue("Stock", "1000000");
            });

            // Then
            Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
            var locationHeader = result.Headers["Location"];
            Assert.NotEmpty(locationHeader);

            result = await browser.Get(locationHeader, with => {
                with.Accept(new Nancy.Responses.Negotiation.MediaRange("application/json"));
            });

            var strBody = result.Body.AsString();
            var productList = result.Body.DeserializeJson<List<Product>>();

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(productList.Any(a => a.Name == "Mahou Clasica"));

        }
    }
}
