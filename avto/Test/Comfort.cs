using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using Xunit;



namespace avto.Test
{
  
    namespace AvtoShow.Tests
    {
        public class ComfortPageTests : IDisposable
        {
            private readonly IWebDriver _driver;
            private readonly string _baseUrl;

            public ComfortPageTests()
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AcceptInsecureCertificates = true;

                _driver = new ChromeDriver(chromeOptions);
                _baseUrl = "https://localhost:7274/Comfort"; // ваш URL страницы "Комфорт"
            }

            [Fact]
            public void PageTitle_ShouldBeCorrect()
            {
                // Открытие страницы "Комфорт"
                _driver.Navigate().GoToUrl(_baseUrl);

                // Проверка заголовка страницы
                var pageTitle = _driver.Title;
                Assert.Equal("Комфорт - AvtoShow", pageTitle);
            }

            [Fact]
            public void WelcomeMessage_ShouldBePresent()
            {
                // Открытие страницы "Комфорт"
                _driver.Navigate().GoToUrl(_baseUrl);

                // Проверка наличия приветственного сообщения
                var welcomeMessage = _driver.FindElement(By.CssSelector(".containerModel h1")).Text;
                Assert.Equal("Добро пожаловать в сегмент Комфорт", welcomeMessage);
            }

            [Fact]
            public void CheckPopularCarModels()
            {
                // Открытие страницы "Комфорт"
                _driver.Navigate().GoToUrl(_baseUrl);

                // Проверка популярных моделей автомобилей
                var carModels = _driver.FindElements(By.CssSelector(".col-md-4 .card-title"));
                Assert.Collection(carModels,
                    item => Assert.Equal("Toyota camry", item.Text),
                    item => Assert.Equal("Renault Logan II", item.Text),
                    item => Assert.Equal("BMW 5 Series", item.Text));
            }

            [Fact]
            public void CheckCarTableHeaders()
            {
                // Открытие страницы "Комфорт"
                _driver.Navigate().GoToUrl(_baseUrl);

                // Проверка заголовков таблицы с данными автомобилей
                var tableHeaders = _driver.FindElements(By.CssSelector(".table th"));
                Assert.Collection(tableHeaders,
                    item => Assert.Equal("Модель", item.Text),
                    item => Assert.Equal("Год выпуска", item.Text),
                    item => Assert.Equal("VIN", item.Text),
                    item => Assert.Equal("Цена", item.Text),
                    item => Assert.Equal("Доступен", item.Text),
                    item => Assert.Equal("Характеристики", item.Text),
                    item => Assert.Equal("Категория", item.Text),
                    item => Assert.Equal("Изображения", item.Text));
            }

            public void Dispose()
            {
                _driver.Quit();
            }
        }
    }
}
