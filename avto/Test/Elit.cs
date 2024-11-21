using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace AvtoShow.Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using Xunit;

    namespace AvtoShow.Tests
    {
        public class ElitPageTests : IDisposable
        {
            private readonly IWebDriver _driver;
            private readonly string _baseUrl;

            public ElitPageTests()
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AcceptInsecureCertificates = true;

                _driver = new ChromeDriver(chromeOptions);
                _baseUrl = "https://localhost:7274/Elit"; // ваш URL страницы "Элит"
            }

            [Fact]
            public void PageTitle_ShouldBeCorrect()
            {
                // Открытие страницы "Элит"
                _driver.Navigate().GoToUrl(_baseUrl);

                // Проверка заголовка страницы
                var pageTitle = _driver.Title;
                Assert.Equal("Элит - AvtoShow", pageTitle);
            }

            [Fact]
            public void WelcomeMessage_ShouldBePresent()
            {
                // Открытие страницы "Элит"
                _driver.Navigate().GoToUrl(_baseUrl);

                // Проверка наличия приветственного сообщения
                var welcomeMessage = _driver.FindElement(By.CssSelector(".containerModel > h1")).Text;
                Assert.Equal("Добро пожаловать в сегмент Элит", welcomeMessage);
            }

            [Fact]
            public void CheckPopularCarModels()
            {
                // Открытие страницы "Элит"
                _driver.Navigate().GoToUrl(_baseUrl);

                // Проверка популярных моделей автомобилей
                var carModels = _driver.FindElements(By.CssSelector(".col-md-4 .card-title"));
                Assert.Collection(carModels,
                    item => Assert.Equal("BMW 8 series", item.Text.Trim()),
                    item => Assert.Equal("Mercedes-Benz S-Class", item.Text.Trim()),
                    item => Assert.Equal("Toyota Land Cruiser 300", item.Text.Trim()));
            }

            [Fact]
            public void CheckCarTableHeaders()
            {
                // Открытие страницы "Элит"
                _driver.Navigate().GoToUrl(_baseUrl);

                // Проверка заголовков таблицы с данными автомобилей
                var expectedHeaders = new[]
                {
                "Модель", "Год выпуска", "VIN", "Цена", "Доступен", "Характеристики", "Категория", "Изображения"
            };

                var tableHeaders = _driver.FindElements(By.CssSelector(".table th"));
                Assert.Collection(tableHeaders,
                    item => Assert.Equal(expectedHeaders[0], item.Text),
                    item => Assert.Equal(expectedHeaders[1], item.Text),
                    item => Assert.Equal(expectedHeaders[2], item.Text),
                    item => Assert.Equal(expectedHeaders[3], item.Text),
                    item => Assert.Equal(expectedHeaders[4], item.Text),
                    item => Assert.Equal(expectedHeaders[5], item.Text),
                    item => Assert.Equal(expectedHeaders[6], item.Text),
                    item => Assert.Equal(expectedHeaders[7], item.Text));
            }

            

            public void Dispose()
            {
                _driver.Quit();
            }
        }
    }
}